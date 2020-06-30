using QD_Tour_Web.Filters;
using QD_Tour_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Web.Controllers
{
    public class TourReservationModel
    {
        public string MemberId { get; set; }
        public DateTime StartTime { get; set; }
        public string TourPackageId { get; set; }
        public int NumberOfPeople { get; set; }

        public DateTime EndTime { get; set; }

        public decimal TotalPrice { get; set; }
    }
    public class TourReservationController : Controller
    {
        private QDTourWebEntities db = new QDTourWebEntities();
        // GET: TourReservation
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public decimal GetTotalPrice(string tourPackageId, DateTime startTime, DateTime endTime, int numberOfPeople)
        {
            var tourPackage = db.Tour_Package.Where(h => h.Id == tourPackageId).FirstOrDefault();
            var tourId = tourPackage.ID_Tour;
            var tourPrices = db.Tour_Price.Where(h => h.ID_Tour == tourId).Where(h => h.Date >= startTime && h.Date <= endTime).ToList();

            decimal totalPrice = 0m;

            foreach (var tour in tourPrices)
            {
                totalPrice += tour.Price;
            }

            return totalPrice * numberOfPeople;
        }

        [AuthorizeMember]
        [HttpPost]
        public string Add(TourReservationModel reservationModel)
        {
            if (HttpContext.Session["logedIn"] == null)
            {
                return "need login";
            }

            var tourPackage = db.Tour_Package.Where(h => h.Id == reservationModel.TourPackageId).FirstOrDefault();
            var tourId = tourPackage.ID_Tour;

            var newTourReservation = new Tour_Reservation()
            {
                Id = Guid.NewGuid().ToString(),
                StartTime = reservationModel.StartTime,
                EndTime = reservationModel.EndTime,
                ID_Package = reservationModel.TourPackageId,
                IsPaid = 0,
                TotalPrice = Convert.ToDecimal(reservationModel.TotalPrice),
                Tour_Package = db.Tour_Package.Where(p => p.Id == reservationModel.TourPackageId).FirstOrDefault(),
                PeopleNumber = reservationModel.NumberOfPeople
            };

            var member = db.Members.FirstOrDefault(m => m.Id == reservationModel.MemberId);

            member.Tour_Reservation.Add(newTourReservation);

            newTourReservation.Members.Add(member);

            db.Tour_Reservation.Add(newTourReservation);

            newTourReservation.Members.Add(member);

            if (db.SaveChanges() > 0)
            {
                return "添加成功";
            }
            return "";
        }
    }
}