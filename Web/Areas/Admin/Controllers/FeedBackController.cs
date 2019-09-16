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
    public class FeedBackController : BaseController
    {
        IFeedBackReporitory feedBackReporitory = new FeedBackReporitory();
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListData(int page)
        {
            var model = feedBackReporitory.GetAll();
            var totalAdv = model.Count();
            model = model.Skip((page - 1) * 10).Take(10).OrderByDescending(x=>x.CreatedDate).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/FeedBack/ListData.cshtml", model),
                totalPages = Math.Ceiling(((double)totalAdv / 10)),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        public ActionResult Reply(int id)
        {
            try
            {
                var obj = feedBackReporitory.Find(id);
                obj.Status = !obj.Status; 
                feedBackReporitory.Reply(obj);
                return Json(new { IsSuccess = true, Message = "Xác nhận thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Aác nhận thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                feedBackReporitory.Delete(id);
                return Json(new { IsSuccess = true, Message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Detail(int id)
        {
            var obj = feedBackReporitory.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/FeedBack/Detail.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
    }
}
