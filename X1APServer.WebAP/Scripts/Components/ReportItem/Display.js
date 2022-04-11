import React from 'react';
import PropType from 'prop-types';

const Display = (props) => {
    let value = props.value;

    if (props.percentage && value != "") {
        value = Math.round(parseFloat(value) * 10000) / 100;
        value = value.toString() + " %";
    }

    return (<span className={props.className} name={props.name}>{value}</span>)
};

Display.defaultprops = {
    percentage: false
}

Display.propTypes = {
    name: PropType.string,
    value: PropType.string,
    className: PropType.string,
    percentage: PropType.bool
}

export default Display;