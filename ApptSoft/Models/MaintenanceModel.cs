using ApptSoft.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ApptSoft.Services;

namespace ApptSoft.Models
{
    public class MaintenanceModel
    {
        public int Id { get; set; }
        public string FlatNo { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string PaymentMode { get; set; }
        public string TransactionNo { get; set; }
        public string PaidDate { get; set; }
        public string DueDate { get; set; }
        public string Receipt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public string CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string UpdateDate { get; set; }

        public string SaveMaintenanceData(HttpPostedFileBase fb, MaintenanceModel model)
        {
            string msg = "Maintenance successfully Added ";
            string filepath = "";
            string fileName = "";
            string sysFileName = "";
            if (fb != null && fb.ContentLength > 0)
            {
                filepath = HttpContext.Current.Server.MapPath("../Content/Img/Maintenance");
                DirectoryInfo di = new DirectoryInfo(filepath);
                if (!di.Exists)
                {
                    di.Create();
                }
                
                fileName = fb.FileName;
                CommonService commonService = new CommonService(); 
                byte[] compressedImage = commonService.CompressImageTo400Kb(fb.InputStream,100);
                sysFileName = model.FlatNo+model.Month+model.Year.ToString() + Path.GetExtension(fb.FileName);
                System.IO.File.WriteAllBytes(filepath + "//" + sysFileName, compressedImage);
                if (!string.IsNullOrWhiteSpace(fb.FileName))
                {
                    string afileName = HttpContext.Current.Server.MapPath("../Content/Img/Maintenance") + "/" + sysFileName;
                }
            }
            ApptSoftEntities db = new ApptSoftEntities();
            {
                if (model.Id == 0)
                {
                    var eximaintenance = db.tblMaintenances.Where(p => p.FlatNo == model.FlatNo && p.Month==model.Month && p.Year==model.Year).FirstOrDefault();
                    if (eximaintenance == null)
                    {
                        var maintenanceData = new tblMaintenance()
                        {
                            // Id = model.Id,
                            FlatNo = model.FlatNo,
                            Month = model.Month,
                            Year = model.Year,
                            Amount = model.Amount,
                            PaymentMode = model.PaymentMode,
                            TransactionNo = model.TransactionNo,
                            PaidDate = Convert.ToDateTime(model.PaidDate),
                            DueDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-15")),
                            Receipt = sysFileName,
                            CreateBy = Convert.ToInt32(HttpContext.Current.Session["Id"].ToString()),
                            CreateDate = Convert.ToDateTime(DateTime.Now),
                            UpdateBy = Convert.ToInt32(HttpContext.Current.Session["Id"].ToString()),
                            UpdateDate = Convert.ToDateTime(DateTime.Now)
                        };
                        db.tblMaintenances.Add(maintenanceData);
                        db.SaveChanges();
                        msg = "Maintenance Saved Successfully";
                    }
                    else
                    {
                        msg = "Maintenance already added for FlatNo: "+ model.FlatNo +" of Month/Year: "+ model.Month+"/"+model.Year;
                    }
                }
                else
                {
                    var maintenanceData = db.tblMaintenances.Where(p => p.Id == model.Id).FirstOrDefault();
                    if (maintenanceData != null)
                    {
                        maintenanceData.FlatNo = model.FlatNo;
                        maintenanceData.Month = model.Month;
                        maintenanceData.Year = model.Year;
                        maintenanceData.Amount = model.Amount;
                        maintenanceData.PaymentMode = model.PaymentMode;
                        maintenanceData.TransactionNo = model.TransactionNo;
                        maintenanceData.PaidDate = Convert.ToDateTime(model.PaidDate);
                        maintenanceData.UpdateDate = DateTime.Now;
                        maintenanceData.UpdateBy  = Convert.ToInt32(HttpContext.Current.Session["Id"].ToString());
                    };
                    db.SaveChanges();
                    msg = "Maintenance Updated Successfully";
                }
            }
            return msg;
        }
        public List<MaintenanceModel> GetMaintenanceData()
        {
            ApptSoftEntities db = new ApptSoftEntities();
            List<MaintenanceModel> listob = new List<MaintenanceModel>();
            var maintenanceData = db.tblMaintenances.OrderByDescending(p => p.Id).ToList();
            if (maintenanceData != null)
            {
                foreach (var model in maintenanceData)
                {
                    listob.Add(new MaintenanceModel()
                    {
                        Id = model.Id,
                        FlatNo = model.FlatNo,
                        Month = model.Month,
                        Year = model.Year,
                        Amount = model.Amount,
                        PaymentMode = model.PaymentMode,
                        TransactionNo = model.TransactionNo,
                        PaidDate = model.PaidDate.Value.ToShortDateString(),
                        DueDate = model.DueDate.Value.ToShortDateString(),
                        Receipt = model.Receipt,
                        CreateBy = model.CreateBy,
                        CreateDate =model.CreateDate.ToString(),
                        UpdateBy = model.UpdateBy,
                        UpdateDate = model.UpdateDate.ToString(),
                    });
                }
            }
            return listob;
        }
        public string DeleteMaintenanceData(int Id)
        {
            string msg = "";
            ApptSoftEntities db = new ApptSoftEntities();
            var maintenanceData = db.tblMaintenances.Where(p => p.Id == Id).FirstOrDefault();
            if (maintenanceData != null)
            {
                db.tblMaintenances.Remove(maintenanceData);
            };
            db.SaveChanges();
            msg = "Record delete";
            return msg;
        }
        public MaintenanceModel GetMaintenanceData(int Id)
        {
            MaintenanceModel model = new MaintenanceModel();
            ApptSoftEntities db = new ApptSoftEntities();
            var maintenanceData = db.tblMaintenances.Where(p => p.Id == Id).FirstOrDefault();
            if (maintenanceData != null)

            {
                model.Id = maintenanceData.Id;
                model.Month = maintenanceData.Month;
                model.Year = maintenanceData.Year;
                model.FlatNo = maintenanceData.FlatNo;
                model.Amount = maintenanceData.Amount;
                model.PaymentMode = maintenanceData.PaymentMode;
                model.TransactionNo = maintenanceData.TransactionNo;
                model.Receipt = maintenanceData.Receipt;
                model.PaidDate = maintenanceData.PaidDate.Value.ToShortDateString();
                model.PaidDate = maintenanceData.PaidDate.Value.ToShortDateString();
                model.CreateBy = maintenanceData.CreateBy;
                model.CreateDate = maintenanceData.CreateDate.ToString();
                model.UpdateBy = maintenanceData.UpdateBy;
                model.UpdateDate = maintenanceData.UpdateDate.ToString();

            };
            return model;
        }
        public List<MaintenanceModel> GetMaintenanceDataseacr(string Year = null, string month = null, string flatNo = null)
        {
            ApptSoftEntities db = new ApptSoftEntities();
            List<MaintenanceModel> listob = new List<MaintenanceModel>();
            var maintenanceData = new List<tblMaintenance>();
            if (flatNo == "1")
            {
                maintenanceData = db.tblMaintenances.ToList().Where(x => x.Year == Year && x.Month == month).ToList();
            }
            else if(Year =="" && month=="")
            {
                maintenanceData = db.tblMaintenances.ToList().Where(x=> x.FlatNo == flatNo).ToList();
            }
             else
            {
                maintenanceData = db.tblMaintenances.ToList().Where(x => x.Year == Year && x.Month == month && x.FlatNo == flatNo).ToList();
            }
            if (maintenanceData != null)
            {
                foreach (var model in maintenanceData)
                {
                    listob.Add(new MaintenanceModel()
                    {
                        Id = model.Id,
                        FlatNo = model.FlatNo,
                        Month = model.Month,
                        Year = model.Year,
                        Amount = model.Amount,
                        PaymentMode = model.PaymentMode,
                        TransactionNo = model.TransactionNo,
                        PaidDate = model.PaidDate.Value.ToShortDateString(),
                        DueDate = model.DueDate.Value.ToShortDateString(),
                        Receipt = model.Receipt,
                        CreateBy = model.CreateBy,
                        CreateDate = model.CreateDate.ToString(),
                        UpdateBy = model.UpdateBy,
                        UpdateDate = model.CreateDate.ToString()
                    });
                }
            }
            return listob;
        }


    }
}