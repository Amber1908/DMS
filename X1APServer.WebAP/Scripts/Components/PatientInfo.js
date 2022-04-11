import React, { useContext, useEffect, useState } from 'react';
import { Ajax } from '../Utilities/AjaxUtility';
import { GetROC, GlobalConstants } from '../Utilities/CommonUtility';
import { usePostAuth } from '../CustomHook/CustomHook';
import { useParams } from 'react-router';
import { CaseInfoContext } from './Context';
import { Link } from 'react-router-dom';

const PatientInfo = (props) => {
    // 個案ID
    const { id } = useParams();
    // 隱藏個案元件
    const [caseInfoHidden, setCaseInfoHidden] = useState(true);
    // 個案資料
    const { caseInfo, setCaseInfo } = useContext(CaseInfoContext);

    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        if (id == null) return;

        let ignore = false;

        // 取得個案資料
        PostWithAuth({
            url: "/Patient/GetPatientInfo",
            data: {
                "ID": id,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (response) => {
                // component unmount 忽略回傳內容
                if (ignore) return;

                setCaseInfo({ ...response.Patient });
                // 顯示個案資料
                setCaseInfoHidden(false);
            }
        });

        return () => { ignore = true; };
    }, [id])

    // 轉換性別文字
    const getGenderText = (gender) => {
        let result = "未知";

        if (gender === "M") {
            result = "Male";
        } else if (gender === "F") {
            result = "Female";
        }
        
        return result;
    }

    // 轉換生日為年齡
    const getAgeText = (birthDateText) => {
        let birthDate = new Date(birthDateText);
        let now = new Date();
        let ageYear = now.getFullYear() - birthDate.getFullYear();
        let ageMonth;

        if (now.getMonth() < birthDate.getMonth()) {
            ageYear -= 1;
            ageMonth = now.getMonth() + 12 - birthDate.getMonth();
        } else {
            ageMonth = now.getMonth() - birthDate.getMonth();
        }

        return `${ageYear}歲${ageMonth}個月`;
}

    // 轉換資料為顯示格式
    let gender = "未知", 
        birthDateText = "未知", 
        ageText = "未知",
        contactInfo = "";

    if (caseInfo != null) {
        gender = getGenderText(caseInfo.Gender);
        birthDateText = GetROC(caseInfo.PUDOB, "ROC.MM.DD");
        ageText = getAgeText(caseInfo.PUDOB);

        if (caseInfo.ContactRelation)
            contactInfo = `${caseInfo.ContactRelation}的電話`
    }
    
    return (
        <>
            <div className="ui-toolBar-Group">
                <div className="ui-toolBar-Avatar">
                    <img src={Ajax.BaseURL + "/Content/Images/user0.png"} />
                </div>
                <span className="toolBar-name">{caseInfo.PUName}</span>
            </div>
            <div className="ui-toolBar-Group" hidden={caseInfoHidden}>
                {/* 身分證號 */}
                <span className="toolbar-info"><b>ID: </b>{caseInfo.IDNo}</span>
                {/* 連絡電話 */}
                <span className="toolbar-info"><b>Cell: </b>{caseInfo.Phone}</span><br />
                {/* 性別 */}
                <span className="toolbar-info"><b>Gender: </b> {gender}</span>
                {/* 生日 */}
                <span className="toolbar-info"><b>DOB: </b> {birthDateText}</span>
                {/* 年齡 */}
                <span className="toolbar-info"><b>Age: </b> {ageText}</span>
                <br />
                {/* 緊急聯絡人資訊 */}
                <span className="toolbar-info"><b>ECI: </b> {contactInfo}{caseInfo.ContactPhone}</span>
            </div>
        </>
    );
}

export default PatientInfo;