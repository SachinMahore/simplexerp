using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using ApptSoft.Data;
using ApptSoft.Services;

namespace ApptSoft.Models
{
    public class VisitorModel
    {
        public int Id { get; set; }

        [Required]
        public string FlatNo { get; set; }

        [Required]
        [Display(Name = "Visitor Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Visitor Address")]
        public string Address { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Visitor Mobile")]
        public string Mobile { get; set; }

        [Display(Name = "Visitor Photo")]
        public string Photo { get; set; } // If storing image as byte[]; otherwise use IFormFile or HttpPostedFileBase in controller/viewmodel

        [Required]
        [Display(Name = "Visitor Type")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Visit Frequency")]
        public string Frequency { get; set; }

        [Required]

        public string VisitDate { get; set; }

        [Required]

        public string VisitTime { get; set; }

        [Display(Name = "Vehicle No.")]
        public string VehicleNo { get; set; }

        public bool Consent { get; set; } = true; // Always true in your form, but useful for data completeness
        public int? CreateBy { get; set; }
        public string CreateDate { get; set; }
        [Display(Name = "Visitor Details")]
        public string VisitorDetails { get; set; }

        [Display(Name = "No. of Persons")]
        public int? NoOfPerson { get; set; }
        public string SaveVisitor(HttpPostedFileBase fb, VisitorModel model)
        {
            string msg = "Visitor Added Successfully";
            ApptSoftEntities db = new ApptSoftEntities();

            string filepath = "";
            string fileName = "";
            string sysFileName = "";

            if (fb != null && fb.ContentLength > 0)
            {
                filepath = HttpContext.Current.Server.MapPath("~/Content/Img/Visitor/");
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
                //fb.SaveAs(Path.Combine(filepath, sysFileName));
            }

            if (model.Id == 0)
            {
                var visitorData = new tblVisitor()
                {
                    FlatNo = model.FlatNo,
                    Name = model.Name,
                    Address = model.Address,
                    Mobile = model.Mobile,
                    Photo = sysFileName,
                    Type = model.Type,
                    Frequency = model.Frequency,
                    VisitDate =Convert.ToDateTime(model.VisitDate),
                    VisitTime =TimeSpan.Parse(model.VisitTime),
                    VehicleNo = model.VehicleNo,
                    Consent = model.Consent,
                    CreateBy = Convert.ToInt32(HttpContext.Current.Session["Id"]),
                    CreateDate = DateTime.Now,
                    VisitorDetails = model.VisitorDetails,
                    NoOfPerson = model.NoOfPerson,
                };

                db.tblVisitors.Add(visitorData);
                db.SaveChanges();
            }
            else
            {
                var visitorData = db.tblVisitors.FirstOrDefault(v => v.Id == model.Id);
                if (visitorData != null)
                {
                    visitorData.FlatNo = model.FlatNo;
                    visitorData.Name = model.Name;
                    visitorData.Address = model.Address;
                    visitorData.Mobile = model.Mobile;

                    visitorData.Type = model.Type;
                    visitorData.Frequency = model.Frequency;
                    visitorData.VisitDate = Convert.ToDateTime(model.VisitDate);
                    visitorData.VisitTime = TimeSpan.Parse(model.VisitTime);
                    visitorData.VehicleNo = model.VehicleNo;
                    visitorData.Consent = model.Consent;
                    visitorData.VisitorDetails = model.VisitorDetails;
                    visitorData.NoOfPerson = model.NoOfPerson;
                    visitorData.CreateBy = Convert.ToInt32(HttpContext.Current.Session["Id"].ToString());
                    visitorData.CreateDate=DateTime.Now;
                    db.SaveChanges();
                    msg = "Visitor Updated Successfully";
                }
            }

            return msg;
        }

        public List<VisitorModel> GetVisitors(DateTime visitDate)
        {
            ApptSoftEntities db = new ApptSoftEntities();
            List<VisitorModel> lstVisitor = new List<VisitorModel>();

            var visitorData = db.tblVisitors.Where(v => DbFunctions.TruncateTime(v.VisitDate) == visitDate.Date)
           .OrderByDescending(v => v.Id).ToList();
            if (visitorData != null)
            {
                foreach (var visitor in visitorData)
                {
                    lstVisitor.Add(new VisitorModel()
                    {
                        Id = visitor.Id,
                        FlatNo = visitor.FlatNo,
                        Name = visitor.Name,
                        Address = visitor.Address,
                        Mobile = visitor.Mobile,
                         Photo = visitor.Photo, // Optional: usually omitted unless converting to base64
                        Type = visitor.Type,
                        Frequency = visitor.Frequency,
                        VisitDate = visitor.VisitDate.ToShortDateString(),
                        VisitTime = visitor.VisitTime.ToString(), // or .ToString(@"hh\:mm")
                        VehicleNo = visitor.VehicleNo,
                        Consent = visitor.Consent,
                        CreateBy = visitor.CreateBy,
                        CreateDate = visitor.CreateDate.ToString(),
                        VisitorDetails = visitor.VisitorDetails,
                        NoOfPerson = visitor.NoOfPerson,
                    });
                }
            }

            return lstVisitor;
        }
        public List<VisitorModel> GetVisitorsByFlat()
        {
            ApptSoftEntities db = new ApptSoftEntities();
            List<VisitorModel> lstVisitor = new List<VisitorModel>();
            string flatno = HttpContext.Current.Session["FlatNo"].ToString();
            var visitorData = db.tblVisitors.Where(v => v.FlatNo== flatno)
           .OrderByDescending(v => v.Id).ToList();
            if (visitorData != null)
            {
                foreach (var visitor in visitorData)
                {
                    lstVisitor.Add(new VisitorModel()
                    {
                        Id = visitor.Id,
                        FlatNo = visitor.FlatNo,
                        Name = visitor.Name,
                        Address = visitor.Address,
                        Mobile = visitor.Mobile,
                         Photo = visitor.Photo, // Optional: usually omitted unless converting to base64
                        Type = visitor.Type,
                        Frequency = visitor.Frequency,
                        VisitDate = visitor.VisitDate.ToShortDateString(),
                        VisitTime = visitor.VisitTime.ToString(), // or .ToString(@"hh\:mm")
                        VehicleNo = visitor.VehicleNo,
                        Consent = visitor.Consent,
                        CreateBy = visitor.CreateBy,
                        CreateDate = visitor.CreateDate.ToString(),
                        VisitorDetails = visitor.VisitorDetails,
                        NoOfPerson = visitor.NoOfPerson,
                    });
                }
            }

            return lstVisitor;
        }
    }
}