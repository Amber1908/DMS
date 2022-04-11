import React from 'react';
import { render } from 'react-dom';
import { BrowserRouter as Router } from 'react-router-dom';
import { CookiesProvider } from 'react-cookie';
import { useCookies } from 'react-cookie';
import { Ajax } from '../Utilities/AjaxUtility';
import App from './App';
import { GlobalConstants } from '../Utilities/CommonUtility';
import { CookieContext } from './Context';

const Root = () => {
    const [cookies, setCookie, removeCookie] = useCookies([GlobalConstants.CookieName]);
    
    return (
        <Router basename={Ajax.BaseURL}>
            <CookiesProvider>
                <CookieContext.Provider value={{ cookies, setCookie, removeCookie }}>
                    <App />
                </CookieContext.Provider>
            </CookiesProvider>
        </Router>
    )
}

render(<Root />, document.getElementById("App"));