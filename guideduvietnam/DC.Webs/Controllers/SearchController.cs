using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DC.Models.Posts;
using DC.Services.Cms;
using DC.Services.Posts;
using DC.Webs.Common;
using DC.Webs.Models;

namespace DC.Webs.Controllers
{
    public class SearchController : BaseController
    {
        HomeViewModel model = new HomeViewModel();
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;

        public SearchController(ICategoryService categoryService, IPostService postService)
        {
            this._categoryService = categoryService;
            this._postService = postService;
            model.Language = Language;
        }

        // GET: Search
        public ActionResult Index(string s = "", int page = 1)
        {
            model.ParameterInfo = GetSeoConfig();
            List<int> cateIds = new List<int>();
            List<string> postTypes = new List<string>();
            var posts = this._postService.GetAll(s.Trim(), cateIds, StatusConst.PUBLISHNAME, null, null, postTypes, "CREATEDATE", "DESC", 1, 20);
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