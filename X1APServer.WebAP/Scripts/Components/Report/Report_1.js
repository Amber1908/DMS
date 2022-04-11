import React, { useContext, useEffect, useRef, useState } from "react";
import { useHistory, useParams } from "react-router-dom";
import { useAuthCheck, usePostAuth } from "../../CustomHook/CustomHook";
import { CaseInfoContext } from "../Context";

import CheckBox from "../ReportItem/CheckBox";
import QuestionGroup from "../ReportItem/QuestionGroup";
import RadioGroup from "../ReportItem/RadioGroup";
import Select from "../ReportItem/Select";
import TextInput from "../ReportItem/TextInput";
import TextArea from "../ReportItem/TextArea";
import QuestionTitle from "../ReportItem/QuestionTitle";
import TextTemplate from "../ReportItem/TextTemplate";
import WorkSpaceCommonReportMain from "./WorkSpaceCommonReportMain";
import { GlobalConstants } from "../../Utilities/CommonUtility";
import ScoreQuestGroup from "../ReportItem/ScoreQuestGroup";
import { Ajax } from "../../Utilities/AjaxUtility";
import FileInput from "../ReportItem/FileInput";
import PDFViewer from "../ReportItem/PDFViewer";
import Log from "../../Utilities/LogUtility";
import TwDatePicker from "../ReportItem/TwDatePicker";
import HospitalCodeAutosuggest from "../ReportItem/HospitalCodeAutosuggest";
import HospitalNameAutosuggest from "../ReportItem/HospitalNameAutosuggest";

const AnsType = {
    Text: 1,
    File: 2,
};

// 表單狀態
export const ReportState = {
    // 檢視
    View: "View",
    // 編輯
    Edit: "Edit",
    // 僅檢視
    ViewOnly: "ViewOnly"
};

const ReportWindow = (props) => {
    // 建造問題群組
    const createQuestGroup = (reportStructure) => {
        var groupElementList = [];

        reportStructure.Children.forEach((group, index) => {
            switch (group.Type) {
                case "ScoreGroup":
                    // 取出總和問題
                    let scoreQuest = group.Children.find((quest) => quest.QuestionType === "scoresum");
                    // 總和問題 name
                    let questName = GlobalConstants.QuestionPreFix + scoreQuest.ID;
                    groupElementList.push(
                        <ScoreQuestGroup
                            handleOnChange={props.handleInputOnChange}
                            scoreQuestValue={props.formData[questName]}
                            scoreQuestName={questName}
                            key={"ScoreGroup_" + index}
                            description={replaceLineBreakWithBrTag(group.Description)}
                            title={group.Title}
                        >
                            {createQuestList(group)}
                        </ScoreQuestGroup>
                    );
                    break;
                default:
                    groupElementList.push(
                        <QuestionGroup key={"Group_" + index} description={replaceLineBreakWithBrTag(group.Description)} title={group.Title}>
                            {createQuestList(group)}
                        </QuestionGroup>
                    );
                    break;
            }
        });

        return <div>{groupElementList}</div>;
    };

    // 替換\n為<br />
    const replaceLineBreakWithBrTag = (text = "") => {
        let oneLineTextAry = text.split("\n");
        return oneLineTextAry.map((oneLineText, index) => (
            <span key={index}>
                {oneLineText}
                <br />
            </span>
        ));
    };

    // 建立問題
    const createQuestList = (group) => {
        if (group.Children == null) return;

        var questElementList = [];

        group.Children.forEach((quest) => {
            if (quest.QuestionType === "scoresum") return;

            // 建標題
            questElementList.push(createQuestTitle(quest));
            // 建圖片
            questElementList.push(createQuestImage(quest));
            // 建描述
            questElementList.push(createQuestDescription(quest));
            // 建問題內容
            questElementList.push(...createQuestContent(quest));
        });

        return questElementList;
    };

    // 建立問題標題元件
    const createQuestTitle = (quest) => {
        if (quest.QuestionText == null) return;

        return <QuestionTitle key={"QuestText_" + quest.ID} colwidth={100} title={quest.QuestionText} />;
    };

    // 建立問題圖片元件
    const createQuestImage = (quest) => {
        if (quest.Image == null) return;

        return <img key={"Image_" + quest.ID} className="form-quest-image" src={`${Ajax.webapiBaseURL}/Report/GetReportMainFile?RQID=${quest.ID}&fileName=${quest.Image}`} />;
    };

    // 建立問題描述元件
    const createQuestDescription = (quest) => {
        if (quest.Description == null) return;

        return (
            <p key={"Desc_" + quest.ID} className="description">
                {replaceLineBreakWithBrTag(quest.Description)}
            </p>
        );
    };

    // 建立不同類型的問題
    const createQuestContent = (quest) => {
        let result = [];
        let questName = GlobalConstants.QuestionPreFix + quest.ID;
        let questValue = props.formData[questName];
        let required = !!quest.Required;
        let otherAnsName = GlobalConstants.QuestionPreFix + quest.OtherAnsID;
        let otherAnsVal = props.formData[otherAnsName];
        switch (quest.QuestionType) {
            case "date":
                result.push(<TwDatePicker key={"Ans_" + quest.ID} required={required} value={questValue} handleOnChange={props.handleInputOnChange} name={questName} colwidth={100} />);
                break;
            case "text":
            case "text(hiden)":
                result.push(
                    <TextArea
                        disabled={props.formInfo.disabled}
                        rows="1"
                        type={1}
                        key={"Ans_" + quest.ID}
                        required={required}
                        value={questValue}
                        handleOnChange={props.handleInputOnChange}
                        name={questName}
                        colwidth={100}
                    />
                );
                break;
            case "number":
                result.push(<TextInput type="number" key={"Ans_" + quest.ID} required={required} value={questValue} handleOnChange={props.handleInputOnChange} name={questName} colwidth={100} />);
                break;
            case "textarea":
                result.push(<TextArea key={"Ans_" + quest.ID} required={required} value={questValue} handleOnChange={props.handleInputOnChange} name={questName} colwidth={100} />);
                break;
            case "radio":
                var optionList = quest.AnswerOption.map((option) => ({
                    text: option.OptionText,
                    value: option.Value,
                }));
                result.push(
                    <RadioGroup
                        key={"Ans_" + quest.ID}
                        otherAnsFlag={!!quest.OtherAnsID}
                        otherAnsValue={otherAnsVal}
                        otherAnsName={otherAnsName}
                        required={required}
                        value={questValue}
                        handleOnChange={props.handleInputOnChange}
                        name={questName}
                        options={optionList}
                    />
                );
                break;
            case "select":
                var optionList = quest.AnswerOption.map((option) => ({
                    text: option.OptionText,
                    value: option.Value,
                }));

                result.push(
                    <Select
                        key={"Ans_" + quest.ID}
                        otherAnsFlag={!!quest.OtherAnsID}
                        otherAnsValue={otherAnsVal}
                        otherAnsName={otherAnsName}
                        required={required}
                        value={questValue}
                        handleOnChange={props.handleInputOnChange}
                        name={questName}
                        colwidth={100}
                        options={optionList}
                    />
                );
                break;
            case "checkbox":
                let checkboxElementList = quest.AnswerOption.map((option, index) => {
                    let checkboxName = GlobalConstants.QuestionPreFix + option.ID;
                    return (
                        <CheckBox
                            key={"Ans_" + quest.ID + "_" + index}
                            otherAnsFlag={!!quest.OtherAnsID}
                            otherAnsValue={otherAnsVal}
                            otherAnsName={otherAnsName}
                            required={required}
                            value={props.formData[checkboxName]}
                            handleOnChange={props.handleInputOnChange}
                            id={checkboxName}
                            name={checkboxName}
                            text={option.OptionText}
                            trueValue={option.Value}
                        />
                    );
                });
                result.push(...checkboxElementList);
                break;
            case "selecttextarea":
                var optionList = quest.AnswerOption.map((option) => ({
                    text: option.OptionText,
                    value: option.Value,
                }));
                result.push(<TextTemplate key={"Ans_" + quest.ID} value={questValue} name={questName} options={optionList} />);
                break;
            case "file":
                result.push(<FileInput key={"Ans_" + quest.ID} required={required} name={questName} value={questValue} handleOnChange={props.handleInputOnChange} />);
                break;
            case "pdf":
                result.push(<PDFViewer key={"Ans_" + quest.ID} required={required} name={questName} value={questValue} handleOnChange={props.handleInputOnChange} />);
                break;
            case "hospitalcode":
                result.push(<HospitalCodeAutosuggest key={"Ans_" + quest.ID} required={required} value={questValue} handleOnChange={props.handleInputOnChange} name={questName} inputProps={{ maxLength: "10" }} />);
                break;
            case "hospitalname":
                result.push(<HospitalNameAutosuggest key={"Ans_" + quest.ID} required={required} value={questValue} handleOnChange={props.handleInputOnChange} name={questName} />);
                break;
        }
        return result;
    };

    let questGroup;
    if (props.reportStructure != null) {
        questGroup = createQuestGroup(props.reportStructure);
    }

    return (
        <div className="QForm-Wrap">
            <form id="Report" onSubmit={props.handleSaveReportClick}>
                <fieldset className={props.formInfo.class}>{questGroup}</fieldset>
            </form>
        </div>
    );
};

const Report = (props) => {
    // report: 表單類別, reportID: 表單答案ID新增表單時為New
    const { reportID, report } = useParams();
    // 選擇的個案資料
    const { caseInfo } = useContext(CaseInfoContext);

    const ignoreAsyncTaskRef = useRef(false);
    // 表單ID，儲存表單用
    const reportMainIDRef = useRef(null);

    // 表單class及禁用狀態
    const [formInfo, setFormInfo] = useState({
        class: "ev-lockForm",
        disabled: true,
    });
    // 表單答案
    const [formData, setFormData] = useState({});
    // 表單結構
    const [reportStructure, setReportStructure] = useState();
    // 表單狀態(檢視、編輯)
    const [state, setState] = useState(ReportState.View);
    // 表單狀態()
    const [status, setStatus] = useState();

    const history = useHistory();

    const { PostWithAuth, PostWithAuthFormData } = usePostAuth();

    const { AuthCheck } = useAuthCheck();

    useEffect(() => {
        if (reportID == null) return;

        ignoreAsyncTaskRef.current = false;
        let getReportStructureData;

        if (isNewReport()) {
            // 新表單紀錄，取正在發布的表單結構
            getReportStructureData = {
                ReportCategory: report,
                IsPublish: true,
            };
        } else {
            // 編輯舊表單，用紀錄ID取舊表單的結構
            getReportStructureData = {
                ReportAnsMID: reportID,
            };
            // 取表單答案
            getReportAnswer();
        }
        // 取表單結構
        getReportStructure(getReportStructureData);

        return () => {
            ignoreAsyncTaskRef.current = true;
        };
    }, [reportID]);

    // 是新表單紀錄?
    const isNewReport = () => {
        return reportID === "New";
    };

    // 取得表單答案資料
    const getReportAnswer = () => {
        PostWithAuth({
            url: "/Report/GetReport",
            data: {
                ID: reportID,
                FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                AuthCode: 1,
            },
            success: (response) => {
                if (ignoreAsyncTaskRef.current) return;
                // 將答案重新組裝盛 { "問題ID": "答案" } 格式
                // 並標記答案類型
                let answers = {};
                let answerTypes = {};

                response.Answers.forEach((item) => {
                    answers[item.QuestionID] = item.Value;
                    answerTypes[item.QuestionID] = AnsType.Text;
                });

                response.Files.forEach((item) => {
                    answers[item.QuestionID] = item.FileURL;
                    answerTypes[item.QuestionID] = AnsType.File;
                });

                setStatus(response.Status > 1);

                setFormData((prev) => {
                    return { ...prev, ...answers };
                });
            },
        });
    };

    // 取得表單問題結構
    const getReportStructure = (data) => {
        // 取得Report結構
        PostWithAuth({
            url: "/Report/GetReportMain",
            data: {
                ...data,
                FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                AuthCode: 1,
            },
            success: (response) => {
                if (ignoreAsyncTaskRef.current) return;

                let tempReport = JSON.parse(response.Data.OutputJson);
                setReportStructure(tempReport);
                setReportStatus(response.FuncCode);

                reportMainIDRef.current = response.Data.ID;
            },
        });
    };

    // 依使用者報告權限設定狀態
    const setReportStatus = (reportFuncCode) => {
        if (!AuthCheck({ FuncCode: reportFuncCode, AuthNos: ["AuthNo2"] })) {
            setState(ReportState.ViewOnly);
        }
    }

    // Input 改變
    const handleInputOnChange = (e) => {
        const { name, value } = e.target;
        // name 為空忽略改變
        if (name == null) return;

        setFormData((prev) => ({
            ...prev,
            [name]: value,
        }));
    };

    // 儲存表單
    const handleSaveReportClick = (e) => {
        e.preventDefault();
        // 設定為檢視表單狀態
        setState(ReportState.View);
        // 轉換資料格式為API格式
        const { normalQuestList, fileQuestList } = prepareRequestData();

        let requestPromiseAry = [];

        if (reportID === "New") {
            // 新表單打新增表單API
            requestPromiseAry.push(createAddReportRequest(normalQuestList));
        } else {
            // 舊表單打更新表單API
            requestPromiseAry.push(createUpdateReportRequest(normalQuestList));
        }

        requestPromiseAry.push(...createAddFileRequest(fileQuestList));

        submitRequest(requestPromiseAry);
    };

    // 轉換儲存資料格式為WebAPI格式
    const prepareRequestData = () => {
        const entries = Object.entries(formData);
        // 非檔案類型答案清單
        let normalQuestList = [];
        // 檔案類型答案清單
        let fileQuestList = [];

        for (const [questID, ans] of entries) {
            // 排除非Q_開頭的id
            if (!questID.startsWith(GlobalConstants.QuestionPreFix)) {
                continue;
            }

            if (ans instanceof File) {
                fileQuestList.push({
                    QuestionID: questID,
                    Value: ans,
                });
            } else {
                normalQuestList.push({
                    QuestionID: questID,
                    Value: ans,
                });
            }
        }

        return { normalQuestList, fileQuestList };
    };

    // 建立新增表單要求
    const createAddReportRequest = (normalQuestList) => {
        return () => {
            return PostWithAuth({
                url: "/Report/AddGeneralReport",
                data: {
                    OID: -1,
                    PID: caseInfo.ID,
                    ID: reportMainIDRef.current,
                    Answers: normalQuestList,
                    FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                    AuthCode: 1,
                },
            }).then((response) => response.ReportID); // 將表單答案ID傳給下一個Promise
        };
    };

    // 建立更新表單要求
    const createUpdateReportRequest = (normalQuestList) => {
        return () => {
            return PostWithAuth({
                url: "/Report/UpdateReport",
                data: {
                    ID: reportID,
                    Answers: normalQuestList,
                    FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                    AuthCode: 1,
                },
            }).then((response) => response.ID);
        };
    };

    // 建立新增檔案要求
    const createAddFileRequest = (fileQuestList) => {
        let result = [];
        fileQuestList.forEach((fileQuest, index) => {
            result.push((reportAnsMID) => {
                let requestData = new FormData();
                requestData.append("AnswerMID", reportAnsMID);
                requestData.append("QuestionID", fileQuest.QuestionID.replace(GlobalConstants.QuestionPreFix, ""));
                requestData.append("File", fileQuest.Value);
                requestData.append("FuncCode", GlobalConstants.FuncCode.ViewWebsite);
                requestData.append("AuthCode", 1);

                return PostWithAuthFormData({
                    url: "/Report/AddReportAnsFile",
                    data: requestData,
                }).then(() => reportAnsMID);
            });
        });
        return result;
    };

    // 發送要求
    const submitRequest = (requestAry) => {
        // 要求發生錯誤?
        let requestFail = false;

        // 接續發送要求
        let result = requestAry.reduce(
            (p, fn) =>
                p.then(fn).catch((e) => {
                    console.error(e);
                    requestFail = true;
                }),
            Promise.resolve()
        );

        result.then((reportAnsMID) => {
            if (requestFail) {
                window.alert("表單儲存失敗!");
                return;
            }

            window.alert("儲存成功");

            // 新表單導致修改表單網址
            if (reportID === "New") {
                history.replace(`/Index/Patient/${caseInfo.ID}/${report}/${reportAnsMID}`);
            }
            // 鎖定表單
            setFormInfo({ class: "ev-lockForm", disabled: true });
        });
    };

    // 點編輯，解鎖表單
    const handleEditClick = (e) => {
        setState(ReportState.Edit);
        // 解除鎖定
        setFormInfo({
            class: "",
            disabled: false,
        });
    };

    return (
        <WorkSpaceCommonReportMain
            handleEditClick={handleEditClick}
            state={state}
            status={status}
            window={<ReportWindow handleSaveReportClick={handleSaveReportClick} reportStructure={reportStructure} handleInputOnChange={handleInputOnChange} formData={formData} formInfo={formInfo} />}
        />
    );
};

export default Report;
