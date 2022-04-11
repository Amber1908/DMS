import React, { useState, useEffect, useRef } from 'react';
import PropType from 'prop-types';

import { Ajax } from '../../Utilities/AjaxUtility';
import { GlobalConstants } from '../../Utilities/CommonUtility';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { hashPwd } from '../../Utilities/HashUtility';

// 個人資料狀態
const PROFILE_STATUS = {
    // 檢視個人資料
    VIEW_PROFILE: 0,
    // 編輯個人資料
    EDIT_PROFILE: 1,
    // 變更密碼
    EDIT_PASSWORD: 2
}

const initialProfileItemStatus = {
    editBtnHidden: false,
    saveBtnHidden: true,
    cancelBtnHidden: true,
    chgPwdBtnHidden: false,
    profileFormClass: "",
    profileMainClass: "",
    changePwdFormClass: ""
};

const initialPasswordForm = {
    nowPassword: "",
    newPassword: "",
    confirmNewPassword: ""
};

const Profile = (props) => {
    // 變更前個人資料
    const originalProfileForm = useRef(null);
    // 隱藏個人資料視窗
    const [profileHidden, setProfileHidden] = useState(true);
    // 個人資料各元件狀態
    const [profileItemStatus, setProfileItemStatus] = useState(initialProfileItemStatus);
    // 個人資料
    const [userProfileForm, setUserProfileForm] = useState({ ...props.userProfile });
    // 變更密碼資料
    const [passwordForm, setPasswordForm] = useState(initialPasswordForm);

    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        setUserProfileForm({ ...props.userProfile });
    }, [props.userProfile]);

    // 點"個人資料"，顯示視窗
    const handleProfileClick = () => {
        setProfileHidden(prev => !prev);
    };

    // 關閉個人資料視窗
    const closeProfile = () => {
        setProfileHidden(true);
    }

    // 點"編輯"，切換元件顯示
    const handleEditClick = () => {
        controlToolBar(PROFILE_STATUS.EDIT_PROFILE);
        // 暫存原本的使用者資料
        originalProfileForm.current = userProfileForm;
    };

    // 點"儲存"，儲存變更資料
    const handleProfileSubmit = (e) => {
        e.preventDefault();
        PostWithAuth({
            url: "/User/UpdateUser",
            data: {
                "RequestAccID": props.userProfile.AccID,
                "AccName": userProfileForm.AccName,
                "AccTitle": userProfileForm.AccTitle,
                "Email": userProfileForm.Email,
                "CellPhone": userProfileForm.Cellphone,
                "RoleCode": props.userProfile.RoleList.map((role) => role.RoleCode),
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: () => {
                controlToolBar(PROFILE_STATUS.VIEW_PROFILE);
                alert("儲存成功");
            }
        });
    };

    // 點"取消"，切換元件顯示
    const handleCancelClick = () => {
        controlToolBar(PROFILE_STATUS.VIEW_PROFILE);
        // 填回原本資料
        setUserProfileForm(originalProfileForm.current);
    };

    // 點"更新密碼"，更新新密碼
    const handleChgPwdSubmit = (e) => {
        e.preventDefault();

        if (!checkPassword()) return;

        PostWithAuth({
            url: "/User/UpdateUserPassword",
            data: {
                "OldPassword": hashPwd(passwordForm.nowPassword, props.userProfile.AccID),
                "NewPassword": hashPwd(passwordForm.newPassword, props.userProfile.AccID),
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: () => {
                controlToolBar(PROFILE_STATUS.VIEW_PROFILE);
                setPasswordForm(initialPasswordForm);
                alert("儲存成功");
            }
        });
    };

    const checkPassword = () => {
        if (passwordForm.newPassword !== passwordForm.confirmNewPassword) {
            alert("密碼不一致");
            return false;
        }

        if (passwordForm.newPassword === passwordForm.nowPassword) {
            alert("新密碼不得與舊密碼一致");
            return false;
        }

        return true;
    }

    // 處理 Profile Input 變更
    const handleProfileFormChange = (e) => {
        let { name, value } = e.target;
        setUserProfileForm({
            ...userProfileForm,
            [name]: value
        });
    };

    // 處理 Password Input 變更
    const handlePasswordFormChange = (e) => {
        let { name, value } = e.target;
        setPasswordForm({
            ...passwordForm,
            [name]: value
        });
    };

    // 點密碼的"取消"，清空資料切換元件顯示
    const handleChgPwdCancelClick = () => {
        controlToolBar(PROFILE_STATUS.VIEW_PROFILE);
        setPasswordForm(initialPasswordForm);
    };

    // 控制個人資料元件顯示
    const controlToolBar = (mode) => {
        switch (mode) {
            case PROFILE_STATUS.VIEW_PROFILE:
                setProfileItemStatus(initialProfileItemStatus);
                break;
            case PROFILE_STATUS.EDIT_PROFILE:
                setProfileItemStatus(prev => {
                    return {
                        ...prev,
                        editBtnHidden: true,
                        saveBtnHidden: false,
                        cancelBtnHidden: false,
                        chgPwdBtnHidden: true,
                        profileFormClass: "cfg-editorMode"
                    };
                });
                break;
            case PROFILE_STATUS.EDIT_PASSWORD:
                setProfileItemStatus(prev => {
                    return {
                        ...prev,
                        editBtnHidden: true,
                        chgPwdBtnHidden: true,
                        profileMainClass: "cfg-changePassword",
                        changePwdFormClass: "cfg-editorMode"
                    };
                });
                break;
        }
    };

    return (
        <div className="myProfileWrap pull-right">
            <img src={Ajax.BaseURL + "/Content/Images/user0.png"} onClick={handleProfileClick} />
            <span className="myUsername" onClick={handleProfileClick}>
                個人資料
            </span>
            <div className="UFormWrap" hidden={profileHidden}>
                <div id="profileMain" className={`UForm ui-profileWrap ${profileItemStatus.profileMainClass}`}>
                    <h4>基本個人資料</h4>
                    <button className="profile close fa fa-times" type="button" onClick={closeProfile}></button>
                    <div className="ui-Col-100">
                        <button onClick={handleEditClick} hidden={profileItemStatus.editBtnHidden}>
                            編輯
                        </button>
                        <button type="submit" form="profile" hidden={profileItemStatus.saveBtnHidden}>
                            儲存
                        </button>
                        <button onClick={handleCancelClick} hidden={profileItemStatus.cancelBtnHidden}>
                            取消
                        </button>
                        <button onClick={() => controlToolBar(PROFILE_STATUS.EDIT_PASSWORD)} hidden={profileItemStatus.chgPwdBtnHidden}>變更密碼</button>
                    </div>
                    <div className="ui-profileUserInfo ui-Col-100" id="uProfile">
                        <form onSubmit={handleProfileSubmit} id="profile" method="post" className={`identityForm ${profileItemStatus.profileFormClass}`}>
                            <div className="input-item ui-Col-40 noInputIcon">
                                <label>帳號</label>
                                <input name="AccID" placeholder="帳號" value={props.userProfile.AccID} readOnly />
                            </div>
                            <div className="input-item ui-Col-40">
                                <label>姓名</label>
                                <input value={userProfileForm.AccName} onChange={handleProfileFormChange} name="AccName" placeholder="姓名" readOnly {...GlobalConstants.ValidationRules.AccName} />
                            </div>
                            <div className="input-item ui-Col-40 noInputIcon">
                                <label>角色類型</label>
                                {props.userProfile.RoleList.map((role) => (
                                    <div key={role.RoleCode}>{role.RoleTitle}</div>
                                ))}
                            </div>
                            <div className="formBreak" />
                            {/*<div className="input-item ui-Col-40">*/}
                            {/*    <label>電話</label>*/}
                            {/*    <input*/}
                            {/*        value={userProfileForm.Cellphone}*/}
                            {/*        onChange={handleProfileFormChange}*/}
                            {/*        type="tel"*/}
                            {/*        name="Cellphone"*/}
                            {/*        placeholder="電話"*/}
                            {/*        {...GlobalConstants.ValidationRules.CellPhone}*/}
                            {/*    />*/}
                            {/*</div>*/}
                            {/*<div className="input-item ui-Col-80">*/}
                            {/*    <label>Email</label>*/}
                            {/*    <input value={userProfileForm.Email} onChange={handleProfileFormChange} type="email" name="Email" placeholder="Email" {...GlobalConstants.ValidationRules.Email} />*/}
                            {/*</div>*/}
                            {/*<div className="formBreak" />*/}
                        </form>
                    </div>
                    <div className="ui-profileUserInfo ui-Col-100" id="uPwdProfile">
                        <form onSubmit={handleChgPwdSubmit} method="post" id="identityPwdForm" className={profileItemStatus.changePwdFormClass}>
                            <div className="input-item" hidden>
                                <label>帳號</label>
                                <input name="AccID" placeholder="帳號" value={props.userProfile.AccID} autoComplete="username" readOnly />
                            </div>
                            <div className="input-item ui-Col-60">
                                <label>目前密碼</label>
                                <input
                                    value={passwordForm.nowPassword}
                                    onChange={handlePasswordFormChange}
                                    name="nowPassword"
                                    placeholder="目前密碼"
                                    type="password"
                                    autoComplete="current-password"
                                    required
                                />
                            </div>
                            <div className="input-item ui-Col-60">
                                <label>新密碼</label>
                                <p>請輸入4~16個字元，不得與舊密碼相同</p>
                                <input
                                    value={passwordForm.newPassword}
                                    onChange={handlePasswordFormChange}
                                    name="newPassword"
                                    placeholder="新密碼"
                                    type="password"
                                    autoComplete="new-password"
                                    {...GlobalConstants.ValidationRules.PassWord}
                                />
                            </div>
                            
                            <div className="input-item ui-Col-60">
                                <label>確認新密碼</label>
                                <input
                                    value={passwordForm.confirmNewPassword}
                                    onChange={handlePasswordFormChange}
                                    name="confirmNewPassword"
                                    placeholder="確認新密碼"
                                    type="password"
                                    autoComplete="new-password"
                                    required
                                    {...GlobalConstants.ValidationRules.PassWord}
                                />
                            </div>
                        </form>
                        <div className="ui-Col-100">
                            <button type="submit" form="identityPwdForm">
                                更新密碼
                            </button>
                            <button onClick={handleChgPwdCancelClick}>取消</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

Profile.prototype = {
    profileData: {
        AccID: "",
        AccName: "",
        AccTitle: "",
        CellPhone: "",
        Email: "",
        Cellphone: "",
        RoleList: [],
    },
};

Profile.propTypes = {
    profileData: PropType.objectOf({
        AccID: PropType.string,
        AccName: PropType.string,
        AccTitle: PropType.string,
        CellPhone: PropType.string,
        Email: PropType.string,
        RoleList: PropType.array
    })
}

export default Profile;