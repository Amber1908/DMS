import React, { useContext } from 'react';

import { GetDateTime } from '../../Utilities/CommonUtility';

const ReportListItem = (props) => {
    let { GUID, MimeType, FileName, ModifyDate, ModifyMan } = props.report;

    return (
        <div className="ui-ItemList" onClick={(e) => props.handleReportClick(e, GUID, MimeType)}>
            <div className="ui-Title">
                <span name="CaseName">{FileName}</span>
            </div>
            <div className="ui-Info">
                <span className="icon-calendar">{GetDateTime(ModifyDate)}</span>
                <span className="ACTIVESER">{ModifyMan}</span>
            </div>
        </div>
    );
};

export default ReportListItem;