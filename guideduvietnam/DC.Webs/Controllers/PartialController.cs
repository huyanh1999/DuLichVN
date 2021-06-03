using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DC.Models.Cms;
using DC.Models.Media;
using DC.Models.Posts;
using DC.Services.Cms;
using DC.Services.Media;
using DC.Services.Posts;
using DC.Webs.Common;
using DC.Webs.Models;

namespace DC.Webs.Controllers
{
    public class PartialController : BaseController
    {
        PublicViewModel model = new PublicViewModel();
        private readonly ICategoryService _categoryService;
        private readonly IMenuService _menuService;
        private readonly IParameterService _parameterService;
        private readonly ISliderService _sliderService;
        private readonly IPostService _postService;
        private readonly ITagsService _tagsService;

        

        public PartialController(ICategoryService categoryService, IMenuService menuService, IParameterService parameterService,
            ISliderService sliderService, IPostService postService, ITagsService tagsService)
        {            
            _categoryService = categoryService;
            _menuService = menuService;
            _parameterService = parameterService;
            _sliderService = sliderService;
            _postService = postService;
            this._tagsService = tagsService;
            model.Language = Language;

        }


        public ActionResult Menu(string type)
        {            
            var menuItems = this._menuService.FindAllBy(m=>m.MenuType==type).OrderBy(m => m.OrderBy).ToList();
            var menuparent = menuItems.Where(m => m.ParentId == 0).ToList();
            model.MenuItems = new List<MenuModel>();
            if (menuparent.Any())
            {
                model.MenuItems = menuparent.Select(c => c.ToModel()).ToList();
                foreach (var item in model.MenuItems)
                {
                    var childItems= menuItems.Where(m => m.ParentId == item.Id).Select(c => new ItemModel()
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
                            child.ChildItems= menuItems.Where(m => m.ParentId == child.Id).Select(c => new SubItemModel()
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
                
            return PartialView("_MenuTopPartial", model);
        }


        public ActionResult PostTop(string type)
        {
            List<int> cateIds = new List<int>();
            List<string> postTypes = new List<string>();
            string tagType = TagConst.TAGPOST;
            if (type.Equals(CategoryConst.CATEGORYPRODUCT, StringComparison.OrdinalIgnoreCase))
            {
                postTypes.Add(PostTypeConst.TOUR);
                tagType= TagConst.TAGTOUR;
            }
            else
            {
                postTypes.Add(PostTypeConst.POST);
                postTypes.Add(PostTypeConst.BLOG);
                postTypes.Add(PostTypeConst.HOT);
                postTypes.Add(PostTypeConst.PROMOTION);
            }
            var posts = this._postService.GetAll("", cateIds, StatusConst.PUBLISHNAME, null, null, postTypes, "CREATEDATE", "DESC", 1, 10);
            model.PostItems = new List<PostModel>();
            if (posts.Any())
            {
                model.PostItems = posts.Select(c => c.ToModel()).ToList();
                foreach (var item in model.PostItems)
                {
                    item.Content = string.Empty;
                }
            }

            //tag
            var tags = this._tagsService.FindAllBy(c => c.TagType == tagType).OrderBy(c => Guid.NewGuid()).Take(3).ToList();
            model.TagItems= new List<TagModel>();
            if (tags.Any())
                model.TagItems = tags.Select(c => c.ToModel()).ToList();
            model.Type = type;
            model.LableInfo = new LableModel();
            if (type.Equals(CategoryConst.CATEGORYPRODUCT, StringComparison.OrdinalIgnoreCase))
                model.LableInfo.SidebarPostNew = base.GetLableConst(LableConst.SIDEBAR_TOURNEW, Language);
            else
                model.LableInfo.SidebarPostNew = base.GetLableConst(LableConst.SIDEBAR_POSTNEW, Language);
            return PartialView("_PostTopPartial", model);
        }

        public ActionResult Footer()
        {
            model.ParameterInfo = new ParametersModel();
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
                ParameterConst.FOOTER
            };
            var parameterItems = this._parameterService.GetAll(values);
            if (parameterItems != null && parameterItems.Count > 0)
            {
                model.ParameterInfo.MetaTitle = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.METATITLE).Content;
                model.ParameterInfo.MetaKeyword = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.METAKEYWORD).Content;
                model.ParameterInfo.MetaDescription = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.METADESCRIPTION).Content;
                model.ParameterInfo.Phone = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.PHONE).Content;
                model.ParameterInfo.Email = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.EMAIL).Content;
                model.ParameterInfo.Address = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.ADDRESS).Content;
                model.ParameterInfo.Youtube = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.YOUTUBE).Content;
                model.ParameterInfo.Facebook = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.FACEBOOK).Content;
                model.ParameterInfo.GooglePlus = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.GOOGLEPLUS).Content;
                model.ParameterInfo.Twitter = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.TWITTER).Content;
                model.ParameterInfo.CompanyName = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.COMPANYNAME).Content;
                model.ParameterInfo.Gmap = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.GMAP).Content;
                model.ParameterInfo.About = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.ABOUT).Content;
                model.ParameterInfo.Footer = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.FOOTER).Content;
            }
            model.LableInfo = new LableModel();
            model.LableInfo.NewsLetter = base.GetLableConst(LableConst.NEWS_LETTER, Language);
            return PartialView("_FooterPartial", model);
        }

        public ActionResult GetLable(string id)
        {
            return Content(base.GetLableConst(id, Language));
        }


        public ActionResult Support()
        {
            model.ParameterInfo = new ParametersModel();
            List<string> values = new List<string>
            {
                ParameterConst.PHONE,
                ParameterConst.EMAIL,
                ParameterConst.IMGMAP
            };
            var parameterItems = this._parameterService.GetAll(values);
            if (parameterItems != null && parameterItems.Count > 0)
            {
                model.ParameterInfo.Phone = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.PHONE).Content;
                model.ParameterInfo.Email = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.EMAIL).Content;                
                model.ParameterInfo.ImgMap = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.IMGMAP).Content;
            }           
            return PartialView("_SupportPartial", model);
        }

        public ActionResult FanPageFb()
        {
            model.ParameterInfo = new ParametersModel();
            List<string> values = new List<string>
            {
                ParameterConst.FACEBOOK
            };
            var parameterItems = this._parameterService.GetAll(values);
            if (parameterItems != null && parameterItems.Count > 0)
            {
                model.ParameterInfo.Facebook = parameterItems.FirstOrDefault(m => m.Value == ParameterConst.FACEBOOK).Content;
            }            
            return PartialView("_FanPageFbPartial", model);
        }
    }
}