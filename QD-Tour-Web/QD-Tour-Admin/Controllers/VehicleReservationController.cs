using QD_Tour_Admin.Model;
using QD_Tour_Admin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Admin.Controllers
{
    public class VehicleReservationModel
    {
        public Vehicle_Package VehiclePackage { get; set; }

        public Vehicle_Reservation VehicleReservation { get; set; }

        public Member Member { get; set; }
    }

    [CustomAuthorize]
    public class VehicleReservationController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();
        // GET: VehicleReservation
        public ActionResult Index()
        {
            List<VehicleReservationModel> guideReservationModel = new List<Controllers.VehicleReservationModel>();

            var vehicleReservations = db.Vehicle_Reservation.ToList();

            foreach (var vehicleReservation in vehicleReservations)
            {
                VehicleReservationModel newGuideReservationModel = new VehicleReservationModel();
                var member = db.Members.Where(m => m.Vehicle_Reservation.Any(r => r.Id == vehicleReservation.Id)).FirstOrDefault();
                newGuideReservationModel.Member = member;
                newGuideReservationModel.VehicleReservation = vehicleReservation;
                newGuideReservationModel.VehiclePackage = db.Vehicle_Package.Where(p => p.Id == vehicleReservation.ID_Vehicle_Package).FirstOrDefault();
                guideReservationModel.Add(newGuideReservationModel);

            }

            return View(guideReservationModel);
        }


        [HttpGet]
        public JsonResult Details(string Id)
        {
            var result = from vehicleReservation in db.Vehicle_Reservation
                         from member in vehicleReservation.Members
                         join vehiclePackage in db.Vehicle_Package on vehicleReservation.ID_Vehicle_Package equals vehiclePackage.Id
                         join vehicle in db.Vehicles on vehicleReservation.ID_Vehicle equals vehicle.Id
                         where vehicleReservation.Id == Id
                         select new
                         {
                             vehicleReservationId = vehicleReservation.Id,
                             memberName = member.Name,
                             email = member.Email,
                             startTime = vehicleReservation.Start_Time,
                             endTime = vehicleReservation.End_Time,
                             numberOfPeople = vehicleReservation.People_Number,
                             isPaid = vehicleReservation.IsPaid,
                             type = vehiclePackage.Type,
                             Photo = vehiclePackage.Photo,
                             totalPrice = vehicleReservation.TotalPrice,
                             area = vehiclePackage.Area
                         };

            var aa = from vehicleReservation in db.Vehicle_Reservation
                     from member in vehicleReservation.Members
                     join vehiclePacakge in db.Vehicle_Package on vehicleReservation.ID_Vehicle_Package equals vehiclePacakge.Id
                     where vehicleReservation.Id == Id
                     select new
                     {
                         vehicleReservationId = vehicleReservation.Id,
                         memberName = member.Name,
                         email = member.Email,
                         startTime = vehicleReservation.Start_Time,
                         endTime = vehicleReservation.End_Time,
                         numberOfPeople = vehicleReservation.People_Number,
                         isPaid = vehicleReservation.IsPaid,
                         Photo = vehiclePacakge.Photo,
                         type = vehiclePacakge.Type,
                         totalPrice = vehicleReservation.TotalPrice,
                         area = vehiclePacakge.Area
                     };

            if (result.Count() == 0)
            {
                return Json(aa, JsonRequestBehavior.AllowGet);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public string Update(Vehicle_Reservation vehicleReservation)
        {
            Vehicle_Reservation newVehicleReservation = db.Vehicle_Reservation.Where(r => r.Id == vehicleReservation.Id).FirstOrDefault();


            newVehicleReservation.IsPaid = vehicleReservation.IsPaid;
            newVehicleReservation.People_Number = vehicleReservation.People_Number;
            newVehicleReservation.TotalPrice = vehicleReservation.TotalPrice;
            newVehicleReservation.Type = vehicleReservation.Type;

            db.Entry(newVehicleReservation).State = System.Data.Entity.EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return "更新成功";
            }

            return "更新失败";
        }

        [HttpPost]
        public string Delete(string vehicleReservationId, string memberId)
        {
            Vehicle_Reservation vehicleReservation = db.Vehicle_Reservation.FirstOrDefault(v => v.Id == vehicleReservationId);
            Member member = vehicleReservation.Members.FirstOrDefault(m => m.Id == memberId);


            if (vehicleReservation != null && member != null)
            {
                vehicleReservation.Members.Remove(member);
                db.Vehicle_Reservation.Remove(vehicleReservation);
                if (db.SaveChanges() > 0)
                {
                    return "删除成功";
                }

            }

            return "删除失败";
        }
    }
}