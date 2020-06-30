using QD_Tour_Admin.Model;
using QD_Tour_Admin.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QD_Tour_Admin.Controllers
{
    [CustomAuthorize]
    public class GuidePageController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();
        // GET: GuidePage
        public ActionResult Index()
        {
            var GuidePackage = db.Guide_Package.ToList();
            return View(GuidePackage);
        }

        public string Add(FormCollection formCollection)
        {
            string photoUrl = "", destination = "", description = "";
            decimal price = decimal.MaxValue;
            DateTime startTime = new DateTime(), endTime = new DateTime();

            foreach (var key in formCollection.AllKeys)
            {
                destination = formCollection["destination"];
                description = formCollection["description"];
                price = Convert.ToDecimal(formCollection["price"]);
                startTime = Convert.ToDateTime(formCollection["startTime"]);
                endTime = Convert.ToDateTime(formCollection["endTime"]);
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
                        
                        Guide_Package guidePackage = new Guide_Package()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = description,
                            Destination = destination,
                            StartDate = Convert.ToDateTime(startTime),
                            EndDate = Convert.ToDateTime(endTime),
                            Price = Convert.ToDecimal(price),
                            Photo = "/Uploads/" + photoUrl
                        };

                        db.Guide_Package.Add(guidePackage);
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

            var result = from guidePackage in db.Guide_Package
                         where guidePackage.Id == Id
                         select new
                         {
                             Id = guidePackage.Id,
                             Destination = guidePackage.Destination,
                             Description = guidePackage.Description,
                             StartDate = guidePackage.StartDate,
                             EndDate = guidePackage.EndDate,
                             Price = guidePackage.Price,
                             Photo = guidePackage.Photo
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Update(FormCollection formCollection)
        {
            string id = "", photoUrl = "", photo = "", destination = "", description = "";
            decimal price = decimal.MaxValue;
            DateTime startTime = new DateTime(), endTime = new DateTime();

            foreach (var key in formCollection.AllKeys)
            {
                id = formCollection["id"];
                destination = formCollection["destination"];
                description = formCollection["description"];
                price = Convert.ToDecimal(formCollection["price"]);
                startTime = Convert.ToDateTime(formCollection["startTime"]);
                endTime = Convert.ToDateTime(formCollection["endTime"]);
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

                        Guide_Package newGuidePackage = db.Guide_Package.FirstOrDefault(g => g.Id == id);

                        newGuidePackage.Destination = destination;
                        newGuidePackage.EndDate = endTime;
                        newGuidePackage.Price = price;
                        newGuidePackage.Photo = "/Uploads/" + photoUrl;
                        newGuidePackage.StartDate = startTime;
                        newGuidePackage.Description = description;

                        db.Entry(newGuidePackage).State = System.Data.Entity.EntityState.Modified;

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
                Guide_Package newGuidePackage = db.Guide_Package.FirstOrDefault(g => g.Id == id);

                newGuidePackage.Destination = destination;
                newGuidePackage.EndDate = endTime;
                newGuidePackage.Price = price;
                newGuidePackage.Photo = "/Uploads/" + photo;
                newGuidePackage.StartDate = startTime;
                newGuidePackage.Description = description;

                db.Entry(newGuidePackage).State = System.Data.Entity.EntityState.Modified;

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
            Guide_Package guidePackage = db.Guide_Package.FirstOrDefault(g => g.Id == Id);
            List<Guide_Reservation> guideReservation = db.Guide_Reservation.Where(r => r.ID_Guide_Package == Id).ToList();

            if (guideReservation.Count != 0)
            {
                return "已有预约";
            }

            if (guidePackage != null)
            {
                db.Guide_Package.Remove(guidePackage);
                if (db.SaveChanges() > 0 )
                {
                    return "删除成功";
                }

            }

            return "删除失败";
        }
    }
}