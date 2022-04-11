using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility.Interface;
using X1APServer.Service.Interface;
using X1APServer.Service.Misc;
using X1APServer.Service.Model;
using X1APServer.Service.Utils;
using static X1APServer.Service.Model.AddPatientM;

namespace X1APServer.Service
{
    public class PatientService : IPatientService
    {
        private readonly IX1UnitOfWork _uow;

        public PatientService(IX1UnitOfWork uow)
        {
            _uow = uow;
        }

        public RSPBase AddPatient(AddPatientM.AddPatientReq ReqData, ref AddPatientM.AddPatientRsp addPatientRsp)
        {
            RSPBase rSPBase = new RSPBase();

            //var patientFlag = _uow.Get<IX1_PatientInfoRepository>().Any(x => x.PUID.Equals(ReqData.PUID));
            //if (patientFlag)
            //{
            //    rSPBase.ReturnCode = ErrorCode.Exist;
            //    rSPBase.ReturnMsg = string.Format("{0} 病患已存在", ReqData.PUID);
            //    return rSPBase;
            //}

            var reqSpecimenID = ReqData.DiagnosisRecord.OrderList.Select(x => x.CCSpecimenID);
            //var specimenFlag = _uow.Get<IX1_SpecimenRepository>().Any(x => reqSpecimenID.Contains(x.CCSpecimenID));
            //if (specimenFlag)
            //{
            //    rSPBase.ReturnCode = ErrorCode.OperateError;
            //    rSPBase.ReturnMsg = "存在重複的檢體編號";
            //    return rSPBase;
            //}

            DateTime now = DateTime.Now;
            List<int> DIDList = new List<int>();
            List<int> OIDList = new List<int>();
            List<int> SIDList = new List<int>();

            try
            {
                _uow.BeginRootTransaction();
                // insert patient
                var patientInfo = _uow.Get<IX1_PatientInfoRepository>().Get(x => x.IDNo == ReqData.IDNo);

                if (patientInfo == null)
                {
                    var patientConfig = new MapperConfiguration(cfg => cfg.CreateMap<AddPatientM.AddPatientReq, X1_PatientInfo>());
                    var patientMapper = patientConfig.CreateMapper();
                    patientInfo = patientMapper.Map<X1_PatientInfo>(ReqData);
                    patientInfo.CreateMan = ReqData.AccID;
                    patientInfo.CreateDate = now;
                    patientInfo.ModifyMan = ReqData.AccID;
                    patientInfo.ModifyDate = now;
                    patientInfo = _uow.Get<IX1_PatientInfoRepository>().Create(patientInfo);
                    _uow.Commit();
                }

                if (patientInfo.PUDOB == null)
                {
                    rSPBase.ReturnCode = ErrorCode.ArgInvalid;
                    rSPBase.ReturnMsg = "PUDOB 為必要項";
                    return rSPBase;
                }

                if (string.IsNullOrEmpty(patientInfo.Gender))
                {
                    rSPBase.ReturnCode = ErrorCode.ArgInvalid;
                    rSPBase.ReturnMsg = "Gender 為必要項";
                    return rSPBase;
                }

                addPatientRsp.PID = patientInfo.ID;

                // insert diagnosis report
                //var diagnosisConfig = new MapperConfiguration(cfg => cfg.CreateMap<AddPatientM.DiagnosisRecord, X1_DiagnosisRecord>());
                //var diagnosisMapper = diagnosisConfig.CreateMapper();
                //var specimenConfig = new MapperConfiguration(cfg => cfg.CreateMap<AddPatientM.Specimen, X1_Specimen>());
                //var specimenMapper = specimenConfig.CreateMapper();
                //var orderConfig = new MapperConfiguration(cfg => cfg.CreateMap<AddPatientM.Order, X1_Order>());
                //var orderMapper = orderConfig.CreateMapper();

                //var diagnosis = ReqData.DiagnosisRecord;
                //var insertDiagnosis = diagnosisMapper.Map<X1_DiagnosisRecord>(diagnosis);
                //insertDiagnosis.PID = patientInfo.ID;
                //insertDiagnosis.CreateMan = ReqData.AccID;
                //insertDiagnosis.CreateDate = now;
                //insertDiagnosis = _uow.Get<IX1_DiagnosisRecordRepository>().Create(insertDiagnosis);
                //_uow.Commit();
                //DIDList.Add(insertDiagnosis.ID);

                //foreach (var order in diagnosis.OrderList)
                //{
                //    // 若有對應的癌醫檢體編號，則設定醫令對應的檢體為現存的
                //    var insertSpecimen = _uow.Get<IX1_SpecimenRepository>().Get(x => x.CCSpecimenID.Equals(order.CCSpecimenID));
                //    // insert specimen
                //    if (order.Specimen != null && insertSpecimen == null)
                //    {
                //        insertSpecimen = specimenMapper.Map<X1_Specimen>(order.Specimen);
                //        insertSpecimen.CreateMan = ReqData.AccID;
                //        insertSpecimen.CreateDate = now;
                //        insertSpecimen.ModifyMan = ReqData.AccID;
                //        insertSpecimen.ModifyDate = now;
                //        insertSpecimen.RecvDate = now;
                //        insertSpecimen.CCSpecimenID = order.CCSpecimenID;
                //        insertSpecimen = _uow.Get<IX1_SpecimenRepository>().Create(insertSpecimen);
                //        _uow.Commit();
                //        SIDList.Add(insertSpecimen.ID);
                //    }

                //    // insert order
                //    var insertOrder = orderMapper.Map<X1_Order>(order);
                //    insertOrder.DID = insertDiagnosis.ID;
                //    insertOrder.SID = insertSpecimen.ID;
                //    insertOrder.CreateDate = now;
                //    insertOrder.CreateMan = ReqData.AccID;
                //    _uow.Get<IX1_OrderRepository>().Create(insertOrder);
                //    _uow.Commit();
                //    OIDList.Add(insertOrder.ID);

                //    // 新增至Report
                //    PrepareReport(ReqData, patientInfo.ID, insertOrder.ID, order);
                //}

                addPatientRsp.DIDList = DIDList;
                addPatientRsp.OIDList = OIDList;
                addPatientRsp.SIDList = SIDList;
                _uow.CommitRootTransaction();
            }
            catch (Exception e)
            {
                _uow.RollBackRootTransaction();
                throw e;
            }

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        // 把新增病患的診斷資料新增至Report
        private void PrepareReport(AddPatientM.AddPatientReq patientReq, int patientID, int orderID, Order order)
        {
            //var orderReportMap = _uow.Get<IOrderReportMapRepository>().GetAll()
            //    .Where(x => order.OrderDetail.Contains(x.OrderKeyWord))
            //    .Select(x => new { ReportID = x.ReportID })
            //    .Distinct()
            //    .ToList();

            // 對應不到醫令關鍵字，不新增Report
            //if (orderReportMap.Count == 0)
            //{
            //    return;
            //}

            // 暫存主Report
            var reportM = _uow.Get<IX1_ReportMRepository>().GetAll();

            //foreach(var map in orderReportMap)
            //{
            //    var answers = new List<UpdateReportM.ReportAnswerD>();

            //    if (map.ReportID == 3)
            //    {
            //        var orderPanelMap = _uow.Get<IOrderPanelMapRepository>().Get(x => order.OrderDetail.Contains(x.OrderKeyWord));
                    
            //        if (orderPanelMap != null)
            //        {
            //            var panel = _uow.Get<IPanelRepository>().GetAll().OrderByDescending(x => x.CreateDate).FirstOrDefault(x => x.PanelCategory == orderPanelMap.PanelCategory);
            //            var panelQuestion = _uow.Get<IX1_ReportQuestionRepository>().Get(x => x.QuestionText.Equals("PanelID"));
            //            var panelNameQuestion = _uow.Get<IX1_ReportQuestionRepository>().Get(x => x.QuestionText.Equals("PanelName"));

            //            // 新增 Flow Panel 答案
            //            answers.Add(new UpdateReportM.ReportAnswerD()
            //            {
            //                QuestionID = Constants.QUEST_PREFIX + panelQuestion.ID,
            //                Value = panel.ID.ToString()
            //            });

            //            answers.Add(new UpdateReportM.ReportAnswerD()
            //            {
            //                QuestionID = Constants.QUEST_PREFIX + panelNameQuestion.ID,
            //                Value = panel.PanelName.ToString()
            //            });
            //        }
            //    }

            //    AddGeneralReportM.AddGeneralReportReq request = new AddGeneralReportM.AddGeneralReportReq()
            //    {
            //        AccID = patientReq.AccID,
            //        OID = orderID,
            //        PID = patientID,
            //        ID = reportM.FirstOrDefault(x => x.ID == map.ReportID).ID,
            //        Answers = answers
            //    };

            //    AddGeneralReportM.AddGeneralReportRsp temp = new AddGeneralReportM.AddGeneralReportRsp();

            //    var reportRsp = _uow.Get<IReportService>().AddGeneralReport(request, ref temp);

            //    if (reportRsp.ReturnCode != ErrorCode.OK)
            //    {
            //        throw new Exception(reportRsp.ReturnMsg);
            //    }
            //}
        }

        public RSPBase AddPatientInfo(AddPatientInfoM.AddPatientInfoReq ReqData, ref AddPatientInfoM.AddPatientInfoRsp x1AddPatientInfoRsp)
        {
            RSPBase rSPBase = new RSPBase();

            if (!IDNoUtility.CheckIDNo(ReqData.IDNo))
            {
                rSPBase.ReturnCode = ErrorCode.OperateError;
                rSPBase.ReturnMsg = "身份證字號格式錯誤!";
                return rSPBase;
            }

            // 輸入驗證
            var patientFlag = _uow.Get<IX1_PatientInfoRepository>().Get(x => x.IDNo.Equals(ReqData.IDNo));

            // 準備 insert 資料
            DateTime now = DateTime.Now;

            if (patientFlag != null)
            {
                patientFlag.IDNo = ReqData.IDNo;
                patientFlag.PUCountry = string.IsNullOrEmpty(ReqData.PUCountry) ? 1 : int.Parse(ReqData.PUCountry);
                patientFlag.PUName = ReqData.PUName;
                patientFlag.PUDOB = ReqData.PUDOB;
                patientFlag.Gender = ReqData.Gender;
                patientFlag.Phone = ReqData.Phone;
                patientFlag.Cellphone = ReqData.Cellphone;
                patientFlag.ContactPhone = ReqData.ContactPhone;
                patientFlag.ContactRelation = ReqData.ContactRelation;
                patientFlag.Education = string.IsNullOrEmpty(ReqData.Education) ? 0 : int.Parse(ReqData.Education);
                patientFlag.AddrCode = ReqData.AddrCode;
                patientFlag.HCCode = ReqData.HCCode;
                patientFlag.Addr = ReqData.Addr;
                patientFlag.HCCode = ReqData.HCCode;
                patientFlag.Domicile = ReqData.Domicile;
                patientFlag.ModifyMan = ReqData.AccID;
                patientFlag.ModifyDate = now;
                _uow.Get<IX1_PatientInfoRepository>().Update(patientFlag);
                // 回傳資料 ID
                _uow.Commit();
                x1AddPatientInfoRsp.ID = patientFlag.ID;
            }
            else
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<AddPatientInfoM.AddPatientInfoReq, X1_PatientInfo>());
                var mapper = config.CreateMapper();
                var patient = mapper.Map<X1_PatientInfo>(ReqData);
                patient.CreateMan = ReqData.AccID;
                patient.CreateDate = now;
                patient.ModifyMan = ReqData.AccID;
                patient.ModifyDate = now;
                patient = _uow.Get<IX1_PatientInfoRepository>().Create(patient);
                // 回傳資料 ID
                _uow.Commit();
                x1AddPatientInfoRsp.ID = patient.ID;
            }

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase GetPatientInfo(GetPatientInfoM.GetPatientInfoReq ReqData, ref GetPatientInfoM.GetPatientInfoRsp x1GetPatientInfoRsp)
        {
            RSPBase rSPBase = new RSPBase();

            // 輸入驗證
            var patientFlag = _uow.Get<IX1_PatientInfoRepository>().Get(x => x.ID == ReqData.ID);
            if (patientFlag == null)
            {
                rSPBase.ReturnCode = ErrorCode.NotFound;
                rSPBase.ReturnMsg = "無此個案";
                return rSPBase;
            }

            // 準備 回傳 資料
            var config = new MapperConfiguration(cfg => cfg.CreateMap<X1_PatientInfo, GetPatientInfoM.PatientInfo>());
            var mapper = config.CreateMapper();
            x1GetPatientInfoRsp.Patient = mapper.Map<GetPatientInfoM.PatientInfo>(patientFlag);

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase GetPatientsLazy(GetPatientsLazyM.GetPatientLazyReq ReqData, ref GetPatientsLazyM.GetPatientLazyRsp x1GetPatientLazyRsp)
        {
            DateTime now = DateTime.Now.Date;
            RSPBase rSPBase = new RSPBase();

            // 準備 回傳 資料
            
            var filterPatients = _uow.Get<IX1_PatientInfoRepository>().GetAll().AsQueryable();

            // 篩選資料
            //if (ReqData.PUID != null)
            //{
            //    filterPatients = filterPatients.Where(x => x.PUID.Contains(ReqData.PUID));
            //}

            if (ReqData.PUName != null)
            {
                filterPatients = filterPatients.Where(x => x.PUName.Contains(ReqData.PUName));
            }

            if (ReqData.IDNo != null)
            {
                filterPatients = filterPatients.Where(x => x.IDNo.Contains(ReqData.IDNo));
            }

            var filteredPatients = new List<GetPatientsLazyM.PatientInfo>();

            var questRepo = _uow.Get<IX1_ReportQuestionRepository>();
            var ansRepo = _uow.Get<IX1_ReportAnswerDRepository>();
            var ansMRepo = _uow.Get<IX1_ReportAnswerMRepository>();
            var scheduleRepo = _uow.Get<IScheduleRepository>();
            var scheduleAfterToday = scheduleRepo.GetAll().Where(s => s.ReturnDate >= now);

            var patientReturnDateAfterToday = (from p in filterPatients
                                 join s in scheduleAfterToday on p.ID equals s.PatientID into sg
                                 select new GetPatientsLazyM.PatientInfo()
                                 {
                                     ID = p.ID,
                                     IDNo = p.IDNo,
                                     Gender = p.Gender,
                                     PUName = p.PUName,
                                     PUDOB = p.PUDOB.Value,
                                     NextVisitTime = sg.Min(s => s.ReturnDate)
                                 });

            filteredPatients = patientReturnDateAfterToday.Where(p => p.NextVisitTime != null)
                .OrderBy(p => p.NextVisitTime).ThenBy(p => p.PUName).ToList();

            var patientIDList = filteredPatients.Select(p => p.ID);
            var scheduleBeforeToday = scheduleRepo.GetAll().Where(s => s.ReturnDate < now);
            filteredPatients = filteredPatients.Union((from p in filterPatients
                                                        join s in scheduleBeforeToday on p.ID equals s.PatientID into sg
                                                        where !patientIDList.Contains(p.ID)
                                                        select new GetPatientsLazyM.PatientInfo()
                                                        {
                                                            ID = p.ID,
                                                            IDNo = p.IDNo,
                                                            Gender = p.Gender,
                                                            PUName = p.PUName,
                                                            PUDOB = p.PUDOB.Value,
                                                            NextVisitTime = sg.Min(s => s.ReturnDate)
                                                        }).OrderBy(p => p.NextVisitTime == null).ThenByDescending(p => p.NextVisitTime).ThenBy(p => p.PUName)).ToList();

            if (ReqData.Page.HasValue && ReqData.RowInPage.HasValue)
            {
                int skipRowCount = ReqData.Page.Value * ReqData.RowInPage.Value;
                filteredPatients = filteredPatients.Skip(skipRowCount)
                .Take(ReqData.RowInPage.Value).ToList();
            }

            x1GetPatientLazyRsp.Patients = filteredPatients;
            x1GetPatientLazyRsp.TotalPatient = _uow.Get<IX1_PatientInfoRepository>().GetAll().Count();

            //if (ReqData.Page.HasValue && ReqData.RowInPage.HasValue)
            //{
            //    int skipRowCount = ReqData.Page.Value * ReqData.RowInPage.Value;
            //    filterPatients = filterPatients.Skip(skipRowCount)
            //    .Take(ReqData.RowInPage.Value);
            //}

            //var patients = filterPatients.ToList();
            //List<GetPatientsLazyM.PatientInfo> response = new List<GetPatientsLazyM.PatientInfo>();
            //var config = new MapperConfiguration(cfg => cfg.CreateMap<X1_PatientInfo, GetPatientsLazyM.PatientInfo>());
            //var mapper = config.CreateMapper();

            //foreach (var item in patients)
            //{
            //    var convertedItem = mapper.Map<GetPatientsLazyM.PatientInfo>(item);
            //    response.Add(convertedItem);
            //}

            //x1GetPatientLazyRsp.Patients = response;
            //x1GetPatientLazyRsp.TotalPatient = _uow.Get<IX1_PatientInfoRepository>().GetAll().Count();

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase GetAllGroup(GetAllGroupM.GetAllGroupReq request, ref GetAllGroupM.GetAllGroupRsp response)
        {
            RSPBase rSPBase = new RSPBase();

            var allGroup = _uow.Get<IX1_PatientGroupRepository>().GetAllGroup();
            response.Data = allGroup.ToList();

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase UpdateGroup(UpdateGroupM.UpdateGroupReq request, ref UpdateGroupM.UpdateGroupRsp response)
        {
            RSPBase rSPBase = new RSPBase();

            try
            {
                var groupRepo = _uow.Get<IX1_PatientGroupRepository>();
                _uow.BeginTransaction();
                foreach (var group in request.PatientGroups)
                {
                    switch (group.State)
                    {
                        case UpdateGroupM.PatientGroupState.New:
                            AddGroup(groupRepo, group.GroupName, request.AccID);
                            break;
                        case UpdateGroupM.PatientGroupState.Modify:
                            if (UpdatePatientGroup(groupRepo, group.GroupName, request.AccID, group.ID) == null)
                            {
                                _uow.RollBackTransaction();

                                rSPBase.ReturnCode = ErrorCode.NotFound;
                                rSPBase.ReturnMsg = "無此 Group ID: " + group.ID;
                                return rSPBase;
                            }
                            break;
                        case UpdateGroupM.PatientGroupState.Delete:
                            if (DeleteGroup(groupRepo, group.GroupName, request.AccID, group.ID) == null)
                            {
                                _uow.RollBackTransaction();

                                rSPBase.ReturnCode = ErrorCode.NotFound;
                                rSPBase.ReturnMsg = "無此 Group ID: " + group.ID;
                                return rSPBase;
                            }
                            break;
                    }
                }
                _uow.CommitTransaction();
            }
            catch (Exception e)
            {
                _uow.RollBackTransaction();
                throw e;
            }

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        private int AddGroup(IX1_PatientGroupRepository repo, string groupName, string accID)
        {
            var group = new X1_PatientGroup()
            {
                GroupName = groupName,
                CreateMan = accID,
                ModifyMan = accID
            };

            group = repo.Create(group);
            _uow.Commit();
            return group.ID;
        }

        private int? UpdatePatientGroup(IX1_PatientGroupRepository repo, string groupName, string accID, int? id = -1)
        {
            var group = repo.GetGroup(id.Value);

            if (group == null)
            {
                return null;
            }

            group.GroupName = groupName;
            group.ModifyMan = accID;
            repo.Update(group);
            _uow.Commit();
            return group.ID;
        }

        private int? DeleteGroup(IX1_PatientGroupRepository repo, string groupName, string accID, int? id = -1)
        {
            var group = repo.GetGroup(id.Value);

            if (group == null)
            {
                return null;
            }

            group.IsDelete = true;
            group.ModifyMan = accID;
            group.DeleteDate = DateTime.Now;
            group.DeleteMan = accID;
            repo.Update(group);
            _uow.Commit();
            return group.ID;
        }

        public RSPBase AddOrUpdateSchedule(AddOrUpdateScheduleM.Request request, ref AddOrUpdateScheduleM.Response response)
        {
            var scheduleRepo = _uow.Get<IScheduleRepository>();
            var patient = _uow.Get<IX1_PatientInfoRepository>().Get(p => p.ID == request.PatientID);
            if (patient == null)
            {
                return ResponseHelper.CreateResponse(ErrorCode.NotFound, $"無此病患ID: {request.PatientID}");
            }

            var schedule = new Schedule()
            {
                PatientID = request.PatientID,
                ContentText = request.ContentText,
                ReturnDate = request.ReturnDate
            };
            if (request.ID.HasValue)
            {
                schedule = scheduleRepo.Get(s => s.ID == request.ID);
                if (schedule == null)
                {
                    return ResponseHelper.CreateResponse(ErrorCode.NotFound, $"無此回診時間ID: {request.ID}");
                }

                schedule.ContentText = request.ContentText;
                schedule.ReturnDate = request.ReturnDate;
                scheduleRepo.Update(schedule, request.AccID);
            }
            else
            {
                schedule = scheduleRepo.Create(schedule, request.AccID);
            }
            _uow.Commit();

            response.ID = schedule.ID;

            return ResponseHelper.Ok();
        }

        public RSPBase DeleteSchedule(DeleteScheduleM.Request request, ref DeleteScheduleM.Response response)
        {
            var scheduleRepo = _uow.Get<IScheduleRepository>();
            var schedule = scheduleRepo.Get(s => s.ID == request.ID);
            if (schedule == null)
            {
                return ResponseHelper.CreateResponse(ErrorCode.NotFound, $"無此回診ID: {request.ID}");
            }

            scheduleRepo.SoftDelete(schedule, request.AccID);
            _uow.Commit();

            return ResponseHelper.Ok();
        }

        public RSPBase GetScheduleList(GetScheduleListM.Request request, ref GetScheduleListM.Response response)
        {
            var scheduleRepo = _uow.Get<IScheduleRepository>();
            var patient = _uow.Get<IX1_PatientInfoRepository>().Get(p => p.ID == request.PatientID);
            if (patient == null)
            {
                return ResponseHelper.CreateResponse(ErrorCode.NotFound, $"無此病患ID: {request.PatientID}");
            }

            var filterSchedule = scheduleRepo.GetAll()
                                .Where(s => s.PatientID == request.PatientID);

            if (request.IgnoreExpiredDate)
            {
                DateTime now = DateTime.Now.Date;
                filterSchedule = filterSchedule.Where(s => s.ReturnDate >= now);
            }

            response.ScheduleList = filterSchedule
                                    .Select(s => new GetScheduleListM.Schedule()
                                    {
                                        ID = s.ID,
                                        PatientID = s.PatientID,
                                        ContentText = s.ContentText,
                                        ReturnDate = s.ReturnDate
                                    })
                                    .OrderBy(s => s.ReturnDate)
                                    .ToList();

            return ResponseHelper.Ok();
        }
    }
}
