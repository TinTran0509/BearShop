using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class HomeMenuController : BaseController
    {
        readonly IHomeMenuRepository _homeMenuRepository = new HomeMenuRepository();
        //
        // GET: /Admin/HomeMenu/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(int page)
        {
            var model = _homeMenuRepository.GetAll();
            var totalAdv = model.Count();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/HomeMenu/_ListData.cshtml", model),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Index")]
        public ActionResult GetAllByLangCode(string LangCode)
        {
            LangCode = LangCode ?? Webconfig.LangCodeVn;
            var lstHomeMenu = _homeMenuRepository.GetAll().ToList();
            var lstLevel = Common.CreateLevel(lstHomeMenu.ToList());
            return Json(lstLevel, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            TempData["MenuHome"] = _homeMenuRepository.GetAll().ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/HomeMenu/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(HomeMenu obj)
        {
            try
            {
                var menu = _homeMenuRepository.GetAll().FirstOrDefault(x => x.Name.Trim() == obj.Name.Trim());
                if(menu != null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Tên menu đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                }
                obj.CreatedDate = DateTime.Now;
                _homeMenuRepository.Add(obj);
                
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới menu thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới menu thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            TempData["MenuHome"] = _homeMenuRepository.GetAll().ToList();
            var objHomeMenu = _homeMenuRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/HomeMenu/_Edit.cshtml", objHomeMenu), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(HomeMenu obj)
        {
            try
           {
                var munuName = _homeMenuRepository.GetAll().FirstOrDefault(x => x.Name.Trim() == obj.Name.Trim() && x.ID != obj.ID);
                if(munuName!= null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Tên menu đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                }
                _homeMenuRepository.Edit(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật menu thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật menu thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _homeMenuRepository.Find(id);
                _homeMenuRepository.Delete(id);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa menu thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa menu thành công",
            }, JsonRequestBehavior.AllowGet);
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
                    // xoa danhh muc
                    _homeMenuRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} menu", count),
            }, JsonRequestBehavior.AllowGet);
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
                var obj = _homeMenuRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _homeMenuRepository.Edit(obj);
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
    }
}
