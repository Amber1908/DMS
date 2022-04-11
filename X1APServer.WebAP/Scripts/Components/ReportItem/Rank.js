import React from 'react';
import PropType from 'prop-types';

const Rank = (props) => {
    let showText = "";
    const valueNum = parseFloat(props.value);
    const lastIndex = props.levelValue.length - 1;

    props.levelValue.forEach((element, index) => {
        if (valueNum < element && props.levelText[index] != null) {
            showText = props.levelText[index];
        }
    });
    
    if (valueNum > props.levelValue[lastIndex] && props.levelText[lastIndex + 1] != null) {
        showText = props.levelText[lastIndex + 1];
    }

    return (<span>{showText}</span>);
}

Rank.defaultProps = {
    levelText: [],
    levelValue: []
};

Rank.propTypes = {
    name: PropType.string,
    value: PropType.string,
    levelText: PropType.arrayOf(PropType.string),
    levelValue: PropType.arrayOf(PropType.number)
};

export default Rank;