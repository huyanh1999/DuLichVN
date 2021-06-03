using DC.Models.Cms;
using DC.Services.Cms;
using DC.Webs.Common;
using DC.Webs.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace DC.Webs.Controllers
{
    public class BaseController : Controller
    {
        public static string Website = ConfigurationManager.AppSettings["Website"];
        public static string FromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
        public static string EmailContactUrl = ConfigurationManager.AppSettings["EmailContactUrl"].ToString();
        public static int PageSizeConst = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
        public static string CustomizeTourUrl = ConfigurationManager.AppSettings["CustomizeTourUrl"];
        public static string Language = ConfigurationManager.AppSettings["Language"];        
        public static string UrlIndex = ConfigurationManager.AppSettings["UrlIndex"];
        public static string SecretkeyCaptcha = ConfigurationManager.AppSettings["Secretkey_CAPTCHA"];
        public static string SitekeyCaptcha = ConfigurationManager.AppSettings["Sitekey_CAPTCHA"];

        private ObjectCache cache = MemoryCache.Default;
        private const string CACHE_MESSAGE_KEY = "Message.ALL";
        private const string CACHE_LABLE_KEY = "Lable.ALL";

        // Fields        

        private readonly ICategoryService _categoryService;
        private readonly IParameterService _parameterService;

        public BaseController()
        {
            this._categoryService = DependencySolve.GetServiceName<ICategoryService>();
            this._parameterService = DependencySolve.GetServiceName<IParameterService>();
            
        }

        public ParametersModel GetSeoConfig()
        {
            ParametersModel model = new ParametersModel();
            List<string> values = new List<string>
            {
                ParameterConst.METATITLE,
                ParameterConst.METAKEYWORD,
                ParameterConst.METADESCRIPTION,
                ParameterConst.PHONE,
                ParameterConst.EMAIL,
                ParameterConst.ADDRESS,
                ParameterConst.YOUTUBE,
                ParameterConst.FACEBOOK,
                ParameterConst.GOOGLEPLUS,
                ParameterConst.TWITTER,
                ParameterConst.COMPANYNAME,
                ParameterConst.GMAP,
                ParameterConst.ABOUT,
                ParameterConst.ABOUTHOME
            };
            var parameterItems = this._parameterService.GetAll(values);
            if (parameterItems != null && parameterItems.Count > 0)
            {
                model.MetaTitle = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.METATITLE).Content;
                model.MetaKeyword = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.METAKEYWORD).Content;
                model.MetaDescription = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.METADESCRIPTION).Content;
                model.Phone = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.PHONE).Content;
                model.Email = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.EMAIL).Content;
                model.Address = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.ADDRESS).Content;
                model.Youtube = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.YOUTUBE).Content;
                model.Facebook = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.FACEBOOK).Content;
                model.GooglePlus = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.GOOGLEPLUS).Content;
                model.Twitter = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.TWITTER).Content;
                model.CompanyName = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.COMPANYNAME).Content;
                model.Gmap = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.GMAP).Content;
                model.About = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.ABOUT).Content;
                model.AboutHome = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.ABOUTHOME).Content;
            }

            return model;
        }

        public List<SelectItemModel> OrderByOptions(bool allOptions)
        {
            List<SelectItemModel> list = new List<SelectItemModel>();
            if(allOptions)
                list.Add(new SelectItemModel() { Text = "Sắp xếp", Value = "0" });
            list.Add(new SelectItemModel() { Text = "A → Z", Value = OrderByConst.AZ.ToString() });
            list.Add(new SelectItemModel() { Text = "Z → A", Value = OrderByConst.ZA.ToString() });
            list.Add(new SelectItemModel() { Text = "Giá tăng dần", Value = OrderByConst.PRICEASC.ToString() });
            list.Add(new SelectItemModel() { Text = "Giá giảm dần", Value = OrderByConst.PRICEDESC.ToString() });
            list.Add(new SelectItemModel() { Text = "Hàng mới nhất", Value = OrderByConst.DATENEW.ToString() });
            list.Add(new SelectItemModel() { Text = "Hàng cũ nhất", Value = OrderByConst.DATEOLD.ToString() });
            return list;
        }

        public List<SelectItemModel> GetPaymentOptions(bool allOptions)
        {
            List<SelectItemModel> list = new List<SelectItemModel>();
            if (allOptions)
                list.Add(new SelectItemModel() { Text = "Chọn phương thức thanh toán", Value = " " });
            list.Add(new SelectItemModel() { Text = "Tiền mặt", Value = PaymentTypeConst.CASH });
            list.Add(new SelectItemModel() { Text = "Chuyển khoản - ATM", Value = PaymentTypeConst.TRANSFER });
            return list;
        }

        public List<SelectItemModel> GetDeliveryOptions(bool allOptions)
        {
            List<SelectItemModel> list = new List<SelectItemModel>();
            if (allOptions)
                list.Add(new SelectItemModel() { Text = "Chọn hình thức giao hàng", Value = " " });
            list.Add(new SelectItemModel() { Text = "Giao hàng tận nơi", Value = DeliveryConst.HOME });
            list.Add(new SelectItemModel() { Text = "Lấy tại cửa hàng", Value = DeliveryConst.STORE });
            list.Add(new SelectItemModel() { Text = "Qua đường bưu điện", Value = DeliveryConst.POSTOFFICE });
            list.Add(new SelectItemModel() { Text = "Hình thức khác", Value = DeliveryConst.OTHER });
            return list;
        }


        /// <summary>
        /// Lấy danh sách thông tin Lable
        /// </summary>
        /// <returns></returns>
        public List<MessageModel> GetLableConst()
        {
            try
            {
                List<MessageModel> items = (List<MessageModel>)cache[CACHE_MESSAGE_KEY];
                if (items == null)
                {
                    var xml = XDocument.Load(Server.MapPath("/App_Data/Lable.xml"));
                    var query = (from c in xml.Root.Descendants("Message")
                                 select new MessageModel
                                 {
                                     Id = c.Attribute("ID").Value,
                                     en = c.Element("en").Value,
                                     fr = c.Element("fr").Value
                                 }).ToList();

                    CacheItemPolicy policy = new CacheItemPolicy();
                    items = query;
                    // Set cache
                    policy.AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddDays(7));
                    cache.Set(CACHE_MESSAGE_KEY, items, policy);
                }
                return items;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy ra Lable theo ngôn ngữ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public string GetLableConst(string id, string language)
        {
            try
            {
                List<MessageModel> lists = (List<MessageModel>)cache[CACHE_MESSAGE_KEY];
                if (lists == null)
                {
                    var xml = XDocument.Load(Server.MapPath("/App_Data/Lable.xml"));
                    var query = (from c in xml.Root.Descendants("Message")
                                 select new MessageModel
                                 {
                                     Id = c.Attribute("ID").Value,
                                     en = c.Element("en").Value,
                                     fr = c.Element("fr").Value
                                 }).ToList();

                    CacheItemPolicy policy = new CacheItemPolicy();
                    lists = query;
                    // Set cache
                    policy.AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddDays(7));
                    cache.Set(CACHE_MESSAGE_KEY, lists, policy);
                }
                var item = lists.FirstOrDefault(m => m.Id == id);
                string message = string.Empty;
                if (item != null)
                {
                    if (language == "en")
                        message = item.en;
                    else if (language == "fr")
                        message = item.fr;
                }
                return message;
            }
            catch
            {
                return "";
            }
        }

        public bool ValidateCaptcha(string response)
        {
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", SecretkeyCaptcha, response));
            var obj = JObject.Parse(result);
            return (bool)obj.SelectToken("success");
        }
    }
}