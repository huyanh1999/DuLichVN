using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DC.Webs.Common;

namespace DC.Webs
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //cate product
            routes.MapRoute(
                name: "cateproduct",
                url: "danh-muc/{slugUrl}",
                defaults: new { controller = "Products", action = "Index", slugUrl = UrlParameter.Optional },
                namespaces: new[] { "DC.Webs.Controllers" }
            );

            //san-pham
            routes.MapRoute(
                name: "listproduct",
                url: "san-pham",
                defaults: new { controller = "Products", action = "Index", slugUrl = "" },
                namespaces: new[] { "DC.Webs.Controllers" }
            );

            routes.MapRoute(
                name: "productdetail",
                url: "san-pham/{slugUrl}",
                defaults: new { controller = "Products", action = "Detail", slugUrl = UrlParameter.Optional },
                namespaces: new[] { "DC.Webs.Controllers" }
            );

            //cate post
            routes.MapRoute(
                name: "catepost",
                url: "tin-tuc/{slugUrl}",
                defaults: new { controller = "News", action = "Index", slugUrl = UrlParameter.Optional },
                namespaces: new[] { "DC.Webs.Controllers" }
            );

            routes.MapRoute(
                name: "postdetail",
                url: "post/{slugUrl}",
                defaults: new { controller = "News", action = "Index", slugUrl = UrlParameter.Optional },
                namespaces: new[] { "DC.Webs.Controllers" }
            );

            routes.MapRoute(
                name: "tourdetail",
                url: "tour/{slugUrl}",
                defaults: new { controller = "Tours", action = "Index", slugUrl = UrlParameter.Optional },
                namespaces: new[] { "DC.Webs.Controllers" }
            );

            routes.MapRoute(
                name: "tagspots",
                url: "tag/{slugUrl}",
                defaults: new { controller = "Tag", action = "Index", slugUrl = UrlParameter.Optional, type = TagConst.TAGPOST },
                namespaces: new[] { "DC.Webs.Controllers" }
            );

            routes.MapRoute(
                name: "tagtour",
                url: "tags/{slugUrl}",
                defaults: new { controller = "Tag", action = "Index", slugUrl = UrlParameter.Optional,type=TagConst.TAGTOUR },
                namespaces: new[] { "DC.Webs.Controllers" }
            );

            routes.MapRoute(
                name: "contact",
                url: "contact",
                defaults: new { controller = "Home", action = "Contact" },
                namespaces: new[] { "DC.Webs.Controllers" }
            );

            //checkout
            routes.MapRoute(
                name: "checkout",
                url: "checkout",
                defaults: new { controller = "Cart", action = "CheckOut" },
                namespaces: new[] { "DC.Webs.Controllers" }
            );

            //category
            routes.MapRoute(
                name: "category",
                url: "category/{slugUrl}",
                defaults: new { controller = "Category", action = "Index" },
                namespaces: new[] { "DC.Webs.Controllers" }
            );

            //sitemap
            routes.MapRoute(
                name: "sitemap",
                url: "site-map",
                defaults: new { controller = "Home", action = "SiteMap" },
                namespaces: new[] { "DC.Webs.Controllers" }
            );
            //
            routes.MapRoute(
                name: "DefaultUrl",
                url: ConfigurationManager.AppSettings["UrlIndex"],
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional,slugUrl= "vietnam-motorbike-tours" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
