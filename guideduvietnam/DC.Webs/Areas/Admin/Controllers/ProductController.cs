using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DC.Common;
using DC.Common.Utility;
using DC.Models.Posts;
using DC.Services.Cms;
using DC.Services.Posts;
using DC.Webs.Common;
using DC.Webs.Models;

namespace DC.Webs.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        PostViewModel model = new PostViewModel();
        private readonly IPostService _postService;
        private readonly IPostImageService _postImageService;
        private readonly ICategoryService _categoryService;
        private readonly ITagsService _tagsService;


        //Ctor
        public ProductController(ICategoryService categoryService, IPostService postService, ITagsService tagsService,
            IPostImageService postImageService)
        {
            this._categoryService = categoryService;
            this._postService = postService;
            this._tagsService = tagsService;
            this._postImageService = postImageService;
        }

        #region #Index

        // GET: Post

        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Index()
        {
            model.TabName = "Post";
            SetupBaseModel(model);

            //add select category
            model.CategoryOptions = GetCategoryOptions(CategoryConst.CATEGORYPRODUCT, true);
            //add select status
            model.StatusOptions = GetStatusOptions(true);
            return View(model);
        }


        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetList(string query, int? cateId, string status, string from, string to, string sortBy, string orderBy,
            int pageIndex, int pageSize)
        {       
            int categoryId = 0;
            DateTime? fromDate = DateTimeTools.CFToDateTime(from);
            DateTime? toDate = DateTimeTools.CFToDateTime(to);
            if (cateId.HasValue)
                categoryId = (int)cateId;
            // Set model
            var postItems = this._postService.GetFilterAdmin(query.Trim(), categoryId, status.Trim(), fromDate, toDate, PostTypeConst.TOUR, sortBy, orderBy, pageIndex, pageSize);
            if (postItems != null && postItems.Count > 0)
            {
                model.TotalCount = postItems.TotalCount;
                model.TotalPages = postItems.TotalPages;
                model.PostItems = postItems.Select(c => c.ToModel()).ToList();

            }
            else
            {
                model.TotalPages = 0;
                model.TotalCount = 0;
                model.PostItems = new List<PostModel>();
            }
            return Json(model);
        }
        #endregion

        #region #Add

        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Add()
        {
            model.TabName = "Post";
            SetupBaseModel(model);
            //add select category
            model.CategoryOptions = GetCategoryOptions(CategoryConst.CATEGORYPRODUCT, false);
            if (model.CategoryOptions != null && model.CategoryOptions.Count > 0)
                model.CategoryOptions.Insert(0, new SelectItemModel("--Uncategorized--", "0"));
            else
                model.CategoryOptions.Add(new SelectItemModel("--Uncategorized--", "0"));
            //add select status
            model.StatusOptions = GetStatusOptions(false);
            model.TagsOptions = GetTagsOptions(TagConst.TAGTOUR);
            return View(model);
        }
        
        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(FormCollection collection)
        {
            try
            {
                var userCurrent = GetUserByName(User.Identity.Name);
                
                int postId = 0;
                // 480 x 288
                //string picture = AddFiles(PostImageUrl, ref maxsize);
                string picture = AddFilesResize(PostImageUrl, 480, 270);

                int cateId = collection["CategoryId"] != null ? int.Parse(collection["CategoryId"].ToString()) : 0;
                string strTags = collection["TagId"] ?? "";
                var postmodel = new PostModel();
                postmodel.KeySlug = collection["Url"] != null ? SlugExtentions.ToSlug(collection["Url"]) : string.Empty;
                postmodel.Name = collection["Name"] != null ? collection["Name"].ToString() : string.Empty;
                postmodel.CateId = cateId;
                postmodel.CateName = cateId > 0 ? this._categoryService.Find(cateId).Name : "";
                postmodel.Title = collection["Title"] != null ? collection["Title"].ToString() : string.Empty;
                postmodel.Url = collection["Url"] != null ? collection["Url"].ToString() : string.Empty;
                postmodel.Picture = picture;
                postmodel.Description = collection["Description"] != null ? collection["Description"].ToString() : string.Empty;
                postmodel.MetaKeyword = collection["MetaKeyword"] != null ? collection["MetaKeyword"].ToString() : string.Empty;
                postmodel.MetaDescription = collection["MetaDescription"] != null ? collection["MetaDescription"].ToString() : string.Empty;
                postmodel.OverView = collection["OverView"] != null ? collection["OverView"].ToString() : "";
                postmodel.Content = collection["Content"] != null ? collection["Content"].ToString() : "";
                postmodel.Status = collection["Status"] != null ? collection["Status"].ToString() : StatusConst.DRAFTNAME;
                postmodel.ViewCount = 1;
                postmodel.TagsId = strTags;
                postmodel.PostType = PostTypeConst.TOUR;
                postmodel.CreateByUser = userCurrent.UserName;
                postmodel.IsComment = collection["IsComment"] != null && collection["IsComment"] == "1";
                postmodel.Target = collection["Target"] != null ? collection["Target"].ToString() : string.Empty;
                this._postService.Insert(postmodel.ToEntity(), ref postId);
                //Tags                
                if (!string.IsNullOrEmpty(strTags.Trim()))
                {
                    string[] taglists = strTags.Split(',');
                    string tagSlug = "";
                    int tagId = 0;
                    for (int i = 0; i < taglists.Count(); i++)
                    {
                        tagSlug = SlugExtentions.ToSlug(taglists[i]);
                        var tagObj = this._tagsService.FirstOrDefault(m => m.KeySlug == tagSlug && m.TagType == TagConst.TAGTOUR);
                        if (tagObj == null)
                        {
                            TagModel tagModel = new TagModel();
                            tagModel.KeySlug = tagSlug;
                            tagModel.Name = taglists[i];
                            tagModel.Title = taglists[i];
                            tagModel.Url = taglists[i];
                            tagModel.MetaKeyword = taglists[i];
                            tagModel.MetaDescription = taglists[i];
                            tagModel.TagType = TagConst.TAGTOUR;
                            this._tagsService.Insert(tagModel.ToEntity(), ref tagId);
                        }
                    }
                }

                #region #AddProductImage

                List<PostImageModel> productImageItems = new List<PostImageModel>();
                PostImageModel productImage = null;
                if (collection["LinkImages"] != null && !string.IsNullOrEmpty(collection["LinkImages"].ToString()))
                {
                    List<string> linkImages = collection["LinkImages"].Split(',').ToList();
                    foreach (var linkId in linkImages)
                    {
                        if (!string.IsNullOrEmpty(linkId))
                        {
                            productImage = new PostImageModel();
                            productImage.PostId = postId;
                            productImage.Images = linkId;
                            productImageItems.Add(productImage);
                        }
                    }
                }
                this._postImageService.Add(productImageItems.Select(c => c.ToEntity()).AsQueryable());
                this._postImageService.Save();
                #endregion
                TempData["MessageSuccess"] = "Thêm mới tour thành công!";
                // Lưu hành động
                string comment = string.Format("Tạo mới tour : ID({0}) - {1}", postId, postmodel.Name);
                AddActivityLog(LogTypeConst.INSERT, comment);
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["MessageError"] = "Error!";
                return RedirectToAction("Add");
            }
        }
        #endregion

        #region #Edit

        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Edit(int id)
        {
            SetupBaseModel(model);
            var postObj = this._postService.Find(id);
            if (postObj == null)
                return RedirectToAction("Index");

            //add select category
            model.CategoryOptions = GetCategoryOptions(CategoryConst.CATEGORYPRODUCT, false);
            if (model.CategoryOptions != null && model.CategoryOptions.Count > 0)
                model.CategoryOptions.Insert(0, new SelectItemModel("--Uncategorized--", "0"));
            else
                model.CategoryOptions.Add(new SelectItemModel("--Uncategorized--", "0"));
            //add select status
            model.StatusOptions = GetStatusOptions(false);
            model.TagsOptions = GetTagsOptions(TagConst.TAGTOUR);
            // PostInfo

            model.PostInfo = postObj.ToModel();
            //product images
            var postImages = this._postImageService.FindAllBy(m=>m.PostId==id).ToList();
            List<JsonItem> postImageItems = postImages.Select(item => new JsonItem()
            {
                Url = item.Images
            }).ToList();
            model.PostImageItems = postImageItems;

            //Tag selected
            
            if (!string.IsNullOrEmpty(model.PostInfo.TagsId))
            {
                List<string> tagArray = model.PostInfo.TagsId.Split(',').ToList();
                foreach (var item in model.TagsOptions)
                {
                    if (tagArray.Contains(item.Text))
                        item.Selected = true;
                }
            }
            return View(model);
        }
        
        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
               
                // 480 x 288
                //string picture = AddFiles(PostImageUrl, ref maxsize);
                string picture = AddFilesResize(PostImageUrl, 480, 270);
                var postObj = this._postService.Find(id);
                if (postObj != null)
                {
                    string strTags = collection["TagId"] ?? "";
                    int cateId = collection["CategoryId"] != null ? int.Parse(collection["CategoryId"].ToString()) : 0;
                    postObj.KeySlug = collection["Url"] != null ? SlugExtentions.ToSlug(collection["Url"]) : postObj.KeySlug;
                    postObj.Name = collection["Name"]?.ToString() ?? postObj.Name;
                    postObj.CateId = cateId;
                    postObj.CateName = cateId > 0 ? this._categoryService.Find(cateId).Name : "";
                    postObj.Title = collection["Title"]?.ToString() ?? postObj.Title;
                    postObj.Url = collection["Url"]?.ToString() ?? postObj.Url;
                    if (!String.IsNullOrEmpty(picture))
                        postObj.Picture = picture;
                    postObj.Description = collection["Description"]?.ToString() ?? postObj.Description;
                    postObj.MetaKeyword = collection["MetaKeyword"]?.ToString() ?? postObj.MetaKeyword;
                    postObj.MetaDescription = collection["MetaDescription"]?.ToString() ?? postObj.MetaDescription;
                    postObj.Content = collection["Content"]?.ToString() ?? postObj.Content;
                    postObj.OverView = collection["OverView"]?.ToString() ?? postObj.OverView;
                    postObj.Status = collection["Status"]?.ToString() ?? postObj.Status;                    
                    postObj.TagsId = strTags;
                    postObj.IsComment = collection["IsComment"] != null && collection["IsComment"] == "1";
                    postObj.Target = collection["Target"] != null ? collection["Target"].ToString() : string.Empty;
                    this._postService.Update(postObj);
                    #region #AddTags

                    //tags Post
                    // xóa tất cả các tagpost hiện tại
                    if (!string.IsNullOrEmpty(strTags.Trim()))
                    {
                        string[] taglists = strTags.Split(',');
                        string tagSlug = "";
                        int tagId = 0;
                        for (int i = 0; i < taglists.Count(); i++)
                        {
                            tagSlug = SlugExtentions.ToSlug(taglists[i]);
                            var tagObj = this._tagsService.FirstOrDefault(m => m.KeySlug == tagSlug && m.TagType == TagConst.TAGTOUR);
                            if (tagObj == null)
                            {
                                TagModel tagModel = new TagModel();
                                tagModel.KeySlug = tagSlug;
                                tagModel.Name = taglists[i];
                                tagModel.Title = taglists[i];
                                tagModel.Url = taglists[i];
                                tagModel.MetaKeyword = taglists[i];
                                tagModel.MetaDescription = taglists[i];
                                tagModel.TagType = TagConst.TAGTOUR;
                                this._tagsService.Insert(tagModel.ToEntity(), ref tagId);
                            }
                        }
                    }
                    #endregion

                    #region #AddPostImage
                    //xóa tất cả ảnh của sản phẩm
                    var postimgs = this._postImageService.FindAllBy(m=>m.PostId==id);
                    this._postImageService.Delete(postimgs);
                    this._postImageService.Save();
                    // add lại ảnh product
                    List<PostImageModel> productImageItems = new List<PostImageModel>();
                    PostImageModel productImage = null;
                    if (collection["LinkImages"] != null && !string.IsNullOrEmpty(collection["LinkImages"].ToString()))
                    {
                        List<string> linkImages = collection["LinkImages"].Split(',').ToList();
                        foreach (var linkId in linkImages)
                        {
                            if (!string.IsNullOrEmpty(linkId))
                            {
                                productImage = new PostImageModel();
                                productImage.PostId = id;
                                productImage.Images = linkId;
                                productImageItems.Add(productImage);
                            }
                        }
                    }
                    this._postImageService.Add(productImageItems.Select(c => c.ToEntity()).AsQueryable());
                    this._postImageService.Save();

                    #endregion

                    TempData["MessageSuccess"] = "Cập nhật tour thành công!";
                    string comment = string.Format("Cập nhật tour : ID({0}) - {1}", postObj.Id, postObj.Name);
                    AddActivityLog(LogTypeConst.UPDATE, comment);
                }
                else
                {
                    TempData["MessageError"] = "Không tồn tại tour trên";
                }
            }
            catch
            {
                TempData["MessageError"] = "Error!";
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region #Delete
        /// <summary>
        /// XÓa tin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var postObj = this._postService.Find(id);

                this._postService.Delete(id);

                // Lưu hành động                
                string comment = string.Format("Xóa Tour : ID({0}) - {1}", postObj.Id, postObj.Name);
                AddActivityLog(LogTypeConst.DELETE, comment);
            }
            catch
            {
            }
            return Json(string.Empty);
        }
        #endregion

        #region #Refresh
        /// <summary>
        /// Làm mới tin trang admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Refresh(int id)
        {
            try
            {
                var postObj = this._postService.Find(id);
                if (postObj != null && postObj.PostType==PostTypeConst.TOUR)
                {
                    this._postService.Refresh(id);
                    TempData["MessageSuccess"] = "Làm mới tour thành công!";
                }
                else
                {
                    TempData["MessageError"] = "Không tồn tại tour trên";

                }
            }
            catch (Exception ex)
            {
                TempData["MessageError"] = ex.ToString();
            }
            return RedirectToAction("Index");
        }


        #endregion

        #region #validate
        /// <summary>
        /// Check xem keyslug đã tồn tại hay chưa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult ExistUrl(int id, string url)
        {
            // Set params
            string slug = SlugExtentions.ToSlug(url);
            var postObj = this._postService.FirstOrDefault(m => m.KeySlug == slug && m.PostType == PostTypeConst.TOUR);
            if (postObj != null && postObj.Id != id)
                return Json("existed");

            return Json(string.Empty);
        }
        #endregion
    }
}