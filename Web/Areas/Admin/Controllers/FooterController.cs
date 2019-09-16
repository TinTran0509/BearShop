using System;
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
    public class FooterController : BaseController
    {
        IFooterRepository footerRepository = new FooterRepository();
        // GET: Menu
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var model = footerRepository.GetAll().FirstOrDefault();
            return View(model);
        }
        [Authorize(Roles = "Add")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(int id, string content)
        {
            try
            {
                if (id == 0)
                    footerRepository.Add(content);
                else
                {
                    Footer footer = new Footer();
                    footer.ID = id;
                    footer.Contents = content;
                    footerRepository.Edit(footer);
                }

                return Json(new { IsSuccess = true, Message = "Lưu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Lưu thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
