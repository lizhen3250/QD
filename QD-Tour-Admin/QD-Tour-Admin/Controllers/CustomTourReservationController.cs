using QD_Tour_Admin.Model;
using QD_Tour_Admin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Admin.Controllers
{
    public class CustomTourReservationModel
    {
        public CustomTour CustomTourReservation { get; set; }

        public Member Member { get; set; }
    }

    [CustomAuthorize]
    public class CustomTourReservationController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();

        // GET: Golf
        public ActionResult Index()
        {
            List<CustomTourReservationModel> customTourReservationModel = new List<Controllers.CustomTourReservationModel>();

            var customTourReservations = db.CustomTours.ToList();

            foreach (var customTourReservation in customTourReservations)
            {
                CustomTourReservationModel newCustomTourReservationModel = new CustomTourReservationModel();
                var member = db.Members.Where(m => m.CustomTours.Any(r => r.Id == customTourReservation.Id)).FirstOrDefault();
                newCustomTourReservationModel.Member = member;
                newCustomTourReservationModel.CustomTourReservation = customTourReservation;
                customTourReservationModel.Add(newCustomTourReservationModel);

            }

            return View(customTourReservationModel);
        }

        [HttpGet]
        public JsonResult Details(string Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var result = from customTourReservation in db.CustomTours
                         join member in db.Members on customTourReservation.MemberId equals member.Id
                         where customTourReservation.Id == Id
                         select new
                         {
                             Id = customTourReservation.Id,
                             IsPaid = customTourReservation.IsPaid,
                             Member = customTourReservation.Member,
                             MemberId = customTourReservation.Member.Id,
                             Message = customTourReservation.Message,
                             MessageSentTime = customTourReservation.MessageSentTime,
                             TotalPrice = customTourReservation.TotalPrice
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Update(CustomTour customTourReservation)
        {
            CustomTour newCustomTourReservation = db.CustomTours.Where(r => r.Id == customTourReservation.Id).FirstOrDefault();

            newCustomTourReservation.IsPaid = customTourReservation.IsPaid;
            newCustomTourReservation.TotalPrice = customTourReservation.TotalPrice;

            db.Entry(newCustomTourReservation).State = System.Data.Entity.EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return "更新成功";
            }

            return "更新失败";
        }

        [HttpPost]
        public string Delete(string customTourReservationId, string memberId)
        {
            CustomTour customTourReservation = db.CustomTours.FirstOrDefault(v => v.Id == customTourReservationId);

            if (customTourReservation != null)
            {
                db.CustomTours.Remove(customTourReservation);
                if (db.SaveChanges() > 0)
                {
                    return "删除成功";
                }

            }

            return "删除失败";
        }
    }
}