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
    }
}