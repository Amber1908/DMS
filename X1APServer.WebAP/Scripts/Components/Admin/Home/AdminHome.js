import React, { useContext, useState, useEffect } from 'react';
import { Redirect } from 'react-router-dom';
import ProfileTextInput from '../../ProfileTextInput';
import { GlobalConstants } from '../../../Utilities/CommonUtility';
import { CaseInfoContext } from '../../Context';
import { Ajax } from '../../../Utilities/AjaxUtility';
import AdminWorkSpaceMain from '../AdminWorkSpaceMain';
import CheckBox from '../../ReportItem/CheckBox';
import { GetDate } from '../../../Utilities/CommonUtility';
import { SharedContext } from '../../Context';
import { URLInfoContext } from '../../Context';
import { usePostAuth } from '../../../CustomHook/CustomHook';
import RadioGroup from '../../ReportItem/RadioGroup';

const ACTION = {
    SUSPEND: 1,
    ACTIVE: 2,
    EDIT: 3,
    SAVE: 4,
    CANCEL: 5,
    REGISTER: 6,
    DELETE: 7,
    SELECT_USER: 8,
    ADD_USER: 9
};

const AdminHomeToolbar = (props) => {

    return (
        <div className="ui-toolBar-Group ui-Col-55" hidden={props.toolbarHidden}>
            <label>帳號編輯工具</label>
            {/* <div className="ui-buttonGroup">
                <button disabled={props.suspendDisabled} onClick={props.handleSuspendClick} className="btnRed" data-tooltip="停用此使用者帳號"><i className="fa fa-ban" />停用</button>
                <button disabled={props.activedDisabled} onClick={props.handleActivedClick} data-tooltip="開通此使用者帳號" className="btnGreen"><i className="fa fa-check" />開通</button>
            </div> */}
            <div className="ui-buttonGroup">
                <button disabled={props.editDisabled} onClick={props.handleEditClick} data-tooltip="編輯使用者資料"><i className="fa fa-pencil" />編輯</button>
                <button form="userForm" disabled={props.saveDisabled} className="offsetLeft5" data-tooltip="儲存"><i className="fa fa-floppy-o" />儲存</button>
                <button disabled={props.cancelDisabled} onClick={props.handleCancelClick} className="offsetLeft5" data-tooltip="取消"><i className="fa fa-times" />取消</button>
            </div>
            {/* <div className="ui-buttonGroup">
                <button disabled={props.deleteDisabled} onClick={props.handleDeleteUserClick} data-tooltip="刪除帳號"><i className="fa fa-pencil" />刪除帳號</button>
            </div> */}
        </div>
    );
}

const AdminHomeWindow = (props) => {
    let roles = [];
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
                    <span>申請時間:</span>
                    <span id="applyDate">{GetDate(props.form.CreateDate)}</span>
                    <span>開通時間:</span>
                    <span id="activeDate">{GetDate(props.form.ActivedDate)}</span>
                    <span>停用時間:</span>
                    <span id="suspendDate">{GetDate(props.form.SuspendDate)}</span>
                    <span>平台帳號</span>
                    <span id="jid">{props.form.AccID}</span>
                </div>
                <div className="ui-profileUserInfo ui-Col-65 pull-right">
                    <form onSubmit={props.handleFormSubmit} method="post" id="userForm" className={`userForm ${props.formClass}`}>
                        <ProfileTextInput handleOnChange={props.handleInputChange} value={props.form.AccName} colWidth={35} name="AccName" label="姓名" placeholder="使用者姓名" inputProps={{ ...GlobalConstants.ValidationRules.AccName, readOnly: true }} />
                        {/*<ProfileTextInput handleOnChange={props.handleInputChange} value={props.form.CellPhone} colWidth={35} name="CellPhone" label="電話" placeholder="使用者電話" inputProps={GlobalConstants.ValidationRules.CellPhone} />*/}
                        {/*<div className="formBreak"></div>*/}
                        {/* <ProfileTextInput handleOnChange={props.handleInputChange} value={props.form.AccTitle} colWidth={35} name="AccTitle" label="職位" placeholder="使用者職位" inputProps={GlobalConstants.ValidationRules.AccTitle} /> */}
                        {/* <div className="formBreak"></div> */}
                        {/*<ProfileTextInput handleOnChange={props.handleInputChange} value={props.form.Email} colWidth={65} name="Email" label="E-mail" placeholder="使用者電子信箱" inputProps={GlobalConstants.ValidationRules.Email} />*/}
                        <div className="formBreak"></div>
                        <div className="input-item noInputIcon">
                            <label>角色</label>
                            {roles}
                        </div>
                        <div className="formBreak"></div>
                        <ProfileTextInput handleOnChange={props.handleInputChange} value={props.form.DoctorNo} colWidth={35} name="DoctorNo" label="醫生/醫檢師 代碼" placeholder="醫生/醫檢師 代碼" inputProps={{ ...GlobalConstants.ValidationRules.DoctorNo, readOnly: false }} />
                        <div className="formBreak"></div>
                        <div className="input-item noInputIcon">
                            <RadioGroup handleOnChange={props.handleInputChange} value={props.form.Senior} className="item-wrap" label="醫檢師資歷" name="Senior" options={[
                                { text: "新進人員", value: "1" },
                                { text: "資深人員", value: "2" },
                            ]} />
                        </div>
                        {/*<div className="input-item noInputIcon">*/}
                        {/*    <label>表單權限</label>*/}
                        {/*    {reportAuth}*/}
                        {/*</div>*/}
                    </form>
                </div>
            </div>
        </div>
    );
}

const AdminHome = (props) => {
    // States
    // 工具列 Disabled 狀態
    const [toolbarState, setToolbarState] = useState({
        toolbarHidden: false,
        suspendDisabled: true,
        activedDisabled: true,
        editDisabled: true,
        saveDisabled: true,
        cancelDisabled: true,
        deleteDisabled: true
    });
    // 表單 Class 控制編輯狀態
    const [formClass, setFormClass] = useState("")
    // 表單 Input State
    const [form, setForm] = useState({
        CreateDate: "",
        ActivedDate: "",
        SuspendDate: "",
        AccID: "",
        AccName: "",
        AccTitle: "",
        CellPhone: "",
        Email: "",
        DoctorNo: "",
        Senior: ""
    });
    // 角色類型
    const [roles, setRoles] = useState([]);
    // 導回 Default Page
    const [redirectToDefault, setRedirectToDefault] = useState(false);
    // Refresh User Info
    const [refreshFlag, setRefreshFlag] = useState((new Date()).getTime());

    // Custom Hooks
    // Post With Auth Function
    const { PostWithAuth } = usePostAuth();

    // Contexts
    // 選取的使用者資料
    const { userInfo, handleUserInfoChange } = useContext(CaseInfoContext);
    // 更新使用者清單 Function
    const { setRefreshUserList } = useContext(SharedContext);
    // match variable
    const match = useContext(URLInfoContext);

    // Effects
    useEffect(() => {
        if (props.match.params.id != null) {


            (new Promise(GetRoleList)).then(() => {
                GetUser(props.match.params.id);
            });
        }
    }, [props.match.params.id, refreshFlag]);

    // Events
    // 停用使用者
    const handleSuspendClick = (e) => {
        stateControl(ACTION.SUSPEND);
        PostWithAuth({
            url: "/User/UpdateUseState",
            data: {
                "RequestAccID": userInfo.AccID,
                "UseState": 83,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: () => {
                alert("停用成功");
                setRefreshUserList((new Date()).getTime());
                setRefreshFlag((new Date()).getTime());
            }
        });
    };

    // 開通使用者
    const handleActivedClick = (e) => {
        stateControl(ACTION.ACTIVE);
        PostWithAuth({
            url: "/User/UpdateUseState",
            data: {
                "RequestAccID": userInfo.AccID,
                "UseState": 89,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: () => {
                alert("開通成功");
                setRefreshUserList((new Date()).getTime());
                setRefreshFlag((new Date()).getTime());
            }
        });
    };

    // 編輯使用者資料
    const handleEditClick = (e) => {
        stateControl(ACTION.EDIT);
    };

    // 取消編輯使用者資料
    const handleCancelClick = (e) => {
        stateControl(ACTION.CANCEL);

        let actived = false;
        // 若有選擇使用者 依使用者狀態變更工具欄狀態
        stateControl(ACTION.SELECT_USER);

        switch (userInfo.UseState) {
            case "Y":
                actived = true;
                break;
        }

        let formRoles = {};

        userInfo.RoleList.forEach((item) => {
            formRoles[item.RoleCode] = item.RoleCode;
        });

        let formFormat = {
            AccID: userInfo.AccID,
            AccName: userInfo.AccName,
            AccTitle: userInfo.AccTitle == null ? "" : userInfo.AccTitle,
            CellPhone: userInfo.CellPhone == null ? "" : userInfo.CellPhone,
            Email: userInfo.Email == null ? "" : userInfo.Email,
            DoctorNo: userinfo.DoctorNo == null ? "" : userInfo.DoctorNo,
            Senior: userinfo.Senior == null ? "2" : userInfo.Senior,
            RoleCode: formRoles,
            CreateDate: userInfo.CreateDate,
            ActivedDate: actived ? userInfo.StateUDate : "",
            SuspendDate: !actived ? userInfo.StateUDate : ""
        };

        setForm({
            ...formFormat,
            RoleCode: {
                ...formFormat.RoleCode
            }
        });
    };

    // 刪除使用者
    const handleDeleteUserClick = (e) => {
        if (confirm("確定刪除使用者?")) {
            stateControl(ACTION.DELETE);
            PostWithAuth({
                url: "/User/DeleteUser",
                data: {
                    RequestedAccID: userInfo.AccID,
                    "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                    "AuthCode": 1
                },
                success: () => {
                    alert("刪除成功");
                    handleUserInfoChange({});
                    setRefreshUserList((new Date()).getTime());
                    setRedirectToDefault(true);
                }
            })
        }
    };

    // 表單變更
    const handleInputChange = (e) => {
        e.persist();
        setForm(prev => {
            return {
                ...prev,
                [e.target.name]: e.target.value
            };
        })
    };

    // 角色變更
    const handleRoleChange = (e) => {
        const target = e.target;
        setForm(prev => {
            return {
                ...prev,
                RoleCode: {
                    ...prev.RoleCode,
                    [target.name]: target.value
                }
            };
        });
    }

    // 儲存使用者資訊
    const handleFormSubmit = (e) => {
        e.preventDefault();

        let roleCodes = [];
        for (let roleCode in form.RoleCode) {
            if (form.RoleCode[roleCode] != "") {
                roleCodes.push(form.RoleCode[roleCode]);
            }
        }

        stateControl(ACTION.SAVE);
        PostWithAuth({
            url: "/User/UpdateUser",
            data: {
                "RequestAccID": userInfo.AccID,
                "AccName": form.AccName,
                "AccTitle": form.AccTitle,
                "Email": form.Email,
                "CellPhone": form.CellPhone,
                "DoctorNo": form.DoctorNo,
                "Senior": form.Senior,
                "RoleCode": roleCodes,
                "FuncCode": GlobalConstants.FuncCode.Backend,
                "AuthCode": 1
            },
            success: () => {
                alert("儲存成功");
                handleUserInfoChange({ newUserInfo: form, state: "user" });
                setRefreshUserList((new Date()).getTime());
            }
        });
    };

    // Functions
    const stateControl = (action) => {
        let toolbarState;

        switch (action) {
            case ACTION.SUSPEND:
                toolbarState = {
                    suspendDisabled: true,
                    activedDisabled: false
                };
                break;
            case ACTION.ACTIVE:
                toolbarState = {
                    suspendDisabled: false,
                    activedDisabled: true
                };
                break;
            case ACTION.EDIT:
                toolbarState = {
                    editDisabled: true,
                    saveDisabled: false,
                    cancelDisabled: false
                };
                setFormClass("cfg-editorMode");
                break;
            case ACTION.SAVE:
            case ACTION.CANCEL:
                toolbarState = {
                    editDisabled: false,
                    saveDisabled: true,
                    cancelDisabled: true
                };
                setFormClass("");
                break;
            case ACTION.SELECT_USER:
            case ACTION.REGISTER:
                toolbarState = {
                    editDisabled: false,
                    saveDisabled: true,
                    cancelDisabled: true,
                    registDisabled: true,
                    deleteDisabled: false
                };
                break;
        }

        setToolbarState(prev => {
            const temp = {
                ...prev,
                ...toolbarState
            };
            return temp;
        });
    }

    const GetRoleList = (resolve, reject) => {
        PostWithAuth({
            url: "/User/GetRoleList",
            data: {
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (response) => {
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

                resolve();
            }
        })
    };

    const GetUser = (id) => {
        PostWithAuth({
            url: "/User/GetUser",
            data: {
                "RequestAccID": id,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (response) => {
                let actived = false;
                // 若有選擇使用者 依使用者狀態變更工具欄狀態
                stateControl(ACTION.SELECT_USER);

                switch (response.UseState) {
                    case "Y":
                        stateControl(ACTION.ACTIVE);
                        actived = true;
                        break;
                    case "B":
                    case "S":
                    case "N":
                        stateControl(ACTION.SUSPEND);
                        break;
                }

                // 表單填入使用者資料
                let formRoles = {};

                response.RoleList.forEach((item) => {
                    formRoles[item.RoleCode] = item.RoleCode;
                });

                let formFormat = {
                    AccID: response.AccID,
                    AccName: response.AccName,
                    AccTitle: response.AccTitle == null ? "" : response.AccTitle,
                    CellPhone: response.CellPhone == null ? "" : response.CellPhone,
                    Email: response.Email == null ? "" : response.Email,
                    DoctorNo: response.DoctorNo == null ? "" : response.DoctorNo,
                    Senior: response.Senior == null ? "2" : response.Senior,
                    RoleCode: formRoles,
                    CreateDate: response.CreateDate,
                    ActivedDate: actived ? response.StateUDate : "",
                    SuspendDate: !actived ? response.StateUDate : ""
                };

                setForm(prev => {
                    return {
                        ...prev,
                        ...formFormat,
                        RoleCode: {
                            ...prev.RoleCode,
                            ...formFormat.RoleCode
                        }
                    }
                });

                handleUserInfoChange({ newUserInfo: { ...response }, state: "user" });
            }
        });
    }

    if (redirectToDefault) {
        return (
            <Redirect to={`${match.url}`} />
        )
    }

    return (
        <AdminWorkSpaceMain
            toolbar={<AdminHomeToolbar
                handleSuspendClick={handleSuspendClick}
                handleActivedClick={handleActivedClick}
                handleEditClick={handleEditClick}
                handleCancelClick={handleCancelClick}
                handleDeleteUserClick={handleDeleteUserClick}
                {...toolbarState}
            />}
            window={<AdminHomeWindow
                formClass={formClass}
                handleInputChange={handleInputChange}
                handleFormSubmit={handleFormSubmit}
                handleRoleChange={handleRoleChange}
                form={form}
                roles={roles}
            />}
        />
    );
}

export default AdminHome;