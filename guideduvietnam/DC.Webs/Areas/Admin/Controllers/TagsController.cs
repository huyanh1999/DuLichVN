using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DC.Entities.Domain;
using DC.Models.Posts;
using DC.Services.Cms;
using DC.Services.Posts;
using DC.Webs.Common;
using DC.Webs.Models;

namespace DC.Webs.Areas.Admin.Controllers
{
    public class TagsController : BaseController
    {
        TagViewModel model = new TagViewModel();
        private readonly ITagsService _tagsService;
        private readonly IMenuService _menuService;

        //Ctor
        public TagsController(IMenuService menuService, ITagsService tagsService)
        {
            this._menuService = menuService;
            this._tagsService = tagsService;
        }


        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Index()
        {
            SetupBaseModel(model);
            model.TagOptions = GetTagOptions(true);
            return View(model);
        }


        /// <summary>
        /// Hàm lấy dữ liệu danh sách tag và trả lên View
        /// </summary>
        /// <param name="query"></param>
        /// <param name="type"></param>
        /// <param name="sortBy"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetList(string query, string type, string sortBy, string orderBy,
            int pageIndex, int pageSize)
        {
            string website = System.Configuration.ConfigurationManager.AppSettings["Website"];
            // Set model
            var tagItems = this._tagsService.GetAlls(query.Trim(), type.Trim(), sortBy, orderBy, pageIndex, pageSize);
            if (tagItems != null && tagItems.Count > 0)
            {
                model.TotalCount = tagItems.TotalCount;
                model.TotalPages = tagItems.TotalPages;
                model.TagItems = tagItems.Select(c => c.ToModel()).ToList();

            }
            else
            {
                model.TotalPages = 0;
                model.TotalCount = 0;
                model.TagItems = new List<TagModel>();
            }
            return Json(model);
        }


        #region #Delete
        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var tagmodel = this._tagsService.Find(id);

                this._tagsService.Delete(id);

                // Lưu hành động
                string comment = string.Format("Xóa tags {0} : ID({1}) - {2} ", tagmodel.TagType, tagmodel.Id, tagmodel.Name);
                AddActivityLog(LogTypeConst.DELETE, comment);
            }
            catch
            {
            }
            return Json(string.Empty);
        }
        #endregion

        private List<SelectItemModel> GetTagOptions(bool all)
        {
            List<SelectItemModel> opt = new List<SelectItemModel>();
            if(all)
                opt.Add(new SelectItemModel() {Text = "--All--",Value = " "});
            opt.Add(new SelectItemModel() { Text = "Tag tour", Value = TagConst.TAGTOUR });
            opt.Add(new SelectItemModel() { Text = "Tag bài viết", Value = TagConst.TAGPOST });
            return opt;
        }
    }

}