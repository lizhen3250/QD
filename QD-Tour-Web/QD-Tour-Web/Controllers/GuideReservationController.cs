using QD_Tour_Web.Filters;
using QD_Tour_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Web.Controllers
{
    public class ReservationModel
    {
        public string MemberId { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string GuidePackageId { get; set; }

        public decimal TotalPrice { get; set; }
         
    }
    public class GuideReservationController : Controller
    {
        private QDTourWebEntities db = new QDTourWebEntities();
        // GET: GuideReservation
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeMember]
        [HttpPost]
        public string Add(ReservationModel reservationModel)
        {
            var newGuideReservation = new Guide_Reservation()
            {
                Id = Guid.NewGuid().ToString(),
                Start_Time = reservationModel.StartTime,
                End_Time = reservationModel.EndTime,
                People_Number = reservationModel.NumberOfPeople,
                ID_Guide_Package = reservationModel.GuidePackageId,
                IsPaid = 0,
                TotalPrice = reservationModel.TotalPrice,
                Guide_Package = db.Guide_Package.Where(p => p.Id == reservationModel.GuidePackageId).FirstOrDefault()
            };

            var member = db.Members.FirstOrDefault(m => m.Id == reservationModel.MemberId);
                     
            member.Guide_Reservation.Add(newGuideReservation);

            newGuideReservation.Members.Add(member);

            db.Guide_Reservation.Add(newGuideReservation);

            newGuideReservation.Members.Add(member);

            if(db.SaveChanges() >0 )
            {
                return "添加成功";
            }
            return "";
        }
    }
}