using ApptSoft.Data;
using ApptSoft.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace ApptSoft.Models
{
    public class ComplaintModel
    {
        public int Id { get; set; }
        public string FlatNo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Complaint { get; set; }
        public string ComplaintDate { get; set; }
        public Nullable<bool> Status { get; set; }
        public string ResolvedBy { get; set; }
        public string ResolveDate { get; set; }
        public string Proof { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public string CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string UpdateDate { get; set; }


        public string SaveComplaint(HttpPostedFileBase fb, ComplaintModel model)
        {
            string msg = "Save SuccessFully";
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
                CommonService commonService = new CommonService();
                byte[] compressedImage = commonService.CompressImageTo400Kb(fb.InputStream, 100);
                sysFileName = DateTime.Now.ToFileTime().ToString() + Path.GetExtension(fb.FileName);
                System.IO.File.WriteAllBytes(filepath + "//" + sysFileName, compressedImage);
                if (!string.IsNullOrWhiteSpace(fb.FileName))
                {
                    string afileName = HttpContext.Current.Server.MapPath("../Content/Img") + "/" + sysFileName;
                }
            }

            if (model.Id == 0)
            {

                var ComplaintData = new tblComplaint()
                {
                    Id = model.Id,
                    FlatNo = model.FlatNo,
                    Title = model.Title,
                    Description = model.Description,
                    Complaint = model.Complaint,
                    ComplaintDate = DateTime.Now,
                    Status = false,
                    ResolvedBy = model.ResolvedBy,
                    ResolveDate = Convert.ToDateTime(model.ResolveDate),
                    Proof = sysFileName,
                    CreateBy  = Convert.ToInt32(HttpContext.Current.Session["Id"].ToString()),
                    CreateDate = Convert.ToDateTime(DateTime.Now),
                    UpdateBy = Convert.ToInt32(HttpContext.Current.Session["Id"].ToString()),
                    UpdateDate = Convert.ToDateTime(DateTime.Now),
                };
                db.tblComplaints.Add(ComplaintData);
                db.SaveChanges();
            }
            else
            {
                var ComplaintData = db.tblComplaints.Where(p => p.Id == model.Id).FirstOrDefault();
                if (ComplaintData != null)
                {
                    ComplaintData.Id = model.Id;
                    ComplaintData.FlatNo = model.FlatNo;
                    ComplaintData.Title = model.Title;
                    ComplaintData.Description = model.Description;
                    ComplaintData.Complaint = model.Complaint;
                    ComplaintData.Status = true;
                    ComplaintData.ResolvedBy = model.ResolvedBy;
                    ComplaintData.ResolveDate = Convert.ToDateTime(model.ResolveDate);
                    //ComplaintData.Proof = sysFileName;

                    ComplaintData.UpdateBy = Convert.ToInt32(HttpContext.Current.Session["Id"].ToString());
                    ComplaintData.UpdateDate = Convert.ToDateTime(DateTime.Now);
                };
                db.SaveChanges();
                msg = "Update Successfully";

            }
            return msg;
        }

        //ListCode

        public List<ComplaintModel> GetComplaint()
        {
            ApptSoftEntities db = new ApptSoftEntities();

            List<ComplaintModel> lstComplaint = new List<ComplaintModel>();
            var ComplaintData = db.tblComplaints.OrderByDescending(p => p.Id).ToList();
            if (ComplaintData != null)
            {
                foreach (var complaint in ComplaintData)
                {
                    lstComplaint.Add(new ComplaintModel()
                    {
                        Id = complaint.Id,
                        FlatNo = complaint.FlatNo,
                        Title = complaint.Title,
                        Description = complaint.Description,
                        Complaint = complaint.Complaint,
                        ComplaintDate = complaint.ComplaintDate.Value.ToShortDateString(),
                        Status = complaint.Status,
                        ResolvedBy = complaint.ResolvedBy,
                        ResolveDate = complaint.ResolveDate.Value.ToShortDateString(),
                        Proof = complaint.Proof,
                        CreateBy = complaint.CreateBy,
                        CreateDate =complaint.CreateDate.ToString(),
                        UpdateBy = complaint.UpdateBy,
                        UpdateDate = complaint.UpdateDate.ToString(),
                    });
                }
            }
            return lstComplaint;

        }


        //DeleteCode

        public string DeleteComplaint(int Id)
        {
            string msg = "";
            ApptSoftEntities db = new ApptSoftEntities();
            {
                var ComplaintData = db.tblComplaints.Where(p => p.Id == Id).FirstOrDefault();
                if (ComplaintData != null)
                {
                    db.tblComplaints.Remove(ComplaintData);
                };
                db.SaveChanges();
                msg = "Complaint delete";
                return msg;

            }    //EditCode


        }
        public ComplaintModel EditComplaint(int Id)
        {
            ComplaintModel model = new ComplaintModel();
            ApptSoftEntities db = new ApptSoftEntities();
            var ComplaintData = db.tblComplaints.Where(p => p.Id == Id).FirstOrDefault();
            if (ComplaintData != null)
            {
                model.Id = Id;
                model.FlatNo = ComplaintData.FlatNo;
                model.Title = ComplaintData.Title;
                model.Description = ComplaintData.Description;
                model.Complaint = ComplaintData.Complaint;
                model.ComplaintDate = ComplaintData.ComplaintDate.Value.ToShortDateString();
                model.ResolvedBy = ComplaintData.ResolvedBy;
                model.ResolveDate = ComplaintData.ResolveDate.Value.ToShortDateString();
                model.Proof = ComplaintData.Proof;
                model.Status = ComplaintData.Status;
            };
            return model;
        }


        //DetailCode


        public ComplaintModel ComplaintDetail(int Id)
        {
            ComplaintModel model = new ComplaintModel();
            ApptSoftEntities db = new ApptSoftEntities();
            var ComplaintData = db.tblComplaints.Where(p => p.Id == Id).FirstOrDefault();
            if (ComplaintData != null)
            {
                model.Id = Id;
                model.FlatNo = ComplaintData.FlatNo;
                model.Title = ComplaintData.Title;
                model.Description = ComplaintData.Description;
                model.Complaint = ComplaintData.Complaint;
                model.ComplaintDate = ComplaintData.ComplaintDate.Value.ToShortDateString();
                model.Status = ComplaintData.Status;
                model.ResolvedBy = ComplaintData.ResolvedBy;
                model.ResolveDate = ComplaintData.ResolveDate.Value.ToShortDateString();
                model.Proof = ComplaintData.Proof;


            };
            return model;
        }
    }
}