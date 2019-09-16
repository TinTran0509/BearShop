using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class VideoController : BaseController
    {
        readonly IVideoRepository _videoRepository = new VideoRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        //
        // GET: /Admin/Video/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Index")]
        public ActionResult ListData(string name,int page)
        {
            var lstVideo = _videoRepository.GetByName(name);
            var totalVideo = lstVideo.Count();
            lstVideo = lstVideo.OrderByDescending(g => g.ID).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit);
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Video/_ListData.cshtml", lstVideo),
                totalPages = Math.Ceiling(((double)totalVideo / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
      
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var objUser = _userAdminRepository.Find(User.ID);
            return Json(RenderViewToString("~/Areas/Admin/Views/Video/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(Video obj)
        {
            try
            {
                var video = _videoRepository.GetAll().FirstOrDefault(x => x.Name == obj.Name.Trim());
                if (video != null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Tên video đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                }
                obj.CreatedBy = User.ID;
                _videoRepository.Add(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới video thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới video thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var objVideo = _videoRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/Video/_Edit.cshtml", objVideo), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(Video obj)
        {
            try
            {
                var video = _videoRepository.GetAll().FirstOrDefault(x => x.Name == obj.Name.Trim() && x.ID != obj.ID);
                if (video != null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Tên video đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                }
                _videoRepository.Edit(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật Video thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật Video thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _videoRepository.Find(id);
                _videoRepository.Delete(id);
                if (obj.Type == 1)
                {
                    var file = Server.MapPath(obj.Url);
                    if (System.IO.File.Exists(file))
                    {
                        Common.TryToDelete(file);
                    }
                }
               
            }
            catch (Exception)
            {

                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa Video thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa Video thành công",
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult DeleteAll(string lstid)
        {
            var arrid = lstid.Split(',');
            var count = 0;
            foreach (var item in arrid)
            {
                try
                {
                    _videoRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} video", count),
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]

        public JsonResult Upload()
        {
            if (Request.Files.Count <= 0)
                return Json(new { status = false, msg = "Bạn chưa chọn file." });
            var videoExtention = new[] { "avi", "mp4", "flv", "wmv", "mov" };
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            var section = config.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            double maxFileSize = 4 * 2048 * 2048;
            if (section != null)
            {
                maxFileSize = section.MaxRequestLength;
            }
            var now = DateTime.Now;
            var objInfo = new FileInfo();
            try
            {

                //  Get all files from Request object  
                var files = Request.Files[0];
                var path = ConfigUpload.TargetUpload;
                var fileData = files;
                var extention = GetExtention(fileData.FileName);
                if (!videoExtention.Contains(extention))
                {
                    return Json(new { status = false, msg = "Video upload không đúng định dạng cho phép." });
                }
                #region KIỂM TRA KÍCH THƯỚC FILE
                var fileSize = fileData.ContentLength;
                if (fileSize > (maxFileSize))
                {
                    return Json(new { status = false, msg = "tập tin này vượt quá dung lượng cho phép" });
                }
                if (fileSize == 0)
                {
                    return Json(new { status = false, msg = "kiểm tra lại tập tin này" });
                }
                #endregion
                #region TẠO THƯ MỤC CHỨA FILES UPLOAD
                path = string.Format("{0}/{1}", path, "Videos");
                if (!Directory.Exists(Server.MapPath(path)))
                {
                    Directory.CreateDirectory(Server.MapPath(path));
                }
                path = string.Format("{0}/{1}/{2}", path, now.Year, now.Month);
                if (!Directory.Exists(Server.MapPath(path)))
                {
                    Directory.CreateDirectory(Server.MapPath(path));
                }
                #endregion
                #region COPY FILE VÀO THƯ MỤC

                var newName = string.Format("{0}-{1}", HelperEncryptor.Md5Hash(DateTime.Now.ToString()), fileData.FileName);
                fileData.SaveAs(string.Format("{0}/{1}", Server.MapPath(path), newName));
                #endregion
                objInfo = new FileInfo
                {
                    FileName = string.Format("{0}/{1}", path, newName),
                    FileNameOriginal = fileData.FileName,
                    FileSize = fileData.ContentLength < 1024 ? string.Format("{0} Bytes", (fileData.ContentLength)) : string.Format("{0} KB", (fileData.ContentLength / 1024))
                };
            }
            catch (Exception)
            {
                return Json(new { status = false, msg = "Upload không thành công", file = objInfo });
            }
            return Json(new { status = true, msg = "Upload thành công", file = objInfo });
        }

        public class FileInfo
        {
            public string FileName { get; set; }
            public string FileSize { get; set; }
            public string FileNameOriginal { get; set; }
        }
        public string GetExtention(string file)
        {
            if (!string.IsNullOrEmpty(file))
            {
                var arr = file.Split('.');
                if (arr.Length > 0)
                {
                    return arr.Last();
                }
            }
            return "";
        }
        public ActionResult RemoveVideo(string link)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string name = serializer.Deserialize<string>(link);
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Upload/Videos/"),name));
                return Json(new
                {
                    Success = true,
                    Message = "Xóa video thành công"
            }, JsonRequestBehavior.AllowGet);
           }
            catch (Exception)
            {

                 return Json(new
            {
                Success = false,
                Message = "Xóa video thất bại"
            }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
