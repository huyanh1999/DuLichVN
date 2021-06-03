using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DC.Common.Utility;
using DC.Models.Authorize;
using DC.Services.Authorize;
using DC.Webs.Common;
using DC.Webs.Models;

namespace DC.Webs.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {

        UserViewModel model = new UserViewModel();
        private readonly IUserService _userService;


        //Ctor
        public UserController(IUserService userService)
        {
            this._userService = userService;
        }



        [AreaAuthorizeAttribute("Admin",Roles = "Administrator")]
        public ActionResult Index()
        {
            model.TabName = "User";
            //model.ActionName = "Home"; 
            SetupBaseModel(model);
            var user = this._userService.GetAll().ToList();
            if (user != null && user.Count > 0)
            {
                model.UserItems = user.Select(c => c.ToModel()).ToList();
            }
            return View(model);
        }


        #region Add        
        [AreaAuthorizeAttribute("Admin", Roles = "Administrator")]
        public ActionResult Add()
        {
            model.TabName = "User";
            SetupBaseModel(model);
            model.RoleOptions = GetUserRoles();
            return View(model);
        }


        [AreaAuthorizeAttribute("Admin", Roles = "Administrator")]
        [HttpPost]
        public ActionResult Add(FormCollection collection)
        {
            try
            {
                int createByUserId = GetCurrentUserId(User.Identity.Name);
                var usermodel = new UserModel();
                usermodel.Guid = Guid.NewGuid().ToString();
                usermodel.FirstName = collection["FirstName"] != null ? collection["FirstName"].ToString() : string.Empty;
                usermodel.LastName = collection["LastName"] != null ? collection["LastName"].ToString() : string.Empty;
                usermodel.Email = collection["Email"].ToString();
                usermodel.UserName = collection["UserName"].ToString();
                usermodel.Password = StringTools.Encryption(collection["Password"].ToString());
                usermodel.RoleName = collection["RoleName"] != null ? collection["RoleName"].ToString() : string.Empty;
                usermodel.Phone = collection["Phone"] != null ? collection["Phone"].ToString() : string.Empty;
                usermodel.Url = collection["Url"] != null ? collection["Url"].ToString() : string.Empty;
                usermodel.Description = collection["Description"] != null ? collection["Description"].ToString() : string.Empty;
                usermodel.ReceiveEmailNotification = collection["ReceiveEmailNotification"] != null && collection["ReceiveEmailNotification"] == "on" ? true : false;
                usermodel.IsLockedOut = collection["IsLockedOut"] != null && collection["IsLockedOut"] == "on" ? true : false;
                usermodel.CreateByUserId = createByUserId;
                var entity = usermodel.ToEntity();
                entity.CreateDate = DateTime.Now;
                this._userService.Add(entity);
                this._userService.Save();
                TempData["MessageSuccess"] = "Thêm mới người dùng thành công.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MessageError"] = ex.ToString();
                SetupBaseModel(model);
                model.RoleOptions = GetUserRoles();
                return View(model);
            }

        }

        #endregion


        #region Edit
        [AreaAuthorizeAttribute("Admin", Roles = "SuperAdministrator,Administrator")]
        public ActionResult Edit(int id)
        {
            model.TabName = "User";
            SetupBaseModel(model);
            model.RoleOptions = GetUserRoles();
            var userObj = this._userService.Find(id);
            if (userObj != null)
            {
                model.UserInfo = userObj.ToModel();
                model.UserInfo.Password = string.Empty;
            }
            else return RedirectToAction("Index");
            return View(model);
        }

        [AreaAuthorizeAttribute("Admin", Roles = "SuperAdministrator,Administrator")]
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var usermodel = _userService.Find(id);
                if (usermodel != null)
                {
                    usermodel.FirstName = collection["FirstName"] != null
                        ? collection["FirstName"].ToString()
                        : string.Empty;
                    usermodel.LastName = collection["LastName"] != null
                        ? collection["LastName"].ToString()
                        : string.Empty;
                    usermodel.FullName = usermodel.FirstName + " " + usermodel.LastName;
                    usermodel.Email = collection["Email"].ToString();
                    if (collection["Password"] != null && !String.IsNullOrEmpty(collection["Password"].ToString()))
                        usermodel.Password = StringTools.Encryption(collection["Password"].ToString());
                    usermodel.Phone = collection["Phone"] != null ? collection["Phone"].ToString() : string.Empty;
                    usermodel.Url = collection["Url"] != null ? collection["Url"].ToString() : string.Empty;
                    usermodel.Description = collection["Description"] != null
                        ? collection["Description"].ToString()
                        : string.Empty;
                    usermodel.ReceiveEmailNotification = collection["ReceiveEmailNotification"] != null &&
                                                         collection["ReceiveEmailNotification"] == "on"
                        ? true
                        : false;
                    usermodel.IsLockedOut = collection["IsLockedOut"] != null && collection["IsLockedOut"] == "LOCK"
                        ? true
                        : false;
                    this._userService.Edit(usermodel);
                    this._userService.Save();
                    TempData["MessageSuccess"] = "Cập nhật người dùng thành công.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["MessageError"] = "Không tồn tại người dùng này!";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                TempData["MessageError"] = ex.ToString();
                return RedirectToAction("Index");
            }

        }
        #endregion

        #region #ChangePass
        [AreaAuthorizeAttribute("Admin")]
        public ActionResult ChangePass()
        {
            model.TabName = "User";
            SetupBaseModel(model);

            return View(model);
        }


        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChangePassword(FormCollection collection)
        {
            try
            {
                if (User.Identity.IsAuthenticated && collection != null)
                {
                    var userObj = this._userService.GetByUserName(User.Identity.Name);
                    if (userObj != null)
                    {
                        string abc = collection["NewPassword"];
                        userObj.Password = StringTools.Encryption(collection["NewPassword"]);
                        this._userService.Edit(userObj);
                        this._userService.Save();
                        TempData["MessageSuccess"] = "Đổi mật khẩu thành công!";
                    }
                }
            }
            catch
            {
                TempData["MessageError"] = "Error. Vui lòng liên hệ quản trị!";
            }
            // Update password

            return RedirectToAction("ChangePass");
        }
        #endregion


        #region #LockUser - UnlockUser - RessetPass

        [AreaAuthorizeAttribute("Admin", Roles = "Administrator")]
        public ActionResult Lock(int id)
        {
            try
            {
                var user = this._userService.Find(id);
                if (user != null)
                    user.IsLockedOut = true;
                this._userService.Edit(user);
                this._userService.Save();
                TempData["MessageSuccess"] = "Đã khóa tài khoản thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MessageError"] = ex.ToString();
                return RedirectToAction("Index");
            }

        }



        [AreaAuthorizeAttribute("Admin", Roles = "Administrator")]
        public ActionResult Unlock(int id)
        {
            try
            {
                var user = this._userService.Find(id);
                if (user != null)
                    user.IsLockedOut = false;
                this._userService.Edit(user);
                this._userService.Save();
                TempData["MessageSuccess"] = "Đã mở khóa tài khoản thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MessageError"] = ex.ToString();
                return RedirectToAction("Index");
            }
        }



        [AreaAuthorizeAttribute("Admin", Roles = "Administrator")]
        public ActionResult ResetPass(int id)
        {
            try
            {

                string pass = StringTools.Encryption(System.Configuration.ConfigurationManager.AppSettings["PassDefault"]);
                var user = this._userService.Find(id);
                if (user != null)
                    user.Password = pass;
                this._userService.Edit(user);
                this._userService.Save();
                TempData["MessageSuccess"] = "Đã resset mật khẩu thành công. Mật khẩu mới là: " + System.Configuration.ConfigurationManager.AppSettings["PassDefault"];
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MessageError"] = ex.ToString();
                return RedirectToAction("Index");
            }
        }
        #endregion


        #region validate



        /// <summary>
        /// check xem tên đăng nhập đã tồn tại khi thêm mới
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        public ActionResult ExistUsername(string username)
        {
            var userObj = this._userService.GetByUserName(username);
            if (userObj != null && userObj.UserName == username)
            {
                return Json("existed", JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region #Options
        private List<SelectItemModel> GetUserStatusOptions()
        {
            List<SelectItemModel> list = new List<SelectItemModel>();
            list.Add(new SelectItemModel("Kích hoạt", "False"));
            list.Add(new SelectItemModel("Tạm khóa", "True"));
            return list;
        }

        private List<SelectItemModel> GetUserRoles()
        {
            List<SelectItemModel> list = new List<SelectItemModel>();
            list.Add(new SelectItemModel(RoleConst.ADMINISTRATOR, RoleConst.ADMINISTRATOR));
            list.Add(new SelectItemModel(RoleConst.ADMIN, RoleConst.ADMIN));
            //list.Add(new SelectItemModel(RoleConst.USER, RoleConst.USER));
            return list;
        }
        #endregion
    }
}