import React, { useState, useEffect } from 'react';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { GlobalConstants } from '../../Utilities/CommonUtility';
import ReportListItem from './ReportListItem';
import UserListTab from '../UserListTab';

const ReportList = (props) => {
    // State
    const [reportList, setReportList] = useState([]);
    const [reportListPage, setReportListPage] = useState({ page: 0, scrollFlag: true });
    const [selectedReportTab, setSelectedReportTab] = useState("all");
    // 搜尋關鍵字
    const [search, setSearch] = useState("");

    // Custom Hooks
    // Post With Auth Function
    const { PostWithAuth } = usePostAuth();

    // Effects
    useEffect(
        () => {
            // 初始化元件抓前30個個案
            setReportListPage({
                page: 0,
                scrollFlag: false
            });
            setReportList([]);
            GetReports({ Page: 0 });
        },
        []
    );

    // Events
    const handleReportListScroll = (e) => {
        let target = e.target;

        // 若當下沒在抓取資料及快滾動至底部，抓取接著30筆資料
        if (reportListPage.scrollFlag &&
            target.scrollHeight - target.scrollTop - target.clientHeight < 50) {
            setReportListPage(prev => {
                return { ...prev, scrollFlag: false }
            });
            GetReports({});
        }
    };

    // 搜尋變更
    const handleSearchChange = (e) => {
        setSearch(e.target.value);
    }

    // 點擊搜尋
    const handleSearchSubmit = (e) => {
        e.preventDefault();
        GetReports({ Page: 0, ReportCategory: selectedReportTab, FileName: search });
    }

    const handleReportTabClick = (e) => {
        let reportCategory = e.target.id;

        setSelectedReportTab(e.target.id);
        GetReports({ Page: 0, ReportCategory: reportCategory});
    }

    // Functions
    const GetReports = ({ Page = reportListPage.page, ReportCategory = undefined, FileName = undefined }) => {
        let reportRowInPage = Math.ceil(window.innerHeight / 50);

        if (ReportCategory == "all") {
            ReportCategory = "";
        }

        if (Page == 0) {
            setReportList([]);
            setReportListPage({
                scrollFlag: true,
                page: 0
            });
        }

        PostWithAuth({
            url: "/Report/GetExportReportListLazy",
            data: {
                "Page": Page,
                "RowInPage": reportRowInPage,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1,
                ReportCategory: ReportCategory,
                PID: document.getElementById("pid").value,
                FileName
            },
            success: (response) => {
                if (response.ExportReportList.length > 0) {
                    AddReportsToList(response);
                    setReportListPage(prev => { return { scrollFlag: true, page: prev.page + 1 }; });
                }
            }
        });
    };

    // 把個案加入清單
    const AddReportsToList = (response) => {
        let insertReportList = [];

        response.ExportReportList.forEach((report) => {
            insertReportList.push(<ReportListItem handleReportClick={props.handleReportClick} report={report} key={report.ID} />);
        });

        setReportList(prevUserList => prevUserList.concat(insertReportList));
    };

    return (
        <>
            <form onSubmit={handleSearchSubmit} className="ui-SearchBlockWrap">
                <h4><i className="fa fa-search" aria-hidden="true" />列表搜尋</h4>
                <input value={search} onChange={handleSearchChange} placeholder="關鍵字" />
                <button>搜尋</button>
            </form>
            <div className="ui-listTabWrap">
                <UserListTab id="all" tabName="所有" selectedTab={selectedReportTab} handleOnClick={handleReportTabClick} />
                <UserListTab id="A" tabName="常規" selectedTab={selectedReportTab} handleOnClick={handleReportTabClick} />
                <UserListTab id="B" tabName="抹片" selectedTab={selectedReportTab} handleOnClick={handleReportTabClick} />
                <UserListTab id="C" tabName="Flow" selectedTab={selectedReportTab} handleOnClick={handleReportTabClick} />
                <UserListTab id="D" tabName="整合" selectedTab={selectedReportTab} handleOnClick={handleReportTabClick} />
            </div>
            <div className="ui-ItemListWrap" onScroll={handleReportListScroll}>
                <div className="scroller-status">
                    {reportList}
                    <div className="ui-ItemList" />
                </div>
            </div>
        </>
    )
}

export default ReportList;

