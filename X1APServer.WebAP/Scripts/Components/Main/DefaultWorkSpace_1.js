import React from 'react';

import WorkSpaceMain from '../WorkSpaceMain';

const DefaultToolbar = (props) => {

    // 新增表單
    const handleEditUser = (e) => {
        window.location = 'New';
    };

    return (
        <>
            <div className="ui-toolBar-Group ui-Col-30">
                <label>工具</label>
                <div className="ui-buttonGroup">
                    <button onClick={handleEditUser} className="offsetLeft-5" data-tooltip="編輯個案基本資料">
                        <i className="fa fa-pencil" aria-hidden="true" />
                        編輯
                    </button>
                </div>
            </div>
        </>
    );
};

const DefaultWorkSpace = (props) => {
    return (
        <WorkSpaceMain toolbar={<DefaultToolbar />} />
    );
};

export default DefaultWorkSpace;