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
    public class PolicyController : Controller
    {
        IPolicyRepository policyRepository = new PolicyRepository();
        // GET: About
        public PartialViewResult LoadPolicy()
        {
            var policie = policyRepository.GetAll().ToList();
            return PartialView(policie);
        }
        public ActionResult Detail(string linkseo)
        {
            var policy = policyRepository.GetAll().FirstOrDefault(x => x.LinkSeo == linkseo);
            return View(policy);
        }
    }
}