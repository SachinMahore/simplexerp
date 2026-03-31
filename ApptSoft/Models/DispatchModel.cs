using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApptSoft.Models
{
    public class DispatchModel
    {
        public int Dispatch_Id { get; set; }

        public int Sr_No { get; set; }
        public string Customer { get; set; }

        public string PO_Number { get; set; }
        public int PO_Id { get; set; }

        public string Size_Of_Bag { get; set; }

        public decimal Req_Wt { get; set; }
        public decimal Total_Req_Wt { get; set; }

        public int Actual_Quantity { get; set; }
        public decimal Act_Wt { get; set; }
        public decimal Total_Act_Wt { get; set; }

        public decimal Rate { get; set; }
        public string Kg_Or_Pcs { get; set; }

        public decimal Total_Cost { get; set; }

        public string Contractor { get; set; }

        public int Status { get; set; }

        public  string Billing_Date { get; set; }
        public string Item_No { get; set; }
        public string Container_No { get; set; }
        public string ORDER_QTY { get; set; }

    }
}