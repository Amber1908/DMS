import produce from 'immer';
import React, { useEffect, useState } from 'react';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { DynamicContent, SetContent, SetInitStatus, SetLoadingStatus } from '../../Utilities/AjaxUtility';
import { GlobalConstants, NullIfEmpty } from '../../Utilities/CommonUtility';
import Loader from '../Loader';
import LoadingComponent from '../LoadingComponent';

const RoleSetting = (props) => {
    const { PostWithAuth } = usePostAuth();
    const [roleList, setRoleList] = useState(DynamicContent([]));
    const [selectRole, setSelectRole] = useState(null);
    const [roleData, setRoleData] = useState({
        RoleCode: "",
        RoleName: "",
        FunctionList: []
    });
    const [saveRoleDataStatus, setSaveRoleDataStatus] = useState(GlobalConstants.Status.INIT);
    const [functionList, setFunctionList] = useState(DynamicContent([]));

    useEffect(() => {
        reqGetRoleList();
        reqGetFunctionList();
    }, [])

    const initRoleData = (tempFuncList = functionList.Data) => {
        setRoleData(prev => ({
            RoleCode: "",
            RoleName: "",
            FunctionList: tempFuncList
        }));
    }

    const reqGetFunctionList = () => {
        SetLoadingStatus(setFunctionList);
        PostWithAuth({
            url: "/User/GetFunctionList",
            data: {
                "FuncCode": GlobalConstants.FuncCode.Backend,
                "AuthCode": 1
            },
            success: (rsp) => {
                SetContent(setFunctionList, rsp.FunctionList);

                if (roleData.FunctionList.length === 0) {
                    initRoleData(rsp.FunctionList);
                }
            },
            final: () => {
                SetInitStatus(setFunctionList);
            }
        });
    }

    const reqGetRoleList = () => {
        SetLoadingStatus(setRoleList);
        PostWithAuth({
            url: "/User/GetRoleList",
            data: {
                "FuncCode": GlobalConstants.FuncCode.Backend,
                "AuthCode": 1
            },
            success: (rsp) => {
                SetContent(setRoleList, rsp.RoleList);
            },
            final: () => {
                SetInitStatus(setRoleList);
            }
        });
    }

    const reqGetRole = (roleCode) => {
        PostWithAuth({
            url: "/User/GetRole",
            data: {
                "RoleCode": roleCode,
                "FuncCode": GlobalConstants.FuncCode.Backend,
                "AuthCode": 1
            },
            success: (rsp) => {
                setRoleData(prev => ({
                    ...prev,
                    RoleCode: rsp.Data.RoleCode,
                    RoleName: rsp.Data.RoleName,
                    FunctionList: rsp.Data.FunctionList
                }));
            }
        });
    }

    const reqAddOrUpdateRole = () => {
        setSaveRoleDataStatus(GlobalConstants.Status.LOADING);
        PostWithAuth({
            url: "/User/AddOrUpdateRole",
            data: {
                "Data": {
                    "RoleCode": roleData.RoleCode,
                    "RoleName": roleData.RoleName,
                    "FunctionList": roleData.FunctionList
                },
                "FuncCode": GlobalConstants.FuncCode.Backend,
                "AuthCode": 1
            },
            success: (rsp) => {
                alert("儲存成功!");
                reqGetRoleList();
                setSelectRole(rsp.RoleCode);
            },
            final: () => {
                setSaveRoleDataStatus(GlobalConstants.Status.INIT);
            }
        });
    }

    const handleRoleClick = (e, roleCode) => {
        setSelectRole(roleCode);
        reqGetRole(roleCode);
    }

    const handleAuthChange = (e, funcCode) => {
        const target = e.target;
        setRoleData(prev => {
            const newState = produce(prev, draft => {
                const roleCodeIndex = prev.FunctionList.findIndex(f => f.FuncCode === funcCode);
                draft.FunctionList[roleCodeIndex][target.name] = target.checked;
            });
            return newState;
        });
    }

    const handleRoleDataChange = (e) => {
        const target = e.target;
        setRoleData(prev => ({ ...prev, [target.name]: target.value }));
    }

    const handleRoleDataSubmit = (e) => {
        e.preventDefault();
        reqAddOrUpdateRole();
    }

    const handleAddRole = (e) => {
        initRoleData();
        setSelectRole(null);
    }

    const generateRoleItem = (data) => {
        let active = selectRole === data.RoleCode ? "active" : "";
        return (<button type="button" className={`list-group-item ${active}`} onClick={e => handleRoleClick(e, data.RoleCode)}>{data.RoleName}</button>)
    };

    const getMatchAuth = (funccode) => {
        const matchAuth = roleData.FunctionList.find(f => f.FuncCode === funccode);
        if (matchAuth != null) {
            return {
                "AuthNo1": matchAuth.AuthNo1,
                "AuthNo2": matchAuth.AuthNo2,
                "AuthNo3": matchAuth.AuthNo3,
                "AuthNo4": matchAuth.AuthNo4,
                "AuthNo5": matchAuth.AuthNo5
            };
        } else {
            return {
                "AuthNo1": false,
                "AuthNo2": false,
                "AuthNo3": false,
                "AuthNo4": false,
                "AuthNo5": false
            };
        }
    }

    const generateFunctionItem = (data, i) => {
        const matchAuth = getMatchAuth(data.FuncCode);

        const generateAuthItem = (authname, checked = false, name, funcCode) => {
            let item = null;
            if (!!authname) {
                item = (<label><input type="checkbox" name={name} checked={checked} onChange={e => handleAuthChange(e, funcCode)} />　{authname}</label>)
            }

            return item;
        }

        return (
            <tr>
                <th>
                    {data.FuncName}
                </th>
                <td>
                    {generateAuthItem(data.AuthName1, matchAuth.AuthNo1, "AuthNo1", data.FuncCode)}
                </td>
                <td>
                    {generateAuthItem(data.AuthName2, matchAuth.AuthNo2, "AuthNo2", data.FuncCode)}
                </td>
                <td>
                    {generateAuthItem(data.AuthName3, matchAuth.AuthNo3, "AuthNo3", data.FuncCode)}
                </td>
                <td>
                    {generateAuthItem(data.AuthName4, matchAuth.AuthNo4, "AuthNo4", data.FuncCode)}
                </td>
                <td>
                    {generateAuthItem(data.AuthName5, matchAuth.AuthNo5, "AuthNo5", data.FuncCode)}
                </td>
            </tr>
        );
    }

    const emptyView = (<div style={{ textAlign: "center" }}>無資料</div>);
    const roleListView = NullIfEmpty(roleList.Data.map(r => generateRoleItem(r))) || emptyView;
    const functionListView = NullIfEmpty(functionList.Data.map((f, i) => generateFunctionItem(f, i))) || emptyView;

    return (
        <div className="ui-Setting-Card-Wrap set-A">
            <div className="row">
                <div className="col-sm-3">
                    <button type="button" className="btn btn-default btn-block" onClick={handleAddRole} style={{ marginBottom: "10px" }}>新增角色</button>
                    <div className="list-group role-list">
                        <LoadingComponent status={roleList.Status}>
                            {roleListView}
                        </LoadingComponent>
                    </div>
                </div>
                <div className="col-sm-9" style={{ height: "100%" }}>
                    <form onSubmit={handleRoleDataSubmit}>
                        <div style={{ textAlign: "right" }}>
                            <button type="submit" className="btn btn-default">
                                <Loader status={saveRoleDataStatus} />
                                    儲存
                                </button>
                        </div>
                        <div className="form-group">
                            <label htmlFor="roleName">角色名稱</label>
                            <input className="form-control" id="roleName" name="RoleName" maxLength="20" required value={roleData.RoleName} onChange={handleRoleDataChange} />
                        </div>
                        <div className="form-group">
                            <table className="table">
                                <caption>權限</caption>
                                <thead>
                                    <th style={{ width: "20%" }}></th>
                                    <th style={{ width: "16%" }}></th>
                                    <th style={{ width: "16%" }}></th>
                                    <th style={{ width: "16%" }}></th>
                                    <th style={{ width: "16%" }}></th>
                                    <th style={{ width: "16%" }}></th>
                                </thead>
                                <tbody>
                                    {functionListView}
                                </tbody>
                            </table>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}

export default RoleSetting;