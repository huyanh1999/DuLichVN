using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using DC.Common.Utility;
using DC.Models.Cms;
using DC.Webs.Common;
using DC.Webs.Models;
using DC.Services.Cms;
using DC.Services.Posts;

namespace DC.Webs.Areas.Admin.Controllers
{
    public class SettingController : BaseController
    {
        ParameterViewModel model = new ParameterViewModel();

        private readonly IParameterService _parameterService;
        private readonly IPostService _postService;


        //Ctor
        public SettingController(IParameterService parameterService, IPostService postService)
        {
            this._parameterService = parameterService;
            this._postService = postService;
        }



        #region #Cấu hình thông tin công ty

        /// <summary>
        /// Cấu hình thông tin công ty
        /// </summary>
        /// <returns></returns>
        [AreaAuthorizeAttribute("Admin")]
        public ActionResult CompanyInfo()
        {
            model.TabName = "Setting";
            //model.ActionName = "Home"; 
            SetupBaseModel(model);
            List<string> values = new List<string>
            {
                ParameterConst.COMPANYNAME,
                ParameterConst.ADDRESS,
                ParameterConst.PHONE,
                ParameterConst.EMAIL,
                ParameterConst.BUSINESSLICENSE,
                ParameterConst.GMAP,
                ParameterConst.FOOTER,
                ParameterConst.ABOUTHOME,
                ParameterConst.FRMCONTACT,
                ParameterConst.CUSTOMIZETOUR,
                ParameterConst.ABOUT
            };
            model.ParameterInfo = new ParametersModel();
            var parameterItems = this._parameterService.GetAll(values);
            if (parameterItems != null && parameterItems.Count > 0)
            {
                model.ParameterInfo.CompanyName = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.COMPANYNAME).Content;
                model.ParameterInfo.Address = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.ADDRESS).Content;
                model.ParameterInfo.Phone = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.PHONE).Content;
                model.ParameterInfo.Email = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.EMAIL).Content;
                model.ParameterInfo.BusinessLicense = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.BUSINESSLICENSE).Content;
                model.ParameterInfo.Gmap = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.GMAP).Content;
                model.ParameterInfo.Footer = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.FOOTER).Content;
                model.ParameterInfo.AboutHome = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.ABOUTHOME).Content;
                model.ParameterInfo.FrmContact = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.FRMCONTACT).Content;
                model.ParameterInfo.CustomizeTour = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.CUSTOMIZETOUR).Content;
                model.ParameterInfo.About = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.ABOUT).Content;
            }

            return View(model);
        }


        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CompanyInfo(FormCollection collection)
        {
            try
            {
                string companyName = collection["COMPANYNAME"] ?? "";
                string address = collection["ADDRESS"] ?? "";
                string phone = collection["PHONE"] ?? "";
                string email = collection["EMAIL"] ?? "";
                string license = collection["BUSINESSLICENSE"] ?? "";
                string gmap = collection["GMAP"] ?? "";
                string footer = collection["FOOTER"] ?? "";
                string aboutHome = collection["ABOUTHOME"] ?? "";
                string frmContact = collection["FRMCONTACT"] ?? "";
                string customizeTour = collection["CUSTOMIZETOUR"] ?? "";
                string about = collection["ABOUT"] ?? "";
                List<string> values = new List<string>
                {
                    ParameterConst.COMPANYNAME,
                    ParameterConst.ADDRESS,
                    ParameterConst.PHONE,
                    ParameterConst.EMAIL,
                    ParameterConst.BUSINESSLICENSE,
                    ParameterConst.GMAP,
                    ParameterConst.FOOTER,
                    ParameterConst.ABOUTHOME,
                    ParameterConst.FRMCONTACT,
                    ParameterConst.CUSTOMIZETOUR,
                    ParameterConst.ABOUT
                };
                var parameterItems = this._parameterService.GetAll(values);
                foreach (var item in parameterItems)
                {
                    if (item.Value.Equals(ParameterConst.COMPANYNAME, StringComparison.OrdinalIgnoreCase))
                        item.Content = companyName;
                    if (item.Value.Equals(ParameterConst.ADDRESS, StringComparison.OrdinalIgnoreCase))
                        item.Content = address;
                    if (item.Value.Equals(ParameterConst.PHONE, StringComparison.OrdinalIgnoreCase))
                        item.Content = phone;
                    if (item.Value.Equals(ParameterConst.EMAIL, StringComparison.OrdinalIgnoreCase))
                        item.Content = email;
                    if (item.Value.Equals(ParameterConst.BUSINESSLICENSE, StringComparison.OrdinalIgnoreCase))
                        item.Content = license;
                    if (item.Value.Equals(ParameterConst.GMAP, StringComparison.OrdinalIgnoreCase))
                        item.Content = gmap;
                    if (item.Value.Equals(ParameterConst.FOOTER, StringComparison.OrdinalIgnoreCase))
                        item.Content = footer;
                    if (item.Value.Equals(ParameterConst.CUSTOMIZETOUR, StringComparison.OrdinalIgnoreCase))
                        item.Content = customizeTour;
                    if (item.Value.Equals(ParameterConst.FRMCONTACT, StringComparison.OrdinalIgnoreCase))
                        item.Content = frmContact;
                    if (item.Value.Equals(ParameterConst.ABOUTHOME, StringComparison.OrdinalIgnoreCase))
                        item.Content = aboutHome;
                    if (item.Value.Equals(ParameterConst.ABOUT, StringComparison.OrdinalIgnoreCase))
                        item.Content = about;
                }

                this._parameterService.Edit(parameterItems.AsQueryable());
                this._parameterService.Save();
                TempData["MessageSuccess"] = "Cấu hình thành công!";

            }
            catch (Exception ex)
            {
                TempData["MessageError"] = ex.ToString();
            }


            return RedirectToAction("CompanyInfo");
        }

        #endregion



        #region #Cấu hình Seo
        /// <summary>
        /// Cấu hình seo
        /// </summary>
        /// <returns></returns>
        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Seo()
        {
            model.TabName = "Setting";
            //model.ActionName = "Home"; 
            SetupBaseModel(model);
            List<string> values = new List<string>
            {
                ParameterConst.METATITLE,
                ParameterConst.METAKEYWORD,
                ParameterConst.METADESCRIPTION
            };
            model.ParameterInfo = new ParametersModel();
            var parameterItems = this._parameterService.GetAll(values);
            if (parameterItems != null && parameterItems.Count > 0)
            {
                model.ParameterInfo.MetaTitle = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.METATITLE).Content;
                model.ParameterInfo.MetaKeyword = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.METAKEYWORD).Content;
                model.ParameterInfo.MetaDescription = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.METADESCRIPTION).Content;
            }
            return View(model);
        }



        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Seo(FormCollection collection)
        {
            try
            {
                string metaTitle = collection["METATITLE"] ?? "";
                string metaKeyword = collection["METAKEYWORD"] ?? "";
                string metaDescription = collection["METADESCRIPTION"] ?? "";

                List<string> values = new List<string>
                {
                     ParameterConst.METATITLE,
                    ParameterConst.METAKEYWORD,
                    ParameterConst.METADESCRIPTION
                };
                var parameterItems = this._parameterService.GetAll(values);
                foreach (var item in parameterItems)
                {
                    if (item.Value.Equals(ParameterConst.METATITLE, StringComparison.OrdinalIgnoreCase))
                        item.Content = metaTitle;
                    if (item.Value.Equals(ParameterConst.METAKEYWORD, StringComparison.OrdinalIgnoreCase))
                        item.Content = metaKeyword;
                    if (item.Value.Equals(ParameterConst.METADESCRIPTION, StringComparison.OrdinalIgnoreCase))
                        item.Content = metaDescription;

                }

                this._parameterService.Edit(parameterItems.AsQueryable());
                this._parameterService.Save();
                TempData["MessageSuccess"] = "Cấu hình thành công!";

            }
            catch (Exception ex)
            {
                TempData["MessageError"] = ex.ToString();
            }


            return RedirectToAction("Seo");
        }

        #endregion


        #region #Cấu hình mạng xã hội
        /// <summary>
        /// Cấu hình mạng xã hội
        /// </summary>
        /// <returns></returns>
        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Social()
        {
            model.TabName = "Setting";
            //model.ActionName = "Home"; 
            SetupBaseModel(model);
            List<string> values = new List<string>
            {
                ParameterConst.TWITTER,
                ParameterConst.FACEBOOK,
                ParameterConst.GOOGLEPLUS,
                ParameterConst.YOUTUBE
            };
            model.ParameterInfo = new ParametersModel();
            var parameterItems = this._parameterService.GetAll(values);
            if (parameterItems != null && parameterItems.Count > 0)
            {
                model.ParameterInfo.Twitter = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.TWITTER).Content;
                model.ParameterInfo.Facebook = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.FACEBOOK).Content;
                model.ParameterInfo.GooglePlus = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.GOOGLEPLUS).Content;
                model.ParameterInfo.Youtube = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.YOUTUBE).Content;
            }
            return View(model);
        }



        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Social(FormCollection collection)
        {
            try
            {
                string twitter = collection["TWITTER"] ?? "";
                string facebook = collection["FACEBOOK"] ?? "";
                string googleplus = collection["GOOGLEPLUS"] ?? "";
                string youtube = collection["YOUTUBE"] ?? "";

                List<string> values = new List<string>
                {
                    ParameterConst.TWITTER,
                    ParameterConst.FACEBOOK,
                    ParameterConst.GOOGLEPLUS,
                    ParameterConst.YOUTUBE
                };
                var parameterItems = this._parameterService.GetAll(values);
                foreach (var item in parameterItems)
                {
                    if (item.Value.Equals(ParameterConst.TWITTER, StringComparison.OrdinalIgnoreCase))
                        item.Content = twitter;
                    if (item.Value.Equals(ParameterConst.FACEBOOK, StringComparison.OrdinalIgnoreCase))
                        item.Content = facebook;
                    if (item.Value.Equals(ParameterConst.GOOGLEPLUS, StringComparison.OrdinalIgnoreCase))
                        item.Content = googleplus;
                    if (item.Value.Equals(ParameterConst.YOUTUBE, StringComparison.OrdinalIgnoreCase))
                        item.Content = youtube;

                }

                this._parameterService.Edit(parameterItems.AsQueryable());
                this._parameterService.Save();
                TempData["MessageSuccess"] = "Cấu hình thành công!";

            }
            catch (Exception ex)
            {
                TempData["MessageError"] = ex.ToString();
            }


            return RedirectToAction("Social");
        }

        #endregion


        #region  #Tạo sitemap

        [AreaAuthorizeAttribute("Admin")]
        public ActionResult RenderSitemap()
        {
            model.TabName = "Setting";
            //model.ActionName = "Home"; 
            SetupBaseModel(model);
            return View(model);
        }

        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        public ActionResult RenderSitemap(FormCollection collection)
        {
            try
            {
                string domainname = System.Configuration.ConfigurationManager.AppSettings["Website"];

                Response.Clear();
                Response.ContentType = "text/xml";
                using (XmlTextWriter writer = new XmlTextWriter(Server.MapPath("/sitemap.xml"), Encoding.UTF8))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("urlset");
                    writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");                    
                    var lsitemapPost = this._postService.GetAll().Where(m => m.Status.ToUpper() == StatusConst.PUBLISHNAME).ToList();                    
                    foreach (var item in lsitemapPost)
                    {
                        writer.WriteStartElement("url");
                        if(item.PostType== PostTypeConst.TOUR)
                            writer.WriteElementString("loc", domainname + "/tour/" + item.KeySlug);
                        else
                            writer.WriteElementString("loc", domainname + "/post/" + item.KeySlug);
                        writer.WriteElementString("lastmod", String.Format("{0:yyyy-MM-dd}", DateTime.Now));
                        writer.WriteElementString("changefreq", "daily");
                        writer.WriteElementString("priority", "1");
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                }

                TempData["MessageSuccess"] = "Tạo sitemap thành công!";

            }
            catch (Exception ex)
            {
                TempData["MessageError"] = ex.ToString();
            }
            return Redirect("/sitemap.xml");
        }
        #endregion


        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Map()
        {
            SetupBaseModel(model);
            model.ParameterInfo = new ParametersModel();
            var parameterObj = this._parameterService.FirstOrDefault(m=>m.Value== ParameterConst.IMGMAP);
            if (parameterObj != null)
            {
                model.ParameterInfo.ImgMap = parameterObj.Content;
            }
            return View(model);
        }


        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadMap()
        {
            try
            {
                bool maxsize = true;
                string picture = AddFiles(MapImageUrl, ref maxsize);
                model.ParameterInfo = new ParametersModel();
                var parameterObj = this._parameterService.FirstOrDefault(m => m.Value == ParameterConst.IMGMAP);
                if (parameterObj != null && !string.IsNullOrEmpty(picture))
                {
                    parameterObj.Content = picture;
                    this._parameterService.Edit(parameterObj);
                    this._parameterService.Save();
                }
                
                TempData["MessageSuccess"] = "Cấu hình thành công!";
            }
            catch
            {
                TempData["MessageError"] = "Error";
            }

            return RedirectToAction("Map");
        }
    }
}