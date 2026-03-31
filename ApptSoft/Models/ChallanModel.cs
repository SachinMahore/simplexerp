using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ApptSoft.Data;

namespace ApptSoft.Models
{
    public class ChallanModel
    {
        public int ChallanID { get; set; }

        public string ChallanNo { get; set; }

        public Nullable<System.DateTime> Date { get; set; }

        public string AshUtilizationFor { get; set; }

        public string PartyName { get; set; }

        public string PartyLocation { get; set; }

        public string VehicleNo { get; set; }

        public string VehicleType { get; set; }

        public Nullable<int> NoOfWheels { get; set; }

        public string GatePassNo { get; set; }

        public Nullable<System.DateTime> GatePassValidity { get; set; }

        public string CommodityType { get; set; }

        public string AshPickupLocation { get; set; }

        public string AshUnloadLocation { get; set; }

        public Nullable<System.DateTime> CreatedOn { get; set; }

        public Nullable<System.DateTime> UpdatedOn { get; set; }

        public Nullable<int> CreatedBy { get; set; }

        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> Status { get; set; }

        //public List<ChallanModel> GetChallans(DateTime visitDate)
        //{
        //    ApptSoftEntities db = new ApptSoftEntities();
        //    List<ChallanModel> lstVisitor = new List<ChallanModel>();

        //    var challanData = db.DeliveryChallans.Where(v => DbFunctions.TruncateTime(v.Date) == visitDate.Date)
        //   .OrderByDescending(v => v.ChallanID).ToList();
        //    if (challanData != null)
        //    {
        //        foreach (var challan in challanData)
        //        {
        //            lstVisitor.Add(new ChallanModel()
        //            {
        //                ChallanID = challan.ChallanID,
        //                ChallanNo = challan.ChallanNo,
        //                Date = challan.Date,

        //                AshUtilizationFor = challan.AshUtilizationFor,

        //                PartyName = challan.PartyName,
        //                PartyLocation = challan.PartyLocation,

        //                VehicleNo = challan.VehicleNo,
        //                VehicleType = challan.VehicleType,
        //                NoOfWheels = challan.NoOfWheels,

        //                GatePassNo = challan.GatePassNo,
        //                GatePassValidity = challan.GatePassValidity,

        //                CommodityType = challan.CommodityType,
        //                AshPickupLocation = challan.AshPickupLocation,
        //                AshUnloadLocation = challan.AshUnloadLocation,

        //                CreatedOn = challan.CreatedOn,
        //                Status= challan.Status
                        
        //            });
        //        }
        //    }

        //    return lstVisitor;
        //}
        //public string SaveChallan(ChallanModel model)
        //{
        //    string msg = "";
        //    ApptSoftEntities db = new ApptSoftEntities();
        //    var getVehicle = db.DeliveryChallans.Where(x => x.VehicleNo == model.VehicleNo && x.Status==0).FirstOrDefault();
        //    if (getVehicle != null)
        //    {
        //        msg = "Vehicle Already loaded for Challan No.: " + getVehicle.ChallanNo;
        //    }

        //    else if (model.ChallanID == 0)
        //    {
        //        var data = new DeliveryChallan()
        //        {
        //            ChallanNo = model.ChallanNo,
        //            Date = model.Date,
        //            AshUtilizationFor = model.AshUtilizationFor,

        //            PartyName = model.PartyName,
        //            PartyLocation = model.PartyLocation,

        //            VehicleNo = model.VehicleNo,
        //            VehicleType = model.VehicleType,
        //            NoOfWheels = model.NoOfWheels,

        //            GatePassNo = model.GatePassNo,
        //            GatePassValidity = model.GatePassValidity,

        //            CommodityType = model.CommodityType,
        //            AshPickupLocation = model.AshPickupLocation,
        //            AshUnloadLocation = model.AshUnloadLocation,
        //            Status=0,
        //            CreatedOn = DateTime.Now
        //        };

        //        db.DeliveryChallans.Add(data);
        //        db.SaveChanges();
        //        msg = "Challan Added Successfully";
        //    }
        //    else
        //    {
        //        var data = db.DeliveryChallans.FirstOrDefault(x => x.ChallanID == model.ChallanID);
        //        if (data != null)
        //        {
        //            //data.ChallanNo = model.ChallanNo;
        //            //data.Date = model.Date;
        //            //data.AshUtilizationFor = model.AshUtilizationFor;

        //            //data.PartyName = model.PartyName;
        //            //data.PartyLocation = model.PartyLocation;

        //            //data.VehicleNo = model.VehicleNo;
        //            //data.VehicleType = model.VehicleType;
        //            //data.NoOfWheels = model.NoOfWheels;

        //            //data.GatePassNo = model.GatePassNo;
        //            //data.GatePassValidity = model.GatePassValidity;

        //            //data.CommodityType = model.CommodityType;
        //            //data.AshPickupLocation = model.AshPickupLocation;
        //            //data.AshUnloadLocation = model.AshUnloadLocation;
        //            data.Status= model.Status;
        //            //data.CreatedOn = DateTime.Now;

        //            db.SaveChanges();
        //            msg = "Challan Updated Successfully";
        //        }
        //    }

        //    return msg;
        //}

        //public ChallanModel GetById(int id)
        //{
        //    ApptSoftEntities db = new ApptSoftEntities();
        //    var challan = db.DeliveryChallans.Where(x => x.ChallanID == id).FirstOrDefault();
        //    if (challan == null)
        //        return null;

        //    // Map Entity to Model
        //    ChallanModel model = new ChallanModel()
        //    {
        //        ChallanID = challan.ChallanID,
        //        ChallanNo = challan.ChallanNo,
        //        PartyName = challan.PartyName,
        //        PartyLocation = challan.PartyLocation,
        //        Date = challan.Date,
        //        VehicleNo = challan.VehicleNo,
        //        VehicleType = challan.VehicleType,
        //        NoOfWheels = challan.NoOfWheels,
        //        GatePassNo = challan.GatePassNo,
        //        GatePassValidity = challan.GatePassValidity,
        //        AshUtilizationFor = challan.AshUtilizationFor,
        //        AshPickupLocation = challan.AshPickupLocation,
        //        AshUnloadLocation = challan.AshUnloadLocation,
        //        CommodityType = challan.CommodityType,
        //        Status = challan.Status
        //        // Add remaining fields here
        //    };
        //    return model;          
        //}

    }
}