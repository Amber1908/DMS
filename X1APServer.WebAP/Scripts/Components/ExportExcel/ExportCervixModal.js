import React, { useState, useEffect, useContext } from 'react';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { GlobalConstants } from '../../Utilities/CommonUtility';
import { CookieContext } from '../Context';
import LoadingComponent from '../LoadingComponent';
import { produce } from 'immer';
import Loader from '../Loader';

const initFormData = {
    startDate: "",
    endDate: "",
    formStatus: "0"
}

const ExportCervixModal = (props) => {

    const [formData, setFormData] = useState(initFormData);
    const [exportBtnStatus, setExportBtnStatus] = useState(GlobalConstants.Status.INIT);
    const { PostWithAuth } = usePostAuth();
    const { cookies } = useContext(CookieContext);

    const init = () => {
        let usercookie = cookies[GlobalConstants.CookieName];
        if (usercookie != null && usercookie.AccID != null && usercookie.AccID !== "") {
        }
    }

    const reqExportCervixExcel = () => {
        setExportBtnStatus(GlobalConstants.Status.LOADING);
        PostWithAuth({
            url: "/Report/ExportCervixData",
            data: {
                "StartDate": formData.startDate,
                "EndDate": formData.endDate,
                "DiagnosedstartDate": formData.DiagnosedstartDate,
                "DiagnosedendDate": formData.DiagnosedendDate,
                "Status": formData.formStatus,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1,
               
                //todo TingYu 加Web_sn  cookies[GlobalConstants.CookieName].WebSn
            },
            success: (rsp) => {
                setExportBtnStatus(GlobalConstants.Status.INIT);
                // window.location = rsp.ExcelUrl;
                openDownloadDialog(rsp.ExcelUrl, "PAS.TXT");
            }
        })
    }

    const openDownloadDialog = (url, fileName) => {
        if (typeof url === 'object' && url instanceof Blob) {
            url = URL.createObjectURL(url); // 创建blob地址
        }
        const aLink = document.createElement('a');
        aLink.href = url;
        aLink.download = fileName;
        aLink.click();
    };

    useEffect(() => {
        $("#exportCervixModal").on('show.bs.modal', init);
        return () => {
            $("#exportCervixModal").off('show.bs.modal', init);
        }
    }, [cookies[GlobalConstants.CookieName]])

    const handleSubmit = (e) => {
        e.preventDefault();
        reqExportCervixExcel();
    }

    const handleFormChange = (e) => {
        const target = e.target;
        setFormData(prev => ({ ...prev, [target.name]: target.value }));
    }

    return (
        <div id="exportCervixModal" className="modal fade" tabIndex={-1} role="dialog">
            <div className="modal-dialog modal-lg" role="document">
                <div className="modal-content">
                    <div className="modal-header">
                        <button type="button" className="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                        <h4 className="modal-title">匯出子宮頸國健署資料</h4>
                    </div>
                    <div className="modal-body">
                        <form onSubmit={handleSubmit}>
                            <div>
                                <div className="form-group col-md-6">
                                    <label htmlFor="startDate">抹片收件日起始</label>
                                    <input type="date" className="form-control" id="startDate" name="startDate" value={formData.startDate} style={{ fontSize: "14px" }} onChange={handleFormChange} />
                                </div>
                                <div className="form-group col-md-6">
                                    <label htmlFor="endDate">抹片收件日結束</label>
                                    <input type="date" className="form-control" id="endDate" name="endDate" value={formData.endDate} style={{ fontSize: "14px" }} onChange={handleFormChange} />
                                </div>
                            </div>
                            <div>
                                <div className="form-group col-md-6">
                                    <label htmlFor="startDate">確診起始</label>
                                    <input type="date" className="form-control" id="DiagnosedstartDate" name="DiagnosedstartDate" value={formData.DiagnosedstartDate} style={{ fontSize: "14px" }} onChange={handleFormChange} />
                                </div>
                                <div className="form-group col-md-6">
                                    <label htmlFor="endDate">確診訖日</label>
                                    <input type="date" className="form-control" id="DiagnosedendDate" name="DiagnosedendDate" value={formData.DiagnosedendDate} style={{ fontSize: "14px" }} onChange={handleFormChange} />
                                </div>
                            </div>

                            <div>
                                <div className="QForm-Table-Label">檢驗單狀態</div>
                                <div className="radio-item">
                                    <input type="radio" id="formStatus1" name="formStatus" required="" value="0" onChange={handleFormChange} checked={formData.formStatus == "0" } />
                                    <label for="formStatus1">全部</label>
                                </div>
                                <div className="radio-item">
                                    <input type="radio" id="formStatus2" name="formStatus" required="" value="1" onChange={handleFormChange} />
                                    <label for="formStatus2">已結案</label>
                                </div>
                                <div className="radio-item">
                                    <input type="radio" id="formStatus3" name="formStatus" required="" value="2" onChange={handleFormChange} />
                                    <label for="formStatus3">已匯出</label>
                                </div>
                            </div>
                            <button className="btn btn-default btn-block" disabled={exportBtnStatus === GlobalConstants.Status.LOADING} style={{ marginTop: "10px" }}>
                                <Loader status={exportBtnStatus} />
                                匯出
                            </button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default ExportCervixModal;