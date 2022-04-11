import React from 'react';
import PropType from 'prop-types';

const LabelItem = (props) => {
    let colwidthClass;

    if (props.colwidth != null) {
        colwidthClass = `ui-Col-${props.colwidth}`;
    }

    return (
        <div className={`label-Item ${colwidthClass}`}>
            <label>{props.title}</label>
            <span>{props.content}</span>
        </div>
    );
};

LabelItem.propTypes = {
    content: PropType.string,
    title: PropType.string,
    colwidth: PropType.number
};

export default LabelItem;