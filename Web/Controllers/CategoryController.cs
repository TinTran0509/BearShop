using CMS.IRepository;
using CMS.Reporitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class CategoryController : BaseController
    {
        private IProductRepository productRepository = new ProductRepository();
        ICategoryRepository categoryRepository = new CategoryRepository();
        // GET: Category
        public ActionResult LoadCategory()
        {
            var categories = categoryRepository.GetAll().ToList();
            return PartialView(categories);
        }
        public ActionResult ProductList(string linkseo)
        {
            ViewBag.LinkSeo = linkseo;
            return View();
        }
        public ActionResult LoadData(string linkseo,int pageIndex)
        {
            string name = string.Empty;
            var category  = categoryRepository.GetAll().FirstOrDefault(x => x.LinkSeo == linkseo);
            if (category != null)
            {
                name = category.Name;
            }
            var lstProduct = productRepository.ProductGetByCategory(linkseo);
            var totalAdv = lstProduct.Count();
            lstProduct = lstProduct.Skip((pageIndex - 1) * 18).Take(18).ToList();
            return Json(new
            {
                categoryName = name,
                viewContent = RenderViewToString("~/Views/Category/ListData.cshtml", lstProduct),
                totalPages = Math.Ceiling(((double)totalAdv / 18)),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}