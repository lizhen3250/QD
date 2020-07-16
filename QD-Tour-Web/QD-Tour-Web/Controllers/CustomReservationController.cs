using QD_Tour_Web.Filters;
using QD_Tour_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Web.Controllers
{
    public class CustomTourReservationModel
    {
        public string MemberId { get; set; }

        public string Message { get; set; }
    }
    public class CustomReservationController : Controller
    {
        private QDTourWebEntities db = new QDTourWebEntities();
        // GET: CustomReservation
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeMember]
        [HttpPost]
        public string Add(CustomTourReservationModel reservationModel)
        {
            if (HttpContext.Session["logedIn"] == null)
            {
                return "need login";
            }

            var newCustomTourReservation = new CustomTour()
            {
                Id = Guid.NewGuid().ToString(),
                TotalPrice = 0m,
                MessageSentTime = DateTime.Now,
                IsPaid = 0,
                MemberId = reservationModel.MemberId,
                Message = reservationModel.Message,
                Member = db.Members.FirstOrDefault(m => m.Id == reservationModel.MemberId)
            };

            db.CustomTours.Add(newCustomTourReservation);

            if (db.SaveChanges() > 0)
            {
                return "添加成功";
            }

            return "添加失败";
        }
    }
}