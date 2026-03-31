using ApptSoft.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApptSoft.Controllers
{
    public class ContractorController : Controller
    {
        ApptSoftEntities db = new ApptSoftEntities();

        public ActionResult Index()
        {
            return View();
        }

        // GET ALL
        public JsonResult GetAll()
        {
            var data = db.tblContractors
                         .Select(x => new
                         {
                             x.Contractor_Id,
                             x.Contractor_Name,
                             x.Status
                         })
                         .ToList();

            return Json(new { model = data }, JsonRequestBehavior.AllowGet);
        }

        // SAVE (INSERT + UPDATE)
        [HttpPost]
        public JsonResult Save(int Contractor_Id, string Contractor_Name, int? Status)
        {
            tblContractor c;

            if (Contractor_Id > 0) // UPDATE
            {
                c = db.tblContractors.FirstOrDefault(x => x.Contractor_Id == Contractor_Id);

                if (c != null)
                {
                    c.Contractor_Name = Contractor_Name;
                    c.Status = Status;
                }
            }
            else // INSERT
            {
                c = new tblContractor()
                {
                    Contractor_Name = Contractor_Name,
                    Status = Status ?? 1
                };

                db.tblContractors.Add(c);
            }

            db.SaveChanges();

            return Json(new { message = "Saved Successfully" });
        }
    }
}