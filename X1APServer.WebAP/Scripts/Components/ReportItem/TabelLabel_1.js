import React from 'react';
import PropType from 'prop-types';

const TableLabel = (props) => {
    return (
        <div className="QForm-Table-Label" style={props.style}>{props.text}</div>
    );
};

TableLabel.propTypes = {
    text: PropType.string,
    style: PropType.object
};

export default TableLabel;