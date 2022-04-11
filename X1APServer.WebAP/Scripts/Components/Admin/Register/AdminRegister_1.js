import React, { useState, useContext, useEffect } from 'react';
import { Redirect } from 'react-router-dom';

import ProfileTextInput from '../../ProfileTextInput';
import { GlobalConstants } from '../../../Utilities/CommonUtility';
import { Ajax } from '../../../Utilities/AjaxUtility';
import AdminWorkSpaceMain from '../AdminWorkSpaceMain';
import { URLInfoContext, CaseInfoContext, SharedContext } from '../../Context';
import { usePostAuth } from '../../../CustomHook/CustomHook';
import CheckBox from '../../ReportItem/CheckBox';
import { hashPwd } from '../../../Utilities/HashUtility';

const AdminRegisterToolbar = (props) => {

    return (
        <div className="ui-toolBar-Group ui-Col-55">
            <label>帳號編輯工具</label>
            <div className="ui-buttonGroup">
                <button form="userForm" disabled={props.registDisabled} data-tooltip="註冊"><i className="fa fa-pencil" />註冊</button>
            </div>
        </div>
    );
}

const AdminRegisterWindow = (props) => {
    const roles = [];
    let reportAuth = [];

    props.roles.forEach((item) => {
        const value = props.form.RoleCode != null ? props.form.RoleCode[item.RoleCode] : "";
        const prefix = item.RoleCode.slice(0, 4);

        if (prefix == "X1WR") {
            reportAuth.push(
                <CheckBox id={`CheckBox_${item.RoleCode}`} key={item.RoleCode} value={value} handleOnChange={props.handleRoleChange} name={item.RoleCode} trueValue={item.RoleCode} text={item.RoleTitle} />
            );
        } else {
            roles.push(
                <CheckBox id={`CheckBox_${item.RoleCode}`} key={item.RoleCode} value={value} handleOnChange={props.handleRoleChange} name={item.RoleCode} trueValue={item.RoleCode} text={item.RoleTitle} />
            );
        }
    });

    return (
        <div className="QForm-Wrap">
            <div className="QForm ui-profileWrap">
                <h4 style={{ fontWeight: 'normal', fontFamily: 'Microsoft Yahei' }}>基本個人資料</h4>
                <div className="ui-profileUserStat ui-Col-30">
                    <img src={Ajax.BaseURL + "/Content/Images/user0.png"} />
                </div>
                <div className="ui-profileUserInfo ui-Col-65 pull-right">
                    <form method="post" id="userForm" className={`userForm cfg-editorMode`} onSubmit={props.handleFormSubmit}>
                        <ProfileTextInput handleOnChange={props.handleInputChange} value={props.form.AccID} colWidth={35} name="AccID" label="帳號" placeholder="使用者帳號" inputProps={GlobalConstants.ValidationRules.AccID} />
                        <ProfileTextInput handleOnChange={props.handleInputChange} value={props.form.AccName} colWidth={35} name="AccName" label="姓名" placeholder="使用者姓名" inputProps={GlobalConstants.ValidationRules.AccName} />
                        <div className="formBreak"></div>
                        <ProfileTextInput handleOnChange={props.handleInputChange} value={props.form.AccPWD} colWidth={35} name="AccPWD" label="密碼" placeholder="使用者密碼" inputProps={GlobalConstants.ValidationRules.PassWord} />
                        <ProfileTextInput handleOnChange={props.handleInputChange} value={props.form.ConfirmAccPWD} colWidth={35} name="ConfirmAccPWD" label="確認密碼" placeholder="確認密碼" inputProps={GlobalConstants.ValidationRules.PassWord} />
                        <div className="formBreak"></div>
                        {/* <ProfileTextInput handleOnChange={props.handleInputChange} value={props.form.AccTitle} colWidth={35} name="AccTitle" label="職位" placeholder="使用者職位" inputProps={GlobalConstants.ValidationRules.AccTitle} /> */}
                        <ProfileTextInput handleOnChange={props.handleInputChange} value={props.form.CellPhone} colWidth={35} name="CellPhone" label="電話" placeholder="使用者電話" inputProps={GlobalConstants.ValidationRules.CellPhone} />
                        <div className="formBreak"></div>
                        <ProfileTextInput handleOnChange={props.handleInputChange} value={props.form.Email} colWidth={65} name="Email" label="E-mail" placeholder="使用者電子信箱" inputProps={GlobalConstants.ValidationRules.Email} />
                        <div className="input-item noInputIcon">
                            <label>角色</label>
                            {roles}
                        </div>
                        {/* <div className="input-item noInputIcon">
                            <label>報告權限</label>
                            {reportAuth}
                        </div> */}
                    </form>
                </div>
            </div>
        </div>
    );
}

const AdminRegister = (props) => {
    // States
    // 表單 Input State
    const [form, setForm] = useState({
        AccID: "",
        AccName: "",
        AccTitle: "",
        CellPhone: "",
        Email: "",
        AccPWD: "",
        ConfirmAccPWD: ""
    });
    // 導回Home頁面
    const [redirectToHome, setRedirectToHome] = useState(false);
    // 角色清單
    const [roles, setRoles] = useState([]);

    // Custom Hooks
    // Post With Auth Function
    const { PostWithAuth } = usePostAuth();

    // Contexts
    const match = useContext(URLInfoContext);
    const { handleUserInfoChange } = useContext(CaseInfoContext);
    const { setRefreshUserList } = useContext(SharedContext);

    // Effects
    useEffect(() => {
        let ignore = false;
        // 初始化點擊使用者資料
        handleUserInfoChange({ newUserInfo: null, state: "new" });
        // 取得角色清單
        GetRoleList(ignore);

        return () => { ignore = true; }
    }, [])

    // 註冊使用者
    const handleFormSubmit = (e) => {
        e.preventDefault();

        if (form.AccPWD != form.ConfirmAccPWD) {
            alert("密碼不一致");
            return;
        }

        let roleCodes = [];
        for (let roleCode in form.RoleCode) {
            if (form.RoleCode[roleCode] != "") {
                roleCodes.push(form.RoleCode[roleCode]);
            }
        }

        PostWithAuth({
            url: "/User/RegisterUser",
            data: {
                "RequestAccID": form.AccID,
                "AccPWD": hashPwd(form.AccPWD, form.AccID),
                "AccName": form.AccName,
                "AccTitle": form.AccTitle,
                "IsAdmin": false,
                "Email": form.Email,
                "CellPhone": form.CellPhone,
                "RoleCode": roleCodes
            },
            success: () => {
                alert("儲存成功");
                setRedirectToHome(true);
                setRefreshUserList((new Date).getTime());
            }
        });
    };

    const handleInputChange = (e) => {
        const target = e.target;
        setForm(prev => {
            return {
                ...prev,
                [target.name]: target.value
            };
        })
    }

    const handleRoleChange = (e) => {
        const target = e.target;
        setForm(prev => {
            return {
                ...prev,
                RoleCode: {
                    ...prev.RoleCode,
                    [target.name]: target.value
                }
            }
        })
    }

    // 取得角色清單
    const GetRoleList = (ignore) => {
        PostWithAuth({
            url: "/User/GetRoleList",
            data: {
                "FuncCode": GlobalConstants.FuncCode.Backend,
                "AuthCode": 1
            },
            success: (response) => {
                if (ignore) return;

                // 顯示角色清單
                setRoles(response.RoleList);
                // 將角色欄位填入 State
                let formRoleCol = {};

                response.RoleList.forEach((item) => {
                    formRoleCol[item.RoleCode] = "";
                });

                setForm(prev => {
                    const temp = {
                        ...prev,
                        RoleCode: {
                            ...formRoleCol,
                        }
                    };
                    return temp;
                });
            }
        })
    };

    if (redirectToHome) {
        return <Redirect to={`${match.url}/User/${form.AccID}/Home`} />;
    }

    return (
        <AdminWorkSpaceMain 
            toolbar={<AdminRegisterToolbar />}
            window={<AdminRegisterWindow 
                handleInputChange={handleInputChange}
                handleFormSubmit={handleFormSubmit}
                handleRoleChange={handleRoleChange}
                form={form}
                roles={roles}
            />} 
        />
    );
}

export default AdminRegister;