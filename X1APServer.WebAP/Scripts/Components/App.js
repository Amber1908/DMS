import React, { useState, useEffect, useContext, useRef } from 'react';
import { useHistory } from 'react-router-dom';
import { Route, Link, Switch, NavLink } from 'react-router-dom';

import Index from './Main/Index';
import Profile from './Profile/Profile';
import Login from './Login/Login';

import Admin from './Admin/Main/Admin';
import { GlobalConstants } from '../Utilities/CommonUtility';
import { CookieContext, UserAuthContext } from './Context';
import { usePostAuth, useAuthCheck } from '../CustomHook/CustomHook';
import Setting from './Setting/Setting';
import AuthComponent from './AuthComponent';
import NoMatch from './NoMatch';
import PrivateRoute from './PrivateRoute';
import ImportDataModal from './ImportExcel/ImportExcelDialog';
import ManageTemplate from './Admin/ManageTemplate/ManageTemplate';
import ExportExcelModal from './ExportExcel/ExportExcelModal';
import ExportCervixModal from './ExportExcel/ExportCervixModal';
import ManagePinedQuestModal from './Admin/ManagePinedQuest/ManagePinedQuestModal';
import SelectWeb from './SelectWeb';
import { IsLogin, IsLoginAndSelectSite } from '../Utilities/TokenUtility';
import ReportCreator from './ReportCreator/ReportCreator';
import ReportMainList from './ReportCreator/ReportMainList';
import ReportCreatorContainer from './ReportCreator/ReportCreatorContainer';

const initialUserProfile = {
    AccID: "",
    AccName: "",
    AccTitle: "",
    CellPhone: "",
    Email: "",
    RoleList: []
};

const initialWebInfo = {
    title: "病程管理系統",
    logo: null
};

const App = () => {
    const { cookies, removeCookie } = useContext(CookieContext);

    // 是否忽略在 useEffect 中非同步工作回傳資料
    const ignoreAsyncTaskResponseRef = useRef(false);
    // 定期更新 token interval
    const updateToken = useRef(null);

    // 登入使用者資料
    const [userProfile, setUserProfile] = useState(initialUserProfile);
    // 登入使用者權限
    const [userAuth, setUserAuth] = useState([]);
    // 顯示子宮頸匯出按鈕
    const [showCervix, setShowCervix] = useState(true);
    // 站台資訊
    const [webInfo, setWebInfo] = useState(initialWebInfo);
    // 驗證權限
    const { PostWithAuth } = usePostAuth();
    const history = useHistory();

    useEffect(() => {
        ignoreAsyncTaskResponseRef.current = false;

        if (IsLoginAndSelectSite(cookies)) {
            // 取得登入使用者資料
            getLoginUserData();
            // 取得站台資訊
            reqGetHealthWeb();
            // 設定定期更新token
            setUpdateTokenInterval();
        } else if (!IsLogin(cookies)) {
            // 清除登入使用者權限
            setUserAuth([]);
            // 導回首頁
            history.replace("/");
        }

        return () => {
            // 忽略取使用者資料回傳結果
            ignoreAsyncTaskResponseRef.current = true;
            // 取消定期更新 token
            if (updateToken.current != null) clearInterval(updateToken.current);
        };
    }, [cookies[GlobalConstants.CookieName]]);

    // 取得登入使用者資料
    const getLoginUserData = () => {
        let userData = cookies[GlobalConstants.CookieName];
        PostWithAuth({
            url: "/User/GetUser",
            data: {
                RequestAccID: userData.AccID,
                FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                AuthCode: 1,
            },
            success: (response) => {
                // component unmout 時忽略回傳結果
                if (ignoreAsyncTaskResponseRef.current) return;

                setUserProfile(response);

                setUserAuth(response.MenuInfo);

                response.MenuInfo.forEach(x => { if (x.FuncCode == "cervix" && x.AuthNo1 == "Y") { setShowCervix(false); } });
            },
        });
    };

    // 取得站台資訊
    const reqGetHealthWeb = () => {
        PostWithAuth({
            url: "/User/GetHealthWeb",
            data: {
                web_sn: cookies[GlobalConstants.CookieName].WebSn,
                FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                AuthCode: 1,
            },
            success: (rsp) => {
                setWebInfo({
                    title: rsp.Data.web_name,
                    logo: rsp.Data.logo,
                });
            },
        });
    };

    // 設定每20分鐘更新token
    const setUpdateTokenInterval = () => {
        const twentyMinutes = 1200000;
        updateToken.current = setInterval(() => {
            PostWithAuth({
                url: "/User/TokenAuthCheck",
                data: {
                    FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                    AuthCode: 1,
                },
                success: () => {},
                statusError: () => {},
            });
        }, twentyMinutes);
    };

    // 登出並清除 token
    const handleLogoutClick = () => {
        const userAccID = cookies[GlobalConstants.CookieName].AccID;

        PostWithAuth({
            url: "/User/Logout",
            data: {
                RequestedAccID: userAccID,
                FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                AuthCode: 1,
            },
            statusError: () => {},
        });
        removeCookie(GlobalConstants.CookieName, { path: "/" });

        setWebInfo(initialWebInfo);
    };

    const setRefreshReport = () => {

    }

    let toolBar = null,
        setting = null;
    if (IsLoginAndSelectSite(cookies)) {
        toolBar = (
            <div>
                <div onClick={handleLogoutClick} className="ui-logout pull-right">
                    登出
                </div>
                <AuthComponent FuncCode={GlobalConstants.FuncCode.Backend}>
                    <NavLink activeClassName="hide" className="ui-logout navbar-item pull-right" to="/Admin">
                        人員管理
                    </NavLink>
                </AuthComponent>
                <AuthComponent FuncCode={GlobalConstants.FuncCode.Backend}>
                    <NavLink activeClassName="hide" className="ui-logout navbar-item pull-right" to="/ReportCreator">
                        表單產生器
                    </NavLink>
                </AuthComponent>
                <NavLink activeClassName="hide" className="ui-logout navbar-item pull-right" to="/Index">
                    個案管理
                </NavLink>
                <div className="ui-logout pull-right" data-toggle="modal" data-target="#importModal">
                    匯入資料
                </div>
                <div className="ui-logout pull-right" data-toggle="modal" data-target="#exportModal">
                    匯出資料
                </div>
                <div className="ui-logout pull-right" data-toggle="modal" data-target="#exportCervixModal" hidden={showCervix}>
                    匯出子宮頸國健署資料
                </div>
                <div className="ui-logout pull-right" data-toggle="modal" data-target="#editPinQuestModal">
                    編輯關注
                </div>
                <Profile userProfile={userProfile} />
            </div>
        );

        setting = (
            <AuthComponent FuncCode={GlobalConstants.FuncCode.Backend}>
                <Setting />
            </AuthComponent>
        );
    }

    let logoElement = null;
    if (webInfo.logo != null) {
        logoElement = <img className="navLogo" src={`https://idoctor.tools/showimage.aspx?SN=${webInfo.logo}`} />;
    }

    return (
        <UserAuthContext.Provider value={userAuth}>
            <div className="navbar navbar-default navbar-fixed-top ui-FX-shadow">
                <div className="container">
                    <div className="navbar-header">
                        <Link to="/" className="navbar-brand">
                            {logoElement}
                            <span className="ui-biomdcare ui-gradientText"></span>
                            <span className="ui-gradientText2"> {webInfo.title}</span>
                        </Link>
                        {toolBar}
                        {setting}
                    </div>
                </div>
            </div>
            <ImportDataModal />
            <ExportExcelModal />
            <ExportCervixModal />
            <ManagePinedQuestModal />

            <Switch>
                <Route exact path="/" component={Login} />
                <Route path="/SelectWeb" component={SelectWeb} />
                <PrivateRoute path="/Index" component={Index} />
                <PrivateRoute path="/Admin" component={Admin} />
                <PrivateRoute path="/ReportCreator" component={ReportCreatorContainer} />
                <PrivateRoute path="/TemplateManage" component={ManageTemplate} />
                <Route component={NoMatch} />
            </Switch>
        </UserAuthContext.Provider>
    );
}

export default App;