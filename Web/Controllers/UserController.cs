using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /Login/
        private readonly IUserRepository _userRepository = new UserRepository();
        private BearShopEntities dbcontext = new BearShopEntities();
        public ActionResult LogIn()
        {
            if (User != null) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(User obj)
        {
            if (string.IsNullOrEmpty(obj.UserName) || string.IsNullOrWhiteSpace(obj.Password))
            {
                Session["Messenger"] = new Notified
                {
                    Value = EnumNotifield.Error,
                    Messenger = "Bạn chưa nhập tên đăng nhập hoặc mật khẩu nhập vào không đúng!"
                };
                return View();
            }
            var user =
                _userRepository.GetAll()
                    .FirstOrDefault(
                        u => u.Password == HelperEncryptor.Md5Hash(obj.Password) && u.UserName == obj.UserName);
            if (user != null)
            {
                if (user.Active == true)
                {
                    user.TimeLogin = DateTime.Now;
                    user.IPLogin = obj.IPLogin;
                    _userRepository.Edit(user);
                    if (user.Active == false)
                    {
                        Session["Messenger"] = "Tài khoản này chưa được kích hoạt!";
                        return View();
                    }
                    var serializeModel = new CustomPrincipalSerializeModel
                    {
                        ID = user.ID,
                        Username = user.UserName,
                        FullName = user.FullName,
                        Email = user.Email,
                        GroupUser = user.GroupUserID,
                        UserType = user.UserType,
                        NoiBo = user.NoiBo,
                    };
                    var serializer = new JavaScriptSerializer();
                    var userData = serializer.Serialize(serializeModel);
                    var authTicket = new FormsAuthenticationTicket(
                        1,
                        obj.UserName,
                        DateTime.Now,
                        DateTime.Now.AddHours(1),
                        false,
                        userData);

                    var encTicket = FormsAuthentication.Encrypt(authTicket);
                    var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);
                }
                else
                {
                    Session["Messenger"] = "Tài Khoản chưa được kích hoạt";
                    return View();
                }
            }
            else
            {
                Session["Messenger"] = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return View();
            }
            var preUrl = (Uri)Session["returnUrl"];
            if (preUrl != null)
            {
                Session["returnUrl"] = null;
                return Redirect(preUrl.ToString());
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user)
        {
            user.CreatedDate = DateTime.Now;
            user.Active = false;
            user.Password = HelperEncryptor.Md5Hash(user.Password);
            var arrUserName = _userRepository.GetAll().Select(g => g.UserName);
            if (arrUserName.Contains(user.UserName))
            {
                ViewBag.existUser = 1;
                return View();
            }
            _userRepository.Add(user);
            return Redirect("/?regsuccess=1");
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            var arrEmail = _userRepository.GetAll().Select(g => g.Email);
            if (!arrEmail.Contains(Email))
            {
                ViewBag.noEmail = 1;
                return View();
            }
            return Redirect("/?resetpass=1");
        }
        #region Đăng nhập rồi 
        [Authorize]
        public ActionResult Edit()
        {
           
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit(User user)
        {
            var userold = _userRepository.Find(User.ID);
            userold.FullName = user.FullName;
            userold.Phone = user.Phone;
            userold.Address = user.Address;
            userold.DonviId = user.DonviId;
            userold.ChucVuId = user.ChucVuId;
            _userRepository.Edit(userold);
            return Redirect("/?success=1");
        }
        [Authorize]
        public ActionResult ChangePass()
        {
            var id = User.ID;
            if (id < 1)
            {
                return Redirect("/Error/AccessDenined");
            }
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult ChangePass(User user)
        {
            var id = User.ID;
            if (id < 1)
            {
                return Redirect("/Error/AccessDenined");
            }
            var userold = _userRepository.Find(id);
            var passnew = HelperEncryptor.Md5Hash(user.Password);
            if (passnew != userold.Password)
            {
                ViewBag.wrongpass = 1;
                return View();
            }
            var newpassword = Request["NewPassword"];
            if (!string.IsNullOrEmpty(newpassword))
            {
                var passcut = HelperEncryptor.Md5Hash(newpassword);
                userold.Password = passcut;
                _userRepository.Edit(userold);
            }
            return Redirect("/?success=1");
        }
        #endregion
        public ActionResult Profiled()
        {
            return View();
        }
        public JsonResult GetDetail(int id)
        {
            var user = dbcontext.Users.Find(id);
            return Json(new
            {
                data = user
            }, JsonRequestBehavior.AllowGet);
        }
    }
}