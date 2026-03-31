using ApptSoft.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ApptSoft.Models
{
    public class TenantModel
    {
        public int Id { get; set; }
        public string FlatNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string PermanentAddress { get; set; }
        public string Adharcard { get; set; }
        public string Photo { get; set; }
        public string PoliceDocumentVerification { get; set; }
        public string RentedForm { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public string CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string UpdateDate { get; set; }

        public string SaveTenantData(HttpPostedFileBase fb, TenantModel model)
        {
            
            string msg = "Save";
            ApptSoftEntities Db = new ApptSoftEntities();
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

                var TenantData = new tblTenant()
                {
                    FlatNo = model.FlatNo,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MobileNo = model.MobileNo,
                    Email = model.Email,
                    PermanentAddress = model.PermanentAddress,
                    Adharcard = model.Adharcard,
                    Photo = sysFileName,
                    PoliceDocumentVerification = model.PoliceDocumentVerification,
                    RentedForm = Convert.ToDateTime(model.RentedForm),
                    IsActive = model.IsActive,
                    CreateBy = model.CreateBy,
                    CreateDate = Convert.ToDateTime(DateTime.Now),
                    UpdateBy = model.UpdateBy,
                    UpdateDate = Convert.ToDateTime(DateTime.Now),

                };
                Db.tblTenants.Add(TenantData);
                Db.SaveChanges();

            }
            else
            {
                var TenantData = Db.tblTenants.Where(p => p.Id == model.Id).FirstOrDefault();
                if (TenantData != null)
                {
                    TenantData.Id = model.Id;
                    TenantData.FlatNo = model.FlatNo;
                    TenantData.FirstName = model.FirstName;
                    TenantData.LastName = model.LastName;
                    TenantData.MobileNo = model.MobileNo;
                    TenantData.Email = model.Email;
                    TenantData.PermanentAddress = model.PermanentAddress;
                    TenantData.Adharcard = model.Adharcard;
                    TenantData.Photo = sysFileName;
                    TenantData.PoliceDocumentVerification = model.PoliceDocumentVerification;
                    TenantData.RentedForm = Convert.ToDateTime(model.RentedForm);
                    TenantData.IsActive = model.IsActive;
                    TenantData.CreateBy = model.CreateBy;
                    TenantData.CreateDate = Convert.ToDateTime(DateTime.Now);
                    TenantData.UpdateBy = model.UpdateBy;
                    TenantData.UpdateDate = Convert.ToDateTime(DateTime.Now);

                };
                Db.SaveChanges();
                msg = "Updated Successfully";

            }
            return msg;
        }

        //ListCode
        public List<TenantModel> GetTenantList()
        {
            ApptSoftEntities db = new ApptSoftEntities();
            List<TenantModel> lstTenant = new List<TenantModel>();
            var TenantData = db.tblTenants.ToList();
            if (TenantData != null)
            {
                foreach (var Tenant in TenantData)
                {
                    lstTenant.Add(new TenantModel()
                    {
                        Id = Tenant.Id,
                        FlatNo = Tenant.FlatNo,
                        FirstName = Tenant.FirstName,
                        LastName = Tenant.LastName,
                        MobileNo = Tenant.MobileNo,
                        Email = Tenant.Email,
                        PermanentAddress = Tenant.PermanentAddress,
                        Adharcard = Tenant.Adharcard,
                        Photo = Tenant.Photo,
                        PoliceDocumentVerification = Tenant.PoliceDocumentVerification,
                        RentedForm = Tenant.RentedForm.Value.ToShortDateString(),
                        IsActive = Tenant.IsActive,
                        CreateBy = Tenant.CreateBy,
                        CreateDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        UpdateBy = Tenant.UpdateBy,
                        UpdateDate = DateTime.Now.ToString("dd/MM/yyyy"),

                    });
                }
            }
            return lstTenant;
        }

        //DeleteCode
        public string DeleteTenantData(int Id)
        {
            string msg = "";
            ApptSoftEntities db = new ApptSoftEntities();
            var TenantData = db.tblTenants.Where(p => p.Id == Id).FirstOrDefault();
            if (TenantData != null)
            {
                db.tblTenants.Remove(TenantData);
            };
            db.SaveChanges();
            msg = "Record delete";
            return msg;
        }

        //EditCode
        public TenantModel EditTenantData(int Id)
        {

            TenantModel model = new TenantModel();
            ApptSoftEntities db = new ApptSoftEntities();
            var TenantData = db.tblTenants.Where(p => p.Id == Id).FirstOrDefault();
            if (TenantData != null)
            {
                model.Id = TenantData.Id;
                model.FlatNo = TenantData.FlatNo;
                model.FirstName = TenantData.FirstName;
                model.LastName = TenantData.LastName;
                model.MobileNo = TenantData.MobileNo;
                model.Email = TenantData.Email;
                model.PermanentAddress = TenantData.PermanentAddress;
                // model.Adharcard = TenantData.Adharcard;
                model.Photo = TenantData.Photo;
                // model.PoliceDocumentVerification = TenantData.PoliceDocumentVerification;
                model.RentedForm = TenantData.RentedForm.Value.ToShortDateString();
                model.IsActive = TenantData.IsActive;
                model.CreateBy = TenantData.CreateBy;
                model.CreateDate = TenantData.CreateDate.ToString();
                model.UpdateBy = TenantData.UpdateBy;
                model.UpdateDate = TenantData.UpdateDate.ToString();



            };
            return model;
        }
    }
}