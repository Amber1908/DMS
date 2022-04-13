﻿using System;
using System.Collections.Generic;
using System.Linq;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility.Interface;
using X1APServer.Service.Model;

namespace X1APServer.Service.Utils
{
    public static class DBUtils
    {
        public static List<CervixTable> GetCervixTablesByDate(IX1UnitOfWork _uow,DateTime? startTime, DateTime? endTime)
        {
            List<CervixTable> cers = new List<CervixTable>();
            try
            {
                //1.將AnswerMain表單中 QuestionID是29的撈出來(收件日期)
                List<Repository.X1_Report_Answer_Detail> Q29s = _uow.Get<IX1_ReportAnswerDRepository>().GetAll().Where(x=>x.QuestionID==29).ToList();
                //2.將AnswerMain表單中 value的值轉換成西元日期 再轉成string
                List<Repository.X1_Report_Answer_Detail> NewQ29s = new List<Repository.X1_Report_Answer_Detail>();
                foreach (var Q29 in Q29s)
                {
                    DateTime NewQ29 = Convert.ToDateTime(Q29.Value).AddYears(1911);
                    //3.篩選出在startTime跟endTime之間的
                    if (startTime>= NewQ29&&endTime<= NewQ29)
                    {
                        NewQ29s.Add(Q29);
                    }
                }
                
                //4.封裝成 CervixTable
            }
            catch (Exception)
            {

                throw;
            }
            return cers;
        }
        public static List<CervixTable> GetCervixTable(IX1UnitOfWork _uow)
        {
            List<CervixTable> cers = new List<CervixTable>();

            try
            {   
                int Fid = _uow.Get<IX1_ReportMRepository>().Get(x => x.FuncCode.Contains("cervix") && x.IsPublish).ID;
                List<Repository.X1_Report_Answer_Main> Xams = _uow.Get<IX1_ReportAnswerMRepository>().GetAll().Where(x => x.ReportID == Fid).ToList();
                List<Repository.X1_PatientInfo> CA = _uow.Get<IX1_PatientInfoRepository>().GetAll().ToList();
                List<Repository.X1_Report_Question> RQ = _uow.Get<IX1_ReportQuestionRepository>().GetAll().ToList();
                List<Repository.X1_Report_Answer_Detail> RD = _uow.Get<IX1_ReportAnswerDRepository>().GetAll().ToList();
               
                foreach (var Xam in Xams)
                {
                    Repository.X1_PatientInfo CAs = CA.FirstOrDefault(x => x.ID == Xam.PID);

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
                    var RQs2 = RQ.FirstOrDefault(x => x.ReportID == Fid && x.QuestionNo == "Vix-30");
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
