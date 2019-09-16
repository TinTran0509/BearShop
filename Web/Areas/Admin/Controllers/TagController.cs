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
    public class TagController : BaseController
    {
        readonly ITagRepository tagRepository = new TagRepository();
        //
        // GET: /Admin/Tag/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(int page = 1)
        {
            var model = tagRepository.GetAll();
            var totalAdv = model.Count();
            model = model.Skip((page - 1) * 10).Take(10).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Tag/_ListData.cshtml", model),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            return Json(RenderViewToString("~/Areas/Admin/Views/Tag/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(Tag obj)
        {
            try
            {
                var tag = tagRepository.GetAll().FirstOrDefault(x => x.Name.Trim() == obj.Name.Trim());
                if(tag != null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Tên thẻ tag đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                }
                tagRepository.Add(obj);
                
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
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
            var objHomeMenu = tagRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/Tag/_Edit.cshtml", objHomeMenu), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(Tag obj)
        {
            try
           {
                var tag = tagRepository.GetAll().FirstOrDefault(x => x.Name.Trim() == obj.Name.Trim() && x.ID != obj.ID);
                if(tag!= null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Tên thẻ tag đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                }
                tagRepository.Edit(obj);
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
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = tagRepository.Find(id);
                tagRepository.Delete(id);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa thành công",
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
                    tagRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} ", count),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
