using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DC.Common;
using DC.Models.Posts;
using DC.Services.Cms;
using DC.Services.Posts;
using DC.Webs.Common;
using DC.Webs.Models;

namespace DC.Webs.Controllers
{
    public class NewsController : BaseController
    {
        HomeViewModel model = new HomeViewModel();

        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;

        public NewsController(ICategoryService categoryService, IPostService postService)
        {
            this._categoryService = categoryService;
            this._postService = postService;
            model.Language = Language;
        }

        // GET: News
        public ActionResult Index(string slugUrl)
        {
            if (string.IsNullOrEmpty(slugUrl.Trim()))
                return Redirect("/");
            var postObj = this._postService.FirstOrDefault(m=>m.KeySlug ==slugUrl && m.PostType == PostTypeConst.POST);
            if (postObj == null || postObj.PostType==PostTypeConst.TOUR)
                return Redirect("/");
            model.PostInfo = postObj.ToModel();
            var cateObj = this._categoryService.Find(postObj.CateId);
            if (cateObj != null)
                model.CategoryInfo = cateObj.ToModel();
            var tags = new List<TagModel>();
            if (!string.IsNullOrEmpty(postObj.TagsId))
            {
                List<string> array = postObj.TagsId.Split(',').ToList();
                tags.AddRange(array.Select(item => new TagModel() {KeySlug = SlugExtentions.ToSlug(item), Name = item}));
            }
            model.TagItems = tags;
            model.LableInfo = new LableModel();            
            model.LableInfo.Home = base.GetLableConst(LableConst.HOME, Language);
            return View(model);
        }
    }
}