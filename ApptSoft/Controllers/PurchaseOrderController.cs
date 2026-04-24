using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using ApptSoft.Data;
using ApptSoft.Models;
using ClosedXML.Excel;

namespace ApptSoft.Controllers
{
    public class PurchaseOrderController : Controller
    {
        ApptSoftEntities db = new ApptSoftEntities();
        // GET: PurchaseOrder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Composition()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Save(PurchaseOrderModel model, HttpPostedFileBase Attachment)
        {
            try
            {
                DateTime billingDate;
                if (!DateTime.TryParse(model.Billing_Date, out billingDate))
                {
                    return Json(new { success = false, message = "Invalid date" });
                }

                var nextDate = billingDate.AddDays(1);

                // ✅ Duplicate Check
                var exists = db.tblPurchaseOrders.Any(x =>
                    x.PO_Number == model.PO_Number &&
                    x.Item_No == model.Item_No &&
                    x.Customer == model.Customer &&
                    x.Billing_Date >= billingDate &&
                    x.Billing_Date < nextDate
                );

                if (exists)
                {
                    return Json(new { success = false, message = "Duplicate PO exists!" });
                }

                tblPurchaseOrder po = new tblPurchaseOrder();

                po.Customer = model.Customer;
                po.Container_No = model.Container_No;
                po.PO_Number = model.PO_Number;
                po.Item_No = model.Item_No;
                po.Size_Of_Bag = model.Size_Of_Bag;
                po.ORDER_QTY = model.ORDER_QTY;

                po.Req_Wt = model.Req_Wt;
                po.Total_Req_Wt = model.Total_Req_Wt;
                po.Act_Wt = model.Act_Wt;
                po.Total_Act_Wt = model.Total_Act_Wt;

                po.PcsPerPallet = model.PcsPerPallet;

                po.Status = 1;
                po.CreatedBy = model.CreatedBy;
                po.Created_Date = DateTime.Now;
                po.Billing_Date = billingDate;

                // ✅ File Upload
                if (Attachment != null && Attachment.ContentLength > 0)
                {
                    string fileName = model.PO_Number + model.Item_No + Path.GetExtension(Attachment.FileName);
                    string path = Path.Combine(Server.MapPath("~/Uploads/PO"), fileName);

                    Attachment.SaveAs(path);

                    po.Attachment = fileName; // if string column
                }

                db.tblPurchaseOrders.Add(po);
                db.SaveChanges();

                return Json(new { success = true, id = po.PO_Id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public JsonResult GetAll()
        {
            var data = db.tblPurchaseOrders
                .AsNoTracking()
                .Select(x => new
                {
                    x.PO_Id,
                    x.Customer,
                    x.PO_Number,
                    x.Item_No,
                    x.Container_No,
                    x.Size_Of_Bag,
                    x.ORDER_QTY,
                    x.Req_Wt,
                    x.Total_Req_Wt,
                    x.Act_Wt,
                    x.Total_Act_Wt,
                    x.PcsPerPallet,
                    Attachment = string.IsNullOrEmpty(x.Attachment)? "" : "/Uploads/PO/" + x.Attachment,
                    x.Billing_Date,
                    x.Status
                })
                .OrderByDescending(x => x.Billing_Date)
                .ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetById(int id)
        {
            var data = db.tblPurchaseOrders
                .Where(x => x.PO_Id == id)
                .FirstOrDefault();

            if (data == null)
            {
                return Json(new { success = false, message = "Record not found" }, JsonRequestBehavior.AllowGet);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update(PurchaseOrderModel model, HttpPostedFileBase Attachment)
        {
            try
            {
                var po = db.tblPurchaseOrders.Find(model.PO_Id);

                if (po == null)
                {
                    return Json(new { success = false, message = "Record not found" });
                }

                DateTime billingDate;
                if (!DateTime.TryParse(model.Billing_Date, out billingDate))
                {
                    return Json(new { success = false, message = "Invalid date" });
                }

                var nextDate = billingDate.AddDays(1);

                // ✅ Duplicate Check (exclude current)
                var exists = db.tblPurchaseOrders.Any(x =>
                    x.PO_Id != model.PO_Id &&
                    x.PO_Number == model.PO_Number &&
                    x.Item_No == model.Item_No &&
                    x.Customer == model.Customer &&
                    x.Billing_Date >= billingDate &&
                    x.Billing_Date < nextDate
                );

                if (exists)
                {
                    return Json(new { success = false, message = "Duplicate PO exists!" });
                }

                po.Customer = model.Customer;
                po.Container_No = model.Container_No;
                po.PO_Number = model.PO_Number;
                po.Item_No = model.Item_No;
                po.Size_Of_Bag = model.Size_Of_Bag;
                po.ORDER_QTY = model.ORDER_QTY;

                po.Req_Wt = model.Req_Wt;
                po.Total_Req_Wt = model.Total_Req_Wt;
                po.Act_Wt = model.Act_Wt;
                po.Total_Act_Wt = model.Total_Act_Wt;

                po.PcsPerPallet = model.PcsPerPallet;
                po.Billing_Date = billingDate;

                // ✅ File Update
                if (Attachment != null && Attachment.ContentLength > 0)
                {
                    string fileName = model.PO_Number+model.Item_No + DateTime.Now + Path.GetExtension(Attachment.FileName);
                    string path = Path.Combine(Server.MapPath("~/Uploads/PO"), fileName);

                    Attachment.SaveAs(path);

                    po.Attachment = fileName; // if string column
                }

                db.SaveChanges();

                return Json(new { success = true, message = "Updated Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var po = db.tblPurchaseOrders.Find(id);

            if (po != null)
            {
                db.tblPurchaseOrders.Remove(po);
                db.SaveChanges();
            }

            return Json(new { success = true, message = "Deleted Successfully" });
        }
        public JsonResult GetComposition(string poNumber)
        {
            var query = db.tblPurchaseOrders.AsQueryable();

            if (!string.IsNullOrEmpty(poNumber))
            {
                query = query.Where(x => x.PO_Number == poNumber);
            }

            var data = query.ToList()
                .Select((x, i) => new
                {
                    x.Customer,
                    SR_NO = i + 1,
                    x.PO_Number,
                    REF_NO = x.Item_No,
                    x.Size_Of_Bag,
                    ORDER_QTY = Convert.ToInt32(x.ORDER_QTY),

                    PALLET_PCS = x.PcsPerPallet,

                    NO_OF_PALLET = x.PcsPerPallet == 0 ? 0 :
                       Convert.ToInt32(x.ORDER_QTY) / x.PcsPerPallet,

                    x.Req_Wt,
                    x.Act_Wt,

                    PALLET_WT = x.Req_Wt * x.PcsPerPallet,

                    TOTAL_WT = (x.Req_Wt * x.PcsPerPallet) * (Convert.ToInt32(x.ORDER_QTY) / x.PcsPerPallet),
                }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}