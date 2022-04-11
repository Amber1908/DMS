import React, { useState, useEffect, useContext } from 'react';
import { useHistory, useRouteMatch } from 'react-router-dom';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { GlobalConstants } from '../../Utilities/CommonUtility';
import { CookieContext } from '../Context';
import ReportMainListItem from './ReportMainListItem';

const ReportMainList = (props) => {
    const { cookies } = useContext(CookieContext);

    // Report Main 清單
    const [reportMList, setReportMList] = useState([]);

    const { url } = useRouteMatch();

    let history = useHistory();

    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        let ignoreAsyncTaskResponse = false;

        // 取得所有 Report
        PostWithAuth({
            url: "/Report/GetAllReportMain",
            data: {
                FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                AuthCode: 1
            },
            success: (response) => {
                if (ignoreAsyncTaskResponse) return;

                // 將取回的所有Report填入State
                setReportMList(response.ReportMainList);
            }
        });

        return () => { ignoreAsyncTaskResponse = true };
    }, [cookies[GlobalConstants.CookieName]])

    // 跳至Report新增頁面
    const handleAddReport = (e) => {
        history.push(`${url}/New`);
    }

    let reports = reportMList.map((reportM) => <ReportMainListItem key={reportM.Category} reportMain={reportM} />);

    const hasReport = reports.length > 0;

    return (
        <>
            <div className="ui-toolBar-Wrap">
                <div className="ui-toolBar-Group ui-Col-33">
                    <label>表單編輯工具</label>
                    <div className="ui-buttonGroup">
                        <button onClick={handleAddReport} className="offsetLeft-5" data-tooltip="新增表單">
                            <i className="fa fa-pencil" aria-hidden="true" />
                            新增表單
                        </button>
                    </div>
                </div>
            </div>
            <div className="QForm-Wrap" id="QFormxContainer" name="QFormContainer">
                <span id="Status" hidden>表單清單</span>
                <div id="HistoryForm" className="Q-History ui-historyWrap ui-Col-100 cfg-historyListView">
                    <h4>表單清單</h4>
                    <div className="ui-blankHistory" hidden={hasReport}>
                        <h4>
                            <i className="fa fa-info-circle" aria-hidden="true" />沒有任何表單
                        </h4>
                    </div>
                    {reports}
                </div>
            </div>
        </>
    );
}

export default ReportMainList;