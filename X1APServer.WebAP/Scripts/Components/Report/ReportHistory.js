import React, { useEffect, useState, useContext } from 'react';
import { useLocation, useHistory, useParams } from 'react-router-dom';

import WorkSpaceMain from '../WorkSpaceMain';
import ReportHistoryItem from './ReportHistoryItem';

import { GlobalConstants } from '../../Utilities/CommonUtility';
import { CaseInfoContext } from '../Context';
import { setTimeout } from 'timers';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { Ajax } from '../../Utilities/AjaxUtility';
import AuthComponent from '../AuthComponent';

const ReportHistoryToolbar = (props) => {
    // 表單類別
    let { report } = useParams();

    let location = useLocation();
    let history = useHistory();

    const [reportFuncCode, setReportFuncCode] = useState("");
    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        if (report == null || report === "") return;

        PostWithAuth({
            url: "/Report/GetReportMain",
            data: {
                ReportCategory: report,
                FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                AuthCode: 1,
            },
            success: (rsp) => {
                setReportFuncCode(rsp.FuncCode);
            },
        });
    }, [report]);

    // 新增表單
    const handleAddReport = (e) => {
        history.push(`${location.pathname}/New`);
    };

    // 匯出編碼簿
    const handleExportCodingBook = (e) => {
        // window.location = `${Ajax.webapiBaseURL}/Report/ExportCodingBook?ID=${report}&isPublish=true`;
        PostWithAuth({
            url: "/Report/ExportCodingBook",
            data: {
                "ReportMID": report,
                "IsPublish": true,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (rsp) => {
                window.location = rsp.ExcelUrl;
            }
        });
    };

    return (
        <>
            <div className="ui-toolBar-Group ui-Col-33">
                <label>表單工具</label>
                <div className="ui-buttonGroup">
                    <AuthComponent FuncCode={reportFuncCode} AuthNos={["AuthNo2"]}>
                        <button onClick={handleAddReport} className="offsetLeft-5" data-tooltip="新增表單">
                            <i className="fa fa-pencil" aria-hidden="true" />
                            新增
                        </button>
                    </AuthComponent>
                    <button onClick={handleExportCodingBook} className="offsetLeft-5" data-tooltip="匯出編碼簿">
                        <i className="fa fa-file-o" aria-hidden="true" />
                        匯出編碼簿
                    </button>
                </div>
            </div>
        </>
    );
}

const ReportHistoryWindow = (props) => {
    // 選擇的個案資料
    const { caseInfo } = useContext(CaseInfoContext);
    // 表單填寫紀錄
    const [reports, setreports] = useState([]);
    // 表單類別
    const { report } = useParams();

    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        if (!isInitializeComplete()) return;

        setreports([]);

        PostWithAuth({
            url: "/Report/GetReports",
            data: {
                "IDNo": caseInfo.IDNo,
                "Category": report,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (response) => {
                // 延遲顯示表單清單避免突兀
                setTimeout(() => {
                    if (response.Reports.length === 0) return;

                    let showReports = response.Reports.map((report, index) => (
                        <ReportHistoryItem key={index} index={index + 1} reportInfo={report} />
                    ));

                    setreports(showReports);
                }, 500);
            }
        });
    }, [report, caseInfo]);

    // 初始化完成?
    const isInitializeComplete = () => {
        return report !== "" && caseInfo.ID != null;
    }

    // 有表單紀錄?
    const haveReport = () => {
        return reports.length > 0;
    }

    return (
        <div className="QForm-Wrap" id="QFormxContainer" name="QFormContainer">
            <span id="Status" hidden>表單清單</span>
            <div id="HistoryForm" className="Q-History ui-historyWrap ui-Col-100 cfg-historyListView">
                <h4>表單清單</h4>
                <div className="ui-blankHistory" hidden={haveReport()}>
                    <h4>
                        <i className="fa fa-info-circle" aria-hidden="true" />沒有任何表單
                    </h4>
                    <span>請在工具欄按新增表單</span>
                </div>
                {reports}
            </div>
        </div>
    );
};

const ReportHistory = (props) => {
    return (<WorkSpaceMain window={<ReportHistoryWindow />} 
                toolbar={<ReportHistoryToolbar />} />);
};

export default ReportHistory;