import React from 'react';
import PropTypes from 'prop-types';
import { GlobalConstants } from '../Utilities/CommonUtility';

const Loader = (props) => {
    let children = null;

    if (props.status === GlobalConstants.Status.LOADING) {
        children = (
            <div style={{ textAlign: "center", display: "inline-block", fontSize: props.fontSize, padding: "3px" }}>
                <i className="fa fa-refresh fa-spin"></i>
            </div>
        );
    }

    return children;
}

Loader.propTypes = {
    fontSize: PropTypes.string,
    status: PropTypes.number
}

export default Loader;