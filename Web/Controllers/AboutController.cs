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
    public class AboutController : Controller
    {
        IAboutRepository aboutRepository = new AboutRepository();
        // GET: About
        public ActionResult LoadAbout()
        {
            var about = aboutRepository.GetAll().ToList();
            return PartialView(about);
        }
        public ActionResult Detail(string linkseo)
        {
            var about = aboutRepository.GetAll().FirstOrDefault(x=>x.LinkSeo == linkseo);
            return View(about);
        }
    }
}