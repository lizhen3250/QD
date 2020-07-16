using QD_Tour_Web.Models;
using QD_Tour_Web.Services;
using System.Linq;
using System.Web.Mvc;

namespace QD_Tour_Web.Controllers
{
    public class UserDetails
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    public class LoginController : Controller
    {
        private QDTourWebEntities db = new QDTourWebEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string Login(UserDetails userDetails)
        {
            var password = MD5Service.Encrypt(userDetails.password);

            var member = db.Members.FirstOrDefault(m => m.Email == userDetails.email && m.Password == password);

            if (member != null && member.Varification == 0)
            {
                return "Please varify your email";
            }
            else if(member != null && member.Varification == 1)
            {
                //Session.
                Session["logedIn"] = true;
                Session["memberId"] = member.Id;
                Session.Timeout = 60;

                return "Login Success";
            }

            return "Login Failed";
        }
    }
}
