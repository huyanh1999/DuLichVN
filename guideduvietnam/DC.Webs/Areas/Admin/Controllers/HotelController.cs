using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DC.Models.Cms;
using DC.Services.Cms;
using DC.Webs.Common;
using DC.Webs.Models;

namespace DC.Webs.Areas.Admin.Controllers
{
    public class HotelController : BaseController
    {
        HotelViewModel model = new HotelViewModel();
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            this._hotelService = hotelService;
        }

        // GET: Admin/Hotel
        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Index()
        {
            SetupBaseModel(model);
            model.HotelInfo=new HotelModel();
            return View(model);
        }


        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        public ActionResult GetList(string query)
        {
            model.HotelItems= new List<HotelModel>();
            var items = this._hotelService.GetAll().OrderBy(m => m.OrderBy).ToList();
            if (!string.IsNullOrEmpty(query))
            {
                items = items.Where(m => m.Name.Contains(query)).OrderBy(m => m.OrderBy).ToList();
            }            
            if (items.Any())
                model.HotelItems = items.Select(c => c.ToModel()).ToList();
            return Json(model);
        }
        

        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        public ActionResult Update(HotelModel item)
        {
            try
            {
                if (item.Id > 0)
                {
                    this._hotelService.Edit(item.ToEntity());
                    this._hotelService.Save();
                }
                else
                {
                    this._hotelService.Add(item.ToEntity());
                    this._hotelService.Save();
                }
            }
            catch
            {
                                
            }
            
            return Json(item.Id);
        }

        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = this._hotelService.Find(id);
                if (obj != null)
                {
                    this._hotelService.Delete(obj);
                    this._hotelService.Save();
                }
            }
            catch
            {
                                
            }
            return Json(string.Empty);
        }
    }
}