import React, { useRef, useState } from "react";
import { useParams, useHistory } from "react-router-dom";
import _ from "lodash";

import QuestionGroup from "../ReportItem/QuestionGroup";
import TextInput from "../ReportItem/TextInput";
import { usePostAuth } from "../../CustomHook/CustomHook";
import { GetDate, GlobalConstants } from "../../Utilities/CommonUtility";
import QuestGroupForm from "./QuestGroupForm";
import produce from "immer";
import { useAuthEffect } from "../../CustomHook/useAuthEffect";
import { CreateUUID } from "../../Utilities/UUID";
import { IsNotNullOrEmpty, IsNullOrEmpty } from "../../Utilities/StringUtility";
import { getClass } from "../../Utilities/ClassUtility";
import { useDragAndDrop } from "../../CustomHook/useDragAndDrop";
import OptionAttr from "./QuestAttrs/OptionAttr";
import QuestAttr from "./QuestAttrs/QuestAttr";
import TextArea from "../ReportItem/TextArea";

const initialValidation = {
    Operator: "",
    Value: "",
    Color: "",
    Normal: false,
    GroupNum: null,
};

const initialValidationGroup = {
    ID: null,
    GroupNum: null,
    AttributeName: "",
    Operator1: "",
    Value1: "",
    Operator2: null,
    Value2: null,
    ValidationList: [],
};

const initialSelectedComponent = {
    index: null,
    id: null,
    indexGroup: {},
    type: null,
};

const initialOptionState = {
    OptionText: "",
    Value: "",
    CodingBookIndex: 0,
    HiddenFromBackend: false,
    AbnormalValue: false,
};

export const initialQuestState = {
    Type: "Question",
    QuestionType: "text",
    QuestionText: "",
    Description: "",
    AnswerOption: [],
    Required: false,
    OtherAns: false,
    OtherAnsID: null,
    Image: null,
    CodingBookIndex: 0,
    QuestionNo: "",
    Pin: false,
    ValidationGroupList: [initialValidationGroup],
};

const initialGroupState = {
    Type: "Group",
    Title: "",
    Description: "",
    Children: [],
};

const initailReportStructure = {
    Title: "",
    Category: "",
    Description: "",
    Children: [],
    IndexNum: 1,
};

const ReportCreator = (props) => {
    // reportMID: Report Main ID, category: 表單類別, sourceReportID: 來源表單ID
    const { reportMID, category, sourceReportID } = useParams();
    // 忽略非同步要求回傳內容
    const ignoreAsyncReponseRef = useRef(false);

    // 是否已發佈
    const [reportInfo, setReportInfo] = useState({
        Publish: false,
    });
    // 表單問題結構
    const [reportStructure, setReportStructure] = useState({ ...initailReportStructure, Category: category });
    // 預定發佈時間
    const [reserveDate, setReserveDate] = useState(GetDate(new Date(), null, "-"));
    // 檔案
    const [fileList, setFileList] = useState({});

    const [saveDisabled, setSaveDisabled] = useState(true);

    const [selectedComponent, setSelectedComponent] = useState(initialSelectedComponent);

    const { PostWithAuth, PostWithAuthFormData } = usePostAuth();

    const { onDragStart, onDrag, onDragOver, onDrop, onDragEnd, onMouseDown, dragAndDrop, container } = useDragAndDrop(reportStructure.Children, setList);

    const history = useHistory();

    const setList = (data) => {
        setReportStructure((prev) => ({ ...prev, Children: data }));
    };

    useAuthEffect(() => {
        ignoreAsyncReponseRef.current = false;

        if (IsNotNullOrEmpty(reportMID)) {
            getReportStructure();
        } else if (IsNotNullOrEmpty(sourceReportID)) {
            getSourceReportStructure();
        }

        return () => {
            ignoreAsyncReponseRef.current = true;
        };
    }, [reportMID, sourceReportID]);

    // 取得點選表單的問題結構
    const getReportStructure = () => {
        GetReportMain(reportMID).then((response) => {
            if (ignoreAsyncReponseRef.current) return;

            ParseReportStructure(response.Data.OutputJson);
            setReportInfo({
                Publish: response.Data.isPublish,
            });
        });
    };

    // 取得來源表單問題結構
    const getSourceReportStructure = () => {
        GetReportMain(sourceReportID).then((response) => {
            if (ignoreAsyncReponseRef.current) return;

            ParseReportStructure(response.Data.OutputJson);
            setReportInfo({
                Publish: false,
            });
        });
    };

    // 取得Report Main資料
    const GetReportMain = (reportID) => {
        return PostWithAuth({
            url: "/Report/GetReportMain",
            data: {
                ID: reportID,
                FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                AuthCode: 1,
            },
        });
    };

    // 轉換回傳格式
    const ParseReportStructure = (structureJson) => {
        let tempReportStructure = JSON.parse(structureJson);
        tempReportStructure.Children = SetChildUniqueID(tempReportStructure.Children);
        setReportStructure(tempReportStructure);
    };

    // 將結構中的每一個物件加上 GUID，在拖曳時用來辨別
    const SetChildUniqueID = (children) => {
        return children.map((child) => {
            child.uniqueID = CreateUUID();

            if (Array.isArray(child.Children)) {
                child.Children = SetChildUniqueID(child.Children);
            }

            if (Array.isArray(child.AnswerOption)) {
                child.AnswerOption = SetChildUniqueID(child.AnswerOption);
            }

            return child;
        });
    };

    //匯出資料表，並存入DB
    const handleSaveReportClick = (e) => {
        e.preventDefault();

        new Promise(SaveReport).then((value) => {
            alert("儲存成功!");
            refreshReportTab();
            //RedirectToReportList();
            history.replace(`/ReportCreator/${category}/${value.Structure.ID}`);
        });

        setSaveDisabled(true);
    };

    // 發佈表單
    const handlePublishReport = (e) => {
        if (!confirm("警告!表單發佈後無法再修改，確定發佈?")) return;

        new Promise(SaveReport).then((response) => {
            let id = response.ID || response.Structure.ID;
            PostWithAuth({
                url: "/Report/PublishReport",
                data: {
                    ID: id,
                    FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                    AuthCode: 1,
                },
                success: (response) => {
                    alert("發佈成功");
                    refreshReportTab();
                    RedirectToReportList();
                },
            });
        });
    };

    // 儲存表單
    const SaveReport = (resolve, reject) => {
        let requestPromiseAry = [];

        if (IsNotNullOrEmpty(reportMID)) {
            requestPromiseAry.push(getUpdateReportRequest());
        } else {
            // 判斷是否為新版本
            let newVersionFlag = false;
            if (IsNotNullOrEmpty(category)) newVersionFlag = true;

            requestPromiseAry.push(getAddReportRequest(newVersionFlag));
        }

        requestPromiseAry.push(...getAddFileRequestList());

        //接續發送 Request
        requestPromiseAry
            .reduce((prev, current) => prev.then(current), Promise.resolve())
            .then((response) => {
                resolve(response);
            });
    };

    // 取得更新表單 Request
    const getUpdateReportRequest = () => {
        return () => {
            return PostWithAuth({
                url: "/Report/UpdateReportMain",
                data: {
                    Structure: reportStructure,
                    ReserveDate: reserveDate,
                    FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                    AuthCode: 1,
                },
            });
        };
    }

    // 取得新增表單 Request
    const getAddReportRequest = (newVersionFlag) => {
        return () => {
            return PostWithAuth({
                url: "/Report/AddReportMain",
                data: {
                    Structure: reportStructure,
                    IsNewVersion: newVersionFlag,
                    ReserveDate: reserveDate,
                    FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                    AuthCode: 1,
                },
            });
        }
    }

    // 取得新增檔案 Request 清單
    const getAddFileRequestList = () => {
        let result = [];
        let fileKeys = Object.keys(fileList);
        fileKeys.forEach(fileKey => {
            let [groupIndex, questIndex] = fileKey.split("_");
            result.push((response) => {
                if (fileList[fileKey] == null) return;

                let formData = new FormData();
                formData.append("RQID", response.Structure.Children[groupIndex].Children[questIndex].ID);
                formData.append("File", fileList[fileKey]);
                formData.append("FuncCode", GlobalConstants.FuncCode.ViewWebsite);
                formData.append("AuthCode", 1);

                return new Promise((success, reject) => {
                    PostWithAuthFormData({
                        url: "/Report/AddReportMainFile",
                        data: formData,
                        success: (response) => {
                            success(response);
                        },
                    });
                });
            });
        })

        return result;
    }

    // 刪除表單
    const handleDeleteReport = (e) => {
        if (!confirm("警告!表單刪除後將永久消失，確定刪除表單?")) return;

        PostWithAuth({
            url: "/Report/DeleteReportMain",
            data: {
                ID: reportMID,
                FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                AuthCode: 1,
            },
            success: (response) => {
                alert("刪除成功");
                refreshReportTab();
                // 導回問卷清單頁面
                RedirectToReportList();
            },
        });
    };

    const refreshReportTab = () => {
        const currentTime = new Date().getTime();
        // 刷新 Tab
        props.setRefreshReport(currentTime);
    };

    const RedirectToReportList = () => {
        history.replace("/ReportCreator");
    };

    // 以此版本新增新版本表單
    const handleAddNewVersion = (e) => {
        history.push(`/ReportCreator/${category}/New/${reportMID}`);
    };

    // 標記點擊元件，標記元件的屬性會顯示在屬性視窗
    const handleClickComponent = (e, indexGroup) => {
        const index = parseInt(e.currentTarget.dataset.position);
        const id = e.currentTarget.dataset.id;
        const type = e.currentTarget.dataset.type;
        setSelectedComponent({
            index,
            id,
            indexGroup,
            type,
        });
    };

    const addQuestGroup = (e, addGroupFunc) => {
        let index = reportStructure.Children.length;
        if (selectedComponent.index != null) index = selectedComponent.index;
        addGroupFunc(e, index);
    };

    const handleAddGroup = (e, index) => {
        setReportStructure((prev) => {
            let newState = produce(prev, (draft) => {
                const newGroup = _.cloneDeep(initialGroupState);
                newGroup.uniqueID = CreateUUID();
                draft.Children.splice(index + 1, 0, newGroup);
            });
            return newState;
        });
    };

    // 加入計分群組
    const handleAddScoreGroup = (e, index) => {
        setReportStructure((prev) => {
            let newState = produce(prev, (draft) => {
                let sumQuest = _.cloneDeep(initialQuestState);
                sumQuest.Type = "AutoReact";
                sumQuest.QuestionType = "scoresum";
                sumQuest.QuestionText = "總分";

                let newGroup = _.cloneDeep(initialGroupState);
                newGroup.Type = "ScoreGroup";
                newGroup.uniqueID = CreateUUID();
                newGroup.push(sumQuest);
                draft.Children.splice(index, 0, newGroup);
            });
            return newState;
        });
    };

    // 刪除群組
    const handleDeleteGroup = (e, index) => {
        e.stopPropagation();
        setReportStructure((prev) => {
            let newState = produce(prev, (draft) => {
                draft.Children.splice(index, 1);
            });
            return newState;
        });
        clearSelect();
    };

    // 問題圖片變更
    const handleFileChange = (e, indexGroup) => {
        let { value } = e.target;
        let { groupIndex, questIndex } = indexGroup;
        let key = `${groupIndex}_${questIndex}`;

        setFileList((prev) => {
            const newState = produce(prev, (draft) => {
                draft[key] = value;
            });
            return newState;
        });
    };

    const handleAddQuest = (e, groupIndex, questIndex, quest) => {
        if (groupIndex == null) return;

        if (questIndex == null) questIndex = reportStructure.Children[groupIndex].Children.length;
        else questIndex += 1;
        setReportStructure((prev) => {
            const newState = produce(prev, (draft) => {
                let questTemplate = quest || initialQuestState;
                let newQuest = _.cloneDeep(questTemplate);
                newQuest.uniqueID = CreateUUID();
                draft.Children[groupIndex].Children.splice(questIndex, 0, newQuest);
            });
            return newState;
        });
    };

    // 變更問題排序
    const handleChangeQuestOrder = (groupIndex, questList) => {
        setReportStructure((prev) => {
            const newState = produce(prev, (draft) => {
                draft.Children[groupIndex].Children = questList;
            });
            return newState;
        });
    };

    // 刪除題目
    const handleDeleteQuest = (e, groupIndex, questIndex) => {
        e.stopPropagation();
        setReportStructure((prev) => {
            let newState = produce(prev, (draft) => {
                draft.Children[groupIndex].Children.splice(questIndex, 1);
            });
            return newState;
        });
        clearSelect();
    };

    // 新增選項
    const handleAddOption = (e, pushArgs) => {
        let { groupIndex, questIndex, optionIndex } = pushArgs;
        setReportStructure((prev) => {
            const newState = produce(prev, (draft) => {
                let newOption = _.cloneDeep(initialOptionState);
                newOption.uniqueID = CreateUUID();
                draft.Children[groupIndex].Children[questIndex].AnswerOption.splice(optionIndex, 0, newOption);
            });
            return newState;
        });
    };

    // 變更選項排序
    const handleChangeOptionOrder = (optionList, indexes) => {
        setReportStructure((prev) => {
            const newState = produce(prev, (draft) => {
                draft.Children[indexes.groupIndex].Children[indexes.questIndex].AnswerOption = optionList;
            });
            return newState;
        });
    };

    // 刪除問題選項
    const handleDeleteOption = (e, pushArgs) => {
        e.stopPropagation();
        const { groupIndex, questIndex, optionIndex } = pushArgs;
        setReportStructure((prev) => {
            const newState = produce(prev, (draft) => {
                draft.Children[groupIndex].Children[questIndex].AnswerOption.splice(optionIndex, 1);
            });
            return newState;
        });
        clearSelect();
    };

    // 清除選取元件
    const clearSelect = () => {
        setSelectedComponent(initialSelectedComponent);
    };

    // 新增標準值群組
    const handleAddValidationGroup = (e, indexGroup) => {
        let { groupIndex, questIndex } = indexGroup;
        setReportStructure((prev) => {
            return produce(prev, (draft) => {
                let newValidation = _.cloneDeep(initialValidationGroup);
                draft.Children[groupIndex].Children[questIndex].ValidationGroupList.push(newValidation);
            });
        });
    };

    // 刪除標準值群組
    const handleDeleteValidationGroup = (e, indexGroup) => {
        let { groupIndex, questIndex, validationGroupIndex } = indexGroup;
        setReportStructure((prev) => {
            return produce(prev, (draft) => {
                draft.Children[groupIndex].Children[questIndex].ValidationGroupList.splice(validationGroupIndex, 1);
            });
        });
    };

    // 變更標準值數量
    const handleChangeValidationAmount = (e, updateArgs, total) => {
        const { groupIndex, questIndex, validationGroupIndex } = updateArgs;
        setReportStructure((prev) => {
            return produce(prev, (draft) => {
                let validationList = [];
                for (let i = 0; i < total; i++) validationList.push(initialValidation);
                draft.Children[groupIndex].Children[questIndex].ValidationGroupList[validationGroupIndex].ValidationList = validationList;
            });
        });
    };

    // 處理表單中任何屬性變更，indexGroup為要變更的屬性索引
    const handleInputOnChange = (e, indexGroup) => {
        const { name, value } = e.target;
        const { groupIndex, questIndex, optionIndex, validationGroupIndex, validationIndex } = indexGroup || {};

        setSaveDisabled(false);

        setReportStructure((prev) => {
            let newState = produce(prev, (draft) => {
                // 改變是問題類型，除了所有類型題目共用的屬性其餘屬性皆設定為初始值
                if (name === "QuestionType") {
                    let newQuest = copyQuestCommonAttr(draft.Children[groupIndex].Children[questIndex]);
                    draft.Children[groupIndex].Children[questIndex] = newQuest;
                }

                if (indexGroup == null) {
                    // 變更表單屬性
                    draft[name] = value;
                } else if (validationIndex != null) {
                    // 變更標準值屬性
                    draft.Children[groupIndex].Children[questIndex].ValidationGroupList[validationGroupIndex].ValidationList[validationIndex][name] = value;
                } else if (validationGroupIndex != null) {
                    // 變更標準值群組屬性
                    draft.Children[groupIndex].Children[questIndex].ValidationGroupList[validationGroupIndex][name] = value;
                } else if (optionIndex != null) {
                    // 變更選項屬性
                    draft.Children[groupIndex].Children[questIndex].AnswerOption[optionIndex][name] = value;
                } else if (questIndex != null) {
                    // 變更問題屬性
                    draft.Children[groupIndex].Children[questIndex][name] = value;
                } else if (groupIndex != null) {
                    // 變更群組屬性
                    draft.Children[groupIndex][name] = value;
                }
            });
            return newState;
        });
    };

    // 複製問題共通屬性
    const copyQuestCommonAttr = (source) => {
        let newQuest = _.cloneDeep(initialQuestState);
        newQuest.uniqueID = source.uniqueID;
        newQuest.QuestionText = source.QuestionText;
        newQuest.Description = source.Description;
        newQuest.Required = source.Required;
        newQuest.QuestionNo = source.QuestionNo;
        // newQuest.Pin = source.Pin;
        newQuest.CodingBookIndex = source.CodingBookIndex;
        return newQuest;
    };

    // 預定時間改變變更state
    const hanldeReserveDateChange = (e) => {
        setReserveDate(e.target.value);
    };

    // 建立問題群組元件清單
    const createGroupElementList = () => {
        return reportStructure.Children.map((group, index) => {
            let className = "";
            // 正在拖曳元件將所有群組加上 dragging class
            if (dragAndDrop.isDragging) className += "dragging ";
            // 如果與目前拖曳目標索引值一致加上 dragTo
            if (dragAndDrop.draggedTo === index) className += "dragTo ";
            // 正在選擇的群組 GUID 與目前一致加上 selected
            if (selectedComponent.id === group.uniqueID) className += "selected ";

            return (
                <QuestGroupForm
                    key={group.uniqueID}
                    groupData={group}
                    index={index}
                    className={className}
                    selectedComponent={selectedComponent}
                    container={container}
                    handleFileChange={handleFileChange}
                    handleDeleteOption={handleDeleteOption}
                    handleDeleteQuest={handleDeleteQuest}
                    handleDeleteGroup={handleDeleteGroup}
                    handleOnChange={handleInputOnChange}
                    handleAddQuest={handleAddQuest}
                    handleAddOption={handleAddOption}
                    handleChangeValidationAmount={handleChangeValidationAmount}
                    handleChangeQuestSortOrder={(data) => handleChangeQuestOrder(index, data)}
                    handleChangeOptionSortOrder={handleChangeOptionOrder}
                    handleClickComponent={handleClickComponent}
                    onDragStart={onDragStart}
                    onDrag={onDrag}
                    onDragOver={onDragOver}
                    onDrop={onDrop}
                    onDragEnd={onDragEnd}
                    onMouseDown={onMouseDown}
                />
            );
        });
    }

    // 建立屬性元件
    const createAttributeElement = () => {
        let result = null;
        let attrArgs = {
            indexGroup: selectedComponent.indexGroup,
            reportStructure: reportStructure,
            handleOnChange: handleInputOnChange,
            handleAddValidationGroup: handleAddValidationGroup,
            handleDeleteValidationGroup: handleDeleteValidationGroup,
            handleChangeValidationAmount: handleChangeValidationAmount,
        };
        // 根據選擇的元件類型建立屬性元件
        switch (selectedComponent.type) {
            case "Quest":
                result = <QuestAttr {...attrArgs} />;
                break;
            case "Option":
                result = <OptionAttr {...attrArgs} />;
                break;
        }
        return result;
    }

    let groupElementList = createGroupElementList();

    let attributeElement = createAttributeElement();

    return (
        <>
            <div className="ui-toolBar-Wrap">
                <div className="ui-toolBar-Group ui-Col-10">
                    <label>表單版本工具</label>
                    <div className="ui-buttonGroup">
                        <button disabled={IsNullOrEmpty(reportMID)} onClick={handleAddNewVersion} className="offsetLeft-5" data-tooltip="以此表單新增新版表單">
                            <i className="fa fa-plus-square" aria-hidden="true" />
                            新增版本
                        </button>
                    </div>
                </div>
                <div className="ui-toolBar-Group ui-Col-20">
                    <label>表單編輯工具</label>
                    <div className="ui-buttonGroup">
                        <button form="SurveyBuild" className="offsetLeft-5" data-tooltip="儲存表單" disabled={saveDisabled}>
                            <i className="fa fa-floppy-o" aria-hidden="true" />
                            儲存
                        </button>
                        <button disabled={reportInfo.Publish} onClick={handlePublishReport} className="offsetLeft-5" data-tooltip="發佈表單給使用者填寫">
                            <i className="fa fa-file-o" aria-hidden="true" />
                            發佈
                        </button>
                        <button disabled={IsNullOrEmpty(reportMID)} onClick={handleDeleteReport} className="offsetLeft-5" data-tooltip="刪除表單">
                            <i className="fa fa-times" aria-hidden="true" />
                            刪除
                        </button>
                    </div>
                </div>
            </div>

            <div ref={container} className="QForm-Wrap quest-form-container">
                <form className="QForm-Content quest-form" id="SurveyBuild" onSubmit={handleSaveReportClick}>
                    <fieldset className={getClass(reportInfo.Publish, "ev-lockForm")}>
                        <QuestionGroup title="表單標題" className="report-property-group">
                            <TextInput name="Title" value={reportStructure.Title} label="標題" colwidth={100} inputProps={{ required: true, maxLength: "1000" }} handleOnChange={handleInputOnChange} />
                            <TextInput name="IndexNum" value={reportStructure.IndexNum} type="number" label="排序編號" colwidth={100} required={true} handleOnChange={handleInputOnChange} />
                            <TextArea
                                type={1}
                                name="Description"
                                value={reportStructure.Description}
                                rows={2}
                                label="表單描述"
                                textAreaProps={{ maxLength: "3000" }}
                                handleOnChange={handleInputOnChange}
                            />
                            <TextInput
                                type="date"
                                colwidth={100}
                                label="預定發佈時間"
                                value={reserveDate}
                                inputProps={{ min: GetDate(new Date(), "", "-") }}
                                handleOnChange={hanldeReserveDateChange}
                            />
                        </QuestionGroup>
                        {groupElementList}
                    </fieldset>
                </form>
                <div className={`QForm-Content quest-attr ${getClass(reportInfo.Publish, "ev-lockForm")}`} style={{ height: "100%" }}>
                    <div>
                        <button type="button" className="btn btn-default btn-component" onClick={(e) => addQuestGroup(e, handleAddGroup)}>
                            新增群組
                        </button>
                        <button
                            type="button"
                            className="btn btn-default btn-component"
                            onClick={(e) => handleAddQuest(e, selectedComponent.indexGroup.groupIndex, selectedComponent.indexGroup.questIndex)}
                        >
                            新增問題
                        </button>
                    </div>
                    <QuestionGroup title="屬性" style={{ maxHeight: "calc(100% - 20px)", overflowY: "auto" }}>
                        {attributeElement || <div className="text-center">無屬性</div>}
                    </QuestionGroup>
                </div>
            </div>
        </>
    );
};

export default ReportCreator;
