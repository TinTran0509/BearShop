using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        ICategoryRepository categoryRepository = new CategoryRepository();
        // GET: Menu
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListData(int page)
        {
            var model = categoryRepository.GetAll().OrderBy(x=>x.Ordering).ToList();
            foreach (var item in model)
            {
                var objParent = model.FirstOrDefault(g => g.ID == item.ParentID);
                if (objParent != null)
                {
                    item.ParentName = objParent.Name;
                }
            }
            var lstLevel = Common.CreateLevel(model.ToList());
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Category/_ListData.cshtml", lstLevel)
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var model = categoryRepository.GetAll().OrderBy(x => x.Ordering).ToList();
            TempData["Categories"] = model;
            return Json(RenderViewToString("~/Areas/Admin/Views/Category/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Category model)
        {
            try
            {
                var obj = categoryRepository.GetAll().FirstOrDefault(x => x.Name.Trim() == model.Name.Trim());
                if (obj != null)
                {
                    return Json(new { IsSuccess = false, Messenger = "Tên danh mục đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }
                categoryRepository.Add(model);
                return Json(new { IsSuccess = true, Messenger = "Thêm mới thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Messenger = "Thêm mới thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = categoryRepository.GetAll().OrderBy(x => x.Ordering).ToList();
            TempData["Categories"] = model;
            var obj = categoryRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/Category/_Edit.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Category model)
        {
            try
            {
                var obj = categoryRepository.GetAll().FirstOrDefault(x => x.Name.Trim() == model.Name.Trim() && x.ID != model.ID);
                if (obj != null)
                {
                    return Json(new { IsSuccess = false, Messenger = "Tên danh mục đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }
                model.LinkSeo = HelperString.RenderLinkSeo(model.Name);
                categoryRepository.Edit(model);
                return Json(new { IsSuccess = true, Messenger = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Messenger = "Cập nhật thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult UpdatePosition(string value)
        {
            var arrValue = value.Split('|');
            foreach (var item in arrValue)
            {
                var id = item.Split(':')[0];
                var pos = item.Split(':')[1];
                var obj = categoryRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    categoryRepository.Edit(obj);

                }
                catch (Exception)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Cập nhật vị trí thất bại")
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Cập nhật vị trí thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                categoryRepository.Delete(id);
                return Json(new { IsSuccess = true, Messenger = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Messenger = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult DeleteAll(string lstid)
        {
            var arrid = lstid.Split(',');
            var count = 0;
           
            foreach (var item in arrid)
            {
                try
                {
                    categoryRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} danh mục", count),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
