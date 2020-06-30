using System.Linq;
using System.Web.Mvc;
using QD_Tour_Web.Models;

namespace QD_Tour_Web.Controllers
{
    public class MemberController : Controller
    {
        private QDTourWebEntities db = new QDTourWebEntities();
        // GET: Member
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(string Id)
        {
            var member = db.Members.FirstOrDefault(m => m.Id == Id);
            return View(member);
        }

        [HttpPost]
        public JsonResult Check(string email)
        {
            var member = db.Members.FirstOrDefault(m => m.Email == email);

            if(member != null)
            {
                return Json(member, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpPost]
        public JsonResult Update(string email)
        {
            var member = db.Members.FirstOrDefault(m => m.Email == email);

            if (member != null)
            {
                member.Varification = 1;
                db.Entry(member).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(member, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpGet]
        public JsonResult GetMemberInfo(string Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var member = db.Members.FirstOrDefault(m => m.Id == Id);
            return Json(member, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTourReservation(string Id)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            var tourReservations = from tourReservation in db.Tour_Reservation
                                 where tourReservation.Members.Any(c => c.Id == Id)
                                 join tourPackage in db.Tour_Package on tourReservation.ID_Package equals tourPackage.Id
                                 join tour in db.Tours on tourPackage.ID_Tour equals tour.Id
                                 select new
                                 {
                                     country = tour.Course,
                                     province = tour.Province,
                                     name = tour.Name,
                                     startTime = tourReservation.StartTime,
                                     endTime = tourReservation.EndTime,
                                     totalPrice = tourReservation.TotalPrice,
                                     isPaid = tourReservation.IsPaid,
                                     photo = tourPackage.Photo,
                                     numberOfPeople = tourReservation.PeopleNumber
                                 };

            return Json(tourReservations.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGuideReservation(string Id)
        {
            var guideReservations = from guideReservation in db.Guide_Reservation
                                    where guideReservation.Members.Any(c => c.Id == Id)
                                    join guidePackage in db.Guide_Package on guideReservation.ID_Guide_Package equals guidePackage.Id
                                    select new
                                    {
                                        country = guidePackage.Destination,
                                        startTime = guideReservation.Start_Time,
                                        endTime = guideReservation.End_Time,
                                        numberOfPeople = guideReservation.People_Number,
                                        totalPrice = guideReservation.TotalPrice,
                                        isPaid = guideReservation.IsPaid
                                    };
            return Json(guideReservations.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetVehicleReservation(string Id)
        {
            var vehicleReservations = from vehicleReservation in db.Vehicle_Reservation
                                      where vehicleReservation.Members.Any(c => c.Id == Id)
                                      join vehiclePackage in db.Vehicle_Package on vehicleReservation.ID_Vehicle_Package equals vehiclePackage.Id
                                      select new
                                      {
                                          area = vehiclePackage.Area,
                                          type = vehicleReservation.Type,
                                          startTime = vehicleReservation.Start_Time,
                                          endTime = vehicleReservation.End_Time,
                                          numberOfPeople = vehicleReservation.People_Number,
                                          totalPrice = vehicleReservation.TotalPrice,
                                          isPaid = vehicleReservation.IsPaid
                                      };
            return Json(vehicleReservations.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetHotelReservation(string Id)
        {
            var hotelReservations = from hotelReservation in db.Hotel_Reservation
                                    where hotelReservation.Members.Any(c => c.Id == Id)
                                    join hotelPackage in db.Hotel_Package on hotelReservation.ID_Package equals hotelPackage.Id
                                    join hotel in db.Hotels on hotelPackage.ID_Hotel equals hotel.Id
                                    select new
                                    {
                                        country = hotelPackage.Country,
                                        area = hotelPackage.Area,
                                        type = hotelReservation.RoomType,
                                        startTime = hotelReservation.StartTime,
                                        endTime = hotelReservation.EndTime,
                                        name = hotel.Name,
                                        totalPrice = hotelReservation.TotalPrice,
                                        isPaid = hotelReservation.IsPaid
                                    };
            return Json(hotelReservations.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}