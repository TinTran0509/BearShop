using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class PolicyController : BaseController
    {
        IPolicyRepository policyRepository = new PolicyRepository();
        // GET: Menu
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListData(int pageIndex, int pageSize)
        {
            var model = policyRepository.GetAll();
            var totalAdv = model.Count();
            model = model.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Policy/_ListData.cshtml", model),
                totalPages = Math.Ceiling(((double)totalAdv / pageSize)),
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Policy model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Contents))
                {
                    return Json(new { IsSuccess = false , Message = "Vui lòng thêm nội dung" }, JsonRequestBehavior.AllowGet);
                }
                model.Createddate = DateTime.Now;
                model.LinkSeo = HelperString.RenderLinkSeo(model.MetaTitle);
                policyRepository.Add(model);
                return Json(new { IsSuccess = true, Message = "Thêm mới thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Thêm mới thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = policyRepository.Find(id);
            return View(obj);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Policy model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Contents))
                {
                    return Json(new { IsSuccess = false, Message = "Vui lòng thêm nội dung" }, JsonRequestBehavior.AllowGet);
                }
                model.LinkSeo = HelperString.RenderLinkSeo(model.MetaTitle);
                policyRepository.Edit(model);
                return Json(new { IsSuccess = true, Message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Cập nhật thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                policyRepository.Delete(id);
                return Json(new { IsSuccess = true, Message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Detail(int id)
        {
            var obj = policyRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/Policy/Detail.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
    }
}
