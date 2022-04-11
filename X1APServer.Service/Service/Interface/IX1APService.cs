using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X1APServer.Repository;
using X1APServer.Service.Model;

namespace X1APServer.Service.Service.Interface
{
    public interface IX1APService
    {
        /// <summary>
        /// 取得個案清單
        /// </summary>
        /// <returns></returns>
        X1UserDataGetM.X1UserDataGetRsp GetUserList(X1UserDataGetM.X1UserDataGetReq acctid);
        /// <summary>
        /// 取得檢驗單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetCervixFormM.GetCervixFormRsp GetCervixForm(GetCervixFormM.GetCervixFormReq request);
        /// <summary>
        /// 取得檢驗單明細
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetCervixTableM.GetCervixTableRsp GetCervixTable(GetCervixTableM.GetCervixTableReq request);
        /// <summary>
        /// 更新檢驗單狀態
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        UpdateCervixStatusM.UpdateCervixStatusRsp UpdateCervixStatus(UpdateCervixStatusM.UpdateCervixStatusReq request);
        /// <summary>
        /// 更新檢驗單詳細資料
        /// </summary>
        /// <param name="retuest"></param>
        /// <returns></returns>
        UpdateCervixTableM.UpdateCervixTableRsp UpdateCervixTable(UpdateCervixTableM.UpdateCervixTableReq retuest);
        /// <summary>
        /// 取得X1儲存資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetX1DataM.Response GetX1Data(GetX1DataM.Request request);
        /// <summary>
        /// X1儲存資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        UpdateX1DataM.UpdateX1DataRsp UpdateX1Data(UpdateX1DataM.UpdateX1DataReq request);
    }
}
