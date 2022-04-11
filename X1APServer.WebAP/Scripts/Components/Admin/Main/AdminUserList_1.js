import React, { useState, useEffect, useContext } from 'react';
import { Link } from 'react-router-dom';

import { GlobalConstants } from '../../../Utilities/CommonUtility';

import AdminUserListItem from './AdminUserListItem';
import UserListTab from '../../UserListTab';
import { URLInfoContext, SharedContext, CookieContext } from '../../Context';
import { usePostAuth } from '../../../CustomHook/CustomHook';

const AdminUserList = (props) => {
    // State
    const [userList, setUserList] = useState([]);
    const [selectedTab, setSelectedTab] = useState("all");
    const [roleList, setRoleList] = useState([]);
    // 搜尋使用者關鍵字
    const [search, setSearch] = useState("");

    // Contexts
    const match = useContext(URLInfoContext);
    const { refreshUserList } = useContext(SharedContext);
    const { cookies } = useContext(CookieContext)

    // Custom Hooks
    // Post With Auth Function
    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        getRoleList();
    },
        []);

    // Effects
    useEffect(
        () => {
            GetUsers({ RoleCodes: selectedTab });
        },
        [refreshUserList]
    );

    const getRoleList = () => {
        PostWithAuth({
            url: "/User/GetRoleList",
            data: {
                "FuncCode": GlobalConstants.FuncCode.Backend,
                "AuthCode": 1
            },
            success: (response) => {
                setRoleList(response.RoleList);
            }
        });
    }

    // Events
    const handleTabOnClick = (e) => {
        const id = e.target.id;
        setSelectedTab(id);
        GetUsers({ RoleCodes: id });
    };

    const handleInputChange = (e) => {
        setSearch(e.target.value);
    };

    // 點擊搜尋
    const handleSearchSubmit = (e) => {
        e.preventDefault();
        GetUsers({ AccName: search, RoleCodes: selectedTab });
    };

    // Functions
    const GetUsers = ({ AccName = undefined, RoleCodes = undefined }) => {
        RoleCodes = RoleCodes == "all" ? undefined : [RoleCodes];

        PostWithAuth({
            url: "/User/GetUserList",
            data: {
                AccName,
                RoleCodes: RoleCodes,
                "FuncCode": GlobalConstants.FuncCode.Backend,
                "AuthCode": 1
            },
            success: (response) => {
                let accid = cookies[GlobalConstants.CookieName].AccID;
                response.UserList = response.UserList.filter((item) => {
                    //if (accid == item.AccID) {
                    //    return false;
                    //}

                    return true;
                });
                AddUsersToList(response);
            }
        });
    };

    // 把使用者加入清單
    const AddUsersToList = (response) => {
        let insertUserList = [];

        response.UserList.forEach((user) => {
            insertUserList.push(<AdminUserListItem handleUserItemClick={props.handleUserItemClick} key={user.AccID} userInfo={user} />);
        });

        setUserList(insertUserList);
    };

    return (
        <>
            <form onSubmit={handleSearchSubmit} className="ui-SearchBlockWrap">
                <h4><i className="fa fa-search" aria-hidden="true" />列表搜尋</h4>
                <input onChange={handleInputChange} value={search} placeholder="關鍵字" />
                <button>搜尋</button>
            </form>
            {/* <Link to={`${match.url}/User/New`} className="ui-listItem-Add" onClick={props.handleAddUserClick} /> */}
            <div style={{ height: "100%" }}>
                <div className="ui-listTabWrap">
                    <UserListTab id="all" tabName="所有" selectedTab={selectedTab} handleOnClick={handleTabOnClick} />
                    {
                        roleList.map(role => <UserListTab id={role.RoleCode} tabName={role.RoleName} selectedTab={selectedTab} handleOnClick={handleTabOnClick} />)
                    }
                </div>
                <div className="ui-ItemListWrap">
                    <div className="scroller-status">
                        {userList}
                        <div className="ui-ItemList" />
                    </div>
                </div>
            </div>
        </>
    );
};

export default AdminUserList;