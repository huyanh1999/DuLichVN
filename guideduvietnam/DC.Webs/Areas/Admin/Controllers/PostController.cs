using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DC.Common;
using DC.Common.Utility;
using DC.Entities.Domain;
using DC.Webs.Common;
using DC.Webs.Models;
using DC.Services.Cms;
using DC.Services.Posts;
using DC.Models.Posts;

namespace DC.Webs.Areas.Admin.Controllers
{
    public class PostController : BaseController
    {
        PostViewModel model = new PostViewModel();
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;
        private readonly ITagsService _tagsService;


        //Ctor
        public PostController(ICategoryService categoryService, IPostService postService, ITagsService tagsService)
        {
            this._categoryService = categoryService;
            this._postService = postService;
            this._tagsService = tagsService;
        }

        #region #Index

        // GET: Post

        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Index()
        {
            model.TabName = "Post";
            SetupBaseModel(model);

            //add select category
            model.CategoryOptions = GetCategoryOptions(CategoryConst.CATEGORYPOST,true);            
            //add select status
            model.StatusOptions = GetStatusOptions(true);
            //add select PostTypeOptions
            model.PostTypeOptions = GetPostTypeOptions(true);
            return View(model);
        }


        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetList(string query, int? cateId, string status, string from, string to, string type, string sortBy, string orderBy,
            int pageIndex, int pageSize)
        {            
            int categoryId = 0;
            DateTime? fromDate = DateTimeTools.CFToDateTime(from);
            DateTime? toDate = DateTimeTools.CFToDateTime(to);
            if (cateId.HasValue)
                categoryId = (int)cateId;
            List<string> postTypes = new List<string>();
            if (!string.IsNullOrEmpty(type.Trim()))
            {
                postTypes.Add(type.ToUpper());
            }
            else
            {
                postTypes.Add(PostTypeConst.POST);
                postTypes.Add(PostTypeConst.BLOG);
            }
            // Set model
            var postItems = this._postService.GetFilterAdmin(query.Trim(), categoryId, status.Trim(), fromDate, toDate, postTypes, sortBy, orderBy, pageIndex, pageSize);
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
            model.CategoryOptions = GetCategoryOptions(CategoryConst.CATEGORYPOST,false);
            if (model.CategoryOptions != null && model.CategoryOptions.Count > 0)
                model.CategoryOptions.Insert(0, new SelectItemModel("--Uncategorized--", "0"));
            else
                model.CategoryOptions.Add(new SelectItemModel("--Uncategorized--", "0"));                       
            //add select status
            model.StatusOptions = GetStatusOptions(false);
            //add select PostTypeOptions
            model.PostTypeOptions = GetPostTypeOptions(false);
            model.TagsOptions = GetTagsOptions(TagConst.TAGPOST);
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
                postmodel.Content = collection["Content"] != null ? collection["Content"].ToString() : "";
                postmodel.Status = collection["Status"] != null ? collection["Status"].ToString() : StatusConst.DRAFTNAME;
                postmodel.ViewCount = 1;
                postmodel.TagsId = strTags;
                postmodel.PostType = collection["PostType"] != null ? collection["PostType"].ToString() : PostTypeConst.POST;
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
                        var tagObj = this._tagsService.FirstOrDefault(m=>m.KeySlug == tagSlug && m.TagType== TagConst.TAGPOST);
                        if (tagObj == null)
                        {
                            TagModel tagModel = new TagModel();
                            tagModel.KeySlug = tagSlug;
                            tagModel.Name = taglists[i];
                            tagModel.Title = taglists[i];
                            tagModel.Url = taglists[i];
                            tagModel.MetaKeyword = taglists[i];
                            tagModel.MetaDescription = taglists[i];
                            tagModel.TagType = TagConst.TAGPOST;
                            this._tagsService.Insert(tagModel.ToEntity(), ref tagId);                            
                        }                        
                    }
                }                
                TempData["MessageSuccess"] = "Thêm mới bài viết thành công!";
                // Lưu hành động
                string comment = string.Format("Tạo mới bài viết : ID({0}) - {1}", postId, postmodel.Name);
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
            model.CategoryOptions = GetCategoryOptions(CategoryConst.CATEGORYPOST,false);
            if (model.CategoryOptions != null && model.CategoryOptions.Count > 0)
                model.CategoryOptions.Insert(0, new SelectItemModel("--Uncategorized--", "0"));
            else
                model.CategoryOptions.Add(new SelectItemModel("--Uncategorized--", "0"));
            //add select status
            model.StatusOptions = GetStatusOptions(false);
            //add select PostTypeOptions
            model.PostTypeOptions = GetPostTypeOptions(false);
            model.TagsOptions = GetTagsOptions(TagConst.TAGPOST);
            // PostInfo

            model.PostInfo = postObj.ToModel();
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
                var userCurrent = GetUserByName(User.Identity.Name);
                
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
                    postObj.Status = collection["Status"]?.ToString() ?? postObj.Status;                    
                    postObj.PostType = collection["PostType"]?.ToString() ?? postObj.PostType;
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
                            var tagObj = this._tagsService.FirstOrDefault(m => m.KeySlug == tagSlug && m.TagType == TagConst.TAGPOST);
                            if (tagObj == null)
                            {
                                TagModel tagModel = new TagModel();
                                tagModel.KeySlug = tagSlug;
                                tagModel.Name = taglists[i];
                                tagModel.Title = taglists[i];
                                tagModel.Url = taglists[i];
                                tagModel.MetaKeyword = taglists[i];
                                tagModel.MetaDescription = taglists[i];
                                tagModel.TagType = TagConst.TAGPOST;
                                this._tagsService.Insert(tagModel.ToEntity(), ref tagId);
                            }
                        }
                    }
                    #endregion

                    TempData["MessageSuccess"] = "Cập nhật bài viết thành công!";
                    string comment = string.Format("Cập nhật bài viết : ID({0}) - {1}", postObj.Id, postObj.Name);
                    AddActivityLog(LogTypeConst.UPDATE, comment);                    
                }
                else
                {
                    TempData["MessageError"] = "Không tồn tại bài viết trên";
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

                this._postService.Delete(postObj);
                this._postService.Save();
                // Lưu hành động                
                string comment = string.Format("Xóa bài viết : ID({0}) - {1}", postObj.Id, postObj.Name);
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
                if (postObj != null)
                {
                    this._postService.Refresh(id);
                    TempData["MessageSuccess"] = "Làm mới bài viết thành công!";
                }
                else
                {
                    TempData["MessageError"] = "Không tồn tại bài viết trên";

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
            var postObj = this._postService.FirstOrDefault(m=>m.KeySlug==slug && m.PostType!=PostTypeConst.TOUR);
            if (postObj != null && postObj.Id != id)
                return Json("existed");

            return Json(string.Empty);
        }
        #endregion

        #region #Option
        
        public List<SelectItemModel> GetPostTypeOptions(bool all)
        {
            List<SelectItemModel> list = new List<SelectItemModel>();
            if (all)
                list.Add(new SelectItemModel() { Text = "Chọn loại tin...",Value = " "});
            list.Add(new SelectItemModel("Tin bài", PostTypeConst.POST));
            list.Add(new SelectItemModel("Tin blog", PostTypeConst.BLOG));            
            return list;
        }
               

        /// <summary>
        /// convert status post
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public string GetStatusPost(string status)
        {
            string strStatus = "";
            switch (status.ToUpper())
            {
                case StatusConst.DRAFTNAME:
                    strStatus = "Nháp";
                    break;
                case StatusConst.PUBLISHNAME:
                    strStatus = "Duyệt";
                    break;
            }
            return strStatus;
        }
        #endregion
    }
}