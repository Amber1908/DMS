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
    latestRecord: false
}

const ExportExcelModal = (props) => {
    const [reportMList, setReportMList] = useState({
        data: [],
        status: GlobalConstants.Status.INIT
    });
    const [patientList, setPatientList] = useState({
        data: [],
        status: GlobalConstants.FuncCode.INIT
    });
    const [formData, setFormData] = useState(initFormData);
    const [exportBtnStatus, setExportBtnStatus] = useState(GlobalConstants.Status.INIT);

    const { PostWithAuth } = usePostAuth();
    const { cookies } = useContext(CookieContext);

    const reqGetReportList = () => {
        setReportMList(prev => ({ ...prev, status: GlobalConstants.Status.LOADING }));
        PostWithAuth({
            url: "/Report/GetAllReportMain",
            data: {
                "isPublish": true,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (rsp) => {
                setReportMList({
                    data: rsp.ReportMainList.map(r => ({ text: r.Title, name: r.ID, value: r.ID, checked: false, versionSelectHidden: true, versionValue: r.ReportVersionList[0].ID, versionList: r.ReportVersionList })),
                    status: GlobalConstants.Status.INIT
                });
            }
        });
    }

    const reqGetPatientList = () => {
        setPatientList(prev => ({ ...prev, status: GlobalConstants.Status.LOADING }));
        PostWithAuth({
            url: "/Patient/GetPatientsLazy",
            data: {
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (rsp) => {
                setPatientList({
                    data: rsp.Patients.map(p => ({ text: `${p.PUName}(${p.IDNo})`, name: p.ID, value: p.ID, checked: false, hidden: false })),
                    status: GlobalConstants.Status.INIT
                });
            }
        });
    }

    const reqExportExcel = () => {
        const reqReportIDs = reportMList.data.filter(r => r.checked).map(r => r.versionValue);
        //const reqPatientIDs = patientList.data.filter(patient => patient.data.text == patientList.Patientname).map(r => r.value);
        const reqPatientIDs = patientList.data.filter(r => r.checked).map(r => r.value);
        setExportBtnStatus(GlobalConstants.Status.LOADING);
        PostWithAuth({
            url: "/Report/ExportExcel",
            data: {
                "ReportMainIDs": reqReportIDs,
                "StartDate": formData.startDate,
                "EndDate": formData.endDate,
                "LatestRecord": !!formData.latestRecord,
                "PatientIDs": reqPatientIDs,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (rsp) => {
                setExportBtnStatus(GlobalConstants.Status.INIT);
                window.location = rsp.ExcelUrl;
            }
        })
    }

    const init = () => {
        let usercookie = cookies[GlobalConstants.CookieName];
        if (usercookie != null && usercookie.AccID != null && usercookie.AccID !== "") {
            reqGetReportList();
            reqGetPatientList();
        }
    }

    useEffect(() => {
        $("#exportModal").on('show.bs.modal', init);
        return () => {
            $("#exportModal").off('show.bs.modal', init);
        }
    }, [cookies[GlobalConstants.CookieName]])

    const handleSubmit = (e) => {
        e.preventDefault();
        reqExportExcel();
    }

    const handleReportChange = (e, i) => {
        setReportMList(prev => {
            let newState = produce(prev, draft => {
                draft.data[i].checked = !draft.data[i].checked;
            });
            return newState;
        });
    }

    const handlePatientChange = (e, i) => {
        setPatientList(prev => {
            let newState = produce(prev, draft => {
                draft.data[i].checked = !draft.data[i].checked;
            });
            return newState;
        })
    }

    const handleVersionChange = (e, i) => {
        let value = e.target.value;
        setReportMList(prev => {
            let newState = produce(prev, draft => {
                draft.data[i].versionValue = value;
                draft.data[i].checked = true;
            });
            return newState;
        })
    }

    const handleCheckAllReport = (e) => {
        let target = e.target;
        setReportMList(prev => {
            let newState = produce(prev, draft => {
                draft.data = draft.data.map(d => ({ ...d, checked: target.checked }));
            });
            return newState;
        });
    }

    const handleCheckAllPatient = (e) => {
        let target = e.target;
        setPatientList(prev => {
            let newState = produce(prev, draft => {
                draft.data = draft.data.map(d => ({ ...d, checked: target.checked }));
            });
            return newState;
        });
    }

    const handleFormChange = (e) => {
        const target = e.target;
        setFormData(prev => ({ ...prev, [target.name]: target.value }));
    }

    //const handlePatientChange = (e) => {
    //    const target = e.target;
    //    setPatientList(prev => ({ ...prev, Patientname: target.value }));
    //}
    
    //const handlePatientSearch = (e) => {
    //    const target = e.target;
    //    setPatientList(prev => {
    //        let newState = produce(prev, draft => {
    //            draft.data = draft.data.map(d => ({ ...d, hidden: !(d.text.indexOf(target.value) > -1) }))
    //        });
            
    //        console.log("draft.data:" + draft.data);
    //        return newState;
    //    })
    //}

    const getDate = (dateStr) => {
        let date = new Date(dateStr);
        let convertDate = "";

        if (date) {
            convertDate = `${date.getFullYear()}.${date.getMonth() + 1}.${date.getDate()} ${date.getHours()}:${date.getMinutes()}`;
            // convertDate = `${date.getFullYear()}.${date.getMonth() + 1}.${date.getDate()}`;
        }

        return convertDate;
    }

    const createVersionDropDown = (versionList, value, index) => {
        return (
            <select key={"versionSelect_" + index} className="form-control" value={value} style={{ fontSize: "14px" }} onChange={e => handleVersionChange(e, index)}>
                {versionList.map(v => (<option value={v.ID}>{getDate(v.PublishDate)}</option>))}
            </select>
        );
    }

    const toggleVersionSelect = (e, i) => {
        setReportMList(prev => {
            let newState = produce(prev, draft => {
                draft.data[i].versionSelectHidden = !draft.data[i].versionSelectHidden;
            });
            return newState;
        });
    }

    const hanldeCheckboxChange = (e) => {
        const target = e.target;
        setFormData(prev => ({ ...prev, [target.name]: target.checked }));
    }

    let reportElements = [], row = [], itemInRow = 4;
    reportMList.data.forEach((r, i) => {

        if (i != 0 && i % itemInRow == 0) {
            reportElements.push(
                <div className="row" key={reportElements.length}>
                    {row}
                </div>
            );
            row = [];
        }

        row.push(
            <div className="col-md-3" key={r.value}>
                <div className="checkbox">
                    <label>
                        <input type="checkbox" value={r.value} checked={r.checked} onChange={e => handleReportChange(e, i)} />
                        {r.text}
                    </label>
                    <button type="button" className="btn btn-link btn-xs pull-right" onClick={e => toggleVersionSelect(e, i)}>版本</button>
                </div>
                <div hidden={r.versionSelectHidden}>
                    {createVersionDropDown(r.versionList, r.versionValue, i)}
                </div>
            </div>
        );
    });
    reportElements.push(
        <div className="row" key={reportElements.length}>
            {row}
        </div>
    );

    return (
        <div id="exportModal" className="modal fade" tabIndex={-1} role="dialog">
            <div className="modal-dialog modal-lg" role="document">
                <div className="modal-content">
                    <div className="modal-header">
                        <button type="button" className="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                        <h4 className="modal-title">匯出資料</h4>
                    </div>
                    <div className="modal-body">
                        <form onSubmit={handleSubmit}>
                            {/* <label>
                                <input type="checkbox" id="latestRecord" name="latestRecord" value="true" checked={formData.latestRecord} onChange={hanldeCheckboxChange} />
                                最新一筆資料
                            </label> */}
                            <div>
                                <div className="form-group">
                                    <label htmlFor="startDate">開始日期</label>
                                    <input type="date" className="form-control" id="startDate" name="startDate" value={formData.startDate} style={{ fontSize: "14px" }} onChange={handleFormChange} />
                                </div>
                                <div className="form-group">
                                    <label htmlFor="endDate">結束日期</label>
                                    <input type="date" className="form-control" id="endDate" name="endDate" value={formData.endDate} style={{ fontSize: "14px" }} onChange={handleFormChange} />
                                </div>
                            </div>
                            <label>表單篩選</label>
                            <LoadingComponent status={reportMList.status} fontSize="24px">
                                <div className="checkbox">
                                    <label>
                                        <input type="checkbox" onChange={handleCheckAllReport} />
                                            全選
                                    </label>
                                </div>
                                <div style={{ maxHeight: "150px", overflowY: "auto", overflowX: "hidden" }}>
                                    {reportElements}
                                </div>
                            </LoadingComponent>
                            <label>個案篩選</label>
                            <LoadingComponent status={patientList.status} fontSize="24px">
                                <div>
                                    <div className="form-group" style={{ display: "inline-block" }}>
                                        <label class="sr-only" htmlFor="searchPatient">搜尋個案(姓名或身份證號)</label>
                                        <input type="text" className="form-control" id="searchPatient" placeholder="搜尋個案(姓名或身份證號)" style={{ fontSize: "14px", width: "200px" }} onChange={handlePatientSearch} />
                                    </div>
                                    <div className="checkbox" style={{ display: "inline-block", padding: "0px 10px" }}>
                                        <label>
                                            <input type="checkbox" onChange={handleCheckAllPatient} />
                                            全選
                                        </label>
                                    </div>
                                </div>
                            </LoadingComponent>
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

export default ExportExcelModal;