using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using X1APServer.Repository;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility.Interface;
using X1APServer.Service.Interface;
using X1APServer.Service.Misc;
using X1APServer.Service.Model;
using X1APServer.Service.Service.Interface;
using X1APServer.Service.Utils;
using static X1APServer.Service.Model.AddPatientM;


namespace X1APServer.Service.Service
{
    public class X1APService : IX1APService
    {
        private readonly IX1UnitOfWork _uow;
        private readonly IDMSShareUnitOfWork _suow;
        private readonly IIDoctorService _idoctorSvc;
        public X1APService(IX1UnitOfWork uow, IDMSShareUnitOfWork suow, IIDoctorService idoctorSvc)
        {
            _uow = uow;
            _suow = suow;
            _idoctorSvc = idoctorSvc;
        }

        public X1UserDataGetM.X1UserDataGetRsp GetUserList(X1UserDataGetM.X1UserDataGetReq request)
        {
            X1UserDataGetM.X1UserDataGetRsp ret = new X1UserDataGetM.X1UserDataGetRsp();
            ret.ReturnCode = ErrorCode.OK;
            ret.ReturnMsg = "OK";
           
            ret.UserList = new List<X1UserDataGetM.X1User>();

            // 準備 回傳 資料
            var Uss = _uow.Get<IUsersRepository>().GetAll().ToList();
            var Ur = _uow.Get<IUserRoleMapRepository>();

            foreach (var user in Uss)
            {
                X1UserDataGetM.X1User ux = new X1UserDataGetM.X1User();

                ux.AccID = user.AccID;
                ux.AccName = user.AccName;
                ux.DoctorNo = user.DoctorNo;
                ux.UserID = user.ID;
                ux.Role = "";

                var urs = Ur.GetAll().Where(x => x.UserID == user.ID).ToList();

                foreach(var ur in urs)
                {
                    ux.Role += ur.RoleCode + ";";
                }

                if (request.AccountID == null || request.AccountID.Length == 0)
                {
                    ret.UserList.Add(ux);
                }
                else if (request.AccountID == user.AccID)
                {
                    ret.UserList.Add(ux);
                }
            }

            return ret;
        }

        public GetCervixFormM.GetCervixFormRsp GetCervixForm(GetCervixFormM.GetCervixFormReq request)
        {
            GetCervixFormM.GetCervixFormRsp ret = new GetCervixFormM.GetCervixFormRsp();
            ret.ReturnCode = ErrorCode.OK;
            ret.ReturnMsg = "OK";

            ret.CervixFormList = new List<GetCervixFormM.CervixForm>();
            // 準備 回傳 資料
            var sessionkey = HttpContext.Current.Request.Headers["SessionKey"];
            var DMSSharesetting = new DMSShareService(_suow, _idoctorSvc);
            var WebDB = DMSSharesetting.GetDMSSetting(sessionkey).Web_db;
            List<CervixTable> cts = DBUtils.GetCervixTable(_uow, WebDB);

            foreach (var ct in cts)
            {
                GetCervixFormM.CervixForm gc = new GetCervixFormM.CervixForm();
                gc.ID = ct.ID;
                gc.Status = ct.Status;

                foreach (CervixQuestion cq in ct.cervixQuestions)
                {
                    if (cq.QuestionNo.Equals("醫檢師姓名"))
                        gc.DoctorName1 = cq.Value;
                    if (cq.QuestionNo.Equals("醫師姓名"))
                        gc.DoctorName2 = cq.Value;
                    if (cq.QuestionNo.Equals("醫檢師代碼"))
                        gc.DoctorNo1 = cq.Value;
                    if (cq.QuestionNo.Equals("醫師代碼"))
                        gc.DoctorNo2 = cq.Value;
                    if (cq.QuestionNo.Equals("Vix-23"))
                        gc.Vix_23 = cq.Value;
                    if (cq.QuestionNo.Equals("Vix-23-1"))
                        gc.Vix_23_1 = cq.Value;
                    if (cq.QuestionNo.Equals("Vix-23-2"))
                        gc.Vix_23_2 = cq.Value;
                }

                if (request != null)
                {
                    if (request.Status != 0 && request.Status != gc.Status)
                        continue;
                    if (request.DoctorNo != null && request.DoctorNo.Length > 0 && (request.DoctorNo != gc.DoctorNo1 && request.DoctorNo != gc.DoctorNo2))
                        continue;
                }

                ret.CervixFormList.Add(gc);
            }

            return ret;
        }

        public GetCervixTableM.GetCervixTableRsp GetCervixTable(GetCervixTableM.GetCervixTableReq request)
        {
            GetCervixTableM.GetCervixTableRsp ret = new GetCervixTableM.GetCervixTableRsp();
            ret.ReturnCode = ErrorCode.OK;
            ret.ReturnMsg = "OK";

            ret.cervixTables = new List<CervixTable>();
            // 準備 回傳 資料 
            var sessionkey = HttpContext.Current.Request.Headers["SessionKey"];
            var DMSSharesetting = new DMSShareService(_suow, _idoctorSvc);
            var WebDB = DMSSharesetting.GetDMSSetting(sessionkey).Web_db;


            List<CervixTable> cts = DBUtils.GetCervixTable(_uow, WebDB);
            foreach (var ct in cts)
            {
                if (request == null)
                {
                    ret.cervixTables.Add(ct);
                }
                else if (request.ID == 0 && request.CaseID == 0)
                {
                    ret.cervixTables.Add(ct);
                }
                else if ((request.ID == 0 || request.ID == ct.ID) && (request.CaseID == 0 || request.CaseID == ct.cervixCase.ID))
                {
                    ret.cervixTables.Add(ct);
                }
            }

            return ret;
        }

        public UpdateCervixStatusM.UpdateCervixStatusRsp UpdateCervixStatus(UpdateCervixStatusM.UpdateCervixStatusReq request)
        {
            UpdateCervixStatusM.UpdateCervixStatusRsp ret = new UpdateCervixStatusM.UpdateCervixStatusRsp();
            ret.ReturnCode = ErrorCode.OK;
            ret.ReturnMsg = "OK";

            try
            {
                // 輸入資料驗證
                var reportAnsM = _uow.Get<IX1_ReportAnswerMRepository>().Get(x => !x.IsDelete && x.ID == request.ID);
                if (reportAnsM == null)
                {
                    ret.ReturnCode = ErrorCode.NotFound;
                    ret.ReturnMsg = "無此Report ID";
                    return ret;
                }

                if (reportAnsM.Lock)
                {
                    ret.ReturnCode = ErrorCode.OperateError;
                    ret.ReturnMsg = "此 Report 已鎖定";
                    return ret;
                }

                if (reportAnsM.Status > 5)
                {
                    ret.ReturnCode = ErrorCode.StatusError;
                    ret.ReturnMsg = "此 Report 已結案";
                    return ret;
                }

                reportAnsM.Status = request.Status;
                reportAnsM.ModifyDate = DateTime.Now;
                reportAnsM.ModifyMan = "X1";
                _uow.Get<IX1_ReportAnswerMRepository>().Update(reportAnsM);
                _uow.Commit();

                // 更新答案
                var repoA = _uow.Get<IX1_ReportQuestionRepository>();
                var repoB = _uow.Get<IX1_ReportAnswerDRepository>();

                var repoA1 = repoA.Get(x => x.ReportID == reportAnsM.ReportID && x.QuestionNo.Equals("醫檢師代碼"));
                var repoB1 = repoB.Get(x => x.AnswerMID == reportAnsM.ID && x.QuestionID == repoA1.ID);

                if (repoB1 == null)
                {
                    repoB1 = new X1_Report_Answer_Detail()
                    {
                        AnswerMID = reportAnsM.ID,
                        QuestionID = repoA1.ID,
                        Value = request.DoctorNo1
                    };
                    repoB.Create(repoB1);
                }
                else
                {
                    repoB1.AnswerMID = reportAnsM.ID;
                    repoB1.QuestionID = repoA1.ID;
                    repoB1.Value = request.DoctorNo1;
                    repoB.Update(repoB1);
                }
                _uow.Commit();
                repoA1 = repoA.Get(x => x.ReportID == reportAnsM.ReportID && x.QuestionNo.Equals("醫檢師姓名"));
                repoB1 = repoB.Get(x => x.AnswerMID == reportAnsM.ID && x.QuestionID == repoA1.ID);

                if (repoB1 == null)
                {
                    repoB1 = new X1_Report_Answer_Detail()
                    {
                        AnswerMID = reportAnsM.ID,
                        QuestionID = repoA1.ID,
                        Value = request.DoctorName1
                    };
                    repoB.Create(repoB1);
                }
                else
                {
                    repoB1.AnswerMID = reportAnsM.ID;
                    repoB1.QuestionID = repoA1.ID;
                    repoB1.Value = request.DoctorName1;
                    repoB.Update(repoB1);
                }
                _uow.Commit();
                repoA1 = repoA.Get(x => x.ReportID == reportAnsM.ReportID && x.QuestionNo.Equals("醫師代碼"));
                repoB1 = repoB.Get(x => x.AnswerMID == reportAnsM.ID && x.QuestionID == repoA1.ID);

                if (repoB1 == null)
                {
                    repoB1 = new X1_Report_Answer_Detail()
                    {
                        AnswerMID = reportAnsM.ID,
                        QuestionID = repoA1.ID,
                        Value = request.DoctorNo2
                    };
                    repoB.Create(repoB1);
                }
                else
                {
                    repoB1.AnswerMID = reportAnsM.ID;
                    repoB1.QuestionID = repoA1.ID;
                    repoB1.Value = request.DoctorNo2;
                    repoB.Update(repoB1);
                }
                _uow.Commit();
                repoA1 = repoA.Get(x => x.ReportID == reportAnsM.ReportID && x.QuestionNo.Equals("醫師姓名"));
                repoB1 = repoB.Get(x => x.AnswerMID == reportAnsM.ID && x.QuestionID == repoA1.ID);

                if (repoB1 == null)
                {
                    repoB1 = new X1_Report_Answer_Detail()
                    {
                        AnswerMID = reportAnsM.ID,
                        QuestionID = repoA1.ID,
                        Value = request.DoctorName2
                    };
                    repoB.Create(repoB1);
                }
                else
                {
                    repoB1.AnswerMID = reportAnsM.ID;
                    repoB1.QuestionID = repoA1.ID;
                    repoB1.Value = request.DoctorName2;
                    repoB.Update(repoB1);
                }
                _uow.Commit();
            }
            catch (Exception ex)
            {
                ret.ReturnCode = ErrorCode.Exception;
                ret.ReturnMsg = ex.Message;
            }
            return ret;
        }

        public UpdateCervixTableM.UpdateCervixTableRsp UpdateCervixTable(UpdateCervixTableM.UpdateCervixTableReq request)
        {
            UpdateCervixTableM.UpdateCervixTableRsp ret = new UpdateCervixTableM.UpdateCervixTableRsp();
            ret.ReturnCode = ErrorCode.OK;
            ret.ReturnMsg = "OK";

            try
            {
                // 輸入資料驗證
                var reportAnsM = _uow.Get<IX1_ReportAnswerMRepository>().Get(x => !x.IsDelete && x.ID == request.cervixTable.ID);
                if (reportAnsM == null)
                {
                    ret.ReturnCode = ErrorCode.NotFound;
                    ret.ReturnMsg = "無此Report ID";
                    return ret;
                }

                if (reportAnsM.Lock)
                {
                    ret.ReturnCode = ErrorCode.OperateError;
                    ret.ReturnMsg = "此 Report 已鎖定";
                    return ret;
                }

                if (reportAnsM.Status > 5)
                {
                    ret.ReturnCode = ErrorCode.StatusError;
                    ret.ReturnMsg = "此 Report 已結案";
                    return ret;
                }

                reportAnsM.Status = request.cervixTable.Status;
                reportAnsM.ModifyDate = DateTime.Now;
                reportAnsM.ModifyMan = "X1";

                _uow.Get<IX1_ReportAnswerMRepository>().Update(reportAnsM);
                _uow.Commit();

                // 更新答案
                var repoA = _uow.Get<IX1_ReportQuestionRepository>();
                var repoB = _uow.Get<IX1_ReportAnswerDRepository>();
                X1_Report_Answer_Detail xra;
                // 複選題清理
                var repoA1 = repoA.Get(x => x.ReportID == request.cervixTable.ReportID && x.QuestionNo.Equals("Vix-30"));
                List<AddReportMainM.Answeroption> options = JsonConvert.DeserializeObject<List<AddReportMainM.Answeroption>>(repoA1.AnswerOption);
                foreach (var op in options)
                {
                    repoB.Delete(x => x.AnswerMID == request.cervixTable.ID && x.QuestionID == op.ID);
                    _uow.Commit();
                }

                // 加入或更新答案
                foreach (var qu in request.cervixTable.cervixQuestions)
                {
                    switch (qu.QuestionNo)
                    {
                        case "Vix-27":
                        case "Vix-27-1":
                        case "Vix-28":
                        case "Vix-29":
                        case "Vix-29-1":
                        case "Vix-29-2":
                        case "Vix-30-1":
                        case "Vix-31":
                        case "Vix-31-1":
                        case "Vix-31-2":
                        case "Vix-31-3":
                        case "Vix-32":
                        case "Vix-33":
                        case "Vix-33-1":
                        case "醫檢師代碼":
                        case "醫檢師姓名":
                        case "醫師代碼":
                        case "醫師姓名":
                        case "確診日期":
                            xra = new X1_Report_Answer_Detail()
                            {
                                ID = qu.AID,
                                AnswerMID = request.cervixTable.ID,
                                QuestionID = repoA.Get(x => x.ReportID == request.cervixTable.ReportID && x.QuestionNo.Equals(qu.QuestionNo)).ID,
                                Value = qu.Value
                            };

                            if (qu.AID == 0)
                            {
                                repoB.Create(xra);
                            }
                            else
                            {
                                repoB.Update(xra);
                            }
                            _uow.Commit();
                            break;
                        case "Vix-30":
                            foreach (var op in options)
                            {
                                if (op.Value == qu.Value)
                                {
                                    xra = new X1_Report_Answer_Detail()
                                    {
                                        ID = qu.AID,
                                        AnswerMID = request.cervixTable.ID,
                                        QuestionID = op.ID,
                                        Value = qu.Value
                                    };
                                    repoB.Create(xra);
                                    break;
                                }
                            }
                            _uow.Commit();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ret.ReturnCode = ErrorCode.Exception;
                ret.ReturnMsg = ex.Message;
            }

            return ret;
        }

        public GetX1DataM.Response GetX1Data(GetX1DataM.Request request)
        {
            GetX1DataM.Response ret = new GetX1DataM.Response();
            ret.ReturnCode = ErrorCode.OK;
            ret.ReturnMsg = "OK";

            // 準備 回傳 資料
            var X1DataString = _uow.Get<IX1_ReportAnswerExtraRepository>().Get(x => x.AnswerMID == request.ID);

            if (X1DataString != null)
            {
                ret.X1Data = X1DataString.X1Data;
            }
            else
            {
                ret.X1Data = "";
            }

            return ret;
        }

        public UpdateX1DataM.UpdateX1DataRsp UpdateX1Data(UpdateX1DataM.UpdateX1DataReq request)
        {
            UpdateX1DataM.UpdateX1DataRsp ret = new UpdateX1DataM.UpdateX1DataRsp();
            ret.ReturnCode = ErrorCode.OK;
            ret.ReturnMsg = "OK";
            var X1DataTable = _uow.Get<IX1_ReportAnswerExtraRepository>();

            try
            {
                X1_Report_Answer_Extra xe = new X1_Report_Answer_Extra()
                {
                    AnswerMID = request.ID,
                    X1Data = request.X1Data
                };

                var xeold = X1DataTable.Get(x => x.AnswerMID == xe.AnswerMID);

                if (xeold != null)
                {
                    xeold.X1Data = xe.X1Data;
                    X1DataTable.Update(xeold);
                }
                else
                {
                    X1DataTable.Create(xe);
                }
                _uow.Commit();
            }
            catch (Exception ex)
            {
                ret.ReturnCode = ErrorCode.Exception;
                ret.ReturnMsg = ex.Message;
            }

            return ret;
        }
    }
}
