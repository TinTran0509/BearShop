using CMS.IRepository;
using CMS.Reporitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using Web.BaseSecurity;
using System.Xml.Linq;

namespace Web.Controllers
{
    public class ProductController : BaseController
    {
        private IProductRepository productRepository = new ProductRepository();
       
        // GET: Product
        public ActionResult Index(string linkseo)
        {
            return View();
        }
        public ActionResult ListData(string keyWord,string code, int pageIndex)
        {
            var model = productRepository.ListAll(0,keyWord).ToList();
            var totalAdv = model.Count();
            model = model.Skip((pageIndex - 1) * 18).Take(18).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Views/Product/ListData.cshtml", model),
                totalPages = Math.Ceiling(((double)totalAdv / 18)),
            }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult HotProduct()
        {
            var lst = productRepository.GetAll().Where(x => x.IsHot == true).ToList();
            return PartialView(lst);
        }
        
        public ActionResult Hot()
        {
            return View();
        }
      
        public ActionResult ListSaleHot(int pageIndex)
        {
            var model = productRepository.GetAll().Where(x => x.IsHot == true).ToList();
            var totalAdv = model.Count();
            model = model.Skip((pageIndex - 1) * 18).Take(18).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Views/Product/ListHot.cshtml", model),
                totalPages = Math.Ceiling(((double)totalAdv / 18)),
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int id)
        {
            var product = productRepository.Find(id);
            var lstProduct = productRepository.GetAll().Where(x => x.CategoryId == product.CategoryId && x.ID != product.ID).ToList();
            ViewBag.RelatedProduct = lstProduct;
            //Load nhiều ảnh hơn
            if(product.ImageMore != null)
            {
                var imageMore = product.ImageMore;
                XElement xImageMore = XElement.Parse(imageMore);
                List<string> lstImageMore = new List<string>();
                foreach (var item in xImageMore.Elements())
                {
                    lstImageMore.Add(item.Value);
                }
                ViewBag.ImageMore = lstImageMore;
            }
            return View(product);
        }
    }
}