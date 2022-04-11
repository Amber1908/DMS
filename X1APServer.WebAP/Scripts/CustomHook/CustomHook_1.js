import React, { useState, useEffect, useContext } from 'react';
import { GlobalConstants, GetDate } from '../Utilities/CommonUtility';

import { Ajax } from '../Utilities/AjaxUtility';
import { CookieContext, UserAuthContext, CaseInfoContext } from '../Components/Context';
import Log from '../Utilities/LogUtility';
import { useHistory } from 'react-router-dom';
import { useToken } from './useToken';

const AnsType = {
    Text: 1,
    File: 2
};

const useReportOperate = (paramReportID, report) => {
    const [redirectToHistory, setRedirectToHistory] = useState(false);
    // Report鎖定
    const [formLock, setFormLock] = useState({ class: "ev-lockForm", disabled: true });
    // 答案
    const [form, setForm] = useState({});
    // 答案類型
    const [formType, setFormType] = useState({});
    // Report Info
    const [reportInfo, setReportInfo] = useState({
        OrderDetail: "",
        CCSepcimenID: "",
        DiagnosisDate: "",
        Division: "",
        MorphFrom: "",
        Doctor: "",
        DiagnosisNo: "",
        Tel: "",
        TCTCSpecimenID: "",
        Specimen: "",
        InspectionDate: "",
        RecvDate: "",
        ReportDate: "",
        Site: "",
        SiteOthers: "",
        Anticoagulate: "",
        DiagnosisState: ""
    });
    // Report 匯出鎖定
    const [exportLock, setExportLock] = useState(true);

    // Custom Hooks
    // Post With Auth Function
    const { PostWithAuth, PostWithAuthFormData } = usePostAuth();

    useEffect(() => {
        // 載入答案
        const reportID = paramReportID;

        switch (report) {
            case "Smear":
                setReportInfo({ ...reportInfo, Specimen: "Bone Marrow Aspirate" });
                break;
        }

        if (reportID != null && reportID != "New") {
            let ignore = false;

            PostWithAuth({
                url: "/Report/GetReport",
                data: {
                    "ID": reportID,
                    "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                    "AuthCode": 1
                },
                success: (response) => {
                    if (ignore) return;

                    // 將答案轉換成 State 的格式
                    let answers = {};
                    let answerTypes = {};

                    response.Answers.forEach((item) => {
                        answers[item.QuestionID] = item.Value;
                        answerTypes[item.QuestionID] = AnsType.Text;
                    });

                    response.Files.forEach((item) => {
                        answers[item.QuestionID] = item.FileURL;
                        answerTypes[item.QuestionID] = AnsType.File;
                    });

                    setForm(prev => {
                        return { ...prev, ...answers };
                    });

                    setFormType(answerTypes);

                    setExportLock(response.Lock);
                }
            });

            PostWithAuth({
                url: "/Report/GetReportInfo",
                data: {
                    "ReportID": reportID,
                    "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                    "AuthCode": 1
                },
                success: (response) => {
                    if (ignore) return;

                    const DiagnosisState = {
                        N: "新判",
                        T: "追蹤",
                        R: "復發"
                    };

                    // 將回傳的診斷、醫令、檢體資料填入State
                    setReportInfo(prev => {
                        return ({
                            ...prev,
                            ...response.Order,
                            ...response.Specimen,
                            ...response.DiagnosisRecord,
                            SpecimenID: response.Specimen.ID,
                            ReportDate: GetDate(response.ReportDate),
                            DiagnosisState: DiagnosisState[response.DiagnosisRecord.State],
                            Specimen: response.Specimen.SpecimenCategory,
                            Tel: response.DoctorInfo != null ? response.DoctorInfo.Tel : ""
                        });
                    });
                }
            })

            return () => { ignore = true; };

        }
    }, [paramReportID]);

    // Input 改變儲存至 State
    const handleInputOnChange = (e) => {
        const target = e.target;
        if (target.name != null) {
            setForm(prev => {
                return {
                    ...prev,
                    [target.name]: target.value
                };
            });

            // 若是新答案判斷答案類型
            if (formType[target.name] == null) {
                let ansType = "";
                if (target.value instanceof File) {
                    ansType = AnsType.File;
                } else {
                    ansType = AnsType.Text;
                }
                setFormType(prev => {
                    return {
                        ...prev,
                        [target.name]: ansType
                    }
                })
            }
        }
    };

    const handleReportInfoChange = (e) => {
        const target = e.target;
        if (target.name != null) {
            setReportInfo(prev => {
                return {
                    ...prev,
                    [target.name]: target.value
                };
            })
        }
    };

    // 儲存 Report
    const handleSaveReportClick = (e) => {
        // 重組答案成 WebAPI 的格式
        const entries = Object.entries(form);
        let updateQuest = [];
        let updateFile = [];
        for (const [id, value] of entries) {
            // 排除非Q_開頭的id
            if (!id.startsWith(GlobalConstants.QuestionPreFix) ||
                value == null) {
                continue;
            }

            if (value instanceof File && formType[id] == AnsType.File) {
                updateFile.push({
                    QuestionID: id,
                    Value: value
                });
            } else if (formType[id] == AnsType.Text) {
                updateQuest.push({
                    QuestionID: id,
                    Value: value
                });
            }
        }

        let funcAry = [];

        if (updateQuest.length > 0) {
            funcAry.push(() => {
                Log.Debug("Upload Answer");

                return PostWithAuth({
                    url: "/Report/UpdateReport",
                    data: {
                        "ID": paramReportID,
                        "Answers": updateQuest,
                        "FuncCode": GlobalConstants.FuncCode.Report[report],
                        "AuthCode": 1
                    },
                    success: () => {

                    }
                })
            });
        }

        updateFile.forEach((element, index) => {
            let requestData = new FormData();
            requestData.append("ReportID", paramReportID);
            requestData.append("QuestionID", element.QuestionID);
            requestData.append("File", element.Value);
            requestData.append("FuncCode", GlobalConstants.FuncCode.Report[report]);
            requestData.append("AuthCode", 1);

            funcAry.push(() => {
                Log.Debug("Upload Image " + index);
                return PostWithAuthFormData({
                    url: "/Report/UpdateReportFile",
                    data: requestData
                });
            });
        });

        funcAry.push(() => {
            Log.Debug("Upload Specimen");

            return PostWithAuth({
                url: "/Report/UpdateSpecimen",
                data: {
                    "ID": reportInfo.SpecimenID,
                    "Site": reportInfo.Site,
                    "SiteOthers": reportInfo.SiteOthers,
                    "Anticoagulate": reportInfo.Anticoagulate,
                    "TCTCSpecimenID": reportInfo.TCTCSpecimenID,
                    "SpecimenCategory": reportInfo.Specimen,
                    "FuncCode": GlobalConstants.FuncCode.Report[report],
                    "AuthCode": 1
                }
            });
        });

        let failFlag = false;

        // 接續執行
        let result = funcAry.reduce((p, fn) => p.then(fn).catch(e => failFlag = true), Promise.resolve());
        return result.then(v => {
            if (failFlag) return;

            alert("儲存成功");
        });
    };

    // 編輯 Report，解鎖Report
    const handleEditClick = (e) => {
        setFormLock({
            class: "",
            disabled: false
        });
    };

    // 取消編輯 Report，鎖定Report
    const handleCancelClick = (e) => {
        setFormLock({ class: "ev-lockForm", disabled: true });
    };

    return {
        redirectToHistory,
        formLock,
        form,
        reportInfo,
        setReportInfo,
        handleInputOnChange,
        handleSaveReportClick,
        handleEditClick,
        handleCancelClick,
        handleReportInfoChange,
        exportLock,
        setExportLock
    };
};

const useAdditionalQuest = (props) => {
    const [additionalQuest, setAdditionalQuest] = useState("");

    // 使填入的值會觸發額外問題
    useEffect(() => {
        setAdditionalQuest(props.value);
    }, [props.value])

    // 切換額外問題
    const ChangeAddionalQuest = (e) => {
        setAdditionalQuest(e.target.value);
        props.handleOnChange(e);
    };

    const RecursiveCleanValue = (children) => {
        React.Children.forEach(children, child => {
            if (React.isValidElement(child)) {
                if (child.props.hasOwnProperty("children")) {
                    RecursiveCleanValue(child.props.children);
                }

                const childName = child.props.name;

                if (childName != null && child.props.value != "") {
                    let event = { target: { name: childName, value: "" } };
                    props.handleOnChange(event);
                }
            }
        });
    }

    const componentChildren = React.Children.map(props.children, child => {
        let hidden = true;
        // split 非 \, 字元
        let showValues = child.props.showvalue;
        // showValues = showValues.split(/(?<!\\),/);
        // showValues = showValues.map(element => element.replace("\\,", ","));
        if (additionalQuest != "" && showValues.indexOf(additionalQuest) > -1) {
            hidden = false;
        } else if (additionalQuest != "") {
            // 清除未顯示的額外問題
            RecursiveCleanValue(child);
        }

        return React.cloneElement(child, { hidden });
    })

    return { ChangeAddionalQuest, componentChildren };
}

const usePostAuth = () => {
    // 從Cookie取出 SecurityInfo
    const { cookies, setCookie, removeCookie } = useContext(CookieContext);

    const { setToken } = useToken();

    const RedirectHome = () => {
        Log.Debug("remove cookie");
        //TODO: 想個偵測alert的方法
        // let usercookie = cookies[GlobalConstants.CookieName];
        // if (usercookie != null) {
        //     removeCookie(GlobalConstants.CookieName, { path: "/" });
        //     // alert("登入憑證錯誤，將導回登入頁面");
        // }
        removeCookie(GlobalConstants.CookieName, { path: "/" });
    }

    const PostWithAuth = (requestBody) => {
        let requestBodyCopy = Object.assign({}, requestBody);
        let securityInfo = cookies[GlobalConstants.CookieName];

        if (securityInfo == null || securityInfo == "") {
            RedirectHome();
            return;
        }

        // if (requestBodyCopy.data == null) requestBodyCopy.data = {};

        requestBodyCopy.data.AccID = securityInfo.AccID;
        requestBodyCopy.data.UserSecurityInfo = securityInfo.UserSecurityInfo;
        requestBodyCopy.headers = { SessionKey: securityInfo.SessionKey };

        requestBodyCopy.contentType = "application/json; charset=UTF-8";
        requestBodyCopy.data = JSON.stringify(requestBodyCopy.data);

        requestBodyCopy.success = (response) => {
            // 若 AccessToken 過期，更新 Cookie 的 SecurityInfo
            if (response.TokenChgFlag) {
                // 儲存Token至Cookie
                setToken(securityInfo.AccID, response.UserSecurityInfo);
            }

            if (requestBody.success != null) {
                requestBody.success(response);
            }
        };
        requestBodyCopy.statusError = (response) => {
            // 若 Token 錯誤導回登入頁面
            if (response.ReturnCode >= GlobalConstants.WebApiStatus.AuthError && response.ReturnCode < GlobalConstants.WebApiStatus.AuthError + 1000) {
                RedirectHome();
                return;
            }

            if (requestBody.statusError != null) {
                requestBody.statusError(response);
            } else {
                let message = `${response.ReturnMsg}(錯誤碼:${response.ReturnCode})`;
                alert(message);
            }
        }
        return Ajax.PostBasic(requestBodyCopy);
    };

    const PostWithAuthFormData = (requestBody) => {
        let requestBodyCopy = Object.assign({}, requestBody);
        let securityInfo = cookies[GlobalConstants.CookieName];

        if (securityInfo == null) {
            RedirectHome();
            return;
        }

        requestBodyCopy.data.append("AccID", securityInfo.AccID);
        requestBodyCopy.data.append("UserSecurityInfo", securityInfo.UserSecurityInfo);
        requestBodyCopy.headers = { SessionKey: securityInfo.SessionKey };

        requestBodyCopy.success = (response) => {
            // 若 AccessToken 過期，更新 Cookie 的 SecurityInfo
            if (response.TokenChgFlag) {
                // 儲存Token至Cookie
                setToken(securityInfo.AccID, response.UserSecurityInfo);
            }

            if (requestBody.success != null) {
                requestBody.success(response);
            }
        };
        requestBodyCopy.statusError = (response) => {
            // 若 Token 錯誤導回登入頁面
            if (response.ReturnCode >= GlobalConstants.WebApiStatus.AuthError && response.ReturnCode < GlobalConstants.WebApiStatus.AuthError + 1000) {
                RedirectHome();
                return;
            }

            if (requestBody.statusError != null) {
                requestBody.statusError(response);
            } else {
                let message = `${response.ReturnMsg}(錯誤碼:${response.ReturnCode})`;
                alert(message);
            }
        }

        requestBodyCopy.cache = false,
            requestBodyCopy.contentType = false;
        requestBodyCopy.processData = false;
        return Ajax.PostBasic(requestBodyCopy);
    }

    return { PostWithAuth, PostWithAuthFormData };
};

const useAuthCheck = () => {
    // 使用者權限
    const userAuth = useContext(UserAuthContext);

    // 登入使用者權限驗證
    const AuthCheck = ({ tempUserAuth = userAuth, FuncCode = "", FuncName = "", AuthNos = ["AuthNo1"] }) => {
        // 只要有一項權限符合回傳 True
        return tempUserAuth.some((auth) => {
            let flag;

            if ((FuncCode != "" && FuncCode != auth.FuncCode) || (FuncName != "" && FuncName != auth.FuncName)) {
                return false;
            }

            // 驗證AuthNo是否都為 Y
            flag = AuthNos.every((authNo) => {
                if (auth[authNo] != "Y") {
                    return false;
                }

                return true;
            });

            return flag;
        })
    }

    return { userAuth, AuthCheck };
}

export { useReportOperate, usePostAuth, useAuthCheck, useAdditionalQuest };