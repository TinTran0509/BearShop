using CMS.IRepository;
using CMS.Reporitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HeaderController : Controller
    {
        IHeaderRepository headerRepository = new HeaderRepository();
        // GET: Footer
        public ActionResult Header()
        {
            var header = headerRepository.GetAll().FirstOrDefault();
            return PartialView(header);
        }
    }
}