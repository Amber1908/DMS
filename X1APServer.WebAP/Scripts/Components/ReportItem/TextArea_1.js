import React, { useImperativeHandle, useRef, forwardRef, useEffect } from 'react';
import PropType from 'prop-types';

const TextArea = (props) => {
    const textareaRef = useRef(null);

    useEffect(() => {
        // 內容改變自動長高
        let textarea = textareaRef.current;
        if (textarea.clientHeight < textarea.scrollHeight)
            textarea.style.height = textarea.scrollHeight + "px";
    }, [props.value])

    let containerClass = "", textareaClass = "";

    switch(props.type) {
        case 1:
            containerClass = "input-item edit";
            break;
        default:
            containerClass = "";
            break;
    }

    return (
        <div style={{width: "100%"}}>
            <div>{props.label}</div>
            <div className={`${containerClass} ${props.className}`}>
                <textarea placeholder={props.placeholder} style={{resize: "vertical", width: "100%"}} ref={textareaRef} className={textareaClass} name={props.name} onChange={props.handleOnChange} rows={props.rows} value={props.value} disabled={props.disabled} required={props.required} {...props.textAreaProps} />
            </div>
        </div>
    );
};

TextArea.defaultProps = {
    rows: 8,
    required: false,
    className: ""
};

TextArea.propTypes = {
    // textarea onchange
    handleOnChange: PropType.func,
    // textarea value
    value: PropType.string,
    // textarea rows
    rows: PropType.number,
    // textarea name
    name: PropType.string,
    // textarea props
    textAreaProps: PropType.object,
    // textarea required
    required: PropType.bool,
    // 問題文字
    label: PropType.string,

    className: PropType.string,
    // textarea 外觀類型
    type: PropType.number,
};

export default TextArea;
