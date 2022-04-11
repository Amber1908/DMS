import React from 'react';
import CheckBox from '../../ReportItem/CheckBox';
import Select from '../../ReportItem/Select';
import TextInput from '../../ReportItem/TextInput';

const OptionAttr = (props) => {
    let { indexGroup, reportStructure } = props;
    let { groupIndex, questIndex, optionIndex } = indexGroup;
    let quest = reportStructure.Children[groupIndex].Children[questIndex];
    let option = reportStructure.Children[groupIndex].Children[questIndex].AnswerOption[optionIndex];

    let otherElement = null;
    let selectColorElement = null;
    if (quest.QuestionType === "checkbox") {
        otherElement = (
            <TextInput
                className="text-editor-container"
                label="匯出資料排序編號(-1: 不顯示)"
                name="CodingBookIndex"
                type="number"
                handleOnChange={e => props.handleOnChange(e, indexGroup)}
                value={option.CodingBookIndex}
                required={true} />            
        );
        selectColorElement = (
            <Select label="異常顏色" 
                name="AbnormalColor" 
                handleOnChange={e => props.handleOnChange(e, indexGroup)}
                value={option.AbnormalColor}
                options={[
                    {text: "紅", value: "red"},
                    {text: "橘", value: "orange"},
                    {text: "藍", value: "blue"},
                    {text: "紫", value: "purple"}
                ]} />
        );
    }

    let abnormalOptionElement = null;
    if (quest.Pin) {
        abnormalOptionElement = (
            <>
                {selectColorElement}
                <CheckBox name="AbnormalValue"
                    className="allow-interact"
                    trueValue="true"
                    text="異常選項"
                    value={option.AbnormalValue}
                    handleOnChange={e => props.handleOnChange(e, indexGroup)} />
            </>
        );
    }

    return (
        <>
            <TextInput
                className="text-editor-container"
                label="值"
                name="Value"
                inputProps={{maxLength: "100"}}
                handleOnChange={e => props.handleOnChange(e, indexGroup)}
                value={option.Value}
                required={true} />
                {otherElement}
                {abnormalOptionElement}
        </>
    );
}

export default OptionAttr;