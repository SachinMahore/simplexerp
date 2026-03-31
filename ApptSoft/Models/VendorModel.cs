using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApptSoft.Models
{
    public class VendorModel
    {
        public int Vendor_Id { get; set; }

        public string Vendor_Name { get; set; }

        public string Contact_Person { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip_Code { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string GST_Number { get; set; }
    }
}