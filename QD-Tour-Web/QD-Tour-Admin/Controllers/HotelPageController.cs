using QD_Tour_Admin.Model;
using QD_Tour_Admin.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Admin.Controllers
{
    [CustomAuthorize]
    public class HotelPageController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();
        // GET: HotelPage
        public ActionResult Index()
        {
            var hotelPackage = db.Hotel_Package.ToList();
            return View(hotelPackage);
        }

        [HttpPost]
        public string Add(FormCollection formCollection)
        {
            string photoUrl = "", area = "", address = "", hotelId = "", country = "", phone = "", name = "", description = "";
            StringBuilder allPhotoUrls = new StringBuilder();

            foreach (var key in formCollection.AllKeys)
            {
                area = formCollection["area"];
                description = formCollection["description"];
                address = formCollection["address"];
                hotelId = formCollection["hotelId"];
                country = formCollection["country"];
                phone = formCollection["phone"];
                name = formCollection["name"];
            }

            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            photoUrl = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            photoUrl = file.FileName;
                        }

                        if (!System.IO.Directory.Exists(Server.MapPath("~/Uploads/")))
                        {

                            System.IO.Directory.CreateDirectory(Server.MapPath("~/Uploads/"));

                        }

                        // Get the complete folder path and store the file inside it.  
                        string fullPathUrl = Path.Combine(Server.MapPath("~/Uploads/"), photoUrl);
                        file.SaveAs(fullPathUrl);
                        allPhotoUrls.Append(photoUrl + ";");
                    }
                    // Returns message that successfully uploaded  
                    //return "File Uploaded Successfully!";
                }
                catch (DbEntityValidationException dbEx)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            sb.Append("Property:" + validationError.PropertyName + "  Error: " + validationError.ErrorMessage);
                        }
                    }

                    return sb.ToString();
                }
            }
            else
            {
                return "No files selected.";
            }

            string[] photos = allPhotoUrls.ToString().Split(';');

            Hotel_Package hotelPackage = new Hotel_Package()
            {
                Id = Guid.NewGuid().ToString(),
                Description = description,
                Area = area,
                Country = country,
                Photo = "/Uploads/" + photos[0],
                SingRommPhoto = "/Uploads/" + photos[1],
                DoubleRoomPhoto = "/Uploads/" + photos[2],
                OtherRoomPhoto = "/Uploads/" + photos[3],
                Hotel = db.Hotels.Where(h => h.Id == hotelId).FirstOrDefault(),
                ID_Hotel = hotelId
            };

            db.Hotel_Package.Add(hotelPackage);

            if (db.SaveChanges() > 0)
            {
               
                return "添加成功";
            }

            return "添加失败";
        }

        [HttpGet]
        public JsonResult Edit(string Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = from hotelPackage in db.Hotel_Package
                         where hotelPackage.Id == Id
                         select new
                         {
                             Id = hotelPackage.Id,
                             Description = hotelPackage.Description,
                             Country = hotelPackage.Country,
                             Area = hotelPackage.Area,
                             Photo = hotelPackage.Photo,
                             SingleRoomPhoto = hotelPackage.SingRommPhoto,
                             DoubleRoomPhoto = hotelPackage.DoubleRoomPhoto,
                             OtherRoomPhoto =hotelPackage.OtherRoomPhoto,
                             HotelObject = hotelPackage.Hotel
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Update(FormCollection formCollection)
        {
            string id = "", photoUrl = "", area = "", address = "", hotelId = "", country = "", phone = "", name = "", description = "",
                                photo = "", singleRoomPhoto = "", doubleRoomPhoto = "", otherRoomPhoto = "";
            StringBuilder allPhotoUrls = new StringBuilder();

            foreach (var key in formCollection.AllKeys)
            {
                id = formCollection["id"];
                area = formCollection["area"];
                description = formCollection["description"];
                address = formCollection["address"];
                hotelId = formCollection["hotelId"];
                country = formCollection["country"];
                phone = formCollection["phone"];
                name = formCollection["name"];
                photo = formCollection["photo"];
                singleRoomPhoto = formCollection["singleRoomPhoto"];
                doubleRoomPhoto = formCollection["doubleRoomPhoto"];
                otherRoomPhoto = formCollection["otherRoomPhoto"];
            }

            

            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            photoUrl = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            photoUrl = file.FileName;
                        }

                        if (!System.IO.Directory.Exists(Server.MapPath("~/Uploads/")))
                        {

                            System.IO.Directory.CreateDirectory(Server.MapPath("~/Uploads/"));

                        }

                        // Get the complete folder path and store the file inside it.  
                        string fullPathUrl = Path.Combine(Server.MapPath("~/Uploads/"), photoUrl);
                        file.SaveAs(fullPathUrl);
                        allPhotoUrls.Append(photoUrl + ";");                       
                    }
                    // Returns message that successfully uploaded  
                    //return "File Uploaded Successfully!";
                }
                catch (DbEntityValidationException dbEx)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            sb.Append("Property:" + validationError.PropertyName + "  Error: " + validationError.ErrorMessage);
                        }
                    }

                    return sb.ToString();
                }
            }
            else
            {
                Hotel_Package newHotelPackages = db.Hotel_Package.FirstOrDefault(g => g.Id == id);
                Hotel hotels = db.Hotels.Where(h => h.Id == hotelId).FirstOrDefault();

                newHotelPackages.Country = country;
                newHotelPackages.Area = area;
                newHotelPackages.Photo = "/Uploads/" + photo;
                newHotelPackages.SingRommPhoto = "/Uploads/" + singleRoomPhoto;
                newHotelPackages.DoubleRoomPhoto = "/Uploads/" + doubleRoomPhoto;
                newHotelPackages.OtherRoomPhoto = "/Uploads/" + otherRoomPhoto;
                newHotelPackages.Hotel = hotels;
                newHotelPackages.Description = description;

                db.Entry(newHotelPackages).State = System.Data.Entity.EntityState.Modified;

                if (db.SaveChanges() > 0)
                {
                    return "更新成功";
                }
            }

            string[] photos = allPhotoUrls.ToString().Split(';');

            Hotel_Package newHotelPackage = db.Hotel_Package.FirstOrDefault(g => g.Id == id);
            Hotel hotel = db.Hotels.Where(h => h.Id == hotelId).FirstOrDefault();

            newHotelPackage.Country = country;
            newHotelPackage.Area = area;
            newHotelPackage.Photo = "/Uploads/" + photos[0];
            newHotelPackage.SingRommPhoto = "/Uploads/" + photos[1];
            newHotelPackage.DoubleRoomPhoto = "/Uploads/" + photos[2];
            newHotelPackage.OtherRoomPhoto = "/Uploads/" + photos[3];
            newHotelPackage.Hotel = hotel;
            newHotelPackage.Description = description;

            db.Entry(newHotelPackage).State = System.Data.Entity.EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return "更新成功";
            }

            return "更新失败";
        }

        [HttpPost]
        public string Delete(string Id)
        {
            Hotel_Package hotelPackage = db.Hotel_Package.FirstOrDefault(g => g.Id == Id);
            List<Hotel_Reservation> hotelReservation = db.Hotel_Reservation.Where(r => r.ID_Package == Id).ToList();

            if (hotelReservation.Count != 0)
            {
                return "已有预约";
            }

            if (hotelPackage != null)
            {
                db.Hotel_Package.Remove(hotelPackage);
                if (db.SaveChanges() > 0)
                {
                    return "删除成功";
                }

            }

            return "删除失败";
        }
    }
}