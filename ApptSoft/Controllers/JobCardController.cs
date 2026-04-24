using ApptSoft.Data;
using ApptSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApptSoft.Controllers
{
    public class JobCardController : Controller
    {
        ApptSoftEntities db = new ApptSoftEntities();
        // GET: JobCard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SaveFullJobCard(JobCardFullModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    int jobCardId = model.JobCard.JobCardId;

                    // ===============================
                    // ✅ INSERT OR UPDATE JOBCARD
                    // ===============================
                    FIBC_JobCard job;

                    if (jobCardId > 0)
                    {
                        job = db.FIBC_JobCard.Find(jobCardId);

                        if (job == null)
                            return Json(new { success = false, message = "JobCard not found" });

                        // Update fields
                        db.Entry(job).CurrentValues.SetValues(model.JobCard);
                    }
                    else
                    {
                        job = model.JobCard;
                        job.CreatedDate = DateTime.Now;

                        db.FIBC_JobCard.Add(job);
                        db.SaveChanges(); // get JobCardId
                        jobCardId = job.JobCardId;
                    }

                    // ===============================
                    // ✅ FABRIC (DELETE + INSERT)
                    // ===============================
                    var oldFabric = db.FIBC_FabricDetails
                                      .Where(x => x.JobCardId == jobCardId)
                                      .ToList();

                    if (oldFabric.Any())
                        db.FIBC_FabricDetails.RemoveRange(oldFabric);

                    model.Fabric.JobCardId = jobCardId;
                    db.FIBC_FabricDetails.Add(model.Fabric);

                    // ===============================
                    // ✅ COMPONENT (DELETE + INSERT)
                    // ===============================
                    var oldComponent = db.FIBC_ComponentDetails
                                         .Where(x => x.JobCardId == jobCardId)
                                         .ToList();

                    if (oldComponent.Any())
                        db.FIBC_ComponentDetails.RemoveRange(oldComponent);

                    model.Component.JobCardId = jobCardId;
                    db.FIBC_ComponentDetails.Add(model.Component);

                    // ===============================
                    // ✅ FINAL SAVE
                    // ===============================
                    db.SaveChanges();
                    transaction.Commit();

                    return Json(new
                    {
                        success = true,
                        id = jobCardId,
                        message = "Saved Successfully"
                    });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return Json(new
                    {
                        success = false,
                        message = ex.Message
                    });
                }
            }
        }
        public JsonResult GetFullJobCard(int id)
        {
            var job = db.FIBC_JobCard.FirstOrDefault(x => x.JobCardId == id);

            var fabric = db.FIBC_FabricDetails
                           .FirstOrDefault(x => x.JobCardId == id);

            var component = db.FIBC_ComponentDetails
                              .FirstOrDefault(x => x.JobCardId == id);

            return Json(new
            {
                job,
                fabric,
                component
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update(FIBC_JobCard model)
        {
            try
            {
                var d = db.FIBC_JobCard.Find(model.JobCardId);

                if (d == null)
                    return Json(new { success = false, message = "Record not found" });

                db.Entry(d).CurrentValues.SetValues(model);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public JsonResult GetById(int id)
        {
            var data = db.FIBC_JobCard
                .Where(x => x.JobCardId == id)
                .FirstOrDefault();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllJobCards()
        {
            var data = db.FIBC_JobCard
                .Select(x => new {
                    x.JobCardId,
                    x.Customer,
                    x.RefNo,
                    x.OrderQty,
                    x.BagType,
                    x.PurchaseNo
                }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}