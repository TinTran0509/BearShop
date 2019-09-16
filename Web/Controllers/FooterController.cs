using CMS.IRepository;
using CMS.Reporitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class FooterController : Controller
    {
        IFooterRepository footerRepository = new FooterRepository();
        // GET: Footer
        public ActionResult LoadFooter()
        {
            var footer = footerRepository.GetAll().FirstOrDefault();
            return PartialView(footer);
        }
       
    }
}