import React, { useContext, useEffect, useRef, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { useToken } from '../CustomHook/useToken';
import { Ajax } from '../Utilities/AjaxUtility';
import { GetDate, GlobalConstants } from '../Utilities/CommonUtility';
import { IsLogin, IsLoginAndSelectSite } from '../Utilities/TokenUtility';
import { CookieContext } from './Context';
import Loader from './Loader';
import LoadingComponent from './LoadingComponent';

const SelectWeb = (props) => {
    const { cookies } = useContext(CookieContext);

    const ignoreAsyncRequestRef = useRef(false);

    // 站台清單
    const [siteList, setSiteList] = useState({ data: [], status: GlobalConstants.Status.INIT });
    // 選擇站台狀態
    const [selectWebStatus, setSelectWebStatus] = useState(GlobalConstants.Status.INIT);

    const history = useHistory();

    const { setToken } = useToken();

    useEffect(() => {
        ignoreAsyncRequestRef.current = false;

        if (!IsLogin(cookies)) {
            // 未登入導回登入頁
            history.replace("/");
            return;
        } else if (IsLoginAndSelectSite(cookies)) {
            // 已登入且已選擇過站台導至首頁
            history.replace("/Index");
            return;
        }

        // 取得站台清單
        getHealthWebList();

        return () => { ignoreAsyncRequestRef.current = true; }
    }, [cookies[GlobalConstants.CookieName]]);

    // 取得使用者擁有的站台清單
    const getHealthWebList = () => {
        // 設定loading狀態
        setSiteList((prev) => ({ ...prev, status: GlobalConstants.Status.LOADING }));
        Ajax.PostBasic({
            url: "/User/GetHealthWebByUser",
            data: {
                Email: cookies[GlobalConstants.CookieName].AccID,
            },
            method: "GET",
            success: (rsp) => {
                if (ignoreAsyncRequestRef.current) return;

                setSiteList((prev) => ({ ...prev, data: rsp.Data }));
            },
            final: () => {
                // 取消loading狀態
                setSiteList((prev) => ({ ...prev, status: GlobalConstants.Status.INIT }));
            },
        });
    };

    const handleWebOnclick = (e, web_sn) => {
        let { AccID, LoginToken } = cookies[GlobalConstants.CookieName];

        // 設定loading狀態
        setSelectWebStatus(GlobalConstants.Status.LOADING);
        Ajax.PostBasic({
            url: "/User/GetToken",
            data: {
                AccID: AccID,
                Web_sn: web_sn,
                LoginToken: LoginToken,
            },
            success: (rsp) => {
                // 儲存token至cookie
                setToken(AccID, rsp.UserSecurityInfo, null, rsp.SessionKey, web_sn);
                // 導至首頁
                history.replace(`/Index`);
            },
            statusError: (errorRsp) => {
                showErrorMessage(errorRsp);
            },
            final: () => {
                // 取消loading狀態
                setSelectWebStatus(GlobalConstants.Status.INIT);
            },
        });
    };

    const showErrorMessage = (rsp) => {
        switch (rsp.ReturnCode) {
            case 4160:
                alert("登入憑證錯誤將導回登入頁面!");
                history.replace("/");
                break;
            default:
                alert(rsp.ReturnMsg);
                break;
        }
    }

    // 站台被停用?
    const isSiteDisabled = (site) => {
        return site.status === 2;
    };

    return (
        <div className="section">
            <div className="container">
                <div className="panel panel-default select-web">
                    <div className="panel-body">
                        <h1>
                            請選擇站台
                            <Loader status={selectWebStatus} />
                        </h1>
                        <LoadingComponent status={siteList.status}>
                            <div className="list-group">
                                {siteList.data.map((site) => (
                                    <button
                                        key={site.web_sn}
                                        type="button"
                                        className="list-group-item"
                                        disabled={isSiteDisabled(site) || selectWebStatus === GlobalConstants.Status.LOADING}
                                        onClick={(e) => handleWebOnclick(e, site.web_sn)}
                                    >
                                        {site.web_name}
                                        <br />
                                        建立時間: {GetDate(site.createtime)}
                                    </button>
                                ))}
                            </div>
                        </LoadingComponent>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default SelectWeb;