using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Service.Model;

namespace X1APServer.Service.Interface
{
    public interface IPatientService
    {
        /// <summary>
        /// 新增個案
        /// </summary>
        /// <param name="ReqData"></param>
        /// <param name="x1AddPatientInfoRsp"></param>
        /// <returns></returns>
        RSPBase AddPatientInfo(AddPatientInfoM.AddPatientInfoReq ReqData, ref AddPatientInfoM.AddPatientInfoRsp x1AddPatientInfoRsp);
        /// <summary>
        /// 新增病患
        /// </summary>
        /// <param name="ReqData"></param>
        /// <param name="addPatientRsp"></param>
        /// <returns></returns>
        RSPBase AddPatient(AddPatientM.AddPatientReq ReqData, ref AddPatientM.AddPatientRsp addPatientRsp);
        /// <summary>
        /// 取得單一個案
        /// </summary>
        /// <param name="ReqData"></param>
        /// <param name="x1GetPatientInfoRsp"></param>
        /// <returns></returns>
        RSPBase GetPatientInfo(GetPatientInfoM.GetPatientInfoReq ReqData, ref GetPatientInfoM.GetPatientInfoRsp x1GetPatientInfoRsp);
        /// <summary>
        /// 取得個案清單
        /// </summary>
        /// <param name="ReqData"></param>
        /// <param name="x1GetPatientLazyRsp"></param>
        /// <returns></returns>
        RSPBase GetPatientsLazy(GetPatientsLazyM.GetPatientLazyReq ReqData, ref GetPatientsLazyM.GetPatientLazyRsp x1GetPatientLazyRsp);
        /// <summary>
        /// 取得所有病患群組
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase GetAllGroup(GetAllGroupM.GetAllGroupReq request, ref GetAllGroupM.GetAllGroupRsp response);
        /// <summary>
        /// 更新病患群組
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase UpdateGroup(UpdateGroupM.UpdateGroupReq request, ref UpdateGroupM.UpdateGroupRsp response);
        /// <summary>
        /// 新增或更新回診時間
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase AddOrUpdateSchedule(AddOrUpdateScheduleM.Request request, ref AddOrUpdateScheduleM.Response response);
        /// <summary>
        /// 刪除回診時間
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase DeleteSchedule(DeleteScheduleM.Request request, ref DeleteScheduleM.Response response);
        /// <summary>
        /// 取得回診清單
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        RSPBase GetScheduleList(GetScheduleListM.Request request, ref GetScheduleListM.Response response);
    }
}
