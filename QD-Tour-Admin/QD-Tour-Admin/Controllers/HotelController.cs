using QD_Tour_Admin.Model;
using QD_Tour_Admin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Admin.Controllers
{
    public class HotelModel
    {
        public string Country { get; set; }
        public string Area { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }

    }

    public class HotelPriceModel
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public decimal SingleRoomPrice { get; set; }
        public decimal DoubleRoomPrice { get; set; }
        public decimal OtherRoomPrice { get; set; }
        public string HotelId { get; set; }
        public int CanReserve { get; set; }
    }

    [CustomAuthorize]
    public class HotelController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();
        // GET: Hotel
        public ActionResult Index()
        {
            var hotel = db.Hotels.ToList();
            return View(hotel);
        }

        public string Add(HotelModel hotel)
        {
            Hotel newHotel = new Hotel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = hotel.Name,
                Country = hotel.Country,
                Address = hotel.Address,
                ContactNumber = hotel.ContactNumber,
                Area = hotel.Area
            };

            db.Hotels.Add(newHotel);

            if (db.SaveChanges() > 0)
            {
                return "添加成功";
            }

            return "添加失败";
        }

        public ActionResult Details(string Id)
        {
            var hotelPrices = db.Hotel_Price.Where(h => h.ID_Hotel == Id).ToList();
            return View(hotelPrices);
        }

        [HttpGet]
        public JsonResult Edit(string Id)
        {

            var hotel = db.Hotels.Where(v => v.Id == Id).FirstOrDefault();

            return Json(hotel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Update(Hotel hotel)
        {
            Hotel newHotel = db.Hotels.FirstOrDefault(h => h.Id == hotel.Id);

            newHotel.Address = hotel.Address;
            newHotel.Name = hotel.Name;
            newHotel.Country = hotel.Country;
            newHotel.ContactNumber = hotel.ContactNumber;
            newHotel.Area = hotel.Area;

            db.Entry(newHotel).State = System.Data.Entity.EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return "更新成功";
            }

            return "更新失败";
        }

        [HttpPost]
        public string Delete(string Id)
        {
            Hotel hotel = db.Hotels.FirstOrDefault(v => v.Id == Id);
            var hotelPrices = db.Hotel_Price.Where(h => h.ID_Hotel == Id).ToList();

            if (hotelPrices != null)
            {
                foreach (var hotelPrice in hotelPrices)
                {
                    db.Hotel_Price.Remove(hotelPrice);
                }

                if (hotel != null)
                {
                    db.Hotels.Remove(hotel);
                    if (db.SaveChanges() > 0)
                    {
                        return "删除成功";
                    }

                }
            }

            return "删除失败";
        }

        [HttpPost]
        public string AddHotelPrice(HotelPriceModel hotelPriceModel)
        {
            Hotel_Price newHotelPrice = new Hotel_Price()
            {
                Id = Guid.NewGuid().ToString(),
                Date = hotelPriceModel.Date,
                SingleRoomPrice = hotelPriceModel.SingleRoomPrice,
                DoulbeRoomPrice = hotelPriceModel.DoubleRoomPrice,
                OtherRoomPrice = hotelPriceModel.OtherRoomPrice,               
                ID_Hotel = hotelPriceModel.HotelId
            };


            if (newHotelPrice.SingleRoomPrice != null || newHotelPrice.DoulbeRoomPrice != null || newHotelPrice.OtherRoomPrice != null)
            {
                newHotelPrice.CanReserve = 1;
            }
            else
            {
                newHotelPrice.CanReserve = 0;
            }

            db.Hotel_Price.Add(newHotelPrice);

            if(db.SaveChanges() > 0)
            {
                return "添加成功";
            }

            return "添加失败";
        }

        [HttpGet]
        public JsonResult EditHotelPrice(string Id)
        {

            var hotelPrice = from hp in db.Hotel_Price
                             where hp.Id == Id
                             select new
                             {
                                 Id = hp.Id,
                                 HotelId = hp.ID_Hotel,
                                 Date = hp.Date,
                                 SingleRoomPrice = hp.SingleRoomPrice,
                                 DoubleRoomPrice = hp.DoulbeRoomPrice,
                                 OtherRoomPrice = hp.OtherRoomPrice,
                                 CanReserve = hp.CanReserve
                             };

            return Json(hotelPrice, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string UpdateHotelPrice(HotelPriceModel hotelPrice)
        {
            Hotel_Price newHotelPrice = db.Hotel_Price.FirstOrDefault(hp => hp.Id == hotelPrice.Id);

            newHotelPrice.SingleRoomPrice = hotelPrice.SingleRoomPrice;
            newHotelPrice.DoulbeRoomPrice = hotelPrice.DoubleRoomPrice;
            newHotelPrice.OtherRoomPrice = hotelPrice.OtherRoomPrice;
            newHotelPrice.Date = hotelPrice.Date;
            newHotelPrice.ID_Hotel = hotelPrice.HotelId;
            

            db.Entry(newHotelPrice).State = System.Data.Entity.EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return "更新成功";
            }

            return "更新失败";
        }

        [HttpGet]
        public JsonResult GetAllHotels()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var hotels = db.Hotels.ToList();
            return Json(hotels, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetHotelByCountryAndArea(string Country, string Area)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var hotel = db.Hotels.Where(h => h.Country == Country).Where(h => h.Area == Area).FirstOrDefault();
            return Json(hotel, JsonRequestBehavior.AllowGet);
        }

    }
}