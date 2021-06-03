using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DC.Common.Utility;
using DC.Webs.Models;

namespace DC.Webs.Areas.Admin.Controllers
{
    public class LoginController : BaseController
    {


        // GET: Admin/Login
        public ActionResult Index()
        {
            string password = StringTools.Encryption("bacgiang");
            LoginViewModels model = new LoginViewModels();            
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Index(LoginViewModels model)
        {
            if (ModelState.IsValid)
            {
                // Check validate login                
                string roleName = string.Empty;
                //string password = model.Password;
                string password = StringTools.Encryption(model.Password);
                bool loginSuccessful = ValidateUser(model.UserName, password, ref roleName);

                if (!loginSuccessful)
                    TempData["MessageError"] = "Sai tên đăng nhập hoặc mật khẩu.";                
                else
                {
                    // Sign in for this current user
                    FormsAuthentication.SignOut();
                    FormsAuthentication.SetAuthCookie(model.UserName, true);

                    // Set current user logged in   
                    var authTicket = new FormsAuthenticationTicket(
                        1,
                        model.UserName,
                        DateTime.Now,
                        DateTime.Now.AddDays(7), // expiry
                        false,
                        roleName,
                        "/"
                    );
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie);

                    return Redirect("/Admin/Dashboard");
                }
            }

            return View(model);
        }


        public ActionResult SingOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Admin/Login");
        }
    }
}