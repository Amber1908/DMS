import React from 'react';

const QuestContainer = (props) => {
    const handleOnClick = (e, indexGroup) => {
        e.stopPropagation();
        props.handleClickComponent(e, indexGroup);
    }

    const onDragStart = (e) => {
        e.stopPropagation();
        if (!props.draggable) return;
        props.onDragStart(e);
    }

    const onDrag = (e) => {
        e.stopPropagation();
        if (!props.draggable) return;
        props.onDrag(e);
    }

    const onDragOver = (e) => {
        e.stopPropagation();
        if (!props.draggable) return;
        props.onDragOver(e);
    }

    const onDragEnd = (e) => {
        e.stopPropagation();
        if (!props.draggable) return;
        props.onDragEnd(e);
    }

    const onDrop = (e) => {
        e.stopPropagation();
        if (!props.draggable) return;
        props.onDrop(e);
    }

    const onMouseDown = (e) => {
        e.stopPropagation();
        props.onMouseDown(e);
    }

    let className = "";
    // if (props.dragAndDrop.isDragging) className += "dragging ";
    if (props.dragAndDrop.draggedTo === props.index) className += "dragTo ";    
    if (props.selectedComponent.id === props.uniqueID) {
        className += "selected ";
    }

    let indexGroup = { groupIndex: props.groupIndex, questIndex: props.index };    

    return (
        <div className={`edit-quest-container draggable ${className}`} 
            id={props.uniqueID}
            draggable
            onClick={e => handleOnClick(e, indexGroup)} 
            onDragStart={onDragStart} 
            onDragOver={onDragOver} 
            onDragEnd={onDragEnd} 
            onDrop={onDrop}
            onDrag={onDrag}
            onMouseDown={onMouseDown}
            onMouseDown={props.onMouseDown}
            data-position={props.index}
            data-id={props.uniqueID}
            data-type="Quest">
            <div className="handle">　</div>
            {props.children}
        </div>
    );
}

QuestContainer.defaultProps = {
    draggable: true
}

export default QuestContainer;