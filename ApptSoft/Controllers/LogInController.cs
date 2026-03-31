using ApptSoft.Data;
using ApptSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ApptSoft.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogIn
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoginIN(LogInModel model)
        {
            if (ModelState.IsValid)
            {
                ApptSoftEntities db = new ApptSoftEntities();
                var residentData = db.tblResidents.Where(p => p.MobileNo == model.UserName && p.Password == model.Password).FirstOrDefault();
                if (residentData !=null)
                {
                    Session["Id"] = residentData.Id.ToString();
                    Session["FlatNo"] = residentData.FlatNo.ToString();
                    Session["UserName"] = residentData.FirstName.Split(' ')[0];
                    Session["dept"] = residentData.Wing.ToString();
                    return RedirectToAction("UserDashBoard");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User name or password.");
                }
            }
            return View("..\\Login\\Index", model);
        }
        public ActionResult UserDashBoard()
        {
            //return View("..\\Login\\Index");
            return View("..\\Dashboard\\DashboardIndex");
        }
        public ActionResult Logout()
        {
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            return View("..\\Login\\Index");
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult ChangePasswordSubmit(string MobileNo, string oldPass, string newPassword)
        {
            try
            {
                return Json(new { model = (new ResidentModel().ChangePassword(MobileNo,oldPass,newPassword)) },
               JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}