using QD_Tour_Admin.Model;
using QD_Tour_Admin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Admin.Controllers
{
    public class HotelReservationModel
    {
        public Hotel_Package HotelPackage { get; set; }

        public Hotel_Reservation HotelReservation { get; set; }

        public Member Member { get; set; }
    }

    [CustomAuthorize]
    public class HotelReservationController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();
        // GET: HotelReservation
        public ActionResult Index()
        {
            List<HotelReservationModel> hotelReservationModel = new List<Controllers.HotelReservationModel>();

            var hotelReservations = db.Hotel_Reservation.ToList();

            foreach (var hotelReservation in hotelReservations)
            {
                HotelReservationModel newHotelReservationModel = new HotelReservationModel();
                var member = db.Members.Where(m => m.Hotel_Reservation.Any(r => r.Id == hotelReservation.Id)).FirstOrDefault();
                newHotelReservationModel.Member = member;
                newHotelReservationModel.HotelReservation = hotelReservation;
                newHotelReservationModel.HotelPackage = db.Hotel_Package.Where(p => p.Id == hotelReservation.ID_Package).FirstOrDefault();
                hotelReservationModel.Add(newHotelReservationModel);

            }

            return View(hotelReservationModel);
        }

        [HttpGet]
        public JsonResult Details(string Id)
        {
            var result = from hotelReservation in db.Hotel_Reservation
                         from member in hotelReservation.Members
                         join hotelPackage in db.Hotel_Package on hotelReservation.ID_Package equals hotelPackage.Id
                         where hotelReservation.Id == Id
                         select new
                         {
                             hotelReservationId = hotelReservation.Id,
                             memberName = member.Name,
                             email = member.Email,
                             startTime = hotelReservation.StartTime,
                             endTime = hotelReservation.EndTime,
                             isPaid = hotelReservation.IsPaid,
                             type = hotelReservation.RoomType,
                             Photo = hotelReservation.Hotel_Package.Photo,
                             totalPrice = hotelReservation.TotalPrice,
                             area = hotelReservation.Hotel_Package.Area,
                             country = hotelReservation.Hotel_Package.Country,
                             hotelName = hotelReservation.Hotel_Package.Hotel.Name
                         };

            //var aa = from vehicleReservation in db.Vehicle_Reservation
            //         from member in vehicleReservation.Members
            //         join vehiclePacakge in db.Vehicle_Package on vehicleReservation.ID_Vehicle_Package equals vehiclePacakge.Id
            //         where vehicleReservation.Id == Id
            //         select new
            //         {
            //             vehicleReservationId = vehicleReservation.Id,
            //             memberName = member.Name,
            //             email = member.Email,
            //             startTime = vehicleReservation.Start_Time,
            //             endTime = vehicleReservation.End_Time,
            //             numberOfPeople = vehicleReservation.People_Number,
            //             isPaid = vehicleReservation.IsPaid,
            //             Photo = vehiclePacakge.Photo,
            //             type = vehiclePacakge.Type,
            //             totalPrice = vehicleReservation.TotalPrice,
            //             area = vehiclePacakge.Area
            //         };

            //if (result.Count() == 0)
            //{
            //    return Json(aa, JsonRequestBehavior.AllowGet);
            //}

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Update(Hotel_Reservation hotelReservation)
        {
            Hotel_Reservation newHotelReservation = db.Hotel_Reservation.Where(r => r.Id == hotelReservation.Id).FirstOrDefault();


            newHotelReservation.IsPaid = hotelReservation.IsPaid;

            db.Entry(newHotelReservation).State = System.Data.Entity.EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return "更新成功";
            }

            return "更新失败";
        }

        [HttpPost]
        public string Delete(string hotelReservationId, string memberId)
        {
            Hotel_Reservation hotelReservation = db.Hotel_Reservation.FirstOrDefault(v => v.Id == hotelReservationId);
            Member member = hotelReservation.Members.FirstOrDefault(m => m.Id == memberId);


            if (hotelReservation != null && member != null)
            {
                hotelReservation.Members.Remove(member);
                db.Hotel_Reservation.Remove(hotelReservation);
                if (db.SaveChanges() > 0)
                {
                    return "删除成功";
                }

            }

            return "删除失败";
        }
    }
}