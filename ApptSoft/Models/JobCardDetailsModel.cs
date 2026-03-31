using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApptSoft.Models
{
    public class JobCardDetailsModel
    {
        public int JCD_Id { get; set; }

        public int JobCard_Id { get; set; }

        public string Item_No { get; set; }

        public string Item_Description { get; set; }

        public int Qty_Per_Bale { get; set; }

        public int Bale_Count { get; set; }

        public int Total_Quantity { get; set; }

        public string Unit { get; set; }

        public string Process_Type { get; set; }

        public int Completed_Qty { get; set; }
    }
}