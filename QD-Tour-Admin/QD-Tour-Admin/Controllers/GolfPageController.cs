using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using QD_Tour_Admin.Model;
using QD_Tour_Admin.Service;

namespace QD_Tour_Admin.Controllers
{
    [CustomAuthorize]
    public class GolfPageController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();
        // GET: HotelPage
        public ActionResult Index()
        {
            var golfPackage = db.Golf_Package.ToList();
            return View(golfPackage);
        }

        [HttpPost]
        public string Add(FormCollection formCollection)
        {
            string photoUrl = "", name = "", address = "", golfId = "", country = "", description = "", photoLabel = "";

            foreach (var key in formCollection.AllKeys)
            {
                name = formCollection["name"];
                description = formCollection["description"];
                address = formCollection["address"];
                golfId = formCollection["golfId"];
                country = formCollection["country"];
                photoLabel = formCollection["photoLabel"];
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

            Golf_Package golfPackage = new Golf_Package()
            {
                Id = Guid.NewGuid().ToString(),
                Description = description,
                Golf_Id = golfId,
                Photo = "/Uploads/" + photoUrl,
                Golf = db.Golves.Where(h => h.Id == golfId).FirstOrDefault(),
            };

            db.Golf_Package.Add(golfPackage);

            if (db.SaveChanges() > 0)
            {
                return "添加成功";
            }

            return "添加失败";
        }
    }
}