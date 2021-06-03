using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DC.Common.Utility;
using DC.Models.Logging;
using DC.Services.Authorize;
using DC.Services.Logging;
using DC.Webs.Common;
using DC.Webs.Models;

namespace DC.Webs.Areas.Admin.Controllers
{
    public class ActivitiesController : BaseController
    {
        ActivityLogViewModel model = new ActivityLogViewModel();
        private readonly IActivityLogService _activityLogService;
        private readonly IActivityLogTypeService _activityLogTypeService;
        private readonly IUserService _userService;


        //Ctor
        public ActivitiesController(IUserService userService, IActivityLogService activityLogService, IActivityLogTypeService activityLogTypeService)
        {
            this._userService = userService;
            this._activityLogService = activityLogService;
            this._activityLogTypeService = activityLogTypeService;
        }



        // GET: Admin/ActivityList
        [AreaAuthorizeAttribute("Admin")]
        public ActionResult Index()
        {
            // BaseViewModel
            model.TabName = "Activities";
            model.ActionName = "Activities";
            SetupBaseModel(model);

            // Set model
            model.ActivityLogTypeOptions = BuildActivityLogTypeOptions();
            if (model.ActivityLogTypeOptions != null)
                model.ActivityLogTypeOptions.Insert(0, new SelectItemModel("Chọn hành động...", "0"));
            else
                model.ActivityLogTypeOptions.Add(new SelectItemModel("Chọn hành động...", "0"));


            return View(model);
        }

        /// <summary>
        /// Get danh sách log
        /// </summary>
        /// <param name="query"></param>
        /// <param name="logeTypeId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="sortBy"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [AreaAuthorizeAttribute("Admin")]
        [HttpPost]
        public ActionResult GetList(string query, int logeTypeId, string from, string to, string sortBy, string orderBy, int pageIndex, int pageSize)
        {
            //int userId = CurrentUserLoggedIn.Id;

            // Set params values
            DateTime? fromDate = DateTimeTools.CFToDateTime(from);
            DateTime? toDate = DateTimeTools.CFToDateTime(to);

            // Set model
            var users = this._activityLogService.GetAll(query, logeTypeId, fromDate, toDate, sortBy, orderBy, pageIndex, pageSize);
            if (users != null && users.Count > 0)
            {
                model.TotalCount = users.TotalCount;
                model.TotalPages = users.TotalPages;
                model.ActivityLogItems = users.Select(c => c.ToModel()).ToList();
                foreach (var item in model.ActivityLogItems)
                {
                    try
                    {
                        item.ActivityLogTypeName =
                        this._activityLogTypeService.GetSingle(item.ActivityLogTypeId).Description;

                        var userObj = this._userService.Find((int)item.UserId);
                        if (userObj != null)
                        {
                            item.FullName = userObj.FirstName + " " + userObj.LastName;
                            item.UserName = userObj.UserName;
                        }
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                model.TotalPages = 0;
                model.TotalCount = 0;
                model.ActivityLogItems = new List<ActivityLogModel>();
            }

            // Return to Json data
            return Json(model);
        }


        #region #Options
        private List<SelectItemModel> BuildActivityLogTypeOptions()
        {
            List<SelectItemModel> list = new List<SelectItemModel>();
            var typeOptions = this._activityLogTypeService.GetAll();
            if (typeOptions != null)
            {
                SelectItemModel type = null;
                foreach (var item in typeOptions)
                {
                    type = new SelectItemModel();
                    type.Value = item.Id.ToString();
                    type.Text = item.Description;
                    list.Add(type);
                }
            }

            return list;
        }
        #endregion
    }
}