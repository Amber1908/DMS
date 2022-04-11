import React, { useState, useEffect } from "react";
import { useHistory, useParams } from "react-router-dom";
import { useAuthCheck, usePostAuth } from '../../CustomHook/CustomHook';
import WorkSpaceMain from "../WorkSpaceMain";
import { ReportState } from "./Report";
import { GlobalConstants, GetDateTime } from '../../Utilities/CommonUtility';

// Report 工具列
const WorkSpaceCommonReportToolBar = (props) => {
    const { id, report, reportID } = useParams();
    // Custom Hooks
    const { PostWithAuth } = usePostAuth();

    // 工具列 Disabled 狀態
    const [toolbarStatus, setToolbarStatus] = useState({
        editDisabled: true,
        saveDisabled: true,
        cancelDisabled: true,
        closeoutDisable: true,
    });

    const history = useHistory();

    useEffect(() => {
        changeToolbarState();
    }, [props.state, props.status]);

    // 根據狀態禁用按鈕
    const changeToolbarState = () => {
        switch (props.state) {
            case ReportState.Edit:
                setToolbarStatus({
                    editDisabled: true,
                    saveDisabled: false,
                    cancelDisabled: false,
                    closeoutDisable: props.status,
                });
                break;
            case ReportState.ViewOnly:
                setToolbarStatus({
                    editDisabled: true,
                    saveDisabled: true,
                    cancelDisabled: true,
                    closeoutDisable: true,
                });
                break;
            default:
                setToolbarStatus({
                    editDisabled: props.status,
                    saveDisabled: true,
                    cancelDisabled: true,
                    closeoutDisable: true,
                });
                break;
        }
    };

    // 取消編輯導回歷史清單
    const handleCancelClick = (e) => {
        if (confirm("未儲存表單確定離開?")) {
            redirectToReportHistory();
        }
    };

    // 結案
    const handleCloseoutClick = (e) => {
        let funcAry = [];

        if (!confirm("確定進行結案 ( 結案前請先確認資料都已儲存 ) ? ")) return;

        let closeoutFunc = () => {
            return PostWithAuth({
                url: "/Report/CloseoutReport",
                data: {
                    "ReportAnsMID": reportID,
                    "FuncCode": GlobalConstants.FuncCode.Backend,
                    "AuthCode": 1
                },
                success: (response) => {
                    alert("結案完成");
                    redirectToReportHistory();
                }
            });
        };

        funcAry.push(closeoutFunc);
        funcAry.reduce((prev, curr) => prev.then(curr), Promise.resolve());
    }

    const handleReturnHistoryClick = (e) => {
        redirectToReportHistory();
    };

    // 導回歷史清單
    const redirectToReportHistory = () => {
        history.replace(`/Index/Patient/${id}/${report}`);
    };

    // 表單狀態重置
    const resetFormStatus = (e) => {
        PostWithAuth({
            url: "/Report/UpdateReportStatus",
            data: {
                "ID": reportID,
                "Status": 1,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (response) => {
                alert("表單狀態重置完成");
                redirectToReportHistory();
            }
        });
    };

    return (
        <>
            <div className="ui-toolBar-Group ui-Col-33">
                <label>表單編輯工具</label>
                <div className="ui-buttonGroup">
                    <button
                        disabled={toolbarStatus.editDisabled}
                        onClick={props.handleEditClick}
                        className="offsetLeft-5"
                        data-tooltip="編輯表單"
                    >
                        <i className="fa fa-pencil" aria-hidden="true" />
                        編輯
                    </button>
                    <button
                        form="Report"
                        type="submit"
                        disabled={toolbarStatus.saveDisabled}
                        className="offsetLeft-5"
                        data-tooltip="儲存表單"
                    >
                        <i className="fa fa-floppy-o" aria-hidden="true" />
                        儲存
                    </button>
                    <button
                        disabled={toolbarStatus.cancelDisabled}
                        onClick={handleCancelClick}
                        className="offsetLeft-5"
                        data-tooltip="取消編輯"
                    >
                        <i className="fa fa-times" aria-hidden="true" />
                        取消
                    </button>
                    <button
                        disabled={toolbarStatus.closeoutDisable}
                        onClick={handleCloseoutClick}
                        className="offsetLeft-5"
                        data-tooltip="表單結案"
                    >
                        <i className="fa fa-briefcase" aria-hidden="true" />
                        結案
                    </button>
                </div>
            </div>
            <div className="ui-toolBar-Group ui-Col-25">
                <label>表單歷史紀錄</label>
                <div className="ui-buttonGroup">
                    <button
                        onClick={handleReturnHistoryClick}
                        className="offsetLeft-5"
                        data-tooltip="查看歷史紀錄清單">
                        <i className="fa fa-list-ul" aria-hidden="true" />
                        歷史紀錄清單
                    </button>
                    <button
                        onClick={resetFormStatus}
                        className="offsetLeft-5"
                        data-tooltip="表單狀態重置為待檢驗">
                        <i className="fa fa-repeat" aria-hidden="true" />
                        表單狀態重置
                    </button>
                </div>
            </div>
        </>
    );
};

const WorkSpaceCommonReportMain = (props) => {
    return (
        <WorkSpaceMain
            toolbar={
                <WorkSpaceCommonReportToolBar
                    state={props.state}
                    status={props.status}
                    handleEditClick={props.handleEditClick}
                />
            }
            window={props.window}
        />
    );
};

export default WorkSpaceCommonReportMain;
