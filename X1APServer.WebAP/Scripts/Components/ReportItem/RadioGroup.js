import React, { useEffect, useState } from 'react';
import RadioButton from './RadioButton';
import PropType from 'prop-types';
import TextInput from './TextInput';

const RadioGroup = (props) => {
    const [options, setOptions] = useState(props.options);
    const [inlineInput, setInlineInput] = useState({});



    useEffect(() => {
        if (props.form != null) {
            setOptions(prev => {
                return prev.map((item, index) => {
                    // item.value = ReplaceQuest(item.value);
                    item.showText = ReplaceQuest(item.text);

                    return item;
                })
            });
        }
    }, [props.form]);

    const ReplaceQuest = (text) => {

        const regex = /\${(Q_[^,}]+?)(,([^,}]))?}/g;
        let matches = [...text.matchAll(regex)];

        for (const match of matches) {
            const questionID = match[1];

            if (props.form == null || props.form[questionID] == null) {
                text = text.replace(match[0], "Null");
                continue;
            };
            text = text.replace(match[0], stringFormat(props.form[questionID], match[3]));
        }

        return text;
    };

    // 字串格式轉換
    const stringFormat = (str, formatType) => {
        let strFormat = str;

        switch (formatType) {
            // 百分比小數兩位
            case "P":
                let num = parseFloat(str);
                if (!isNaN(num)) {
                    strFormat = (Math.round(num * 10000.0) / 100).toString();
                }
                break;
        }

        return strFormat;
    }

    const ReplaceInput = (text) => {
        let response = [];
        const regex = /\${I_(Q_.*?)}/g;
        let matches = [...text.matchAll(regex)];
        let temp = 0;

        for (const match of matches) {
            const questionID = match[1];
            let value = "";
            if (props.form != null && props.form[questionID] != null) value = props.form[questionID];

            response.push(text.substring(temp, match.index));
            response.push(<TextInput key={questionID} style={{ width: "50px", height: "22px" }} inline={true} name={questionID} handleOnChange={props.handleOnChange} value={value} />);
            temp = match.index + match[0].length;
        }

        response.push(text.substring(temp, text.length));

        return response;
    }

    const radiobuttons = options.map((item, index) => {
        if (item.showText == null) item.showText = item.text;

        const id = `${props.name}_${index}`;

        const optionText = ReplaceInput(item.showText);

        return (
            <RadioButton handleOnChange={props.handleOnChange} checked={props.value == item.value} key={id} id={id} name={props.name} value={item.value} text={optionText} required={props.required} />
        );
    });

    let otherAnsElement = null;
    if (props.value === "other") {
        otherAnsElement = (
            <div className="item-wrap edit ui-Col-100">
                <div className="input-item">
                    <input type="text" name={props.otherAnsName} value={props.otherAnsValue} required={true} onChange={props.handleOnChange} />
                </div>
            </div>
        );
    }

    return (
        <>
            <div className={props.className}>
                <div>{props.label}</div>
                {radiobuttons}
            </div>
            {otherAnsElement}
        </>
    );
};

RadioGroup.defaultProps = {
    required: false
};

RadioGroup.propTypes = {
    name: PropType.string,
    handleOnChange: PropType.func,
    value: PropType.string,
    options: PropType.arrayOf(PropType.shape({
        text: PropType.string.isRequired,
        value: PropType.string.isRequired,
        additionalQuest: PropType.string
    })).isRequired,
    className: PropType.string,
    label: PropType.string,
    required: PropType.bool,
    hasChild: PropType.bool,
    form: PropType.object,
    otherAnsFlag: PropType.bool,
    otherAnsName: PropType.string,
    otherAnsValue: PropType.string
};

export default RadioGroup;