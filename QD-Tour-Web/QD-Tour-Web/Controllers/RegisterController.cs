using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QD_Tour_Web.Models;
using System.Security.Cryptography;
using System.Text;
using QD_Tour_Web.Services;

namespace QD_Tour_Web.Controllers
{
    public class RegisterController : Controller
    {
        private QDTourWebEntities db = new QDTourWebEntities();
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string MemberExist(string email)
        {
            var member = db.Members.FirstOrDefault(m => m.Email == email | m.Varification == 0);

            if(member == null)
            {
                return "can use";
            }

            return "already exist"; 
        }

        [HttpPost]
        public string ValidationSuccess(Member memberData)
        {
            Member newMember = new Member()
            {
                Id = Guid.NewGuid().ToString(),
                Email = memberData.Email,
                Password =MD5Service.Encrypt(memberData.Password),
                Name = memberData.Name,
                Phone = memberData.Phone,
                Sex = memberData.Sex,
                Birth = memberData.Birth,
                Varification = memberData.Varification
            };

            db.Members.Add(newMember);
            if (db.SaveChanges() >0)
            {
                //发送邮箱验证
                //如果发送成功，return
                StringBuilder sb = new StringBuilder();
                sb.Append("안녕하세요");
                sb.Append("<br/>");
                sb.Append("청도투어에 오신걸 환영합니다.");
                sb.Append("<br/>");
                sb.Append("아래에 link를 클릭후 메일 인증 바랍니다");
                sb.Append("<br/>");
                sb.Append("http://localhost:53749/Register/Success?email=" + memberData.Email);
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("감사합니다");
                MailService mailService = new MailService(memberData.Email, "[청도투어]회원가입 인증", sb.ToString());
                if (mailService.isSent() == true)
                {
                    return "회원가입 완료";
                }
            }

            return "회원가입 실패";

        }

        public ActionResult EmailValidation()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }
        
    }
}