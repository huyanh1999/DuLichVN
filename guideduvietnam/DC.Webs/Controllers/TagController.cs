using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DC.Common.Utility;
using DC.Models.Posts;
using DC.Services.Cms;
using DC.Services.Posts;
using DC.Webs.Common;
using DC.Webs.Models;

namespace DC.Webs.Controllers
{
    public class TagController : BaseController
    {
        HomeViewModel model = new HomeViewModel();
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;
        private readonly ITagsService _tagsService;


        //Ctor
        public TagController(IPostService postService, ICategoryService categoryService, ITagsService tagsService)
        {
            _postService = postService;
            _categoryService = categoryService;
            _tagsService = tagsService;
            model.Language = Language;
        }

        // GET: Tag
        public ActionResult Index(string slugUrl = "", string type="", int page = 1)
        {
            model.ParameterInfo = GetSeoConfig();
            model.TagInfo = new TagModel();
            var tagObj = this._tagsService.FirstOrDefault(m=>m.KeySlug==slugUrl && m.TagType==type);
            if (tagObj == null)
                return Redirect("/");
            model.TagInfo = tagObj.ToModel();            
            List<string> postTypes = new List<string>();
            if (type.Equals(TagConst.TAGTOUR, StringComparison.OrdinalIgnoreCase))
            {
                postTypes.Add(PostTypeConst.TOUR);
            }
            else
            {
                postTypes.Add(PostTypeConst.POST);
                postTypes.Add(PostTypeConst.BLOG);
                postTypes.Add(PostTypeConst.HOT);
                postTypes.Add(PostTypeConst.PROMOTION);
            }

            //
            var posts = this._postService.GetAllByTag("", tagObj.Name, StatusConst.PUBLISHNAME, null, null, postTypes, "CREATEDATE", "DESC", page, 20);
            if (posts != null)
            {
                model.TotalCount = posts.TotalCount;
                model.TotalPages = posts.TotalPages;
                model.PageIndex = page;
                model.PostItems = posts.Select(c => c.ToModel()).ToList();
                foreach (var item in model.PostItems)
                {
                    item.Content = string.Empty;
                    item.Description = item.Description.Length > 240 ? string.Format("{0}...", item.Description.Substring(0, 240)) : item.Description;
                }
            }
            else
            {
                model.TotalCount = 0;
                model.TotalPages = 0;
                model.PageIndex = page;
                model.PostItems = new List<PostModel>();
            }
            model.LableInfo = new LableModel();
            model.LableInfo.ReadMore = base.GetLableConst(LableConst.READ_MORE, Language);
            model.LableInfo.Home = base.GetLableConst(LableConst.HOME, Language);
            return View(model);
        }
    }
}