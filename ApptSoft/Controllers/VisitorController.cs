using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApptSoft.Models;

namespace ApptSoft.Controllers
{
    public class VisitorController : Controller
    {
        // GET: Visitor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            return View();
        }
        // Save Visitor
        public ActionResult SaveVisitor(VisitorModel model)
        {
            try
            {
                HttpPostedFileBase fb = null;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    fb = Request.Files[i];
                }

                return Json(new { Message = new VisitorModel().SaveVisitor(fb, model) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Get Visitors
        public ActionResult GetVisitors(string date)
        {
            try
            {
                DateTime filterDate = DateTime.Parse(date);
                return Json(new { model = new VisitorModel().GetVisitors(filterDate) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetVisitorsByFlat()
        {
            try
            {
                return Json(new { model = new VisitorModel().GetVisitorsByFlat() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}