import React from 'react';
import PropType from 'prop-types';

const Switch = (props) => {
    const handleOnChange = (e) => {
        if (!e.target.checked) {
            e.target.value = "";
        }

        props.handleOnChange(e);
    }

    return (
        <>
            <input className="vselect" type="checkbox" onChange={handleOnChange} name={props.name} value={props.trueValue} checked={props.value} />
            <div className="toggle"></div>
            <div className="Switch_Content">
                {props.children}
            </div>
        </>
    );
};

Switch.propTypes = {
    name: PropType.string,
    value: PropType.string,
    trueValue: PropType.string,
    children: PropType.node,
    handleOnChange: PropType.func
};

export default Switch;
