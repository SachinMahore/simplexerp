using ApptSoft.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ApptSoft.Controllers
{ 
    public class TenantController : Controller
    {
        // GET: Tenant
        public ActionResult TenantIndex()
        {
            if (Session["Id"] == null)
            {
                return View("..\\Login\\Index");
            }
            return View();
        }
        public ActionResult SaveTenant(TenantModel model)
        {

            try
            {
                HttpPostedFileBase fb = null;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    fb = Request.Files[i];
                }
                return Json(new { message = new TenantModel().SaveTenantData(fb, model) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



            //ListCode
            public ActionResult GetTenantList()
            {
                try

                {
                    return Json(new { model = (new TenantModel().GetTenantList()) }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }



            //DeleteCode
            public ActionResult DeleteTenant(int Id)
            {
                try
                {
                    return Json(new { model = (new TenantModel().DeleteTenantData(Id)) },
                   JsonRequestBehavior.AllowGet);
                }
                catch (Exception Ex)
                {
                    return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }


            //EditCode
            public ActionResult EditTenant(int Id)
            {
                try
                {
                    return Json(new { model = (new TenantModel().EditTenantData(Id)) },
                   JsonRequestBehavior.AllowGet);
                }
                catch (Exception Ex)
                {
                    return Json(new { Ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }

        // DetailCOde

        public ActionResult DetailTenant(int Id)
        {
            try
            {
                return Json(new { model = new TenantModel().EditTenantData(Id) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new  { ex.Message},JsonRequestBehavior.AllowGet) ;
            }
        }
    }
}



