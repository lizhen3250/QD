using QD_Tour_Admin.Model;
using QD_Tour_Admin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Admin.Controllers
{
    public class CustomTourReservationModel
    {
        public CustomTour CustomTourReservation { get; set; }

        public Member Member { get; set; }
    }

    [CustomAuthorize]
    public class CustomTourReservationController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();

        // GET: Golf
        public ActionResult Index()
        {
            List<CustomTourReservationModel> customTourReservationModel = new List<Controllers.CustomTourReservationModel>();

            var customTourReservations = db.CustomTours.ToList();

            foreach (var customTourReservation in customTourReservations)
            {
                CustomTourReservationModel newCustomTourReservationModel = new CustomTourReservationModel();
                var member = db.Members.Where(m => m.CustomTours.Any(r => r.Id == customTourReservation.Id)).FirstOrDefault();
                newCustomTourReservationModel.Member = member;
                newCustomTourReservationModel.CustomTourReservation = customTourReservation;
                customTourReservationModel.Add(newCustomTourReservationModel);

            }

            return View(customTourReservationModel);
        }
    }
}