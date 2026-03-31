using AppSoft.Models;
using ApptSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApptSoft.Controllers
{
    public class ResidentController : Controller
    {
        // GET: Resident
       
        public ActionResult ResidentIndex()
        {
            if (Session["Id"] == null)
            {
                return View("..\\Login\\Index");
            }
            return View();
        }
        public ActionResult Parking()
        {
            
            return View();
        }

        public ActionResult SaveResident(ResidentModel model)
        {
            try
            {
                HttpPostedFileBase fb = null;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    fb = Request.Files[i];
                }
                return Json(new { message = new ResidentModel().SaveResidentData(fb, model) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult GetResident()
        {
            try
            {
                return Json(new { message = new ResidentModel().GetResidentData() }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteResident(int Id)
        {
            try
            {
                return Json(new { model = (new ResidentModel().DeleteResidentData(Id)) },
               JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
       
        public ActionResult EditResident(int Id)
        {
            try
            {
                return Json(new { model = (new ResidentModel().EditResidentData(Id)) },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        //DetailCode
        public ActionResult DetailResident(int Id)
        {
            try
            {
                return Json(new { model = (new ResidentModel().EditResidentData(Id)) },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}