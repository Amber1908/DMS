import React from 'react';
import PropType from 'prop-types';

const TableData = (props) => {
    let colwidthClass = "";
    if (props.colwidth != null) {
        colwidthClass = `ui-Col-${props.colwidth}`;
    }

    return (
        <div className={`QForm-Table-Item ${props.className} ${colwidthClass}`} style={props.style}>
            {props.children}
        </div>
    );
};

TableData.defaultProps = {
    className: ""
}

TableData.propTypes = {
    colwidth: PropType.number,
    children: PropType.node,
    className: PropType.string,
    style: PropType.object
};

export default TableData;