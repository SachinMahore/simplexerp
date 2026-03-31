using AppSoft.Models;
using ApptSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApptSoft.Controllers
{
    public class ComplaintController : Controller
    {
        // GET: Complaint
        public ActionResult ComplaintIndex()
        {
            if (Session["Id"] == null)
            {
                return View("..\\Login\\Index");
            }
            return View();
        }
        public ActionResult SaveComplaint(ComplaintModel model)
        {
            try

            {
                HttpPostedFileBase fb = null;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    fb = Request.Files[i];
                }
                return Json(new { Message = new ComplaintModel().SaveComplaint(fb, model) }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //ListCode

        public ActionResult GetComplaint()
        {
            try
            {
                return Json(new { model = new ComplaintModel().GetComplaint() }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Delete Code

        public ActionResult DeleteComplaint(int Id)
        {
            try
            {
                return Json(new { model = new ComplaintModel().DeleteComplaint(Id) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        //EditCode

        public ActionResult EditComplaint(int Id)
        {
            try
            {
                return Json(new { model = (new ComplaintModel().EditComplaint(Id)) },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        //DetailCode

        public ActionResult ComplaintDetail(int Id)
        {
            try
            {
                return Json(new { model = new ComplaintModel().EditComplaint(Id) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}