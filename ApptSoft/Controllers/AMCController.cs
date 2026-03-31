using AppSoft.Models;
using ApptSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApptSoft.Controllers
{
    public class AMCController : Controller
    {
        // GET: AMC
        public ActionResult AMCIndex()
        {
            if (Session["Id"] == null)
            {
                return View("..\\Login\\Index");
            }
            return View();
        }


        public ActionResult SaveAMCData(AMCModel model)
        {

            try
            {
                HttpPostedFileBase fb = null;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    fb = Request.Files[i];
                }
                return Json(new { message = new AMCModel().SaveAMCData(fb, model) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //ListCode
        public ActionResult GetAMCList()
        {
            try
            {
                return Json(new { model = (new AMCModel().GetAMCList()) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //DeleteCode
        public ActionResult DeleteAMC(int Id)
        {
            try
            {
                return Json(new { model = (new AMCModel().DeleteAMC(Id)) },
               JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //EditCode

        public ActionResult EditAMC(int Id)
        {
            try
            {
                return Json(new { model = (new AMCModel().EditAMC(Id)) },
               JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //DetailCode
        public ActionResult DetailAMC(int Id)
        {
            try
            {
                return Json(new { model = (new AMCModel().EditAMC(Id)) },  JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}