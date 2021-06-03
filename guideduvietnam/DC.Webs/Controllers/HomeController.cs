using DC.Common.Utility;
using DC.Models.Cms;
using DC.Models.Media;
using DC.Models.Posts;
using DC.Services.Cms;
using DC.Services.Media;
using DC.Services.Posts;
using DC.Webs.Common;
using DC.Webs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DC.Webs.Controllers
{
    public class HomeController : BaseController
    {
        HomeViewModel model = new HomeViewModel();
        private readonly ISliderService _slideService;
        private readonly IPostService _posttService;
        private readonly IMenuService _menuService;

        //Ctor
        public HomeController(ISliderService slideService,IPostService posttService, IMenuService menuService)
        {
            _slideService = slideService;
            _posttService = posttService;
            _menuService = menuService;
            model.Language = Language;
        }


        public ActionResult Index(string slugUrl="")
        {            
            if (string.IsNullOrEmpty(slugUrl) && !string.IsNullOrEmpty(UrlIndex))
                return Redirect(string.Format("/{0}", UrlIndex));
            model.ParameterInfo = GetSeoConfig();

            //Get slide
            var slides = this._slideService.GetAll().ToList();
            model.SlideItems = new List<SlideModel>();
            model.PostItems = new List<PostModel>();

            if (slides.Any())
                model.SlideItems = slides.Select(c => c.ToModel()).ToList();


            //Get tour
            List<int> cateIds = new List<int>();
            var posts = this._posttService.GetAll("", cateIds, StatusConst.PUBLISHNAME,null,null,PostTypeConst.TOUR, "CREATEDATE","DESC",1, 15);
            if (posts != null)
            {
                model.PostItems = posts.Select(c => c.ToModel()).ToList();
                foreach(var item in model.PostItems)
                {
                    item.Content = string.Empty;
                    item.Description = item.Description.Length > 240 ? string.Format("{0}...", item.Description.Substring(0, 240)) : item.Description;
                }
            }
            model.LableInfo = new LableModel();
            model.LableInfo.ReadMore = base.GetLableConst(LableConst.READ_MORE, Language);
            model.LableInfo.TitleTourHome = base.GetLableConst(LableConst.TITLE_TOUR_HOME, Language);
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult SiteMap()
        {
            model.ParameterInfo = GetSeoConfig();
            var menuItems = this._menuService.FindAllBy(m => m.MenuType == MenuConst.MAINMENU).OrderBy(m => m.OrderBy).ToList();
            var menuparent = menuItems.Where(m => m.ParentId == 0).ToList();
            model.MenuItems = new List<MenuModel>();
            if (menuparent.Any())
            {
                model.MenuItems = menuparent.Select(c => c.ToModel()).ToList();
                foreach (var item in model.MenuItems)
                {
                    var childItems = menuItems.Where(m => m.ParentId == item.Id).Select(c => new ItemModel()
                    {
                        Id = c.Id,
                        Title = c.Name,
                        Url = c.Url,
                        OrderBy = c.OrderBy
                    }).OrderBy(c => c.OrderBy).ToList();
                    item.ChildItems = childItems;
                    if (childItems.Any())
                    {
                        foreach (var child in item.ChildItems)
                        {
                            child.ChildItems = menuItems.Where(m => m.ParentId == child.Id).Select(c => new SubItemModel()
                            {
                                Id = c.Id,
                                Title = c.Name,
                                Url = c.Url,
                                OrderBy = c.OrderBy
                            }).OrderBy(c => c.OrderBy).ToList();
                        }
                    }

                }
            }
            return View(model);
        }

        public ActionResult Contact()
        {
            model.ParameterInfo = GetSeoConfig();
            return View(model);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Contact(FormCollection collection)
        {
            var response = Request["g-recaptcha-response"];
            if (ValidateCaptcha(response))
            {
                string subjectTitle = "[" + Website + "]Email liên hệ: ";
                string name = collection["Name"] ?? "";
                string phone = collection["Phone"] ?? "";
                string email = collection["Email"] ?? "";
                string content = collection["Content"] ?? "";
                string body = BuildEmailRessetPassword(subjectTitle, name, phone, email, content);
                bool success = UserEmailToken.SendMail(FromEmailAddress, subjectTitle, body);
                TempData["Success"] = "Thông tin của bạn đã được gửi đến quản trị. Xin cảm ơn!";
            }
            return Redirect("/contact");
        }

        #region #BuildEmailTemplates
        /// <summary>
        /// Đọc file mẫu email
        /// </summary>
        /// <param name="emailTemplateFileLocation"></param>
        /// <returns></returns>
        public string GetEmailTemplateFile(string emailTemplateFileLocation)
        {
            string body = string.Empty;

            try
            {
                string filePath = System.Web.HttpContext.Current.Server.MapPath(emailTemplateFileLocation);
                body = System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return body;
        }


        // Build Email Content
        // Merge expired mail
        public string BuildEmailRessetPassword(string title, string name,string phone, string email, string content)
        {
            // 1. Get Email Template
            string body = GetEmailTemplateFile(EmailContactUrl);

            // 2. Merge custom info
            body = body.Replace("{Title}", title);
            body = body.Replace("{Name}", name);
            body = body.Replace("{Phone}", phone);
            body = body.Replace("{Email}", email);
            body = body.Replace("{Content}", content);
            return body;
        }
        #endregion
    }
}