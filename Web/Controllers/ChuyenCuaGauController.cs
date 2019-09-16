﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class ChuyenCuaGauController : BaseController
    {
        readonly  INewsRepository newsRepository = new NewsRepository();
        readonly ICategoryRepository categoryRepository = new CategoryRepository();
        // GET: ChuyenCuaGau
        public ActionResult Index()
        {
                return View();
        }
        public ActionResult Detail()
        {
            return View();
        }
    }
}