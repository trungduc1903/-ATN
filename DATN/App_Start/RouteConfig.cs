using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopDGHouse
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Products",
                url: "danh-muc-san-pham/{id}",
                defaults: new { controller = "Products", action = "GetSanPham", id = UrlParameter.Optional },
                namespaces: new[] { "ShopDGHouse.Controllers" }
            );

            routes.MapRoute(
                name: "ProductDeltail",
                url: "chi-tiet/{id}",
                defaults: new { controller = "Products", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "ShopDGHouse.Controllers" }
            );

            routes.MapRoute(
                name: "Register",
                url: "dang-ky",
                defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
                namespaces: new[] { "ShopDGHouse.Controllers" }
            );
            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "ShopDGHouse.Controllers" }
            );

            routes.MapRoute(
                name: "Profile",
                url: "profileUser/{id}",
                defaults: new { controller = "Account", action = "ProfileUser", id = UrlParameter.Optional },
                namespaces: new[] { "ShopDGHouse.Controllers" }
            );

            routes.MapRoute(
                name: "ChangePass",
                url: "changepass/{id}",
                defaults: new { controller = "Account", action = "ChangePass", id = UrlParameter.Optional },
                namespaces: new[] { "ShopDGHouse.Controllers" }
            );

            routes.MapRoute(
                name: "ChitetBrands",
                url: "chi-tiet-brands/{id}",
                defaults: new { controller = "Brands", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "ShopDGHouse.Controllers" }
            );

            routes.MapRoute(
                name: "Cart",
                url: "gio-hang",
                defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "ShopDGHouse.Controllers" }
            );
            routes.MapRoute(
                name: "CheckOut",
                url: "thanh-toan",
                defaults: new { controller = "Cart", action = "CheckOut", id = UrlParameter.Optional },
                namespaces: new[] { "ShopDGHouse.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "ShopDGHouse.Controllers" }
            );
        }
    }
}
