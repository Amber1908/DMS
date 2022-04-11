import QuestCommon from "./QuestCommon";
import QuestContainer from "./QuestContainer";

import React from 'react';

const FileQuest = (props) => {
    return (
        <QuestContainer {...props}>
            <QuestCommon {...props} />
        </QuestContainer>
    );
}

export default FileQuest;