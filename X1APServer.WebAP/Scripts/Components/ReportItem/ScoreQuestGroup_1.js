import React, { useState } from 'react';
import PropTypes from 'prop-types';
import update from 'immutability-helper';

const ScoreQuestGroup = (props) => {
    // 儲存所有值
    const [value, setValue] = useState({});

    const { children } = props;

    const handleOnChange = (e, originHandleChange) => {
        let target = e.target;
        setValue(prev => {
            let newValue = update(prev, { [target.name]: { $set: target.value }});

            let sum = 0,
                keys = Object.keys(newValue);
            for (let i = 0; i < keys.length; i++) {
                let valueF = parseFloat(newValue[keys[i]]);
                // 忽略無法轉換的值
                if (valueF != null && !isNaN(valueF)) sum += valueF;
            }

            props.handleOnChange({ target: { name: props.scoreQuestName, value: sum } });

            return newValue;
        });
        originHandleChange(e);
    }

    // 將每個子元件 handleOnChange 改成會加總的 handleOnChange
    const RecursiveSum = (children) => {
        return React.Children.map(children, child => {
            let childschildren, props = {};

            if (React.isValidElement(child)) {
                if (child.props.hasOwnProperty('children')) {
                    childschildren = RecursiveSum(child.props.children);
                }
                const childHandleChange = child.props.handleOnChange;

                if (childHandleChange != null) {
                    props.handleOnChange = e => handleOnChange(e, childHandleChange);
                    props.type = "number";
                }

                if (childschildren != null) {
                    // childschildren為null的話，無法 render
                    return React.cloneElement(child, props, childschildren);
                } else {
                    return React.cloneElement(child, props);
                }
            }

            return child;
        });
    }

    let tool;
    if (props.toolFlag) {
        tool = (
            <>
                <input type="checkbox" className="dock" />
                <div className="form-ico-collapse"></div>
                <input type="checkbox" className="collapse-form" />
                <div className="form-ico-dock"></div>
                <div className="peek">Peek</div>
            </>
        )
    }

    // 計算總分數
    // let sum = 0,
    //     keys = Object.keys(value);
    // for (let i = 0; i < keys.length; i++) {
    //     let valueF = parseFloat(value[keys[i]]);
    //     // 忽略無法轉換的值
    //     if (valueF != null && !isNaN(valueF)) sum += valueF;
    // }

    return (
        <div className={`QForm Layer2 ${props.className}`}>
            <div className="QIndex" />
            <h4>{props.title}</h4>
            {tool}
            <p className="description">{props.description}</p>
            <div className="formContent">
                {RecursiveSum(children)}
                <div style={{ fontSize: "24px" }}><b>總分:</b> {props.scoreQuestValue}</div>
            </div>
        </div>
    );
};

ScoreQuestGroup.defaultProps = {
    className: "",
    title: "",
    toolFlag: false
}

ScoreQuestGroup.propTypes = {
    title: PropTypes.string,
    className: PropTypes.string,
    toolFlag: PropTypes.bool
};

export default ScoreQuestGroup;