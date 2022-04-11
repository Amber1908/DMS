import QuestCommon from "./QuestCommon";
import QuestContainer from "./QuestContainer";

import React from 'react';

const TextQuest = (props) => {
    return (
        <QuestContainer {...props}>
            <QuestCommon {...props} />
        </QuestContainer>
    );
}

export default TextQuest;