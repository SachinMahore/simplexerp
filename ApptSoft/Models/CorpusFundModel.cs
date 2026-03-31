using ApptSoft.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApptSoft.Models
{
    public class CorpusFundModel
    { 
        public int Id { get; set; }
        public string FlatNo { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public string CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string UpdateDate { get; set; }
        public string ResidentName { get; set; }

        public string SaveCorpusFundData(CorpusFundModel model)
        {
            string msg = "Data Save Successfully";
            ApptSoftEntities Db = new ApptSoftEntities();

            if (model.Id == 0)
            {
                var corpusData = new tblCorpusFund()
                {

                    FlatNo = model.FlatNo,
                    Amount=model.Amount,
                    PaidAmount=model.PaidAmount,
                    BalanceAmount=model.BalanceAmount,
                    CreateBy = model.CreateBy,
                    CreateDate = Convert.ToDateTime(DateTime.Now),
                    UpdateBy = model.UpdateBy,
                    UpdateDate = Convert.ToDateTime(DateTime.Now)

                };
                Db.tblCorpusFunds.Add(corpusData);
                Db.SaveChanges();
                return msg;
            }
            else
            {
                var corpusData = Db.tblCorpusFunds.Where(p => p.Id == model.Id).FirstOrDefault();
                if (corpusData != null)
                {
                    corpusData.Id = model.Id;
                    corpusData.FlatNo = model.FlatNo;
                    corpusData.Amount = model.Amount;
                    corpusData.PaidAmount = model.PaidAmount;
                    corpusData.BalanceAmount = model.BalanceAmount;
                    corpusData.CreateBy = model.CreateBy;
                    corpusData.CreateDate = Convert.ToDateTime(DateTime.Now);
                    corpusData.UpdateBy = model.UpdateBy;
                    corpusData.UpdateDate = Convert.ToDateTime(DateTime.Now);

                };
                Db.SaveChanges();
                msg = "Updated Successfully";
            }
            return msg;
        }

        public List<CorpusFundModel> GetcCorpusFundData()
        {
            ApptSoftEntities db = new ApptSoftEntities();
            List<CorpusFundModel> listcorpus = new List<CorpusFundModel>();
            var corpusData = (from corpus in db.tblCorpusFunds
                              join resident in db.tblResidents
                              on corpus.FlatNo equals resident.FlatNo
                              orderby corpus.Id descending
                              select new
                              {
                                  corpus.Id,
                                  corpus.Amount,
                                  corpus.BalanceAmount,
                                  corpus.PaidAmount,
                                  corpus.FlatNo,
                                  resident.FirstName,
                                  corpus.CreateDate
                                  // Add any other fields you need
                              }).ToList();
            if (corpusData != null)
            {
                foreach (var model in corpusData)
                {
                    listcorpus.Add(new CorpusFundModel()
                    {
                        Id = model.Id,
                        FlatNo = model.FlatNo,
                        Amount= model.Amount,
                        PaidAmount = model.PaidAmount,
                        BalanceAmount = model.BalanceAmount,
                        CreateDate = model.CreateDate.Value.ToString("dd/MM/yyyy"),
                        ResidentName = model.FirstName,
                    });
                }
            }
            return listcorpus;

        }

        public string DeleteCorpusFundData(int Id)
        {
            string msg = "";
            ApptSoftEntities db = new ApptSoftEntities();
            var corpusData = db.tblCorpusFunds.Where(p => p.Id == Id).FirstOrDefault();
            if (corpusData != null)
            {
                db.tblCorpusFunds.Remove(corpusData);
            };
            db.SaveChanges();
            msg = "Record delete";
            return msg;
        }

        public CorpusFundModel EditCorpusFundData(int Id)
        {

            CorpusFundModel model = new CorpusFundModel();
            ApptSoftEntities db = new ApptSoftEntities();
            var corpusData = db.tblCorpusFunds.Where(p => p.Id == Id).FirstOrDefault();
            if (corpusData != null)

            {
                model.Id = corpusData.Id;
                model.FlatNo = corpusData.FlatNo;
                model.Amount= corpusData.Amount;
                model.PaidAmount= corpusData.PaidAmount;
                model.BalanceAmount= corpusData.BalanceAmount;
                model.CreateBy = corpusData.CreateBy;
                model.CreateDate = corpusData.CreateDate.ToString();
                model.UpdateBy = corpusData.UpdateBy;
                model.UpdateDate = corpusData.UpdateDate.ToString();
            };

            return model;
        }





    }
}