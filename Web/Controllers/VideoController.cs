using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Web.BaseSecurity;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class VideoController : BaseController
    {
        //
        // GET: /Video/
        readonly IVideoRepository _videoRepository = new VideoRepository();
        public ActionResult Index()
        {
            var lstVideo = _videoRepository.GetAll().Where(x=>x.Status).ToList();
            return View(lstVideo);
        }
        public ActionResult VideoProduct()
        {
            var video = _videoRepository.GetAll().FirstOrDefault(g => g.Status && g.IsShowPlay);
            return PartialView(video);
        }
        public ActionResult Fanpage()
        {
            var video = _videoRepository.GetAll().FirstOrDefault(g => g.Status);
            return PartialView(video);
        }
    }
}
