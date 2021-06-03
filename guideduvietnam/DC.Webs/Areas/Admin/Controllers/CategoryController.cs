using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DC.Common;
using DC.Models.Cms;
using DC.Services;
using DC.Webs.Common;
using DC.Webs.Models;
using DC.Services.Cms;

namespace DC.Webs.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        CategoryViewModel model = new CategoryViewModel();
        private readonly ICategoryService _categoryService;
        private readonly IMenuService _menuService;



        //Ctor
        public CategoryController(ICategoryService categoryService, IMenuService menuService)
        {
            this._categoryService = categoryService;
            this._menuService = menuService;
        }



        // GET: Category
        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Index()
        {
            SetupBaseModel(model);
            model.TypeOptions = base.GetCateTypeOptions(true);
            return View(model);
        }

        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetList(string query, string type, string sortBy, string orderBy,
            int pageIndex, int pageSize)
        {
            var cateItems = this._categoryService.GetAll().ToList();
            // Set model
            var objItems = this._categoryService.GetAll(query.Trim(),type.Trim(), sortBy, orderBy, pageIndex, pageSize);
            if (objItems != null && objItems.Count > 0)
            {
                model.TotalCount = objItems.TotalCount;
                model.TotalPages = objItems.TotalPages;
                model.CategoryItems = objItems.Select(c => c.ToModel()).ToList();
                foreach (var item in model.CategoryItems)
                {
                    var cateObj = cateItems.FirstOrDefault(m => m.Id == item.ParentId);
                    item.ParentName = cateObj != null ? cateObj.Name : "--Root--";
                }             
            }
            else
            {
                model.TotalPages = 0;
                model.TotalCount = 0;
                model.CategoryItems = new List<CategoryModel>();
            }
            return Json(model);
        }

        #region #Add

        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Add()
        {
            SetupBaseModel(model);
            model.TypeOptions = base.GetCateTypeOptions(false);
            model.ParentItems = GetParentItems(CategoryConst.CATEGORYPOST);
            return View(model);
        }



        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(FormCollection collection)
        {
            try
            {
                int currentUserId = GetCurrentUserId(User.Identity.Name);
                var catemodel = new CategoryModel();
                catemodel.KeySlug = collection["Url"] != null ? SlugExtentions.ToSlug(collection["Url"]) : string.Empty;
                catemodel.Name = collection["Name"]?.ToString() ?? string.Empty;
                catemodel.Title = collection["Title"]?.ToString() ?? string.Empty;
                catemodel.Url = collection["Url"]?.ToString() ?? string.Empty;
                catemodel.CateType = collection["Type"]?.ToString() ?? CategoryConst.CATEGORYPOST;
                catemodel.Description = collection["Description"]?.ToString() ?? string.Empty;
                catemodel.MetaKeyword = collection["MetaKeyword"]?.ToString() ?? string.Empty;
                catemodel.MetaDescription = collection["MetaDescription"]?.ToString() ?? string.Empty;
                catemodel.ParentId = collection["ParentId"] != null ? int.Parse(collection["ParentId"]) : 0;
                catemodel.OrderBy = collection["OrderBy"] != null ? int.Parse(collection["OrderBy"]) : 1;                            
                this._categoryService.Add(catemodel.ToEntity());
                this._categoryService.Save();
                TempData["MessageSuccess"] = "Thêm mới danh mục thành công!";

                // Lưu hành động
                string comment = string.Format("Tạo mới danh mục {0} : {1}", catemodel.CateType, catemodel.Name);
                AddActivityLog(LogTypeConst.INSERT,comment);

                
            }
            catch
            {
                TempData["MessageError"] = "ERROR";                
            }
            return RedirectToAction("Index");
        }

        #endregion



        #region #Edit

        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Edit(int id)
        {
            SetupBaseModel(model);
            var cateObj = this._categoryService.Find(id);
            if (cateObj == null)
                return RedirectToAction("Index");

            model.CategoryInfo = cateObj.ToModel();
            model.TypeOptions = base.GetCateTypeOptions(false);
            model.ParentItems = GetParentItems(id, cateObj.CateType);            
            return View(model);
        }


        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {                
                var catemodel = _categoryService.Find(id);
                if (catemodel != null)
                {
                    catemodel.KeySlug = collection["Url"] != null
                        ? SlugExtentions.ToSlug(collection["Url"])
                        : string.Empty;
                    catemodel.Name = collection["Name"]?.ToString() ?? string.Empty;
                    catemodel.Title = collection["Title"]?.ToString() ?? string.Empty;
                    catemodel.Url = collection["Url"]?.ToString() ?? string.Empty;
                    catemodel.CateType = collection["Type"]?.ToString() ?? CategoryConst.CATEGORYPOST;
                    catemodel.Description = collection["Description"]?.ToString() ?? string.Empty;
                    catemodel.MetaKeyword = collection["MetaKeyword"]?.ToString() ?? string.Empty;
                    catemodel.MetaDescription = collection["MetaDescription"]?.ToString() ?? string.Empty;
                    catemodel.ParentId = collection["ParentId"] != null ? int.Parse(collection["ParentId"]) : 0;
                    catemodel.OrderBy = collection["OrderBy"] != null ? int.Parse(collection["OrderBy"]) : 1;
                    
                    this._categoryService.Update(catemodel);
                    TempData["MessageSuccess"] = "Cập nhật danh mục thành công!";

                    #region #Update lại Table menu

                    var menuItems = this._menuService.GetAll(id, catemodel.CateType);
                    foreach (var item in menuItems)
                    {
                        item.Url =string.Format("/{0}/{1}", "category", catemodel.KeySlug);
                    }
                    this._menuService.Edit(menuItems.AsQueryable());
                    this._menuService.Save();

                    #endregion

                    // Lưu hành động
                    string comment = string.Format("Cập nhật danh mục {0} :ID({1}) - {2}", catemodel.CateType, catemodel.Id, catemodel.Name);
                    AddActivityLog(LogTypeConst.UPDATE, comment);
                }
                else
                {
                    TempData["MessageError"] = "Error!";
                }
                
            }
            catch
            {
                TempData["MessageError"] = "Error!";                
            }
            return RedirectToAction("Index");
        }
        #endregion

        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Delete(int id, string type)
        {
            try
            {                
                var cateObj = this._categoryService.Find(id);
                this._categoryService.Delete(id);
                TempData["MessageSuccess"] = "Xóa danh mục thành công!";
                // Lưu hành động
                string comment = string.Format("Xóa danh mục {0} :ID({1}) - {2}", CategoryConst.CATEGORYPOST, cateObj.Id, cateObj.Name);
                AddActivityLog(LogTypeConst.DELETE, comment);
            }
            catch
            {
                TempData["MessageError"] = "Error!";
            }
            return RedirectToAction("Index");
        }

        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Delete(int id)
        {
            try
            {
                var examObj = this._categoryService.Find(id);
                this._categoryService.Delete(id);                
                string comment = string.Format("Delete exam test : ID({0}) - {1}", examObj.Id, examObj.Title);
                base.AddActivityLog(LogTypeConst.DELETE, comment);

                return Json(string.Empty);
            }
            catch
            {
                return Json("ERROR");
            }
        }

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
            var cateObj = this._categoryService.GetByKeySlug(slug);
            if (cateObj != null && cateObj.Id != id)
                return Json("existed");

            return Json(string.Empty);
        }


        #region #Option

        public List<SelectItemModel> GetParentItems(string type)
        {
            List<SelectItemModel> list = new List<SelectItemModel>();
            list.Add(new SelectItemModel()
            {
                Text = "--Root--",
                Value = "0"
            });
            var cateObj = this._categoryService.GetAll(type);
            int n = cateObj.Count;
            for (int i = 0; i < n; i++)
            {
                if (cateObj[i].ParentId == 0)
                {
                    list.Add(new SelectItemModel()
                    {
                        Text = cateObj[i].Name,
                        Value = cateObj[i].Id.ToString()
                    });
                    for (int j = 0; j < n; j++)
                    {
                        if (cateObj[j].ParentId == cateObj[i].Id)
                        {
                            list.Add(new SelectItemModel()
                            {
                                Text = "--" + cateObj[j].Name,
                                Value = cateObj[j].Id.ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }




        public List<SelectItemModel> GetParentItems(int id, string type)
        {
            List<SelectItemModel> list = new List<SelectItemModel>();
            list.Add(new SelectItemModel()
            {
                Text = "--Root--",
                Value = "0"
            });
            var cateObj = this._categoryService.GetAll(type).Where(m => m.Id != id).ToList();
            int n = cateObj.Count;
            for (int i = 0; i < n; i++)
            {
                if (cateObj[i].ParentId == 0)
                {
                    list.Add(new SelectItemModel()
                    {
                        Text = cateObj[i].Name,
                        Value = cateObj[i].Id.ToString()
                    });
                    for (int j = 0; j < n; j++)
                    {
                        if (cateObj[j].ParentId == cateObj[i].Id)
                        {
                            list.Add(new SelectItemModel()
                            {
                                Text = "--" + cateObj[j].Name,
                                Value = cateObj[j].Id.ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region #Extend method

        [HttpPost]
        public ActionResult GetParent(string type)
        {
            model.ParentItems = GetParentItems(type);
            return Json(model);
        }
        #endregion
    }
}