import React, { useState, useEffect, useContext } from 'react';
import { useHistory } from 'react-router-dom';

import { Ajax } from '../../Utilities/AjaxUtility';
import { GlobalConstants } from '../../Utilities/CommonUtility';
import { CookieContext } from '../Context';
import { hashPwd } from '../../Utilities/HashUtility';
import { useToken } from '../../CustomHook/useToken';
import { IsLoginAndSelectSite } from '../../Utilities/TokenUtility';

// 登入回傳代碼
const LoginReturnCode = {
    // 帳號錯誤
    AccountInvalid: 4100,
    // 密碼錯誤
    PasswordInvalid: 4110,
    // 帳號被停權
    AccountSuspended: 4120,
    // 密碼錯誤超過5次
    ExceedRetryLimit: 4130,
    // Email尚未驗證
    EmailNotVerify: 4150,
};

const Login = (props) => {
    const { cookies } = useContext(CookieContext);

    // 登入表單
    const [loginForm, setLoginForm] = useState({ AccID: "", Password: "", Status: GlobalConstants.Status.INIT });

    const history = useHistory();
    
    const { setToken } = useToken();

    // 若有 token 自動導至主畫面
    useEffect(() => {
        if (IsLoginAndSelectSite(cookies)) {
            redirectToIndex();
        }
    }, [])

    // 重導向至主畫面
    const redirectToIndex = () => {
        history.replace("/Index");
    }

    // 表單變更
    const handleLoginInputChange = (e) => {
        const { name, value } = e.target;
        setLoginForm(prev => {
            return {
                ...prev,
                [name]: value
            };
        });
    };

    // 登入
    const handleLoginSubmit = (e) => {
        e.preventDefault();

        // 顯示 loading 狀態
        setLoginForm(prev => ({ ...prev, Status: GlobalConstants.Status.LOADING }));
        Ajax.PostBasic({
            url: "/User/Login",
            data: {
                "AccID": loginForm.AccID,
                "AccPWD": hashPwd(loginForm.Password, loginForm.AccID)
            },
            success: function (response) {
                // 儲存 token
                setToken(response.AccID, response.UserSecurityInfo, response.Token);
                // 導至首頁
                history.replace("/SelectWeb");
            },
            statusError: function ({ ReturnCode, ReturnMsg }) {
                showErrorMessage(ReturnCode, ReturnMsg);

                // 隱藏 loading 狀態
                setLoginForm((prev) => ({ ...prev, Status: GlobalConstants.Status.INIT }));
            }
        });
    };

    // 判斷錯誤代碼，顯示對應訊息
    const showErrorMessage = (returnCode, returnMsg) => {
        switch (returnCode) {
            case LoginReturnCode.AccountInvalid:
            case LoginReturnCode.PasswordInvalid:
                alert("帳號或密碼錯誤");
                break;
            case LoginReturnCode.AccountSuspended:
                alert("帳號已被停權");
                break;
            case LoginReturnCode.ExceedRetryLimit:
                alert("密碼錯誤次數超過5次，請通知系統管理員");
                break;
            case LoginReturnCode.EmailNotVerify:
                alert("Email尚未驗證");
                break;
            default:
                alert(returnMsg);
                break;
        }
    }

    // 正在 loading
    const isLoading = () => {
        return loginForm.Status === GlobalConstants.Status.LOADING;
    }

    return (
        <div className="section">
            <div className="container">
                <div className="row">
                    <div id="ui-loginWrap" className="ui-loginWrap col-md-12">
                        <div className="ui-loginLeftPanel ui-Col-45">
                            <div className="ui-loginLogo">
                                {/* <img src="/img/logo_NHRI_small.png" /> */}
                                <h2>病程管理系統</h2>
                                <h3>Version 1.0</h3>
                            </div>
                        </div>
                        <div className="ui-loginRightPanel ui-Col-50 cfg-login step-1">
                            <div className="ui-eventLogin">
                                <div className="QForm ui-Col-80 dd">
                                    <div className="QForm-subForm">
                                        <h4>平台登入</h4>
                                        <form method="post" onSubmit={handleLoginSubmit}>
                                            <div className="ui-formWrap">
                                                <label>帳號</label>
                                                <input className="ui-Col-100" name="AccID" placeholder="請輸入您的帳號" autoComplete="username" required onChange={handleLoginInputChange} value={loginForm.AccID} />
                                                <div className="decoLine"></div>
                                            </div>
                                            <div className="ui-formWrap">
                                                <label>密碼</label>
                                                <input className="ui-Col-100" type="password" name="Password" placeholder="請輸入您的密碼" autoComplete="current-password" required onChange={handleLoginInputChange} value={loginForm.Password} />
                                                <div className="decoLine"></div>
                                            </div>
                                            <div className="ui-formWrap">
                                                <button type="submit" id="login" disabled={isLoading()}>
                                                    <i className="fa fa-refresh fa-spin" hidden={!isLoading()} style={{color: "black"}}></i>
                                                    登入
                                                </button>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Login;