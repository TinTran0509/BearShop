using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using Web.Models;
using Web.DAO;
using Web.Core;
using Web.Commons;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(Login lg)
        {
            if (ModelState.IsValid)
            {
                var dao = new login();
                var result = dao.Login(lg.Username, HelperEncryptor.Md5Hash(lg.Password));
                if (result == 1)
                {
                    var user = dao.GetById(lg.Username);
                    var userSession = new UserLogin();
                    Session["name"] = user.UserName;
                    Session["id"] = user.ID;
                    userSession.Username = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstant.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không đúng");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không thành công");
                }
            }
            return View("Index");
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}
