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
    public class TourPageController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();
        // GET: TourPage
        public ActionResult Index()
        {
            var tourPackage = db.Tour_Package.ToList();
            return View(tourPackage);
        }

        [HttpPost]
        public string Add(FormCollection formCollection)
        {
            string photoUrl = "", country = "", date = "", tourId = "", province = "", name = "", description = "";

            foreach (var key in formCollection.AllKeys)
            {
                province = formCollection["province"];
                description = formCollection["description"];
                country = formCollection["country"];
                tourId = formCollection["tourId"];
                date = formCollection["date"];
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

                        Tour_Package tourPackage = new Tour_Package()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = description,
                            Photo = "/Uploads/" + photoUrl,
                            Tour = db.Tours.FirstOrDefault(t => t.Id == tourId)
                        };

                        db.Tour_Package.Add(tourPackage);
                        if (db.SaveChanges() > 0)
                        {
                            file.SaveAs(fullPathUrl);
                            return "添加成功";
                        }
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

            return "添加失败";
        }

        [HttpGet]
        public JsonResult Edit(string Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = from tourPackage in db.Tour_Package
                         where tourPackage.Id == Id
                         select new
                         {
                             Id = tourPackage.Id,
                             Description = tourPackage.Description,
                             Tour = tourPackage.Tour,
                             Photo = tourPackage.Photo
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Update(FormCollection formCollection)
        {
            string id = "", photoUrl = "", province = "", date = "", tourId = "", country = "", name = "", description = "",
                                photo = "";
            StringBuilder allPhotoUrls = new StringBuilder();

            foreach (var key in formCollection.AllKeys)
            {
                id = formCollection["id"];
                province = formCollection["province"];
                description = formCollection["description"];
                date = formCollection["date"];
                tourId = formCollection["tourId"];
                country = formCollection["country"];
                name = formCollection["name"];
                photo = formCollection["photo"];
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
                        Tour_Package newTourPackage = db.Tour_Package.FirstOrDefault(g => g.Id == id);
                        Tour tour = db.Tours.FirstOrDefault(t => t.Id == tourId);
                        newTourPackage.Tour = tour;
                        newTourPackage.Photo = "/Uploads/" + photoUrl;
                        newTourPackage.Description = description;

                        db.Entry(newTourPackage).State = System.Data.Entity.EntityState.Modified;

                        if (db.SaveChanges() > 0)
                        {
                            file.SaveAs(fullPathUrl);
                            return "更新成功";
                        }
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
                Tour_Package newTourPackage = db.Tour_Package.FirstOrDefault(g => g.Id == id);
                Tour tour = db.Tours.FirstOrDefault(t => t.Id == tourId);
                newTourPackage.Tour = tour;
                newTourPackage.Photo = "/Uploads/" + photo;
                newTourPackage.Description = description;

                db.Entry(newTourPackage).State = System.Data.Entity.EntityState.Modified;

                if (db.SaveChanges() > 0)
                {
                    return "更新成功";
                }
            }

            return "更新失败";
        }

        [HttpPost]
        public string Delete(string Id)
        {
            Tour_Package tourPackage = db.Tour_Package.FirstOrDefault(g => g.Id == Id);
            List<Tour_Reservation> tourReservation = db.Tour_Reservation.Where(r => r.ID_Package == Id).ToList();

            if (tourReservation.Count != 0)
            {
                return "已有预约";
            }

            if (tourReservation != null)
            {
                db.Tour_Package.Remove(tourPackage);
                if (db.SaveChanges() > 0)
                {
                    return "删除成功";
                }

            }

            return "删除失败";
        }
    }
}