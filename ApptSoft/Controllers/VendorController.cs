using ApptSoft.Data;
using ApptSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApptSoft.Controllers
{
    public class VendorController : Controller
    {
        // GET: Vendor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetVendors()
        {
            ApptSoftEntities db = new ApptSoftEntities();

            var data = db.Vendor_Master
                .Select(x => new
                {
                    x.Vendor_Id,
                    x.Vendor_Name
                }).ToList();

            return Json(new { model = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveVendor(VendorModel model)
        {
            var db = new ApptSoftEntities();

            Vendor_Master v = new Vendor_Master()
            {
                Vendor_Name = model.Vendor_Name,
                Contact_Person = model.Contact_Person,
                Phone = model.Phone,
                Email = model.Email
            };

            db.Vendor_Master.Add(v);
            db.SaveChanges();

            return Json(new { message = "Vendor Added Successfully" });
        }
    }
}