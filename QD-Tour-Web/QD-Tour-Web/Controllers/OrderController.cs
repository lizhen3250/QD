using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Web.Controllers
{
    public class GuideOrderModel
    {

    }
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GuideOrderDetails()
        {
            return View();
        }

        public ActionResult VehicleOrderDetails()
        {
            return View();
        }

        public ActionResult TourOrderDetails()
        {
            return View();
        }

        public ActionResult HotelOrderDetails()
        {
            return View();
        }

        public ActionResult GolfOrderDetails()
        {
            return View();
        }

        public ActionResult OrderComplete()
        {
            return View();
        }
    }
}