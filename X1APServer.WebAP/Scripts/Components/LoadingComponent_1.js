import React from 'react';
import PropTypes from 'prop-types';
import { GlobalConstants } from '../Utilities/CommonUtility';

const LoadingComponent = (props) => {
    let children = null;

    if (props.status === GlobalConstants.Status.INIT) {
        children = props.children;
    } else if (props.status === GlobalConstants.Status.LOADING) {
        children = (
            <div style={{ textAlign: "center", fontSize: props.fontSize }}>
                <i className="fa fa-refresh fa-spin" style={{ margin: "20px" }}></i>
            </div>
        );
    }

    return (
        <>
            {children}
        </>
    );
}

LoadingComponent.defaultProps = {
    fontSize: "12px"
}

LoadingComponent.propTypes = {
    fontSize: PropTypes.string,
    status: PropTypes.number
}

export default LoadingComponent;