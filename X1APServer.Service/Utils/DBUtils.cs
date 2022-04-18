using Connection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility.Interface;
using X1APServer.Service.Model;
using X1APServer.Service.Service;

namespace X1APServer.Service.Utils
{
    public static class DBUtils
    {

        //public static List<CervixTable> GetCervixTable(IX1UnitOfWork _uow)
        //{
        //    List<CervixTable> cers = new List<CervixTable>();

        //    try
        //    {
        //        int Fid = _uow.Get<IX1_ReportMRepository>().Get(x => x.FuncCode.Contains("cervix") && x.IsPublish).ID;
        //        List<Repository.X1_Report_Answer_Main> Xams = _uow.Get<IX1_ReportAnswerMRepository>().GetAll().Where(x => x.ReportID == Fid).ToList();
        //        List<Repository.X1_PatientInfo> CA = _uow.Get<IX1_PatientInfoRepository>().GetAll().ToList();
        //        List<Repository.X1_Report_Question> RQ = _uow.Get<IX1_ReportQuestionRepository>().GetAll().ToList();
        //        List<Repository.X1_Report_Answer_Detail> RD = _uow.Get<IX1_ReportAnswerDRepository>().GetAll().ToList();

        //        foreach (var Xam in Xams)
        //        {
        //            Repository.X1_PatientInfo CAs = CA.FirstOrDefault(x => x.ID == Xam.PID);

        //            CervixTable cer = new CervixTable()
        //            {
        //                ID = Xam.ID,
        //                ReportID = Xam.ReportID,
        //                FillingDate = Xam.FillingDate,
        //                CreateDate = Xam.CreateDate,
        //                ModifyDate = Xam.ModifyDate,
        //                Status = Xam.Status,
        //                cervixCase = new CervixCase()
        //                {
        //                    ID = CAs.ID,
        //                    PUCountry = CAs.PUCountry,
        //                    PUName = CAs.PUName,
        //                    PUDOB = CAs.PUDOB,
        //                    IDNo = CAs.IDNo,
        //                    Cellphone = CAs.Phone,
        //                    Education = CAs.Education,
        //                    AddrCode = CAs.AddrCode,
        //                    HCCode = CAs.HCCode,
        //                    Addr = CAs.Addr,
        //                    Domicile = CAs.Domicile
        //                },
        //                cervixQuestions = new List<CervixQuestion>()

        //            };
        //            //用 sql 組CervixQuestion
        //            //CervixQuestion cerQ = GetCervixQuestion(Xam.ID);
        //            //cer 加組好的CervixQuestion
        //            //cer.cervixQuestions.Add(cerQ);
        //            cers.Add(cer);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string err = ex.Message;
        //        cers = null;
        //    }

        //    return cers;
        //}
        //    TingYu
        public static CervixQuestion GetCervixQuestion(int AIMID, ExportCervixDataM.Request request)
        {
            CervixQuestion cervixQuestion = new CervixQuestion();
            string ConnString = WebConfigurationManager.ConnectionStrings["ConnectionStringTemplateStandard"].ToString();
            //找出站台 改連線字串的catalog

            //string HealthWeb = GetHealthWeb(request.UserSecurityInfo);
            string ConnectionString = string.Format(ConnString, "GetHealthWeb");
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand sqlcomm = new SqlCommand("", conn);
                    SqlDataReader sqlDataReader;
                    conn.Open();
                    sqlcomm.CommandText = @"select X1_Report_Answer_Detail.ID As AID,X1_Report_Answer_Detail.[Value],X1_Report_Question.*
                                                from X1_Report_Answer_Detail
                                                left join X1_Report_Question on X1_Report_Question.ID =X1_Report_Answer_Detail.QuestionID
                                                where X1_Report_Answer_Detail.AnswerMID=@AMID";
                    sqlcomm.Parameters.AddWithValue("@AMID", AIMID);
                    sqlDataReader = sqlcomm.ExecuteReader();
                    if (sqlDataReader.Read())
                    {
                        cervixQuestion = new CervixQuestion()
                        {
                            ID = Convert.ToInt32(sqlDataReader["ID"]),
                            QuestionNo = sqlDataReader["QuestionNo"].ToString(),
                            QuestionType = Convert.ToInt32(sqlDataReader["QuestionType"]),
                            QuestionText = sqlDataReader["QuestionText"].ToString(),
                            Description = sqlDataReader["Description"].ToString(),
                            AnswerOption = sqlDataReader["AnswerOption"].ToString(),// ( Json )
                            AID = Convert.ToInt32(sqlDataReader["AID"]),
                            Value = sqlDataReader["Value"].ToString()
                        };
                    }
                    sqlDataReader.Close();
                }
                catch (Exception ex)
                {

                    string err = ex.Message;
                    cervixQuestion = null;
                }
            }

            return cervixQuestion;
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

                    foreach (var RQss in RQs)
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

                    //補上 Vix-30 複選題
                    var RQs2 = RQ.FirstOrDefault(x => x.ReportID == Fid && x.QuestionNo == "Vix-30");
                    var RQs3 = RQ.Where(x => x.ReportID == Fid && x.ParentQuestID == RQs2.ID).ToList();
                    List<int> al = new List<int>();
                    foreach (var RQTemp in RQs3)
                    {
                        al.Add(RQTemp.ID);
                    }
                    var RDs2 = RD.Where(x => x.AnswerMID == Xam.ID && al.Contains(x.QuestionID)).ToList();
                    foreach (var RDs21 in RDs2)
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
            catch (Exception ex)
            {
                string err = ex.Message;
                cers = null;
            }

            return cers;
        }

    }
}


