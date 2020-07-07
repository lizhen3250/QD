using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QD_Tour_Admin.Model;

namespace QD_Tour_Admin.Controllers
{
    public class GolfController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();

        // GET: Golf
        public ActionResult Index()
        {
            var golfs = db.Golves.ToList();

            return View(golfs);
        }
    }
}