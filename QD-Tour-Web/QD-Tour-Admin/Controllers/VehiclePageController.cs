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
    public class VehiclePageController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();
        // GET: VehiclePage
        public ActionResult Index()
        {
            var vehiclePackage = db.Vehicle_Package.ToList();
            return View(vehiclePackage);
        }

        public string Add(FormCollection formCollection)
        {
            string photoUrl = "", area = "", description = "", type = "";
            decimal price = decimal.MaxValue;

            foreach (var key in formCollection.AllKeys)
            {
                area = formCollection["area"];
                description = formCollection["description"];
                price = Convert.ToDecimal(formCollection["price"]);
                type = formCollection["type"];
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

                        Vehicle_Package vehiclePackage = new Vehicle_Package()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = description,
                            Type = type,
                            Area = area,
                            Price = Convert.ToDecimal(price),
                            Photo = "/Uploads/" + photoUrl
                        };

                        db.Vehicle_Package.Add(vehiclePackage);
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

            var result = from vehiclePackage in db.Vehicle_Package
                         where vehiclePackage.Id == Id
                         select new
                         {
                             Id = vehiclePackage.Id,
                             Description = vehiclePackage.Description,
                             Type= vehiclePackage.Type,
                             Area = vehiclePackage.Area,
                             Price = vehiclePackage.Price,
                             Photo = vehiclePackage.Photo
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Update(FormCollection formCollection)
        {
            string id = "", photoUrl = "", area = "", description = "", type = "", photo="";
            decimal price = decimal.MaxValue;

            foreach (var key in formCollection.AllKeys)
            {
                id = formCollection["id"];
                area = formCollection["area"];
                description = formCollection["description"];
                price = Convert.ToDecimal(formCollection["price"]);
                type = formCollection["type"];
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

                        Vehicle_Package newVehiclePackage = db.Vehicle_Package.FirstOrDefault(g => g.Id == id);

                        newVehiclePackage.Area = area;
                        newVehiclePackage.Price = price;
                        newVehiclePackage.Photo = "/Uploads/" + photoUrl;
                        newVehiclePackage.Description = description;
                        newVehiclePackage.Type = type;

                        db.Entry(newVehiclePackage).State = System.Data.Entity.EntityState.Modified;

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
                Vehicle_Package newVehiclePackage = db.Vehicle_Package.FirstOrDefault(g => g.Id == id);

                newVehiclePackage.Area = area;
                newVehiclePackage.Price = price;
                newVehiclePackage.Photo = "/Uploads/" + photo;
                newVehiclePackage.Description = description;
                newVehiclePackage.Type = type;

                db.Entry(newVehiclePackage).State = System.Data.Entity.EntityState.Modified;

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
            Vehicle_Package vehiclePacakge = db.Vehicle_Package.FirstOrDefault(g => g.Id == Id);
            List<Vehicle_Reservation> vehicleReservation = db.Vehicle_Reservation.Where(r => r.ID_Vehicle_Package == Id).ToList();

            if (vehicleReservation.Count != 0)
            {
                return "已有预约";
            }

            if (vehiclePacakge != null)
            {
                db.Vehicle_Package.Remove(vehiclePacakge);
                if (db.SaveChanges() > 0)
                {
                    return "删除成功";
                }

            }

            return "删除失败";
        }


    }
}