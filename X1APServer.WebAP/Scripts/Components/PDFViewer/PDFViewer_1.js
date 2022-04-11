import React, { useState, useEffect } from 'react';
import { render } from 'react-dom';
import { Document, Page } from 'react-pdf/dist/entry.webpack';
import { checkPropTypes } from 'prop-types';
import PatientInfo from '../PatientInfo';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { GlobalConstants } from '../../Utilities/CommonUtility';

const PDFViewer = (props) => {
    const [pdfState, setPdfState] = useState({
        numPages: 1,
        pageNumber: 1
    });
    const [caseInfo, setCaseInfo] = useState({});

    // Custom Hook
    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        let ignore = false;

        PostWithAuth({
            url: "/Patient/GetPatientInfo",
            data: {
                "ID": document.getElementById("pid").value,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (response) => {
                if (ignore) return;

                setCaseInfo({ ...response.Patient });
            }
        });

        return () => { ignore = true; };
    }, [])

    const onDocumentLoadSuccess = ({ numPages }) => {
        setPdfState(prev => {
            return {
                pageNumber: 1,
                numPages
            };
        });
    }

    const handlePrevClick = () => {
        setPdfState(prev => {
            let pageNumber = prev.pageNumber;

            if (prev.pageNumber > 1) {
                pageNumber -= 1;
            }

            return {
                ...prev,
                pageNumber
            }
        })
    }

    const handleNextClick = () => {
        setPdfState(prev => {
            let pageNumber = prev.pageNumber;

            if (prev.pageNumber < prev.numPages) {
                pageNumber += 1;
            }

            return {
                ...prev,
                pageNumber
            }
        })
    }

    return (
        <div className={`ui-tableBlock ${props.toolbarState}`}>
            <div id="mainContainer" className="ui-workspace" style={{height: "100%"}}>
                <div className="ui-toolBar-Wrap">
                    <PatientInfo caseInfo={caseInfo} caseInfoHidden={false} />
                    <div className="ui-toolBar-Group ui-Col-30" style={{borderRightWidth: "0px"}}>
                        {/* <span className="toolbar-info"><b>頁數: </b>1/10</span> */}
                        <label><b>頁數: </b>{pdfState.pageNumber}/{pdfState.numPages}</label>
                        <div className="ui-buttonGroup">
                            <button onClick={handlePrevClick} className="offsetLeft-5" data-tooltip="上一頁">
                                <i className="fa fa-caret-left" aria-hidden="true" />
                                上一頁
                        </button>
                            <button onClick={handleNextClick} className="offsetLeft-5" data-tooltip="下一頁">
                                下一頁
                                <i className="fa fa-caret-right" aria-hidden="true" style={{ marginLeft: "5px", marginRight: "0px" }} />
                            </button>
                        </div>
                    </div>
                </div>
                <div className="QForm-Wrap" id="QFormxContainer" name="QFormContainer">
                    <div id="HistoryForm" className="Q-History ui-historyWrap ui-Col-100 cfg-historyListView">
                        <Document file={props.documentFile}
                            onLoadSuccess={onDocumentLoadSuccess}>
                            <Page pageNumber={pdfState.pageNumber} scale={1.5} />
                        </Document>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default PDFViewer;