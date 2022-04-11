import { useContext, useEffect } from "react"
import { CookieContext } from '../Components/Context';
import { GlobalConstants } from "../Utilities/CommonUtility";

export const useAuthEffect = (effect, dep) => {
    const { cookies } = useContext(CookieContext);

    useEffect(() => {
        if (cookies[GlobalConstants.CookieName] != null && cookies[GlobalConstants.CookieName] != "") {
            effect();
        }
    }, [cookies[GlobalConstants.CookieName], ...dep]);
}