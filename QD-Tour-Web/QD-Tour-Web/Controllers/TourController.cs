using QD_Tour_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Web.Controllers
{
    public class TourController : Controller
    {
        private QDTourWebEntities db = new QDTourWebEntities();
        // GET: Tour
        public ActionResult Index()
        {
            var tourPackage = db.Tour_Package.ToList();
            return View(tourPackage);
        }

        public ActionResult Details(string Id)
        {
            var tourPackage = db.Tour_Package.FirstOrDefault(p => p.Id == Id);
            return View(tourPackage);
        }

        [HttpGet]
        public JsonResult GetAllPrices(string Id)
        {

            var tourPrices = from tourPackage in db.Tour_Package
                              where tourPackage.Id == Id
                              join tourPrice in db.Tour_Price on tourPackage.ID_Tour equals tourPrice.ID_Tour
                              select new
                              {
                                  Price = tourPrice.Price,
                                  Id = tourPrice.Id,
                                  Date = tourPrice.Date
                              };

            return Json(tourPrices.ToList(), JsonRequestBehavior.AllowGet);

        }
    }
}