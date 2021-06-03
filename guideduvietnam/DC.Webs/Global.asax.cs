using DC.Webs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DC.Webs
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperStartupTask.Configure();

            UnityConfig.RegisterComponents();
        }

        /// <summary>
        /// Khởi tạo phiên làm việc mới
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
            Session[SessionConst.SESSION_START] = Guid.NewGuid().ToString();
        }


        /// <summary>
        /// Tắt phiên làm việc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End(object sender, EventArgs e)
        {
            Session.Remove(SessionConst.SESSION_START);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.Url.Host.StartsWith("www") && !Request.Url.IsLoopback)
            {
                UriBuilder builder = new UriBuilder(Request.Url);
                builder.Host = Request.Url.Host.Replace("www.", "");
                Response.StatusCode = 301;
                Response.AddHeader("Location", builder.ToString());
                Response.End();
            }
        }
    }
}
