import React, { useState, useRef, useEffect } from 'react';
import XLSX from 'xlsx';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { GlobalConstants, GetDate, GetDateTime } from '../../Utilities/CommonUtility';

const LOAD_STATE = {
    INIT: 0,
    LOADING: 1,
    FINISH: 2
}

const acceptExcelType = [
    "application/vnd.ms-excel",
    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
]

const fileArgsInit = {
    // 題號行數
    idRowNum: 1,
    // 資料開始行數
    dataRowNum: 2
}

const tableDataInit = {
    state: LOAD_STATE.INIT,
    head: [],
    body: []
}

const outputInit = {
    state: LOAD_STATE.INIT,
    message: []
}

const ImportExcelModal = (props) => {
    // Excel 參數
    const [fileArgs, setFileArgs] = useState(fileArgsInit);
    // Tabel 資料
    const [tableData, setTableData] = useState(tableDataInit);
    // 回傳訊息
    const [output, setOutput] = useState(outputInit);
    // 是否可強制匯入
    const [canForceInsert, setCanForceInsert] = useState(false);

    const { PostWithAuthFormData } = usePostAuth();

    const formRef = useRef(null);
    const fileInputRef = useRef(null);

    // 初始化
    const init = () => {
        setFileArgs(fileArgsInit);
        setTableData(tableDataInit);
        setOutput(outputInit);
        setCanForceInsert(false);
        fileInputRef.current.value = "";
    }

    useEffect(() => {
        // 隱藏視窗時清空資料
        $("#importModal").on("hidden.bs.modal", init);

        return () => {
            $("#importModal").off("hidden.bs.modal", init);
        };
    }, []);

    // 載入資料
    const loadData = (data) => {
        let headIndex = fileArgs.idRowNum - 1,
            dataIndex = fileArgs.dataRowNum - 1;

        setTableData(prev => {
            return {
                ...prev,
                head: data[headIndex].map(x => x.toString()),
                body: data.slice(dataIndex)
            };
        });
    }

    // 讀取Excel
    const handleFile = (file) => {
        setTableData(prev => {
            return {
                ...prev,
                state: LOAD_STATE.LOADING
            }
        });

        const reader = new FileReader();
        const binaryStr = !!reader.readAsBinaryString;
        reader.onload = (e) => {
            const bstr = e.target.result;
            const wb = XLSX.read(bstr, { type: binaryStr ? 'binary' : 'array', cellDates: true });
            const wsname = wb.SheetNames[0];
            const ws = wb.Sheets[wsname];
            const data = XLSX.utils.sheet_to_json(ws, { header: 1, cellDates: true });
            loadData(data);

            setTableData(prev => {
                return {
                    ...prev,
                    state: LOAD_STATE.FINISH
                }
            });
        };
        if (binaryStr) reader.readAsBinaryString(file); else reader.readAsArrayBuffer(file);
    }

    const checkFile = (file, target) => {
        if (!acceptExcelType.some(value => value === file.type)) {
            alert("僅支援.csv,.xls,.xlsx格式!");
            target.value = "";
            return;
        }
    }

    const handleFileChange = (e) => {
        const files = e.target.files;
        checkFile(files[0], e.target);
    }

    const checkInputValue = (value) => {
        let valueInt = parseInt(value).toString();
        let valueOrigin = value.toString();

        if (valueInt === valueOrigin || value === "") return true;
        else return false;
    }

    const handleInputChange = (e) => {
        let target = e.target;

        setFileArgs(prev => {
            return {
                ...prev,
                [target.name]: target.value
            }
        });
    }

    const handleInputBlur = (e) => {
        let target = e.target;

        if (!checkInputValue(target.value)) {
            alert("行數不得為小數!");
            setFileArgs(prev => {
                return {
                    ...prev,
                    [target.name]: ""
                }
            });
        }
    }

    // 預覽資料
    const handlePreview = (e) => {
        if (!formRef.current.reportValidity()) return;

        let files = fileInputRef.current.files;
        handleFile(files[0]);
    }

    const importDataReq = (forceInsert) => {
        setOutput(prev => ({ ...prev, state: LOAD_STATE.LOADING }));

        let file = fileInputRef.current.files[0];
        let data = new FormData();
        data.append("QuestIDRowNum", fileArgs.idRowNum);
        data.append("DataStartRowNum", fileArgs.dataRowNum);
        data.append("File", file);
        data.append("ForceInsert", forceInsert);
        data.append("FuncCode", GlobalConstants.FuncCode.ViewWebsite);
        data.append("AuthCode", 1);

        PostWithAuthFormData({
            url: "/Report/ImportData",
            data: data,
            success: (response) => {
                setOutput(prev => {
                    return {
                        ...prev,
                        message: ["上傳成功!"]
                    }
                });
                formRef.current.reset();
            },
            statusError: (error) => {
                if (forceInsert) {
                    setOutput(prev => {
                        return {
                            ...prev,
                            message: ["上傳成功!"],
                            state: LOAD_STATE.FINISH
                        }
                    });
                }

                let errorMsg = [];
                if (error.FormatError != null && error.FormatError.length > 0) {
                    errorMsg = error.FormatError;
                } else {
                    errorMsg = [error.ReturnMsg];
                }

                setOutput(prev => {
                    return {
                        ...prev,
                        message: errorMsg
                    }
                });

                setCanForceInsert(error.CanForceInsert);

                if (!error.CanForceInsert) {
                    formRef.current.reset();
                }
            },
            final: () => {
                setOutput(prev => {
                    return {
                        ...prev,
                        state: LOAD_STATE.FINISH
                    }
                });
            }
        });
    }

    // 上傳資料
    const handleSubmit = (e) => {
        e.preventDefault();
        importDataReq(false);
    }

    // 取得Loading Text
    const getLoadingText = (flag, defaultText) => {
        return flag ? "Loading..." : defaultText;
    }

    let tableElement = [];
    if (tableData.body.length > 0) {
        let indexAry = Object.keys(tableData.head);

        tableData.body.forEach((row, i1) => {
            let tdElement = [];

            indexAry.forEach((colIndex, i2) => {
                let show = row[colIndex];

                if (row[colIndex] instanceof Date) {
                    let dateStr = GetDateTime(row[colIndex]);
                    show = dateStr;
                }

                tdElement.push(<td key={i2}>{show}</td>);
            });

            tableElement.push(<tr key={i1}>{tdElement}</tr>);
        });
    }

    const handleForceInsert = (e) => {
        setCanForceInsert(false);
        importDataReq(true);
    }

    return (
        <div id="importModal" className="modal fade" tabIndex={-1} role="dialog">
            <div className="modal-dialog modal-lg" role="document">
                <div className="modal-content">
                    <div className="modal-header">
                        <button type="button" className="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                        <h4 className="modal-title">匯入資料</h4>
                    </div>
                    <div className="modal-body">
                        <form ref={formRef} className="form-inline" onSubmit={handleSubmit}>
                            <div className="form-group" style={{ paddingRight: "10px" }}>
                                <label htmlFor="idRowNum">題號行數：</label>
                                <input type="number" className="form-control" id="idRowNum" name="idRowNum" style={{ width: "100px" }} value={fileArgs.idRowNum} required onChange={handleInputChange} onBlur={handleInputBlur} />
                            </div>
                            <div className="form-group" style={{ paddingRight: "10px" }}>
                                <label htmlFor="dataRowNum">資料開始行數：</label>
                                <input type="number" className="form-control" id="dataRowNum" name="dataRowNum" style={{ width: "100px" }} value={fileArgs.dataRowNum} required onChange={handleInputChange} onBlur={handleInputBlur} />
                            </div>
                            <div className="form-group">
                                <label htmlFor="importFile">檔案：</label>
                                <input ref={fileInputRef} accept=".csv,.xls,.xlsx" type="file" id="importFile" style={{ display: "inline-block" }} required onChange={handleFileChange} />
                            </div>
                            <button type="button" className="btn btn-default" disabled={tableData.state === LOAD_STATE.LOADING} style={{ marginRight: "10px" }} onClick={handlePreview}>{getLoadingText(tableData.state === LOAD_STATE.LOADING, "預覽")}</button>
                            <button type="submit" className="btn btn-primary" disabled={output.state === LOAD_STATE.LOADING}>{getLoadingText(output.state === LOAD_STATE.LOADING, "上傳")}</button>
                        </form>
                        <div hidden={tableData.state !== LOAD_STATE.FINISH}>
                            <h2 style={{ fontSize: "14px", fontWeight: "bold" }}>預覽資料</h2>
                            <div className="table-responsive" style={{ maxHeight: "300px", marginBottom: "10px" }}>
                                <table className="table" style={{ wordBreak: "keep-all" }}>
                                    <thead>
                                        <tr>
                                            {tableData.head.map(value => <th>{value}</th>)}
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {tableElement}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div hidden={output.state !== LOAD_STATE.FINISH}>
                            <h2 style={{ fontSize: "14px", fontWeight: "bold" }}>輸出訊息</h2>
                            <div style={{ maxHeight: "300px", marginBottom: "10px", overflow: "auto" }}>
                                {output.message.map(x => <div>{x}</div>)}
                            </div>
                            <button type="button" className="btn btn-danger" hidden={!canForceInsert} onClick={handleForceInsert}>忽略錯誤資料強制上傳</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default ImportExcelModal;