import React, { useState } from 'react';
import PropType from 'prop-types';
import _ from 'lodash';
import TextInput from '../ReportItem/TextInput';
import TextArea from '../ReportItem/TextArea';
import Select from '../ReportItem/Select';
import QuestFormContainer from './QuestFormContainer';
import AutoGenerateFormContainer from './AutoGenerateFormContainer';
import CheckBox from '../ReportItem/CheckBox';
import TextQuest from './Quests/TextQuest';
import NumberQuest from './Quests/NumberQuest';
import SelectQuest from './Quests/SelectQuest';
import MultiSelectQuest from './Quests/MultiSelectQuest';
import FileQuest from './Quests/FileQuest';
import { useDragAndDrop } from '../../CustomHook/useDragAndDrop';
import DeleteButton from './Quests/DeleteButton';
import { initialQuestState } from './ReportCreator';
import Scoresum from './Quests/Scoresum';

const questTypes = [
        { text: "文字", value: "text", optionFlag: false },
        { text: "數值", value: "number", optionFlag: false },
        { text: "下拉", value: "select", optionFlag: true },
        { text: "單選", value: "radio", optionFlag: true },
        { text: "多選", value: "checkbox", optionFlag: true },
        { text: "檔案", value: "file", optionFlag: false },
        { text: "PDF", value: "pdf", optionFlag: false },
    ];

const allGroupType = [
    { text: "一般群組", value: "Group" },
    { text: "計分群組", value: "ScoreGroup"}
]

const QuestGroupForm = (props) => {
    const { onDragStart, onDragOver, onDrop, onDragEnd, onMouseDown, onDrag, dragAndDrop } = useDragAndDrop(props.groupData.Children, props.handleChangeQuestSortOrder, props.container.current);

    const createQuestionList = () => {
        return props.groupData.Children.map((value, index) => {
            return createQuest(value.QuestionType, value, index);
        })
    }

    const createQuest = (questType, questData, index) => {
        let args = {
            key: questData.uniqueID,
            index: index,
            groupIndex: props.index,
            selectedComponent: props.selectedComponent,
            handleDeleteQuest: props.handleDeleteQuest,
            handleImageChange: props.handleFileChange,
            handleOnChange: props.handleOnChange,
            handleAddOption: props.handleAddOption,
            handleDeleteOption: props.handleDeleteOption,
            handleChangeOptionSortOrder: props.handleChangeOptionSortOrder,
            handleClickComponent: props.handleClickComponent,
            onDragStart, 
            onDragOver, 
            onDrop, 
            onDragEnd, 
            onMouseDown,
            onDrag,
            dragAndDrop,
            ...questData
        };
        switch(questType) {
            case "text":
                return <TextQuest {...args} />;
            case "number":
                return <NumberQuest {...args} />;
            case "select":
            case "radio":
            case "checkbox":
                return <SelectQuest {...args} />;
            case "file":
            case "pdf":
                return <FileQuest {...args} />;
            case "scoresum":
                return <Scoresum {...args} />
                break;
        }
    }

    const handleGroupTypeChange = (e) => {
        if (e.target.value === "ScoreGroup") {
            let insertQuest = _.cloneDeep(initialQuestState);
            insertQuest.QuestionType = "scoresum";
            insertQuest.QuestionText = "總分";
            props.handleAddQuest(e, props.index, -1, insertQuest);
        }
        else {
            props.handleDeleteQuest(e, props.index, 0);
        }
        props.handleOnChange(e, { groupIndex: props.index });
    }

    let questions = createQuestionList();

    let title = "問題群組";
    if (props.groupData.type == "ScoreGroup") {
        title = "計分群組";
    }

    let indexGroup = { groupIndex: props.index };

    return (
        <>
            <div
                className={`QForm Layer2 draggable ${props.className}`}
                draggable
                onDragStart={props.onDragStart}
                onDrag={props.onDrag}
                onDragOver={props.onDragOver}
                onDrop={props.onDrop}
                onDragEnd={props.onDragEnd}
                onMouseDown={props.onMouseDown}
                onClick={(e) => props.handleClickComponent(e, indexGroup)}
                data-position={props.index}
                data-id={props.groupData.uniqueID}
                data-type="Group"
            >
                <div className="handle"></div>
                <div className="content">
                    <div className="QIndex" />
                    <TextInput
                        placeholder="群組標題"
                        required={true}
                        colwidth={80}
                        name={`Title`}
                        inputProps={{ maxLength: "1000" }}
                        handleOnChange={(e) => props.handleOnChange(e, { groupIndex: props.index })}
                        value={props.groupData.Title}
                    />
                    <Select colwidth={20} name="Type" handleOnChange={handleGroupTypeChange} value={props.groupData.Type} options={allGroupType} style={{float : "none"}} />
                    <DeleteButton handleOnClick={(e) => props.handleDeleteGroup(e, props.index)} />
                    {/* <button className="close fa fa-times" type="button" style={{ float: "right", fontSize: "20px" }} onClick={e => props.handleDeleteGroup(e, props.index)}></button> */}
                    <div className="formContent quest-group-editor-container">
                        <TextArea
                            placeholder="群組描述"
                            rows={3}
                            name="Description"
                            textAreaProps={{ maxLength: "3000" }}
                            handleOnChange={(e) => props.handleOnChange(e, { groupIndex: props.index })}
                            value={props.groupData.Description}
                        />
                        {questions}
                    </div>
                </div>
            </div>
        </>
    );
}

QuestGroupForm.defaultProps = {
    questType: ""
}

QuestGroupForm.propTypes = {
    questions: PropType.array,
    index: PropType.number,
    handleOnChange: PropType.func,
    handleAddOption: PropType.func,
    groupTitle: PropType.string,
    questType: PropType.string,
    // 群組類型
    type: PropType.string
};

export default QuestGroupForm;