using QD_Tour_Admin.Model;
using QD_Tour_Admin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QD_Tour_Admin.Controllers
{
    public class LoginController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public string Login(string name, string password)
        {
            var passwords = MD5Service.Encrypt(password);

            var admin = db.Admins.FirstOrDefault(a => a.Name == name && a.Password == passwords);

            if (admin != null)
            {
                //Session.
                Session["adminLogedIn"] = true;
                Session["adminId"] = admin.Name;
                Session.Timeout = 60;

                return "Login Success";
            }

            return "登录失败";
        }
    }
}