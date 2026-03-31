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
        // GET: JobCard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GenerateJobCard(int poId)
        {
            ApptSoftEntities db = new ApptSoftEntities();

            var po = db.PurchaseOrders.FirstOrDefault(x => x.PO_Id == poId);

            var items = db.PurchaseOrder_Details
                .Where(x => x.PO_Id == poId)
                .ToList();

            JobCardModel model = new JobCardModel();

            model.PO_Id = po.PO_Id;
            model.PO_Number = po.PO_Number;
            model.Vendor_Id = po.Vendor_Id;
            model.Company_Id = po.Company_Id;
            model.Job_Date = DateTime.Now;

            model.Items = items.Select(x => new JobCardDetailsModel
            {
                Item_No = x.Item_No,
                Item_Description = x.Item_Description,
                Qty_Per_Bale = x.Qty_Per_Bale ?? 0,
                Bale_Count = x.Bale_Count ?? 0,
                Total_Quantity = x.Total_Quantity ?? 0,
                Unit = x.Unit
            }).ToList();
            ViewBag.PO_Id = poId;
            return View(model);
        }
        public JsonResult SaveJobCard(JobCardModel model)
        {
            ApptSoftEntities db = new ApptSoftEntities();

            JobCard_Master job = new JobCard_Master()
            {
                JobCard_Number = "JC" + DateTime.Now.Ticks,
                PO_Id = model.PO_Id,
                PO_Number = model.PO_Number,
                Vendor_Id = model.Vendor_Id,
                Company_Id = model.Company_Id,
                Job_Date = model.Job_Date,
                Status = 0,
                Created_Date = DateTime.Now
            };

            db.JobCard_Master.Add(job);
            db.SaveChanges();

            int jobId = job.JobCard_Id;

            foreach (var item in model.Items)
            {
                JobCard_Details d = new JobCard_Details()
                {
                    JobCard_Id = jobId,
                    Item_No = item.Item_No,
                    Item_Description = item.Item_Description
                };

                db.JobCard_Details.Add(d);
            }

            db.SaveChanges();

            return Json(new { JobCard_Id = jobId }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string SaveTracking(List<ProductionTrackingModel> model)
        {
            ApptSoftEntities db = new ApptSoftEntities();

            foreach (var item in model)
            {
                JobCard_Production_Tracking t = new JobCard_Production_Tracking();

                t.JobCard_Id = item.JobCard_Id;
                t.JCD_Id = item.JCD_Id;

                t.Operation_Name = item.Operation_Name;

                t.Input_Qty = item.Input_Qty;
                t.Output_Qty = item.Output_Qty;
                t.Wastage_Qty = item.Wastage_Qty;

                t.Machine_Name = item.Machine_Name;
                t.Operator_Name = item.Operator_Name;

                t.Start_Time = item.Start_Time;
                t.End_Time = item.End_Time;

                t.Status = item.Status;

                db.JobCard_Production_Tracking.Add(t);
            }

            db.SaveChanges();

            return "Tracking Saved Successfully";
        }
    }
}