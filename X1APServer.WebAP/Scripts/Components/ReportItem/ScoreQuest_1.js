import React from 'react';
import PropType from 'prop-types';

// 分數題
const ScoreQuest = (props) => {
    let questElements = [];
    props.quests.forEach(element => {
        questElements.push(
            <div>
                <h4>{element.quest}</h4>
                <p>{element.description}</p>
                <input type="number" name={element.name} required={element.required} />
            </div>
        )
    });

    return (
        <div>
            <h3>{props.title}</h3>
            <p>{props.description}</p>
            <div>
                {questElements}
            </div>
        </div>
    );
}

ScoreQuest.defaultProps = {
    quest: []
};

ScoreQuest.propTypes = {
    // 問題標題
    title: PropType.string.isRequired,
    // 問題描述
    description: PropType.string,
    // 分數題問題
    quests: PropType.arrayOf(PropType.shape({
        // 分數題標題
        quest: PropType.string,
        // 分數題描述
        description: PropType.string,
        // 必填
        required: PropType.bool,
        // 問題代號
        name: PropType.string
    }))
}

export default ScoreQuest;