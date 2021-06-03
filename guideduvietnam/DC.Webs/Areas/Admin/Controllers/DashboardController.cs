using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DC.Webs.Models;

namespace DC.Webs.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        BaseViewModel model = new BaseViewModel();


        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Index()
        {
            model.TabName = "Dashboard";
            //model.ActionName = "Home";
            SetupBaseModel(model);

            // Set model
            return View(model);
        }
    }
}