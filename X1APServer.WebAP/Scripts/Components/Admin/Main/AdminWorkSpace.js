import React, { useState, useEffect, useContext, useRef } from 'react';
import { Route, Switch } from "react-router-dom";
import { Ajax } from '../../../Utilities/AjaxUtility';
import { GlobalConstants } from '../../../Utilities/CommonUtility';

import { CaseInfoContext, URLInfoContext } from '../../Context';
import Tab from '../../Tab';
import AdminHome from '../Home/AdminHome';
import AdminDefaultWorkSpace from './AdminDefaultWorkSpace';
import AdminRegister from '../Register/AdminRegister';
import NoMatch from '../../NoMatch';

const initUserInfo = {
    "AccID": "",
    "UserGUID": "",
    "AccName": "",
    "AccTitle": "",
    "Email": "",
    "CellPhone": "",
    "UseState": "",
    "UserState": "",
    "IsAdmin": "",
    "State": "init"
};

const AdminWorkSpace = (props) => {
    // State
    const [WToolbarActived, setWToolbarActived] = useState("ev-workspaceUnset");
    const [WTabActived, setWTabActived] = useState("ev-workspaceLockTab");
    const [userInfo, setUserInfo] = useState(initUserInfo);

    let match = useContext(URLInfoContext);

    useEffect(
        () => {
            if (userInfo.State == "init") {
                setWToolbarActived("ev-workspaceUnset");
                setWTabActived("ev-workspaceLockTab");  
            } else if (userInfo.State == "user") {
                setWTabActived("");
                setWToolbarActived("");
            } else if (userInfo.State == "new") {
                setWToolbarActived("");
                setWTabActived("ev-workspaceLockTab");  
            }
        }, [userInfo]
    );

    const handleUserInfoChange = ({newUserInfo, state = "init"}) => {
        newUserInfo = newUserInfo == null ? initUserInfo : newUserInfo;

        setUserInfo(prev => {
            return {
                ...prev,
                ...newUserInfo,
                State: state
            };
        });
    };

    // if (tabselected != "") {
    //     return <Redirect push to="/sample" />;
    // }

    return (
        <CaseInfoContext.Provider value={{userInfo, handleUserInfoChange}}>
            <div className={[WToolbarActived, WTabActived, "ui-tableBlock"].join(" ")}>
                <div className="ui-TabWrap">
                    <Tab hidden={true} id="new" link={`${match.url}/User/New`} selected={props.tabselected == "new" ? "Y" : ""} />
                    <Tab id="home" handleTabClick={props.handleTabClick} link={`${match.url}/User/${userInfo.AccID}/Home`} selected={props.tabselected == "home" ? "Y" : ""} tabName="Home" tabToolTip="使用者資訊" />
                </div>
                <div id="mainContainer" className="ui-workspace">
                    <div className="ui-contentLoader hide">
                        <span className="hide">還沒載入完成？<br />請嘗試 <button className="ev-forceReload">刷新頁面</button> 或檢查您的網路連接</span>
                        <a href="http://google.com.tw" id="downloadReport">下載報表</a>
                    </div>
                    <Switch>
                        <Route exact path={`${match.url}`} component={AdminDefaultWorkSpace} />
                        <Route exact path={`${match.url}/User/:id/Home`} component={AdminHome} />
                        <Route exact path={`${match.url}/User/New`} component={AdminRegister} />
                        <Route component={NoMatch} />
                    </Switch>
                </div>
            </div>
        </CaseInfoContext.Provider>
    );
};

export default AdminWorkSpace;