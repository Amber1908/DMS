import React, { useState, useContext, useEffect } from 'react';
import { Redirect } from "react-router-dom";

import WorkSpaceMain from '../WorkSpaceMain';
import { URLInfoContext } from '../Context';
import { useAuthCheck, usePostAuth } from '../../CustomHook/CustomHook';
import { Ajax } from '../../Utilities/AjaxUtility';
import { GlobalConstants, GetDateTime } from '../../Utilities/CommonUtility';

const EXPORT_STATE = {
    NONE: "",
    LOADING: "cfg-loaderFullScreen",
    DOWNLOADABLE: "cfg-loaderFullScreen-download"
}

// Report 工具欄
const WorkSpaceReportToolBar = (props) => {
    // States
    // 是否導回歷史頁面
    const [redirectToHistory, setRedirectToHistory] = useState(false);
    // 工具欄 Disabled 狀態
    const [toolbarStatus, setToolbarStatus] = useState({
        editDisabled: true,
        saveDisabled: true,
        cancelDisabled: true
    });
    // 匯出 Disabled 狀態 
    const [exportDisabled, setExportDisabled] = useState(true);
    // 解鎖顯示
    const [unlockDisable, setUnlockDisable] = useState(true);
    // 匯出狀態
    const [exportState, setExportState] = useState(EXPORT_STATE.NONE);
    // PDF URL
    const [pdfURL, setPdfURL] = useState("");

    // Contexts
    // 前後台 URL
    const match = useContext(URLInfoContext);
    // 使用者權限
    const { userAuth, AuthCheck } = useAuthCheck();

    const { id, report, reportID } = props.match.params;

    // Custom Hooks
    const { PostWithAuth } = usePostAuth();

    // Effects
    useEffect(() => {
        setRedirectToHistory(false);
        setToolbarStatus({
            editDisabled: true,
            saveDisabled: true,
            cancelDisabled: true
        });
        setExportDisabled(true);
        setUnlockDisable(true);

        // 驗證是否有填寫權限
        if (AuthCheck({ FuncName: report })) {
            setToolbarStatus({
                editDisabled: false,
                saveDisabled: true,
                cancelDisabled: true
            });
        }

        // 驗證是否有匯出權限
        if (AuthCheck({ FuncName: report, AuthNos: ["AuthNo2"] })) {
            setExportDisabled(false);
        }

        if (AuthCheck({ FuncCode: GlobalConstants.FuncCode.Backend }) && props.exportLock) {
            setUnlockDisable(false);
        }

        // Report 鎖定
        if (props.exportLock) {
            setToolbarStatus({
                editDisabled: true,
                saveDisabled: true,
                cancelDisabled: true
            });
        }
    }, [userAuth, props.exportLock]);

    // 切換工具欄狀態
    const handleEditClick = (e) => {
        props.handleEditClick(e);
        setToolbarStatus({
            editDisabled: true,
            saveDisabled: false,
            cancelDisabled: false
        });
    };

    // 導回歷史頁
    const handleCancelClick = (e) => {
        if (confirm("未儲存 Report 確定離開?")) {
            setRedirectToHistory(true);
        }
    };

    const handleReturnHistoryClick = (e) => {
        setRedirectToHistory(true);
    };

    // 匯出
    const handleExportClick = () => {
        let funcAry = [];

        // 若還未儲存先儲存再匯出
        if (toolbarStatus.saveDisabled == false) {
            funcAry.push(props.handleSaveReportClick);
        }

        let exportFunc = () => {
            setExportState(EXPORT_STATE.LOADING);

            return PostWithAuth({
                url: "/Report/ExportReport",
                data: {
                    "ID": reportID,
                    "FuncCode": GlobalConstants.FuncCode.Report[report],
                    "AuthCode": 2
                },
                success: (response) => {
                    // 鎖定 Report
                    props.setExportLock(true);
                    setPdfURL(response.pdfURL);
                    setExportState(EXPORT_STATE.DOWNLOADABLE);
                }
            });
        };

        switch (report) {
            case "Smear":
                PostWithAuth({
                    url: "/Report/GetLatestCBCExportDate",
                    data: {
                        "ReportAnsMID": reportID,
                        "FuncCode": GlobalConstants.FuncCode.Report[report],
                        "AuthCode": 2
                    }
                }).then(value => {
                    if (confirm("表單匯出後將被鎖定無法修改\nCBC 匯出日期為 " + GetDateTime(value.CBCExportDate) + " 確定匯出?")) {
                        funcAry.push(exportFunc);
                        funcAry.reduce((prev, curr) => prev.then(curr), Promise.resolve());
                    }
                })
                break;
            default:
                if (confirm("表單匯出後將被鎖定無法修改\n確定匯出?")) {
                    funcAry.push(exportFunc);
                    funcAry.reduce((prev, curr) => prev.then(curr), Promise.resolve());
                }
                break;
        }

        
    }

    const handleUnlockClick = (e) => {
        PostWithAuth({
            url: "/Report/UnlockReport",
            data: {
                "ReportAnsMID": reportID,
                "FuncCode": GlobalConstants.FuncCode.Backend,
                "AuthCode": 1
            },
            success: (response) => {
                alert("解鎖成功");
                props.setExportLock(false);
            }
        })
    }
    const handleSaveReportClick = (e) => {
        setToolbarStatus({
            editDisabled: false,
            saveDisabled: true,
            cancelDisabled: true
        });
        props.handleCancelClick(e);
        props.handleSaveReportClick(e);
    }

    // 開啟表單檢視器
    const handleReportViewerClick = () => {
        window.open(`${Ajax.BaseURL}/ReportViewer/${id}`);
    }

    // 關閉下載畫面
    const handleDownloadPdfClick = () => {
        setExportState(EXPORT_STATE.NONE);
    }

    if (redirectToHistory === true) {
        return <Redirect to={`${match.url}/Patient/${id}/${report}`} />;
    }

    return (
        <>
            <div className="ui-toolBar-Group ui-Col-33">
                <label>表單編輯工具</label>
                <div className="ui-buttonGroup">
                    {/* <button className="btnGreen offsetLeft-5" data-tooltip="新增表單"><i className="fa fa-plus" aria-hidden="true" />新增</button> */}
                    <button disabled={toolbarStatus.editDisabled} onClick={handleEditClick} className="offsetLeft-5" data-tooltip="編輯表單">
                        <i className="fa fa-pencil" aria-hidden="true" />
                        編輯
                    </button>
                    <button disabled={toolbarStatus.saveDisabled} onClick={handleSaveReportClick} className="offsetLeft-5" data-tooltip="儲存表單">
                        <i className="fa fa-floppy-o" aria-hidden="true" />
                        儲存
                    </button>
                    <button disabled={toolbarStatus.cancelDisabled} onClick={handleCancelClick} className="offsetLeft-5" data-tooltip="取消編輯">
                        <i className="fa fa-times" aria-hidden="true" />
                        取消
                    </button>
                    <button disabled={exportDisabled} onClick={handleExportClick} className="offsetLeft-5" data-tooltip="表單匯出">
                        <i className="fa fa-share-square-o" aria-hidden="true" />
                        匯出
                    </button>
                    <button disabled={unlockDisable} onClick={handleUnlockClick} className="offsetLeft-5" data-tooltip="表單解鎖">
                        <i className="fa fa-unlock" aria-hidden="true" />
                        解鎖
                    </button>
                </div>
            </div>
            <div className="ui-toolBar-Group ui-Col-25">
                <label>表單歷史紀錄</label>
                <div className="ui-buttonGroup">
                    <button onClick={handleReturnHistoryClick} className="offsetLeft-5" data-tooltip="查看歷史紀錄清單"><i className="fa fa-list-ul" aria-hidden="true" />歷史紀錄清單</button>
                    <button onClick={handleReportViewerClick} className="offsetLeft-5" data-tooltip="查看過去匯出的報告"><i className="fa fa-list-ul" aria-hidden="true" />報告檢視器</button>
                </div>
            </div>
            <div className={`ui-contentLoader hide ${exportState}`}>
                <span className="hide">還沒載入完成？<br />請嘗試 <button className="ev-forceReload">刷新頁面</button> 或檢查您的網路連接</span>
                <a href={pdfURL} id="downloadReport" onClick={handleDownloadPdfClick}>下載報表</a>
            </div>
        </>
    );
};

const WorkSpaceReportMain = (props) => {
    return (
        <WorkSpaceMain
            match={props.match}
            toolbar={<WorkSpaceReportToolBar match={props.match} handleCancelClick={props.handleCancelClick} handleSaveReportClick={props.handleSaveReportClick} handleEditClick={props.handleEditClick} exportLock={props.exportLock} setExportLock={props.setExportLock} />}
            window={props.window} 
        />
    );
};

export default WorkSpaceReportMain;