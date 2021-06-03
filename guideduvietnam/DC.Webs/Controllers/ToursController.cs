using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DC.Common;
using DC.Common.Utility;
using DC.Models.Cms;
using DC.Models.Posts;
using DC.Services.Cms;
using DC.Services.Posts;
using DC.Webs.Common;
using DC.Webs.Models;

namespace DC.Webs.Controllers
{
    public class ToursController : BaseController
    {
        HomeViewModel model = new HomeViewModel();
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;
        private readonly IPostImageService _postImageService;
        private readonly IParameterService _parameterService;
        private readonly IHotelService _hotelService;

        public ToursController(ICategoryService categoryService, IPostService postService, IPostImageService postImageService,
            IParameterService parameterService, IHotelService hotelService)
        {
            this._categoryService = categoryService;
            this._postService = postService;
            this._postImageService = postImageService;
            this._parameterService = parameterService;
            this._hotelService = hotelService;
            model.Language = Language;
        }

        // GET: Tours
        public ActionResult Index(string slugUrl)
        {
            if (string.IsNullOrEmpty(slugUrl.Trim()))
                return Redirect("/");
            var postObj = this._postService.FirstOrDefault(m => m.KeySlug == slugUrl && m.PostType == PostTypeConst.TOUR);
            if (postObj == null || postObj.PostType != PostTypeConst.TOUR)
                return Redirect("/");
            model.PostInfo = postObj.ToModel();
            var cateObj = this._categoryService.Find(postObj.CateId);
            if (cateObj != null)
                model.CategoryInfo = cateObj.ToModel();
            var tags = new List<TagModel>();
            if (!string.IsNullOrEmpty(postObj.TagsId))
            {
                List<string> array = postObj.TagsId.Split(',').ToList();
                tags.AddRange(array.Select(item => new TagModel() { KeySlug = SlugExtentions.ToSlug(item), Name = item }));
            }
            model.TagItems = tags;
            //image slide
            List<PostImageModel> imgItems = new List<PostImageModel>();
            
            var tourImgs = this._postImageService.FindAllBy(m => m.PostId == postObj.Id).ToList();
            if (tourImgs.Any())
            {
                imgItems.AddRange(tourImgs.Select(item => new PostImageModel()
                {
                    Images = item.Images
                }));
            }
            else
            {
                imgItems.Add(new PostImageModel()
                {
                    Images = postObj.Picture
                });
            }
            model.PostImageItems = imgItems;

            //Tour liên quan
            model.PostItems = new List<PostModel>();
            List<string> postTypes = new List<string>();
            postTypes.Add(PostTypeConst.TOUR);
            var objRelateds = this._postService.GetPostRelated(postObj.Id, postObj.CateId, StatusConst.PUBLISHNAME,postTypes, 9);
            if (objRelateds.Any())
                model.PostItems = objRelateds.Select(c => c.ToModel()).ToList();

            //Form customize tour
            model.ParameterInfo = new ParametersModel();            
            var parameterItems = this._parameterService.FirstOrDefault(m=>m.Value== ParameterConst.CUSTOMIZETOUR);
            model.ParameterInfo.CustomizeTour = parameterItems!=null? parameterItems.Content : string.Empty;
            
            //Hotel
            model.HotelItems = new List<HotelModel>();
            var hotels = this._hotelService.GetAll().OrderBy(m => m.OrderBy).ToList();
            if (hotels.Any())
                model.HotelItems = hotels.Select(c => c.ToModel()).ToList();
            model.LableInfo = new LableModel();
            model.LableInfo.Home = base.GetLableConst(LableConst.HOME, Language);
            model.CustomizeTourModel = new CustomizeTourModel();
            model.CustomizeTourModel.TourName = postObj.Name;
            return View(model);
        }


        [HttpPost]
        public ActionResult CustomizeTour(CustomizeTourModel item,string response)
        {
            try
            {
                if (ValidateCaptcha(response))
                {
                    string subjectTitle = "[" + Website + "]Email customize tour: ";
                    string body = BuildEmailTour(subjectTitle, item);
                    bool success = UserEmailToken.SendMail(FromEmailAddress, subjectTitle, body);
                }
            }
            catch (Exception)
            {
                return Json("ERROR");
            }                        
            return Json(string.Empty);
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
        public string BuildEmailTour(string title,CustomizeTourModel item)
        {
            // 1. Get Email Template
            string body = GetEmailTemplateFile(CustomizeTourUrl);
            string hotelName = string.Empty;
            if (!string.IsNullOrEmpty(item.HotelId))
            {
                List<int> hotelIds = NumberTools.ConvertToListInt(item.HotelId);
                var hotels = this._hotelService.FindAllBy(m => hotelIds.Contains(m.Id)).ToList();
                foreach (var ht in hotels)
                {
                    hotelName += string.Format("<p>-{0}</p>", ht.Name);
                }
            }
            // 2. Merge custom info
            body = body.Replace("{Title}", title);
            body = body.Replace("{TourName}", item.TourName);
            body = body.Replace("{Name}", item.Name);
            body = body.Replace("{YourName}", item.YourName);
            body = body.Replace("{Email}", item.Email);
            body = body.Replace("{Nationality}", item.Nationality);
            body = body.Replace("{Telephone}", item.Telephone);
            body = body.Replace("{ArrivalDate}", item.ArrivalDate);
            body = body.Replace("{DepatureDate}", item.DepatureDate);
            body = body.Replace("{PersonNumber}", item.PersonNumber);
            body = body.Replace("{HotelName}", hotelName);
            body = body.Replace("{Lunch}", item.Lunch==true?"Yes":"No");
            body = body.Replace("{Dinner}", item.Dinner == true ? "Yes" : "No");
            body = body.Replace("{Flight}", item.Dinner == true ? "Yes" : "No");
            body = body.Replace("{Message}", item.Message);
            return body;
        }
        #endregion
    }

}