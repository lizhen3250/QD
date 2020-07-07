using QD_Tour_Web.Filters;
using QD_Tour_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QD_Tour_Web.Controllers
{
    public class HotelReservationModel
    {
        public string MemberId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string HotelPackageId { get; set; }

        public decimal TotalPrice { get; set; }

        public string RoomType { get; set; }
    }

    public class HotelReservationController : Controller
    {
        private QDTourWebEntities db = new QDTourWebEntities();
        // GET: HotelReservation
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeMember]
        [HttpPost]
        public string Add(HotelReservationModel reservationModel)
        {
            if (HttpContext.Session["logedIn"] == null)
            {
                return "need login";
            }

            var hotelPackage = db.Hotel_Package.Where(h => h.Id == reservationModel.HotelPackageId).FirstOrDefault();
            var hotelId = hotelPackage.ID_Hotel;
          
            var newHotelReservation = new Hotel_Reservation()
            {
                Id = Guid.NewGuid().ToString(),
                StartTime = reservationModel.StartTime,
                EndTime = reservationModel.EndTime,
                ID_Package = reservationModel.HotelPackageId,
                IsPaid = 0,
                TotalPrice = reservationModel.TotalPrice,
                Hotel_Package = db.Hotel_Package.Where(p => p.Id == reservationModel.HotelPackageId).FirstOrDefault(),
                RoomType = reservationModel.RoomType
            };

            var member = db.Members.FirstOrDefault(m => m.Id == reservationModel.MemberId);

            member.Hotel_Reservation.Add(newHotelReservation);

            newHotelReservation.Members.Add(member);

            db.Hotel_Reservation.Add(newHotelReservation);

            newHotelReservation.Members.Add(member);

            if (db.SaveChanges() > 0)
            {
                return "添加成功";
            }

            return "添加失败";
        }

        [HttpGet]
        public decimal GetTotalPrice(string hotelPackageId, DateTime startTime, DateTime endTime, string roomType)
        {
            var hotelPackage = db.Hotel_Package.Where(h => h.Id == hotelPackageId).FirstOrDefault();
            var hotelId = hotelPackage.ID_Hotel;
            var hotelPrices = db.Hotel_Price.Where(h => h.ID_Hotel == hotelId).Where(h => h.Date >= startTime && h.Date < endTime).ToList();

            decimal? totalPrice = 0m;

            foreach (var hotel in hotelPrices)
            {
                if (roomType == "单人间")
                {
                    totalPrice += hotel.SingleRoomPrice;
                }
                else if (roomType == "双人间")
                {
                    totalPrice += hotel.DoulbeRoomPrice;
                }
                else
                {
                    totalPrice += hotel.OtherRoomPrice;
                }

            }

            return Convert.ToDecimal(totalPrice);
        }
    }
}