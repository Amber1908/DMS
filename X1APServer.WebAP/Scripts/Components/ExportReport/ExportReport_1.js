import React, { useContext, useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import WorkSpaceMain from '../WorkSpaceMain';
import TextInput from '../ReportItem/TextInput';
import Select from '../ReportItem/Select';
import { CaseInfoContext } from '../Context';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { GlobalConstants } from '../../Utilities/CommonUtility';
import LoadingComponent from '../LoadingComponent';
import Loader from '../Loader';

const ExportReportToolbar = (props) => {
    return (
        <div className="ui-toolBar-Group ui-Col-55" hidden>
            <label>帳號編輯工具</label>
            <div className="ui-buttonGroup">
                <button className="btnRed" data-tooltip="停用此使用者帳號"><i className="fa fa-ban" aria-hidden="true" />停用</button>
                <button data-tooltip="開通此使用者帳號" className="btnGreen"><i className="fa fa-check" aria-hidden="true" />開通</button>
            </div>
            <div className="ui-buttonGroup">
                <button data-tooltip="編輯使用者資料"><i className="fa fa-pencil" aria-hidden="true" />編輯</button>
                <button className="offsetLeft5" data-tooltip="儲存" disabled="disabled"><i className="fa fa-floppy-o" aria-hidden="true" />儲存</button>
                <button className="offsetLeft5" data-tooltip="取消" disabled="disabled"><i className="fa fa-times" aria-hidden="true" />取消</button>
            </div>
            <div className="ui-buttonGroup">
                <button data-tooltip="註冊" disabled><i className="fa fa-pencil" aria-hidden="true" />註冊</button>
            </div>
            <div className="ui-buttonGroup">
                <button data-tooltip="刪除帳號"><i className="fa fa-pencil" aria-hidden="true" />刪除帳號</button>
            </div>
        </div>
    );
}

const ExportReportWindow = () => {
    const [templateOption, setTemplateOption] = useState({
        options: [],
        status: GlobalConstants.Status.BEFORE_INIT
    });
    const [templateID, setTemplateID] = useState("");
    const [templateQuest, setTemplateQuest] = useState({
        quests: [],
        status: GlobalConstants.Status.BEFORE_INIT
    });
    const [templateQuestValue, setTemplateQuestValue] = useState({});
    const [exportReportStatus, setExportReportStatus] = useState(GlobalConstants.Status.INIT);

    const { caseInfo } = useContext(CaseInfoContext);
    const { PostWithAuth } = usePostAuth();

    const reqGetTemplateList = () => {
        setTemplateOption(prev => ({ ...prev, status: GlobalConstants.Status.LOADING }));
        PostWithAuth({
            url: "/Report/GetReportTemplateList",
            data: {
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (rsp) => {
                setTemplateOption({
                    options: rsp.TemplateList.map(x => ({ text: x.Name, value: x.ID })),
                    status: GlobalConstants.Status.INIT
                });
            }
        });
    }

    const reqGetTemplateEQuest = (id) => {
        if (id === "" || id == null) return;

        setTemplateQuest(prev => ({ ...prev, status: GlobalConstants.Status.LOADING }));
        PostWithAuth({
            url: "/Report/GetETemplateEQuestList",
            data: {
                "ExportTemplateID": id,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (rsp) => {
                setTemplateQuest({
                    quests: rsp.ExtraQuestList,
                    status: GlobalConstants.Status.INIT
                });
            }
        });
    }

    const reqExportReport = () => {
        let keys = Object.keys(templateQuestValue);
        let extraQuests = []
        keys.forEach(key => {
            extraQuests.push({ Key: key, Value: templateQuestValue[key] });
        });

        setExportReportStatus(GlobalConstants.Status.LOADING);
        let now = new Date();
        let dateFilter = now.getFullYear() + "-" + (now.getMonth() + 1) + "-" + (now.getDate() + 1);
        PostWithAuth({
            url: "/Report/ExportReport",
            data: {
                "ID": templateID,
                "PatientID": caseInfo.ID,
                "SpecificDate": dateFilter,
                "Extra": extraQuests,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (rsp) => {
                setExportReportStatus(GlobalConstants.Status.INIT);
                window.location = rsp.pdfURL;
            }
        });
    }

    useEffect(() => {
        reqGetTemplateList();
    }, [caseInfo.ID]);

    const handleTemplateChange = (e) => {
        setTemplateID(e.target.value);
        reqGetTemplateEQuest(e.target.value);
    }

    const handleInputChange = (e) => {
        const target = e.target;
        setTemplateQuestValue(prev => ({ ...prev, [target.name]: target.value }));
    }

    const handleReportExport = (e) => {
        e.preventDefault();
        reqExportReport();
    }

    let questElements = [];
    templateQuest.quests.map(x => {
        questElements.push(<TextInput label={x.QuestText} name={x.QuestName} handleOnChange={handleInputChange} value={templateQuestValue[x.QuestName]} type={x.QuestType} required={x.Required} />);
    });

    return (
        <div className="QForm-Wrap">
            <form onSubmit={handleReportExport}>
                <div className="QForm Layer2">
                    <div className="QIndex" />
                    <h4>匯出報告資訊</h4>
                    <Select label="報告模板" name="ReportTemplate" handleOnChange={handleTemplateChange} value={templateID} options={templateOption.options} />
                    <LoadingComponent status={templateQuest.status} fontSize="24px">
                        {questElements}
                        <button className="btn btn-default btn-block" disabled={exportReportStatus === GlobalConstants.Status.LOADING}>
                            <Loader status={exportReportStatus} />
                            匯出
                        </button>
                    </LoadingComponent>
                </div> 
            </form>    
        </div>
    );
}

const ExportReport = (props) => {
    return (
        <WorkSpaceMain match={props.match} window={<ExportReportWindow />} toolbar={<ExportReportToolbar />} />
    )
}

export default ExportReport;