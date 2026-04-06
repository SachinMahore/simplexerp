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
            // =====================================================
            // ✅ FETCH DISPATCHES (ONLY REQUIRED DATA)
            // =====================================================
            var dispatches = db.tblDispatches
                .AsNoTracking()
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
                    d.Total_Req_Wt
                })
                .ToList();

            // =====================================================
            // ✅ FETCH & GROUP CONTRACTORS (ONCE)
            // =====================================================
            var contractorMap = db.tblDispatchContractors
                .AsNoTracking()
                .GroupBy(c => c.Dispatch_Id)
                .ToDictionary(
                    g => g.Key,
                    g => string.Join(", ", g.Select(x => x.Contractor_Name))
                );

            // =====================================================
            // ✅ MERGE DATA (NO EXTRA DB CALLS)
            // =====================================================
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

                    ContractorNames = contractorMap.ContainsKey(d.Dispatch_Id)
                        ? contractorMap[d.Dispatch_Id]
                        : ""
                })
                .OrderBy(x => x.Billing_Date)   // better than re-ordering twice
                .ToList();

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
                wb.CalculateMode = XLCalculateMode.Manual;

                // =====================================================
                // ✅ FETCH DISPATCH DATA (SAFE + OPTIMIZED)
                // =====================================================
                var dispatchList = db.tblDispatches
                    .AsNoTracking()
                    .Where(x => x.Billing_Date.HasValue &&
                                x.Billing_Date.Value.Month == month &&
                                x.Billing_Date.Value.Year == year)
                    .Select(x => new
                    {
                        x.Dispatch_Id,
                        x.Sr_No,
                        x.Customer,
                        x.PO_Number,
                        x.PO_Id,
                        x.Size_Of_Bag,
                        x.Req_Wt,
                        x.Total_Req_Wt,
                        x.Actual_Quantity,
                        x.Act_Wt,
                        x.Total_Act_Wt,
                        x.Rate,
                        x.Kg_Or_Pcs,
                        x.Total_Cost,
                        x.Contractor,
                        x.Status,
                        x.Created_Date
                    })
                    .ToList();

                // =====================================================
                // ✅ FETCH CONTRACTOR DATA (DB JOIN OPTIMIZED)
                // =====================================================
                var contractorList = (
                    from q in db.tblDispatchContractors.AsNoTracking()
                    join d in db.tblDispatches.AsNoTracking()
                        on q.Dispatch_Id equals d.Dispatch_Id into dj
                    from d in dj.DefaultIfEmpty()
                    where d.Billing_Date.HasValue &&
                          d.Billing_Date.Value.Month == month &&
                          d.Billing_Date.Value.Year == year
                    select new
                    {
                        q.Contractor_Id,
                        q.Contractor_Name,
                        Customer = d.Customer,
                        q.PO_Ref_No,
                        q.Size_Of_Bag,
                        d.Item_No,
                        q.Actual_Quantity,
                        q.Act_Wt,
                        q.Total_Wt,
                        q.Rate,
                        q.Total_Cost,
                        q.Kg_Or_Pcs,
                        q.Created_Date,
                        Container_No = d.Container_No
                    }
                ).ToList();

                // =====================================================
                // ✅ PRE-CALCULATIONS (PERFORMANCE)
                // =====================================================
                var totalDispatch = dispatchList.Count;
                var totalQty = dispatchList.Sum(x => x.Actual_Quantity ?? 0);
                var totalWt = dispatchList.Sum(x => x.Total_Act_Wt ?? 0);
                var totalCost = dispatchList.Sum(x => x.Total_Cost ?? 0);
                var totalContractors = contractorList
                                        .Select(x => x.Contractor_Id)
                                        .Distinct()
                                        .Count();

                // =====================================================
                // ✅ DASHBOARD SHEET
                // =====================================================
                var wsDash = wb.Worksheets.Add("Dashboard");

                wsDash.Cell("A1").Value = $"DISPATCH DASHBOARD - {month}/{year}";
                wsDash.Range("A1:I1").Merge().Style
                    .Font.SetBold().Font.FontSize = 16;
                wsDash.Range("A1:I1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                wsDash.Cell("A3").Value = "Total Dispatch";
                wsDash.Cell("B3").Value = totalDispatch;

                wsDash.Cell("A4").Value = "Total Quantity";
                wsDash.Cell("B4").Value = totalQty;

                wsDash.Cell("A5").Value = "Total Weight";
                wsDash.Cell("B5").Value = totalWt;

                wsDash.Cell("A6").Value = "Total Cost";
                wsDash.Cell("B6").Value = totalCost;

                wsDash.Cell("A7").Value = "Total Contractors";
                wsDash.Cell("B7").Value = totalContractors;

                wsDash.Range("A3:B7").Style.Fill.BackgroundColor = XLColor.LightBlue;

                // =====================================================
                // ✅ MAIN DISPATCH SHEET
                // =====================================================
                var wsMain = wb.Worksheets.Add($"Dispatch-{month}-{year}");

                wsMain.Cell(1, 1).InsertTable(dispatchList);

                wsMain.Columns().AdjustToContents();

                wsMain.Column(17).Style.DateFormat.Format = "dd-MM-yyyy";
                wsMain.Column(14).Style.NumberFormat.Format = "₹ #,##0.00";

                // =====================================================
                // ✅ GROUP CONTRACTORS
                // =====================================================
                var contractorGroups = contractorList
                    .GroupBy(x => new { x.Contractor_Id, x.Contractor_Name })
                    .ToList();

                // =====================================================
                // ✅ CONTRACTOR SHEETS
                // =====================================================
                foreach (var group in contractorGroups)
                {
                    var contractor = group.Key;
                    var list = group.OrderByDescending(x => x.Created_Date).ToList();

                    // Safe sheet name
                    string sheetName = contractor.Contractor_Name ?? "Contractor";
                    sheetName = sheetName.Length > 30 ? sheetName.Substring(0, 30) : sheetName;

                    string originalName = sheetName;
                    int i = 1;

                    while (wb.Worksheets.Any(w => w.Name == sheetName))
                    {
                        sheetName = originalName + "_" + i++;
                    }

                    var ws = wb.Worksheets.Add(sheetName);

                    // Bulk insert
                    ws.Cell(1, 1).InsertTable(list);

                    int lastRow = list.Count + 2;

                    // TOTAL ROW
                    ws.Cell(lastRow, 7).Value = "TOTAL";
                    ws.Cell(lastRow, 8).Value = list.Sum(x => x.Actual_Quantity ?? 0);
                    ws.Cell(lastRow, 10).Value = list.Sum(x => x.Total_Wt ?? 0);
                    ws.Cell(lastRow, 14).Value = list.Sum(x => x.Total_Cost ?? 0);

                    ws.Range(lastRow, 1, lastRow, 16).Style.Font.Bold = true;

                    // Formatting
                    ws.Column(14).Style.NumberFormat.Format = "₹ #,##0.00";
                    ws.Column(16).Style.DateFormat.Format = "dd-MM-yyyy";

                    ws.Columns().AdjustToContents();
                }

                // =====================================================
                // ✅ DOWNLOAD (MEMORY OPTIMIZED)
                // =====================================================
                var stream = new MemoryStream();
                wb.SaveAs(stream);
                stream.Position = 0;

                return File(
                    stream,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Dispatch_MDR_{month}_{year}.xlsx"
                );
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