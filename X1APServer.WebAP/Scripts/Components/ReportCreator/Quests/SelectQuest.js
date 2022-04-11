import QuestCommon from "./QuestCommon";
import QuestContainer from "./QuestContainer";

import React, { useRef } from 'react';
import TextInput from "../../ReportItem/TextInput";
import { useDragAndDrop } from "../../../CustomHook/useDragAndDrop";

const SelectQuest = (props) => {
    const { onDragStart, onDragOver, onDrop, onDragEnd, onMouseDown, dragAndDrop } = useDragAndDrop(props.AnswerOption, list => props.handleChangeOptionSortOrder(list, { groupIndex: props.groupIndex, questIndex: props.index }));

    const dragStart = (e) => {
        e.stopPropagation();
        onDragStart(e);
    }

    const dragOver = (e) => {
        e.stopPropagation();
        onDragOver(e);
    }

    const dragEnd = (e) => {
        e.stopPropagation();
        onDragEnd(e);
    }

    const drop = (e) => {
        e.stopPropagation();
        onDrop(e);
    }

    const onClick = (e, indexGroup) => {
        e.stopPropagation();
        props.handleClickComponent(e, indexGroup);
    }

    const handleOptionTextChange = (e, indexGroup) => {
        props.handleOnChange(e, indexGroup);
        let fakeEvent = {
            target: {
                name: "Value",
                value: e.target.value,
            },
        };
        props.handleOnChange(fakeEvent, indexGroup);
    }

    let options = props.AnswerOption.map((option, index) => {
        if (option.HiddenFromBackend) return;

        let indexGroup = { groupIndex: props.groupIndex, questIndex: props.index, optionIndex: index };
        let className = "";
        if (dragAndDrop.isDragging) className += "dragging ";
        if (dragAndDrop.draggedTo === index) className += "dragTo ";  
        if (props.selectedComponent.id === option.uniqueID) {
            className += "selected ";
        }

        let inputProps = {
            style: { width: "calc(100% - 15px)" },
            name: "OptionText",
            value: option.OptionText,
            required: true,
            placeholder: "選項",
            inputProps: { maxLength: "100" },
            handleOnChange: (e) => handleOptionTextChange(e, indexGroup),
        };
        
        return (
            <div className={`draggable ${className}`} 
                draggable
                onDragStart={dragStart} 
                onDragOver={dragOver}
                onDrop={drop}
                onDragEnd={dragEnd}
                onMouseDown={onMouseDown}
                onClick={e => onClick(e, indexGroup)}
                data-position={index}
                data-id={option.uniqueID}
                data-type="Option"
                style={{ display: "flex", alignItems: "center", position: "relative", marginTop: "5px" }}>
                <div className="handle"></div>
                <TextInput {...inputProps} />
                <button className="remove-option close fa fa-times" key={"Delete_" + index} type="button" onClick={e => props.handleDeleteOption(e, { groupIndex: props.groupIndex, questIndex: props.index, optionIndex: index })}></button>
            </div>
        )
    });

    const addOption = (e) => {
        props.handleAddOption(e, { groupIndex: props.groupIndex, questIndex: props.index, optionIndex: props.AnswerOption.length });
    }

    

    return (
        <QuestContainer {...props}>
            <QuestCommon {...props} />
            {options}
            
            <TextInput name="OptionText"
                placeholder="點擊新增選項"        
                handleOnClick={addOption} />
        </QuestContainer>
    );
}

export default SelectQuest;