using QD_Tour_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Web.Controllers
{
    public class GuideController : Controller
    {
        private QDTourWebEntities db = new QDTourWebEntities();
        // GET: Guide
        public ActionResult Index()
        {
            var guidePackage = db.Guide_Package.ToList();
            return View(guidePackage);
        }

        public ActionResult Details(string Id)
        {
            var guidePackage = db.Guide_Package.FirstOrDefault(p => p.Id == Id);
            return View(guidePackage);
        }
    }
}