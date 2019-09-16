using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("sanpham", "san-pham.html", new { controller = "Product", action = "Index" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("gioithieu", "ve-chung-toi/{linkseo}.html", new { controller = "About", action = "Detail", linkseo = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("chinhsach", "chinh-sach/{linkseo}.html", new { controller = "Policy", action = "Detail", linkseo = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("lienhe", "lien-he.html", new { controller = "Contact", action = "Index" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("AddCart", "dat-mua-hang", new { controller = "Cart", action = "AddProduct" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("Giohang", "gio-hang.html", new { controller = "Cart", action = "Index" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("Tintuc", "tin-tuc-tong-hop.html", new { controller = "News", action = "Index" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("chuyencuagau", "chuyen-cua-gau.html", new { controller = "ChuyenCuaGau", action = "Index" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("home", "trang-chu.html", new { controller = "Home", action = "Index" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("video", "video-san-pham.html", new { controller = "Video", action = "Index" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("product", "san-pham/{linkseo}.html", new { controller = "Product", action = "Index" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("InfoDetail", "thong-tin-huu-ich/{id}/{metatitle}.html", new { controller = "Footer", action = "Detail", id = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("NewDetail", "chi-tiet-tin-tuc/{id}/{metatitle}.html", new { controller = "News", action = "Detail", id = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("ProductDetail", "chi-tiet-san-pham/{id}/{name}.html", new { controller = "Product", action = "Detail", id = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("ProductCategory", "danh-muc-san-pham/{linkseo}.html", new { controller = "Category", action = "ProductList", linkseo = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("NewsCategory", "danh-muc-tin-tuc/{linkseo}.html", new { controller = "News", action = "NewsList", linkseo = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("huong-dan", "huong-dan-mua-hang.html", new { controller = "Instruction", action = "Index" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("video_detail", "pages/video/{id}/{title}.html", new { controller = "video", action = "Index", id = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("dangnhap", "dang-nhap.html", new { controller = "Login", action = "Index", id = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("thoat", "thoat.html", new { controller = "Login", action = "LogOut", id = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("trangcanhan", "trang-ca-nhan.html", new { controller = "User", action = "Profiled", id = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Web.Controllers" }
            );
        }
    }
}