import React, { useEffect, useState } from 'react';
import TextInput from '../../ReportItem/TextInput';

const NumberValidation = (props) => {
    let { indexGroup, reportStructure } = props;
    let { groupIndex, questIndex } = indexGroup;
    let validationGroupList = reportStructure.Children[groupIndex].Children[questIndex].ValidationGroupList || [];

    return (
        <div>
            <div>標準值</div>
            {
                validationGroupList.map((g, i) => {
                    let indexGroupArg = { ...indexGroup };
                    indexGroupArg.validationGroupIndex = i;
                    console.log(indexGroupArg);
                    return (
                        <div key={i}>
                            <button className="close fa fa-times" onClick={e => props.handleDeleteValidationGroup(e, indexGroupArg)}></button>
                            <ValidationCondition {...props} {...g} indexGroup={indexGroupArg} />
                            <Block {...props} indexGroup={indexGroupArg} index={i} ValidationList={g.ValidationList} />
                        </div>
                    );
                })
            }
            <button onClick={e => props.handleAddValidationGroup(e, props.indexGroup)}>新增</button>
        </div>
    )
}

const ValidationCondition = (props) => {
    const condition = {
        Gender: [
            { text: "男性", value: "M", operator: "==" },
            { text: "女性", value: "F", operator: "==" }
        ],
        Age: [
            { text: "0 ~ 12 歲", value: "0", operator: ">=", value2: "12", operator2: "<" },
            { text: "12 ~ 20 歲", value: "12", operator: ">=", value2: "20", operator2: "<" },
            { text: "20 ~ 40 歲", value: "20", operator: ">=", value2: "40", operator2: "<" },
            { text: "40 ~ 65 歲", value: "40", operator: ">=", value2: "65", operator2: "<" },
            { text: "65 歲以上", value: "65", operator: ">=" },
        ]
    }

    const changeAttributeValue = (op1, value1, op2, value2) => {
        props.handleOnChange({target: { name: "Operator1", value: op1 }}, props.indexGroup);
        props.handleOnChange({target: { name: "Value1", value: value1 }}, props.indexGroup);
        props.handleOnChange({target: { name: "Operator2", value: op2 }}, props.indexGroup);
        props.handleOnChange({target: { name: "Value2", value: value2 }}, props.indexGroup);        
    }

    const handleValueChange = (e) => {
        let selectedIndex = e.target.selectedIndex;
        let option = e.target.options[selectedIndex];
        let op1 = option.dataset.op1,
            op2 = option.dataset.op2,
            value2 = option.dataset.value2;

        changeAttributeValue(op1, e.target.value, op2, value2);
    }

    const handleAttributeNameChange = (e) => {
        changeAttributeValue("", "", "", "");
        props.handleOnChange(e, props.indexGroup);
    }

    let valueElement = null;
    if (props.AttributeName) {
        valueElement = (
            <>
                是
                <select value={props.Value1} onChange={handleValueChange}>
                    <option disabled value="">--請選擇--</option>
                    {condition[props.AttributeName].map(c => (
                        <option key={c.value} value={c.value} data-value2={c.value2} data-op1={c.operator} data-op2={c.operator2}>{c.text}</option>
                    ))}
                </select>
            </>
        );
    }

    return (
        <div>
            <select name="AttributeName" value={props.AttributeName} style={{width: "55px"}} onChange={handleAttributeNameChange}>
                <option value="">--無前置條件--</option>
                <option value="Gender">性別</option>
                <option value="Age">年齡</option>
            </select>
            {valueElement}
        </div>
    )
}

const Block = (props) => {
    const [blockNum, setBlockNum] = useState("");

    let { indexGroup } = props;
    let validationList = props.ValidationList;

    useEffect(() => {
        if (validationList.length > 0) {
            setBlockNum((validationList.length).toString());
        }
    }, [validationList.length])

    const handleValidationChange = (valueEvent, updateArgs, operator) => {
        let operatorEvent = {
            target: {
                name: "Operator",
                value: operator
            }
        }
        let fakeValueEvent = {
            target: {
                name: "Value",
                value: valueEvent.target.value
            }
        }
        props.handleOnChange(fakeValueEvent, updateArgs);
        props.handleOnChange(operatorEvent, updateArgs);

        if (updateArgs.validationIndex === validationList.length - 2) {
            updateArgs.validationIndex = validationList.length - 1;
            handleValidationChange(valueEvent, updateArgs, ">");
        }
    }

    const handleBlockNumChange = (e) => {
        setBlockNum(e.target.value);
        const blockNumInt = parseInt(e.target.value);
        props.handleChangeValidationAmount(e, indexGroup, blockNumInt);
    }

    const handleSelectColor = (e, index) => {
        e.target.name = "Color";
        props.handleOnChange(e, { ...indexGroup , validationIndex: index });

        let fakeEvent = {
            target: {
                name: "Normal",
                value: false
            }
        };
        if (e.target.value === "#68b5ee") {
            fakeEvent.target.value = true;
            props.handleOnChange(fakeEvent, { ...indexGroup , validationIndex: index });
        } else {
            props.handleOnChange(fakeEvent, { ...indexGroup , validationIndex: index });
        }
    }

    const createValidationElement = () => {
        if (blockNum === "") return;

        const blockNumInt = parseInt(blockNum);
        let validationElements = [];
        for (let i = 0; i < blockNumInt - 1; i++) {
            validationElements.push(createColorPicker(i, validationList[i].Color));
            validationElements.push(createThresholdElement(i));
        }
        validationElements.push(createColorPicker(blockNumInt - 1, validationList[blockNumInt - 1].Color));
        return validationElements;
    }

    const createColorPicker = (index, value) => {
        return (
            <div style={{ display: "flex", margin: "10px" }}>
                <select value={value} style={{width: "calc(100% - 20px)"}} onChange={e => handleSelectColor(e, index)}>
                    <option disabled value="">--請選擇--</option>
                    <option value="red" style={{color: "red"}}>紅</option>
                    <option value="orange" style={{color: "orange"}}>橘</option>
                    <option value="#68b5ee" style={{color: "#68b5ee"}}>淺藍(正常值)</option>
                    <option value="blue" style={{color: "blue"}}>藍</option>
                    <option value="purple" style={{color: "purple"}}>紫</option>
                </select>
                <div style={{ width: "20px", height: "20px", background: value}}></div>
            </div>
        );
    };

    const createThresholdElement = (index) => {
        return (
            <div>
                &le;
                <input type="number" value={validationList[index].Value} onChange={e => handleValidationChange(e, { ...indexGroup , validationIndex: index }, "<=")} />
            </div>
        )
    }

    return (
        <>
            <div>
                <label>區段數量: </label>
                <select value={blockNum} onChange={handleBlockNumChange}>
                    <option disabled value="">--請選擇--</option>
                    <option value="2">2</option>
                    <option value="3">3</option>
                    <option value="4">4</option>
                    <option value="5">5</option>
                </select>
            </div>
            <div style={{display: "flex", flexDirection: "column"}}>
                {createValidationElement()}
            </div>        
        </>        
    );
}

export default NumberValidation;