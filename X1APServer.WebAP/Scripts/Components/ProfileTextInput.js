import React from 'react';
import PropType from 'prop-types';

const ProfileTextInput = (props) => {

    let colWidthClass = "";

    if (props.colWidth != null) {
        colWidthClass = `ui-Col-${props.colWidth}`;
    }

    return (
        <div className={`input-item ${colWidthClass}`}>
            <label>{props.label}</label>
            <input value={props.value} onChange={props.handleOnChange} name={props.name} placeholder={props.placeholder} {...props.inputProps} />
        </div>
    );
}

ProfileTextInput.propTypes = {
    colWidth: PropType.number,
    label: PropType.string,
    name: PropType.string.isRequired,
    placeholder: PropType.string,
    inputProps: PropType.object,
    handleOnChange: PropType.func.isRequired,
    value: PropType.string.isRequired
}

export default ProfileTextInput;