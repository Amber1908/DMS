import QuestCommon from "./QuestCommon";
import QuestContainer from "./QuestContainer";

import React from 'react';

const NumberQuest = (props) => {
    return (
        <QuestContainer {...props}>
            <QuestCommon {...props} />
        </QuestContainer>
    );
}

export default NumberQuest;