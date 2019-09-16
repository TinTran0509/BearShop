using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class NewsController : BaseController
    {

        INewsRepository newsRepository = new NewsRepository();
        ICategoryRepository categoryRepository = new CategoryRepository();
        // GET: News
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListData(string keyWord, int pageIndex,int status)
        {
            Session["PAGE"] = pageIndex;
            int page =Convert.ToInt32(Request.QueryString["page"]);
            if (page > 1)
                pageIndex = page;
            var model = newsRepository.ListAll(keyWord,status).ToList();
            var totalAdv = model.Count();
            model = model.Skip((pageIndex - 1) * 10).Take(10).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/News/ListData.cshtml", model),
                totalPages = Math.Ceiling(((double)totalAdv / 10)),
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id)
        {
            return View();
        }
        [Authorize(Roles = "Add")]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [Authorize(Roles = "Add")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(News model,string close)
        {
            try
            {
                var news = newsRepository.FindByTitle(model.MetaTitle);
                if (news != null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Message = "Tiêu đề tin tức đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.Image))
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Message = "Vui lòng thêm ảnh đại diện bài viết",
                    }, JsonRequestBehavior.AllowGet);

                }
                if (string.IsNullOrEmpty(model.Contents))
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Message = "Vui lòng thêm nội dung bài viết",
                    }, JsonRequestBehavior.AllowGet);
                }
               
                model.CreatedBy =  User.ID;
                newsRepository.Add(model);
                return Json(new
                {
                    Close = close,
                    IsSuccess = true,
                    Message = "Thêm mới thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = "Thêm mới thất bại "
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = newsRepository.Find(id);
            return View(obj);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(News model)
        {
            try
            {
                var news = newsRepository.FindByTitle(model.MetaTitle);
                if (news != null && news.ID != model.ID)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Message = "Tiêu đề tức đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.Image))
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Message = "Vui lòng thêm ảnh đại diện bài viết",
                    }, JsonRequestBehavior.AllowGet);

                }
                if (string.IsNullOrEmpty(model.Contents))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        Message = "Vui lòng thêm nội dung bài viết",
                    }, JsonRequestBehavior.AllowGet);
                }
                var page = Session["PAGE"];
                if (page == null)
                    page = 1;
                newsRepository.Edit(model);
                return Json(new {
                    Page = (int)page,
                    IsSuccess = true,
                    Message = "Cập nhật thành công",
                    JsonRequestBehavior.AllowGet });
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, Message = "Cập nhật thất bại", JsonRequestBehavior.AllowGet });
            }
        }
        [Authorize(Roles = "Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                newsRepository.Delete(id);
                return Json(new { IsSuccess = true, Message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UnPublish(int id)
        {
            try
            {
                newsRepository.UnPublish(id);
                return Json(new { IsSuccess = true, Message = "Hủy đăng bài thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Hủy đăng bài thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Publish(int id)
        {
            try
            {
                newsRepository.Publish(id);
                return Json(new { IsSuccess = true, Message = "Đăng bài thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Đăng bài thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Detail(int id)
        {
            var obj = newsRepository.Detail(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/News/Detail.cshtml",obj), JsonRequestBehavior.AllowGet);
        }
    }
}