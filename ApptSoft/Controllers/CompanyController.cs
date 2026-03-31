using ApptSoft.Data;
using ApptSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApptSoft.Controllers
{
    public class CompanyController : Controller
    {
        // GET: Company
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetCompanies()
        {
            ApptSoftEntities db = new ApptSoftEntities();

            var data = db.Company_Master
                .Select(x => new
                {
                    x.Company_Id,
                    x.Company_Name,
                    x.Address1,
                    x.Address2,
                    x.City,
                    x.State,
                    x.Zip_Code,
                    x.Country,
                    x.Phone,
                    x.Fax,
                    x.Is_Active
                }).Where(x =>x.Company_Id !=1 & x.Company_Id != 2).ToList();
            return Json(new { model = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveCompany(CompanyModel model)
        {
            var db = new ApptSoftEntities();
            Company_Master c;



            if (model.Company_Id > 0) // UPDATE
            {
                c = db.Company_Master.FirstOrDefault(x => x.Company_Id == model.Company_Id);

                if (c != null)
                {
                    c.Company_Name = model.Company_Name; // enable if you allow edit
                    c.Address1 = model.Address1;
                    c.Address2 = model.Address2;
                    c.City = model.City;
                    c.State = model.State;
                    c.Zip_Code = model.Zip_Code;
                    c.Country = model.Country;
                    c.Phone = model.Phone;
                    c.Fax = model.Fax;
                    c.Is_Active = model.Is_Active;
                }
            }
            else // INSERT
            {            // 🔴 CHECK DUPLICATE NAME
                bool isExists = db.Company_Master
                    .Any(x => x.Company_Name == model.Company_Name); // exclude current record

                if (isExists)
                {
                    return Json(new { success = false, message = "Company Name already exists!" });
                }
                c = new Company_Master()
                {
                    Company_Name = model.Company_Name,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    City = model.City,
                    State = model.State,
                    Zip_Code = model.Zip_Code,
                    Country = model.Country,
                    Phone = model.Phone,
                    Fax = model.Fax,
                    Is_Active = model.Is_Active
                };

                db.Company_Master.Add(c);
            }

            db.SaveChanges();

            return Json(new { success = true, message = "Saved Successfully" });
        }
    }
}