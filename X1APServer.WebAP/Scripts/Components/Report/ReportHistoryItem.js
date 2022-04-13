import React, { useState } from 'react';
import PropType from 'prop-types';
import { Link, useRouteMatch } from 'react-router-dom';
import { GetDate, GetDateTime, GetStatus, GetStatusColor } from '../../Utilities/CommonUtility';

const ReportHistoryItem = (props) => {
    const { url } = useRouteMatch();
    let scolor = GetStatusColor(props.reportInfo.Status);
    let scontent = GetStatus(props.reportInfo.Status);

    return (
        <Link className="ui-historyEntity type-Questionnaire" to={`${url}/${props.reportInfo.ID}`}>
            <span className={`title ${scolor}`}>{props.index} 建檔日期:{GetDate(props.reportInfo.FillingDate)}　{scontent}</span>
            <span className="attribute">病患姓名: {props.reportInfo.PatientName}　</span>
            <span className="attribute">最後修改人員: {props.reportInfo.ModifyMan}　</span>
            <span className="attribute">最後修改時間: {GetDateTime(props.reportInfo.ModifyDate)}　</span>
            <span className="score" />
        </Link>
    );
};

ReportHistoryItem.propTypes = {
    reportInfo: PropType.shape({
        // 表單ID
        ID: PropType.number,
        // 填寫日期
        FillingDate: PropType.string,
        // 個案姓名
        PatientName: PropType.string,
        // 修改人員
        ModifyMan: PropType.string,
        // 修改日期
        ModifyDate: PropType.string,
        // 表單狀態
        Status: PropType.number
    })
}

export default ReportHistoryItem;