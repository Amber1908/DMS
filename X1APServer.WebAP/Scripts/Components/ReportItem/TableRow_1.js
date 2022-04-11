import React from 'react';
import PropType from 'prop-types';

const TableRow = (props) => {
    return (
        <div className={`QForm-Table-ItemWrap ${props.className}`}>
            {props.children}
        </div>
    );
};

TableRow.defaultProps = {
    className: ""
}

TableRow.propTypes = {
    children: PropType.node,
    className: PropType.string
};

export default TableRow;