import React, { useContext, useEffect } from 'react';
import PropType from 'prop-types';

import { CaseInfoContext } from '../Context';
import { Ajax } from '../../Utilities/AjaxUtility';

const AdminWorkSpaceMain = (props) => {

    const {userInfo} = useContext(CaseInfoContext);

    return (
        <>
            <div className="ui-toolBar-Wrap">
                <div className="ui-toolBar-Group" style={{ border: 'none', overflow: 'visible' }}>
                    <div className="ui-toolBar-Avatar">
                        <img src={Ajax.BaseURL + "/Content/Images/user0.png"} />
                    </div>
                    <span className="toolBar-name">{userInfo.AccName}</span>
                </div>
                {props.toolbar}
            </div>
            {props.window}
        </>
    );
};

AdminWorkSpaceMain.propTypes = {
    toolbar: PropType.node,
    window: PropType.node
}

export default AdminWorkSpaceMain;