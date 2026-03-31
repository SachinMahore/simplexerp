using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApptSoft.Models
{
    public class JobCardModel
    {
        public int JobCard_Id { get; set; }

        public string JobCard_Number { get; set; }

        public int PO_Id { get; set; }

        public string PO_Number { get; set; }

        public int Vendor_Id { get; set; }

        public int Company_Id { get; set; }

        public DateTime Job_Date { get; set; }

        public string Buyer_Name { get; set; }

        public int Status { get; set; }

        public List<JobCardDetailsModel> Items { get; set; }

    }
}