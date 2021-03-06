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
    // reportMID: Report Main ID, category: ????????????, sourceReportID: ????????????ID
    const { reportMID, category, sourceReportID } = useParams();
    // ?????????????????????????????????
    const ignoreAsyncReponseRef = useRef(false);

    // ???????????????
    const [reportInfo, setReportInfo] = useState({
        Publish: false,
    });
    // ??????????????????
    const [reportStructure, setReportStructure] = useState({ ...initailReportStructure, Category: category });
    // ??????????????????
    const [reserveDate, setReserveDate] = useState(GetDate(new Date(), null, "-"));
    // ??????
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

    // ?????????????????????????????????
    const getReportStructure = () => {
        GetReportMain(reportMID).then((response) => {
            if (ignoreAsyncReponseRef.current) return;

            ParseReportStructure(response.Data.OutputJson);
            setReportInfo({
                Publish: response.Data.isPublish,
            });
        });
    };

    // ??????????????????????????????
    const getSourceReportStructure = () => {
        GetReportMain(sourceReportID).then((response) => {
            if (ignoreAsyncReponseRef.current) return;

            ParseReportStructure(response.Data.OutputJson);
            setReportInfo({
                Publish: false,
            });
        });
    };

    // ??????Report Main??????
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

    // ??????????????????
    const ParseReportStructure = (structureJson) => {
        let tempReportStructure = JSON.parse(structureJson);
        tempReportStructure.Children = SetChildUniqueID(tempReportStructure.Children);
        setReportStructure(tempReportStructure);
    };

    // ???????????????????????????????????? GUID???????????????????????????
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

    //???????????????????????????DB
    const handleSaveReportClick = (e) => {
        e.preventDefault();

        new Promise(SaveReport).then((value) => {
            alert("????????????!");
            refreshReportTab();
            //RedirectToReportList();
            history.replace(`/ReportCreator/${category}/${value.Structure.ID}`);
        });

        setSaveDisabled(true);
    };

    // ????????????
    const handlePublishReport = (e) => {
        if (!confirm("??????!??????????????????????????????????????????????")) return;

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
                    alert("????????????");
                    refreshReportTab();
                    RedirectToReportList();
                },
            });
        });
    };

    // ????????????
    const SaveReport = (resolve, reject) => {
        let requestPromiseAry = [];

        if (IsNotNullOrEmpty(reportMID)) {
            requestPromiseAry.push(getUpdateReportRequest());
        } else {
            // ????????????????????????
            let newVersionFlag = false;
            if (IsNotNullOrEmpty(category)) newVersionFlag = true;

            requestPromiseAry.push(getAddReportRequest(newVersionFlag));
        }

        requestPromiseAry.push(...getAddFileRequestList());

        //???????????? Request
        requestPromiseAry
            .reduce((prev, current) => prev.then(current), Promise.resolve())
            .then((response) => {
                resolve(response);
            });
    };

    // ?????????????????? Request
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

    // ?????????????????? Request
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

    // ?????????????????? Request ??????
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

    // ????????????
    const handleDeleteReport = (e) => {
        if (!confirm("??????!????????????????????????????????????????????????????")) return;

        PostWithAuth({
            url: "/Report/DeleteReportMain",
            data: {
                ID: reportMID,
                FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                AuthCode: 1,
            },
            success: (response) => {
                alert("????????????");
                refreshReportTab();
                // ????????????????????????
                RedirectToReportList();
            },
        });
    };

    const refreshReportTab = () => {
        const currentTime = new Date().getTime();
        // ?????? Tab
        props.setRefreshReport(currentTime);
    };

    const RedirectToReportList = () => {
        history.replace("/ReportCreator");
    };

    // ?????????????????????????????????
    const handleAddNewVersion = (e) => {
        history.push(`/ReportCreator/${category}/New/${reportMID}`);
    };

    // ??????????????????????????????????????????????????????????????????
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

    // ??????????????????
    const handleAddScoreGroup = (e, index) => {
        setReportStructure((prev) => {
            let newState = produce(prev, (draft) => {
                let sumQuest = _.cloneDeep(initialQuestState);
                sumQuest.Type = "AutoReact";
                sumQuest.QuestionType = "scoresum";
                sumQuest.QuestionText = "??????";

                let newGroup = _.cloneDeep(initialGroupState);
                newGroup.Type = "ScoreGroup";
                newGroup.uniqueID = CreateUUID();
                newGroup.push(sumQuest);
                draft.Children.splice(index, 0, newGroup);
            });
            return newState;
        });
    };

    // ????????????
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

    // ??????????????????
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

    // ??????????????????
    const handleChangeQuestOrder = (groupIndex, questList) => {
        setReportStructure((prev) => {
            const newState = produce(prev, (draft) => {
                draft.Children[groupIndex].Children = questList;
            });
            return newState;
        });
    };

    // ????????????
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

    // ????????????
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

    // ??????????????????
    const handleChangeOptionOrder = (optionList, indexes) => {
        setReportStructure((prev) => {
            const newState = produce(prev, (draft) => {
                draft.Children[indexes.groupIndex].Children[indexes.questIndex].AnswerOption = optionList;
            });
            return newState;
        });
    };

    // ??????????????????
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

    // ??????????????????
    const clearSelect = () => {
        setSelectedComponent(initialSelectedComponent);
    };

    // ?????????????????????
    const handleAddValidationGroup = (e, indexGroup) => {
        let { groupIndex, questIndex } = indexGroup;
        setReportStructure((prev) => {
            return produce(prev, (draft) => {
                let newValidation = _.cloneDeep(initialValidationGroup);
                draft.Children[groupIndex].Children[questIndex].ValidationGroupList.push(newValidation);
            });
        });
    };

    // ?????????????????????
    const handleDeleteValidationGroup = (e, indexGroup) => {
        let { groupIndex, questIndex, validationGroupIndex } = indexGroup;
        setReportStructure((prev) => {
            return produce(prev, (draft) => {
                draft.Children[groupIndex].Children[questIndex].ValidationGroupList.splice(validationGroupIndex, 1);
            });
        });
    };

    // ?????????????????????
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

    // ????????????????????????????????????indexGroup???????????????????????????
    const handleInputOnChange = (e, indexGroup) => {
        const { name, value } = e.target;
        const { groupIndex, questIndex, optionIndex, validationGroupIndex, validationIndex } = indexGroup || {};

        setSaveDisabled(false);

        setReportStructure((prev) => {
            let newState = produce(prev, (draft) => {
                // ????????????????????????????????????????????????????????????????????????????????????????????????
                if (name === "QuestionType") {
                    let newQuest = copyQuestCommonAttr(draft.Children[groupIndex].Children[questIndex]);
                    draft.Children[groupIndex].Children[questIndex] = newQuest;
                }

                if (indexGroup == null) {
                    // ??????????????????
                    draft[name] = value;
                } else if (validationIndex != null) {
                    // ?????????????????????
                    draft.Children[groupIndex].Children[questIndex].ValidationGroupList[validationGroupIndex].ValidationList[validationIndex][name] = value;
                } else if (validationGroupIndex != null) {
                    // ???????????????????????????
                    draft.Children[groupIndex].Children[questIndex].ValidationGroupList[validationGroupIndex][name] = value;
                } else if (optionIndex != null) {
                    // ??????????????????
                    draft.Children[groupIndex].Children[questIndex].AnswerOption[optionIndex][name] = value;
                } else if (questIndex != null) {
                    // ??????????????????
                    draft.Children[groupIndex].Children[questIndex][name] = value;
                } else if (groupIndex != null) {
                    // ??????????????????
                    draft.Children[groupIndex][name] = value;
                }
            });
            return newState;
        });
    };

    // ????????????????????????
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

    // ????????????????????????state
    const hanldeReserveDateChange = (e) => {
        setReserveDate(e.target.value);
    };

    // ??????????????????????????????
    const createGroupElementList = () => {
        return reportStructure.Children.map((group, index) => {
            let className = "";
            // ??????????????????????????????????????? dragging class
            if (dragAndDrop.isDragging) className += "dragging ";
            // ???????????????????????????????????????????????? dragTo
            if (dragAndDrop.draggedTo === index) className += "dragTo ";
            // ????????????????????? GUID ????????????????????? selected
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

    // ??????????????????
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
        // ?????????????????????????????????????????????
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
                    <label>??????????????????</label>
                    <div className="ui-buttonGroup">
                        <button disabled={IsNullOrEmpty(reportMID)} onClick={handleAddNewVersion} className="offsetLeft-5" data-tooltip="??????????????????????????????">
                            <i className="fa fa-plus-square" aria-hidden="true" />
                            ????????????
                        </button>
                    </div>
                </div>
                <div className="ui-toolBar-Group ui-Col-20">
                    <label>??????????????????</label>
                    <div className="ui-buttonGroup">
                        <button form="SurveyBuild" className="offsetLeft-5" data-tooltip="????????????" disabled={saveDisabled}>
                            <i className="fa fa-floppy-o" aria-hidden="true" />
                            ??????
                        </button>
                        <button disabled={reportInfo.Publish} onClick={handlePublishReport} className="offsetLeft-5" data-tooltip="??????????????????????????????">
                            <i className="fa fa-file-o" aria-hidden="true" />
                            ??????
                        </button>
                        <button disabled={IsNullOrEmpty(reportMID)} onClick={handleDeleteReport} className="offsetLeft-5" data-tooltip="????????????">
                            <i className="fa fa-times" aria-hidden="true" />
                            ??????
                        </button>
                    </div>
                </div>
            </div>

            <div ref={container} className="QForm-Wrap quest-form-container">
                <form className="QForm-Content quest-form" id="SurveyBuild" onSubmit={handleSaveReportClick}>
                    <fieldset className={getClass(reportInfo.Publish, "ev-lockForm")}>
                        <QuestionGroup title="????????????" className="report-property-group">
                            <TextInput name="Title" value={reportStructure.Title} label="??????" colwidth={100} inputProps={{ required: true, maxLength: "1000" }} handleOnChange={handleInputOnChange} />
                            <TextInput name="IndexNum" value={reportStructure.IndexNum} type="number" label="????????????" colwidth={100} required={true} handleOnChange={handleInputOnChange} />
                            <TextArea
                                type={1}
                                name="Description"
                                value={reportStructure.Description}
                                rows={2}
                                label="????????????"
                                textAreaProps={{ maxLength: "3000" }}
                                handleOnChange={handleInputOnChange}
                            />
                            <TextInput
                                type="date"
                                colwidth={100}
                                label="??????????????????"
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
                            ????????????
                        </button>
                        <button
                            type="button"
                            className="btn btn-default btn-component"
                            onClick={(e) => handleAddQuest(e, selectedComponent.indexGroup.groupIndex, selectedComponent.indexGroup.questIndex)}
                        >
                            ????????????
                        </button>
                    </div>
                    <QuestionGroup title="??????" style={{ maxHeight: "calc(100% - 20px)", overflowY: "auto" }}>
                        {attributeElement || <div className="text-center">?????????</div>}
                    </QuestionGroup>
                </div>
            </div>
        </>
    );
};

export default ReportCreator;
