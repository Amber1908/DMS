import React, { useEffect, useState } from 'react';
import PropType from 'prop-types';

const Select = (props) => {
    const handleOnChange = (e) => {
        if (e.target.value === "other") {
            props.handleOnChange({
                target: {
                    name: props.otherAnsName,
                    value: ""
                }
            });
        }
        props.handleOnChange(e);
    }

    let colwidthClass = "";
    let hidden = false;
    let options = [];
    let value = "";

    if (props.value != null) {
        value = props.value;
    }

    if (props.colwidth != null) {
        colwidthClass = `ui-Col-${props.colwidth}`;
    }

    if (props.hidden != null) {
        hidden = props.hidden;
    }

    if (props.options != null) {
        props.options.forEach((item) => {
            options.push(<option key={item.value} value={item.value}>{item.text}</option>);
        });

    }

    let otherAnsElement = null;
    if (value === "other") {
        otherAnsElement = (
            <div className="item-wrap edit ui-Col-100">
                <div className="input-item">
                    <input type="text" name={props.otherAnsName} value={props.otherAnsValue} required={true} onChange={props.handleOnChange} />
                </div>
            </div>
        );
    }

    let labelElement = null;
    if (props.label) {
        labelElement = (<span>{props.label}</span>);
    }

    return (
        <>
            <div className={`select-item ${colwidthClass} ${props.className}`} style={props.style} hidden={hidden}>
                {labelElement}
                <select name={props.name} onChange={handleOnChange} value={value} disabled={props.disabled} required={props.required}>
                    <option value="" disabled>--請選擇--</option>
                    {options}
                    {/* {otherAnsElement} */}
                </select>
            </div>
            {otherAnsElement}
        </>
    );
};

Select.defaultProps = {
    colwidth: 100,
    disabled: false,
    value: "",
    required: false,
    className: "",
    style: {},
    otherAnsFlag: false,
    options: []
};

Select.propTypes = {
    colwidth: PropType.number,
    label: PropType.string,
    name: PropType.string,
    handleOnChange: PropType.func,
    value: PropType.string,
    options: PropType.arrayOf(PropType.shape({
        text: PropType.string.isRequired,
        value: PropType.string.isRequired
    })).isRequired,
    disabled: PropType.bool,
    required: PropType.bool,
    className: PropType.string,
    style: PropType.object,
    otherAnsFlag: PropType.bool,
    otherAnsName: PropType.string,
    otherAnsValue: PropType.string
};

export default Select;