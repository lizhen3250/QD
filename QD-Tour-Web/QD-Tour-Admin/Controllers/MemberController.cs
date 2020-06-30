using QD_Tour_Admin.Model;
using QD_Tour_Admin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QD_Tour_Admin.Controllers
{
    [CustomAuthorize]
    public class MemberController : Controller
    {
        private QDTourAdminEntities db = new QDTourAdminEntities();
        // GET: Member
        public ActionResult Index()
        {
            List<Member> member = db.Members.ToList();
            return View(member);
        }

        public string UpdateMemberById(Member member)
        {
            var result = db.Members.FirstOrDefault(m => m.Id == member.Id);
            if (result != null)
            {
                try
                {
                    db.Members.Attach(result);
                    result.Name = member.Name;
                    result.Email = member.Email;
                    result.Phone = member.Phone;
                    result.Birth = member.Birth;
                    db.Entry(member).State = System.Data.Entity.EntityState.Modified;
                    if (db.SaveChanges()>0)
                    {
                        return "更新成功";
                    }
                }
                catch(Exception ex)
                {
                    throw;
                }
            }

            return "更新失败";
        }

        public JsonResult GetMemberById(string id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var member = db.Members.FirstOrDefault(m => m.Id == id);
            return Json(member, JsonRequestBehavior.AllowGet);
        }
    }
}