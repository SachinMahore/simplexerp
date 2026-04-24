using ApptSoft.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApptSoft.Models
{
    public class JobCardFullModel
    {
        public FIBC_JobCard JobCard { get; set; }
        public FIBC_FabricDetails Fabric { get; set; }
        public FIBC_ComponentDetails Component { get; set; }
    }
}