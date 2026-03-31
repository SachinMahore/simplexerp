using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApptSoft.Models
{
    public class ProductionTrackingModel
    {
        public int Tracking_Id { get; set; }

        public int JobCard_Id { get; set; }

        public int JCD_Id { get; set; }

        public string Operation_Name { get; set; }

        public int Input_Qty { get; set; }

        public int Output_Qty { get; set; }

        public int Wastage_Qty { get; set; }

        public string Machine_Name { get; set; }

        public DateTime Start_Time { get; set; }

        public DateTime End_Time { get; set; }

        public string Operator_Name { get; set; }

        public int Status { get; set; }
    }
}