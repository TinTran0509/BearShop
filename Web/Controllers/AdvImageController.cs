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
    public class AdvImageController : Controller
    {
        IAdvImageRepository adv_ImageRepository = new AdvImageRepository();
        // GET: AdvImage
        public ActionResult ImageBanner()
        {
            var obj = adv_ImageRepository.GetAll().FirstOrDefault(x=>x.Position == 1 && x.Status);
            return PartialView(obj);
        }
        public ActionResult ImageButtom()
        {
            var obj = adv_ImageRepository.GetAll().FirstOrDefault(x => x.Position == 4 && x.Status);
            return PartialView(obj);
        }
        public ActionResult AdvNews()
        {
            var lst = adv_ImageRepository.GetAll().Where(x => x.Position == 5 && x.Status).Take(2).ToList();
            return PartialView(lst);
        }
    }
}