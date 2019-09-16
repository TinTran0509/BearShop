using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Controllers;
using Web.Core;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class LogoController : BaseController
    {
        readonly ILogoRepository logoRepository = new LogoRepository();
        const string Keycache = "keyadminmenu";
        //
        // GET: /AdminMenu/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Index")]
        public ActionResult ListData()
        {
            var lstLogo = logoRepository.GetAll();         
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Logo/_ListData.cshtml", lstLogo),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            return Json(RenderViewToString("~/Areas/Admin/Views/Logo/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(Logo obj)
        {
            try
            {
                if (string.IsNullOrEmpty(obj.Image))
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Vui lòng thêm ảnh",
                    }, JsonRequestBehavior.AllowGet);
                }
                var logo = logoRepository.GetAll().FirstOrDefault(x => x.Name.Trim() == obj.Name.Trim());
                if (logo != null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Tên logo đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                }
                logoRepository.Add(obj);
                HelperCache.RemoveCache(Keycache);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var logo = logoRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/Logo/_Edit.cshtml", logo), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(Logo obj)
        {
            try
            {
                if (string.IsNullOrEmpty(obj.Image))
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Vui lòng thêm ảnh",
                    }, JsonRequestBehavior.AllowGet);
                }
                var logo = logoRepository.GetAll().FirstOrDefault(x => x.Name.Trim() == obj.Name.Trim() && x.ID != obj.ID);
                if (logo != null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Tên logo đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                }
                logoRepository.Edit(obj);
                
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                logoRepository.Delete(id);
                HelperCache.RemoveCache(Keycache);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Xóa menu thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa menu thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
           
        }
       
        public ActionResult Active(int id,int active)
        {
            try
            {
                bool status = false;
                if (active == 1)
                    status = true;
                logoRepository.Active(id, status);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        
    }
}
