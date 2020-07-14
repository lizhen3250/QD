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
    }
}