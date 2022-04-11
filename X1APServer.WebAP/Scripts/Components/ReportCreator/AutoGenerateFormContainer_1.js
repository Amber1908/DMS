import React, { useState, useEffect } from 'react';
import PropType from 'prop-types';
import TextInput from '../ReportItem/TextInput';
import TextArea from '../ReportItem/TextArea';
import CheckBox from '../ReportItem/CheckBox';
import FileUpload from '../ReportItem/FileUpload';
import { Ajax } from '../../Utilities/AjaxUtility';
import Log from '../../Utilities/LogUtility';
import Select from '../ReportItem/Select';

const AutoGenerateFormContainer = (props) => {

    const handleAddQuest = (e) => {
        let questType = props.questTypes.filter(element => element.value == props.questType);
        props.handleAddQuest(e, props.groupIndex, questType[0].optionFlag, props.index);
    }


    let indexArg = { groupIndex: props.groupIndex, questIndex: props.index };
    Log.Debug(props);
    Log.Debug(indexArg);
    return (
        <>
            <div className="quest-editor-container" key={props.index}>
                <div className="row">
                    <TextInput className="text-editor-container" key={"QuestionNo_" + props.groupIndex + "_" + props.index} label="問題編號(對應匯入及匯出模板編號)" name="QuestionNo" handleOnChange={e => props.handleOnChange(e, indexArg)} handleOnBlur={e => props.handleOnBlur(e, indexArg)} value={props.questionNo} required={true} />
                    <TextInput className="text-editor-container" key={"CodingBookIndex_" + props.groupIndex + "_" + props.index} label="CodingBook Index(-1: 不顯示)" name="CodingBookIndex" handleOnChange={e => props.handleOnChange(e, indexArg)} value={props.codingBookIndex} required={true} />
                    {/* <TextInput className="text-editor-container" key={"CodingBookTitle_" + props.groupIndex + "_" + props.index} label="CodingBook 標題" name="CodingBookTitle" handleOnChange={e => props.handleOnChange(e, indexArg)} value={props.codingBookTitle} /> */}
                </div>
            </div>
            {/* <div className="row">
                <Select colwidth={null} className="quest-type-select-container" name="QuestType" handleOnChange={e => props.handleQuestTypeChange(e, props.groupIndex, props.index)} value={props.questType} options={props.questTypes} />
                <button className="btn btn-default quest-type-button" type="button" onClick={handleAddQuest}>新增題目</button>
            </div> */}
        </>
    );
}

AutoGenerateFormContainer.propTypes = {
    index: PropType.number,
    groupIndex: PropType.number,
    titleLabel: PropType.string,
    handleDeleteQuest: PropType.func,
    handleOnChange: PropType.func,
    handleOnBlur: PropType.func,
}

export default AutoGenerateFormContainer;