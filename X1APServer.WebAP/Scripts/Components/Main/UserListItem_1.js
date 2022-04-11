import React from 'react';
import { Link, useRouteMatch } from "react-router-dom";
import PropType from 'prop-types';

import { Ajax } from '../../Utilities/AjaxUtility';
import { GetDate } from '../../Utilities/CommonUtility';

const UserListItem = (props) => {
    const { url } = useRouteMatch();

    let showGender;
    // 轉換性別
    if (props.userGender === "M") {
        showGender = "男";
    } else {
        showGender = "女";
    }

    // 轉換回診時間
    let nextVisitTime = "無";
    if (props.nextVisitTime) {
        // const nextVisitTime = props.nextVisitTime.split(" ")[0].replace("/", "-");
        nextVisitTime = GetDate(props.nextVisitTime);
    }

    return (
        <Link to={`${url}/Patient/${props.caseID}/index`}>
            <div className="ui-ItemList">
                <img src={Ajax.BaseURL + "/Content/Images/case.png"} />
                <div className="ui-Title">
                    <span name="CaseName">{props.caseName}({props.idno})</span>
                </div>
                <div className="ui-Info">
                    <span className="GENDER">{showGender}</span>
                    <span>回診時間: {nextVisitTime}</span>
                </div>
            </div>
        </Link>
    );
};

UserListItem.propTypes = {
    caseID: PropType.number,
    caseName: PropType.string,
    idno: PropType.string,
    userGender: PropType.string,
    nextVisitTime: PropType.string
}

export default UserListItem;