using ApptSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApptSoft.Controllers
{
    public class IncomeController : Controller
    {
        // GET: Income

        public ActionResult IncomeIndex()
        {
            if (Session["Id"] == null)
            {
                return View("..\\Login\\Index");
            }
            return View();
        }
        public ActionResult SaveIncome(IncomeModel model)
        {
            try
            {
                return Json(new { Message = new IncomeModel().SaveIncomeData(model) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
              
                return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult GetIncome(string Year = "", string month = "")
        {
            try
            {
                if (Year == "" && month == "")
                {
                    return Json(new { Message = new IncomeModel().GetIncomeData() }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = new IncomeModel().GetIncomeSearch(Year, month) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EditIncome(int Id)
        {
            try
            {
                return Json(new { model = (new IncomeModel().EditIncomeData(Id)) },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult DeleteIncome(int Id)
        {
            try
            {
                return Json(new { model = (new IncomeModel().DeleteIncomeData(Id)) },
                   JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //Detail
        public ActionResult DetailIncome(int Id)
        {
            try
            {
                return Json(new { model = (new IncomeModel().EditIncomeData(Id)) },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}