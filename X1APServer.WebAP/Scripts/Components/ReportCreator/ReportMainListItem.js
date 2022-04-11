import React from 'react';
import { Link, useRouteMatch } from 'react-router-dom';
import PropTypes from 'prop-types';
import { GetDateTime } from '../../Utilities/CommonUtility';

const ReportMainListItem = (props) => {
    const { url, path } = useRouteMatch();

    return (
        <Link className="ui-historyEntity type-Questionnaire" to={`${url}/${props.reportMain.Category}`}>
            <span className="title">{`${props.reportMain.Title}`}</span>
            <span className="date">修改時間 {GetDateTime(props.reportMain.ModifyDate)}</span>
            <span className="interviewer">
                <span className="interviewerTitle">修改人員</span>
                {props.reportMain.ModifyMan}
            </span>
        </Link>
    );
}

ReportMainListItem.propTypes = {
    reportMain: PropTypes.shape({
        // 表單類別
        Category: PropTypes.string,
        // 表單標題
        Title: PropTypes.string,
        // 表單最後修改時間
        ModifyDate: PropTypes.string,
        // 表單修改人員
        ModifyMan: PropTypes.string
    })
}

export default ReportMainListItem;