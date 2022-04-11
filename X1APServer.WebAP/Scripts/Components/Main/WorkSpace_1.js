import React, { useState, useEffect, useContext, useRef } from "react";
import { Route, Link, Switch, useRouteMatch } from "react-router-dom";
import { GlobalConstants } from "../../Utilities/CommonUtility";

import { CaseInfoContext, URLInfoContext, CookieContext } from "../Context";
import Home from "../Home/Home";
import Tab from "../Tab";
import DefaultWorkSpace from "./DefaultWorkSpace";
import ReportHistory from "../Report/ReportHistory";
import { usePostAuth } from "../../CustomHook/CustomHook";
import AddPatient from "../AddPatient/AddPatient";
import NoMatch from "../NoMatch";
import Report from "../Report/Report";
import ReportMainList from "../ReportCreator/ReportMainList";
import ReportCreator from "../ReportCreator/ReportCreator";
import ReportVersionHistory from "../ReportCreator/ReportVersionHistory";
import ExportReport from "../ExportReport/ExportReport";
import AuthComponent from "../AuthComponent";

const initCaseInfo = {
    ID: null,
    PUID: "",
    IDNo: "",
    PUName: "個案　　",
    PUDOB: "",
    Gender: "",
};

// 每頁頁籤最大數量
const maxTabInRow = 10;

const WorkSpace = (props) => {
    const { cookies } = useContext(CookieContext);

    // 忽略非同步作業結果
    const ignoreAsyncTaskResponseRef = useRef(false);

    // 鎖定工具列 class
    const [lockToolbarClass, setLockToolbarClass] = useState("ev-workspaceUnset");
    // 鎖定頁籤 class
    const [lockTabClass, setLockTabClass] = useState("ev-workspaceLockTab");
    // 選擇的個案
    const [caseInfo, setCaseInfo] = useState(initCaseInfo);
    // 頁籤清單
    const [tabList, setTabList] = useState([]);
    // 目前頁籤起始位置
    const [startTabIndex, setStartTabIndex] = useState(0);
    // 重新整理頁籤
    const [refreshReport, setRefreshReport] = useState(new Date().getTime());

    const { path, url } = useRouteMatch();

    let { PostWithAuth } = usePostAuth();

    useEffect(() => {
        if (!isTokenExist()) return;
        ignoreAsyncTaskResponseRef.current = false;

        // 取得頁籤清單
        PostWithAuth({
            url: "/Report/GetAllReportMain",
            data: {
                isPublish: true,
                FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                AuthCode: 1,
            },
            success: (response) => {
                if (ignoreAsyncTaskResponseRef.current) return;

                // 將所有表單轉換成頁籤存至 state
                setTabList(response.ReportMainList);
            },
        });

        return () => {
            ignoreAsyncTaskResponseRef.current = true;
        };
    }, [cookies[GlobalConstants.CookieName], refreshReport]);

    // token 是否存在
    const isTokenExist = () => {
        return cookies[GlobalConstants.CookieName] != null;
    };

    useEffect(() => {
        if (isPatientSelected()) {
            // 解除 頁籤 及 工具列鎖定
            setLockToolbarClass("");
            setLockTabClass("");
        } else if (isNewPatient()) {
            // 只解除工具列鎖定
            setLockToolbarClass("");
            setLockTabClass("ev-workspaceLockTab");
        } else {
            // 鎖定頁籤及工具列
            setLockToolbarClass("ev-workspaceUnset");
            setLockTabClass("ev-workspaceLockTab");
        }
    }, [caseInfo]);

    // 是否有選擇個案
    const isPatientSelected = () => {
        return caseInfo.ID != null;
    };

    // 是新個案
    const isNewPatient = () => {
        return caseInfo.ID == null && caseInfo.PUName === "新個案";
    };

    // 選擇個案變更
    const handleCaseInfoChange = (caseInfo) => {
        caseInfo = caseInfo == null ? initCaseInfo : caseInfo;

        if (typeof caseInfo == "function") {
            setCaseInfo((prev) => caseInfo(prev));
        } else {
            setCaseInfo(caseInfo);
        }
    };

    // 下一頁頁籤
    const handleNextTab = () => {
        setStartTabIndex((prev) => {
            return prev + maxTabInRow;
        });
    };

    // 上一頁頁籤
    const handlePrevTab = () => {
        setStartTabIndex((prev) => {
            return prev - maxTabInRow;
        });
    };

    // 取得當前頁面頁籤
    const getCurrentPageTabs = () => {
        let nextPageTab = (
            <div key="prevTab" className={`ui-Tab X`} onClick={handleNextTab}>
                ...
            </div>
        );
        let prevPageTab = (
            <div key="nextTab" className={`ui-Tab X`} onClick={handlePrevTab}>
                ...
            </div>
        );

        let tabElements = [<Tab key="home" id="home" link={`${url}/Patient/${caseInfo.ID}/Home`} tabName="Home" tabToolTip="Home" />];

        let tabInRow = 1, // 目前頁面頁籤數量
            totalTab = tabList.length + 3; // 總頁籤數量
        tabList.forEach((element) => {
            tabInRow++;

            tabElements.push(
                <AuthComponent FuncCode={element.FuncCode}>
                    <Tab key={element.Category} id={element.Category} link={`${url}/Patient/${caseInfo.ID}/${element.Category}`} tabName={element.Title} tabToolTip={element.Title} />
                </AuthComponent>
            );

            // 到這頁最後一個位置仍有超過一個頁籤還未放入，加入下一頁及上一頁頁籤
            const isNotLastTab = maxTabInRow - tabInRow === 1 && totalTab - tabElements.length > 1;
            if (isNotLastTab) {
                tabElements.push(nextPageTab);
                tabElements.push(prevPageTab);
                tabInRow = 1;
            }
        });

        tabElements.push(<Tab key="exportReport" id="exportReport" link={`${url}/Patient/${caseInfo.ID}/ExportReport`} tabName="匯出報告" tabToolTip="匯出報告" />);
        // tabElements.push(
        //     <AuthComponent key="reportCreator" FuncCode={GlobalConstants.FuncCode.Backend}>
        //         <Tab id="reportCreator" link={`${url}/ReportMain`} tabName="表單產生器" tabToolTip="表單產生器" />
        //     </AuthComponent>
        // );

        return tabElements.slice(startTabIndex, startTabIndex + maxTabInRow);
    };

    return (
        <CaseInfoContext.Provider value={{ caseInfo, setCaseInfo: handleCaseInfoChange }}>
            <div className={`ui-tableBlock ${lockToolbarClass} ${lockTabClass}`}>
                <div className="ui-TabWrap">{getCurrentPageTabs()}</div>
                <div id="mainContainer" className="ui-workspace">
                    <Switch>
                        <Route exact path={`${path}`} component={DefaultWorkSpace} />
                        <Route exact path={`${path}/Patient/New`} component={AddPatient} />
                        <Route exact path={`${path}/Patient/:id/New`} component={AddPatient} />
                        <Route exact path={`${path}/Patient/:id/index`} component={DefaultWorkSpace} />
                        <Route exact path={`${path}/Patient/:id/Home`} component={Home} />
                        <Route exact path={`${path}/Patient/:id/ExportReport`} component={ExportReport} />
                        <Route exact path={`${path}/Patient/:id/:report`} component={ReportHistory} />
                        {/* <Route exact path={`${path}/Patient/:id/:report/New`} component={Report} /> */}
                        <Route exact path={`${path}/Patient/:id/:report/:reportID`} component={Report} />
                        {/* <Route exact path={`${path}/ReportMain`} render={(routerProps) => <ReportMainList setRefreshReport={setRefreshReport} {...routerProps} />} />
                        <Route exact path={`${path}/ReportMain/New`} render={(routerProps) => <ReportCreator setRefreshReport={setRefreshReport} {...routerProps} />} />
                        <Route exact path={`${path}/ReportMain/:category`} render={(routerProps) => <ReportVersionHistory setRefreshReport={setRefreshReport} {...routerProps} />} />
                        <Route exact path={`${path}/ReportMain/:category/New`} render={(routerProps) => <ReportCreator setRefreshReport={setRefreshReport} {...routerProps} />} />
                        <Route exact path={`${path}/ReportMain/:category/New/:sourceReportID`} render={(routerProps) => <ReportCreator setRefreshReport={setRefreshReport} {...routerProps} />} />
                        <Route exact path={`${path}/ReportMain/:category/:reportMID`} render={(routerProps) => <ReportCreator setRefreshReport={setRefreshReport} {...routerProps} />} /> */}
                        <Route component={NoMatch} />
                    </Switch>
                </div>
            </div>
        </CaseInfoContext.Provider>
    );
};

export default WorkSpace;
