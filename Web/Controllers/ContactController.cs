using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class ContactController : Controller
    {
        readonly IContactReporitory _ContactReporitory = new ContactReporitory();
        readonly IFeedBackReporitory feedBackReporitory = new FeedBackReporitory();
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
    }
}