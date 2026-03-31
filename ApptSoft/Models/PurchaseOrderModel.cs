using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ApptSoft.Data;

namespace ApptSoft.Models
{
    public class PurchaseOrderModel
    {
        public int PO_Id { get; set; }

        public string PO_Number { get; set; }

        public DateTime PO_Date { get; set; }

        public int Vendor_Id { get; set; }
        public string Vendor_Name { get; set; }
        public int Company_Id { get; set; }

        public string Buyer_Name { get; set; }

        public string Buyer_Phone { get; set; }

        public string Ship_Via { get; set; }

        public Nullable<System.DateTime> Receive_By_Date { get; set; }

        public string Payment_Terms { get; set; }

        public Nullable<decimal> Sub_Total { get; set; }

        public Nullable<decimal> Discount { get; set; }

        public Nullable<decimal> Tax_Amount { get; set; }

        public Nullable<decimal> Grand_Total { get; set; }

        public Nullable<int> Status { get; set; }

        public Nullable<System.DateTime> Created_Date { get; set; }

        public List<PurchaseOrderDetailsModel> Items { get; set; }

        public Vendor_Master Vendor { get; set; }

        public Company_Master Company { get; set; }

        public List<PurchaseOrderModel> GetPurchaseOrders(DateTime poDate, string poNumber)
        {
            ApptSoftEntities db = new ApptSoftEntities();

            var query = from po in db.PurchaseOrders
                        join v in db.Vendor_Master
                        on po.Vendor_Id equals v.Vendor_Id
                        where DbFunctions.TruncateTime(po.PO_Date) == poDate.Date
                        select new PurchaseOrderModel
                        {
                            PO_Id = po.PO_Id,
                            PO_Number = po.PO_Number,
                            PO_Date = po.PO_Date,
                            Vendor_Name = v.Vendor_Name,
                            Grand_Total = po.Grand_Total,
                            Status = po.Status
                        };

            if (!string.IsNullOrEmpty(poNumber))
            {
                query = query.Where(x => x.PO_Number.Contains(poNumber));
            }

            return query.OrderByDescending(x => x.PO_Id).ToList();
        }

        public string SavePurchaseOrder(PurchaseOrderModel model)
        {
            string msg = "";

            ApptSoftEntities db = new ApptSoftEntities();

            if (model.PO_Id == 0)
            {
                var data = new PurchaseOrder()
                {
                    PO_Number = model.PO_Number,
                    PO_Date = model.PO_Date,

                    Vendor_Id = model.Vendor_Id,
                    Company_Id = model.Company_Id,

                    Buyer_Name = model.Buyer_Name,
                    Buyer_Phone = model.Buyer_Phone,

                    Ship_Via = model.Ship_Via,
                    Receive_By_Date = model.Receive_By_Date,

                    Payment_Terms = model.Payment_Terms,

                    Sub_Total = model.Sub_Total,
                    Discount = model.Discount,
                    Tax_Amount = model.Tax_Amount,

                    Grand_Total = model.Grand_Total,

                    Status = 0,
                    Created_Date = DateTime.Now
                };

                db.PurchaseOrders.Add(data);
                db.SaveChanges();

                int poId = data.PO_Id;

                if (model.Items != null)
                {
                    foreach (var item in model.Items)
                    {
                        var detail = new PurchaseOrder_Details()
                        {
                            PO_Id = poId,

                            Item_No = item.Item_No,
                            Item_Description = item.Item_Description,

                            Qty_Per_Bale = item.Qty_Per_Bale,
                            Bale_Count = item.Bale_Count,

                            Total_Quantity = item.Total_Quantity,

                            Unit = item.Unit,

                            Unit_Price = item.Unit_Price,
                            Line_Total = item.Line_Total
                        };

                        db.PurchaseOrder_Details.Add(detail);
                    }

                    db.SaveChanges();
                }

                msg = "Purchase Order Added Successfully";
            }
            else
            {
                var data = db.PurchaseOrders.FirstOrDefault(x => x.PO_Id == model.PO_Id);

                if (data != null)
                {
                    data.Vendor_Id = model.Vendor_Id;
                    data.Company_Id = model.Company_Id;

                    data.Buyer_Name = model.Buyer_Name;
                    data.Buyer_Phone = model.Buyer_Phone;

                    data.Ship_Via = model.Ship_Via;
                    data.Receive_By_Date = model.Receive_By_Date;

                    data.Payment_Terms = model.Payment_Terms;

                    data.Sub_Total = model.Sub_Total;
                    data.Discount = model.Discount;
                    data.Tax_Amount = model.Tax_Amount;

                    data.Grand_Total = model.Grand_Total;

                    db.SaveChanges();

                    msg = "Purchase Order Updated Successfully";
                }
            }

            return msg;
        }


        public PurchaseOrderModel GetById(int id)
        {
            ApptSoftEntities db = new ApptSoftEntities();

            var po = db.PurchaseOrders.FirstOrDefault(x => x.PO_Id == id);

            if (po == null)
                return null;

            PurchaseOrderModel model = new PurchaseOrderModel()
            {
                PO_Id = po.PO_Id,
                PO_Number = po.PO_Number,
                PO_Date = po.PO_Date,
                Vendor_Id = po.Vendor_Id,
                Company_Id = po.Company_Id,
                Buyer_Name = po.Buyer_Name,
                Buyer_Phone = po.Buyer_Phone,
                Ship_Via = po.Ship_Via,
                Receive_By_Date = po.Receive_By_Date,
                Payment_Terms = po.Payment_Terms,
                Sub_Total = po.Sub_Total,
                Discount = po.Discount,
                Tax_Amount = po.Tax_Amount,
                Grand_Total = po.Grand_Total,
                Status = po.Status
            };

            model.Items = new PurchaseOrderDetailsModel().GetItems(id);

            model.Vendor = db.Vendor_Master.FirstOrDefault(v => v.Vendor_Id == po.Vendor_Id);

            model.Company = db.Company_Master.FirstOrDefault(c => c.Company_Id == po.Company_Id);

            return model;
        }
        // SEARCH PURCHASE ORDERS
        public List<PurchaseOrderModel> SearchPO(string poNumber, int? vendorId, DateTime fromDate, DateTime toDate)
        {
            ApptSoftEntities db = new ApptSoftEntities();

            var query = from po in db.PurchaseOrders
                        join v in db.Vendor_Master
                        on po.Vendor_Id equals v.Vendor_Id
                        where DbFunctions.TruncateTime(po.PO_Date) >= fromDate.Date
                        && DbFunctions.TruncateTime(po.PO_Date) <= toDate.Date
                        select new PurchaseOrderModel
                        {
                            PO_Id = po.PO_Id,
                            PO_Number = po.PO_Number,
                            PO_Date = po.PO_Date,
                            Vendor_Id = po.Vendor_Id,
                            Vendor_Name = v.Vendor_Name,
                            Company_Id = po.Company_Id,
                            Grand_Total = po.Grand_Total,
                            Status = po.Status
                        };

            // Optional PO Number filter
            if (!string.IsNullOrEmpty(poNumber))
            {
                query = query.Where(x => x.PO_Number.Contains(poNumber));
            }

            // Optional Vendor filter
            if (vendorId != null && vendorId > 0)
            {
                query = query.Where(x => x.Vendor_Id == vendorId);
            }

            return query.OrderByDescending(x => x.PO_Id).ToList();
        }

    }
}