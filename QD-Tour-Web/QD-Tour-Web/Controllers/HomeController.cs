using QD_Tour_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Web.Controllers
{
    public class TourPackageModel
    {
        public string Id { get; set; }
        public Tour Tour { get; set; }

        public string Photo { get; set; }
    }
    public class HomeController : Controller
    {
        private QDTourWebEntities db = new QDTourWebEntities();
        public ActionResult Index()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var tours = from tourPackage in db.Tour_Package
                        select new TourPackageModel
                        {
                            Id = tourPackage.Id,
                            Tour = tourPackage.Tour,
                            Photo = tourPackage.Photo
                        };

            return View(tours.Take(6).ToList());
        }

        public JsonResult GetPopularDestination()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var tours = from tourPackage in db.Tour_Package
                        select new
                        {
                            Id = tourPackage.Id,
                            Tour = tourPackage.Tour,
                            Photo = tourPackage.Photo
                        };

            return Json(tours.Take(6).ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
