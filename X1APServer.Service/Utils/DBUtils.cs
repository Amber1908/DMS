using System;
using System.Collections.Generic;
using System.Linq;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility.Interface;
using X1APServer.Service.Model;

namespace X1APServer.Service.Utils
{
    public static class DBUtils
    {
        public static List<CervixTable> GetCervixTable(IX1UnitOfWork _uow)
        {
            List<CervixTable> cers = new List<CervixTable>();

            try
            {
                int Fid = _uow.Get<IX1_ReportMRepository>().Get(x => x.FuncCode.Contains("cervix") && x.IsPublish).ID;
                var Xams = _uow.Get<IX1_ReportAnswerMRepository>().GetAll().Where(x => x.ReportID == Fid).ToList();
                var CA = _uow.Get<IX1_PatientInfoRepository>().GetAll().ToList();
                var RQ = _uow.Get<IX1_ReportQuestionRepository>().GetAll().ToList();
                var RD = _uow.Get<IX1_ReportAnswerDRepository>().GetAll().ToList();

                foreach (var Xam in Xams)
                {
                    var CAs = CA.Where(x => x.ID == Xam.PID).FirstOrDefault();

                    CervixTable cer = new CervixTable()
                    {
                        ID = Xam.ID,
                        ReportID = Xam.ReportID,
                        FillingDate = Xam.FillingDate,
                        CreateDate = Xam.CreateDate,
                        ModifyDate = Xam.ModifyDate,
                        Status = Xam.Status,
                        cervixCase = new CervixCase()
                        {
                            ID = CAs.ID,
                            PUCountry = CAs.PUCountry,
                            PUName = CAs.PUName,
                            PUDOB = CAs.PUDOB,
                            IDNo = CAs.IDNo,
                            Cellphone = CAs.Phone,
                            Education = CAs.Education,
                            AddrCode = CAs.AddrCode,
                            HCCode = CAs.HCCode,
                            Addr = CAs.Addr,
                            Domicile = CAs.Domicile
                        },
                        cervixQuestions = new List<CervixQuestion>()
                    };

                    var RQs = RQ.Where(x => x.ReportID == Fid && x.QuestionType != 9).ToList();

                    foreach(var RQss in RQs)
                    {
                        var RDs = RD.Where(x => x.AnswerMID == Xam.ID && x.QuestionID == RQss.ID).FirstOrDefault();
                        CervixQuestion cq = new CervixQuestion()
                        {
                            ID = RQss.ID,
                            QuestionNo = RQss.QuestionNo,
                            QuestionType = RQss.QuestionType,
                            QuestionText = RQss.QuestionText,
                            Description = RQss.Description,
                            AnswerOption = RQss.AnswerOption,
                            AID = RDs == null ? 0 : RDs.ID,
                            Value = RDs == null ? "" : RDs.Value
                        };
                        cer.cervixQuestions.Add(cq);
                    }

                    // 補上 Vix-30 複選題
                    var RQs2 = RQ.Where(x => x.ReportID == Fid && x.QuestionNo == "Vix-30").FirstOrDefault();
                    var RQs3 = RQ.Where(x => x.ReportID == Fid && x.ParentQuestID == RQs2.ID).ToList();
                    List<int> al = new List<int>();
                    foreach(var RQTemp in RQs3)
                    {
                        al.Add(RQTemp.ID);
                    }
                    var RDs2 = RD.Where(x => x.AnswerMID == Xam.ID && al.Contains(x.QuestionID)).ToList();
                    foreach(var RDs21 in RDs2)
                    {
                        CervixQuestion cq = new CervixQuestion()
                        {
                            ID = RQs2.ID,
                            QuestionNo = RQs2.QuestionNo,
                            QuestionType = RQs2.QuestionType,
                            QuestionText = RQs2.QuestionText,
                            Description = RQs2.Description,
                            AnswerOption = RQs2.AnswerOption,
                            AID = RDs21.ID,
                            Value = RDs21.Value
                        };
                        cer.cervixQuestions.Add(cq);
                    }

                    cers.Add(cer);
                }
            }
            catch(Exception ex)
            {
                string err = ex.Message;
                cers = null;
            }

            return cers;
        }
    }
}
