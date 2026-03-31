using ApptSoft.Data;
using ApptSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApptSoft.Models
{
    public class IncomeModel
    {

        public int Id { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string IncomeFrom { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string AddedDate { get; set; }
        public string CreateDate { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public string UpdateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }

        public string SaveIncomeData(IncomeModel model)
        {
            string msg = "Data Save Successfully";
            ApptSoftEntities Db = new ApptSoftEntities();
            {
                if (model.Id == 0)
            {

               var incomeData = new tblIncome()
                {

                Month = model.Month,
                Year = model.Year,
                IncomeFrom = model.IncomeFrom,
                Amount = model.Amount,
                AddedDate = Convert.ToDateTime(model.AddedDate),
                CreateBy = Convert.ToInt32(HttpContext.Current.Session["Id"]),
                CreateDate = DateTime.Now,


                };
                  Db.tblIncomes.Add(incomeData);
                  Db.SaveChanges();
           
              return msg;
            }
            else
                 {
                    var incomeData = Db.tblIncomes.Where(p => p.Id == model.Id).FirstOrDefault();
                    if (incomeData != null)
                    {
                         incomeData.Id = model.Id;
                         incomeData.Month = model.Month;
                         incomeData.Year = model.Year;
                         incomeData.IncomeFrom = model.IncomeFrom;
                         incomeData.Amount = model.Amount;
                         incomeData.AddedDate = Convert.ToDateTime(model.AddedDate);

                        incomeData.UpdateBy = Convert.ToInt32(HttpContext.Current.Session["Id"]);
                         incomeData.UpdateDate = DateTime.Now;

                    };
                    Db.SaveChanges();
                    msg= "Updated Successfully";
                    return msg;
                }

            }
            
        }


        public List<IncomeModel> GetIncomeData()
        {
            ApptSoftEntities db = new ApptSoftEntities();
            List<IncomeModel> listob = new List<IncomeModel>();
            var incomeData = db.tblIncomes.ToList();
            if (incomeData != null)
            {
                foreach (var model in incomeData)
                {
                    listob.Add(new IncomeModel()
                    {
                        Id = model.Id,
                        Month = model.Month,
                        Year = model.Year,
                        CreateBy = model.CreateBy,
                        IncomeFrom = model.IncomeFrom,
                        Amount = model.Amount,
                        AddedDate = model.AddedDate.Value.ToShortDateString(),
                        CreateDate = model.CreateDate.ToString(),

                    });
                }
            }
            return listob;
        }
        public List<IncomeModel> GetIncomeSearch(string Year = null, string month = null)
        {
            ApptSoftEntities db = new ApptSoftEntities();
            List<IncomeModel> listob = new List<IncomeModel>();
            var incomeData = db.tblIncomes.ToList().Where(x => x.Year == Year && x.Month == month).ToList(); ;
            if (incomeData != null)
            {
                foreach (var model in incomeData)
                {
                    listob.Add(new IncomeModel()
                    {
                        Id=model.Id,
                        Month = model.Month,
                        Year =model.Year,
                        CreateBy = model.CreateBy,
                        IncomeFrom=model.IncomeFrom,
                        Amount=model.Amount,
                        AddedDate = model.AddedDate.Value.ToShortDateString(),
                        CreateDate = model.CreateDate.ToString(),
                    });
                }
            }
            return listob;
        }

        public string DeleteIncomeData(int Id)
        {
            string msg = "";
            ApptSoftEntities db = new ApptSoftEntities();
            var incomedata = db.tblIncomes.Where(p => p.Id == Id).FirstOrDefault();
            if (incomedata != null)
            {
                db.tblIncomes.Remove(incomedata);
            };
            db.SaveChanges();
            msg = "Record delete";
            
            
            return msg;
        }
        public IncomeModel GetIncomeData(int Id)
        {

            IncomeModel model = new IncomeModel();
            ApptSoftEntities db = new ApptSoftEntities();
            var incomedata = db.tblIncomes.Where(p => p.Id == Id).FirstOrDefault();
            if (incomedata != null)

            {
                model.Id = incomedata.Id;
                model.Month = incomedata.Month;
                model.Year = incomedata.Year;
                model.IncomeFrom = incomedata.IncomeFrom;
                model.Amount = incomedata.Amount;
                model.AddedDate = incomedata.AddedDate.Value.ToShortDateString();
                model.CreateBy = incomedata.CreateBy;
                model.CreateDate = incomedata.CreateDate.ToString();
                model.UpdateBy = incomedata.UpdateBy;
                model.UpdateDate = incomedata.UpdateDate.ToString();

            };

            return model;
        }
        public IncomeModel EditIncomeData(int Id)
        {

            IncomeModel model = new IncomeModel();
            ApptSoftEntities db = new ApptSoftEntities();
            var incomedata = db.tblIncomes.Where(p => p.Id == Id).FirstOrDefault();
            if (incomedata != null)

            {
                model.Id = incomedata.Id;
                model.Month = incomedata.Month;
                model.Year = incomedata.Year;   
                model.IncomeFrom= incomedata.IncomeFrom;
                model.Amount= incomedata.Amount;
                model.AddedDate = incomedata.AddedDate.Value.ToShortDateString();
                model.CreateBy = incomedata.CreateBy;
                model.CreateDate = incomedata.CreateDate.ToString();
                model.UpdateBy = incomedata.UpdateBy;
                model.UpdateDate = incomedata.UpdateDate.ToString();

            };

            return model;
        }

    }
}