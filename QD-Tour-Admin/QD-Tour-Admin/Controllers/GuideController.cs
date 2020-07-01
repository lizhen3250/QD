using QD_Tour_Admin.Model;
using QD_Tour_Admin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Admin.Controllers
{
    [CustomAuthorize]
    public class GuideController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();
        // GET: Guide
        public ActionResult Index()
        {
            List<Guide> guideInfo = db.Guides.ToList();
            return View(guideInfo);
        }

        [HttpPost]
        public string Add(string name)
        {
            Guide guide = new Guide()
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                StartTime = DateTime.Now
            };

            db.Guides.Add(guide);

            if (db.SaveChanges() > 0)
            {
                return "添加成功";
            }

            return "添加失败";
        }

        [HttpGet]
        public JsonResult GetAllGuides()
        {
            var guideInfo = from guide in db.Guides
                            select new
                            {
                                Id = guide.Id,
                                Name = guide.Name
                            };

            return Json(guideInfo.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetReservationDetails(string guideId)
        {
            var reservations = from r in db.Guide_Reservation
                               where r.ID_Guide == guideId
                               select new
                               {
                                   Id = r.Id
                               };
            

            return Json(reservations.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(string guideId)
        {
            var reservations = db.Guide_Reservation.Where(r => r.ID_Guide == guideId).ToList();
                               


            return Json(reservations, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Delete(string guideId)
        {
            Guide guide = db.Guides.FirstOrDefault(v => v.Id == guideId);

            if (guide != null)
            {
                db.Guides.Remove(guide);
                if (db.SaveChanges() > 0)
                {
                    return "删除成功";
                }
            }



            //if (hotelPrices != null)
            //{
            //    foreach (var hotelPrice in hotelPrices)
            //    {
            //        db.Hotel_Price.Remove(hotelPrice);
            //    }

            //    if (hotel != null)
            //    {
            //        db.Hotels.Remove(hotel);
            //        if (db.SaveChanges() > 0)
            //        {
            //            return "删除成功";
            //        }

            //    }
            //}

            return "删除失败";
        }
    }
}