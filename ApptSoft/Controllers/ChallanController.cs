using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApptSoft.Models;

namespace ApptSoft.Controllers
{
    public class ChallanController : Controller
    {
        // GET: Challan
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        //public ActionResult GetChallans(string date)
        //{
        //    try
        //    {
        //        DateTime filterDate = DateTime.Parse(date);
        //        return Json(new { model = new ChallanModel().GetChallans(filterDate) }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public ActionResult SaveChallan(ChallanModel model)
        //{
        //    try
        //    {
        //        HttpPostedFileBase fb = null;
        //        for (int i = 0; i < Request.Files.Count; i++)
        //        {
        //            fb = Request.Files[i];
        //        }

        //        return Json(new { Message = new ChallanModel().SaveChallan(model) }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public ActionResult GetById(int id)
        //{
        //    try
        //    {
        //        return Json(new { model = new ChallanModel().GetById(id) }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}