using ApptSoft.Data;
using ApptSoft.Models;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApptSoft.Controllers
{
    public class DispatchController : Controller
    {
        ApptSoftEntities db = new ApptSoftEntities();
        // GET: Dispatch
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dispatch()
        {
            return View();
        }
        public ActionResult ContractorReport()
        {
            return View();
        }
        public ActionResult Summary()
        {
            return View();
        }
        public ActionResult BallingBill()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Save(DispatchModel model)
        {
            tblDispatch d = new tblDispatch();

            var item = model;

            d.Customer = item.Customer;
            d.PO_Number = item.PO_Number;
            d.PO_Id = item.PO_Id;

            d.Size_Of_Bag = item.Size_Of_Bag;
            d.Req_Wt = item.Req_Wt;
            d.Total_Req_Wt = item.Total_Req_Wt;
            d.Actual_Quantity = item.Actual_Quantity;
            d.Act_Wt = item.Act_Wt;
            d.Total_Act_Wt = item.Total_Act_Wt;

            d.Rate = item.Rate;
            d.Kg_Or_Pcs = item.Kg_Or_Pcs;

            d.Total_Cost = item.Total_Cost;

            d.Status = 1;
            d.Created_Date = DateTime.Now;
            d.Billing_Date = Convert.ToDateTime(item.Billing_Date);
            d.Container_No = item.Container_No;
            d.Item_No = item.Item_No;
            d.ORDER_QTY = item.ORDER_QTY;



            db.tblDispatches.Add(d);
            db.SaveChanges();

            return Json(d.Dispatch_Id); // ✅ MUST RETURN ID
        }
        public JsonResult GetAll()
        {
            //var data = db.tblDispatches
            //    .OrderByDescending(x => x.Dispatch_Id)
            //    .ToList();

            var dispatches = db.tblDispatches
              .OrderByDescending(d => d.Dispatch_Id)
              .ToList();

            var contractors = db.tblDispatchContractors.ToList();

            var data = dispatches
                .Select(d => new
                {
                    d.Dispatch_Id,
                    d.Customer,
                    d.PO_Number,
                    d.Actual_Quantity,
                    d.Total_Act_Wt,
                    d.Rate,
                    d.Total_Cost,
                    d.Billing_Date,
                    d.Container_No,
                    d.Item_No,
                    d.ORDER_QTY,
                    d.Size_Of_Bag,
                    d.Act_Wt,
                    d.Kg_Or_Pcs,
                    d.Req_Wt,
                    d.Total_Req_Wt,
                    ContractorNames = string.Join(", ",
                            db.tblDispatchContractors
                              .Where(c => c.Dispatch_Id == d.Dispatch_Id)
                              .Select(c => c.Contractor_Name))
                })
                .ToList();


            //var data = db.tblDispatches
            //        .AsEnumerable()   // 🔥 switch to LINQ to Objects
            //        .Select(d => new
            //        {
            //            d.Dispatch_Id,
            //            d.Customer,
            //            d.PO_Number,
            //            d.Actual_Quantity,
            //            d.Total_Act_Wt,
            //            d.Rate,
            //            d.Total_Cost,
            //            d.Billing_Date,
            //            d.Container_No,
            //            d.Item_No,
            //            d.ORDER_QTY,
            //            d.Size_Of_Bag,
            //            ContractorNames = string.Join(", ",
            //                db.tblDispatchContractors
            //                  .Where(c => c.Dispatch_Id == d.Dispatch_Id)
            //                  .Select(c => c.Contractor_Name))
            //        })
            //        .ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetById(int id)
        {
            var data = db.tblDispatches
                .Where(x => x.Dispatch_Id == id)
                .FirstOrDefault();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update(DispatchModel model)
        {
            var d = db.tblDispatches.Find(model.Dispatch_Id);

            if (d != null)
            {
                d.Customer = model.Customer;
                d.PO_Number = model.PO_Number;
                d.PO_Id = model.PO_Id;

                d.Size_Of_Bag = model.Size_Of_Bag;

                d.Req_Wt = model.Req_Wt;
                d.Total_Req_Wt = model.Total_Req_Wt;

                d.Actual_Quantity = model.Actual_Quantity;
                d.Act_Wt = model.Act_Wt;
                d.Total_Act_Wt = model.Total_Act_Wt;

                d.Rate = model.Rate;
                d.Kg_Or_Pcs = model.Kg_Or_Pcs;

                d.Total_Cost = model.Total_Cost;

                d.Contractor = model.Contractor;
                d.Billing_Date = Convert.ToDateTime(model.Billing_Date);
                d.Container_No = model.Container_No;
                d.Item_No = model.Item_No;
                d.ORDER_QTY = model.ORDER_QTY;

                db.SaveChanges();
            }

            return Json("Updated Successfully");
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            // Get Dispatch
            var dispatch = db.tblDispatches.Find(id);

            if (dispatch != null)
            {
                // 🔥 Delete related contractors first
                var contractors = db.tblDispatchContractors
                                    .Where(x => x.Dispatch_Id == id)
                                    .ToList();

                if (contractors.Any())
                {
                    db.tblDispatchContractors.RemoveRange(contractors);
                }

                // Delete dispatch
                db.tblDispatches.Remove(dispatch);

                db.SaveChanges();
            }

            return Json("Deleted Successfully", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]

        public JsonResult SaveContractor(List<tblDispatchContractor> model)
        {
            if (model == null || model.Count == 0)
            {
                return Json(new { success = false, message = "No data found" });
            }

            try
            {
                int dispatchId = Convert.ToInt32(model.First().Dispatch_Id);

                // 🔥 STEP 1: DELETE OLD DATA
                var oldData = db.tblDispatchContractors
                                .Where(x => x.Dispatch_Id == dispatchId)
                                .ToList();

                if (oldData.Any())
                {
                    db.tblDispatchContractors.RemoveRange(oldData);
                }

                // 🔥 STEP 2: INSERT NEW DATA
                foreach (var item in model)
                {
                    item.Status = 1;
                    item.Created_Date = DateTime.Now;
                    db.tblDispatchContractors.Add(item);
                }

                db.SaveChanges();

                return Json(new { success = true, message = "Contractor Updated Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public JsonResult GetContractorByDispatchId(int id)
        {
            var data = db.tblDispatchContractors
                         .Where(x => x.Dispatch_Id == id && x.Status == 1)
                         .Select(x => new
                         {
                             x.DisContr_Id,
                             x.Dispatch_Id,

                             x.Contractor_Id,
                             x.Contractor_Name,

                             x.Customer,
                             x.PO_Ref_No,
                             x.Size_Of_Bag,

                             x.Actual_Quantity,
                             x.Act_Wt,
                             x.Total_Wt,

                             x.Rate,
                             x.Crate,

                             x.Kg_Or_Pcs,
                             x.Total_Cost
                         })
                         .ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetContractorReport(int? contractorId, int? month, int? year)
        {
            try
            {
                var query = db.tblDispatchContractors.AsQueryable();

                // 🔍 Contractor Filter
                if (contractorId.HasValue && contractorId.Value > 0)
                {
                    query = query.Where(x => x.Contractor_Id == contractorId);
                }
             
                // 🔍 Month + Year Filter
                if (month.HasValue && year.HasValue)
                {
                    query = query.Where(x =>
                        x.Created_Date.Value.Month == month.Value &&
                        x.Created_Date.Value.Year == year.Value);
                }

                var data = (from q in query
                            join d in db.tblDispatches
                            on q.Dispatch_Id equals d.Dispatch_Id into dj
                            from d in dj.DefaultIfEmpty()
                            orderby q.Created_Date descending
                            select new
                            {
                                q.DisContr_Id,
                                q.Contractor_Name,
                                Customer=d.Customer,
                                q.PO_Ref_No,
                                q.Size_Of_Bag,
                                d.Item_No,
                                q.Actual_Quantity,
                                q.Act_Wt,
                                q.Total_Wt,
                                q.Rate,
                                q.Total_Cost,

                                q.Kg_Or_Pcs,
                                Date = d.Billing_Date,

                                Container_No = d.Container_No
                            }).ToList();
                return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult GetsummaryReport(int? contractorId, int? month, int? year)
        {
            try
            {
                var query = db.tblDispatchContractors.AsQueryable();

                // 🔍 Contractor Filter
                if (contractorId.HasValue && contractorId.Value > 0)
                {
                    query = query.Where(x => x.Contractor_Id == contractorId);
                }

                // 🔍 Month + Year Filter
                if (month.HasValue && year.HasValue)
                {
                    query = query.Where(x =>
                        x.Created_Date.Value.Month == month.Value &&
                        x.Created_Date.Value.Year == year.Value);
                }

               
                var data = query
                .GroupBy(x => new { x.Contractor_Id, x.Contractor_Name })
                .Select(g => new
                {
                ContractorId = g.Key.Contractor_Id,
                ContractorName = g.Key.Contractor_Name,

                TotalQuantity = g.Sum(x => x.Actual_Quantity),
                TotalWeight = g.Sum(x => x.Total_Wt),
                TotalCost = g.Sum(x => x.Total_Cost),

                Count = g.Count()
                })
                .OrderByDescending(x => x.TotalCost)
                .ToList();

                return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetBellingReport(int? contractorId, int? month, int? year)
        {
            try
            {
                var query = db.tblDispatchContractors.AsQueryable();

                // 🔍 Contractor Filter
                if (contractorId.HasValue && contractorId.Value > 0)
                {
                    query = query.Where(x => x.Contractor_Id == contractorId);
                }

                // 🔍 Month + Year Filter
                if (month.HasValue && year.HasValue)
                {
                    query = query.Where(x =>
                        x.Created_Date.Value.Month == month.Value &&
                        x.Created_Date.Value.Year == year.Value);
                }           
                var data = query
                .GroupBy(x => new { x.Contractor_Id, x.Contractor_Name })
                .Select(g => new
                {
                    ContractorId = g.Key.Contractor_Id,
                    ContractorName = g.Key.Contractor_Name,

                    TotalQuantity = g.Sum(x => x.Actual_Quantity),
                    TotalWeight = g.Sum(x => x.Total_Wt),
                    TotalCost = g.Sum(x => x.Total_Cost),

                    Count = g.Count()
                })
                .OrderByDescending(x => x.TotalCost)
                .ToList();

                return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetBellingReportCustmor(int? month, int? year)
        {
            try
            {
                var data = (
                    from c in db.Company_Master
                    join d in db.tblDispatches
                    on c.Company_Name equals d.Customer into gj
                    from sub in gj.DefaultIfEmpty()

                    //where
                    //    (!month.HasValue || (sub.Created_Date.HasValue && sub.Created_Date.Value.Month == month)) &&
                    //    (!year.HasValue || (sub.Created_Date.HasValue && sub.Created_Date.Value.Year == year))

                    group sub by c.Company_Name into g

                    select new
                    {
                        CustmorName = g.Key,

                        TotalQuantity = g.Sum(x => (decimal?)x.Actual_Quantity) ?? 0,
                        TotalCost = g.Sum(x => (decimal?)x.Total_Cost) ?? 0
                    }
                )
                // ✅ CUSTOM ORDER
                .OrderBy(x =>
                    x.CustmorName == "WITHOUT LABEL & LINEAR" ? 0 :
                    x.CustmorName == "RE-PACKING" ? 1 : 2)
                .ThenByDescending(x => x.TotalCost)
                .ToList();

                return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDashbordData()
        {
            var totalAct_Wt = db.tblDispatches
                                .Where(x => x.Total_Act_Wt.HasValue)
                                .Sum(x => (decimal?)x.Total_Act_Wt) ?? 0;

            var totalReq_Wt = db.tblDispatches
                                .Where(x => x.Total_Req_Wt.HasValue)
                                .Sum(x => (decimal?)x.Total_Req_Wt) ?? 0;

            var totalamt = db.tblDispatches
                             .Where(x => x.Total_Cost.HasValue)
                             .Sum(x => (decimal?)x.Total_Cost) ?? 0;

            var totaldiff = totalAct_Wt - totalReq_Wt;

            var data = new
            {
                totalAct_Wt,
                totalReq_Wt,
                totaldiff,
                totalamt
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportDispatchExcel(int month, int year)
        {
            using (var wb = new XLWorkbook())
            {
                // =====================================================
                // ✅ FILTER DATA BY MONTH
                // =====================================================
                var dispatchList = db.tblDispatches
                    .Where(x => x.Created_Date.Value.Month == month && x.Created_Date.Value.Year == year)
                    .ToList();

                var contractorList = db.tblDispatchContractors
                    .Where(x => x.Created_Date.Value.Month == month && x.Created_Date.Value.Year == year)
                    .ToList();

                // =====================================================
                // ✅ DASHBOARD
                // =====================================================
                var wsDash = wb.Worksheets.Add("Dashboard");

                wsDash.Cell("A1").Value = $"DISPATCH DASHBOARD - {month}/{year}";
                wsDash.Range("A1:I1").Merge().Style
                    .Font.SetBold().Font.FontSize = 16;
                wsDash.Range("A1:I1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                wsDash.Cell("A3").Value = "Total Dispatch";
                wsDash.Cell("B3").Value = dispatchList.Count;

                wsDash.Cell("A4").Value = "Total Quantity";
                wsDash.Cell("B4").Value = dispatchList.Sum(x => x.Actual_Quantity);

                wsDash.Cell("A5").Value = "Total Weight";
                wsDash.Cell("B5").Value = dispatchList.Sum(x => x.Total_Act_Wt);

                wsDash.Cell("A6").Value = "Total Cost";
                wsDash.Cell("B6").Value = dispatchList.Sum(x => x.Total_Cost);

                wsDash.Cell("A7").Value = "Total Contractors";
                wsDash.Cell("B7").Value = contractorList.Select(x => x.Contractor_Id).Distinct().Count();

                wsDash.Range("A3:B7").Style.Fill.BackgroundColor = XLColor.LightBlue;


                var wsMain = wb.Worksheets.Add($"Dispatch -" + month + "-" + year);

                // HEADER
                wsMain.Cell(1, 1).Value = "Dispatch_Id";
                wsMain.Cell(1, 2).Value = "Sr_No";
                wsMain.Cell(1, 3).Value = "Customer";
                wsMain.Cell(1, 4).Value = "PO_Number";
                wsMain.Cell(1, 5).Value = "PO_Id";
                wsMain.Cell(1, 6).Value = "Size_Of_Bag";
                wsMain.Cell(1, 7).Value = "Req_Wt";
                wsMain.Cell(1, 8).Value = "Total_Req_Wt";
                wsMain.Cell(1, 9).Value = "Actual_Quantity";
                wsMain.Cell(1, 10).Value = "Act_Wt";
                wsMain.Cell(1, 11).Value = "Total_Act_Wt";
                wsMain.Cell(1, 12).Value = "Rate";
                wsMain.Cell(1, 13).Value = "Kg_Or_Pcs";
                wsMain.Cell(1, 14).Value = "Total_Cost";
                wsMain.Cell(1, 15).Value = "Contractor";
                wsMain.Cell(1, 16).Value = "Status";
                wsMain.Cell(1, 17).Value = "Created_Date";

                int rMain = 2;

                foreach (var d in dispatchList)
                {
                    wsMain.Cell(rMain, 1).Value = d.Dispatch_Id;
                    wsMain.Cell(rMain, 2).Value = d.Sr_No;
                    wsMain.Cell(rMain, 3).Value = d.Customer;
                    wsMain.Cell(rMain, 4).Value = d.PO_Number;
                    wsMain.Cell(rMain, 5).Value = d.PO_Id;
                    wsMain.Cell(rMain, 6).Value = d.Size_Of_Bag;
                    wsMain.Cell(rMain, 7).Value = d.Req_Wt;
                    wsMain.Cell(rMain, 8).Value = d.Total_Req_Wt;
                    wsMain.Cell(rMain, 9).Value = d.Actual_Quantity;
                    wsMain.Cell(rMain, 10).Value = d.Act_Wt;
                    wsMain.Cell(rMain, 11).Value = d.Total_Act_Wt;
                    wsMain.Cell(rMain, 12).Value = d.Rate;
                    wsMain.Cell(rMain, 13).Value = d.Kg_Or_Pcs;
                    wsMain.Cell(rMain, 14).Value = d.Total_Cost;
                    wsMain.Cell(rMain, 15).Value = d.Contractor;
                    wsMain.Cell(rMain, 16).Value = d.Status;
                    wsMain.Cell(rMain, 17).Value = d.Created_Date;

                    rMain++;
                }

                wsMain.Columns().AdjustToContents();


                // =====================================================
                // ✅ GET DISTINCT CONTRACTORS
                // =====================================================
                var contractors = contractorList
                    .Select(x => new { x.Contractor_Id, x.Contractor_Name })
                    .Distinct()
                    .ToList();


                // =====================================================
                // ✅ SHEET PER CONTRACTOR (ALL FIELDS)
                // =====================================================
                foreach (var contractor in contractors)
                {
                    var list = contractorList
                        .Where(x => x.Contractor_Id == contractor.Contractor_Id)
                        .OrderByDescending(x => x.Created_Date)
                        .ToList();

                    string sheetName = contractor.Contractor_Name;
                    if (sheetName.Length > 30)
                        sheetName = sheetName.Substring(0, 30);

                    var ws = wb.Worksheets.Add(sheetName);

                    // HEADER
                    ws.Cell(1, 1).Value = "DisContr_Id";
                    ws.Cell(1, 2).Value = "Dispatch_Id";
                    ws.Cell(1, 3).Value = "Contractor_Id";
                    ws.Cell(1, 4).Value = "Contractor_Name";
                    ws.Cell(1, 5).Value = "Customer";
                    ws.Cell(1, 6).Value = "PO_Ref_No";
                    ws.Cell(1, 7).Value = "Size_Of_Bag";
                    ws.Cell(1, 8).Value = "Actual_Quantity";
                    ws.Cell(1, 9).Value = "Act_Wt";
                    ws.Cell(1, 10).Value = "Total_Wt";
                    ws.Cell(1, 11).Value = "Rate";
                    ws.Cell(1, 12).Value = "Crate";
                    ws.Cell(1, 13).Value = "Kg_Or_Pcs";
                    ws.Cell(1, 14).Value = "Total_Cost";
                    ws.Cell(1, 15).Value = "Status";
                    ws.Cell(1, 16).Value = "Created_Date";

                    int r = 2;

                    foreach (var item in list)
                    {
                        ws.Cell(r, 1).Value = item.DisContr_Id;
                        ws.Cell(r, 2).Value = item.Dispatch_Id;
                        ws.Cell(r, 3).Value = item.Contractor_Id;
                        ws.Cell(r, 4).Value = item.Contractor_Name;
                        ws.Cell(r, 5).Value = item.Customer;
                        ws.Cell(r, 6).Value = item.PO_Ref_No;
                        ws.Cell(r, 7).Value = item.Size_Of_Bag;
                        ws.Cell(r, 8).Value = item.Actual_Quantity;
                        ws.Cell(r, 9).Value = item.Act_Wt;
                        ws.Cell(r, 10).Value = item.Total_Wt;
                        ws.Cell(r, 11).Value = item.Rate;
                        ws.Cell(r, 12).Value = item.Crate;
                        ws.Cell(r, 13).Value = item.Kg_Or_Pcs;
                        ws.Cell(r, 14).Value = item.Total_Cost;
                        ws.Cell(r, 15).Value = item.Status;
                        ws.Cell(r, 16).Value = item.Created_Date;

                        r++;
                    }

                    // TOTAL ROW
                    ws.Cell(r, 7).Value = "TOTAL";
                    ws.Cell(r, 8).Value = list.Sum(x => x.Actual_Quantity);
                    ws.Cell(r, 10).Value = list.Sum(x => x.Total_Wt);
                    ws.Cell(r, 14).Value = list.Sum(x => x.Total_Cost);

                    // STYLE
                    ws.Range(1, 1, 1, 16).Style.Font.Bold = true;
                    ws.Range(r, 1, r, 16).Style.Font.Bold = true;

                    int totalRows = rMain - 1;
                    int totalCols = 17;

                    ApplyExcelStyle(wsMain, totalRows, totalCols);
                    ws.Column(16).Style.DateFormat.Format = "dd-MM-yyyy";
                    ws.Column(14).Style.NumberFormat.Format = "₹ #,##0.00";

                    ws.Columns().AdjustToContents();
                }


                // =====================================================
                // ✅ DOWNLOAD
                // =====================================================
                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;

                    return File(
                        stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Dispatch_MDR_" + month + "_" + year + ".xlsx"
                    );
                }
            }
        }
        private void ApplyExcelStyle(IXLWorksheet ws, int totalRows, int totalCols)
        {
            // HEADER STYLE
            var header = ws.Range(1, 1, 1, totalCols);
            header.Style.Font.Bold = true;
            header.Style.Font.FontColor = XLColor.White;
            header.Style.Fill.BackgroundColor = XLColor.DarkBlue;
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // BORDERS (ALL CELLS)
            var fullRange = ws.Range(1, 1, totalRows, totalCols);
            fullRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            fullRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            // ALTERNATE ROW COLOR (ZEBRA)
            for (int i = 2; i <= totalRows; i++)
            {
                if (i % 2 == 0)
                {
                    ws.Row(i).Style.Fill.BackgroundColor = XLColor.LightGray;
                }
            }

            // FREEZE HEADER
            ws.SheetView.FreezeRows(1);

            // AUTO WIDTH
            ws.Columns().AdjustToContents();
        }

        [HttpPost]
        public JsonResult SaveSummaryReport(List<tblsummaryreport> model)
        {
            try
            {
                var month = model.FirstOrDefault()?.summary_month;

                var oldData = db.tblsummaryreports
                    .Where(x => x.Extra_Amt != null && x.summary_month == month)
                    .ToList();

                db.tblsummaryreports.RemoveRange(oldData);
                db.SaveChanges();

                foreach (var item in model)
                    {
                        db.tblsummaryreports.Add(new tblsummaryreport
                        {
                            Contractor_id = item.Contractor_id,
                            Contractor_Name = item.Contractor_Name,
                            Qty = item.Qty,
                            Amount = item.Amount,
                            Extra_Amt = item.Extra_Amt,
                            Non_Dispatch_Amt = item.Non_Dispatch_Amt,
                            Total_Amt = item.Total_Amt,
                            summary_month = item.summary_month,
                            Rate = item.Rate,
                            Belling_Amt = item.Belling_Amt,
                            Is_Customor = item.Is_Customor
                        });

                    db.SaveChanges();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public JsonResult GetSavedSummaryReport(int? contractorId, int? month, int? year)
        {
            try
            {

                    var data = db.tblsummaryreports
                        .Where(x => x.Extra_Amt !=null &&
                            (contractorId == null || contractorId == 0 || x.Contractor_id == contractorId) &&
                            (month == null || x.summary_month == month)
                        // if you add year column later, filter here
                        )
                        .ToList();

                    return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SaveBellingReport(List<tblsummaryreport> model)
        {
            try
            {
                var month = model.FirstOrDefault()?.summary_month;

                var oldData = db.tblsummaryreports
                    .Where(x => x.Extra_Amt == null && x.summary_month == month)
                    .ToList();

                db.tblsummaryreports.RemoveRange(oldData);
                db.SaveChanges();

                foreach (var item in model)
                {
                    db.tblsummaryreports.Add(new tblsummaryreport
                    {
                        Contractor_id = item.Contractor_id,
                        Contractor_Name = item.Contractor_Name,
                        Qty = item.Qty,
                        Amount = item.Amount,
                        Extra_Amt = item.Extra_Amt,
                        Non_Dispatch_Amt = item.Non_Dispatch_Amt,
                        Total_Amt = item.Total_Amt,
                        summary_month = item.summary_month,
                        Rate = item.Rate,
                        Belling_Amt = item.Belling_Amt,
                        Is_Customor = item.Is_Customor
                    });

                    db.SaveChanges();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public JsonResult GetSavedBellingReport(int month, bool isCustomer)
        {
            try
            {
                var data = db.tblsummaryreports
                    .Where(x => x.Extra_Amt == null && (month == null || x.summary_month == month) && x.Is_Customor == isCustomer)
                    .ToList();

                return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}