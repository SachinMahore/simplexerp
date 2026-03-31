using AppSoft.Models;
using ApptSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppSoft.Controllers
{
    public class ExpensesController : Controller
    {
        // GET: Expenses

        public ActionResult ExpensesIndex()
        {
            if (Session["Id"] == null)
            {
                return View("..\\Login\\Index");
            }
            return View();
        }


        //SaveCode
        public ActionResult SaveExpensesData(ExpensesModel model)
        {
           

            try
            {
                HttpPostedFileBase fb = null;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    fb = Request.Files[i];
                }
                return Json(new { message = new ExpensesModel().SaveExpensesData(fb, model) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //ListCode
        public ActionResult GetExpensesList()
        {
            try
            {
                return Json(new { model = (new ExpensesModel().GetExpensesList()) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //DeleteCode
        public ActionResult DeleteExpenses(int Id)
        {
            try
            {
                return Json(new { model = (new ExpensesModel().DeleteExpenses(Id)) },
               JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //EditCode

        public ActionResult EditExpenses(int Id)
        {
            try
            {
                return Json(new { model = (new ExpensesModel().EditExpenses(Id)) },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DetailExpenses(int Id)
        {
            try
            {
                return Json(new { model = (new ExpensesModel().EditExpenses(Id)) },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}