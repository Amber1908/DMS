import React, { useState, useEffect, useContext } from 'react';
import { Link, useRouteMatch } from 'react-router-dom';

import { GlobalConstants } from '../../Utilities/CommonUtility';

import UserListItem from './UserListItem';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { SharedContext } from '../Context';
import AuthComponent from '../AuthComponent';

const UserList = (props) => {
    const { refreshUserList } = useContext(SharedContext);

    // 個案清單
    const [userList, setUserList] = useState([]);
    // 目前頁數、是否可以取下一頁個案
    const [pageInfo, setPageInfo] = useState({ page: 0, canFetchUser: true });
    // 個案總數
    const [totalUser, setTotalUser] = useState(0);
    // 搜尋關鍵字
    const [search, setSearch] = useState("");

    const { url } = useRouteMatch();

    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        // 初始化元件抓第一頁的個案
        setPageInfo({ page: 0, canFetchUser: false });
        setUserList([]);
        GetUsers({ Page: 0 });
    }, [refreshUserList]);


    const handleUserListScroll = (e) => {
        // 若當下沒在抓取資料及快滾動至底部，抓取下一頁資料
        if (pageInfo.canFetchUser &&
            isNearBottomOfPage(e.target)) {
            setPageInfo({ ...pageInfo, canFetchUser: false });
            GetUsers({});
        }
    };

    // 是否滾動接近個案清單底部
    const isNearBottomOfPage = (target) => {
        return target.scrollHeight - target.scrollTop - target.clientHeight < 50;
    }

    // 搜尋變更
    const handleSearchChange = (e) => {
        setSearch(e.target.value);
    }

    // 搜尋個案
    const handleSearchSubmit = (e) => {
        e.preventDefault();
        setUserList([]);
        setPageInfo({ canFetchUser: true, page: 0 });
        GetUsers({ Page: 0, PUName: search });
    }

    // 依頁數(Page)及關鍵字(PUName)取回個案清單
    const GetUsers = ({ Page = pageInfo.page, PUName = null }) => {
        PostWithAuth({
            url: "/Patient/GetPatientsLazy",
            data: {
                "Page": Page,
                "RowInPage": 50,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1,
                PUName
            },
            success: (response) => {
                if (response.Patients.length > 0) {
                    AddUsersToList(response.Patients);
                    setPageInfo(prev => ({ canFetchUser: true, page: prev.page + 1 }));
                    setTotalUser(response.TotalPatient);
                }
            }
        });
    };

    // 把個案加入清單
    const AddUsersToList = (patients) => {
        let insertUserList = patients.map((patient) => (
            <UserListItem key={patient.ID} 
                caseID={patient.ID} 
                caseName={patient.PUName} 
                idno={patient.IDNo} 
                userGender={patient.Gender} 
                nextVisitTime={patient.NextVisitTime} />
        ));

        setUserList(prevUserList => prevUserList.concat(insertUserList));
    };

    return (
        <div className="ui-controllerBlock col-md-3">
            <form onSubmit={handleSearchSubmit} className="ui-SearchBlockWrap">
                <h4>
                    <i className="fa fa-search" aria-hidden="true" />
                    列表搜尋(總共 {totalUser} 個病患)
                </h4>
                <input value={search} onChange={handleSearchChange} placeholder="關鍵字" />
                <button>搜尋</button>
                {/* <button type="button" className="collapse-btn">&#60;</button> */}
            </form>
            <AuthComponent FuncCode={GlobalConstants.FuncCode.AddPatient}>
                <Link to={`${url}/Patient/New`} className="ui-listItem-Add" hidden={ window.location.pathname.indexOf('New') > 0 && window.location.pathname.indexOf('Patient') > 0 } />
            </AuthComponent>
            <div className="ui-ItemListWrap" onScroll={handleUserListScroll}>
                <div className="scroller-status">
                    {userList}
                    <div className="ui-ItemList" />
                </div>
            </div>
        </div>
    );
};

export default UserList;