using System;
using System.Collections.Generic;
using System.Linq;
using ApptSoft.Data;

namespace ApptSoft.Models
{
    public class PurchaseOrderDetailsModel
    {
        public int POD_Id { get; set; }

        public int PO_Id { get; set; }

        public string Item_No { get; set; }

        public string Item_Description { get; set; }

        public Nullable<int> Qty_Per_Bale { get; set; }

        public Nullable<int> Bale_Count { get; set; }

        public Nullable<int> Total_Quantity { get; set; }

        public string Unit { get; set; }

        public Nullable<decimal> Unit_Price { get; set; }

        public Nullable<decimal> Line_Total { get; set; }


        public List<PurchaseOrderDetailsModel> GetItems(int poId)
        {
            ApptSoftEntities db = new ApptSoftEntities();

            List<PurchaseOrderDetailsModel> items = new List<PurchaseOrderDetailsModel>();

            var data = db.PurchaseOrder_Details
                .Where(x => x.PO_Id == poId)
                .ToList();

            if (data != null)
            {
                foreach (var item in data)
                {
                    items.Add(new PurchaseOrderDetailsModel()
                    {
                        POD_Id = item.POD_Id,
                        PO_Id = item.PO_Id,

                        Item_No = item.Item_No,
                        Item_Description = item.Item_Description,

                        Qty_Per_Bale = item.Qty_Per_Bale,
                        Bale_Count = item.Bale_Count,
                        Total_Quantity = item.Total_Quantity,

                        Unit = item.Unit,

                        Unit_Price = item.Unit_Price,
                        Line_Total = item.Line_Total
                    });
                }
            }

            return items;
        }
    }
}