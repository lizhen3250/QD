using QD_Tour_Web.Filters;
using QD_Tour_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Web.Controllers
{
    public class VehicleReservationModel
    {
        public string MemberId { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string GuidePackageId { get; set; }

        public decimal TotalPrice { get; set; }

        public string VehicleType { get; set; }
    }
    public class VehicleReservationController : Controller
    {
        private QDTourWebEntities db = new QDTourWebEntities();
        // GET: VehicleReservation
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeMember]
        [HttpPost]
        public string Add(VehicleReservationModel reservationModel)
        {
            if (HttpContext.Session["logedIn"] == null)
            {
                return "need login";
            }

            var newVehicleReservation = new Vehicle_Reservation()
            {
                Id = Guid.NewGuid().ToString(),
                Start_Time = reservationModel.StartTime,
                End_Time = reservationModel.EndTime,
                People_Number = reservationModel.NumberOfPeople,
                ID_Vehicle_Package = reservationModel.GuidePackageId,
                IsPaid = 0,
                TotalPrice = reservationModel.TotalPrice,
                Vehicle_Package = db.Vehicle_Package.Where(p => p.Id == reservationModel.GuidePackageId).FirstOrDefault(),
                Type = reservationModel.VehicleType

            };

            var member = db.Members.FirstOrDefault(m => m.Id == reservationModel.MemberId);

            member.Vehicle_Reservation.Add(newVehicleReservation);

            newVehicleReservation.Members.Add(member);

            db.Vehicle_Reservation.Add(newVehicleReservation);

            newVehicleReservation.Members.Add(member);

            if (db.SaveChanges() > 0)
            {
                return "添加成功";
            }
            return "";
        }
    }
}