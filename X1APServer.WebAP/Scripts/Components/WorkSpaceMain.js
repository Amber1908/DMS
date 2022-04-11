import React from 'react';
import PropType from 'prop-types';

import PatientInfo from './PatientInfo';

const WorkSpaceMain = (props) => {
    return (
        <>
            <div className="ui-toolBar-Wrap">
                <PatientInfo />
                {props.toolbar}
            </div>
            {props.window}
        </>
    );
};

WorkSpaceMain.propTypes = {
    toolbar: PropType.node,
    window: PropType.node
}

export default WorkSpaceMain;