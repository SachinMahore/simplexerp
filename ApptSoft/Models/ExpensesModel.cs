using ApptSoft.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AppSoft.Models
{
    public class ExpensesModel
    {
        public int Id { get; set; }
        public string FlatNo { get; set; }
        public string Title { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string PaidDate { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public string PaidBy { get; set; }
        public string PaymentMode { get; set; }
        public string TransactionNo { get; set; }
        public string Receipt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public string CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string UpdateDate { get; set; }

        public string SaveExpensesData(HttpPostedFileBase fb, ExpensesModel model)
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

                var ExpensesData = new tblExpens()
                {
                    Id = model.Id,
                    FlatNo = model.FlatNo,
                    Title = model.Title,
                    Month = model.Month,
                    Year = model.Year,
                    Amount = model.Amount,
                    PaidDate = Convert.ToDateTime(DateTime.Now),
                    PaidAmount = model.PaidAmount,
                    PaidBy = model.PaidBy,
                    PaymentMode = model.PaymentMode,
                    TransactionNo = model.TransactionNo,
                    Receipt = sysFileName,
                    CreateBy = model.CreateBy,
                    CreateDate = Convert.ToDateTime(DateTime.Now),
                    UpdateBy = model.UpdateBy,
                    UpdateDate = Convert.ToDateTime(DateTime.Now),
                };
                db.tblExpenses.Add(ExpensesData);
                db.SaveChanges();
            }
            else
            {
                var ExpensesData = db.tblExpenses.Where(p => p.Id == model.Id).FirstOrDefault();
                if (ExpensesData != null)
                {
                    ExpensesData.Id = model.Id;
                    ExpensesData.FlatNo = model.FlatNo;
                    ExpensesData.Title = model.Title;
                    ExpensesData.Month = model.Month;
                    ExpensesData.Year = model.Year;
                    ExpensesData.Amount = model.Amount;
                    ExpensesData.PaidDate = Convert.ToDateTime(DateTime.Now);
                    ExpensesData.PaidAmount = model.PaidAmount;
                    ExpensesData.PaidBy = model.PaidBy;
                    ExpensesData.PaymentMode = model.PaymentMode;
                    ExpensesData.TransactionNo = model.TransactionNo;
                    ExpensesData.Receipt = sysFileName;
                    ExpensesData.CreateBy = model.CreateBy;
                    ExpensesData.CreateDate = Convert.ToDateTime(DateTime.Now);
                    ExpensesData.UpdateBy = model.UpdateBy;
                    ExpensesData.UpdateDate = Convert.ToDateTime(DateTime.Now);
                };
                db.SaveChanges();
                msg = "UpdateSuccessfully";

            }
            return msg;
        }

        //ListCode
        public List<ExpensesModel> GetExpensesList()
        {
            ApptSoftEntities db = new ApptSoftEntities();
            List<ExpensesModel> lstExpens = new List<ExpensesModel>();
            var ExpensesData = db.tblExpenses.ToList();
            if (ExpensesData != null)
            {
                foreach (var Expens in ExpensesData)
                {
                    lstExpens.Add(new ExpensesModel()
                    {
                        Id = Expens.Id,
                        FlatNo = Expens.FlatNo,
                        Title = Expens.Title,
                        Month = Expens.Month,
                        Year = Expens.Year,
                        Amount = Expens.Amount,
                        PaidDate = DateTime.Now.ToString("dd/MM/yyyy"),
                        PaidAmount = Expens.PaidAmount,
                        PaidBy = Expens.PaidBy,
                        PaymentMode = Expens.PaymentMode,
                        TransactionNo = Expens.TransactionNo,
                        Receipt = Expens.Receipt,
                        CreateBy = Expens.CreateBy,
                        CreateDate = Expens.CreateDate.ToString(),
                        UpdateBy = Expens.UpdateBy,
                        UpdateDate = Expens.UpdateDate.ToString(),

                    });
                }
            }
            return lstExpens;
        }

        //DeleteCode
        public string DeleteExpenses(int Id)
        {
            string msg = "";
            ApptSoftEntities db = new ApptSoftEntities();
            var ExpensesData = db.tblExpenses.Where(p => p.Id == Id).FirstOrDefault();
            if (ExpensesData != null)
            {
                db.tblExpenses.Remove(ExpensesData);
            };
            db.SaveChanges();
            msg = "Record delete";
            return msg;
        }

        //EditCode

        public ExpensesModel EditExpenses(int Id)
        {

            ExpensesModel model = new ExpensesModel();
            ApptSoftEntities db = new ApptSoftEntities();
            var ExpensesData = db.tblExpenses.Where(p => p.Id == Id).FirstOrDefault();
            if (ExpensesData != null)
            {
                model.Id = Id;
                model.FlatNo = ExpensesData.FlatNo;
                model.Title = ExpensesData.Title;
                model.Month = ExpensesData.Month;
                model.Year = ExpensesData.Year;
                model.Amount = ExpensesData.Amount;
                model.PaidDate = ExpensesData.PaidDate.ToString();
                model.PaidAmount = ExpensesData.PaidAmount;
                model.PaidBy = ExpensesData.PaidBy;
                model.PaymentMode = ExpensesData.PaymentMode;
                model.TransactionNo = ExpensesData.TransactionNo;
                model.Receipt = ExpensesData.Receipt;
                model.CreateBy = ExpensesData.CreateBy;
                model.CreateDate = ExpensesData.CreateDate.ToString();
                model.UpdateBy = ExpensesData.UpdateBy;
                model.UpdateDate = ExpensesData.UpdateDate.ToString();
            };
            return model;
        }


    }
}