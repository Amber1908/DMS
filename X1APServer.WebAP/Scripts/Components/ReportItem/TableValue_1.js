import React from 'react';
import PropType from 'prop-types';

const TableValue = (props) => {
    return (
        <span name={props.name} className={props.className}>{props.value}</span>
    );
};

TableValue.propTypes = {
    className: PropType.string,
    value: PropType.string,
    name: PropType.string
}

export default TableValue;