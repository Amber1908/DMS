import React from "react";
import TypeProp from "prop-types";

// 一般類型問題
const TextInput = (props) => {
    let colwidthClass = "";
    let unitTag;
    let value = "";

    if (props.colwidth != null) {
        colwidthClass = `ui-Col-${props.colwidth}`;
    }

    if (props.unit != null) {
        unitTag = <span>{props.unit}</span>;
    }

    if (props.value != null) {
        value = props.value;
    }

    if (props.inline) {
        return (
            <span className={`inline-item-wrap edit ${props.inputStyleType} ${props.className}`} style={props.style}>
                <input value={value} type={props.type} name={props.name} placeholder={props.placeholder} onClick={props.handleOnClick} onChange={props.handleOnChange} {...props.inputProps} />
                {unitTag}
            </span>
        );
    } else {
        return (
            <div className={`item-wrap edit ${colwidthClass} ${props.className}`} style={props.style}>
                <div>{props.label}</div>
                <div className={props.inputStyleType}>
                    <input value={value} type={props.type} name={props.name} placeholder={props.placeholder} onClick={props.handleOnClick} onChange={props.handleOnChange} onBlur={props.handleOnBlur} required={props.required} {...props.inputProps} />
                    {unitTag}
                </div>
            </div>
        );
    }
};

TextInput.defaultProps = {
    placeholder: "",
    type: "text",
    inputStyleType: "input-item",
    colwidth: 100,
    inline: false,
    required: false,
    className: "",
};

TextInput.propTypes = {
    // 元件寬度
    colwidth: TypeProp.number,
    // 問題文字
    label: TypeProp.string,
    name: TypeProp.string,
    placeholder: TypeProp.string,
    // input onchange
    handleOnChange: TypeProp.func,
    // input onblur
    handleOnBlur: TypeProp.func,
    // 答案
    value: TypeProp.string,
    // 問題類型
    type: TypeProp.string,
    // 問題單位
    unit: TypeProp.string,
    // input parent div class
    inputStyleType: TypeProp.string,
    // input 屬性
    inputProps: TypeProp.object,
    // 單行顯示?
    inline: TypeProp.bool,
    style: TypeProp.object,
    // 必填?
    required: TypeProp.bool,
    className: TypeProp.string,
    // input onclick
    handleOnClick: TypeProp.func,
};

export default TextInput;
