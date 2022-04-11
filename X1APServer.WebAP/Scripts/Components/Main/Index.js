import React, { useState } from 'react';

import UserList from './UserList';
import WorkSpace from './WorkSpace';
import { URLInfoContext, SharedContext } from '../Context';

const Index = ({ match }) => {
    // 刷新個案清單
    const [refreshUserList, setRefreshUserList] = useState((new Date).getTime());

    return (
        <SharedContext.Provider value={{ refreshUserList, setRefreshUserList }}>
            <URLInfoContext.Provider value={match}>
                <div className="section">
                    <div className="container containerMax">
                        <div className="row">
                            <div className="col-md-12">
                                <div className="ui-monoBlock ui-cardBlock ui-FX-shadow">
                                    <UserList />
                                    <WorkSpace />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </URLInfoContext.Provider>
        </SharedContext.Provider>
    );
};

export default Index;