import React, { useState, useEffect } from 'react';
import { useHistory, useParams, useRouteMatch } from 'react-router-dom';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { GlobalConstants } from '../../Utilities/CommonUtility';
import ReportVersionHistoryItem from './ReportVersionHistoryItem';

const ReportVersionHistory = (props) => {
    let history = useHistory();
    let { category } = useParams();
    // 所有版本Report
    const [allVersionReportList, setAllVersionReportList] = useState([]);

    const { PostWithAuth } = usePostAuth();

    const { url, path } = useRouteMatch();

    // 跳至Report新增頁面
    const handleAddNewVersion = (e) => {
        history.push(`${url}/New`);
    }

    const GetAllVersionReport = (category) => {
        // 取得所有同類別不同版本的Report
        return new Promise((resolve, reject) => PostWithAuth({
            url: "/Report/GetAllVersionReport",
            data: {
                Category: category,
                FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                AuthCode: 1
            },
            success: (response) => {
                resolve(response);
            }
        }));
    }

    useEffect(() => {
        if (category != null) {
            let ignore = false;

            GetAllVersionReport(category).then(value => {
                if (ignore) return;
                setAllVersionReportList(value.Data);
            });

            return () => { ignore = true; };
        }
    }, [category])

    let reports = [];

    allVersionReportList.forEach((element, index) => {
        reports.push(<ReportVersionHistoryItem key={index} reportMain={element} url={props.match.url} />);
    });

    const hasReport = reports.length > 0;

    return (
        <>
            <div className="ui-toolBar-Wrap">
                <div className="ui-toolBar-Group ui-Col-33">
                    <label>表單版本工具</label>
                    <div className="ui-buttonGroup">
                        <button onClick={handleAddNewVersion} className="offsetLeft-5" data-tooltip="新增新版本的表單">
                            <i className="fa fa-plus-square" aria-hidden="true" />
                            新增表單
                        </button>
                    </div>
                </div>
            </div>
            <div className="QForm-Wrap" id="QFormxContainer" name="QFormContainer">
                <span id="Status" hidden>表單版本清單</span>
                <div id="HistoryForm" className="Q-History ui-historyWrap ui-Col-100 cfg-historyListView">
                    <h4>表單版本清單</h4>
                    <div className="ui-blankHistory" hidden={hasReport}>
                        <h4>
                            <i className="fa fa-info-circle" aria-hidden="true" />沒有任何版本的表單
                        </h4>
                        {/* <span>請在工具欄按新增表單</span> */}
                    </div>
                    {reports}
                </div>
            </div>
        </>
    );
}

export default ReportVersionHistory;