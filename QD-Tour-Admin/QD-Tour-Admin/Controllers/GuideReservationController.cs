using Newtonsoft.Json;
using QD_Tour_Admin.Model;
using QD_Tour_Admin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace QD_Tour_Admin.Controllers
{
    public class GuideReservationModel
    {
        public Guide_Package GuidePackages { get; set; }

        public Guide_Reservation GuideReservations { get; set; }

        public Member Member { get; set; }
    }

    [CustomAuthorize]
    public class GuideReservationController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();
        // GET: GuideReservation
        public ActionResult Index()
        {
            List<GuideReservationModel> guideReservationModel = new List<Controllers.GuideReservationModel>();

            var guideReservations = db.Guide_Reservation.ToList();

            foreach (var guideReservation in guideReservations)
            {
                GuideReservationModel newGuideReservationModel = new GuideReservationModel();
                var member = db.Members.Where(m => m.Guide_Reservation.Any(r => r.Id == guideReservation.Id)).FirstOrDefault();
                newGuideReservationModel.Member = member;
                newGuideReservationModel.GuideReservations = guideReservation;
                newGuideReservationModel.GuidePackages = db.Guide_Package.Where(p => p.Id == guideReservation.ID_Guide_Package).FirstOrDefault();
                guideReservationModel.Add(newGuideReservationModel);

            }

            return View(guideReservationModel);
        }

        [HttpGet]
        public JsonResult Details(string Id)
        {

            QDTourAdminEntities db = new QDTourAdminEntities();

            var result = from guideReservation in db.Guide_Reservation
                         from member in guideReservation.Members
                         join guidePackage in db.Guide_Package on guideReservation.ID_Guide_Package equals guidePackage.Id
                         join guide in db.Guides on guideReservation.ID_Guide equals guide.Id
                         where guideReservation.Id == Id
                         select new
                         {
                             guideReservationId = guideReservation.Id,
                             memberName = member.Name,
                             email = member.Email,
                             startTime = guideReservation.Start_Time,
                             endTime = guideReservation.End_Time,
                             numberOfPeople = guideReservation.People_Number,
                             isPaid = guideReservation.IsPaid,
                             guideId = guide.Id,
                             Photo = guidePackage.Photo,
                             totalPrice = guideReservation.TotalPrice,
                             destination = guidePackage.Destination
                         };

                var aa = from guideReservation in db.Guide_Reservation
                         from member in guideReservation.Members
                         join guidePackage in db.Guide_Package on guideReservation.ID_Guide_Package equals guidePackage.Id
                         where guideReservation.Id == Id
                         select new
                         {
                             guideReservationId = guideReservation.Id,
                             memberName = member.Name,
                             email = member.Email,
                             startTime = guideReservation.Start_Time,
                             endTime = guideReservation.End_Time,
                             numberOfPeople = guideReservation.People_Number,
                             isPaid = guideReservation.IsPaid,
                             Photo = guidePackage.Photo,
                             totalPrice = guideReservation.TotalPrice,
                             destination = guidePackage.Destination,
                         };
            
            if (result.Count() == 0)
            {
                return Json(aa, JsonRequestBehavior.AllowGet);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public string Update(Guide_Reservation guideReservation)
        {
            Guide_Reservation newGuideReservation = db.Guide_Reservation.Where(r => r.Id == guideReservation.Id).FirstOrDefault();

            if (guideReservation.ID_Guide == "choose")
            {
                newGuideReservation.ID_Guide = null;
                newGuideReservation.IsPaid = guideReservation.IsPaid;
                newGuideReservation.People_Number = guideReservation.People_Number;
                newGuideReservation.TotalPrice = guideReservation.TotalPrice;
            }
            else
            {
                newGuideReservation.ID_Guide = guideReservation.ID_Guide;
                newGuideReservation.IsPaid = guideReservation.IsPaid;
                newGuideReservation.People_Number = guideReservation.People_Number;
                newGuideReservation.TotalPrice = guideReservation.TotalPrice;
            }

            db.Entry(newGuideReservation).State = System.Data.Entity.EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return "更新成功";
            }

            return "更新失败";
        }

        [HttpPost]
        public string Delete(string guideReservationId, string memberId)
        {
            Guide_Reservation guideReservation = db.Guide_Reservation.FirstOrDefault(r => r.Id == guideReservationId);
            Member member = guideReservation.Members.FirstOrDefault(m => m.Id == memberId);
            

            if (guideReservation != null && member != null)
            {
                guideReservation.Members.Remove(member);
                db.Guide_Reservation.Remove(guideReservation);
                if (db.SaveChanges() > 0)
                {
                    return "删除成功";
                }

            }

            return "删除失败";
        }
    }
}