using QD_Tour_Web.Filters;
using QD_Tour_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Web.Controllers
{
    public class GolfReservationModel
    {
        public string MemberId { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime StartTime { get; set; }

        public string GolfPackageId { get; set; }

        public decimal TotalPrice { get; set; }

        public string GolfHole { get; set; }

    }

    public class GolfReservationController : Controller
    {
        private QDTourWebEntities db = new QDTourWebEntities();
        // GET: GolfReservation
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeMember]
        [HttpPost]
        public string Add(GolfReservationModel reservationModel)
        {
            var newGolfReservation = new Golf_Reservation()
            {
                Id = Guid.NewGuid().ToString(),
                StartTime = reservationModel.StartTime,
                PeopleNumber = reservationModel.NumberOfPeople,
                Golf_Package = db.Golf_Package.Where(p => p.Id == reservationModel.GolfPackageId).FirstOrDefault(),
                IsPaid = 0,
                TotalPrice = reservationModel.TotalPrice,
                Golf_Package_Id = reservationModel.GolfPackageId,
                GolfHole = reservationModel.GolfHole
            };

            var member = db.Members.FirstOrDefault(m => m.Id == reservationModel.MemberId);

            member.Golf_Reservation.Add(newGolfReservation);

            newGolfReservation.Members.Add(member);

            db.Golf_Reservation.Add(newGolfReservation);

            newGolfReservation.Members.Add(member);

            if (db.SaveChanges() > 0)
            {
                return "添加成功";
            }
            return "";
        }
    }
}