using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        readonly IContactReporitory _ContactReporitory = new ContactReporitory();

        [Authorize(Roles = "Index")]
        public ActionResult Add()
        {
            var contact = _ContactReporitory.GetAll().FirstOrDefault();
            return View(contact);
        }
        [Authorize(Roles = "Add")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(int id, string content)
        {
            try
            {
                if (id == 0)
                    _ContactReporitory.Add(content);
                else
                {
                    Contact contact = new Contact();
                    contact.ID = id;
                    contact.Contents = content;
                    _ContactReporitory.Edit(contact);
                }

                return Json(new { IsSuccess = true, Message = "Lưu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, Message = "Lưu thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
