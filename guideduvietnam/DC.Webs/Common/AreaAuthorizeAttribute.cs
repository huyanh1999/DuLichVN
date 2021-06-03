using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DC.Webs
{
    public class AreaAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string area;

        public AreaAuthorizeAttribute(string area)
        {
            this.area = area;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            string loginUrl = "";

            if (area == "Admin")
            {
                loginUrl = "~/Admin/Login";
            }
            else if (area == "Members")
            {
                loginUrl = "~/Members/Login";
            }

            filterContext.Result = new RedirectResult(loginUrl + "?returnUrl=" + filterContext.HttpContext.Request.Url.PathAndQuery);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string cookieName = FormsAuthentication.FormsCookieName;

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated ||
                filterContext.HttpContext.Request.Cookies == null ||
                filterContext.HttpContext.Request.Cookies[cookieName] == null
            )
            {
                HandleUnauthorizedRequest(filterContext);
                return;
            }

            var authCookie = filterContext.HttpContext.Request.Cookies[cookieName];
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            string[] roles = authTicket.UserData.Split(',');

            var userIdentity = new GenericIdentity(authTicket.Name);
            var userPrincipal = new GenericPrincipal(userIdentity, roles);

            filterContext.HttpContext.User = userPrincipal;
            base.OnAuthorization(filterContext);
        }
    }
}