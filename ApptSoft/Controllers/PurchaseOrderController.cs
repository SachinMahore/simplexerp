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
        // GET: PurchaseOrder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        //public ActionResult GetPurchaseOrders(string date, string poNumber)
        //{
        //    try
        //    {
        //        DateTime filterDate = DateTime.Parse(date);

        //        return Json(new
        //        {
        //            model = new PurchaseOrderModel().GetPurchaseOrders(filterDate, poNumber)
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        public ActionResult GetPurchaseOrders(string poNumber, int? vendorId, string fromDate, string toDate)
        {
            DateTime fdate = DateTime.Parse(fromDate);
            DateTime tdate = DateTime.Parse(toDate);

            return Json(new
            {
                model = new PurchaseOrderModel().SearchPO(poNumber, vendorId, fdate, tdate)
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SavePurchaseOrder(PurchaseOrderModel model)
        {
            try
            {
                HttpPostedFileBase fb = null;

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    fb = Request.Files[i];
                }

                return Json(new
                {
                    Message = new PurchaseOrderModel().SavePurchaseOrder(model)
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetById(int id)
        {
            try
            {
                return Json(new
                {
                    model = new PurchaseOrderModel().GetById(id)
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { model = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PrintPO(int id)
        {
            PurchaseOrderModel model = new PurchaseOrderModel().GetById(id);

            return View(model);
        }
        public ActionResult DeletePO(int id)
        {
            var db = new ApptSoftEntities();

            var po = db.PurchaseOrders.FirstOrDefault(x => x.PO_Id == id);

            if (po != null)
            {
                db.PurchaseOrders.Remove(po);
                db.SaveChanges();
            }

            return Json(new { message = "PO Deleted Successfully" });
        }
        public ActionResult ExportExcel(string poNumber, int? vendorId, string fromDate, string toDate)
        {
            var data = new PurchaseOrderModel().SearchPO(poNumber, vendorId,
            DateTime.Parse(fromDate), DateTime.Parse(toDate));

            GridView gv = new GridView();

            gv.DataSource = data;

            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=POList.xls");
            Response.ContentType = "application/ms-excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            gv.RenderControl(hw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View();
        }
        public ActionResult ExportPOItems(int poId)
        {
            ApptSoftEntities db = new ApptSoftEntities();

            var po = db.PurchaseOrders.FirstOrDefault(x => x.PO_Id == poId);

            var items = db.PurchaseOrder_Details
                .Where(x => x.PO_Id == poId)
                .ToList();

            var vendor = db.Vendor_Master.FirstOrDefault(x => x.Vendor_Id == po.Vendor_Id);
            var company = db.Company_Master.FirstOrDefault(x => x.Company_Id == po.Company_Id);

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Purchase Order");

                int row = 1;

                // Company Details
                ws.Cell(row, 1).Value = company.Company_Name;
                ws.Range(row, 1, row, 8).Merge().Style.Font.Bold = true;
                row++;

                ws.Cell(row, 1).Value = company.Address1 + " " + company.Address2;
                ws.Range(row, 1, row, 8).Merge();
                row++;

                ws.Cell(row, 1).Value = company.City + " " + company.State;
                ws.Range(row, 1, row, 8).Merge();
                row++;

                ws.Cell(row, 1).Value = "Phone : " + company.Phone;
                ws.Range(row, 1, row, 8).Merge();
                row += 2;

                // PO Info
                ws.Cell(row, 1).Value = "Purchase Order";
                ws.Range(row, 1, row, 8).Merge().Style.Font.FontSize = 16;
                ws.Range(row, 1, row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                row += 2;

                ws.Cell(row, 1).Value = "PO Number";
                ws.Cell(row, 2).Value = po.PO_Number;

                ws.Cell(row, 5).Value = "PO Date";
                ws.Cell(row, 6).Value = po.PO_Date.ToShortDateString();
                row++;

                ws.Cell(row, 1).Value = "Vendor";
                ws.Cell(row, 2).Value = vendor.Vendor_Name;

                ws.Cell(row, 5).Value = "Phone";
                ws.Cell(row, 6).Value = vendor.Phone;
                row += 2;

                // Table Header
                ws.Cell(row, 1).Value = "Item No";
                ws.Cell(row, 2).Value = "Description";
                ws.Cell(row, 3).Value = "Qty/Bale";
                ws.Cell(row, 4).Value = "Bale Count";
                ws.Cell(row, 5).Value = "Total Qty";
                ws.Cell(row, 6).Value = "Unit";
                ws.Cell(row, 7).Value = "Unit Price";
                ws.Cell(row, 8).Value = "Line Total";

                ws.Range(row, 1, row, 8).Style.Font.Bold = true;
                ws.Range(row, 1, row, 8).Style.Fill.BackgroundColor = XLColor.LightGray;

                row++;

                int startRow = row;

                foreach (var item in items)
                {
                    ws.Cell(row, 1).Value = item.Item_No;
                    ws.Cell(row, 2).Value = item.Item_Description;
                    ws.Cell(row, 3).Value = item.Qty_Per_Bale;
                    ws.Cell(row, 4).Value = item.Bale_Count;
                    ws.Cell(row, 5).Value = item.Total_Quantity;
                    ws.Cell(row, 6).Value = item.Unit;
                    ws.Cell(row, 7).Value = item.Unit_Price;
                    ws.Cell(row, 8).Value = item.Line_Total;

                    row++;
                }

                int endRow = row - 1;

                // Table Border
                ws.Range(startRow - 1, 1, endRow, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(startRow - 1, 1, endRow, 8).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                row++;

                // Totals
                ws.Cell(row, 7).Value = "Subtotal";
                ws.Cell(row, 8).FormulaA1 = "=SUM(H" + startRow + ":H" + endRow + ")";
                row++;

                ws.Cell(row, 7).Value = "Discount";
                ws.Cell(row, 8).Value = po.Discount;
                row++;

                ws.Cell(row, 7).Value = "Tax";
                ws.Cell(row, 8).Value = po.Tax_Amount;
                row++;

                ws.Cell(row, 7).Value = "Grand Total";
                ws.Cell(row, 8).Value = po.Grand_Total;

                ws.Range(row, 7, row, 8).Style.Font.Bold = true;

                ws.Columns().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "PurchaseOrder_" + po.PO_Number + ".xlsx");
                }
            }
        }
    }
}