using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApptSoft.Models
{
    public class CompanyModel
    {
        public int Company_Id { get; set; }

        public string Company_Name { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip_Code { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }
        public bool Is_Active { get; set; }
    }
}