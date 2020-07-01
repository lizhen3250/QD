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
    public class VehicleController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();
        // GET: Vehicle
        public ActionResult Index()
        {
            var vehicle = db.Vehicles.ToList();
            return View(vehicle);
        }

        public string Add(FormCollection formCollection)
        {
            string photoUrl = "", type = "", area = "";
            decimal price = decimal.MaxValue;

            foreach (var key in formCollection.AllKeys)
            {
                type = formCollection["type"];
                area = formCollection["area"];
                price = Convert.ToDecimal(formCollection["price"]);
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

                        Vehicle vehicle = new Vehicle()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Type = type,
                            Area = area,
                            Price = Convert.ToDecimal(price),
                            Photo = "/Uploads/" + photoUrl
                        };

                        db.Vehicles.Add(vehicle);
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

            var vehicle = db.Vehicles.Where(v => v.Id == Id).FirstOrDefault();

            return Json(vehicle, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Update(FormCollection formCollection)
        {
            string id ="",  photoUrl = "", type = "", area = "", photo="";
            decimal price = decimal.MaxValue;

            foreach (var key in formCollection.AllKeys)
            {
                id = formCollection["id"];
                type = formCollection["type"];
                area = formCollection["area"];
                price = Convert.ToDecimal(formCollection["price"]);
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

                        Vehicle vehicle = db.Vehicles.Where(v => v.Id == id).FirstOrDefault();

                        vehicle.Area = area;
                        vehicle.Type = type;
                        vehicle.Photo = "/Uploads/" + photoUrl;
                        vehicle.Price = price;


                        db.Entry(vehicle).State = System.Data.Entity.EntityState.Modified;

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
                Vehicle vehicle = db.Vehicles.Where(v => v.Id == id).FirstOrDefault();

                vehicle.Area = area;
                vehicle.Type = type;
                vehicle.Photo = "/Uploads/" + photo;
                vehicle.Price = price;


                db.Entry(vehicle).State = System.Data.Entity.EntityState.Modified;

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
            Vehicle vehicle = db.Vehicles.FirstOrDefault(v => v.Id == Id);

            if (vehicle != null)
            {
                db.Vehicles.Remove(vehicle);
                if (db.SaveChanges() > 0)
                {
                    return "删除成功";
                }

            }

            return "删除失败";
        }
    }
}