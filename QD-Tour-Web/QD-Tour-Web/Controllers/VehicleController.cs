using QD_Tour_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Web.Controllers
{
    public class VehicleController : Controller
    {
        private QDTourWebEntities db = new QDTourWebEntities();
        // GET: Vehicle
        public ActionResult Index()
        {
            var vehiclePackage = db.Vehicle_Package.ToList();
            return View(vehiclePackage);
        }

        public ActionResult Details(string Id)
        {
            var vehiclePackage = db.Vehicle_Package.FirstOrDefault(p => p.Id == Id);
            return View(vehiclePackage);
        }
    }
}