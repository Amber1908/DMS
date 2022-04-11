import React from 'react';
import PropType from 'prop-types';

const RadioButton = (props) => {
    return (
        <div className="radio-item">
            <input onChange={props.handleOnChange} checked={props.checked} type="radio" id={props.id} name={props.name} value={props.value} required={props.required} />
            <label htmlFor={props.id}>{props.text}</label>
        </div>
    );
};

RadioButton.defaultProps = {
    checked: false,
    required: false
};

RadioButton.propTypes = {
    handleOnChange: PropType.func.isRequired,
    checked: PropType.bool,
    id: PropType.string.isRequired,
    name: PropType.string.isRequired,
    value: PropType.string.isRequired,
    required: PropType.bool
};

export default RadioButton;