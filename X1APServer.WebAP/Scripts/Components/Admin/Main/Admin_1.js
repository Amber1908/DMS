import React, { useState } from 'react';

import AdminUserList from './AdminUserList';
import AdminWorkSpace from './AdminWorkSpace';
import { URLInfoContext, SharedContext } from '../../Context';

const Admin = ({ match }) => {
    // States
    // 選擇的 Main Tab
    const [tabselected, setTabselected] = useState({
        userID: null,
        tabselected: ""
    });
    // 刷新 UserList
    const [refreshUserList, setRefreshUserList] = useState((new Date).getTime());

    const handleUserItemClick = (e, userID) => {
        setTabselected({
            userID: userID,
            tabselected: "home"
        });
    };

    // 點新增使用者
    const handleAddUserClick = () => {
        setTabselected({
            userID: null,
            tabselected: "new"
        });
    };

    const handleTabClick = (e) => {
        let id = e.target.id;
        setTabselected(prev => {
            return {
                ...prev,
                tabselected: id
            };
        });
    };

    return (
        <SharedContext.Provider value={{ refreshUserList, setRefreshUserList }}>
            <URLInfoContext.Provider value={match}>
                <div className="section">
                    <div className="container containerMax">
                        <div className="row">
                            <div className="col-md-12">
                                <div className="ui-monoBlock ui-cardBlock ui-FX-shadow">
                                    <div className="ui-controllerBlock col-md-3">
                                        {/* <div className="ui-userBadge-Wrap">
                                            <input className="ctrl-launchWorkFeed" type="checkbox" autoComplete="off" />
                                            <div className="ui-userBadge-Tab">
                                                <div className="ui-userBadge-Avatar">
                                                    <img src={Ajax.BaseURL + "/Content/Images/doctor.png"} />
                                                </div>
                                                <div className="ui-userBadge-Info">
                                                    <span className="ui-userBadge-Name">醫檢師</span>
                                                    <span className="ui-userBadge-Pos">doctor</span>
                                                    <div className="ui-userBadge-break" />
                                                    <span className="ui-userBadge-WF hide">0</span>
                                                    <span className="ui-userBadge-Msg hide">0</span>
                                                </div>
                                            </div>
                                        </div> */}
                                        <AdminUserList refresh={refreshUserList} handleAddUserClick={handleAddUserClick} handleUserItemClick={handleUserItemClick} />
                                    </div>
                                    <AdminWorkSpace handleTabClick={handleTabClick} tabselected={tabselected.tabselected} />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </URLInfoContext.Provider>
        </SharedContext.Provider>
    );
};

export default Admin;