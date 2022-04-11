import React, { useState, useEffect, useRef, useContext } from 'react';
import { Document, Page } from 'react-pdf/dist/entry.webpack';
import PropType from 'prop-types';
import { CookieContext } from '../Context';
import { GlobalConstants } from '../../Utilities/CommonUtility';

const PDFViewer = (props) => {
    const fileInputRef = useRef();
    const [pdfState, setPdfState] = useState({
        totalPage: 1,
        pageNumber: 1
    });
    const [pageNumValue, setPageNumValue] = useState(1);
    const [fileName, setFileName] = useState("");
    const [fileContent, setFileContent] = useState(null);

    const { cookies } = useContext(CookieContext);

    useEffect(() => {
        if (props.value == null) return;

        if (typeof props.value !== "string") {
            setFileContent(props.value);
            return;
        }

        fetch(props.value, { headers: new Headers({ 'SessionKey': cookies[GlobalConstants.CookieName].SessionKey }) })
            .then((response) => {
                if (response.ok) {
                    let fileNameRsp = decodeURIComponent(response.headers.get("FileName"));
                    setFileName(fileNameRsp);
                    return response.blob();
                }
            }).then((blob) => {
                const fileUrl = URL.createObjectURL(blob);
                setFileContent(fileUrl);
            });
    }, [props.value]);

    const onDocumentLoadSuccess = ({ numPages }) => {
        setPdfState(prev => {
            return {
                pageNumber: 1,
                totalPage: numPages
            };
        });
    }

    const handlePrevClick = () => {
        setPageNum(pageNumValue - 1);
    }

    const handleNextClick = () => {
        setPageNum(pageNumValue + 1);
    }

    const handleOnChange = (e) => {
        let fakeEvent = {
            target: {
                name: e.target.name,
                value: e.target.files[0]
            }
        };

        if (fakeEvent.target.value) {
            let target = fakeEvent.target;
            if (!target.value.type.match("application/pdf")) {
                alert("檔案不是PDF格式!");
                e.target.value = "";
                return;
            }

            let onehundredMegaBytes = 104857600;
            if (target.value.size > onehundredMegaBytes) {
                alert("上傳的PDF大小不得超過 100 MB!");
                e.target.value = "";
                return;
            }

            setFileName(e.target.files[0].name);
            setPageNumValue(1);
        }

        props.handleOnChange(fakeEvent);
        // e.target.value = "";
    }

    // 刪除上傳檔案
    const handleDeleteFile = (e) => {
        fileInputRef.current.value = "";
        props.handleOnChange({target: {name: props.name, value: ""}});
    }

    const handlePageNumEnter = (e) => {
        let keyCode = (typeof e.which == "number") ? e.which : e.keyCode;;
        if (keyCode == 13) {
            e.preventDefault();
            const target = e.target;
            let pageNum = parseInt(target.value);
            setPageNum(pageNum);
        }
    }

    const handlePageNumBlur = (e) => {
        const target = e.target;
        let pageNum = parseInt(target.value);
        setPageNum(pageNum);
    }

    const setPageNum = (pageNumArg) => {
        let pageNum = pageNumArg;

        if (pageNum < 1) {
            pageNum = 1;
        } else if (pageNum > pdfState.totalPage) {
            pageNum = pdfState.totalPage;
        }

        setPageNumValue(pageNum);
        setPdfState(prev => ({ ...prev, pageNumber: pageNum }));
    }

    const handlePageNumChange = (e) => {
        setPageNumValue(e.target.value);
    }

    let downloadElement = null;
    if (typeof props.value === "string" && props.value !== "") {
        downloadElement = (
            <a className="btn btn-default" href={props.value} style={{marginRight: "10px", pointerEvents: "all", cursor: "pointer"}}>
                <i classname="fa fa-download" style={{margin: "0px"}}></i>
                下載檔案
            </a>
        );
    }

    let deleteElement = null;
    if (props.value != null && props.value !== "") {
        deleteElement = (
            <button className="btn btn-danger" type="button" onClick={handleDeleteFile}>刪除檔案</button>
        );
    }

    let fileUrl = props.value;
    if (fileUrl instanceof File) {
        fileUrl = URL.createObjectURL(props.value);
    }

    return (
        <div>
            <div style={{ display: "flex", justifyContent: "center", position: "relative" }} hidden={!props.value}>
                {/* <div style={{position: "absolute", top: "0", left: "0"}}>
                    <i classname="fa fa-download" style={{margin: "0px"}}></i>
                    {fileName}
                </div> */}
                <a href={fileUrl} className="btn btn-default pdf-download" download={fileName}>
                    <i className="fa fa-download"></i>
                    {fileName}
                </a>
                <button type="button" className="btn btn-default" onClick={handlePrevClick} style={{ marginRight: "10px", pointerEvents: "all" }}>上一頁</button>
                <button type="button" className="btn btn-default" onClick={handleNextClick} style={{ pointerEvents: "all" }}>下一頁</button>
                <div style={{position: "absolute", top: "0", right: "0"}}>
                    <input type="number" value={pageNumValue} onKeyPress={handlePageNumEnter} onBlur={handlePageNumBlur} onChange={handlePageNumChange} style={{ width: "60px", pointerEvents: "all" }} />
                    /{pdfState.totalPage}
                </div>
            </div>
            <Document file={fileContent}
                onLoadSuccess={onDocumentLoadSuccess} style={{maxWidth: "100%"}}>
                <Page pageNumber={pdfState.pageNumber} scale={1.5} />
            </Document>
            <div>
                <div>
                    {/* {downloadElement} */}
                    <input ref={fileInputRef} type="file" accept="application/pdf" name={props.name} required={props.required && !props.value} style={{display: "inline-block"}} onChange={handleOnChange} />
                    {deleteElement}
                </div>
                <p>僅能上傳小於100MB的檔案</p>
            </div>
        </div>
    );
}

PDFViewer.propTypes = {
    name: PropType.string,
    required: PropType.bool,
    value: PropType.string,
    handleOnChange: PropType.func
}

export default PDFViewer;