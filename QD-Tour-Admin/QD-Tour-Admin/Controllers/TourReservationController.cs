using QD_Tour_Admin.Model;
using QD_Tour_Admin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Admin.Controllers
{
    public class TourReservationModel
    {
        public Tour_Package TourPackage { get; set; }

        public Tour_Reservation TourReservation { get; set; }

        public Member Member { get; set; }
    }

    [CustomAuthorize]
    public class TourReservationController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();
        // GET: TourReservation
        public ActionResult Index()
        {
            List<TourReservationModel> tourReservationModel = new List<Controllers.TourReservationModel>();

            var tourReservations = db.Tour_Reservation.ToList();

            foreach (var tourReservation in tourReservations)
            {
                TourReservationModel newTourReservationModel = new TourReservationModel();
                var member = db.Members.Where(m => m.Tour_Reservation.Any(r => r.Id == tourReservation.Id)).FirstOrDefault();
                newTourReservationModel.Member = member;
                newTourReservationModel.TourReservation = tourReservation;
                newTourReservationModel.TourPackage = db.Tour_Package.Where(p => p.Id == tourReservation.ID_Package).FirstOrDefault();
                tourReservationModel.Add(newTourReservationModel);

            }

            return View(tourReservationModel);
        }

        [HttpGet]
        public JsonResult Details(string Id)
        {
            var result = from tourReservation in db.Tour_Reservation
                         from member in tourReservation.Members
                         join tourPackage in db.Tour_Package on tourReservation.ID_Package equals tourPackage.Id
                         where tourReservation.Id == Id
                         select new
                         {
                             tourReservationId = tourReservation.Id,
                             memberName = member.Name,
                             email = member.Email,
                             startTime = tourReservation.StartTime,
                             endTime = tourReservation.EndTime,
                             isPaid = tourReservation.IsPaid,
                             Photo = tourReservation.Tour_Package.Photo,
                             totalPrice = tourReservation.TotalPrice,
                             province = tourReservation.Tour_Package.Tour.Province,
                             country = tourReservation.Tour_Package.Tour.Course,
                             name = tourReservation.Tour_Package.Tour.Name
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
        public string Update(Tour_Reservation tourReservation)
        {
            Tour_Reservation newHTourReservation = db.Tour_Reservation.Where(r => r.Id == tourReservation.Id).FirstOrDefault();


            newHTourReservation.IsPaid = tourReservation.IsPaid;

            db.Entry(newHTourReservation).State = System.Data.Entity.EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return "更新成功";
            }

            return "更新失败";
        }

        [HttpPost]
        public string Delete(string tourReservationId, string memberId)
        {
            Tour_Reservation tourReservation = db.Tour_Reservation.FirstOrDefault(v => v.Id == tourReservationId);
            Member member = tourReservation.Members.FirstOrDefault(m => m.Id == memberId);


            if (tourReservation != null && member != null)
            {
                tourReservation.Members.Remove(member);
                db.Tour_Reservation.Remove(tourReservation);
                if (db.SaveChanges() > 0)
                {
                    return "删除成功";
                }

            }

            return "删除失败";
        }

    }
}