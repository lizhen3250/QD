using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QD_Tour_Admin.Model;
using QD_Tour_Admin.Service;

namespace QD_Tour_Admin.Controllers
{
    public class GolfModel
    {
        public string City { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }

    }

    public class GolfPriceModel
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public decimal EighteenHolePrice { get; set; }
        public decimal TwentySevenHolePrice { get; set; }
        public decimal ThirtySixHolePrice { get; set; }
        public string GolfId { get; set; }
    }

    [CustomAuthorize]
    public class GolfController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();

        // GET: Golf
        public ActionResult Index()
        {
            var golfs = db.Golves.ToList();

            return View(golfs);
        }

        [HttpPost]
        public string Add(GolfModel golf)
        {
            Golf newGolf = new Golf()
            {
                Id = Guid.NewGuid().ToString(),
                Name = golf.Name,
                City = golf.City,
                Address = golf.Address

            };

            db.Golves.Add(newGolf);

            if (db.SaveChanges() > 0)
            {
                return "添加成功";
            }

            return "添加失败";
        }

        public ActionResult Details(string Id)
        {
            var golfPrices = db.Golf_Price.Where(h => h.Golf_Id == Id).ToList();
            return View(golfPrices);
        }

        [HttpGet]
        public JsonResult Edit(string Id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var golf = db.Golves.Where(v => v.Id == Id).FirstOrDefault();

            return Json(golf, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string AddGolfPrice(GolfPriceModel golfPriceModel)
        {
            Golf_Price newGolfPrice = new Golf_Price()
            {
                Id = Guid.NewGuid().ToString(),
                Date = golfPriceModel.Date,
                Eighteen_Hole_Price = golfPriceModel.EighteenHolePrice,
                TwentySeven_Hole_Price = golfPriceModel.TwentySevenHolePrice,
                ThirySix_Hole_Price = golfPriceModel.ThirtySixHolePrice,
                Golf_Id = golfPriceModel.GolfId
            };

            db.Golf_Price.Add(newGolfPrice);

            if (db.SaveChanges() > 0)
            {
                return "添加成功";
            }

            return "添加失败";
        }

        [HttpPost]
        public string Update(Golf golf)
        {
            Golf newGolf = db.Golves.FirstOrDefault(h => h.Id == golf.Id);

            newGolf.Address = golf.Address;
            newGolf.Name = golf.Name;
            newGolf.City = golf.City;

            db.Entry(newGolf).State = System.Data.Entity.EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return "更新成功";
            }

            return "更新失败";
        }

        [HttpPost]
        public string Delete(string Id)
        {
            Golf golf = db.Golves.FirstOrDefault(v => v.Id == Id);
            var golfPrices = db.Golf_Price.Where(h => h.Golf_Id == Id).ToList();

            if (golfPrices != null)
            {
                foreach (var golfPrice in golfPrices)
                {
                    db.Golf_Price.Remove(golfPrice);
                }

                if (golf != null)
                {
                    db.Golves.Remove(golf);
                    if (db.SaveChanges() > 0)
                    {
                        return "删除成功";
                    }

                }
            }

            return "删除失败";
        }

        [HttpGet]
        public JsonResult GetAllGolves()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var golves = db.Golves.ToList();
            return Json(golves, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGolfByCountryAndName(string Country, string Name)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var golf = db.Golves.Where(h => h.City == Country).Where(h => h.Name == Name).FirstOrDefault();
            return Json(golf, JsonRequestBehavior.AllowGet);
        }
    }
}