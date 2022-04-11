using Microsoft.VisualStudio.TestTools.UnitTesting;
using X1APServer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using X1APServer.Repository.Interface;
using X1APServer.Repository;
using X1APServer.Repository.Utility.Interface;
using X1APServer.Service.Model;
using Newtonsoft.Json;

namespace X1APServer.Service.Tests
{
    [TestClass()]
    public class PatientServiceTests
    {
        [TestMethod()]
        public void GetAllGroupTest()
        {
            // Arrange
            var allGroup = new List<X1_PatientGroup>()
                {
                    new X1_PatientGroup()
                    {
                        ID = 1,
                        GroupName = "Group1",
                        IsDelete = false
                    },
                    new X1_PatientGroup()
                    {
                        ID = 2,
                        GroupName = "Group2",
                        IsDelete = false
                    },
                };

            var mockPGroupRepo = new Mock<IX1_PatientGroupRepository>();
            mockPGroupRepo.Setup(x => x.GetAllGroup()).Returns(() =>
            {
                return allGroup.AsQueryable();
            });

            var mockUOW = new Mock<IX1UnitOfWork>();
            mockUOW.Setup(x => x.Get<IX1_PatientGroupRepository>()).Returns(mockPGroupRepo.Object);

            var service = new PatientService(mockUOW.Object);

            var request = new GetAllGroupM.GetAllGroupReq();
            var response = new GetAllGroupM.GetAllGroupRsp();

            var expectedRsp = new GetAllGroupM.GetAllGroupRsp()
            {
                Data = allGroup
            };

            var expectedBaseRsp = new RSPBase()
            {
                ReturnCode = ErrorCode.OK,
                ReturnMsg = "OK"
            };

            // Act
            var baseRsp = service.GetAllGroup(request, ref response);

            // Assert
            Assert.AreEqual(JsonConvert.SerializeObject(expectedBaseRsp), JsonConvert.SerializeObject(baseRsp));
            Assert.AreEqual(JsonConvert.SerializeObject(expectedRsp), JsonConvert.SerializeObject(response));
        }

        [TestMethod()]
        public void UpdateGroupTest()
        {
            // Arrange
            var updateGroup = new List<X1_PatientGroup>();
            var insertGroup = new List<X1_PatientGroup>();

            var mockPGroupRepo = new Mock<IX1_PatientGroupRepository>();
            mockPGroupRepo.Setup(x => x.Update(It.IsAny<X1_PatientGroup>())).Callback((X1_PatientGroup group) =>
            {
                updateGroup.Add(group);
            });
            mockPGroupRepo.Setup(x => x.Create(It.IsAny<X1_PatientGroup>())).Returns((X1_PatientGroup group) =>
            {
                insertGroup.Add(group);
                return group;
            });
            mockPGroupRepo.Setup(x => x.GetGroup(It.IsAny<int>())).Returns((int id) =>
            {
                return new X1_PatientGroup()
                {
                    ID = id,
                    GroupName = "GroupOrigin"
                };
            });

            var mockUOW = new Mock<IX1UnitOfWork>();
            mockUOW.Setup(x => x.Get<IX1_PatientGroupRepository>()).Returns(mockPGroupRepo.Object);

            var service = new PatientService(mockUOW.Object);

            var request = new UpdateGroupM.UpdateGroupReq()
            {
                PatientGroups = new List<UpdateGroupM.PatientGroup>()
                {
                    new UpdateGroupM.PatientGroup()
                    {
                        ID = 1,
                        GroupName = "GroupModify",
                        State = UpdateGroupM.PatientGroupState.Modify
                    },
                    new UpdateGroupM.PatientGroup()
                    {
                        GroupName = "GroupNew",
                        State = UpdateGroupM.PatientGroupState.New
                    },
                    new UpdateGroupM.PatientGroup()
                    {
                        ID = 2,
                        GroupName = "GroupDetete",
                        State = UpdateGroupM.PatientGroupState.Delete
                    }
                },
                AccID = "User"
            };

            var response = new UpdateGroupM.UpdateGroupRsp();

            var expectedBaseRsp = new RSPBase()
            {
                ReturnCode = ErrorCode.OK,
                ReturnMsg = "OK"
            };

            var expectedInsert = new List<X1_PatientGroup>()
            {
                new X1_PatientGroup()
                {
                    GroupName = "GroupNew",
                    CreateMan = "User",
                    ModifyMan = "User"
                }
            };

            var expectedUpdate = new List<X1_PatientGroup>()
            {
                new X1_PatientGroup()
                {
                    ID = 1,
                    GroupName = "GroupModify",
                    ModifyMan = "User"
                },
                new X1_PatientGroup()
                {
                    ID = 2,
                    GroupName = "GroupOrigin",
                    ModifyMan = "User",
                    DeleteMan = "User",
                    IsDelete = true
                }
            };

            // Act
            var baseRsp = service.UpdateGroup(request, ref response);

            // Assert
            Assert.AreEqual(JsonConvert.SerializeObject(expectedBaseRsp), JsonConvert.SerializeObject(baseRsp));
            Assert.AreEqual(JsonConvert.SerializeObject(expectedInsert), JsonConvert.SerializeObject(insertGroup));
            Assert.AreEqual(JsonConvert.SerializeObject(expectedUpdate), JsonConvert.SerializeObject(updateGroup));
        }
    }
}