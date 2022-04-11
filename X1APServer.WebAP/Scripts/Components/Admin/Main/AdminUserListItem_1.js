import React, { useState, useContext } from 'react';
import { Link } from "react-router-dom";

import { Ajax } from '../../../Utilities/AjaxUtility';
import { GetDate } from '../../../Utilities/CommonUtility';
import { URLInfoContext } from '../../Context';

const AdminUserListItem = (props) => {
    let dateClass;

    let match = useContext(URLInfoContext);

    switch (props.userInfo.UseState) {
        case "Y":
            dateClass = "ACTIVESER";
        break;
        case "B":
        case "S":
            dateClass = "SUSPENDUSER";
        break;
        case "N":
            dateClass = "APPLYUSER";
        break;
    }

    return (
        <Link to={`${match.url}/User/${props.userInfo.AccID}/Home`}>
            <div className="ui-ItemList" onClick={(e) => props.handleUserItemClick(e, props.userInfo.AccID)}>
                <img src={Ajax.BaseURL + "/Content/Images/user0.png"} />
                <div className="ui-Title">
                    <span name="CaseName">{props.userInfo.AccName}({props.userInfo.AccID})</span>
                </div>
                <div className="ui-Info">
                    <span className={dateClass}>{GetDate(props.userInfo.StateUDate)}</span>
                </div>
            </div>
        </Link>
    );
};

export default AdminUserListItem;