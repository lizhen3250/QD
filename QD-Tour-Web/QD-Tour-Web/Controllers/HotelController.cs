using QD_Tour_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Web.Controllers
{
    public class HotelController : Controller
    {
        private QDTourWebEntities db = new QDTourWebEntities();
        // GET: Hotel
        public ActionResult Index()
        {
            var hotelPackages = db.Hotel_Package.ToList();
            return View(hotelPackages);
        }

        public ActionResult Details(string Id)
        {
            var hotelPackage = db.Hotel_Package.FirstOrDefault(p => p.Id == Id);
            return View(hotelPackage);
        }

        [HttpGet]
        public JsonResult GetAllPrices(string Id)
        {

            var hotelPrices = from hotelPackage in db.Hotel_Package
                              where hotelPackage.Id == Id
                              join hotelPrice in db.Hotel_Price on hotelPackage.ID_Hotel equals hotelPrice.ID_Hotel
                              select new
                              {
                                  SingleRoomPrice = hotelPrice.SingleRoomPrice,
                                  Id = hotelPrice.Id,
                                  DoulbeRoomPrice = hotelPrice.DoulbeRoomPrice,
                                  OtherRoomPrice =hotelPrice.OtherRoomPrice,
                                  Date = hotelPrice.Date
                              };

            return Json(hotelPrices.ToList(), JsonRequestBehavior.AllowGet);

        }
    }
}