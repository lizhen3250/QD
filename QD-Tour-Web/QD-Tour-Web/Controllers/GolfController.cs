using QD_Tour_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Web.Controllers
{
    public class GolfController : Controller
    {
        private QDTourWebEntities db = new QDTourWebEntities();
        // GET: Hotel
        public ActionResult Index()
        {
            var golfPackages = db.Golf_Package.ToList();
            return View(golfPackages);
        }

        public ActionResult Details(string Id)
        {
            var golfPackage = db.Golf_Package.FirstOrDefault(p => p.Id == Id);
            return View(golfPackage);
        }

        [HttpGet]
        public JsonResult GetAllPrices(string Id)
        {
            var golfTotalPrice = from golfPackage in db.Golf_Package
                                 where golfPackage.Id == Id
                                 join golfPrice in db.Golf_Price on golfPackage.Golf_Id equals golfPrice.Golf_Id
                                 select new
                                 {
                                     Id = golfPrice.Id,
                                     EighteenHolePrice = golfPrice.Eighteen_Hole_Price,
                                     TwentySevenHolePrice = golfPrice.TwentySeven_Hole_Price,
                                     ThirtySixHolePrice = golfPrice.ThirySix_Hole_Price,
                                     Date = golfPrice.Date
                                 };

            

            return Json(golfTotalPrice.ToList(), JsonRequestBehavior.AllowGet);

        }
    }
}