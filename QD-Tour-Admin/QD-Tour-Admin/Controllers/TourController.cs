using QD_Tour_Admin.Model;
using QD_Tour_Admin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Admin.Controllers
{
    public class TourModel
    {
        public string Province { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
        public int TourDate { get; set; }

    }

    public class TourPriceModel
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string TourId { get; set; }

        public decimal Price { get; set; }
    }

    [CustomAuthorize]
    public class TourController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();
        // GET: Tour
        public ActionResult Index()
        {
            var tours = db.Tours.ToList();
            return View(tours);
        }

        public ActionResult Details(string Id)
        {
            var tourPrices = db.Tour_Price.Where(h => h.ID_Tour == Id).ToList();
            return View(tourPrices);
        }

        public string Add(TourModel tour)
        {
            Tour newTour = new Tour()
            {
                Id = Guid.NewGuid().ToString(),
                Name = tour.Name,
                Course = tour.Country,
                Province = tour.Province,
                TourDate = tour.TourDate,                
            };

            db.Tours.Add(newTour);

            if (db.SaveChanges() > 0)
            {
                return "添加成功";
            }

            return "添加失败";
        }

        [HttpGet]
        public JsonResult Edit(string Id)
        {

            var tour = db.Tours.Where(v => v.Id == Id).FirstOrDefault();

            return Json(tour, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Update(Tour tour)
        {
            Tour newTour = db.Tours.FirstOrDefault(h => h.Id == tour.Id);

            newTour.Province = tour.Province;
            newTour.Name = tour.Name;
            newTour.Course = tour.Course;
            newTour.TourDate = tour.TourDate;

            db.Entry(newTour).State = System.Data.Entity.EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return "更新成功";
            }

            return "更新失败";
        }

        [HttpPost]
        public string Delete(string Id)
        {
            Tour tour = db.Tours.FirstOrDefault(v => v.Id == Id);
            var tourPrices = db.Tour_Price.Where(t => t.ID_Tour == Id).ToList();

            if (tourPrices != null)
            {
                foreach(var tourPrice in tourPrices)
                {
                    db.Tour_Price.Remove(tourPrice);
                }

                if (tour != null)
                {
                    db.Tours.Remove(tour);
                    if (db.SaveChanges() > 0)
                    {
                        return "删除成功";
                    }

                }
            }

            return "删除失败";
        }

        [HttpPost]
        public string AddTourPrice(TourPriceModel tourPriceModel)
        {
            Tour_Price newTourPrice = new Tour_Price()
            {
                Id = Guid.NewGuid().ToString(),
                Date = tourPriceModel.Date,
                Price = tourPriceModel.Price,
                ID_Tour = tourPriceModel.TourId
            };

            db.Tour_Price.Add(newTourPrice);

            if (db.SaveChanges() > 0)
            {
                return "添加成功";
            }

            return "添加失败";
        }

        [HttpGet]
        public JsonResult EditTourPrice(string Id)
        {

            var tourPrice = from tp in db.Tour_Price
                             where tp.Id == Id
                             select new
                             {
                                 Id = tp.Id,
                                 TourId = tp.ID_Tour,
                                 Date = tp.Date,
                                 Price = tp.Price
                             };

            return Json(tourPrice, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string UpdateTourPrice(TourPriceModel tourPrice)
        {
            Tour_Price newTourPrice = db.Tour_Price.FirstOrDefault(tp => tp.Id == tourPrice.Id);

            newTourPrice.Date = tourPrice.Date;
            newTourPrice.ID_Tour = tourPrice.TourId;
            newTourPrice.Price = tourPrice.Price;


            db.Entry(newTourPrice).State = System.Data.Entity.EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return "更新成功";
            }

            return "更新失败";
        }

        [HttpGet]
        public JsonResult GetAllTours()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var tours = db.Tours.ToList();
            return Json(tours, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTourByProvince(string Province)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var tours = db.Tours.Where(h => h.Province == Province).ToList();
            return Json(tours, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public JsonResult GetTourByProvinceAndCountry(string Province, string Country)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var tours = db.Tours.Where(h => h.Province == Province).Where(h => h.Course == Country).ToList();
            return Json(tours, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTour(string Province, string Country, string Name)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var tour = db.Tours.Where(h => h.Province == Province).Where(h => h.Course == Country).Where(h => h.Name == Name).FirstOrDefault();
            return Json(tour, JsonRequestBehavior.AllowGet);
        }
    }
}