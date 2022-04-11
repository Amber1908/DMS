import React, { useState, useEffect } from 'react';
import { render } from 'react-dom';
import PDFViewer from './PDFViewer';
import ReportList from './ReportList';
import { Ajax } from '../../Utilities/AjaxUtility';
import { useCookies, CookiesProvider } from 'react-cookie';
import { CookieContext } from '../Context';
import { GlobalConstants } from '../../Utilities/CommonUtility';

const Root = () => {
    const [documentFile, setDocumentFile] = useState("");
    const [toolbarState, setToolbarState] = useState("")

    const [cookies, setCookie, removeCookie] = useCookies([GlobalConstants.CookieName]);
    
    useEffect(() => {
        let usercookie = cookies[GlobalConstants.CookieName];
        if (usercookie == null || cookies[GlobalConstants.CookieName] == null || cookies[GlobalConstants.CookieName] === "") {
            window.location = Ajax.BaseURL;
        }
    }, [cookies])

    const handleReportClick = (e, guid, mimetype) => {
        switch (mimetype) {
            case "application/pdf":
                setDocumentFile(`${Ajax.BaseURL}/ReportPDF/${guid}`);
                setToolbarState("");
                break;
        }
    }

    return (
        <CookiesProvider>
            <CookieContext.Provider value={{ cookies, setCookie, removeCookie }}>
                <div className="navbar navbar-default navbar-fixed-top ui-FX-shadow" style={{ background: 'rgba(255,255,255,1)  4px #3bc8f7' }}>
                    <div className="container">
                        <div className="navbar-header" style={{ width: '100%' }}>
                            <div className="navbar-brand">
                                <img className="navLogo" src={Ajax.BaseURL + "/Content/Images/TCTC Logo.png"} />
                                <span className="ui-biomdcare ui-gradientText"><b>TCTC</b></span><span className="ui-gradientText2"> 台成細胞中心-報告檢視器</span>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="section">
                    <div className="container containerMax">
                        <div className="row">
                            <div className="col-md-12">
                                <div className="ui-monoBlock ui-cardBlock ui-FX-shadow">
                                    <div className="ui-controllerBlock col-md-3">
                                        <ReportList handleReportClick={handleReportClick} />
                                    </div>
                                    <PDFViewer documentFile={documentFile} toolbarState={toolbarState}/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </CookieContext.Provider>
        </CookiesProvider>
    )
}

render(<Root />, document.getElementById("App"));