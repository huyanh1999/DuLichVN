using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DC.Webs.Common;
using DC.Webs.Models;
using DC.Services.Media;
using DC.Models.Media;

namespace DC.Webs.Areas.Admin.Controllers
{
    public class SliderController : BaseController
    {
        SliderViewModel model = new SliderViewModel();
        private readonly ISliderService _sliderService;


        //Ctor
        public SliderController(ISliderService sliderService)
        {
            this._sliderService = sliderService;
        }


        #region #Index
        // GET: Slider

        [AreaAuthorizeAttribute("Admin", Roles = "SuperAdministrator,Administrator,Admin")]
        public ActionResult Index()
        {
            model.TabName = "Slider";
            SetupBaseModel(model);
            var slider = this._sliderService.GetAll().ToList();
            if (slider != null && slider.Count() > 0)
            {
                model.SliderItems = slider.Select(c => c.ToModel()).ToList();
            }
            return View(model);
        }
        #endregion

        #region #Add
        /// <summary>
        /// Thêm slider
        /// </summary>
        /// <returns></returns>
        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Add()
        {
            model.TabName = "Slider";
            SetupBaseModel(model);
            return View(model);
        }

        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(FormCollection collection)
        {
            try
            {
                bool maxsize = true;
                string picture = AddFiles(SliderImageUrl, ref maxsize);
                if (!String.IsNullOrEmpty(picture))
                {
                    var slidemodel = new SlideModel
                    {
                        Name = collection["Name"]?.ToString() ?? string.Empty,
                        Url = collection["Url"]?.ToString() ?? string.Empty,
                        Images = picture,
                        Description = collection["Description"]?.ToString() ?? string.Empty,
                        OrderBy = collection["OrderBy"] != null ? int.Parse(collection["OrderBy"]) : 1,                        
                    };
                    this._sliderService.Add(slidemodel.ToEntity());
                    this._sliderService.Save();
                    TempData["MessageSuccess"] = "Thêm mới slide ảnh thành công!";

                    // Lưu hành động                    
                    string comment = string.Format("Thêm mới slide ảnh : {0} ", slidemodel.Name);
                    AddActivityLog(LogTypeConst.INSERT, comment);

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["MessageError"] = "Vui lòng chọn ảnh đúng định dạng";
                }
            }
            catch (Exception ex)
            {
                TempData["MessageError"] = ex.ToString();
            }

            model.TabName = "Slider";
            //model.ActionName = "Home"; 
            SetupBaseModel(model);
            return View(model);
        }
        #endregion


        #region #Edit
        /// <summary>
        /// Cập nhật ảnh silde
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Edit(int id)
        {
            model.TabName = "Slider";
            SetupBaseModel(model);
            var slideObj = this._sliderService.Find(id);
            if (slideObj != null)
            {
                model.SliderInfo = slideObj.ToModel();
            }
            else
                return RedirectToAction("Index");
            return View(model);
        }

        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                bool maxsize = true;
                string picture = AddFiles(SliderImageUrl, ref maxsize);
                var sliderObj = this._sliderService.Find(id);
                if (sliderObj != null)
                {
                    sliderObj.Name = collection["Name"]?.ToString() ?? sliderObj.Name;
                    sliderObj.Url = collection["Url"]?.ToString() ?? sliderObj.Url;
                    if (!String.IsNullOrEmpty(picture))
                        sliderObj.Images = picture;
                    sliderObj.Description = collection["Description"]?.ToString() ?? sliderObj.Name;
                    sliderObj.OrderBy = collection["OrderBy"] != null
                        ? int.Parse(collection["OrderBy"])
                        : sliderObj.OrderBy;                    

                    this._sliderService.Edit(sliderObj);
                    this._sliderService.Save();
                    TempData["MessageSuccess"] = "Cập nhật slide ảnh thành công!";

                    // Lưu hành động                    
                    string comment = string.Format("Cập nhật slide ảnh : ID({0}) - {1} ", sliderObj.Id, sliderObj.Name);
                    AddActivityLog(LogTypeConst.UPDATE, comment);
                }
                else
                {
                    TempData["MessageError"] = "Không tồn tại slide ảnh trên";
                }
            }
            catch 
            {
                TempData["MessageError"] = "Error!";
            }
            return RedirectToAction("Index");
        }
        #endregion


        #region #Delete

        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                var sliderObj = this._sliderService.Find(id);

                this._sliderService.Delete(sliderObj);
				this._sliderService.Save();
                TempData["MessageSuccess"] = "Xóa slide ảnh thành công!";

                // Lưu hành động                
                string comment = string.Format("Xóa slide ảnh : ID({0}) - {1} ", sliderObj.Id, sliderObj.Name);
                AddActivityLog(LogTypeConst.DELETE, comment);
            }
            catch
            {
                TempData["MessageError"] = "Error!";
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}