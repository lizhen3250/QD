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
    }
}