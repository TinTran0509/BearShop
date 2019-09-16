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
    public class AdvImageController : BaseController
    {
        IAdvImageRepository adv_ImageRepository = new AdvImageRepository();

        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListData(int pageIndex)
        {
            var model = adv_ImageRepository.GetAll();
            var totalAdv = model.Count();
            model = model.Skip((pageIndex - 1) * 10).Take(10).OrderBy(x => x.DisplayOrder).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/AdvImage/ListData.cshtml", model),
                totalPages = Math.Ceiling(((double)totalAdv / 10)),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            return Json(RenderViewToString("~/Areas/Admin/Views/AdvImage/Add.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(AdvImag model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.Title))
                {
                    var obj = adv_ImageRepository.GetAll().FirstOrDefault(x => x.Title == model.Title);
                    if (obj != null)
                        return Json(new { IsSuccess = false, Message = "Tiêu đề đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }
                model.CreatedDate = DateTime.Now;
                model.Type = 1;
                adv_ImageRepository.Add(model);
                return Json(new { IsSuccess = true, Message = "Thêm mới thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, Message = "Thêm mới thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var obj = adv_ImageRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/AdvImage/Edit.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(AdvImag model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.Title))
                {
                    var obj = adv_ImageRepository.GetAll().FirstOrDefault(x => x.Title == model.Title && x.ID != model.ID);
                    if (obj != null)
                        return Json(new { IsSuccess = false, Message = "Tiêu đề đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }
                model.Type = 1;
                adv_ImageRepository.Edit(model);
                return Json(new { IsSuccess = true, Message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Cập nhật thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                adv_ImageRepository.Delete(id);
                return Json(new { IsSuccess = true, Message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
