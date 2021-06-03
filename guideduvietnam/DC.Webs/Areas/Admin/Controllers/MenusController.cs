using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DC.Common.Utility;
using DC.Models.Cms;
using DC.Services;
using DC.Webs.Common;
using DC.Webs.Models;
using DC.Services.Cms;
using DC.Services.Posts;

namespace DC.Webs.Areas.Admin.Controllers
{
    public class MenusController : BaseController
    {
        MenuViewModel model = new MenuViewModel();
        private readonly IMenuService _menuService;
        private readonly ICategoryService _categoryService;
        private readonly ITagsService _tagsService;


        //Ctor
        public MenusController(ICategoryService categoryService, IMenuService menuService, ITagsService tagsService)
        {
            this._categoryService = categoryService;
            this._menuService = menuService;
            this._tagsService = tagsService;
        }


        #region #Index
        // GET: Menus

        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Index()
        {
            model.TabName = "Category";
            //model.ActionName = "Home"; 
            SetupBaseModel(model);
            model.MenuTypeOptions = GetMenuType();
            model.MenuSourceOptions = GetMenuSourceType();
            return View(model);
        }

        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetList(string type)
        {

            // Set model
            var menuItems = this._menuService.GetAll(type);
            if (menuItems != null && menuItems.Count > 0)
            {
                model.MenuItems = this.GetAllItems(0, 0, menuItems.Select(c => c.ToModel()).ToList());
                foreach (var item in model.MenuItems)
                {
                    string itemType = item.Type;
                    switch (item.Type.ToUpper())
                    {
                        case CategoryConst.CATEGORYPOST:
                            itemType = "Danh mục Tin";
                            break;
                        case CategoryConst.CATEGORYPRODUCT:
                            itemType = "Danh mục tour";
                            break;
                        case TagConst.TAGPOST:
                            itemType = "Tag tin bài";
                            break;
                        case TagConst.TAGTOUR:
                            itemType = "Tag tour";
                            break;
                    }
                    item.Type = itemType;
                }
            }
            else
            {
                model.TotalPages = 0;
                model.TotalCount = 0;
                model.MenuItems = new List<MenuModel>();
            }
            return Json(model);
        }
        #endregion


        #region #Add
        /// <summary>
        /// Thêm mới menu
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="menuType"></param>
        /// <param name="linkUrl"></param>
        /// <param name="orderby"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        public ActionResult Add(int parentId, string name, string type, string menuType, string linkUrl, int orderby, int itemId = 0)
        {
            try
            {
                string url = "";
                switch (type.ToUpper())
                {
                    case "LINK":
                        url = linkUrl;
                        break;
                    case CategoryConst.CATEGORYPOST:
                        url = "/category/" + this._categoryService.Find(itemId).KeySlug;
                        break;
                    case CategoryConst.CATEGORYPRODUCT:
                        url = "/category/" + this._categoryService.Find(itemId).KeySlug;
                        break;
                    case TagConst.TAGPOST:
                        url = "/tag/" + this._tagsService.Find(itemId).KeySlug;
                        break;
                    case TagConst.TAGTOUR:
                        url = "/tags/" + this._tagsService.Find(itemId).KeySlug;
                        break;
                }
                MenuModel menuModel = new MenuModel();
                menuModel.ItemId = itemId;
                menuModel.ParentId = parentId;
                menuModel.Name = name;
                menuModel.Alias = name;
                menuModel.Url = url;
                menuModel.Type = type;
                menuModel.MenuType = menuType;
                menuModel.OrderBy = orderby;
                this._menuService.Add(menuModel.ToEntity());
                this._menuService.Save();
                // Lưu hành động                
                string comment = string.Format("Thêm mới menu : ID({0}) - {1}", itemId, name);
                AddActivityLog(LogTypeConst.INSERT, comment);
            }
            catch
            {
                return Json("error");
            }
            return Json("sucess");
        }

        #endregion


        #region #Update
        /// <summary>
        /// cập nhật menu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentId"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="menuType"></param>
        /// <param name="linkUrl"></param>
        /// <param name="orderby"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        public ActionResult Update(int id, int parentId, string name, string type, string menuType, string linkUrl, int orderby, int itemId = 0)
        {
            try
            {
                string url = "";
                switch (type.ToUpper())
                {
                    case "LINK":
                        url = linkUrl;
                        break;
                    case CategoryConst.CATEGORYPOST:
                        url = "/category/" + this._categoryService.Find(itemId).KeySlug;
                        break;
                    case CategoryConst.CATEGORYPRODUCT:
                        url = "/category/" + this._categoryService.Find(itemId).KeySlug;
                        break;
                    case TagConst.TAGPOST:
                        url = "/tag/" + this._tagsService.Find(itemId).KeySlug;
                        break;
                    case TagConst.TAGTOUR:
                        url = "/tags/" + this._tagsService.Find(itemId).KeySlug;
                        break;
                }
                var menuObj = this._menuService.Find(id);
                menuObj.ItemId = itemId;
                menuObj.ParentId = parentId;
                menuObj.Name = name;
                menuObj.Alias = name;
                menuObj.Url = url;
                menuObj.Type = type;
                menuObj.MenuType = menuType;
                menuObj.OrderBy = orderby;
                this._menuService.Edit(menuObj);
                this._menuService.Save();
                // Lưu hành động                
                string comment = string.Format("Cập nhật menu menu : ID({0}) - {1}", itemId, name);
                AddActivityLog(LogTypeConst.UPDATE, comment);
            }
            catch
            {
                return Json("error");
            }
            return Json("sucess");
        }

        #endregion


        #region #Delete
        /// <summary>
        /// XÓa tin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                var menuObj = this._menuService.Find(id);

                this._menuService.Delete(id);

                // Lưu hành động                
                string comment = string.Format("Xóa menu : ID({0}) - {1}", menuObj.Id, menuObj.Name);
                AddActivityLog(LogTypeConst.DELETE, comment);
            }
            catch
            {
            }
            return Json(string.Empty);
        }
        #endregion

        #region #Options

        /// <summary>
        /// Combobox loại menu
        /// </summary>
        /// <returns></returns>
        private List<SelectItemModel> GetMenuType()
        {
            List<SelectItemModel> list = new List<SelectItemModel>();
            list.Add(new SelectItemModel()
            {
                Text = "Main Menu",
                Value = MenuConst.MAINMENU
            });
            list.Add(new SelectItemModel()
            {
                Text = "Menu Footer",
                Value = MenuConst.FOOTERMENU
            });
            return list;
        }

        /// <summary>
        /// Combobox loại menu
        /// </summary>
        /// <returns></returns>
        private List<SelectItemModel> GetMenuSourceType()
        {
            List<SelectItemModel> list = new List<SelectItemModel>();
            list.Add(new SelectItemModel()
            {
                Text = "Danh mục tin",
                Value = CategoryConst.CATEGORYPOST
            });
            list.Add(new SelectItemModel()
            {
                Text = "Danh mục tour",
                Value = CategoryConst.CATEGORYPRODUCT
            });
            list.Add(new SelectItemModel()
            {
                Text = "Tag bài viết",
                Value = TagConst.TAGPOST
            });
            list.Add(new SelectItemModel()
            {
                Text = "Tag tour",
                Value = TagConst.TAGTOUR
            });
            list.Add(new SelectItemModel()
            {
                Text = "Nhập link",
                Value = "LINK"
            });
            return list;
        }

        private List<SelectItemModel> GetTagOptions(string type)
        {
            var tagItems = this._tagsService.GetAll(type);
            return tagItems.Select(item => new SelectItemModel()
            {
                Text = item.Name,
                Value = item.Id.ToString()
            }).ToList();
        }

        /// <summary>
        /// Lấy danh sách danh mục theo đệ quy tuần tự cấp danh mục cha
        /// </summary>
        /// <param name="parentId">Mã danh mục cha</param>
        /// <param name="gradeId">Mã cấp: 1,2</param>
        /// <param name="menuItems">Danh sách các mục thuộc 1 loại</param>
        /// <returns></returns>
        private List<MenuModel> GetAllItems(int parentId, int gradeId, List<MenuModel> menuItems)
        {
            List<MenuModel> list = new List<MenuModel>();
            var items = menuItems.Where(c => c.ParentId == parentId).OrderBy(m => m.OrderBy);

            if (items != null)
            {
                gradeId++;
                foreach (var item in items)
                {
                    item.Name = string.Format("{0} {1}", StringTools.MakeSeparator(gradeId), item.Name);
                    list.Add(item);

                    if (item.Id != parentId)
                        list.AddRange(GetAllItems(item.Id, gradeId, menuItems));
                }
            }

            return list;
        }

        /// <summary>
        /// Lấy danh sách danh mục cha của menu
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="gradeId"></param>
        /// <param name="menuItems"></param>
        /// <returns></returns>
        private List<SelectItemModel> GetParentItems(int parentId, int gradeId, List<MenuModel> menuItems)
        {
            List<SelectItemModel> list = new List<SelectItemModel>();
            var items = menuItems.Where(c => c.ParentId == parentId);

            if (items != null)
            {
                gradeId++;
                foreach (var item in items)
                {
                    item.Name = string.Format("{0} {1}", StringTools.MakeSeparator(gradeId), item.Name);
                    list.Add(new SelectItemModel()
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
                    });

                    if (item.Id != parentId && gradeId <= 1)
                        list.AddRange(GetParentItems(item.Id, gradeId, menuItems));
                }
            }

            return list;
        }


        /// <summary>
        /// Danh sách category đệ quy
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="gradeId"></param>
        /// <param name="menuItems"></param>
        /// <returns></returns>
        private List<SelectItemModel> GetCategoryItems(int parentId, int gradeId, List<CategoryModel> menuItems)
        {
            List<SelectItemModel> list = new List<SelectItemModel>();
            var items = menuItems.Where(c => c.ParentId == parentId);

            if (items != null)
            {
                gradeId++;
                foreach (var item in items)
                {
                    item.Name = string.Format("{0} {1}", StringTools.MakeSeparator(gradeId), item.Name);
                    list.Add(new SelectItemModel()
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
                    });

                    if (item.Id != parentId)
                        list.AddRange(GetCategoryItems(item.Id, gradeId, menuItems));
                }
            }

            return list;
        }



        #endregion

        #region #ExtendMethod

        /// <summary>
        /// Danh sách menu cha
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetMenuParentOptions(string type)
        {

            // Set model
            var menuItems = this._menuService.GetAll(type);
            if (menuItems != null && menuItems.Count > 0)
            {
                model.ParentOptions = this.GetParentItems(0, 0, menuItems.Select(c => c.ToModel()).ToList());

            }
            else
            {
                model.TotalPages = 0;
                model.TotalCount = 0;
                model.ParentOptions = new List<SelectItemModel>();
            }
            if (model.ParentOptions != null && model.ParentOptions.Count > 0)
                model.ParentOptions.Insert(0, new SelectItemModel("--Root--", "0"));
            else
                model.ParentOptions.Add(new SelectItemModel("--Root--", "0"));
            return Json(model);
        }


        /// <summary>
        /// Danh sách danh mục, tag : bài viết - sản phẩm
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetMenuConentOptions(string type)
        {

            // Nếu nguồn là danh mục
            if (type.ToUpper() == CategoryConst.CATEGORYPOST || type.ToUpper() == CategoryConst.CATEGORYPRODUCT)
            {
                var menuContentItems = this._categoryService.GetAll(type);
                if (menuContentItems != null && menuContentItems.Count > 0)
                {
                    model.MenuContentOptions = this.GetCategoryItems(0, 0, menuContentItems.Select(c => c.ToModel()).ToList());

                }
                else
                {
                    model.TotalPages = 0;
                    model.TotalCount = 0;
                    model.MenuContentOptions = new List<SelectItemModel>();
                }
            }
            // Nếu nguồn là tag
            if (type.ToUpper() == TagConst.TAGPOST || type.ToUpper() == TagConst.TAGTOUR)
            {
                model.MenuContentOptions = GetTagOptions(type);
            }

            return Json(model);
        }
        #endregion

        #region #Get menu by Id

        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        public ActionResult GetById(int id)
        {
            // Set models
            var menuObj = this._menuService.Find(id);
            if (menuObj != null)
            {
                model.MenuInfo = menuObj.ToModel();
            }
            return Json(model.MenuInfo);
        }
        #endregion
    }
}