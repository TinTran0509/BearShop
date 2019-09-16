using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        IOrderRepository orderRepository = new OrderRepository();
        // GET: Order
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListData(int status,string name,string tungay,string denngay, int page)
        {
            var model = orderRepository.GetList(status, name, tungay, denngay);
            var totalAdv = model.Count();
            model = model.Skip((page - 1) * 10).Take(10).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Order/_ListData.cshtml", model),
                totalPages = Math.Ceiling(((double)totalAdv / 10)),
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(int id)
        {
            var order = orderRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/Order/Edit.cshtml", order), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Edit(tbl_Order obj)
        {
            try
            {
                orderRepository.Edit(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Cập nhật thất bại"
                }, JsonRequestBehavior.AllowGet);
            }
           
        }
        [HttpPost]
        public ActionResult Delete(int  id)
        {
            try
            {
                orderRepository.Delete(id);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Xóa thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Xóa thất bại"
                }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Detail(int id)
        {
            var customer = orderRepository.Find(id);
            TempData["Customer"] = customer;
            var lstOrderDetail = orderRepository.OrderDetailGetList(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/Order/Detail.cshtml", lstOrderDetail), JsonRequestBehavior.AllowGet);
        }
    }
}
