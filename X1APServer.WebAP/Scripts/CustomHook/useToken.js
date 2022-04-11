import { useContext } from "react";
import { CookieContext } from "../Components/Context";
import { GlobalConstants } from "../Utilities/CommonUtility";

export const useToken = () => {
    const { setCookie } = useContext(CookieContext);

    const setToken = (accId, securityInfo, loginToken = null, sessionKey, webSn) => {
        let cookieContent = { AccID: accId, UserSecurityInfo: securityInfo, SessionKey: sessionKey, WebSn: webSn, LoginToken: loginToken };
        let OneWeekLater = new Date(new Date().getTime() + 7 * 24 * 60 * 60 * 1000);
        // 儲存Token至Cookie
        setCookie(GlobalConstants.CookieName, cookieContent, { path: "/", expires: OneWeekLater });
    }

    return { setToken };
}