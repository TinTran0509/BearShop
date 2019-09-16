using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Web.BaseSecurity;
using Web.Controllers;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;
using Web.Model;

namespace Web.Areas.Admin.Controllers
{
    public class CartController : BaseController
    {
        ICartRepository cartRepository = new CartRepository();

        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListData(int pageIndex)
        {
            var model = cartRepository.GetAll();
            var totalAdv = model.Count();
            model = model.Skip((pageIndex - 1) * 10).Take(10).OrderByDescending(x=>x.CreatedDate).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Cart/ListData.cshtml", model),
                totalPages = Math.Ceiling(((double)totalAdv / 10)),
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
