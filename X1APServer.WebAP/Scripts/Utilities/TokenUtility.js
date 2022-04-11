import { GlobalConstants } from "./CommonUtility";
import { IsNotNullOrEmpty } from "./StringUtility";

export const IsLoginAndSelectSite = (cookie) => {
    const securityCookie = cookie[GlobalConstants.CookieName] || {};
    const result = IsNotNullOrEmpty(securityCookie.UserSecurityInfo) && 
        IsNotNullOrEmpty(securityCookie.WebSn);
    return result;
}

export const IsLogin = (cookie) => {
    const securityCookie = cookie[GlobalConstants.CookieName] || {};
    return IsNotNullOrEmpty(securityCookie.AccID);
}