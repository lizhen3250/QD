using QD_Tour_Admin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Admin.Controllers
{
    public class GolfReservationModel
    {
        public Golf_Package GolfPackage { get; set; }

        public Golf_Reservation GolfReservation { get; set; }

        public Member Member { get; set; }
    }
    public class GolfReservationController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();
        // GET: HotelReservation
        public ActionResult Index()
        {
            List<GolfReservationModel> golfReservationModel = new List<Controllers.GolfReservationModel>();

            var golfReservations = db.Golf_Reservation.ToList();

            foreach (var golflReservation in golfReservations)
            {
                GolfReservationModel newGolfReservationModel = new GolfReservationModel();
                var member = db.Members.Where(m => m.Golf_Reservation.Any(r => r.Id == golflReservation.Id)).FirstOrDefault();
                newGolfReservationModel.Member = member;
                newGolfReservationModel.GolfReservation = golflReservation;
                newGolfReservationModel.GolfPackage = db.Golf_Package.Where(p => p.Id == golflReservation.Golf_Package_Id).FirstOrDefault();
                golfReservationModel.Add(newGolfReservationModel);

            }

            return View(golfReservationModel);
        }

        [HttpPost]
        public string Delete(string golfReservationId, string memberId)
        {
            Golf_Reservation golfReservation = db.Golf_Reservation.FirstOrDefault(v => v.Id == golfReservationId);
            Member member = golfReservation.Members.FirstOrDefault(m => m.Id == memberId);


            if (golfReservation != null && member != null)
            {
                golfReservation.Members.Remove(member);
                db.Golf_Reservation.Remove(golfReservation);
                if (db.SaveChanges() > 0)
                {
                    return "删除成功";
                }

            }

            return "删除失败";
        }
    }
}