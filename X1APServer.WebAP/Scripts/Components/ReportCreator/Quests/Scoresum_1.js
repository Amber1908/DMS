import React from 'react';
import TextInput from "../../ReportItem/TextInput";
import QuestContainer from './QuestContainer';

const Scoresum = (props) => {
    let indexArg = { groupIndex: props.groupIndex, questIndex: props.index };

    const handleQuestTextChange = (e) => {
        props.handleOnChange(e, indexArg);
        let fakeEvent = {
            target: {
                name: "QuestionNo",
                value: e.target.value
            },
        };
        props.handleOnChange(fakeEvent, indexArg);
    }

    return (
        <QuestContainer draggable={false} {...props}>
            <div className="edit-quest-heading">
                <TextInput
                    placeholder="問題標題"
                    required={true}
                    colwidth={60}
                    name="QuestionText"
                    value={props.QuestionText}
                    inputProps={{ maxLength: "1000" }}
                    handleOnChange={handleQuestTextChange}
                />
            </div>
        </QuestContainer>
    );
}

export default Scoresum;