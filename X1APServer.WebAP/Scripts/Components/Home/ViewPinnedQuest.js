import React from 'react';
import PropType from 'prop-types';

import { HomeMode } from './Home';

const ViewPinnedQuest = (props) => {
    return (
        <div hidden={props.hidden}>
            <div className="pinned-quest-toolbar">
                <h4>關注項目</h4>
                <div>
                    <button type="button" className="btn btn-v2" onClick={() => props.setCurrentMode(HomeMode.EditMode)}>編輯病患關注</button>
                </div>
            </div>
            <LoadingComponent status={props.pinnedQuestDataList.status}>
                <div>
                    {CreatePinGroup()}
                </div>
            </LoadingComponent>
        </div>
    )
}

ViewPinnedQuest.propTypes = {
    hidden: PropType.bool,
    setCurrentMode: PropType.func
}

export default ViewPinnedQuest;