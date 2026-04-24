using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ApptSoft.Data;

namespace ApptSoft.Models
{
    public class PurchaseOrderModel
    {
        public int PO_Id { get; set; }

        public string Billing_Date { get; set; }
        public string Customer { get; set; }
        public string Container_No { get; set; }
        public string PO_Number { get; set; }
        public string Item_No { get; set; }
        public string Size_Of_Bag { get; set; }
        public string ORDER_QTY { get; set; }

        public decimal Req_Wt { get; set; }
        public decimal Total_Req_Wt { get; set; }
        public decimal Act_Wt { get; set; }
        public decimal Total_Act_Wt { get; set; }

        public decimal PcsPerPallet { get; set; }

        public string Attachment { get; set; } // ✅ correct

        public int Status { get; set; }
        public int CreatedBy { get; set; }
    }

}