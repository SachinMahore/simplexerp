using ApptSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApptSoft.Controllers
{
    public class CorpusFundController : Controller
    {
        // GET: CorpusFund
        public ActionResult CorpusIndex()
        {
            if (Session["Id"] == null)
            {
                return View("..\\Login\\Index");
            }
            return View();
        }

        public ActionResult SaveCorpusFund(CorpusFundModel model)
        {
            try
            {
                return Json(new { message = new CorpusFundModel().SaveCorpusFundData(model) }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {

                return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult GetCorpusFund()
        {
            try
            {
                return Json(new { message = new CorpusFundModel().GetcCorpusFundData() }, JsonRequestBehavior.AllowGet);


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
                return Json(new { model = (new CorpusFundModel().DeleteCorpusFundData(Id)) },
               JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Edit Code
        public ActionResult EditCorpusFund(int Id)
        {
            try
            {
                return Json(new { model = (new CorpusFundModel().EditCorpusFundData(Id)) },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        //DetailCode

        public ActionResult DetailCopusFund(int Id)
        {
            try
            {
                return Json(new { model = new CorpusFundModel().EditCorpusFundData(Id) }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception Ex)
            {
                return Json(new {Ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}