import QuestCommon from "./QuestCommon";

import React from 'react';
import QuestContainer from "./QuestContainer";

const MultiSelectQuest = (props) => {
    return (
        <QuestContainer {...props}>
            <QuestCommon {...props} />
        </QuestContainer>
    );
}

export default MultiSelectQuest;