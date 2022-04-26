using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using AutoMapper;
using ExcelDataReader;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using X1APServer.Repository;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility.Interface;
using X1APServer.Service.Enum;
using X1APServer.Service.ExtMethod;
using X1APServer.Service.Interface;
using X1APServer.Service.Misc;
using X1APServer.Service.Model;
using X1APServer.Service.Utils;
using X1APServer.Service;
using Xceed.Words.NET;
using System.Web;

namespace X1APServer.Service
{
    public class ReportService : IReportService
    {
        private readonly IX1UnitOfWork _uow;
        private readonly IPatientService _patientSvc;
        private readonly IFileService _fileSvc;
        private static readonly string _ManagedFolderPath = ConfigurationManager.AppSettings["ManagedFolderPath"];
        public Guid GuidNo { get; set; } = Guid.NewGuid();
        private static BMDC.Models.Auth.AuthCommon.ServiceInfo _serviceInfo = null;
        private List<string> _formatError;
        private readonly string _errorTemplate = "{0} 行:{1} 欄:{2}";
        

        public ReportService(IX1UnitOfWork uow, IPatientService patientSvc, IFileService fileSvc)
        {
            _uow = uow;
            _patientSvc = patientSvc;
            _fileSvc = fileSvc;

            _serviceInfo = new BMDC.Models.Auth.AuthCommon.ServiceInfo()
            {
                SysCode = ConfigurationManager.AppSettings["WebSysCode"],
                ServiceKey = ConfigurationManager.AppSettings["WebFrameServiceToken"]
            };
        }

        public RSPBase GetReport(GetReportM.GetReportReq ReqData, ref GetReportM.GetReportRsp x1GetReportsRsp, string serverImageFolder)
        {
            RSPBase rSPBase = new RSPBase();

            // 資料驗證
            var report = _uow.Get<IX1_ReportAnswerMRepository>().Get(x => !x.IsDelete && x.ID == ReqData.ID);
            if (report == null)
            {
                rSPBase.ReturnCode = ErrorCode.NotFound;
                rSPBase.ReturnMsg = "無此資料";
                return rSPBase;
            }

            x1GetReportsRsp.ID = report.ID;

            // 填入填答答案
            var answers = (from a in _uow.Get<IX1_ReportAnswerDRepository>().GetAll()
                           join q in _uow.Get<IX1_ReportQuestionRepository>().GetAll() on a.QuestionID equals q.ID into qg
                           from q in qg.DefaultIfEmpty()
                           join qt in _uow.Get<IX1_ReportQuestionTypeRepository>().GetAll() on q.QuestionType equals qt.ID into qtg
                           from qt in qtg.DefaultIfEmpty()
                           where a.AnswerMID == report.ID
                           select new GetReportM.ReportAnswerD()
                           {
                               QuestionID = Constants.QUEST_PREFIX + a.QuestionID,
                               Value = a.Value,
                               Type = qt.Name
                           }).ToList();

            DeleteExpiredFiles(serverImageFolder);
            var files = (from a in _uow.Get<IX1_ReportAnswerFileRepository>().GetAll()
                         join q in _uow.Get<IX1_ReportQuestionRepository>().GetAll() on a.QuestionID equals q.ID into qg
                         from q in qg.DefaultIfEmpty()
                         join qt in _uow.Get<IX1_ReportQuestionTypeRepository>().GetAll() on q.QuestionType equals qt.ID into qtg
                         from qt in qtg.DefaultIfEmpty()
                         where a.AnswerMID == report.ID
                         select new
                         {
                             QuestionID = Constants.QUEST_PREFIX + a.QuestionID,
                             Type = qt.Name,
                             ID = a.ID
                         }).ToList();

            var fileresponse = new List<GetReportM.ReportAnswerFile>();
            foreach (var item in files)
            {
                fileresponse.Add(new GetReportM.ReportAnswerFile()
                {
                    QuestionID = item.QuestionID,
                    FileURL = item.ID.ToString(),
                    Type = item.Type
                });
            }

            x1GetReportsRsp.Answers = answers;
            x1GetReportsRsp.Files = fileresponse;
            x1GetReportsRsp.Lock = report.Lock;
            x1GetReportsRsp.Status = report.Status;

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        /// <summary>
        /// 刪除 Temp 資料夾過期檔案
        /// </summary>
        /// <param name="path"></param>
        private void DeleteExpiredFiles(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            var fileInfos = directoryInfo.GetFiles();

            foreach (var fileInfo in fileInfos)
            {
                if (DateTime.Now.Subtract(fileInfo.LastAccessTime).TotalHours > 24)
                {
                    fileInfo.Delete();
                }
            }
        }

        public RSPBase GetReports(GetReportsM.GetReportsReq ReqData, ref GetReportsM.GetReportsRsp x1GetReportsRsp)
        {
            RSPBase rSPBase = new RSPBase();

            // 輸入資料驗證
            var reportMs = _uow.Get<IX1_ReportMRepository>().GetAllVersionReportM(ReqData.Category).Select(x => x.ID).ToList();
            if (reportMs.Count == 0)
            {
                rSPBase.ReturnCode = ErrorCode.NotFound;
                rSPBase.ReturnMsg = "無此Report類別";
                return rSPBase;
            }

            var patient = _uow.Get<IX1_PatientInfoRepository>().Get(x => x.IDNo.Equals(ReqData.IDNo));

            // 準備回傳資料
            var reports = (from r in _uow.Get<IX1_ReportAnswerMRepository>().GetAll()
                           where !r.IsDelete && r.PID == patient.ID && reportMs.Contains(r.ReportID)
                           select new GetReportsM.Report()
                           {
                               ID = r.ID,
                               FillingDate = r.FillingDate,
                               CreateDate = r.CreateDate,
                               CreateMan = r.CreateMan,
                               ModifyDate = r.ModifyDate,
                               ModifyMan = r.ModifyMan,
                               PatientName = patient.PUName,
                               Status = r.Status
                           }).OrderByDescending(x => x.FillingDate).ToList();

            reports = reports.Select(r =>
            {
                // 取最新的匯出 Report
                var exportReport = _uow.Get<IReportExportFileRepository>().GetAll()
                    .Where(x => x.RAMID == r.ID)
                    .OrderByDescending(x => x.CreateDate).FirstOrDefault();

                if (exportReport != null)
                {
                    // 取醫生姓名
                    //using (var client = new AuthService.AuthServiceClient())
                    //{
                    //    var request = new BMDC.Models.Auth.GetUserM.GetUserReq()
                    //    {
                    //        AccID = exportReport.CreateMan
                    //    };
                    //    var response = client.GetUser(_serviceInfo, request);

                    //    r.ExportDate = exportReport.CreateDate;
                    //    r.ExportDoctor = response.AccName;
                    //}
                }

                return r;
            }).ToList();

            x1GetReportsRsp.Reports = reports;

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase UpdateReport(UpdateReportM.UpdateReportReq ReqData, ref UpdateReportM.UpdateReportRsp x1SaveReportRsp)
        {
            RSPBase rSPBase = new RSPBase();

            // 輸入資料驗證
            var reportAnsM = _uow.Get<IX1_ReportAnswerMRepository>().Get(x => !x.IsDelete && x.ID == ReqData.ID);
            if (reportAnsM == null)
            {
                rSPBase.ReturnCode = ErrorCode.NotFound;
                rSPBase.ReturnMsg = "無此Report ID";
                return rSPBase;
            }

            if (reportAnsM.Lock)
            {
                rSPBase.ReturnCode = ErrorCode.OperateError;
                rSPBase.ReturnMsg = "此 Report 已鎖定";
                return rSPBase;
            }

            reportAnsM.ModifyMan = ReqData.AccID;
            _uow.Get<IX1_ReportAnswerMRepository>().Update(reportAnsM);
            _uow.Commit();

            // 更新答案
            var repo = _uow.Get<IX1_ReportAnswerDRepository>();
            var fileRepo = _uow.Get<IX1_ReportAnswerFileRepository>();
            var reportAnsDs = repo.GetAll()
                .Where(x => x.AnswerMID == reportAnsM.ID).ToList();
            var reportFiles = fileRepo.GetAll().Where(x => x.AnswerMID == reportAnsM.ID);

            foreach (var item in ReqData.Answers)
            {
                string questionID = item.QuestionID.Replace(Constants.QUEST_PREFIX, ""); // 去除 Q_ prefix
                int questionIDInt = -1;
                int.TryParse(questionID, out questionIDInt);

                if (questionIDInt == -1)
                {
                    rSPBase.ReturnCode = ErrorCode.OperateError;
                    rSPBase.ReturnMsg = string.Format("QuestionID: {0} 格式錯誤", item.QuestionID);
                    return rSPBase;
                }

                var question = _uow.Get<IX1_ReportQuestionRepository>().Get(x => x.ID == questionIDInt);

                var ansD = reportAnsDs.FirstOrDefault(x => x.QuestionID == questionIDInt);

                if (ansD == null)
                {
                    ansD = new X1_Report_Answer_Detail()
                    {
                        AnswerMID = reportAnsM.ID,
                        QuestionID = questionIDInt,
                        Value = item.Value
                    };
                    repo.Create(ansD);
                }
                else
                {
                    ansD.AnswerMID = reportAnsM.ID;
                    ansD.QuestionID = questionIDInt;
                    ansD.Value = item.Value;
                    repo.Update(ansD);
                }

                var file = reportFiles.FirstOrDefault(x => x.QuestionID == questionIDInt);
                if (file != null && string.IsNullOrEmpty(item.Value))
                {
                    fileRepo.Delete(file);
                }
            }
            _uow.Commit();

            x1SaveReportRsp.ID = reportAnsM.ID;

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase UpdateReportStatus(UpdateReportStatusM.UpdateReportStatusReq request)
        {
            UpdateReportStatusM.UpdateReportStatusRsp ret = new UpdateReportStatusM.UpdateReportStatusRsp();
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

                reportAnsM.Status = request.Status;
                reportAnsM.ModifyDate = DateTime.Now;
                reportAnsM.ModifyMan = "User";

                _uow.Get<IX1_ReportAnswerMRepository>().Update(reportAnsM);
                _uow.Commit();
            }
            catch (Exception ex)
            {
                ret.ReturnCode = ErrorCode.Exception;
                ret.ReturnMsg = ex.Message;
            }
            return ret;
        }

        public RSPBase AddGeneralReport(AddGeneralReportM.AddGeneralReportReq ReqData, ref AddGeneralReportM.AddGeneralReportRsp x1AddGeneralReportRsp)
        {
            RSPBase rSPBase = new RSPBase();
            DateTime now = DateTime.Now;

            var patientExisted = _uow.Get<IX1_PatientInfoRepository>().Any(x => x.ID == ReqData.PID);
            if (!patientExisted)
            {
                rSPBase.ReturnCode = ErrorCode.NotFound;
                rSPBase.ReturnMsg = string.Format("無此病患ID: {0}", ReqData.PID);
                return rSPBase;
            }

            var report = _uow.Get<IX1_ReportMRepository>().GetLatestReportM(id: ReqData.ID);
            if (report == null)
            {
                rSPBase.ReturnCode = ErrorCode.NotFound;
                rSPBase.ReturnMsg = string.Format("無此Report ID: {0}", ReqData.ID);
                return rSPBase;
            }

            var ansMRepo = _uow.Get<IX1_ReportAnswerMRepository>();
            //if (!string.IsNullOrEmpty(ReqData.SequenceNum))
            //{
            //    var ansM = ansMRepo.Get(m => m.SequenceNum == ReqData.SequenceNum);
            //    if (ansM == null)
            //    {
            //        rSPBase.ReturnCode = ErrorCode.NotFound;
            //        rSPBase.ReturnMsg = $"批號不存在!";
            //        return rSPBase;
            //    }
            //}
            //else
            //{
            //    ReqData.SequenceNum = ansMRepo.GetSequenceNum().Item2;
            //}


            // 開始一個 Transaction
            try
            {
                _uow.BeginTransaction();

                // Insert Report Answer Main
                X1_Report_Answer_Main addReportData = new X1_Report_Answer_Main()
                {
                    OID = ReqData.OID,
                    PID = ReqData.PID.Value,
                    ReportID = report.ID,
                    FillingDate = ReqData.FillingDate ?? now,
                    CreateDate = now,
                    CreateMan = ReqData.AccID,
                    ModifyDate = now,
                    ModifyMan = ReqData.AccID,
                    IsDelete = false,
                    Status = 1
                };
                addReportData = ansMRepo.Create(addReportData);
                _uow.Commit();

                foreach (var item in ReqData.Answers)
                {
                    string questionStr = item.QuestionID.Replace(Constants.QUEST_PREFIX, "");
                    int questionID = -1;
                    int.TryParse(questionStr, out questionID);

                    if (questionID == -1)
                    {
                        rSPBase.ReturnCode = ErrorCode.OperateError;
                        rSPBase.ReturnMsg = string.Format("Question ID 非數字: {0}", questionStr);
                        _uow.RollBackTransaction();
                        return rSPBase;
                    }

                    var question = _uow.Get<IX1_ReportQuestionRepository>().Any(x => x.ID == questionID);

                    X1_Report_Answer_Detail answer = new X1_Report_Answer_Detail()
                    {
                        QuestionID = questionID,
                        AnswerMID = addReportData.ID,
                        Value = item.Value
                    };
                    _uow.Get<IX1_ReportAnswerDRepository>().Create(answer);
                }
                x1AddGeneralReportRsp.ReportID = addReportData.ID;

                _uow.Commit();

                _uow.CommitTransaction();
        }
            catch (Exception ex)
            {
                var err = ex;
                _uow.RollBackTransaction();
            throw;
        }

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase ExportReport(ExportReportM.ExportReportReq ReqData, ref ExportReportM.ExportReportRsp exportReportRsp, string rootPath)
        {
            RSPBase rSPBase = new RSPBase();

            var report = _uow.Get<IX1_ReportExportTemplateRepository>().GetTemplate(ReqData.ID.Value);
            if (report == null)
            {
                return ResponseHelper.CreateResponse(ErrorCode.NotFound, string.Format("無此報告模板 ID: {0}", ReqData.ID));
            }

            var file = _uow.Get<ISystemFileRepository>().GetFile(report.SystemFileID);

            string templatePath = Path.Combine(rootPath, file.FilePath);
            if (!File.Exists(templatePath))
            {
                return ResponseHelper.CreateResponse(ErrorCode.NotFound, "找不到對應模板檔案");
            }

            try
            {
                // 匯出路徑
                string outputPath = Path.Combine(_ManagedFolderPath, "ExportFile");
                Directory.CreateDirectory(outputPath);
                // 取得題目最新答案
                var answerList = GetMappingAnswer(templatePath, ReqData.PatientID.Value);
                answerList = AddExtraAns(answerList, ReqData.Extra);
                //取得病患資料
                var patient = _uow.Get<IX1_PatientInfoRepository>().Get(x => x.ID == ReqData.PatientID);
                answerList.AddIfNotExist("姓名", patient.PUName);
                answerList.AddIfNotExist("身分證字號", patient.IDNo);
                answerList.AddIfNotExist("生日", patient.PUDOB != null ? patient.PUDOB.Value.ToString("yyyy-MM-dd") : "");
                answerList.AddIfNotExist("性別", patient.Gender.Equals("M") ? "男" : "女");
                // 將資料填入對應樣板
                string wordTempPath = Path.Combine(rootPath, "App_Data/Temp");
                string wordPath = InsertDataToDoc(answerList, templatePath, wordTempPath);
                // 轉換成 PDF
                exportReportRsp.pdfURL = ConvertWordToPdf(wordPath, outputPath);
                // 複製 PDF 至 Content
                string pdfPath = Path.Combine(outputPath, exportReportRsp.pdfURL);
                string shareDirectory = Path.Combine(rootPath, "Content\\Temp");
                Directory.CreateDirectory(shareDirectory);
                string sharePath = Path.Combine(shareDirectory, exportReportRsp.pdfURL);
                File.Copy(pdfPath, sharePath, true);
            }
            catch (CustomException e)
            {
                return ResponseHelper.CreateResponse(e.ErrorCode, e.ErrorMsg);
            }

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        private Dictionary<string, string> GetMappingAnswer(string templatePath, int pateintID)
        {
            Dictionary<string, string> answerList = new Dictionary<string, string>();
            using (DocX document = DocX.Load(templatePath))
            {
                var questRepo = _uow.Get<IX1_ReportQuestionRepository>();
                var answerDRepo = _uow.Get<IX1_ReportAnswerDRepository>();
                var answerMRepo = _uow.Get<IX1_ReportAnswerMRepository>();

                var patientAnswerList = (from ansM in answerMRepo.GetAll()
                                         join ansD in answerDRepo.GetAll() on ansM.ID equals ansD.AnswerMID
                                         join quest in questRepo.GetAll() on ansD.QuestionID equals quest.ID
                                         where ansM.PID == pateintID
                                         orderby ansD.ID descending
                                         select new { AnsM = ansM, AnsD = ansD, Quest = quest }
                                         ).ToList();
                string questPattern = "«([^,]+?)(,(.+?))?»";

                foreach (var matchQuest in document.FindUniqueByPattern(questPattern, RegexOptions.IgnoreCase))
                {
                    var matchGroup = Regex.Match(matchQuest, questPattern, RegexOptions.IgnoreCase);
                    // 問題編號
                    var matchString = matchGroup.Groups[0].Value;
                    var questionNo = matchGroup.Groups[1].Value.Trim();
                    var option = matchGroup.Groups[3].Value.Trim();

                    var answer = patientAnswerList.FirstOrDefault();

                    switch (option)
                    {
                        case "Pre1":
                            answer = patientAnswerList.Where(a => a.Quest.QuestionNo == questionNo)
                                            .ElementAtOrDefault(1);
                            break;
                        case "Pre2":
                            answer = patientAnswerList.Where(a => a.Quest.QuestionNo == questionNo)
                                            .ElementAtOrDefault(2);
                            break;
                        default:
                            answer = patientAnswerList.FirstOrDefault(a => a.Quest.QuestionNo == questionNo);
                            break;
                    }

                    if (answer != null)
                    {
                        var singleReportAnsList = patientAnswerList.Where(a => a.AnsM.ID == answer.AnsM.ID)
                                                    .Select(a => a.AnsD).ToList();
                        answerList.Add(matchString, GetValue(singleReportAnsList, answer.Quest));
                    }
                }
            }

            return answerList;
        }

        private Dictionary<string, string> AddExtraAns(Dictionary<string, string> answerList, List<ExportReportM.KeyValue> extraAnsList)
        {
            if (extraAnsList == null) return answerList;

            foreach (var extraAns in extraAnsList)
            {
                if (!answerList.ContainsKey(extraAns.Key))
                {
                    answerList.Add(extraAns.Key, extraAns.Value);
                }
            }

            return answerList;
        }

        private Dictionary<string, string> GetIDMapAnswer(Dictionary<int, List<int>> idList, int patientID, DateTime specificDate)
        {
            var latestReportDict = new Dictionary<int, int>();
            var answerList = new Dictionary<string, string>();
            var notFoundReportAnsID = new List<int>();
            var reportAnsMRepo = _uow.Get<IX1_ReportAnswerMRepository>();
            var reportAnsDRepo = _uow.Get<IX1_ReportAnswerDRepository>();
            var questRepo = _uow.Get<IX1_ReportQuestionRepository>();

            foreach (var report in idList)
            {
                if (!latestReportDict.ContainsKey(report.Key))
                {
                    var reportAns = reportAnsMRepo.GetMaxFillingDateReport(report.Key, patientID, specificDate);
                    if (reportAns == null)
                    {
                        continue;
                    }

                    latestReportDict.Add(report.Key, reportAns.ID);
                }

                var reportAllAns = (from a in reportAnsDRepo.GetAllAns(latestReportDict[report.Key])
                                    join q in questRepo.GetAll() on a.QuestionID equals q.ID
                                    select new { Ans = a, Quest = q }).ToList();
                var reportAllQuest = (from q in questRepo.GetAll()
                                      where q.ReportID == report.Key
                                      select q);
                foreach (var quest in reportAllQuest)
                {
                    if (!string.IsNullOrEmpty(quest.QuestionNo))
                    {
                        var ansList = reportAllAns.Select(a => a.Ans).ToList();
                        answerList.Add(quest.QuestionNo, GetValue(ansList, quest));
                    }

                }

            }

            return answerList;
        }

        private string GetValue(List<X1_Report_Answer_Detail> source, X1_Report_Question question)
        {
            var answer = source.FirstOrDefault(ans => ans.QuestionID == question.ID);
            if (answer == null && (QuestionType)question.QuestionType != QuestionType.checkbox) return null;

            switch ((QuestionType)question.QuestionType)
            {
                case QuestionType.radio:
                case QuestionType.select:
                    if (question.OtherAnsID.HasValue &&
                        question.OtherAnsID != 0 &&
                        answer.Value.Equals("other"))
                    {
                        var otherAns = source.FirstOrDefault(ans => ans.QuestionID == question.OtherAnsID);
                        answer.Value = otherAns.Value;
                    }
                    else
                    {
                        var radioOptions = JsonConvert.DeserializeObject<List<AddReportMainM.Answeroption>>(question.AnswerOption);
                        answer.Value = radioOptions.FirstOrDefault(o => o.Value == answer.Value)?.OptionText;
                    }

                    return answer.Value;
                case QuestionType.checkbox:
                    var options = new List<AddReportMainM.Answeroption>();

                    if (string.IsNullOrEmpty(question.AnswerOption))
                    {
                        return answer.Value;
                    }

                    options = JsonConvert.DeserializeObject<List<AddReportMainM.Answeroption>>(question.AnswerOption);

                    if (question.OtherAnsID.HasValue &&
                        question.OtherAnsID != 0)
                    {
                        var otherAns = source.FirstOrDefault(ans => ans.QuestionID == question.OtherAnsID);
                        var otherOption = options.FirstOrDefault(o => o.Value.Equals("other"));
                        var otherOptionAns = source.FirstOrDefault(ans => ans.QuestionID == otherOption.ID);

                        if (!string.IsNullOrEmpty(otherOptionAns?.Value))
                        {
                            otherOptionAns.X1_Report_Question.QuestionText = otherAns.Value;
                        }
                    }

                    var ansAry = source.Where(a => options.Any(o => o.ID == a.QuestionID) && !string.IsNullOrEmpty(a.Value))
                        .Select(a => a.X1_Report_Question.QuestionText).ToArray();
                    return string.Join(",", ansAry);
                default:
                    return answer.Value;
            }
        }

        private Dictionary<int, List<int>> GetTemplateQuestID(string templatePath)
        {
            using (DocX document = DocX.Load(templatePath))
            {
                var idList = new Dictionary<int, List<int>>();
                var questRepo = _uow.Get<IX1_ReportQuestionRepository>();
                var reportRepo = _uow.Get<IX1_ReportMRepository>();
                var questList = (from r in reportRepo.GetAll()
                                 join q in questRepo.GetAll() on r.ID equals q.ReportID
                                 where r.IsPublish && !r.IsDelete
                                 select q).ToList();
                string questPattern = "«(.+?)»";

                foreach (var matchQuest in document.FindUniqueByPattern(questPattern, RegexOptions.IgnoreCase))
                {
                    var matchGroup = Regex.Match(matchQuest, questPattern, RegexOptions.IgnoreCase);
                    // 問題ID
                    var id = matchGroup.Groups[1].Value;
                    string idStr = id.Replace(" ", "");

                    var quest = questList.FirstOrDefault(q => q.QuestionNo.Replace(" ", "").Equals(idStr));
                    if (quest == null)
                    {
                        continue;
                        throw new CustomException()
                        {
                            ErrorCode = ErrorCode.NotFound,
                            ErrorMsg = "資料庫中無ID對應題目! ID: " + id
                        };
                    }

                    if (!idList.ContainsKey(quest.ReportID))
                    {
                        idList.Add(quest.ReportID, new List<int>());
                    }

                    if (!idList[quest.ReportID].Any(x => x == quest.ID))
                    {
                        idList[quest.ReportID].Add(quest.ID);
                    }
                }

                return idList;
            }
        }

        private string InsertDataToDoc(Dictionary<string, string> ansList, string tamplatePath, string savePath)
        {
            // 樣板路徑
            string WTPath = tamplatePath;
            // 儲存名稱
            string saveFileName = Guid.NewGuid().ToString() + ".docx";
            // 儲存路徑
            string saveFilePath = Path.Combine(savePath, saveFileName);

            using (DocX document = DocX.Load(WTPath))
            {
                // 替換樣板值
                foreach (var ans in ansList)
                {
                    string searchText = ans.Key;
                    string replaceValue = ans.Value ?? "";
                    document.ReplaceText(searchText, replaceValue);
                }

                // 未對應的填空
                document.ReplaceText("«.+?»", "", options: RegexOptions.IgnoreCase, escapeRegEx: false);
                document.SaveAs(saveFilePath);
            }

            return saveFilePath;
        }

        private string ConvertWordToPdf(string wordPath, string outputFolderPath)
        {
            Directory.CreateDirectory(outputFolderPath);

            var wordFileName = Path.GetFileName(wordPath);
            // 沒有附檔名的檔名
            var wordFileNameWithoutExt = Path.GetFileNameWithoutExtension(wordPath);
            // 匯出名稱
            var outputFileName = wordFileNameWithoutExt + ".pdf";
            // 匯出路徑
            var pdfOutputPath = ConfigurationManager.AppSettings["ConvertPdfPath"];
            var outputPath = Path.Combine(pdfOutputPath, outputFileName);
            // 重新命名包含路徑
            var renamePath = Path.Combine(outputFolderPath, wordFileNameWithoutExt);

            var wordInputPath = Path.Combine(ConfigurationManager.AppSettings["ConvertWordPath"], wordFileName);
            File.Copy(wordPath, wordInputPath, true);

            int i = 0;
            // 等待Pdf產生
            while (!File.Exists(outputPath))
            {
                Thread.Sleep(500);
                if (++i >= 120) break;
            }

            if (File.Exists(outputPath))
            {
                File.Copy(outputPath, renamePath, true);
                try
                {
                    File.Delete(wordInputPath);
                    File.Delete(outputPath);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                throw new CustomException()
                {
                    ErrorCode = ErrorCode.OperateError,
                    ErrorMsg = "Pdf產生錯誤，請嘗試重新產生"
                };
            }

            return wordFileNameWithoutExt;
        }

        public RSPBase GetExportReportListLazy(GetExportReportListLazyM.GetExportReportListLazyReq ReqData, ref GetExportReportListLazyM.GetExportReportListLazyRsp getExportReportList)
        {
            RSPBase rSPBase = new RSPBase();

            var allExportReport = _uow.Get<IReportExportFileRepository>().GetAll();
            // 篩選個案
            if (ReqData.PID != null)
            {
                allExportReport = _uow.Get<IX1_ReportAnswerMRepository>().GetAll()
                    .Where(x => x.PID == ReqData.PID)
                    .Join(allExportReport,
                    o => o.ID,
                    i => i.RAMID,
                    (o, i) => i);
            }

            if (!string.IsNullOrEmpty(ReqData.ReportCategory))
            {
                var report = _uow.Get<IX1_ReportMRepository>().Get(x => x.Category.Equals(ReqData.ReportCategory));
                if (report == null)
                {
                    rSPBase.ReturnCode = ErrorCode.NotFound;
                    rSPBase.ReturnMsg = "無此Report類別: " + ReqData.ReportCategory;
                    return rSPBase;
                }

                allExportReport = _uow.Get<IX1_ReportAnswerMRepository>().GetAll()
                    .Where(x => x.ReportID == report.ID)
                    .Join(allExportReport,
                    o => o.ID,
                    i => i.RAMID,
                    (o, i) => i);
            }

            if (!string.IsNullOrEmpty(ReqData.FileName))
            {
                allExportReport = allExportReport.Where(x => x.FileName.Contains(ReqData.FileName));
            }

            allExportReport = allExportReport.OrderByDescending(x => x.ModifyDate);
            if (ReqData.Page != null)
            {
                allExportReport = allExportReport.Skip(ReqData.Page.Value * ReqData.RowInPage)
                    .Take(ReqData.RowInPage);
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<X1_Report_Export_File, GetExportReportListLazyM.ExportReport>());
            var mapper = config.CreateMapper();
            getExportReportList.ExportReportList = mapper.Map<List<GetExportReportListLazyM.ExportReport>>(allExportReport.ToList());

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase GetQuestionList(GetQuestionListM.GetQuestionListReq ReqData, ref GetQuestionListM.GetQuestionListRsp getQuestionListRsp)
        {
            RSPBase rSPBase = new RSPBase();

            var dbQuestList = _uow.Get<IX1_ReportQuestionRepository>().GetAll().AsEnumerable();
            var qList = new List<GetQuestionListM.Question>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<X1_Report_Question, GetQuestionListM.Question>());
            var mapper = config.CreateMapper();

            // 取得對應的題目組成List
            foreach (var quest in ReqData.QuestTextList)
            {
                var findQuest = dbQuestList.FirstOrDefault(x => x.QuestionText.Equals(quest));

                if (findQuest != null)
                {
                    qList.Add(mapper.Map<GetQuestionListM.Question>(findQuest));
                }
                else
                {
                    qList.Add(null);
                }

            }

            getQuestionListRsp.QuestionList = qList;

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase UpdateReportFile(UpdateReportFileM.UpdateReportFileReq ReqData, ref UpdateReportFileM.UpdateReportFileRsp updateReportFileRsp)
        {
            RSPBase rSPBase = new RSPBase();

            // 輸入驗證
            #region 輸入驗證
            var report = _uow.Get<IX1_ReportAnswerMRepository>().Get(x => x.ID == ReqData.ReportID);
            if (report == null)
            {
                rSPBase.ReturnCode = ErrorCode.OperateError;
                rSPBase.ReturnMsg = "無此 Report ID: " + ReqData.ReportID;
                return rSPBase;
            }

            string questionID = ReqData.QuestionID.Replace(Constants.QUEST_PREFIX, ""); // 去除 Q_ prefix
            int questionIDInt = -1;
            int.TryParse(questionID, out questionIDInt);

            if (questionIDInt == -1)
            {
                rSPBase.ReturnCode = ErrorCode.OperateError;
                rSPBase.ReturnMsg = string.Format("QuestionID: {0} 格式錯誤", ReqData.QuestionID);
                return rSPBase;
            }

            //var question = _uow.Get<IX1_ReportQuestionRepository>().Get(x => x.ID == questionIDInt);
            //if (question == null)
            //{
            //    rSPBase.ReturnCode = ErrorCode.OperateError;
            //    rSPBase.ReturnMsg = string.Format("無此 Question ID: ", questionIDInt);
            //    return rSPBase;
            //}

            if (!File.Exists(ReqData.FilePath))
            {
                rSPBase.ReturnCode = ErrorCode.OperateError;
                rSPBase.ReturnMsg = string.Format("檔案不存在: ", ReqData.FilePath);
                return rSPBase;
            }
            #endregion

            var answer = _uow.Get<IX1_ReportAnswerFileRepository>().Get(x => x.AnswerMID == ReqData.ReportID && x.QuestionID == questionIDInt);
            string storeDir = Path.Combine(_ManagedFolderPath, "File");
            Directory.CreateDirectory(storeDir);
            string guid, filePath;

            if (answer == null)
            {
                var storeData = new X1_Report_Answer_File();
                guid = Guid.NewGuid().ToString().Replace("-", "");
                filePath = Path.Combine(storeDir, guid);

                storeData.AnswerMID = report.ID;
                storeData.QuestionID = questionIDInt;
                storeData.GUID = Guid.Parse(guid);
                storeData.FilePath = storeDir;
                storeData.FileName = ReqData.FileName;
                storeData.FileExt = Path.GetExtension(ReqData.FileName);

                _uow.Get<IX1_ReportAnswerFileRepository>().Create(storeData);
            }
            else
            {
                guid = answer.GUID.ToString();
                filePath = Path.Combine(_ManagedFolderPath, "File", guid);
                answer.FileName = ReqData.FileName;
                answer.FileExt = Path.GetExtension(ReqData.FileName);
            }

            _uow.Commit();
            File.Copy(ReqData.FilePath, filePath, true);
            File.Delete(ReqData.FilePath);

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase GetLatestCBCExportDate(GetLatestCBCExportDateM.GetLatestCBCExportDateReq request, ref GetLatestCBCExportDateM.GetLatestCBCExportDateRsp getLatestCBCExportDateRsp)
        {
            RSPBase rSPBase = new RSPBase();
            // 驗證Report
            var reportAnsM = _uow.Get<IX1_ReportAnswerMRepository>().Get(x => !x.IsDelete && x.ID == request.ReportAnsMID);
            if (reportAnsM == null)
            {
                rSPBase.ReturnCode = ErrorCode.OperateError;
                rSPBase.ReturnMsg = "無此Report答案ID: " + request.ReportAnsMID;
                return rSPBase;
            }

            // 取得所有 Convention Report ID
            var conventionM = _uow.Get<IX1_ReportMRepository>().GetAll().Where(x => x.Category.Equals("A")).Select(x => x.ID).ToList();
            // 取得靠近 request report create date 最近的 Convention Report
            var conventionAnsM = _uow.Get<IX1_ReportAnswerMRepository>().GetAll()
                .Where(x => conventionM.Contains(x.ReportID) && x.CreateDate <= reportAnsM.CreateDate && x.PID == reportAnsM.PID && !x.IsDelete)
                .OrderByDescending(x => x.CreateDate).FirstOrDefault();

            // 驗證 Convention Report
            if (conventionAnsM == null)
            {
                rSPBase.ReturnCode = ErrorCode.OperateError;
                rSPBase.ReturnMsg = "無CBC資料";
                return rSPBase;
            }

            var conventionExport = _uow.Get<IReportExportFileRepository>().Get(x => x.RAMID == conventionAnsM.ID);
            if (conventionExport == null)
            {
                rSPBase.ReturnCode = ErrorCode.OperateError;
                rSPBase.ReturnMsg = "CBC尚未匯出";
                return rSPBase;
            }

            getLatestCBCExportDateRsp.CBCExportDate = conventionExport.CreateDate;

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase UnlockReport(UnlockReportM.UnlockReportReq request, ref UnlockReportM.UnlockReportRsp unlockReportRsp)
        {
            RSPBase rSPBase = new RSPBase();
            // 驗證Report
            var reportAnsM = _uow.Get<IX1_ReportAnswerMRepository>().Get(x => !x.IsDelete && x.ID == request.ReportAnsMID);
            if (reportAnsM == null)
            {
                rSPBase.ReturnCode = ErrorCode.OperateError;
                rSPBase.ReturnMsg = "無此Report答案ID: " + request.ReportAnsMID;
                return rSPBase;
            }

            reportAnsM.Lock = false;
            _uow.Get<IX1_ReportAnswerMRepository>().Update(reportAnsM);
            _uow.Commit();

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase CloseoutReport(CloseoutReportM.CloseoutReportReq request, ref CloseoutReportM.CloseoutReportRsp closeoutReportRsp)
        {
            RSPBase rSPBase = new RSPBase();
            // 驗證Report
            var reportAnsM = _uow.Get<IX1_ReportAnswerMRepository>().Get(x => x.Status < 2 && x.ID == request.ReportAnsMID);
            if (reportAnsM == null)
            {
                rSPBase.ReturnCode = ErrorCode.OperateError;
                rSPBase.ReturnMsg = "無此Report答案ID: " + request.ReportAnsMID;
                return rSPBase;
            }

            reportAnsM.Status = 6;
            reportAnsM.ModifyMan = request.AccID;
            reportAnsM.ModifyDate = DateTime.Now;
            _uow.Get<IX1_ReportAnswerMRepository>().Update(reportAnsM);
            _uow.Commit();

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase GetAllReportMain(GetAllReportMainM.GetAllReportMainReq request, ref GetAllReportMainM.GetAllReportMainRsp response)
        {
            RSPBase rSPBase = new RSPBase();

            var reportMRepo = _uow.Get<IX1_ReportMRepository>();
            var allReportM = reportMRepo.GetAll()
                .Where(r => !r.IsDelete)
                .GroupBy(r => r.Category)
                .Select(r => new
                {
                    Category = r.Key,
                    CreateDate = r.Min(cr => cr.CreateDate)
                });
            var reportMList = reportMRepo.GetAllCategoryLatestReportM(request.isPublish)
                .Join(allReportM, o => o.Category, i => i.Category, (o, i) => new { ReportM = o, FirstCreateDate = i.CreateDate })
                .OrderBy(r => r.ReportM.IndexNum).ThenByDescending(r => r.FirstCreateDate)
                .Select(r => r.ReportM).ToList();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<X1_Report_Main, GetAllReportMainM.ReportMain>());
            var mapper = config.CreateMapper();
            var responseList = mapper.Map<List<GetAllReportMainM.ReportMain>>(reportMList);
            response.ReportMainList = responseList;

            var publishedReportMList = _uow.Get<IX1_ReportMRepository>().GetAll().Where(r => !r.IsDelete && r.PublishDate != null).ToList();
            response.ReportMainList = response.ReportMainList.Select(rm =>
            {
                rm.ReportVersionList = publishedReportMList.Where(prm => prm.Category == rm.Category)
                .OrderByDescending(prm => prm.PublishDate)
                .Select(prm => new GetAllReportMainM.ReportVersionItem()
                {
                    ID = prm.ID,
                    PublishDate = prm.PublishDate
                }).ToList();
                return rm;
            });

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase GetReportMain(GetReportMainM.GetReportMainReq request, ref GetReportMainM.GetReportMainRsp response)
        {
            RSPBase rSPBase = new RSPBase();
            X1_Report_Main reportMain;

            if (request.ID.HasValue)
            {
                reportMain = _uow.Get<IX1_ReportMRepository>().GetLatestReportM(id: request.ID, isPublish: request.IsPublish);
                if (reportMain == null)
                {
                    rSPBase.ReturnCode = ErrorCode.NotFound;
                    rSPBase.ReturnMsg = "無此Report Main ID: " + request.ID;
                    return rSPBase;
                }
            }
            else if (request.ReportAnsMID.HasValue)
            {
                // 檢查 Report Answer Main
                var reportAnsM = _uow.Get<IX1_ReportAnswerMRepository>().GetReport(request.ReportAnsMID, null);
                if (reportAnsM == null)
                {
                    rSPBase.ReturnCode = ErrorCode.NotFound;
                    rSPBase.ReturnMsg = "無此Report Answer ID: " + request.ReportAnsMID;
                    return rSPBase;
                }

                reportMain = _uow.Get<IX1_ReportMRepository>().GetLatestReportM(id: reportAnsM.ReportID, isPublish: request.IsPublish);
            }
            else if (!string.IsNullOrEmpty(request.ReportCategory))
            {
                reportMain = _uow.Get<IX1_ReportMRepository>().GetLatestReportM(request.ReportCategory, isPublish: request.IsPublish);
                if (reportMain == null)
                {
                    rSPBase.ReturnCode = ErrorCode.NotFound;
                    rSPBase.ReturnMsg = "無此Report 類別: " + request.ReportCategory;
                    return rSPBase;
                }
            }
            else
            {
                rSPBase.ReturnCode = ErrorCode.ArgInvalid;
                rSPBase.ReturnMsg = "參數不得為空: ID, ReportCategory, ReportAnsMID";
                return rSPBase;
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<X1_Report_Main, GetReportMainM.ReportMain>());
            var mapper = config.CreateMapper();
            response.Data = mapper.Map<GetReportMainM.ReportMain>(reportMain);

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase UpdateReportMain(UpdateReportMainM.UpdateReportMainReq request, ref UpdateReportMainM.UpdateReportMainRsp response)
        {
            RSPBase rSPBase = new RSPBase();

            // 檢查ID
            var reportM = _uow.Get<IX1_ReportMRepository>().GetLatestReportM(id: request.Structure.ID);
            if (reportM == null)
            {
                rSPBase.ReturnCode = ErrorCode.NotFound;
                rSPBase.ReturnMsg = "無此Report Main ID: " + request.Structure.ID;
                return rSPBase;
            }

            try
            {
                _uow.BeginTransaction();

                _uow.Get<IX1_ReportQuestFileRepository>().DeleteAllByReportMID(reportM.ID);

                // 取得所有問題類型
                var structure = request.Structure;

                ProcessQuestion(ref structure, false);

                request.Structure = structure;
                var json = JsonConvert.SerializeObject(request.Structure);
                reportM.Title = request.Structure.Title;
                reportM.Category = request.Structure.Category;
                reportM.Description = request.Structure.Description;
                reportM.IsDelete = request.IsDelete ?? reportM.IsDelete;
                reportM.ModifyMan = request.AccID;
                reportM.OutputJson = json;
                reportM.ReserveDate = request.ReserveDate.Value;
                reportM.IndexNum = request.Structure.IndexNum;
                _uow.Get<IX1_ReportMRepository>().Update(reportM);
                _uow.Commit();

                response.Structure = request.Structure;

                _uow.CommitTransaction();
            }
            catch (Exception)
            {
                _uow.RollBackTransaction();
                throw;
            }

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        private string AddReportFunction(string dbname, string accid, string title)
        {
            var client = new AuthService.AuthServiceClient();
            var reqAddFunc = new BMDC.Models.Auth.AddFunctionM.Request()
            {
                DBName = dbname,
                AccID = accid,
                GroupCode = "G01",
                FuncName = title,
                FuncPath = "/",
                FuncStatus = true,
                AuthFlag1 = true,
                AuthFlag2 = true,
                AuthName1 = "檢視",
                AuthName2 = "編輯"
            };
            var rsp = client.AddFunction(_serviceInfo, reqAddFunc);
            client.Close();
            return rsp.FuncCode;
        }

        private void AddRootRoleFunction(string funccode)
        {
            RoleAuthMap rm = new RoleAuthMap()
            {
                RoleCode = "X1R002",
                SysCode = "SYS002",
                FuncCode = funccode,
                AuthNo1 = "Y",
                AuthNo2 = "Y",
                AuthNo3 = "N",
                AuthNo4 = "N",
                AuthNo5 = "N",
                CreateMan = 0
            };

            var reportRole = _uow.Get<IRoleAuthMapRepository>().Create(rm);
            _uow.Commit();
        }

        public RSPBase AddReportMain(AddReportMainM.AddReportMainReq request, ref AddReportMainM.AddReportMainRsp response, string dbname)
        {
            RSPBase rSPBase = new RSPBase();

            try
            {
                _uow.BeginTransaction();

                string funccode = "";
                var reportMain = _uow.Get<IX1_ReportMRepository>().GetLatestReportM(request.Structure.Category);

                if (request.IsNewVersion)
                {
                    funccode = reportMain.FuncCode;
                }
                else
                {
                    funccode = AddReportFunction(dbname, request.AccID, request.Structure.Title);
                    AddRootRoleFunction(funccode);
                    request.Structure.Category = Guid.NewGuid().ToString().Replace("-", "");
                }

                // 新增Report Main紀錄
                X1_Report_Main addReportData = new X1_Report_Main()
                {
                    Category = request.Structure.Category,
                    Title = request.Structure.Title,
                    Description = request.Structure.Description,
                    OutputJson = null,
                    CreateMan = request.AccID,
                    ModifyMan = request.AccID,
                    IsDelete = false,
                    ReserveDate = request.ReserveDate.Value,
                    IndexNum = request.Structure.IndexNum,
                    FuncCode = funccode
                };
                addReportData = _uow.Get<IX1_ReportMRepository>().Create(addReportData);
                _uow.Commit();

                // 將ID回填
                request.Structure.ID = addReportData.ID;

                // 取得所有問題型態
                var structure = request.Structure;

                ProcessQuestion(ref structure, true);

                request.Structure = structure;
                //更新Json
                addReportData.OutputJson = JsonConvert.SerializeObject(request.Structure);
                addReportData.ReserveDate = request.ReserveDate.Value;
                _uow.Get<IX1_ReportMRepository>().Update(addReportData);
                _uow.Commit();
                _uow.CommitTransaction();

                response.Structure = request.Structure;
                response.ID = addReportData.ID;
                response.Category = addReportData.Category;
            }
            catch (Exception e)
            {
                _uow.RollBackTransaction();
                throw;
            }



            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        private void ProcessQuestion(ref AddReportMainM.QuestionnaireStructure structure, bool isNewReport)
        {
            // 取得所有問題型態
            var dictType = _uow.Get<IX1_ReportQuestionTypeRepository>().GetAll().ToDictionary(x => x.Name);
            var questRepo = _uow.Get<IX1_ReportQuestionRepository>();
            var validationRepo = _uow.Get<IQuestionValidationRepository>();

            foreach (var group in structure.Children)
            {
                foreach (var question in group.Children)
                {
                    // 如果有其他選項，先從選項中移除，之後再新增至尾端
                    var otherAns = question.AnswerOption.FirstOrDefault(o => o.Value.Equals("other"));

                    if (otherAns != null)
                    {
                        question.AnswerOption.Remove(otherAns);
                    }

                    if (question.OtherAns)
                    {
                        if (isNewReport || !question.OtherAnsID.HasValue || question.OtherAnsID == 0)
                        {
                            X1_Report_Question addOtherAns = new X1_Report_Question()
                            {
                                QuestionType = dictType["otherans"].ID,
                                ReportID = structure.ID,
                                QuestionText = "其他",
                                Description = null,
                                Required = false,
                                CodingBookTitle = "",
                                CodingBookIndex = -1,
                                AnswerOption = "",
                            };
                            addOtherAns = questRepo.Create(addOtherAns);
                            _uow.Commit();
                            question.OtherAnsID = addOtherAns.ID;
                        }

                        question.AnswerOption.Add(new AddReportMainM.Answeroption()
                        {
                            CodingBookIndex = -1,
                            CodingBookTitle = "其他",
                            OptionText = "其他",
                            Value = "other",
                            HiddenFromBackend = true
                        });
                    }

                    // 題型是多選時，將每個選項轉換成獨立問題
                    var optionList = new List<X1_Report_Question>();
                    if (question.QuestionType.ToLower().Equals("checkbox"))
                    {
                        for (int option = 0; option < question.AnswerOption.Count; option++)
                        {
                            X1_Report_Question addOption = new X1_Report_Question();
                            if (isNewReport || question.AnswerOption[option].ID == 0)
                            {
                                addOption = new X1_Report_Question()
                                {
                                    QuestionType = dictType[question.QuestionType.ToLower()].ID,
                                    ReportID = structure.ID,
                                    QuestionText = question.AnswerOption[option].OptionText,
                                    Description = null,
                                    Required = false,
                                    CodingBookTitle = question.AnswerOption[option].CodingBookTitle,
                                    CodingBookIndex = -1,
                                    AnswerOption = "",
                                };
                                addOption = questRepo.Create(addOption);
                            }
                            else
                            {
                                addOption = new X1_Report_Question()
                                {
                                    ID = question.AnswerOption[option].ID,
                                    ReportID = structure.ID,
                                    QuestionType = dictType[question.QuestionType.ToLower()].ID,
                                    QuestionText = question.AnswerOption[option].OptionText,
                                    Description = null,
                                    Required = false,
                                    CodingBookTitle = question.AnswerOption[option].CodingBookTitle,
                                    CodingBookIndex = -1,
                                    AnswerOption = "",
                                };
                                questRepo.Update(addOption);
                            }

                            _uow.Commit();
                            optionList.Add(addOption);
                            question.AnswerOption[option].ID = addOption.ID;
                        }
                    }

                    //var validationList = question.ValidationList != null ?
                    //    question.ValidationList.Where(validation => !string.IsNullOrEmpty(validation.Value))
                    //    .Select(validation => new QuestionValidation()
                    //    {
                    //        Operator = validation.Operator,
                    //        Value = validation.Value
                    //    }).ToList() : new List<QuestionValidation>();

                    var jsonAns = JsonConvert.SerializeObject(question.AnswerOption);

                    X1_Report_Question addItemData = new X1_Report_Question();
                    if (isNewReport || question.ID == 0)
                    {
                        addItemData = new X1_Report_Question()
                        {
                            QuestionType = dictType[question.QuestionType.ToLower()].ID,
                            ReportID = structure.ID,
                            QuestionText = question.QuestionText,
                            Description = question.Description,
                            Required = question.Required,
                            CodingBookTitle = question.CodingBookTitle,
                            CodingBookIndex = question.CodingBookIndex,
                            QuestionNo = question.QuestionNo,
                            AnswerOption = jsonAns,
                            OtherAnsID = question.OtherAnsID,
                            Pin = question.Pin
                        };

                        addItemData = questRepo.Create(addItemData);
                    }
                    else
                    {
                        addItemData = questRepo.AllIncluding(q => q.Pinned_Question).FirstOrDefault(q => q.ID == question.ID);
                        addItemData.QuestionType = dictType[question.QuestionType.ToLower()].ID;
                        addItemData.QuestionText = question.QuestionText;
                        addItemData.Description = question.Description;
                        addItemData.Required = question.Required;
                        addItemData.CodingBookTitle = question.CodingBookTitle;
                        addItemData.CodingBookIndex = question.CodingBookIndex;
                        addItemData.QuestionNo = question.QuestionNo;
                        addItemData.AnswerOption = jsonAns;
                        addItemData.OtherAnsID = question.OtherAnsID;
                        addItemData.Pin = question.Pin;

                        questRepo.Update(addItemData);
                    }

                    _uow.Commit();

                    question.ID = addItemData.ID;

                    optionList.ForEach(option =>
                    {
                        option.ParentQuestID = addItemData.ID;
                        questRepo.Update(option);
                        _uow.Commit();
                    });

                    if (addItemData.Pinned_Question.Count > 0)
                    {
                        // 清除問題關注關聯
                        foreach (var pinnedQuest in addItemData.Pinned_Question.ToList())
                        {
                            addItemData.Pinned_Question.Remove(pinnedQuest);
                        }
                        _uow.Get<IX1_ReportQuestionRepository>().Update(addItemData);
                        _uow.Commit();
                    }

                    _uow.Get<IQuestionValidationRepository>().Delete(v => v.QuestionID == question.ID);
                    if (question.Pin)
                    {
                        Pinned_Question PQ;
                        if (question.PinnedID != null && question.PinnedID > 0)
                        {
                            PQ = _uow.Get<IPinnedQuestionRepository>().AllIncluding(pq => pq.X1_Report_Question).FirstOrDefault(q => q.PinnedID == question.PinnedID);
                        }
                        else
                        {
                            PQ = AddPinnedQuest(question.PinnedName);
                        }

                        question.PinnedID = PQ.PinnedID;
                        question.PinnedName = null;
                        AddRelation(PQ, addItemData);
                        AddValidation(addItemData.ID, question.ValidationGroupList);
                    }
                    else
                    {
                        question.PinnedID = null;
                        question.PinnedName = null;
                    }

                    if (question.AnswerOption.Count > 0)
                    {
                        ProcessAbnormalOption(addItemData.ID, question.AnswerOption, question.QuestionType.ToLower());
                    }
                }
            }
        }

        private Pinned_Question AddPinnedQuest(string name)
        {
            var pinnedQuestRepo = _uow.Get<IPinnedQuestionRepository>();
            //pinnedQuestRepo.Delete(q => q.QuestionID == questionID);

            var lastPinnedQuest = pinnedQuestRepo.GetAll().OrderByDescending(q => q.Index).FirstOrDefault();
            int maxIndex = 0;

            if (lastPinnedQuest != null) maxIndex = lastPinnedQuest.Index;

            var pinnedQuest = new Pinned_Question()
            {
                PinnedName = name,
                Visible = true,
                Index = maxIndex + 1
            };
            pinnedQuest = pinnedQuestRepo.Create(pinnedQuest);
            _uow.Commit();
            return pinnedQuest;
        }

        private void AddRelation(Pinned_Question PQ, X1_Report_Question XQ)
        {
            if (!PQ.X1_Report_Question.Any(q => q.ID == XQ.ID))
            {
                PQ.X1_Report_Question.Add(XQ);
                _uow.Get<IPinnedQuestionRepository>().Update(PQ);
                _uow.Commit();
            }
        }

        private void ProcessAbnormalOption(int questID, List<AddReportMainM.Answeroption> optionList, string questtype)
        {
            //重建所有異常值項目
            var validationRepo = _uow.Get<IQuestionValidationRepository>();
            string operatorStr = "Equals";
            //validationRepo.Delete(v => v.QuestionID == questID);

            if (questtype.Equals("checkbox"))
            {
                operatorStr = "Contains";
            }

            var abnormalOptionList = optionList.Where(o => o.AbnormalValue != null && o.AbnormalValue.Equals("true")).Select(o => new QuestionValidation()
            {
                Operator = operatorStr,
                QuestionID = questID,
                Value = o.Value,
                Color = o.AbnormalColor
            });
            validationRepo.Create(abnormalOptionList);
            _uow.Commit();
        }

        private void AddValidation(int questID, List<AddReportMainM.ValidationGroup> validationList)
        {
            if (validationList == null) validationList = new List<AddReportMainM.ValidationGroup>();

            var validationRepo = _uow.Get<IQuestionValidationRepository>();
            var conditionRepo = _uow.Get<IValidationConditionRepository>();

            validationRepo.Delete(validation => validation.QuestionID == questID);
            conditionRepo.Delete(c => c.QuestionID == questID);

            int groupNum = 0;
            var insertValidationList = new List<QuestionValidation>();
            var insertConditionList = new List<ValidationCondition>();
            validationList.ForEach(g =>
            {
                insertValidationList.AddRange(
                    g.ValidationList.Where(validation => !string.IsNullOrEmpty(validation.Value))
                        .Select(validation => new QuestionValidation()
                        {
                            QuestionID = questID,
                            Operator = validation.Operator,
                            Value = validation.Value,
                            Color = validation.Color,
                            Normal = validation.Normal,
                            GroupNum = groupNum
                        }).ToList()
                );
                insertConditionList.Add(new ValidationCondition()
                {
                    QuestionID = questID,
                    AttributeName = g.AttributeName,
                    GroupNum = groupNum,
                    Operator1 = g.Operator1,
                    Value1 = g.Value1,
                    Operator2 = g.Operator2,
                    Value2 = g.Value2
                });

                groupNum++;
            });

            validationRepo.Create(insertValidationList);
            conditionRepo.Create(insertConditionList);
            _uow.Commit();
        }

        public RSPBase DeleteReportMain(DeleteReportMainM.DeleteReportMainReq request, ref DeleteReportMainM.DeleteReportMainRsp response)
        {
            RSPBase rSPBase = new RSPBase();

            // 檢查 Report 是否存在
            var reportM = _uow.Get<IX1_ReportMRepository>().GetLatestReportM(id: request.ID);
            if (reportM == null)
            {
                rSPBase.ReturnCode = ErrorCode.NotFound;
                rSPBase.ReturnMsg = "無此Report Main ID: " + request.ID;
                return rSPBase;
            }

            // 刪除 Report
            reportM.IsDelete = true;
            reportM.ModifyMan = request.AccID;
            _uow.Get<IX1_ReportMRepository>().Update(reportM);
            _uow.Get<IRoleAuthMapRepository>().Delete(rm => rm.FuncCode == reportM.FuncCode);
            _uow.Get<IFunctionsRepository>().Delete(rf => rf.FuncCode == reportM.FuncCode);
            _uow.Commit();

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        /// <summary>
        /// 發佈 Report
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public RSPBase PublishReport(PublishReportM.PublishReportReq request, ref PublishReportM.PublishReportRsp response)
        {
            RSPBase rSPBase = new RSPBase();

            // 檢查 Report 是否存在
            var reportMRepo = _uow.Get<IX1_ReportMRepository>();
            var reportM = reportMRepo.GetLatestReportM(id: request.ID);
            if (reportM == null)
            {
                rSPBase.ReturnCode = ErrorCode.NotFound;
                rSPBase.ReturnMsg = "無此Report Main ID: " + request.ID;
                return rSPBase;
            }

            // 停止發佈其他 Report
            var allVersionReport = reportMRepo.GetAllVersionReportM(reportM.Category).Where(x => x.IsPublish).ToList();
            allVersionReport.ForEach(x =>
            {
                x.IsPublish = false;
                x.ModifyMan = request.AccID;
                reportMRepo.Update(x);
            });

            // 發佈 Report
            reportM.IsPublish = true;
            reportM.PublishDate = DateTime.Now;
            reportM.ModifyMan = request.AccID;
            reportMRepo.Update(reportM);

            _uow.Commit();

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        /// <summary>
        /// 取得所有版本的 Report
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public RSPBase GetAllVersionReport(GetAllVerionReportM.GetAllVersionReportReq request, ref GetAllVerionReportM.GetAllVersionReportRsp response)
        {
            RSPBase rSPBase = new RSPBase();

            var reportMs = _uow.Get<IX1_ReportMRepository>().GetAllVersionReportM(request.Category);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<X1_Report_Main, GetAllReportMainM.ReportMain>());
            var mapper = config.CreateMapper();
            response.Data = mapper.Map<GetAllReportMainM.ReportMain[]>(reportMs);

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        /// <summary>
        /// 上傳問題圖片
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public RSPBase AddReportMainFile(AddReportMainFileM.AddReportMainFileReq request, ref AddReportMainFileM.AddReportMainFileRsp response)
        {
            RSPBase rSPBase = new RSPBase();

            var quest = _uow.Get<IX1_ReportQuestionRepository>().GetQuest(request.RQID);
            if (quest == null)
            {
                rSPBase.ReturnCode = ErrorCode.NotFound;
                rSPBase.ReturnMsg = "找不到此RQID: " + request.RQID;
                return rSPBase;
            }

            // 刪除所有之前的檔案
            //var questFileRepo = _uow.Get<IX1_ReportQuestFileRepository>();
            //questFileRepo.DeleteAllByReportMID(quest.ReportID);

            if (!File.Exists(request.FilePath))
            {
                rSPBase.ReturnCode = ErrorCode.NotFound;
                rSPBase.ReturnMsg = "檔案不存在";
                return rSPBase;
            }

            var file = File.ReadAllBytes(request.FilePath);

            var reportFile = new X1_Report_Question_File()
            {
                FileName = request.FileName,
                MimeType = request.MimeType,
                RQID = request.RQID,
                FileData = file,
                RMID = quest.ReportID
            };
            _uow.Get<IX1_ReportQuestFileRepository>().Create(reportFile);
            _uow.Commit();

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public GetReportMainFileM.GetReportMainFileRsp GetReportMainFile(int RQID, string fileName)
        {
            var questFile = _uow.Get<IX1_ReportQuestFileRepository>().GetReportQuestFile(RQID, fileName);
            if (questFile == null)
            {
                return null;
            }

            return new GetReportMainFileM.GetReportMainFileRsp()
            {
                ID = questFile.ID,
                RMID = questFile.RMID,
                RQID = questFile.RQID,
                FileData = questFile.FileData,
                FileName = questFile.FileName,
                MimeType = questFile.MimeType
            };
        }

        /// <summary>
        /// 新增答案檔案
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public RSPBase AddReportAnsFile(AddReportAnsFileM.AddReportAnsFileReq request, ref AddReportAnsFileM.AddReportAnsFileRsp response)
        {
            RSPBase rSPBase = new RSPBase();

            // 驗證
            var ansM = _uow.Get<IX1_ReportAnswerMRepository>().GetReport(request.AnswerMID, null);
            if (ansM == null)
            {
                rSPBase.ReturnCode = ErrorCode.NotFound;
                rSPBase.ReturnMsg = "找不到此AnswerMID: " + request.AnswerMID;
                return rSPBase;
            }

            var quest = _uow.Get<IX1_ReportQuestionRepository>().GetQuest(request.QuestionID);
            if (quest == null)
            {
                rSPBase.ReturnCode = ErrorCode.NotFound;
                rSPBase.ReturnMsg = "找不到此QuestionID: " + request.QuestionID;
                return rSPBase;
            }

            if (!File.Exists(request.FilePath))
            {
                rSPBase.ReturnCode = ErrorCode.NotFound;
                rSPBase.ReturnMsg = "檔案不存在";
                return rSPBase;
            }

            string fileExt = Path.GetExtension(request.FileName);

            var questD = _uow.Get<IX1_ReportAnswerFileRepository>().GetAnsFile(questID: request.QuestionID, ansMId: request.AnswerMID);
            if (questD == null)
            {
                var filePath = _fileSvc.SaveFile(request.FilePath);
                var fileName = Path.GetFileName(filePath);
                var reportFile = new X1_Report_Answer_File()
                {
                    AnswerMID = request.AnswerMID,
                    QuestionID = request.QuestionID,
                    GUID = Guid.Parse(fileName),
                    FileName = request.FileName,
                    FileExt = fileExt,
                    FilePath = filePath,
                    MimeType = request.MimeType
                };
                _uow.Get<IX1_ReportAnswerFileRepository>().Create(reportFile);
            }
            else
            {
                var filePath = _fileSvc.SaveFile(request.FilePath, questD.GUID.ToString());
                questD.AnswerMID = request.AnswerMID;
                questD.FileName = request.FileName;
                questD.FileExt = fileExt;
                questD.FilePath = filePath;
                questD.MimeType = request.MimeType;
                _uow.Get<IX1_ReportAnswerFileRepository>().Update(questD);
            }

            _uow.Commit();

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public GetReportAnsFileM.GetReportAnsFileRsp GetReportAnsFile(int ID)
        {
            var ansFile = _uow.Get<IX1_ReportAnswerFileRepository>().GetAnsFile(ID);
            if (ansFile == null)
            {
                return null;
            }

            return new GetReportAnsFileM.GetReportAnsFileRsp()
            {
                ID = ansFile.ID,
                AnswerMID = ansFile.AnswerMID,
                QuestionID = ansFile.QuestionID,
                FilePath = ansFile.FilePath,
                FileName = ansFile.FileName,
                MimeType = ansFile.MimeType
            };
        }

        /// <summary>
        /// 匯出Coding Book
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public RSPBase ExportCodingBook(ExportCodingBookM.ExportCodingBookReq request, ref ExportCodingBookM.ExportCodingBookRsp response, string saveFolderPath)
        {
            RSPBase rSPBase = new RSPBase();

            // 檢查Report
            int id;
            int.TryParse(request.ReportMID, out id);
            bool isIdFlag = id > 0 ? true : false;
            var reportM = _uow.Get<IX1_ReportMRepository>().GetLatestReportM(category: !isIdFlag ? request.ReportMID : null, id: isIdFlag ? (int?)id : null, isPublish: request.IsPublish);
            if (reportM == null)
            {
                rSPBase.ReturnCode = ErrorCode.NotFound;
                rSPBase.ReturnMsg = "OK";
                return rSPBase;
            }

            // 取得所有 Report 題目
            var textQuests = _uow.Get<IX1_ReportQuestionRepository>().GetAllQuestions(reportM.ID);

            // 產生excel
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("CodingBook");

            // 標題列
            IRow row = sheet.CreateRow(0);
            ICell cell1 = row.CreateCell(0);
            cell1.SetCellValue("問題編號");
            ICell cell2 = row.CreateCell(1);
            cell2.SetCellValue("題目");
            ICell cell3 = row.CreateCell(2);
            cell3.SetCellValue("選項");

            int rowNum = 1;
            foreach (var quest in textQuests)
            {
                // 忽略不顯示題目
                if (quest.CodingBookIndex < 0) continue;

                string optionOutput = "";
                if (!string.IsNullOrEmpty(quest.AnswerOption))
                {
                    var options = JsonConvert.DeserializeObject<List<AddReportMainM.Answeroption>>(quest.AnswerOption);

                    foreach (var option in options)
                    {
                        optionOutput += string.Format("{0}: {1}\n", option.Value, option.OptionText);
                    }
                }

                IRow tempRow = sheet.CreateRow(rowNum);

                ICell tempCell1 = tempRow.CreateCell(0),
                    tempCell2 = tempRow.CreateCell(1),
                    tempCell3 = tempRow.CreateCell(2);

                tempCell1.SetCellValue(quest.QuestionNo);
                tempCell2.SetCellValue(quest.QuestionText);
                tempCell3.SetCellValue(optionOutput);

                rowNum++;
            }
            
            var responseFileName = string.Format("{0}_{1}_CodingBook.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"), reportM.Title);
            var saveFilePath = Path.Combine(saveFolderPath, responseFileName);
            FileStream file = File.Create(saveFilePath);
            workbook.Write(file);
            file.Close();
            
            response.FileName = responseFileName;
            response.FilePath = saveFilePath;

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase ExportData(ExportDataM.ExportDataReq request, ref ExportDataM.ExportDataRsp response, string savePath)
        {
            RSPBase rSPBase = new RSPBase();

            // 檢查Report
            var reportM = _uow.Get<IX1_ReportMRepository>().GetLatestReportM(id: request.ID);
            if (reportM == null)
            {
                rSPBase.ReturnCode = ErrorCode.NotFound;
                rSPBase.ReturnMsg = "OK";
                return rSPBase;
            }

            // 取得所有 Report 題目
            var reportAnsM = _uow.Get<IX1_ReportAnswerMRepository>().GetAllReport(reportM.ID);
            var reportQuests = _uow.Get<IX1_ReportQuestionRepository>().GetAllQuestions(reportM.ID);

            var allAns = (from ansM in reportAnsM
                          join u in _uow.Get<IX1_PatientInfoRepository>().GetAll() on ansM.PID equals u.ID
                          join quest in reportQuests on ansM.ReportID equals quest.ReportID
                          join anstmp in _uow.Get<IX1_ReportAnswerDRepository>().GetAll() on new { QuestID = quest.ID, AnsMID = ansM.ID } equals new { QuestID = anstmp.QuestionID, AnsMID = anstmp.AnswerMID } into ansgrp
                          from ans in ansgrp.DefaultIfEmpty()
                          select new
                          {
                              AnswerMID = ansM.ID,
                              u.PUName,
                              QuestionID = quest.ID,
                              quest.QuestionText,
                              quest.CodingBookIndex,
                              quest.CodingBookTitle,
                              quest.AnswerOption,
                              ans.Value
                          }).ToList();

            // 產生excel
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("CodingBook");

            // 標題列
            int baseCell = 2;
            IRow row = sheet.CreateRow(0);
            ICell cell1 = row.CreateCell(0);
            cell1.SetCellValue("編號");
            ICell cell2 = row.CreateCell(1);
            cell2.SetCellValue("使用者姓名");

            foreach (var quest in reportQuests)
            {
                if (quest.CodingBookIndex < 0) continue;

                ICell celltmp = row.CreateCell(baseCell + quest.CodingBookIndex);
                celltmp.SetCellValue(quest.CodingBookTitle);
            }

            // 填入資料
            var ansMIDIndex = new Dictionary<int, int>();
            int totalRow = 1;
            foreach (var ans in allAns)
            {
                int rowNum;
                // 忽略不顯示題目
                if (ans.CodingBookIndex < 0) continue;

                // 將目前答案Report對應的列儲存
                if (!ansMIDIndex.ContainsKey(ans.AnswerMID))
                {
                    var rowtmp = sheet.CreateRow(totalRow);
                    ansMIDIndex.Add(ans.AnswerMID, totalRow);
                    totalRow++;

                    var noCell = rowtmp.CreateCell(0);
                    var nameCell = rowtmp.CreateCell(1);

                    noCell.SetCellValue(ans.AnswerMID);
                    nameCell.SetCellValue(ans.PUName);
                }

                rowNum = ansMIDIndex[ans.AnswerMID];

                IRow tempRow = sheet.GetRow(rowNum);

                ICell tempCell = tempRow.CreateCell(baseCell + ans.CodingBookIndex);

                var ansOptions = JsonConvert.DeserializeObject<List<AddReportMainM.Answeroption>>(ans.AnswerOption);
                string value = "";

                // 將多選題答案串起
                foreach (var option in ansOptions)
                {
                    if (option.ID == 0) break;

                    var optionAns = allAns.FirstOrDefault(x => x.AnswerMID == ans.AnswerMID && x.QuestionID == option.ID);
                    if (optionAns != null) value += optionAns.Value + ",";
                }

                value = value == "" ? ans.Value : value.TrimEnd(',');
                tempCell.SetCellValue(value);
            }

            var saveFilePath = Path.Combine(savePath, Guid.NewGuid().ToString().Replace("-", ""));
            FileStream file = File.Create(saveFilePath);
            workbook.Write(file);
            file.Close();

            var responseFileName = string.Format("{0}_{1}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"), reportM.Title);
            response.FileName = responseFileName;
            response.FilePath = saveFilePath;

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase AddReportAuth(AddReportAuthM.AddReportAuthReq request, ref AddReportAuthM.AddReportAuthRsp response)
        {
            throw new NotImplementedException();
        }

        private static string ConvertIntToExcelColumn(int columnNum)
        {
            int firstIndex = columnNum / 26;
            string first = firstIndex == 0 ? "" : Convert.ToChar(65 + firstIndex - 1).ToString();
            return first + Convert.ToChar(65 + columnNum % 26).ToString();
        }

        private AddGeneralReportM.AddGeneralReportReq ProcessCellData(KeyValuePair<int, List<QuestColumnMap>> reportMapColumn, DataTable table, int row, int pid, string accid, DateTime? fillingDate, int? status)
        {
            var _answers = new List<UpdateReportM.ReportAnswerD>();

            foreach (var q in reportMapColumn.Value)
            {
                switch (q.Question.QuestionType)
                {
                    case 9:
                        var options = JsonConvert.DeserializeObject<List<AddReportMainM.Answeroption>>(q.Question.AnswerOption);
                        var values = table.Rows[row][q.ColumnNum].ToString().Split(',');

                        foreach (var value in values)
                        {
                            if (string.IsNullOrEmpty(value.Trim())) continue;
                            var formatValue = value.Replace(" ", "");

                            var matchOption = options.FirstOrDefault(o =>
                            {
                                var formatOptionVal = o.Value.Replace(" ", "");
                                var formatOptionText = o.OptionText.Replace(" ", "");
                                return formatOptionVal.Equals(formatValue) || formatOptionText.Equals(formatValue);
                            });

                            if (matchOption != null)
                            {
                                _answers.Add(new UpdateReportM.ReportAnswerD()
                                {
                                    QuestionID = "Q_" + matchOption.ID,
                                    Value = matchOption.Value
                                });
                            }
                            else if (q.Question.OtherAnsID != null && !_answers.Any(a => a.QuestionID.Equals("Q_" + q.Question.OtherAnsID)))
                            {
                                var otherAnsOption = options.FirstOrDefault(o => o.Value.Equals("other"));
                                _answers.Add(new UpdateReportM.ReportAnswerD()
                                {
                                    QuestionID = "Q_" + otherAnsOption.ID,
                                    Value = "true"
                                });
                                _answers.Add(new UpdateReportM.ReportAnswerD()
                                {
                                    QuestionID = "Q_" + q.Question.OtherAnsID,
                                    Value = value
                                });
                            }
                            else
                            {
                                _formatError.Add(string.Format(_errorTemplate, "無對應選項值: " + value, row + 1, ConvertIntToExcelColumn(q.ColumnNum)));
                            }
                        }
                        break;
                    case 7:
                    case 8:
                        var radioOptions = JsonConvert.DeserializeObject<List<AddReportMainM.Answeroption>>(q.Question.AnswerOption);
                        var radioValue = table.Rows[row][q.ColumnNum].ToString().Replace(" ", "");


                        var radioMatchOption = radioOptions.FirstOrDefault(o =>
                        {
                            var formatOptionVal = o.Value.Replace(" ", "");
                            var formatOptionText = o.OptionText.Replace(" ", "");
                            return formatOptionVal.Equals(radioValue) || formatOptionText.Equals(radioValue);
                        });

                        if (!string.IsNullOrEmpty(radioValue))
                        {
                            if (radioMatchOption != null)
                            {
                                _answers.Add(new UpdateReportM.ReportAnswerD()
                                {
                                    QuestionID = "Q_" + q.Question.ID,
                                    Value = radioMatchOption.Value
                                });
                            }
                            else
                            {
                                if (q.Question.OtherAnsID != null && !_answers.Any(a => a.QuestionID.Equals("Q_" + q.Question.OtherAnsID)))
                                {
                                    _answers.Add(new UpdateReportM.ReportAnswerD()
                                    {
                                        QuestionID = "Q_" + q.Question.ID,
                                        Value = "other"
                                    });
                                    _answers.Add(new UpdateReportM.ReportAnswerD()
                                    {
                                        QuestionID = "Q_" + q.Question.OtherAnsID,
                                        Value = radioValue
                                    });
                                }
                                else
                                {
                                    _formatError.Add(string.Format(_errorTemplate, "無對應選項值", row + 1, ConvertIntToExcelColumn(q.ColumnNum)));
                                }
                            }
                        }

                        break;
                    default:
                        var questionID = "Q_" + q.QuestId;
                        _answers.Add(new UpdateReportM.ReportAnswerD()
                        {
                            QuestionID = questionID,
                            Value = table.Rows[row][q.ColumnNum].ToString().Trim()
                        });
                        break;
                }
            }

            var addReportReq = new AddGeneralReportM.AddGeneralReportReq()
            {
                OID = -1,
                PID = pid,
                ID = reportMapColumn.Key,
                Answers = _answers,
                AccID = accid,
                FillingDate = fillingDate,
                Status = status
            };
            return addReportReq;
        }

        private void AddMultipleReport(List<AddGeneralReportM.AddGeneralReportReq> requestList, string accid)
        {
            if (requestList.Count == 0) return;

            var dateStr = DateTime.Now.ToString("yyyyMMdd");

            var reportAnsMRepo = _uow.Get<IX1_ReportAnswerMRepository>();
            var reportAnsDRepo = _uow.Get<IX1_ReportAnswerDRepository>();

            var ansMList = new List<X1_Report_Answer_Main>();
            var ansDList = new List<X1_Report_Answer_Detail>();

            var insertReportM = requestList.Select(r =>
            {
                return new X1_Report_Answer_Main()
                {
                    OID = r.OID,
                    PID = r.PID.Value,
                    ReportID = r.ID,
                    FillingDate = r.FillingDate ?? DateTime.Now,
                    Lock = false,
                    IsDelete = false,
                    Status = r.Status ?? 1
                };
            }).ToList();
            reportAnsMRepo.BulkCreate(insertReportM, accid);
            _uow.Commit();

            for (int i = 0; i < requestList.Count; i++)
            {
                ansDList.AddRange(requestList[i].Answers.Select(x => new X1_Report_Answer_Detail()
                {
                    AnswerMID = insertReportM.ElementAt(i).ID,
                    QuestionID = int.Parse(x.QuestionID.Replace(Constants.QUEST_PREFIX, "")),
                    Value = x.Value
                }));
            }

            reportAnsDRepo.BulkCreate(ansDList, accid);
            _uow.Commit();
        }

        public RSPBase ImportData(ImportDataM.ImportDataReq request, ref ImportDataM.ImportDataRsp response)
        {
            RSPBase rSPBase = new RSPBase();
            _formatError = new List<string>();
            Dictionary<string, int> IDs = new Dictionary<string, int>();

            using (var stream = File.Open(request.ExcelPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    try
                    {
                        var result = reader.AsDataSet();
                        var table = result.Tables[0];
                        var rowCount = table.Rows.Count;
                        var columnCount = table.Columns.Count;
                        var patientInfoDict = new Dictionary<PatientInfoKey, int>();
                        var columnReportMap = new Dictionary<int, List<QuestColumnMap>>();
                        Dictionary<DateTime, int> seqIdDict = new Dictionary<DateTime, int>();
                        var questIDRowIndex = request.QuestIDRowNum - 1;
                        var dataStartRowIndex = request.DataStartRowNum - 1;
                        var patientRepo = _uow.Get<IX1_PatientInfoRepository>();
                        var allPatients = patientRepo.GetAll().Select(p => p.IDNo).ToList();

                        bool insertFlag = true;

                        // 找出個案資料欄位
                        var keys = System.Enum.GetValues(typeof(PatientInfoKey)).Cast<PatientInfoKey>();
                        var headAry = table.Rows[questIDRowIndex].ItemArray
                            .Select((x, i) => new { value = x.ToString().Trim(), index = i })
                            .Where(x => x.value != "")
                            .ToList();
                        foreach (var key in keys)
                        {
                            var keyStr = key.GetDescriptionText().Split(',');
                            var item = headAry.FirstOrDefault(x => keyStr.Contains(x.value));
                            var itemCount = headAry.Where(x => x.value.Equals(keyStr)).Count();
                            if (item != null)
                            {
                                patientInfoDict.Add(key, item.index);
                            }

                            if (itemCount > 1)
                            {
                                _formatError.Add("欄位重複: " + keyStr);
                            }
                        }
                        // 檢查必要欄位
                        if (!patientInfoDict.ContainsKey(PatientInfoKey.IdNo))
                        {
                            _formatError.Add("沒有身分證字號欄位");
                            throw new CustomException();
                        }

                        // 取得所有題號對應Report ID
                        var reportQuestRepo = _uow.Get<IX1_ReportQuestionRepository>();
                        var reportQuest = (from r in _uow.Get<IX1_ReportMRepository>().GetAll()
                                           join q in reportQuestRepo.GetAll() on r.ID equals q.ReportID
                                           where r.IsPublish
                                           select q).ToList();
                        headAry.ForEach(x =>
                        {
                            string questNo = x.value.Replace(" ", "");
                            var patientInfoFlag = patientInfoDict.ContainsValue(x.index);
                            var questQuery = reportQuest.Where(q => q.QuestionNo != null && q.QuestionNo.Replace(" ", "").Equals(questNo));
                            var quest = questQuery.FirstOrDefault();
                            var questCount = questQuery.Count();
                            if (questCount > 1)
                            {
                                _formatError.Add(string.Format(_errorTemplate, $"問題編號({x.value})重複: " + questNo, questIDRowIndex + 1, ConvertIntToExcelColumn(x.index)));
                                insertFlag = false;
                            }
                            else if (quest == null && !patientInfoFlag)
                            {
                                _formatError.Add(string.Format(_errorTemplate, $"問題編號({x.value})不存在", questIDRowIndex + 1, ConvertIntToExcelColumn(x.index)));
                                insertFlag = false;
                            }
                            else if (quest != null)
                            {
                                columnReportMap.GetOrAdd(quest.ReportID).Add(new QuestColumnMap()
                                {
                                    QuestId = quest.ID,
                                    ColumnNum = x.index,
                                    Question = quest
                                });
                            }
                        });
                        _uow.BeginRootTransaction();

                        var requestList = new List<AddGeneralReportM.AddGeneralReportReq>();

                        for (int i = dataStartRowIndex; i < rowCount; i++)
                        {
                            string idNo = table.Rows[i][patientInfoDict[PatientInfoKey.IdNo]].ToString().Trim().ToUpper(),
                                country = "1", name = "", gender = "", birthStr = "", fillingDateStr = null;
                            string cellphone = "", contactphone = "", contactrelation = "", education = "", addrcode = "", hccode = "", addr = "", domicile = "";
                            int status = 1;
                            DateTime birth = new DateTime(), temp;
                            DateTime? fillingDate = DateTime.Now;
                            bool forceInsert = request.ForceInsert;

                            if (!allPatients.Contains(idNo))
                            {
                                var flag = false;

                                if (!patientInfoDict.ContainsKey(PatientInfoKey.PUCountry))
                                {
                                    _formatError.Add("病患不存在且沒有國籍欄位 行: " + (i + 1));
                                    flag = true;
                                }

                                if (!patientInfoDict.ContainsKey(PatientInfoKey.Name))
                                {
                                    _formatError.Add("病患不存在且沒有姓名欄位 行: " + (i + 1));
                                    flag = true;
                                }

                                if (!patientInfoDict.ContainsKey(PatientInfoKey.Gender))
                                {
                                    _formatError.Add("病患不存在且沒有性別欄位 行: " + (i + 1));
                                    flag = true;
                                }

                                if (!patientInfoDict.ContainsKey(PatientInfoKey.Birth))
                                {
                                    _formatError.Add("病患不存在且沒有生日欄位 行: " + (i + 1));
                                    flag = true;
                                }

                                if (flag) throw new CustomException();

                                country = table.Rows[i][patientInfoDict[PatientInfoKey.PUCountry]].ToString().Trim();
                                name = table.Rows[i][patientInfoDict[PatientInfoKey.Name]].ToString().Trim();
                                gender = table.Rows[i][patientInfoDict[PatientInfoKey.Gender]].ToString().Trim().ToLower();
                                birthStr = table.Rows[i][patientInfoDict[PatientInfoKey.Birth]].ToString().Trim();

                                //cellphone = table.Rows[i][patientInfoDict[PatientInfoKey.Cellphone]].ToString().Trim();
                                contactphone = table.Rows[i][patientInfoDict[PatientInfoKey.ContactPhone]].ToString().Trim();
                                contactrelation = table.Rows[i][patientInfoDict[PatientInfoKey.ContactRelation]].ToString().Trim();
                                education = table.Rows[i][patientInfoDict[PatientInfoKey.Education]].ToString().Trim();
                                addrcode = table.Rows[i][patientInfoDict[PatientInfoKey.AddrCode]].ToString().Trim();
                                hccode = table.Rows[i][patientInfoDict[PatientInfoKey.HCCode]].ToString().Trim();
                                addr = table.Rows[i][patientInfoDict[PatientInfoKey.Addr]].ToString().Trim();
                                domicile = table.Rows[i][patientInfoDict[PatientInfoKey.Domicile]].ToString().Trim();

                                if (string.IsNullOrEmpty(country))
                                {
                                    _formatError.Add(string.Format(_errorTemplate, "國籍為空", i + 1, ConvertIntToExcelColumn(patientInfoDict[PatientInfoKey.PUCountry])));
                                    insertFlag = false;
                                    forceInsert = false;
                                }

                                if (string.IsNullOrEmpty(name))
                                {
                                    _formatError.Add(string.Format(_errorTemplate, "姓名為空", i + 1, ConvertIntToExcelColumn(patientInfoDict[PatientInfoKey.Name])));
                                    insertFlag = false;
                                    forceInsert = false;
                                }

                                if (!(ROC.TryParse(birthStr, out birth) || DateTime.TryParse(birthStr, out birth)))
                                {
                                    _formatError.Add(string.Format(_errorTemplate, $"生日日期({birthStr})格式錯誤", i + 1, ConvertIntToExcelColumn(patientInfoDict[PatientInfoKey.Birth])));
                                    insertFlag = false;
                                    forceInsert = false;
                                }

                                if (!IDNoUtility.CheckIDNo(idNo))
                                {
                                    _formatError.Add(string.Format(_errorTemplate, $"身分證字號({idNo})錯誤", i + 1, ConvertIntToExcelColumn(patientInfoDict[PatientInfoKey.IdNo])));
                                    insertFlag = false;
                                    forceInsert = false;
                                }

                                var maleKeyword = new string[] { "男", "1", "male" };
                                var femaleKeyword = new string[] { "女", "2", "female" };
                                if (femaleKeyword.Any(k => gender.Contains(k.ToLower()) || gender.Equals("f")))
                                {
                                    gender = "F";
                                }
                                else if (maleKeyword.Any(k => gender.Contains(k.ToLower()) || gender.Equals("m")))
                                {
                                    gender = "M";
                                }
                                else
                                {
                                    _formatError.Add(string.Format(_errorTemplate, $"性別({gender})格式錯誤", i + 1, ConvertIntToExcelColumn(patientInfoDict[PatientInfoKey.Gender])));
                                    insertFlag = false;
                                    forceInsert = false;
                                }
                            }
                            else
                            {
                                var patientFlag = _uow.Get<IX1_PatientInfoRepository>().Get(x => x.IDNo.Equals(idNo));

                                country = patientFlag.PUCountry.ToString();
                                name = patientFlag.PUName;
                                gender = patientFlag.Gender;
                                birth = (DateTime)patientFlag.PUDOB;
                                cellphone = patientFlag.Cellphone;
                                contactphone = patientFlag.ContactPhone;
                                contactrelation = patientFlag.ContactRelation;
                                education = patientFlag.Education.ToString();
                                addrcode = patientFlag.AddrCode;
                                hccode = patientFlag.HCCode;
                                addr = patientFlag.Addr;
                                domicile = patientFlag.Domicile;
                            }

                            if (patientInfoDict.ContainsKey(PatientInfoKey.FillingDate))
                            {
                                fillingDateStr = table.Rows[i][patientInfoDict[PatientInfoKey.FillingDate]].ToString().Trim();
                                if (!DateTime.TryParse(fillingDateStr, out temp))
                                {
                                    _formatError.Add(string.Format(_errorTemplate, $"填寫日期({fillingDateStr})格式錯誤", i + 1, ConvertIntToExcelColumn(patientInfoDict[PatientInfoKey.FillingDate])));
                                    insertFlag = false;
                                    forceInsert = false;
                                }
                                else
                                {
                                    fillingDate = temp;
                                }
                            }

                            if (patientInfoDict.ContainsKey(PatientInfoKey.Status))
                            {
                                if (!int.TryParse(table.Rows[i][patientInfoDict[PatientInfoKey.Status]].ToString().Trim(), out status))
                                {
                                    _formatError.Add(string.Format(_errorTemplate, $"表單狀態({fillingDateStr})格式錯誤", i + 1, ConvertIntToExcelColumn(patientInfoDict[PatientInfoKey.Status])));
                                    insertFlag = false;
                                    forceInsert = false;
                                }
                            }

                            if (insertFlag || forceInsert)
                            {
                                var patient = new AddPatientInfoM.AddPatientInfoReq()
                                {
                                    IDNo = idNo,
                                    PUCountry = country,
                                    PUName = name,
                                    Gender = gender,
                                    PUDOB = birth,
                                    AccID = request.AccID,
                                    
                                    Phone = cellphone,
                                    Cellphone = cellphone,
                                    ContactPhone = contactphone,
                                    ContactRelation = contactrelation,
                                    Education = education,
                                    AddrCode = addrcode,
                                    HCCode = hccode,
                                    Addr = addr,
                                    Domicile = domicile
                                };
                                var patientRsp = new AddPatientInfoM.AddPatientInfoRsp();

                                if (!IDs.ContainsKey(patient.IDNo))
                                {
                                    
                                    var patientBaseRsp = _patientSvc.AddPatientInfo(patient, ref patientRsp);
                                    if (patientBaseRsp.ReturnCode != ErrorCode.OK &&
                                        patientBaseRsp.ReturnCode != ErrorCode.Exist)
                                    {
                                        _uow.RollBackRootTransaction();
                                        return patientBaseRsp;
                                    }
                                    
                                    IDs.Add(patient.IDNo, patientRsp.ID);
                                }
                                else
                                {
                                    foreach(var di in IDs)
                                    {
                                        if(patient.IDNo.Equals(di.Key))
                                        {
                                            patientRsp.ID = di.Value;
                                            break;
                                        }
                                    }
                                }

                                //var firstReport = columnReportMap.FirstOrDefault();
                                //string seqNum = "";
                                //if (seqIdDict.ContainsKey(fillingDate.Value.Date))
                                //{
                                //    seqIdDict[fillingDate.Value.Date] += 1;
                                //    seqNum = fillingDate.Value.ToString("yyyyMMdd") + seqIdDict[fillingDate.Value.Date].ToString().PadLeft(3, '0');
                                //}
                                //else
                                //{
                                //    var seq = _uow.Get<IX1_ReportAnswerMRepository>().GetSequenceNum(fillingDate);
                                //    seqNum = seq.Item2;
                                //    seqIdDict.AddIfNotExist(fillingDate.Value.Date, seq.Item1);
                                //}

                                foreach (var entry in columnReportMap)
                                {
                                    var addReportReq = ProcessCellData(entry, table, i, patientRsp.ID, request.AccID, fillingDate, status);
                                    //addReportReq.SequenceNum = seqNum;
                                    requestList.Add(addReportReq);
                                }
                            }
                        }

                        AddMultipleReport(requestList, request.AccID);

                        if (!insertFlag && !request.ForceInsert)
                        {
                            response.CanForceInsert = true;
                            _uow.RollBackRootTransaction();
                            response.FormatError = _formatError;
                            rSPBase.ReturnCode = ErrorCode.OperateError;
                            rSPBase.ReturnMsg = "資料處理發生錯誤";
                            return rSPBase;
                        }

                        _uow.CommitRootTransaction();
                    }
                    catch (CustomException e)
                    {
                        response.CanForceInsert = false;
                        _uow.RollBackRootTransaction();
                        response.FormatError = _formatError;
                        rSPBase.ReturnCode = ErrorCode.OperateError;
                        rSPBase.ReturnMsg = "資料處理發生錯誤";
                        return rSPBase;
                    }
                    catch (Exception e)
                    {
                        _uow.RollBackRootTransaction();
                        rSPBase.ReturnCode = ErrorCode.OperateError;
                        rSPBase.ReturnMsg = e.ToString();
                        return rSPBase;
                    }
                }
            }

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase GetReportTemplateList(GetReportTemplateListM.GetReportTemplateListReq request, ref GetReportTemplateListM.GetReportTemplateListRsp response)
        {
            RSPBase rSPBase = new RSPBase();

            var templateList = _uow.Get<IX1_ReportExportTemplateRepository>().GetTemplateList();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<X1_Report_Export_Template, GetReportTemplateListM.ExportTemplate>());
            var mapper = config.CreateMapper();
            response.TemplateList = mapper.Map<List<GetReportTemplateListM.ExportTemplate>>(templateList);

            rSPBase.ReturnCode = ErrorCode.OK;
            rSPBase.ReturnMsg = "OK";
            return rSPBase;
        }

        public RSPBase GetETemplateEQuestList(GetETemplateEQuestListM.Request request, ref GetETemplateEQuestListM.Response response)
        {
            var questList = _uow.Get<IETemplateEQuestRepository>().GetETemplateEQuestList(request.ExportTemplateID).ToList();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ExportTemplateExtraQuest, GetETemplateEQuestListM.TemplateExtraQuest>());
            var mapper = config.CreateMapper();
            response.ExtraQuestList = mapper.Map<List<GetETemplateEQuestListM.TemplateExtraQuest>>(questList);
            return ResponseHelper.Ok();
        }

        public RSPBase GetReportTemplate(GetReportTemplateM.Request request, ref GetReportTemplateM.Response response, string rootPath)
        {
            var template = _uow.Get<IX1_ReportExportTemplateRepository>().GetTemplate(request.ID);
            if (template == null)
            {
                return ResponseHelper.CreateResponse(ErrorCode.NotFound, "無此模板ID: " + request.ID);
            }

            var file = _uow.Get<ISystemFileRepository>().GetFile(template.SystemFileID);
            if (file == null)
            {
                return ResponseHelper.CreateResponse(ErrorCode.NotFound, "找不到模板對應檔案紀錄");
            }

            string srcFilePath = Path.Combine(rootPath, file.FilePath);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<X1_Report_Export_Template, GetReportTemplateM.ReportTemplate>());
            var mapper = config.CreateMapper();
            response.Data = mapper.Map<GetReportTemplateM.ReportTemplate>(template);
            response.Data.FileUrl = file.ID.ToString();

            return ResponseHelper.Ok();
        }

        private string GetCurrentQuestAns(List<AnsWithPatient> allAns, Quest currentQuest, int currentAnsMID)
        {
            var currentAns = allAns.FirstOrDefault(a => a.QuestID == currentQuest.ID && a.AnswerM.ID == currentAnsMID);
            //todo
            //if (currentAns == null)
            //{
            //    var ansM = allAns.Select(a => a.AnswerM.ID = currentAnsMID).ToList();
            //}
            if (currentAns == null && currentQuest.Question.QuestionType == 9)
            {
                var ans = "";
                var answerOption = currentQuest.Question.AnswerOption ?? "[]";
                //將多選題的選項拆解
                var options = JsonConvert.DeserializeObject<List<AnswerOption>>(answerOption);
                foreach (var option in options)
                {
                    //取對應選項的答案
                    var optionAns = allAns.FirstOrDefault(a => a.QuestID == option.ID && a.AnswerM.ID == currentAnsMID);

                    if (optionAns != null)
                    {
                        //如果目前選項是其他，也要將其他填寫的值串接上去
                        if (option.Value.Equals("other"))
                        {
                            var otherAns = allAns.FirstOrDefault(a => a.QuestID == currentQuest.Question.OtherAnsID && a.AnswerM.ID == currentAnsMID);
                            if (otherAns != null)
                            {
                                ans += otherAns.Answer + ",";
                            }
                        }
                        else
                        {
                            ans += optionAns.Answer + ",";
                        }
                    }
                }
                //填入答案
                return string.IsNullOrEmpty(ans) ? "Null" : ans.TrimEnd(',');
            }
            else if (currentAns != null)
            {
                string ans = currentAns.Answer;
                //題目若是單選或下拉檢查是否選擇"其他"選項
                switch (currentQuest.Question.QuestionType)
                {
                    //case 21:
                    //    if (currentQuest.CodingBookTitle)
                    //    {
                    //        //合併機構代碼跟機構名稱
                    //        ans = currentAns.Answer.Substring(11);
                    //        return ans;
                    //    }
                        //break;
                    case 7:
                    case 8:
                        if (currentAns.Answer.Equals("other"))
                        {
                            var otherAns = allAns.FirstOrDefault(a => a.QuestID == currentQuest.Question.OtherAnsID && a.AnswerM.ID == currentAns.AnswerM.ID);
                            if (otherAns != null)
                            {
                                ans = otherAns.Answer;
                            }
                        }
                        break;
                }
                return ans;
            }
            else
            {
                //無答案填Null
                return "Null";
            }
        }

        public RSPBase ExportCervixData(ExportCervixDataM.Request request, ref ExportCervixDataM.Response response, string rootPath,string WebDB)
        {


            var ecUpdate = _uow.Get<IX1_ReportAnswerMRepository>();
            List<CervixExport> cervixExports = new List<CervixExport>();
            List<CervixTable> cervixTables = DBUtils.GetCervixTable(_uow, WebDB);

            string Vix30 = "";
            int tempInt = 0;

            foreach (CervixTable ct in cervixTables)
            {
                if (ct.Status < 6)
                    continue;
                //改成搜尋收件日期
                if (request.StartDate != null && request.StartDate >= Convert.ToDateTime(ct.cervixQuestions.First(x => x.QuestionText.Contains("抹片收到日期")).Value).AddYears(1911))
                continue;
                if (request.EndDate != null && request.EndDate <= Convert.ToDateTime(ct.cervixQuestions.First(x => x.QuestionText.Contains("抹片收到日期")).Value).AddYears(1911))
               continue;
                //確診日
                if (request.DiagnosedstartDate != null&& ct.cervixQuestions.Any(x => x.QuestionText.Contains("確診日期")))
                {
                    DateTime diagnosedstartDate = Convert.ToDateTime(ct.cervixQuestions.First(x => x.QuestionText.Contains("確診日期")).Value).AddYears(1911);
                    if (request.DiagnosedstartDate>= diagnosedstartDate)
                        continue;
                }
                if (request.DiagnosedendDate != null && ct.cervixQuestions.Any(x => x.QuestionText.Contains("確診日期")))
                {
                    DateTime diagnosedendDate = Convert.ToDateTime(ct.cervixQuestions.First(x => x.QuestionText.Contains("確診日期")).Value).AddYears(1911);
                    if (request.DiagnosedendDate <= diagnosedendDate)
                        continue;
                }
                //if (request.StartDate != null && request.StartDate >= ct.FillingDate)
                //    continue;
                //if (request.EndDate != null && request.EndDate <= ct.FillingDate)
                //    continue;
                if (request.Status != 0 && request.Status + 5 != ct.Status)
                    continue;
                
                // 開始 mapping 表單
                CervixExport ce = new CervixExport()
                {
                    PTNAME = ct.cervixCase.PUName,
                    PTBIRTH = ct.cervixCase.PUDOB?.ToString("yyyyMMdd"),
                    PTID = ct.cervixCase.IDNo,
                    PTEDUCAT = ct.cervixCase.Education ?? 7,
                    ADDCODEA = ct.cervixCase.AddrCode,
                    ADDCODEB = ct.cervixCase.Domicile,
                    MEDIORG = ct.cervixCase.HCCode,
                    PTTEL = ct.cervixCase.Cellphone,
                    ADDR = ct.cervixCase.Addr,
                    NATIONALIT = ct.cervixCase.PUCountry.ToString()
                };

                foreach (CervixQuestion cq in ct.cervixQuestions)
                {
                    if (cq.QuestionNo == "Vix-30")
                    {
                        Vix30 += cq.Value;
                        continue;
                    }
                    switch (cq.QuestionNo)
                    {
                        case "Vix-1":
                            if (int.TryParse(cq.Value, out tempInt))
                                ce.PTSUPPER = tempInt;
                            break;
                        case "Vix-17":
                            ce.CHARTNO = cq.Value;
                            break;
                        case "Vix-18":
                            ce.PASDATE = ROC.CDate2WDate(cq.Value);
                            break;
                        case "Vix-19":
                                ce.PASCODE = cq.Value.Substring(0, 10);
                            break;
                        case "Vix-24":
                                ce.CHKCODE = cq.Value.Substring(0, 10);
                            break;
                        case "Vix-25":
                            ce.CHKREC = ROC.CDate2WDate(cq.Value);
                            break;
                        case "Vix-23":
                            ce.CHKNO = cq.Value;
                            break;
                        case "Vix-28":
                            if (int.TryParse(cq.Value, out tempInt))
                                ce.CHKQUL = tempInt;
                            break;
                        case "Vix-29":
                            ce.CHKDIF = cq.Value;
                            break;
                        case "Vix-29-1":
                            ce.CHKDIF2 = cq.Value;
                            break;
                        case "Vix-31-1":
                            ce.CHKDATA = cq.Value;
                            break;
                        case "醫檢師代碼": // 需特殊處理
                            var userData = _uow.Get<IUsersRepository>().Get(x => x.DoctorNo == cq.Value);
                            if (userData != null)
                            {
                                if (userData.Senior == 1)
                                    ce.JPATH = cq.Value;
                                else
                                    ce.SPATH = cq.Value;
                            }
                            break;
                        case "醫師代碼":
                            ce.PATH = cq.Value;
                            break;
                        case "確診日期":
                            ce.CHKSURED = ROC.CDate2WDate(cq.Value);
                            break;
                        case "Vix-23-1":
                            if (int.TryParse(cq.Value, out tempInt))
                                ce.CHKQTY = tempInt;
                            break;
                        case "Vix-13":
                            ce.ULTOMY = cq.Value;
                            break;
                        case "Vix-14":
                            ce.X_RAY = cq.Value;
                            break;
                        case "Vix-26":
                            ce.SPL_TYPE = cq.Value;
                            break;
                        case "Vix-27":
                            ce.CHK_WAY = cq.Value;
                            break;
                        case "抹片車或設站篩檢":
                            ce.CAR_STA = cq.Value;
                            break;
                        case "Vix-20-1":
                            ce.PRSN_TYPE = cq.Value;
                            break;
                        case "Vix-9":
                            ce.LASTTIME = cq.Value;
                            break;
                        case "Vix-15":
                            ce.VACCINE = cq.Value;
                            break;
                        case "Vix-15-1":
                            ce.VACCINE_YY = cq.Value;
                            break;
                        case "Vix-31":
                            ce.PRECHKDATA = cq.Value;
                            break;
                        case "Vix-21":
                            ce.PURPOSE2 = cq.Value;
                            break;
                        case "有無自覺症狀?":
                            ce.SYMPTOM = cq.Value;
                            break;
                        case "Vix-16":
                            ce.HPV_TEST = cq.Value;
                            break;
                        case "時程代碼":
                            ce.CARDNO = cq.Value;
                            break;
                        default:
                            break;
                    }
                }

                // 可能的感染 複選題
                ce.CHKINF = Vix30;
                ce.FUN_TYPE = "I";

                cervixExports.Add(ce);
                var ecData = ecUpdate.Get(x => x.ID == ct.ID);
                if (ecData != null && ecData.Status < 7)
                {
                    ecData.Status = 7;
                    ecUpdate.Update(ecData);
                    _uow.Commit();
                }
            }

            // 產生txt
            string outputDir = Path.Combine(rootPath, "Content/Temp");
            Directory.CreateDirectory(outputDir);
            string fileName = Guid.NewGuid().ToString().Replace("-", "") + ".txt";
            string outputPath = Path.Combine(outputDir, fileName);

            using (StreamWriter tw = new StreamWriter(outputPath, false, Encoding.GetEncoding("big5")))
            {
                foreach (CervixExport ce in cervixExports)
                {
                    tw.WriteLine(Encoding.GetEncoding("big5").GetString(ce.ExportToBytes()).ToCharArray());
                }
            }

            response.ExcelUrl = fileName;

            return ResponseHelper.Ok();
        }


        public RSPBase ExportExcel(ExportExcelM.Request request, ref ExportExcelM.Response response, string rootPath)
        {
            //取得篩選過後的病患
            var patients = _uow.Get<IX1_PatientInfoRepository>().GetAll();
            if (request.PatientIDs != null)
            {
                patients = patients.Where(p => request.PatientIDs.Contains(p.ID));
            }
            //病患依照姓名排序
            var patientList = patients.OrderBy(p => p.PUName).ToList();

            var allReport = _uow.Get<IX1_ReportMRepository>().GetAll().Where(r => !r.IsDelete);

            //取得所有問題排除已刪除及不顯示，依照報告設定的排序編號、CodingBookIndex排序
            var reportQuests = from r in allReport
                               join q in _uow.Get<IX1_ReportQuestionRepository>().GetAll() on r.ID equals q.ReportID
                               where q.CodingBookIndex != -1
                               orderby r.IndexNum, r.ID, q.CodingBookIndex
                               select new { Report = r, Quest = q };

            //依照要求篩選報告
            if (request.ReportMainIDs != null)
            {
                reportQuests = reportQuests.Where(rq => request.ReportMainIDs.Contains(rq.Report.ID));
                allReport = allReport.Where(rq => request.ReportMainIDs.Contains(rq.ID));
            }

            var questResult = reportQuests.Select(rq => new Quest { ReportID = rq.Report.ID, ID = rq.Quest.ID, CodingBookTitle = rq.Quest.QuestionNo, CodingBookIndex = rq.Quest.CodingBookIndex, Question = rq.Quest }).ToList();

            //取所有報告答案
            var reportAnsQuery = from q in (from r in _uow.Get<IX1_ReportMRepository>().GetAll()
                                            join q in _uow.Get<IX1_ReportQuestionRepository>().GetAll() on r.ID equals q.ReportID
                                            where !r.IsDelete
                                            orderby r.ID, q.CodingBookIndex
                                            select new { Report = r, Quest = q })
                                 join ansM in _uow.Get<IX1_ReportAnswerMRepository>().GetAll() on q.Report.ID equals ansM.ReportID
                                 join p in patients on ansM.PID equals p.ID
                                 join qa in _uow.Get<IX1_ReportAnswerDRepository>().GetAll() on new { ReportID = ansM.ID, q.Quest.ID } equals new { ReportID = qa.AnswerMID, ID = qa.QuestionID }
                                 where qa.Value != ""
                                 orderby p.PUName, ansM.ReportID, ansM.FillingDate descending
                                 select new AnsWithPatient
                                 {
                                     PUCountry=p.PUCountry,
                                     PID = p.ID,
                                     IDNo = p.IDNo,
                                     Name = p.PUName,
                                     Gender = p.Gender,
                                     Birth = p.PUDOB,
                                     ContactPhone=p.ContactPhone,
                                     ContactRelation=p.ContactRelation,
                                     AddrCode=p.AddrCode,
                                     Domicile=p.Domicile,
                                     Addr=p.Addr,
                                     Education=p.Education,
                                     QuestID = q.Quest.ID,
                                     Answer = qa.Value,
                                     FillingDate = ansM.FillingDate,
                                     Status=ansM.Status,
                                     Question = q.Quest,
                                     AnswerM = ansM
                                 };

            //依照日期篩選
            if (request.StartDate.HasValue)
            {
                reportAnsQuery = reportAnsQuery.Where(a => a.FillingDate >= request.StartDate);
            }

            if (request.EndDate.HasValue)
            {
                var compareDate = request.EndDate.Value.Date.AddDays(1);
                reportAnsQuery = reportAnsQuery.Where(a => a.FillingDate < compareDate);
            }

            var reportAnsList = reportAnsQuery.ToList();

            //病患固定欄位
            var patientInfoColTitle = new string[] { "國籍", "身分證字號", "姓名", "性別", "生日", "電話", "緊急聯絡人電話", "緊急聯絡人關係", "現居地區", "所屬衛生所醫療機構", "戶籍地區", "現居完整地址", "教育", "填寫日期", "表單狀態" };
            //Excel所有欄位名稱
            var colTitle = patientInfoColTitle.Concat(questResult.Select(x => x.CodingBookTitle)).ToArray();
            //保留欄位數
            int remainColCount = patientInfoColTitle.Length;
            //欄位總數
            int colCount = remainColCount + questResult.Count;

            var allReportList = allReport.ToList();

            var excelData = new Dictionary<int, List<string[]>>();
            //目前病患索引
            int currentPatientIndex = 0;

            var sheetData = new List<string[]>() { colTitle };

            while (currentPatientIndex < patientList.Count)
            {
                //取目前病患的所有答案
                var patientAnsList = reportAnsList.Where(a => a.PID == patientList[currentPatientIndex].ID).ToList();
                {
                    int ansMID = -1, reportID = -1;
                    //此列資料
                    var rowData = new string[colCount];
                    //國籍
                    rowData[0] = patientList[currentPatientIndex].PUCountry.ToString();
                    //身分證字號
                    rowData[1] = patientList[currentPatientIndex].IDNo;
                    //姓名
                    rowData[2] = patientList[currentPatientIndex].PUName;
                    //性別
                    rowData[3] = patientList[currentPatientIndex].Gender.Equals("M") ? "男" : "女";
                    //生日
                    rowData[4] = patientList[currentPatientIndex].PUDOB.Value.ToString("yyyy-MM-dd");
                    //電話
                    rowData[5] = patientList[currentPatientIndex].Cellphone;
                    //緊急聯絡人電話
                    rowData[6] = patientList[currentPatientIndex].ContactPhone;
                    //緊急聯絡人關係
                    rowData[7] = patientList[currentPatientIndex].ContactRelation;
                    //現居地區
                    rowData[8] = patientList[currentPatientIndex].AddrCode;
                    //所屬衛生所醫療機構
                    rowData[9] = " ";
                    //戶籍地區
                    rowData[10] = patientList[currentPatientIndex].Domicile;
                    //現居完整地址
                    rowData[11] = patientList[currentPatientIndex].Addr;
                    //教育
                    rowData[12] = patientList[currentPatientIndex].Education.ToString();
                    //填寫日期
                    rowData[13] = patientList.Max(a => a.FillingDate).ToString("yyyy-MM-dd");

                    //表單狀態 todo
                    //rowData[14] = patientList[currentPatientIndex].
                    //依照設定的問題順序填入答案
                    for (int i = 0; i < questResult.Count; i++)
                    {
                        //若目前問題跟前個問題屬於不同報告，查詢下個報告最新資料
                        if (questResult[i].ReportID != reportID)
                        {
                            //儲存目前報告ID
                            reportID = questResult[i].ReportID;

                            int j = i;
                            ansMID = -1;

                            var sameReportQuests = patientAnsList.Where(a => a.Question.ReportID == reportID).ToList();
                            var maxValQuest = ListUtils.FindMaxValueItem(sameReportQuests, a => a.FillingDate);
                            ansMID = maxValQuest != null ? maxValQuest.AnswerM.ID : -1;
                        }

                        //多選題要將所有選項的答案串接
                        rowData[remainColCount + i] = GetCurrentQuestAns(patientAnsList, questResult[i], ansMID);
                        // joe fix Null column
                        if (rowData[remainColCount + i] == null || rowData[remainColCount + i] == "Null")
                            rowData[remainColCount + i] = "";
                    }

                    if (!excelData.ContainsKey(0))
                    {
                        excelData.Add(0, new List<string[]>() { colTitle });
                    }

                    excelData[0].Add(rowData);

                    //取最新一筆資料不加入剩餘答案
                    break;
                }

                foreach (var report in allReportList)
                {
                    var filterByReport = patientAnsList.Where(pa => pa.AnswerM.ReportID == report.ID).ToList();
                    var requestQuestList = questResult.Where(q => q.ReportID == report.ID).ToList();
                    int reportColCount = remainColCount + requestQuestList.Count;
                    int temp = -1;

                    foreach (var reportAns in filterByReport)
                    {
                        if (temp == reportAns.AnswerM.ID) continue;
                        temp = reportAns.AnswerM.ID;
                        //此列資料
                        var rowData = new string[reportColCount];
                        //身分證字號
                        rowData[0] = patientList[currentPatientIndex].IDNo;
                        //姓名
                        rowData[1] = patientList[currentPatientIndex].PUName;
                        //性別
                        rowData[2] = patientList[currentPatientIndex].Gender.Equals("M") ? "男" : "女";
                        //生日
                        rowData[3] = patientList[currentPatientIndex].PUDOB.Value.ToString("yyyy-MM-dd");
                        //填寫日期
                        rowData[4] = reportAns.FillingDate.ToString("yyyy-MM-dd");

                        //依照設定的問題順序填入答案
                        for (int i = 0; i < requestQuestList.Count; i++)
                        {
                            //多選題要將所有選項的答案串接
                            rowData[remainColCount + i] = GetCurrentQuestAns(filterByReport, requestQuestList[i], reportAns.AnswerM.ID);
                            // joe fix Null column
                            if (rowData[remainColCount + i] == null || rowData[remainColCount + i] == "Null")
                                rowData[remainColCount + i] = "";
                        }

                        if (!excelData.ContainsKey(report.ID))
                        {
                            var reportColTitle = patientInfoColTitle.Concat(requestQuestList.Select(x => x.CodingBookTitle)).ToArray();
                            excelData.Add(report.ID, new List<string[]>() { reportColTitle });
                        }

                        excelData[report.ID].Add(rowData);
                    }
                }

                currentPatientIndex++;
            }

            // 產生Excel
            string outputDir = Path.Combine(rootPath, "Content/Temp");
            Directory.CreateDirectory(outputDir);
            string fileName = Guid.NewGuid().ToString().Replace("-", "") + ".xlsx";
            string outputPath = Path.Combine(outputDir, fileName);

            var workbook = new XSSFWorkbook();

            if (excelData.Count == 0)
            {
                workbook.CreateSheet("總覽");
            }

            foreach (var sheet in excelData)
            {
                var report = allReportList.FirstOrDefault(r => r.ID == sheet.Key);
                var reportTitle = report == null ? "總覽" : report.Title;
                ExcelUtils.GenerateSheet(ref workbook, reportTitle, sheet.Value);
            }

            FileStream stream = File.Create(outputPath);
            workbook.Write(stream);
            stream.Close();

            response.ExcelUrl = fileName;

            return ResponseHelper.Ok();
        }

        public RSPBase CheckQuestNo(CheckQuestNo.Request request, ref CheckQuestNo.Response response)
        {
            //只比對未刪除且已發佈的，不比對同個類別的報告
            var questList = (from r in _uow.Get<IX1_ReportMRepository>().GetAll()
                             join q in _uow.Get<IX1_ReportQuestionRepository>().GetAll() on r.ID equals q.ReportID
                             where r.IsPublish && !r.IsDelete && r.Category != request.Category && q.QuestionNo == request.QuestNo
                             select new { Quest = q, Report = r }).ToList();
            var quest = questList.FirstOrDefault();
            response.IsDuplicated = quest != null;
            response.Title = quest != null ? quest.Report.Title : null;
            return ResponseHelper.Ok();
        }

        public RSPBase GetPinQuestData(GetPinQuestM.Request request, ref GetPinQuestM.Response response)
        {
            var patientRepo = _uow.Get<IX1_PatientInfoRepository>();
            // 檢查個案是否存在
            var patient = patientRepo.Get(p => p.ID == request.PatientID);
            if (patient == null)
            {
                return ResponseHelper.CreateResponse(ErrorCode.NotFound, $"無此病患ID: {request.PatientID}");
            }

            // 取得個案關注項目清單
            var getPersonalReq = new GetPersonalPinnedQuestListM.Request()
            {
                PatientID = request.PatientID
            };
            var getPersonalRsp = new GetPersonalPinnedQuestListM.Response();
            GetPersonalPinnedQuestList(getPersonalReq, ref getPersonalRsp);

            // 取的關注項目跟問題的關聯
            var pinnedQuestList = _uow.Get<IPinnedQuestionRepository>().AllIncluding(q => q.X1_Report_Question).ToList();
            var questValidationList = _uow.Get<IQuestionValidationRepository>().GetAll().ToList();
            var validationConditionList = _uow.Get<IValidationConditionRepository>().GetAll().ToList();

            // 取得此個案填寫的表單答案、表單問題、表單問題標準值
            var answerList = (from ansM in _uow.Get<IX1_ReportAnswerMRepository>().GetAll()
                              join ansD in _uow.Get<IX1_ReportAnswerDRepository>().GetAll() on ansM.ID equals ansD.AnswerMID
                              join reportM in _uow.Get<IX1_ReportMRepository>().GetAll() on ansM.ReportID equals reportM.ID
                              where ansM.PID == request.PatientID && !ansM.IsDelete && !reportM.IsDelete
                              orderby ansM.FillingDate descending
                              select new { AnsM = ansM, AnsD = ansD }).ToList();
            var responseList = new List<GetPinQuestM.QuestionGroup>();
            foreach (var pinnedQuest in getPersonalRsp.Data)
            {
                // 關注項目不顯示跳下一個
                if (!pinnedQuest.Visible) continue;

                var questGroup = new GetPinQuestM.QuestionGroup()
                {
                    GroupTitle = pinnedQuest.PinnedName,
                    RecordList = new List<GetPinQuestM.Record>()
                };

                var pinnedQuest2 = pinnedQuestList.FirstOrDefault(pq => pq.PinnedID == pinnedQuest.PinnedID);
                int i = 0;
                foreach (var ans in answerList)
                {
                    if (i >= request.Amount) break;

                    foreach (var quest in pinnedQuest2.X1_Report_Question)
                    {
                        if (ans.AnsD.QuestionID == quest.ID)
                        {
                            var thisQuestValidation = questValidationList.Where(v => v.QuestionID == ans.AnsD.QuestionID).ToList();
                            var thisQuestValidationCondition = validationConditionList.Where(c => c.QuestionID == ans.AnsD.QuestionID).ToList();
                            string showColor = "";
                            bool isNormal = true;
                            var validationList = thisQuestValidation;
                            ValidationCondition validationCondition = null;

                            if (thisQuestValidationCondition != null && thisQuestValidation != null)
                            {
                                validationCondition = thisQuestValidationCondition.FirstOrDefault(vc => IsConditionMeet(patient, vc, ans.AnsM.FillingDate));
                            }

                            if (validationCondition != null)
                            {
                                validationList = validationList.Where(v => v.GroupNum == validationCondition.GroupNum).ToList();
                            }

                            var questionValidation = validationList.FirstOrDefault(validation => IsValueInRange(ans.AnsD.Value, validation.Operator, validation.Value));

                            isNormal = questionValidation != null ? false : true;
                            if (questionValidation != null)
                            {
                                showColor = questionValidation.Color;
                            }

                            questGroup.RecordList.Add(new GetPinQuestM.Record()
                            {
                                Date = ans.AnsM.FillingDate,
                                QuestionTitle = quest.QuestionText,
                                Value = ans.AnsD.Value,
                                Color = showColor,
                                Normal = isNormal
                            });
                            i++;
                        }
                    }


                }

                responseList.Add(questGroup);
            }

            response.Data = responseList;

            return ResponseHelper.Ok();
        }

        private bool IsConditionMeet(X1_PatientInfo patient, ValidationCondition condition, DateTime fillingDate)
        {
            switch (condition.AttributeName)
            {
                case "Gender":
                    return patient.Gender == condition.Value1;
                case "Age":
                    var birthDate = patient.PUDOB.Value;
                    int offset = fillingDate.Month > birthDate.Month ||
                        (fillingDate.Month == birthDate.Month && fillingDate.Day > birthDate.Day) ? -1 : 0;
                    int age = fillingDate.Year - birthDate.Year + offset;
                    bool result1 = IsValueInRange(age.ToString(), condition.Operator1, condition.Value1),
                        result2 = IsValueInRange(age.ToString(), condition.Operator2, condition.Value2);
                    return result1 && result2;
            }

            return false;
        }

        private bool IsValueInRange(string comparedValue, string operatorStr, string value)
        {
            double comparedValueInt,
                valueInt;
            double.TryParse(comparedValue, out comparedValueInt);
            double.TryParse(value, out valueInt);

            switch (operatorStr)
            {
                case ">":
                    return comparedValueInt > valueInt;
                case ">=":
                    return comparedValueInt >= valueInt;
                case "=":
                    return comparedValueInt == valueInt;
                case "<":
                    return comparedValueInt < valueInt;
                case "<=":
                    return comparedValueInt <= valueInt;
                case "Equals":
                    return comparedValue.Equals(value);
                case "Contains":
                    return comparedValue.Contains(value);
                default:
                    return true;
            }
        }

        public RSPBase UpdatePinnedQuest(UpdatePinnedQuestM.Request request, ref UpdatePinnedQuestM.Response response)
        {
            var pinnedQuestRepo = _uow.Get<IPinnedQuestionRepository>();
            var pinnedQuestList = request.PinnedQuestList.Select(q => new Pinned_Question()
            {
                PinnedID = q.PinnedID,
                PinnedName = q.PinnedName,
                Visible = q.Visible,
                Index = q.Index
            });

            pinnedQuestRepo.Update(pinnedQuestList);
            _uow.Commit();

            return ResponseHelper.Ok();
        }

        public RSPBase GetPinnedQuestList(GetPinnedQuestListM.Request request, ref GetPinnedQuestListM.Response response)
        {
            var pinnedQuestList = _uow.Get<IPinnedQuestionRepository>().GetAll()
                .OrderBy(q => q.Index)
                .Select(q => new GetPinnedQuestListM.PinnedQuest()
                {
                    PinnedID = q.PinnedID,
                    PinnedName = q.PinnedName,
                    Visible = q.Visible,
                    Index = q.Index
                }).ToList();
            response.PinnedQuestList = pinnedQuestList;

            return ResponseHelper.Ok();
        }

        public RSPBase UpdatePersonalPinnedQuest(UpdatePersonalPinnedQuestM.Request request, ref UpdatePersonalPinnedQuestM.Response response)
        {
            try
            {
                _uow.BeginTransaction();
                var personalPinQuestRepo = _uow.Get<IPersonalPinnedQuestRepository>();
                personalPinQuestRepo.Delete(q => q.PatientID == request.PatientID);
                _uow.Commit();
                var personalPinnedQuest = request.PinnedQuestList.Select(quest => new Personal_Pinned_Question()
                {
                    Index = quest.Index,
                    PatientID = request.PatientID,
                    PinnedID = quest.PinnedID,
                    Visible = quest.Visible
                }).ToList();
                personalPinQuestRepo.Create(personalPinnedQuest);
                _uow.Commit();
                _uow.CommitTransaction();
            }
            catch (Exception)
            {
                _uow.RollBackTransaction();
                throw;
            }

            return ResponseHelper.Ok();
        }

        public RSPBase GetPersonalPinnedQuestList(GetPersonalPinnedQuestListM.Request request, ref GetPersonalPinnedQuestListM.Response response)
        {
            var pinnedQuestList = _uow.Get<IPinnedQuestionRepository>().GetAll()
                    .OrderBy(q => q.Index).ToList();

            var patientPinnedQuestList = _uow.Get<IPersonalPinnedQuestRepository>().GetAll()
                .Where(q => q.PatientID == request.PatientID)
                .OrderBy(q => q.Index).ToList();

            List<GetPinnedQuestListM.PinnedQuest> res = new List<GetPinnedQuestListM.PinnedQuest>();

            foreach (var pinnedQuest in pinnedQuestList)
            {
                res.Add(new GetPinnedQuestListM.PinnedQuest()
                {
                    Index = pinnedQuest.Index,
                    PinnedID = pinnedQuest.PinnedID,
                    PinnedName = pinnedQuest.PinnedName,
                    Visible = pinnedQuest.Visible
                });
            }

            for (int i = 0; i < res.Count; i++)
            {
                foreach (var personalQuest in patientPinnedQuestList)
                {
                    if (personalQuest.PinnedID == res[i].PinnedID)
                    {
                        res[i].Index = personalQuest.Index;
                        res[i].Visible = personalQuest.Visible;
                    }
                }
            }

            response.Data = res;

            return ResponseHelper.Ok();
        }
        //TingYu
        //public static List<CervixTable> GetCervixTableByDapper()
        //{
        //    List<CervixTable> cervixTable = new List<CervixTable>();
        //var connectionFactory = new ConnectionFactory();

        //    var sql = @"select RAM.ID,RAM.ReportID,RAM.FillingDate,RAM.CreateDate,RAM.ModifyDate,RAM.[Status],
        //          Patient.ID as CaseID,Patient.PUCountry,Patient.PUName,Patient.PUDOB,Patient.IDNo,Patient.Cellphone,Patient.Education,Patient.AddrCode,Patient.Addr,Patient.HCCode,Patient.Addr,Patient.Domicile,
        //          CervixQ.ID as QId,CervixQ.QuestionNo,CervixQ.QuestionType,CervixQ.QuestionText,CervixQ.[Description],CervixQ.AnswerOption,
        //          Ans.ID as AId,Ans.[Value]
        //        from X1_Report_Answer_Main AS RAM 
        //        Left Join X1_PatientInfo As Patient on Patient.ID=RAM.PID
        //        Left Join X1_Report_Question As CervixQ on CervixQ.ReportID=RAM.ReportID
        //        Left Join X1_Report_Answer_Detail AS Ans on Ans.AnswerMID=RAM.ID and Ans.QuestionID=CervixQ.ID
        //        where RAM.ReportID=1
        //    ";
        //    var dy = connectionFactory.CreateConnection(sql);
        //    cervixTable = Slapper.AutoMapper.MapDynamic<CervixTable>(dy, false).ToList();

        //    return cervixTable;
        //}

        //public RSPBase GetAllSequenceNum(GetAllSequenceNumM.Request request, ref GetAllSequenceNumM.Response response)
        //{
        //    var reportAnsMList = _uow.Get<IX1_ReportAnswerMRepository>().GetAll().Where(r => !r.IsDelete);
        //    var result = reportAnsMList.OrderByDescending(r => r.SequenceNum)
        //                    .Select(r => r.SequenceNum).Distinct().ToList();
        //    response.SequenceNumList = result;
        //    return ResponseHelper.Ok();
        //}
        public RSPBase DeletePersonalPinnedQuest(DeletePersonalPinnedQuestM.Request request, ref DeletePersonalPinnedQuestM.Response response)
        {
            _uow.Get<IPersonalPinnedQuestRepository>().Delete(p => p.PatientID == request.PatientID);
            _uow.Commit();
            return ResponseHelper.Ok();
           
        }

    }
}
