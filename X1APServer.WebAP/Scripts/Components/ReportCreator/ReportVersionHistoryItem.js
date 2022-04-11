import React from 'react';
import { Link, useRouteMatch } from 'react-router-dom';
import PropTypes from 'prop-types';
import { GetDateTime } from '../../Utilities/CommonUtility';

const ReportVersionHistoryItem = (props) => {
    const { url, path } = useRouteMatch();

    let publishElement = [(<span key={"default_" + props.reportMain.ID}>尚未發佈</span>)];
    if (props.reportMain.PublishDate != null) {
        publishElement = [(
            <span key={"publishDate_" + props.reportMain.ID} className="date">發佈時間 {GetDateTime(props.reportMain.PublishDate)}</span>
        )];

        if (props.reportMain.IsPublish) {
            publishElement.push(
                <span key={"publishState_" + props.reportMain.ID}>正在發佈</span>
            );
        }
    }

    return (
        <Link className="ui-historyEntity type-Questionnaire" to={`${url}/${props.reportMain.ID}`}>
            <span className="title">{`${props.reportMain.Title}`}</span>
            <span className="date">新增時間 {GetDateTime(props.reportMain.CreateDate)}</span>
            <span className="interviewer">
                <span className="interviewerTitle">修改人員</span>
                {props.reportMain.ModifyMan}
            </span>
            {publishElement}
        </Link>
    );
}

ReportVersionHistoryItem.propTypes = {
    url: PropTypes.string,
    reportMain: PropTypes.object
}

export default ReportVersionHistoryItem;