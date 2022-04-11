import React from 'react';
import PropType from 'prop-types';
import Log from '../../Utilities/LogUtility';

const CheckBox = (props) => {
    const id = props.id == null ? props.name : props.id;

    const handleOnChange = (e) => {
        if (!e.target.checked) {
            e.target.value = "";
        }

        props.handleOnChange(e);
    }

    let otherAnsElement = null;
    if (props.trueValue === "other" && !!props.value) {
        otherAnsElement = (
            <div className="item-wrap edit ui-Col-100">
                <div className="input-item">
                    <input type="text" name={props.otherAnsName} value={props.otherAnsValue} required={true} onChange={props.handleOnChange} />
                </div>
            </div>
        );
    }

    let checked = false;
    if (
        props.value != "" &&
        props.value != null &&
        props.value !== "false"
    ) checked = true;

    return (
        <>
            <div className={`radio-item ${props.className} float-none`} style={props.style}>
                <input disabled={props.disabled} onChange={handleOnChange} checked={checked} type="checkbox" id={id} name={props.name} value={props.trueValue} />
                <label htmlFor={id}>{props.text}</label>
            </div>
            {otherAnsElement}
        </>
    );
};

CheckBox.defaultProps = {
    className: "",
    otherAnsFlag: false,
    disabled: false
}

CheckBox.propTypes = {
    handleOnChange: PropType.func,
    name: PropType.string,
    trueValue: PropType.string.isRequired,
    text: PropType.string.isRequired,
    value: PropType.any,
    id: PropType.string,
    style: PropType.object,
    className: PropType.string,
    otherAnsFlag: PropType.bool,
    otherAnsName: PropType.string,
    otherAnsValue: PropType.string
};

export default CheckBox;