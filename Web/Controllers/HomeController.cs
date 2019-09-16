using CMS.IRepository;
using CMS.Reporitory;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.BaseSecurity;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        ISlideIRepository slideRepository = new SlideRepository();
        IHomeMenuRepository menuHomeRepository = new HomeMenuRepository();
        INewsRepository newsRepository = new NewsRepository();
        ILogoRepository logoRepository = new LogoRepository();
        IProductRepository productRepository = new ProductRepository();
        ICategoryRepository categoryRepository = new CategoryRepository();
        public ActionResult Index()
        {
            var cate = categoryRepository.GetAll().FirstOrDefault(x => x.LinkSeo == "gau-bong-teddy");
            var lst = productRepository.GetAll().Where(x => x.CategoryId == cate.ID).ToList();
            return View(lst);
        }
      
       
        public PartialViewResult Header()
        {
            return PartialView();
        }
        public PartialViewResult MenuHome()
        {
            var menu = menuHomeRepository.GetAll().ToList();
            return PartialView(menu);

        }
        public PartialViewResult Slider()
        {
            var slider = slideRepository.GetAll().ToList();
            return PartialView(slider);
        }
        public PartialViewResult NewsHome()
        {
            var lstNews = newsRepository.GetAll().Where(x=>x.Status==2).OrderBy(x=>x.CreatedDate).Take(3).ToList();
            return PartialView(lstNews);
        }
        public  PartialViewResult PathWay()
        {
            string  path= string.Empty;
            var linkSeo = HttpContext.Request.FilePath;
            var arrLink = linkSeo.Split('/');
            var link = arrLink[1];
            if(link== "danh-muc-san-pham")
            {
                path  = "Sản phẩm";
                ViewBag.LinkPath = "san-pham.html";
            }
            else if (link == "danh-muc-tin-tuc")
            {
                path = "Tin tức";
                ViewBag.LinkPath = "tin-tuc.html";
            }
            ViewBag.Path = path;
            return PartialView();
        }
        public PartialViewResult Logo()
        {
            var logo = logoRepository.GetAll().FirstOrDefault(x => x.Status == true && x.Type==1);
            return PartialView(logo);
        }
        
    }
}