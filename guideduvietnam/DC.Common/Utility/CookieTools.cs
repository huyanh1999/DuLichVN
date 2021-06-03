using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DC.Common.Utility
{
    public static class CookieTools
    {
        public static void SetCookie(string cookieName, string strValue)
        {
            if (System.Web.HttpContext.Current.Request.Cookies[cookieName] == null)
            {
                HttpCookie cookie = new HttpCookie(cookieName);
                cookie.Value = strValue;
                cookie.Expires = DateTime.Now.AddDays(7);
                cookie.Path = "/";
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[cookieName];
                cookie.Value = strValue;
                cookie.Expires = DateTime.Now.AddDays(7);
                cookie.Path = "/";
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }


        public static string GetCookie(string cookieName)
        {
            if (System.Web.HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                return HttpContext.Current.Request.Cookies[cookieName].Value;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
