using Microsoft.VisualStudio.TestTools.UnitTesting;
using X1APServer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using X1APServer.Repository.Interface;
using X1APServer.Repository.Utility.Interface;
using X1APServer.Service.Interface;
using X1APServer.Service.Model;
using Newtonsoft.Json;
using X1APServer.Repository;
using X1APServer.ServiceTests;
using System.IO;

namespace X1APServer.Service.Tests
{
    [TestClass()]
    public class ReportServiceTests
    {
        //private string tempDirPath;

        //[TestInitialize]
        //public void SetUp()
        //{
        //    string currentDir = Directory.GetCurrentDirectory();
        //    tempDirPath = Path.Combine(currentDir, "Temp");
        //    Directory.CreateDirectory(tempDirPath);
        //}

        //[TestCleanup]
        //public void CleanUp()
        //{
        //    Directory.Delete(tempDirPath, true);
        //}

        //[TestMethod()]
        //public void GetAllReportMainTest()
        //{
        //    // Arrange
        //    Mock<IX1_ReportMRepository> mockReportMRepo = new Mock<IX1_ReportMRepository>();
        //    DateTime now = DateTime.Now;
        //    List<X1_Report_Main> list = new List<X1_Report_Main>()
        //        {
        //            new X1_Report_Main()
        //            {
        //                ID = 1,
        //                Category = "A",
        //                Title = "TitleA",
        //                Description = "DescA",
        //                CreateDate = now,
        //                CreateMan = "system",
        //                ModifyDate = now,
        //                ModifyMan = "system",
        //                IsDelete = false
        //            },
        //            new X1_Report_Main()
        //            {
        //                ID = 2,
        //                Category = "B",
        //                Title = "TitleB",
        //                Description = "DescB",
        //                CreateDate = now,
        //                CreateMan = "system",
        //                ModifyDate = now,
        //                ModifyMan = "system",
        //                IsDelete = false
        //            }
        //        };
        //    mockReportMRepo.Setup(m => m.GetAllCategoryLatestReportM(null)).Returns(() =>
        //    {
        //        return list.AsQueryable();
        //    });

        //    Mock<IUnitOfWork> mockUnitOfWork;
        //    mockUnitOfWork = new Mock<IUnitOfWork>();
        //    mockUnitOfWork.Setup(m => m.Get<IX1_ReportMRepository>()).Returns(() => mockReportMRepo.Object);

        //    var service = new ReportService(mockUnitOfWork.Object);

        //    var request = new GetAllReportMainM.GetAllReportMainReq();
        //    var response = new GetAllReportMainM.GetAllReportMainRsp();

        //    var expectedResponse = new List<GetAllReportMainM.ReportMain>()
        //    {
        //            new GetAllReportMainM.ReportMain()
        //            {
        //                ID = 1,
        //                Category = "A",
        //                Title = "TitleA",
        //                Description = "DescA",
        //                CreateDate = now,
        //                CreateMan = "system",
        //                ModifyDate = now,
        //                ModifyMan = "system"
        //            },
        //            new GetAllReportMainM.ReportMain()
        //            {
        //                ID = 2,
        //                Category = "B",
        //                Title = "TitleB",
        //                Description = "DescB",
        //                CreateDate = now,
        //                CreateMan = "system",
        //                ModifyDate = now,
        //                ModifyMan = "system"
        //            }
        //    };

        //    var expected = new RSPBase()
        //    {
        //        ReturnCode = ErrorCode.OK,
        //        ReturnMsg = "OK"
        //    };

        //    // Act
        //    var baseResponse = service.GetAllReportMain(request, ref response);

        //    // Assert
        //    Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(baseResponse));

        //    int i = 0;
        //    foreach (var main in response.ReportMainList)
        //    {
        //        var compareItem = JsonConvert.SerializeObject(expectedResponse[i]);
        //        i++;
        //        Assert.AreEqual(compareItem, JsonConvert.SerializeObject(main));
        //    }
        //}

        ///// <summary>
        ///// 測試取得所有已發佈 Report
        ///// </summary>
        //[TestMethod()]
        //public void GetAllReportMainTest1()
        //{
        //    // Arrange
        //    Mock<IX1_ReportMRepository> mockReportMRepo = new Mock<IX1_ReportMRepository>();
        //    DateTime now = DateTime.Now;
        //    List<X1_Report_Main> list = new List<X1_Report_Main>()
        //        {
        //            new X1_Report_Main()
        //            {
        //                ID = 1,
        //                Category = "A",
        //                Title = "TitleA",
        //                Description = "DescA",
        //                CreateDate = now,
        //                CreateMan = "system",
        //                ModifyDate = now,
        //                ModifyMan = "system",
        //                IsDelete = false,
        //                IsPublish = true
        //            },
        //            new X1_Report_Main()
        //            {
        //                ID = 2,
        //                Category = "B",
        //                Title = "TitleB",
        //                Description = "DescB",
        //                CreateDate = now,
        //                CreateMan = "system",
        //                ModifyDate = now,
        //                ModifyMan = "system",
        //                IsDelete = false,
        //                IsPublish = true
        //            }
        //        };
        //    mockReportMRepo.Setup(m => m.GetAllCategoryLatestReportM(true)).Returns(() =>
        //    {
        //        return list.AsQueryable();
        //    });

        //    Mock<IUnitOfWork> mockUnitOfWork;
        //    mockUnitOfWork = new Mock<IUnitOfWork>();
        //    mockUnitOfWork.Setup(m => m.Get<IX1_ReportMRepository>()).Returns(() => mockReportMRepo.Object);

        //    var service = new ReportService(mockUnitOfWork.Object);

        //    var request = new GetAllReportMainM.GetAllReportMainReq()
        //    {
        //        isPublish = true
        //    };
        //    var response = new GetAllReportMainM.GetAllReportMainRsp();

        //    var expectedResponse = new List<GetAllReportMainM.ReportMain>()
        //    {
        //            new GetAllReportMainM.ReportMain()
        //            {
        //                ID = 1,
        //                Category = "A",
        //                Title = "TitleA",
        //                Description = "DescA",
        //                CreateDate = now,
        //                CreateMan = "system",
        //                ModifyDate = now,
        //                ModifyMan = "system"
        //            },
        //            new GetAllReportMainM.ReportMain()
        //            {
        //                ID = 2,
        //                Category = "B",
        //                Title = "TitleB",
        //                Description = "DescB",
        //                CreateDate = now,
        //                CreateMan = "system",
        //                ModifyDate = now,
        //                ModifyMan = "system"
        //            }
        //    };

        //    var expected = new RSPBase()
        //    {
        //        ReturnCode = ErrorCode.OK,
        //        ReturnMsg = "OK"
        //    };

        //    // Act
        //    var baseResponse = service.GetAllReportMain(request, ref response);

        //    // Assert
        //    Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(baseResponse));

        //    int i = 0;
        //    foreach (var main in response.ReportMainList)
        //    {
        //        var compareItem = JsonConvert.SerializeObject(expectedResponse[i]);
        //        i++;
        //        Assert.AreEqual(compareItem, JsonConvert.SerializeObject(main));
        //    }
        //}

        //[TestMethod()]
        //public void GetReportMainTest()
        //{
        //    // Arrange
        //    Mock<IX1_ReportMRepository> mockReportMRepo = new Mock<IX1_ReportMRepository>();
        //    DateTime now = DateTime.Now;
        //    X1_Report_Main reportMain = new X1_Report_Main()
        //    {
        //        ID = 10,
        //        Category = "A",
        //        Title = "TitleA",
        //        Description = "DescA",
        //        OutputJson = "{\"SomeQuest\": \"Some Value\"}",
        //        CreateDate = now,
        //        CreateMan = "system",
        //        ModifyDate = now,
        //        ModifyMan = "system",
        //        IsDelete = false,
        //        PublishDate = DateTime.Parse("2019-11-07"),
        //        IsPublish = true,
        //        ReserveDate = DateTime.Parse("2019-11-15")
        //    };
        //    mockReportMRepo.Setup(m => m.GetLatestReportM(null, 10, null)).Returns(() =>
        //    {
        //        return reportMain;
        //    });

        //    var mockReportAnsMRepo = new Mock<IX1_ReportAnswerMRepository>();
        //    mockReportAnsMRepo.Setup(m => m.GetReport(1, null)).Returns(new X1_Report_Answer_Main()
        //    {
        //        ReportID = 10
        //    });

        //    Mock<IUnitOfWork> mockUnitOfWork;
        //    mockUnitOfWork = new Mock<IUnitOfWork>();
        //    mockUnitOfWork.Setup(m => m.Get<IX1_ReportMRepository>()).Returns(() => mockReportMRepo.Object);
        //    mockUnitOfWork.Setup(m => m.Get<IX1_ReportAnswerMRepository>()).Returns(() => mockReportAnsMRepo.Object);

        //    var service = new ReportService(mockUnitOfWork.Object);

        //    var request = new GetReportMainM.GetReportMainReq()
        //    {
        //        ReportAnsMID = 1,
        //        ReportCategory = "A"
        //    };
        //    var response = new GetReportMainM.GetReportMainRsp();

        //    var expectedResponse = new GetReportMainM.GetReportMainRsp()
        //    {
        //        Data = new GetReportMainM.ReportMain()
        //        {
        //            ID = 10,
        //            Category = "A",
        //            Title = "TitleA",
        //            Description = "DescA",
        //            OutputJson = "{\"SomeQuest\": \"Some Value\"}",
        //            CreateDate = now,
        //            CreateMan = "system",
        //            ModifyDate = now,
        //            ModifyMan = "system",
        //            PublishDate = DateTime.Parse("2019-11-07"),
        //            isPublish = true,
        //            ReserveDate = DateTime.Parse("2019-11-15")
        //        }
        //    };

        //    var expected = new RSPBase()
        //    {
        //        ReturnCode = ErrorCode.OK,
        //        ReturnMsg = "OK"
        //    };

        //    // Act
        //    var baseResponse = service.GetReportMain(request, ref response);

        //    // Assert
        //    Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(baseResponse));
        //    Assert.AreEqual(JsonConvert.SerializeObject(expectedResponse), JsonConvert.SerializeObject(response));
        //}

        //[TestMethod()]
        //public void UpdateReportMainTest()
        //{
        //    // Arrange
        //    Mock<IX1_ReportMRepository> mockReportMRepo = new Mock<IX1_ReportMRepository>();
        //    var updatedReportMain = new X1_Report_Main();
        //    var createdReportQuest = new List<X1_Report_Question>();
        //    var deleteReportQuest = new object[2];
        //    DateTime now = DateTime.Now;
        //    X1_Report_Main reportMain = new X1_Report_Main()
        //    {
        //        ID = 1,
        //        Category = "A",
        //        Title = "TitleA",
        //        Description = "DescA",
        //        OutputJson = "{\"SomeQuest\": \"Some Value\"}",
        //        CreateDate = new DateTime(),
        //        CreateMan = "system",
        //        ModifyDate = now,
        //        ModifyMan = "system",
        //        IsDelete = false
        //    };
        //    mockReportMRepo.Setup(m => m.GetLatestReportM(null, 1, null)).Returns(reportMain);
        //    mockReportMRepo.Setup(m => m.Update(It.IsAny<X1_Report_Main>())).Callback((X1_Report_Main main) =>
        //    {
        //        updatedReportMain = main;
        //    });

        //    Mock<IX1_ReportQuestionRepository> mockQuestRepo = new Mock<IX1_ReportQuestionRepository>();
        //    mockQuestRepo.Setup(m => m.GetAllQuestions(1)).Returns(() =>
        //    {
        //        List<X1_Report_Question> list = new List<X1_Report_Question>()
        //        {
        //            new X1_Report_Question()
        //            {
        //                ID = 1
        //            },
        //            new X1_Report_Question()
        //            {
        //                ID = 2
        //            }
        //        };

        //        return list.AsEnumerable();
        //    });
        //    int id = 1;
        //    mockQuestRepo.Setup(m => m.Create(It.IsAny<X1_Report_Question>())).Callback((X1_Report_Question quest) =>
        //    {
        //        createdReportQuest.Add(quest);
        //    }).Returns((X1_Report_Question quest) =>
        //    {
        //        quest.ID = id;
        //        id++;
        //        return quest;
        //    });
        //    mockQuestRepo.Setup(m => m.Delete(It.IsAny<object[]>())).Callback((object[] objs) =>
        //    {
        //        deleteReportQuest = objs;
        //    });

        //    Mock<IX1_ReportQuestionTypeRepository> mockQuestTypeRepo = new Mock<IX1_ReportQuestionTypeRepository>();
        //    mockQuestTypeRepo.Setup(m => m.GetAll()).Returns(() =>
        //    {
        //        List<X1_Report_Question_Type> list = new List<X1_Report_Question_Type>()
        //        {
        //            new X1_Report_Question_Type()
        //            {
        //                ID = 1,
        //                Name = "radio"
        //            },
        //            new X1_Report_Question_Type()
        //            {
        //                ID = 2,
        //                Name = "checkbox"
        //            }
        //        };

        //        return list.AsQueryable();
        //    });

        //    Mock<IUnitOfWork> mockUnitOfWork;
        //    mockUnitOfWork = new Mock<IUnitOfWork>();
        //    mockUnitOfWork.Setup(m => m.Get<IX1_ReportMRepository>()).Returns(() => mockReportMRepo.Object);
        //    mockUnitOfWork.Setup(m => m.Get<IX1_ReportQuestionRepository>()).Returns(() => mockQuestRepo.Object);
        //    mockUnitOfWork.Setup(m => m.Get<IX1_ReportQuestionTypeRepository>()).Returns(() => mockQuestTypeRepo.Object);

        //    var service = new ReportService(mockUnitOfWork.Object);

        //    var request = new UpdateReportMainM.UpdateReportMainReq()
        //    {
        //        Structure = new UpdateReportMainM.QuestionnaireStructure()
        //        {
        //            ID = 1,
        //            Title = "Report Title",
        //            Description = "Report Description",
        //            Category = "Report Category",
        //            Children = new List<AddReportMainM.Group>()
        //            {
        //                new AddReportMainM.Group()
        //                {
        //                    ID = 1,
        //                    Title = "Group1 Title",
        //                    Type = "Group",
        //                    Children = new List<AddReportMainM.Item>()
        //                    {
        //                        new AddReportMainM.Item()
        //                        {
        //                            ID = 1,
        //                            QuestionText = "Quest1",
        //                            QuestionType = "radio",
        //                            Type = "Question",
        //                            AnswerOption = new List<AddReportMainM.Answeroption>()
        //                            {
        //                                new AddReportMainM.Answeroption()
        //                                {
        //                                    ID = 0,
        //                                    OptionText = "Option1",
        //                                    Value = "0"
        //                                },
        //                                new AddReportMainM.Answeroption()
        //                                {
        //                                    ID = 0,
        //                                    OptionText = "Option2",
        //                                    Value = "1"
        //                                }
        //                            }
        //                        },
        //                        new AddReportMainM.Item()
        //                        {
        //                            ID = 4,
        //                            QuestionText = "Quest2",
        //                            QuestionType = "checkbox",
        //                            Type = "Question",
        //                            AnswerOption = new List<AddReportMainM.Answeroption>()
        //                            {
        //                                new AddReportMainM.Answeroption()
        //                                {
        //                                    ID = 2,
        //                                    OptionText = "Option1",
        //                                    Value = "0"
        //                                },
        //                                new AddReportMainM.Answeroption()
        //                                {
        //                                    ID = 3,
        //                                    OptionText = "Option2",
        //                                    Value = "1"
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        },
        //        AccID = "system",
        //        ReserveDate = DateTime.Parse("2019-11-15")
        //    };
        //    var response = new UpdateReportMainM.UpdateReportMainRsp();

        //    var expectedResponse = new UpdateReportMainM.UpdateReportMainRsp()
        //    {
        //        Structure = request.Structure
        //    };

        //    var expectedUpdatedReport = new X1_Report_Main()
        //    {
        //        ID = reportMain.ID,
        //        Category = request.Structure.Category,
        //        Title = request.Structure.Title,
        //        Description = request.Structure.Description,
        //        OutputJson = JsonConvert.SerializeObject(request.Structure),
        //        CreateDate = new DateTime(),
        //        CreateMan = request.AccID,
        //        ModifyDate = now,
        //        ModifyMan = request.AccID,
        //        IsDelete = false,
        //        ReserveDate = DateTime.Parse("2019-11-15")
        //    };

        //    var quests = request.Structure.Children[0].Children;
        //    var expectedCreatedQuest = new List<X1_Report_Question>()
        //    {
        //        new X1_Report_Question()
        //        {
        //            ID = 1,
        //            ReportID = reportMain.ID,
        //            QuestionText = quests[0].QuestionText,
        //            QuestionType = 1,
        //            AnswerOption = JsonConvert.SerializeObject(quests[0].AnswerOption),
        //            CodingBookIndex = -1,
        //            CodingBookTitle = quests[0].QuestionText,
        //            Required = false
        //        },
        //        new X1_Report_Question()
        //        {
        //            ID = 2,
        //            ReportID = reportMain.ID,
        //            QuestionText = quests[1].AnswerOption[0].OptionText,
        //            QuestionType = 2,
        //            AnswerOption = "",
        //            CodingBookIndex = -1,
        //            CodingBookTitle = quests[1].AnswerOption[0].OptionText,
        //            Required = false
        //        },
        //        new X1_Report_Question()
        //        {
        //            ID = 3,
        //            ReportID = reportMain.ID,
        //            QuestionText = quests[1].AnswerOption[1].OptionText,
        //            QuestionType = 2,
        //            AnswerOption = "",
        //            CodingBookIndex = -1,
        //            CodingBookTitle = quests[1].AnswerOption[1].OptionText,
        //            Required = false
        //        },
        //        new X1_Report_Question()
        //        {
        //            ID = 4,
        //            ReportID = reportMain.ID,
        //            QuestionText = quests[1].QuestionText,
        //            QuestionType = 2,
        //            AnswerOption = JsonConvert.SerializeObject(quests[1].AnswerOption),
        //            CodingBookIndex = -1,
        //            CodingBookTitle = quests[1].QuestionText,
        //            Required = false
        //        },
        //    };

        //    var expectedDeleteQuest = new object[] { 1, 2 };

        //    var expected = new RSPBase()
        //    {
        //        ReturnCode = ErrorCode.OK,
        //        ReturnMsg = "OK"
        //    };

        //    // Act
        //    var baseResponse = service.UpdateReportMain(request, ref response);

        //    // Assert
        //    Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(baseResponse));
        //    Assert.AreEqual(JsonConvert.SerializeObject(expectedResponse), JsonConvert.SerializeObject(response));
        //    Assert.AreEqual(JsonConvert.SerializeObject(expectedUpdatedReport), JsonConvert.SerializeObject(updatedReportMain));
        //    Assert.AreEqual(JsonConvert.SerializeObject(expectedCreatedQuest), JsonConvert.SerializeObject(createdReportQuest));
        //    //Assert.AreEqual(JsonConvert.SerializeObject(expectedDeleteQuest), JsonConvert.SerializeObject(deleteReportQuest));
        //}

        ///// <summary>
        ///// 測試新增Report
        ///// </summary>
        //[TestMethod()]
        //public void AddReportMainTest()
        //{
        //    // Arrange
        //    Mock<IX1_ReportMRepository> mockReportMRepo = new Mock<IX1_ReportMRepository>();
        //    var createdReportMain = new X1_Report_Main();
        //    mockReportMRepo.Setup(m => m.Create(It.IsAny<X1_Report_Main>())).Callback((X1_Report_Main main) =>
        //        {
        //            createdReportMain = main;
        //        })
        //        .Returns((X1_Report_Main main) =>
        //        {
        //            main.ID = 1;
        //            return main;
        //        });
        //    mockReportMRepo.Setup(m => m.Update(It.IsAny<X1_Report_Main>()))
        //        .Callback((X1_Report_Main main) =>
        //        {
        //            createdReportMain = main;
        //        });

        //    Mock<IX1_ReportQuestionRepository> mockQuestRepo = new Mock<IX1_ReportQuestionRepository>();
        //    var createdQuest = new List<X1_Report_Question>();
        //    int questId = 1;
        //    mockQuestRepo.Setup(m => m.Create(It.IsAny<X1_Report_Question>())).Callback((X1_Report_Question quest) =>
        //        {
        //            createdQuest.Add(quest);
        //        })
        //        .Returns((X1_Report_Question quest) =>
        //        {
        //            quest.ID = questId;
        //            questId++;
        //            return quest;
        //        });

        //    Mock<IX1_ReportQuestionTypeRepository> mockQuestTypeRepo = new Mock<IX1_ReportQuestionTypeRepository>();
        //    mockQuestTypeRepo.Setup(m => m.GetAll())
        //        .Returns(() =>
        //        {
        //            var list = new List<X1_Report_Question_Type>()
        //            {
        //                new X1_Report_Question_Type()
        //                {
        //                    ID = 1,
        //                    Name = "radio"
        //                },
        //                new X1_Report_Question_Type()
        //                {
        //                    ID = 2,
        //                    Name = "checkbox"
        //                }
        //            };
        //            return list.AsQueryable();
        //        });
        //    //mockQuestTypeRepo.Setup(m => m.GetQuestType("radio"))
        //    //    .Returns(new X1_Report_Question_Type() { ID = 1, Name = "radio" });
        //    //mockQuestTypeRepo.Setup(m => m.GetQuestType("checkbox"))
        //    //    .Returns(new X1_Report_Question_Type() { ID = 2, Name = "checkbox" });

        //    Mock<IUnitOfWork> mockUnitOfWork;
        //    mockUnitOfWork = new Mock<IUnitOfWork>();
        //    mockUnitOfWork.Setup(m => m.Get<IX1_ReportMRepository>()).Returns(() => mockReportMRepo.Object);
        //    mockUnitOfWork.Setup(m => m.Get<IX1_ReportQuestionRepository>()).Returns(() => mockQuestRepo.Object);
        //    mockUnitOfWork.Setup(m => m.Get<IX1_ReportQuestionTypeRepository>()).Returns(() => mockQuestTypeRepo.Object);

        //    var service = new ReportService(mockUnitOfWork.Object);

        //    var request = new AddReportMainM.AddReportMainReq
        //    {
        //        Structure = new AddReportMainM.QuestionnaireStructure()
        //        {
        //            Title = "TitleA",
        //            Category = "CategoryA",
        //            Children = new List<AddReportMainM.Group>()
        //            {
        //                new AddReportMainM.Group()
        //                {
        //                    Title = "GroupTitleA",
        //                    Type = "Group",
        //                    Children = new List<AddReportMainM.Item>()
        //                    {
        //                        new AddReportMainM.Item()
        //                        {
        //                            QuestionText = "QuestTextA",
        //                            QuestionType = "radio",
        //                            Type = "Question",
        //                            AnswerOption = new List<AddReportMainM.Answeroption>()
        //                            {
        //                                new AddReportMainM.Answeroption()
        //                                {
        //                                    OptionText = "OptionA",
        //                                    Value = "0"
        //                                },
        //                                new AddReportMainM.Answeroption()
        //                                {
        //                                    OptionText = "OptionB",
        //                                    Value = "1"
        //                                },
        //                            }
        //                        },
        //                        new AddReportMainM.Item()
        //                        {
        //                            QuestionText = "QuestTextB",
        //                            QuestionType = "checkbox",
        //                            Type = "Question",
        //                            AnswerOption = new List<AddReportMainM.Answeroption>()
        //                            {
        //                                new AddReportMainM.Answeroption()
        //                                {
        //                                    OptionText = "OptionA",
        //                                    Value = "0"
        //                                },
        //                                new AddReportMainM.Answeroption()
        //                                {
        //                                    OptionText = "OptionB",
        //                                    Value = "1"
        //                                },
        //                            }
        //                        },
        //                    }
        //                }
        //            },
        //            Description = "DescA",
        //        },
        //        AccID = "User",
        //        ReserveDate = DateTime.Parse("2019-11-15")
        //    };
        //    var response = new AddReportMainM.AddReportMainRsp();

        //    var expectedResponse = new AddReportMainM.AddReportMainRsp()
        //    {
        //        Structure = request.Structure
        //    };

        //    string json = "{\"ID\":1,\"Title\":\"TitleA\",\"Category\":\"CategoryA\",\"Description\":\"DescA\",\"Children\":[{\"ID\":0,\"Type\":\"Group\",\"Title\":\"GroupTitleA\",\"Children\":[{\"ID\":1,\"Type\":\"Question\",\"QuestionType\":\"radio\",\"QuestionText\":\"QuestTextA\",\"AnswerOption\":[{\"ID\":0,\"OptionText\":\"OptionA\",\"Value\":\"0\"},{\"ID\":0,\"OptionText\":\"OptionB\",\"Value\":\"1\"}]},{\"ID\":4,\"Type\":\"Question\",\"QuestionType\":\"checkbox\",\"QuestionText\":\"QuestTextB\",\"AnswerOption\":[{\"ID\":2,\"OptionText\":\"OptionA\",\"Value\":\"0\"},{\"ID\":3,\"OptionText\":\"OptionB\",\"Value\":\"1\"}]}]}]}";
        //    var expectedCreatedReport = new X1_Report_Main()
        //    {
        //        ID = 1,
        //        Category = "CategoryA",
        //        Title = "TitleA",
        //        Description = "DescA",
        //        OutputJson = json,
        //        CreateMan = "User",
        //        ModifyMan = "User",
        //        IsDelete = false,
        //        ReserveDate = DateTime.Parse("2019-11-15")
        //    };

        //    var expectedCreatedQuest = new List<X1_Report_Question>()
        //    {
        //        new X1_Report_Question()
        //        {
        //            ID = 1,
        //            AnswerOption = "[{\"ID\":0,\"OptionText\":\"OptionA\",\"Value\":\"0\"},{\"ID\":0,\"OptionText\":\"OptionB\",\"Value\":\"1\"}]",
        //            CodingBookIndex = 1,
        //            CodingBookTitle = "QuestTextA",
        //            QuestionText = "QuestTextA",
        //            QuestionType = 1,
        //            ReportID = 1
        //        },
        //        new X1_Report_Question()
        //        {
        //            ID = 2,
        //            AnswerOption = "",
        //            CodingBookIndex = 2,
        //            CodingBookTitle = "OptionA",
        //            QuestionText = "OptionA",
        //            QuestionType = 2,
        //            ReportID = 1
        //        },
        //        new X1_Report_Question()
        //        {
        //            ID = 3,
        //            AnswerOption = "",
        //            CodingBookIndex = 3,
        //            CodingBookTitle = "OptionB",
        //            QuestionText = "OptionB",
        //            QuestionType = 2,
        //            ReportID = 1
        //        },
        //        new X1_Report_Question()
        //        {
        //            ID = 4,
        //            AnswerOption = "[{\"ID\":2,\"OptionText\":\"OptionA\",\"Value\":\"0\"},{\"ID\":3,\"OptionText\":\"OptionB\",\"Value\":\"1\"}]",
        //            CodingBookIndex = 4,
        //            CodingBookTitle = "QuestTextB",
        //            QuestionText = "QuestTextB",
        //            QuestionType = 2,
        //            ReportID = 1
        //        }
        //    };

        //    var expected = new RSPBase()
        //    {
        //        ReturnCode = ErrorCode.OK,
        //        ReturnMsg = "OK"
        //    };

        //    // Act
        //    var baseResponse = service.AddReportMain(request, ref response);

        //    // Assert
        //    Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(baseResponse));
        //    Assert.AreEqual(JsonConvert.SerializeObject(expectedResponse), JsonConvert.SerializeObject(response));
        //    Assert.AreEqual(JsonConvert.SerializeObject(expectedCreatedQuest), JsonConvert.SerializeObject(createdQuest));
        //    Assert.AreEqual(JsonConvert.SerializeObject(expectedCreatedReport), JsonConvert.SerializeObject(createdReportMain));
        //}

        ///// <summary>
        ///// 測試刪除 Report Main
        ///// </summary>
        //[TestMethod()]
        //public void DeleteReportMainTest()
        //{
        //    // Arrange
        //    var updatedReportM = new X1_Report_Main();
        //    Mock<IX1_ReportMRepository> mockReportMRepo = new Mock<IX1_ReportMRepository>();
        //    mockReportMRepo.Setup(x => x.GetLatestReportM(null, 1, null))
        //        .Returns(new X1_Report_Main()
        //        {
        //            ID = 1,
        //            Category = "A",
        //            IsDelete = false
        //        });
        //    mockReportMRepo.Setup(x => x.Update(It.IsAny<X1_Report_Main>()))
        //        .Callback((X1_Report_Main reportM) =>
        //         {
        //             updatedReportM = reportM;
        //         });

        //    Mock<IUnitOfWork> mockUOW = new Mock<IUnitOfWork>();
        //    mockUOW.Setup(x => x.Get<IX1_ReportMRepository>()).Returns(mockReportMRepo.Object);

        //    ReportService service = new ReportService(mockUOW.Object);

        //    var request = new DeleteReportMainM.DeleteReportMainReq()
        //    {
        //        AccID = "user1",
        //        ID = 1
        //    };
        //    var response = new DeleteReportMainM.DeleteReportMainRsp();

        //    var expectedBaseResponse = new RSPBase()
        //    {
        //        ReturnCode = ErrorCode.OK,
        //        ReturnMsg = "OK"
        //    };

        //    var expectedUpdatedReportM = new X1_Report_Main()
        //    {
        //        ID = 1,
        //        Category = "A",
        //        IsDelete = true,
        //        ModifyMan = "user1"
        //    };

        //    // Act
        //    var baseResponse = service.DeleteReportMain(request, ref response);

        //    // Assert
        //    Assert.AreEqual(JsonConvert.SerializeObject(expectedBaseResponse), JsonConvert.SerializeObject(baseResponse));
        //    Assert.IsTrue(CommonUtils.ReflectiveEquals(expectedUpdatedReportM, updatedReportM));
        //}

        ///// <summary>
        ///// 測試發佈報告
        ///// </summary>
        //[TestMethod()]
        //public void PublishReportTest()
        //{
        //    // Arrange
        //    var updatedReportM = new List<X1_Report_Main>();
        //    Mock<IX1_ReportMRepository> mockReportMRepo = new Mock<IX1_ReportMRepository>();
        //    mockReportMRepo.Setup(x => x.GetLatestReportM(null, 1, null))
        //        .Returns(new X1_Report_Main()
        //        {
        //            ID = 1,
        //            Category = "A",
        //            IsDelete = false,
        //            PublishDate = null,
        //            IsPublish = false
        //        });
        //    mockReportMRepo.Setup(x => x.GetAllVersionReportM("A"))
        //        .Returns((string category) =>
        //        {
        //            List<X1_Report_Main> reportMs = new List<X1_Report_Main>()
        //            {
        //                new X1_Report_Main()
        //                {
        //                    ID = 2,
        //                    Category = "A",
        //                    IsDelete = false,
        //                    PublishDate = DateTime.Parse("2019-11-08"),
        //                    IsPublish = true
        //                },
        //                new X1_Report_Main()
        //                {
        //                    ID = 3,
        //                    Category = "A",
        //                    IsDelete = false,
        //                    PublishDate = DateTime.Parse("2019-11-09"),
        //                    IsPublish = true
        //                }
        //            };
        //            return reportMs.AsQueryable();
        //        });
        //    mockReportMRepo.Setup(x => x.Update(It.IsAny<X1_Report_Main>()))
        //        .Callback((X1_Report_Main reportM) =>
        //        {
        //            updatedReportM.Add(reportM);
        //        });

        //    Mock<IUnitOfWork> mockUOW = new Mock<IUnitOfWork>();
        //    mockUOW.Setup(x => x.Get<IX1_ReportMRepository>()).Returns(mockReportMRepo.Object);

        //    ReportService service = new ReportService(mockUOW.Object);

        //    var request = new PublishReportM.PublishReportReq()
        //    {
        //        AccID = "user1",
        //        ID = 1
        //    };
        //    var response = new PublishReportM.PublishReportRsp();

        //    var expectedBaseResponse = new RSPBase()
        //    {
        //        ReturnCode = ErrorCode.OK,
        //        ReturnMsg = "OK"
        //    };

        //    var expectedUpdatedReportM = new List<X1_Report_Main>()
        //    {
        //        new X1_Report_Main()
        //        {
        //            ID = 2,
        //            Category = "A",
        //            IsDelete = false,
        //            PublishDate = DateTime.Parse("2019-11-08"),
        //            IsPublish = false
        //        },
        //        new X1_Report_Main()
        //        {
        //            ID = 3,
        //            Category = "A",
        //            IsDelete = false,
        //            PublishDate = DateTime.Parse("2019-11-09"),
        //            IsPublish = false
        //        },
        //        new X1_Report_Main()
        //        {
        //            ID = 1,
        //            Category = "A",
        //            IsPublish = true,
        //            ModifyMan = "user1"
        //        }
        //    };

        //    // Act
        //    var baseResponse = service.PublishReport(request, ref response);

        //    // Assert
        //    Assert.AreEqual(JsonConvert.SerializeObject(expectedBaseResponse), JsonConvert.SerializeObject(baseResponse));
        //    for (int i = 0; i < expectedUpdatedReportM.Count; i++)
        //    {
        //        Assert.IsTrue(CommonUtils.ReflectiveEquals(expectedUpdatedReportM[i], updatedReportM[i]));
        //    }

        //    Assert.IsNotNull(updatedReportM[0].PublishDate);
        //}

        //[TestMethod()]
        //public void GetAllVersionReportTest()
        //{
        //    // Arrange
        //    Mock<IX1_ReportMRepository> mockReportMRepo = new Mock<IX1_ReportMRepository>();
        //    mockReportMRepo.Setup(x => x.GetAllVersionReportM("A")).Returns((string category) =>
        //    {
        //        List<X1_Report_Main> reportMs = new List<X1_Report_Main>()
        //        {
        //            new X1_Report_Main()
        //            {
        //                Title = "title1",
        //                Category = "A",
        //                Description = "description"
        //            }
        //        };
        //        return reportMs.AsQueryable();
        //    });

        //    Mock<IUnitOfWork> mockUOW = new Mock<IUnitOfWork>();
        //    mockUOW.Setup(x => x.Get<IX1_ReportMRepository>()).Returns(mockReportMRepo.Object);

        //    var request = new GetAllVerionReportM.GetAllVersionReportReq()
        //    {
        //        Category = "A"
        //    };

        //    var response = new GetAllVerionReportM.GetAllVersionReportRsp();

        //    var expected = new GetAllVerionReportM.GetAllVersionReportRsp()
        //    {
        //        Data = new GetAllReportMainM.ReportMain[]
        //        {
        //            new GetAllReportMainM.ReportMain()
        //            {
        //                Title = "title1",
        //                Category = "A",
        //                Description = "description"
        //            }
        //        }
        //    };

        //    var expectedBaseRsp = new RSPBase()
        //    {
        //        ReturnCode = ErrorCode.OK,
        //        ReturnMsg = "OK"
        //    };

        //    var service = new ReportService(mockUOW.Object);

        //    // Act
        //    var baseRsp = service.GetAllVersionReport(request, ref response);

        //    // Assert
        //    Assert.AreEqual(JsonConvert.SerializeObject(expectedBaseRsp), JsonConvert.SerializeObject(baseRsp));
        //    Assert.IsTrue(CommonUtils.ReflectiveEquals(expected, response));
        //}

        //[TestMethod()]
        //public void AddReportMainFileTest()
        //{
        //    // Arrange
        //    string tempFilePath = Path.Combine(tempDirPath, "TempFile");
        //    var stream = File.CreateText(tempFilePath);
        //    stream.WriteLine("some test message");
        //    stream.Close();

        //    X1_Report_Question_File insertFile = null;
        //    Mock<IX1_ReportQuestFileRepository> questFileRepo = new Mock<IX1_ReportQuestFileRepository>();
        //    questFileRepo.Setup(x => x.Create(It.IsAny<X1_Report_Question_File>()))
        //        .Callback((X1_Report_Question_File questFile) =>
        //        {
        //            insertFile = questFile;
        //        });
        //    //questFileRepo.Setup(x => x.DeleteAllByReportMID(10));

        //    Mock<IX1_ReportQuestionRepository> questRepo = new Mock<IX1_ReportQuestionRepository>();
        //    questRepo.Setup(x => x.GetQuest(1)).Returns(new X1_Report_Question() { ReportID = 10 });

        //    Mock<IUnitOfWork> mockUOW = new Mock<IUnitOfWork>();
        //    mockUOW.Setup(x => x.Get<IX1_ReportQuestFileRepository>()).Returns(questFileRepo.Object);
        //    mockUOW.Setup(x => x.Get<IX1_ReportQuestionRepository>()).Returns(questRepo.Object);

        //    var request = new AddReportMainFileM.AddReportMainFileReq()
        //    {
        //        FileName = "testFileName",
        //        FilePath = tempFilePath,
        //        MimeType = "text/plain",
        //        RQID = 1,
        //        RMID = 10
        //    };

        //    var response = new AddReportMainFileM.AddReportMainFileRsp();

        //    var expected = new AddReportMainFileM.AddReportMainFileRsp();

        //    var expectedInsertFile = new X1_Report_Question_File()
        //    {
        //        FileName = request.FileName,
        //        FileData = File.ReadAllBytes(request.FilePath),
        //        MimeType = request.MimeType,
        //        RQID = request.RQID,
        //        RMID = request.RMID
        //    };

        //    var expectedBaseRsp = new RSPBase()
        //    {
        //        ReturnCode = ErrorCode.OK,
        //        ReturnMsg = "OK"
        //    };

        //    var service = new ReportService(mockUOW.Object);

        //    // Act
        //    var baseRsp = service.AddReportMainFile(request, ref response);

        //    // Assert
        //    Assert.AreEqual(JsonConvert.SerializeObject(expectedBaseRsp), JsonConvert.SerializeObject(baseRsp));
        //    Assert.AreEqual(JsonConvert.SerializeObject(expectedInsertFile), JsonConvert.SerializeObject(insertFile));
        //    questFileRepo.Verify(x => x.DeleteAllByReportMID(10), Times.AtLeastOnce);
        //}

        //[TestMethod()]
        //public void AddReportAnsFileTest()
        //{
        //    // Arrange
        //    string tempFilePath = Path.Combine(tempDirPath, "TempFile");
        //    var stream = File.CreateText(tempFilePath);
        //    stream.WriteLine("some test message");
        //    stream.Close();

        //    X1_Report_Answer_File insertFile = null;
        //    var questFileRepo = new Mock<IX1_ReportAnswerFileRepository>();
        //    questFileRepo.Setup(x => x.Create(It.IsAny<X1_Report_Answer_File>()))
        //        .Callback((X1_Report_Answer_File ansFile) =>
        //        {
        //            insertFile = ansFile;
        //        });
        //    //questFileRepo.Setup(x => x.DeleteAllByReportMID(10));

        //    var ansMRepo = new Mock<IX1_ReportAnswerMRepository>();
        //    ansMRepo.Setup(x => x.GetReport(1, null)).Returns(new X1_Report_Answer_Main() { });

        //    var questRepo = new Mock<IX1_ReportQuestionRepository>();
        //    questRepo.Setup(x => x.GetQuest(2)).Returns(new X1_Report_Question() { });

        //    Mock<IUnitOfWork> mockUOW = new Mock<IUnitOfWork>();
        //    mockUOW.Setup(x => x.Get<IX1_ReportAnswerFileRepository>()).Returns(questFileRepo.Object);
        //    mockUOW.Setup(x => x.Get<IX1_ReportAnswerMRepository>()).Returns(ansMRepo.Object);
        //    mockUOW.Setup(x => x.Get<IX1_ReportQuestionRepository>()).Returns(questRepo.Object);

        //    var request = new AddReportAnsFileM.AddReportAnsFileReq()
        //    {
        //        FileName = "testFileName",
        //        FilePath = tempFilePath,
        //        MimeType = "text/plain",
        //        AnswerMID = 1,
        //        QuestionID = 2
        //    };

        //    var response = new AddReportAnsFileM.AddReportAnsFileRsp();

        //    var expected = new AddReportAnsFileM.AddReportAnsFileRsp();

        //    var expectedInsertFile = new X1_Report_Answer_File()
        //    {
        //        FileName = request.FileName,
        //        FileData = File.ReadAllBytes(request.FilePath),
        //        MimeType = request.MimeType,
        //        AnswerMID = request.AnswerMID,
        //        QuestionID = request.QuestionID
        //    };

        //    var expectedBaseRsp = new RSPBase()
        //    {
        //        ReturnCode = ErrorCode.OK,
        //        ReturnMsg = "OK"
        //    };

        //    var service = new ReportService(mockUOW.Object);

        //    // Act
        //    var baseRsp = service.AddReportAnsFile(request, ref response);

        //    // Assert
        //    Assert.AreEqual(JsonConvert.SerializeObject(expectedBaseRsp), JsonConvert.SerializeObject(baseRsp));
        //    Assert.AreEqual(JsonConvert.SerializeObject(expectedInsertFile), JsonConvert.SerializeObject(insertFile));
        //}

        //[TestMethod()]
        //public void ExportCodingBookTest()
        //{
        //    #region Arrange
        //    var mockReportMRepo = new Mock<IX1_ReportMRepository>();
        //    mockReportMRepo.Setup(x => x.GetLatestReportM(null, 1, null)).Returns(new X1_Report_Main
        //    {
        //        ID = 1,
        //        Title = "Smear"
        //    });

        //    var mockReportQRepo = new Mock<IX1_ReportQuestionRepository>();
        //    mockReportQRepo.Setup(x => x.GetAllQuestions(1)).Returns((int reportMID) =>
        //    {
        //        var list = new List<X1_Report_Question>()
        //        {
        //            new X1_Report_Question()
        //            {
        //                QuestionText = "Quest1",
        //                AnswerOption = "[{\"OptionText\":\"Option1\",\"Value\":\"0\"},{\"OptionText\":\"Option2\",\"Value\":\"1\"},{\"OptionText\":\"Option3\",\"Value\":\"2\"}]"
        //            },
        //            new X1_Report_Question()
        //            {
        //                QuestionText = "Quest2"
        //            }
        //        };

        //        return list.AsEnumerable();
        //    });

        //    var mockUOWRepo = new Mock<IUnitOfWork>();
        //    mockUOWRepo.Setup(x => x.Get<IX1_ReportMRepository>()).Returns(mockReportMRepo.Object);
        //    mockUOWRepo.Setup(x => x.Get<IX1_ReportQuestionRepository>()).Returns(mockReportQRepo.Object);

        //    var request = new ExportCodingBookM.ExportCodingBookReq()
        //    {
        //        ReportMID = 1
        //    };

        //    var response = new ExportCodingBookM.ExportCodingBookRsp();

        //    var expectedResponse = new ExportCodingBookM.ExportCodingBookRsp()
        //    {
        //        FileName = "[0-9]{14}_Smear_CodingBook.xlsx",
        //        FilePath = Path.Combine(tempDirPath, "[0-9a-f]{32}").Replace("\\", "\\\\")
        //    };

        //    var expectedBaseResponse = new RSPBase()
        //    {
        //        ReturnCode = ErrorCode.OK,
        //        ReturnMsg = "OK"
        //    };

        //    var svc = new ReportService(mockUOWRepo.Object);
        //    #endregion

        //    #region Act
        //    var baseRsp = svc.ExportCodingBook(request, ref response, tempDirPath);
        //    #endregion

        //    #region Assert
        //    Assert.AreEqual(JsonConvert.SerializeObject(expectedBaseResponse), JsonConvert.SerializeObject(baseRsp));
        //    Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(response.FileName, expectedResponse.FileName));
        //    Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(response.FilePath, expectedResponse.FilePath));
        //    #endregion
        //}

        //[TestMethod()]
        //public void ExportDataTest()
        //{
        //    #region Arrange
        //    var mockReportMRepo = new Mock<IX1_ReportMRepository>();
        //    mockReportMRepo.Setup(x => x.GetLatestReportM(null, 1, null)).Returns(new X1_Report_Main
        //    {
        //        ID = 1,
        //        Title = "Smear"
        //    });

        //    var mockReportQRepo = new Mock<IX1_ReportQuestionRepository>();
        //    mockReportQRepo.Setup(x => x.GetAllQuestions(1)).Returns((int reportMID) =>
        //    {
        //        var list = new List<X1_Report_Question>()
        //        {
        //            new X1_Report_Question()
        //            {
        //                ID = 1000,
        //                CodingBookIndex = 1,
        //                CodingBookTitle = "Quest1",
        //                ReportID = 1,
        //                QuestionText = "Quest1"
        //            },
        //            new X1_Report_Question()
        //            {
        //                ID = 1001,
        //                CodingBookIndex = 2,
        //                CodingBookTitle = "Quest2",
        //                ReportID = 1,
        //                QuestionText = "Quest2"
        //            },
        //            new X1_Report_Question()
        //            {
        //                ID = 1002,
        //                CodingBookIndex = -1,
        //                CodingBookTitle = "Quest3",
        //                ReportID = 1,
        //                QuestionText = "Quest3"
        //            }
        //        };

        //        return list.AsEnumerable();
        //    });

        //    var mockReportAnsMRepo = new Mock<IX1_ReportAnswerMRepository>();
        //    mockReportAnsMRepo.Setup(x => x.GetReport(null, 1)).Returns(new X1_Report_Answer_Main() { ID = 10, ReportID = 1 });

        //    var mockReportAnsDRepo = new Mock<IX1_ReportAnswerDRepository>();
        //    mockReportAnsDRepo.Setup(x => x.GetAllAns(10)).Returns((int ansMID) =>
        //    {
        //        List<X1_Report_Answer_Detail> list = new List<X1_Report_Answer_Detail>()
        //        {
        //            new X1_Report_Answer_Detail()
        //            {
        //                ID = 100,
        //                AnswerMID = 10,
        //                QuestionID = 1000,
        //                Value = "0"
        //            },
        //            new X1_Report_Answer_Detail()
        //            {
        //                ID = 101,
        //                AnswerMID = 10,
        //                QuestionID = 1001,
        //                Value = "abc"
        //            },
        //            new X1_Report_Answer_Detail()
        //            {
        //                ID = 102,
        //                AnswerMID = 10,
        //                QuestionID = 1000,
        //                Value = "1"
        //            },
        //            new X1_Report_Answer_Detail()
        //            {
        //                ID = 101,
        //                AnswerMID = 10,
        //                QuestionID = 1001,
        //                Value = "abc"
        //            }
        //        };

        //        return list.AsQueryable();
        //    });

        //    var mockUOWRepo = new Mock<IUnitOfWork>();
        //    mockUOWRepo.Setup(x => x.Get<IX1_ReportMRepository>()).Returns(mockReportMRepo.Object);
        //    mockUOWRepo.Setup(x => x.Get<IX1_ReportQuestionRepository>()).Returns(mockReportQRepo.Object);
        //    mockUOWRepo.Setup(x => x.Get<IX1_ReportAnswerMRepository>()).Returns(mockReportAnsMRepo.Object);
        //    mockUOWRepo.Setup(x => x.Get<IX1_ReportAnswerDRepository>()).Returns(mockReportAnsDRepo.Object);

        //    var request = new ExportDataM.ExportDataReq
        //    {
        //        ID = 1
        //    };

        //    var response = new ExportDataM.ExportDataRsp();

        //    var expectedResponse = new ExportDataM.ExportDataRsp()
        //    {
        //        FileName = "[0-9]{14}_Smear.xlsx",
        //        FilePath = Path.Combine(tempDirPath, "[0-9a-f]{32}").Replace("\\", "\\\\")
        //    };

        //    var expectedBaseResponse = new RSPBase()
        //    {
        //        ReturnCode = ErrorCode.OK,
        //        ReturnMsg = "ok"
        //    };

        //    var svc = new ReportService(mockUOWRepo.Object);
        //    #endregion

        //    #region Act
        //    var baseRsp = svc.ExportData(request, ref response, tempDirPath);
        //    #endregion

        //    #region Assert
        //    Assert.Equals(JsonConvert.SerializeObject(expectedBaseResponse), JsonConvert.SerializeObject(baseRsp));
        //    Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(response.FileName, expectedResponse.FileName));
        //    Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(response.FilePath, expectedResponse.FilePath));
        //    #endregion
        //}

        //[TestMethod()]
        //public void AddReportAuthTest()
        //{
        //    //// Arrange
        //    //var insertAuth = new List<X1_Report_Authorization>();
        //    //object[] deleteAuth = null;

        //    //var mockAuthRepo = new Mock<IX1_ReportAuthRepository>();
        //    //mockAuthRepo.Setup(x => x.Create(It.IsAny<X1_Report_Authorization>())).Callback((X1_Report_Authorization auth) =>
        //    //{
        //    //    insertAuth.Add(auth);
        //    //});
        //    //mockAuthRepo.Setup(x => x.GetAllAuth(1)).Returns(() =>
        //    //{
        //    //    var list = new List<X1_Report_Authorization>()
        //    //    {
        //    //        new X1_Report_Authorization()
        //    //        {
        //    //            ID = 10,
        //    //            ReportID = 1,
        //    //            Name = "Hospital",
        //    //            Value = "H01"
        //    //        },
        //    //        new X1_Report_Authorization()
        //    //        {
        //    //            ID = 11,
        //    //            ReportID = 1,
        //    //            Name = "Hospital",
        //    //            Value = "H02"
        //    //        }
        //    //    };
        //    //    return list.AsQueryable();
        //    //});
        //    //mockAuthRepo.Setup(x => x.Delete(It.IsAny<object[]>())).Callback((object[] keys) =>
        //    //{
        //    //    deleteAuth = keys;
        //    //});

        //    //var mockReport = new Mock<IX1_ReportMRepository>();
        //    //mockReport.Setup(x => x.GetLatestReportM(null, 1, null)).Returns(new X1_Report_Main()
        //    //{
        //    //    ID = 1
        //    //});

        //    //var mockUOW = new Mock<IUnitOfWork>();
        //    //mockUOW.Setup(x => x.Get<IX1_ReportAuthRepository>()).Returns(mockAuthRepo.Object);
        //    //mockUOW.Setup(x => x.Get<IX1_ReportMRepository>()).Returns(mockReport.Object);

        //    //var request = new AddReportAuthM.AddReportAuthReq()
        //    //{
        //    //    ReportID = 1,
        //    //    AuthList = new List<AddReportAuthM.AuthItem>()
        //    //    {
        //    //        new AddReportAuthM.AuthItem()
        //    //        {
        //    //            Name = "Auth1",
        //    //            Value = "Value1"
        //    //        },
        //    //        new AddReportAuthM.AuthItem()
        //    //        {
        //    //            Name = "Auth2",
        //    //            Value = "Value2"
        //    //        }
        //    //    },
        //    //    AccID = "user"
        //    //};

        //    //var response = new AddReportAuthM.AddReportAuthRsp();

        //    //var expectedRsp = new AddReportAuthM.AddReportAuthRsp()
        //    //{
        //    //    ReturnCode = ErrorCode.OK,
        //    //    ReturnMsg = "OK"
        //    //};

        //    //var expectedInsertAuth = new List<X1_Report_Authorization>()
        //    //{
        //    //    new X1_Report_Authorization()
        //    //    {
        //    //        ReportID = 1,
        //    //        Name = "Auth1",
        //    //        Value = "Value1",
        //    //        CreateMan = "user"
        //    //    },
        //    //    new X1_Report_Authorization()
        //    //    {
        //    //        ReportID = 1,
        //    //        Name = "Auth2",
        //    //        Value = "Value2",
        //    //        CreateMan = "user"
        //    //    }
        //    //};

        //    //var expectedDeleteAuth = new object[] { 10, 11 };

        //    //var service = new ReportService(mockUOW.Object);

        //    //// Act
        //    //var baseRsp = service.AddReportAuth(request, ref response);

        //    //// Assert
        //    //Assert.AreEqual(JsonConvert.SerializeObject(expectedRsp), JsonConvert.SerializeObject(baseRsp));
        //    //Assert.AreEqual(JsonConvert.SerializeObject(expectedInsertAuth), JsonConvert.SerializeObject(insertAuth));
        //    //Assert.AreEqual(JsonConvert.SerializeObject(expectedDeleteAuth), JsonConvert.SerializeObject(deleteAuth));
        //}
    }
}