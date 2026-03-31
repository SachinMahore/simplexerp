using AppSoft.Models;
using ApptSoft.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ApptSoft.Models
{
    public class AMCModel
    {
        public int Id { get; set; }
        public string FlatNo { get; set; }
        public string Title { get; set; }
        public string MobileNo { get; set; }
        public string VendorName { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string PaidDate { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public string PaidBy { get; set; }
        public string PaidMode { get; set; }
        public string TransactionNo { get; set; }
        public string Receipt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public string CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string UpdateDate { get; set; }

        public string SaveAMCData(HttpPostedFileBase fb, AMCModel model)
        {
            string msg = "Save";
            ApptSoftEntities db = new ApptSoftEntities();
            string filepath = "";
            string fileName = "";
            string sysFileName = "";
            if (fb != null && fb.ContentLength > 0)
            {
                filepath = HttpContext.Current.Server.MapPath("../Content/Img/");
                DirectoryInfo di = new DirectoryInfo(filepath);
                if (!di.Exists)
                {
                    di.Create();

                }
                fileName = fb.FileName;
                sysFileName = DateTime.Now.ToFileTime().ToString() + Path.GetExtension(fb.FileName);
                fb.SaveAs(filepath + "//" + sysFileName);
                if (!string.IsNullOrWhiteSpace(fb.FileName))
                {
                    string afileName = HttpContext.Current.Server.MapPath("../Content/Img") + "/" + sysFileName;
                }
            }
            if (model.Id == 0)
            {

                var AMCData = new tblAMC()
                {
                    Id = model.Id,
                    FlatNo = model.FlatNo,
                    Tittle = model.Title,
                    MobileNo= model.MobileNo,
                    VendorName = model.VendorName,
                    Amount = model.Amount,
                    PaidDate = Convert.ToDateTime(DateTime.Now),
                    PaidAmount = model.PaidAmount,
                    PaidBy = model.PaidBy,
                    PaidMode = model.PaidMode,
                    TransactionNo = model.TransactionNo,
                    Receipt = sysFileName,
                    CreateBy = model.CreateBy,
                    CreateDate = Convert.ToDateTime(DateTime.Now),
                    UpdateBy = model.UpdateBy,
                    UpdateDate = Convert.ToDateTime(DateTime.Now),
                };
                db.tblAMCs.Add(AMCData);
                db.SaveChanges();
            }
            else
            {
                var AMCData = db.tblAMCs.Where(p => p.Id == model.Id).FirstOrDefault();
                if (AMCData != null)
                {
                    AMCData.Id = model.Id;
                    AMCData.FlatNo = model.FlatNo;
                    AMCData.Tittle = model.Title;
                    AMCData.MobileNo = model.MobileNo;
                    AMCData.VendorName = model.VendorName;
                    AMCData.Amount = model.Amount;
                    AMCData.PaidDate = Convert.ToDateTime(DateTime.Now);
                    AMCData.PaidAmount = model.PaidAmount;
                    AMCData.PaidBy = model.PaidBy;
                    AMCData.PaidMode = model.PaidMode;
                    AMCData.TransactionNo = model.TransactionNo;
                    AMCData.Receipt = sysFileName;
                    AMCData.CreateBy = model.CreateBy;
                    AMCData.CreateDate = Convert.ToDateTime(DateTime.Now);
                    AMCData.UpdateBy = model.UpdateBy;
                    AMCData.UpdateDate = Convert.ToDateTime(DateTime.Now);
                };
                db.SaveChanges();
                msg = "UpdateSuccessfully";

            }
            return msg;
        }

        //ListCode
        public List<AMCModel> GetAMCList()
        {
            ApptSoftEntities db = new ApptSoftEntities();
            List<AMCModel> lstamc = new List<AMCModel>();
            var AMCData = db.tblAMCs.ToList();
            if (AMCData != null)
            {
                foreach (var model in AMCData)
                {
                    lstamc.Add(new AMCModel()
                    {
                        Id = model.Id,
                        FlatNo = model.FlatNo,
                        Title = model.Tittle,
                        MobileNo = model.MobileNo,
                        VendorName = model.VendorName,
                        Amount = model.Amount,
                        PaidDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        PaidAmount = model.PaidAmount,
                        PaidBy = model.PaidBy,
                        PaidMode = model.PaidMode,
                        TransactionNo = model.TransactionNo,
                        Receipt = model.Receipt,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UpdateBy = model.UpdateBy,
                        UpdateDate = DateTime.Now.ToString("dd/MM/yyyy"),

                    });
                }
            }
            return lstamc;
        }

        //DeleteCode
        public string DeleteAMC(int Id)
        {
            string msg = "";
            ApptSoftEntities db = new ApptSoftEntities();
            var AMCData = db.tblAMCs.Where(p => p.Id == Id).FirstOrDefault();
            if (AMCData != null)
            {
                db.tblAMCs.Remove(AMCData);
            };
            db.SaveChanges();
            msg = "Record delete";
            return msg;
        }

        //EditCode

        public AMCModel EditAMC(int Id)
        {

            AMCModel model = new AMCModel();
            ApptSoftEntities db = new ApptSoftEntities();
            var AMCData = db.tblAMCs.Where(p => p.Id == Id).FirstOrDefault();
            if (AMCData != null)
            {
                model.Id = Id;
                model.FlatNo = AMCData.FlatNo;
                model.Title = AMCData.Tittle;
                model.MobileNo = AMCData.MobileNo;
                model.VendorName = AMCData.VendorName;
                model.Amount = AMCData.Amount;
                model.PaidDate = AMCData.PaidDate.ToString();
                model.PaidAmount = AMCData.PaidAmount;
                model.PaidBy = AMCData.PaidBy;
                model.PaidMode = AMCData.PaidMode;
                model.TransactionNo = AMCData.TransactionNo;
                model.Receipt = AMCData.Receipt;
                model.CreateBy = AMCData.CreateBy;
                model.CreateDate = AMCData.CreateDate.ToString();
                model.UpdateBy = AMCData.UpdateBy;
                model.UpdateDate = AMCData.UpdateDate.ToString();
            };
            return model;
        }


    }
}
