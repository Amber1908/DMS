import React, { useEffect } from 'react';
import { useState } from 'react';
import { usePostAuth } from '../../../CustomHook/CustomHook';
import { GlobalConstants } from '../../../Utilities/CommonUtility';
import CheckBox from '../../ReportItem/CheckBox';
import TextInput from '../../ReportItem/TextInput';
import NumberValidation from '../QuestValidation/NumberValidation';

const State = {
    // 新增關注
    addPinnedQuest: 0,
    // 選擇關注
    selectPinnedQuest: 1
}

const QuestAttr = (props) => {
    // 關注項目清單
    const [pinnedQuestList, setPinnedQuestList] = useState([]);
    // 目前關注選擇狀態，選擇關注或新增關注
    const [currentState, setCurrentState] = useState(State.selectPinnedQuest);
    // indexGroup: 索引群組，問題群組索引、問題索引, reportStructure: 表單結構
    let { indexGroup, reportStructure } = props;
    // groupIndex: 問題群組索引, questIndex: 問題索引
    let { groupIndex, questIndex } = indexGroup;
    // 取出目前的問題
    let quest = reportStructure.Children[groupIndex].Children[questIndex];

    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        // 取關注項目清單
        getPinnedQuestList();
    }, [])

    // 取消關注時，清空選擇關注及新增關注
    useEffect(() => {
        if (!quest.Pin) {
            setValue("PinnedID", "");
            setValue("PinnedName", "");
        }
    }, [quest.Pin])

    //取關注項目清單
    const getPinnedQuestList = () => {
        PostWithAuth({
            url: "/Report/GetPinnedQuestList",
            data: {
                FuncCode: GlobalConstants.FuncCode.Backend,
                AuthCode: 1
            },
            success: (rsp) => {
                setPinnedQuestList(rsp.PinnedQuestList);

                if (rsp.PinnedQuestList.length === 0) {
                    // 沒有任何關注切成新增狀態
                    setCurrentState(State.addPinnedQuest);
                } else if (quest.PinnedID == null) {
                    // 沒有選擇關注預設第一個選項
                    setValue("PinnedID", rsp.PinnedQuestList[0].PinnedID);
                }
            }
        })
    }

    const handleCheckboxChange = (e, indexArg) => {
        // 轉換value型態為boolean
        let changeEvent = {
            target: {
                name: e.target.name,
                value: e.target.value == "true" || e.target.value == true ? true : false
            }
        }

        props.handleOnChange(changeEvent, indexArg)
    }

    const createValidation = (questType) => {
        // 依問題類型產生標準值元件
        switch (questType) {
            case "number":
            case "scoresum":
                return <NumberValidation {...props} />
        }
    }

    const changeState = (state) => {
        // 切換選擇關注狀態，清空選擇關注及新增關注
        setCurrentState(state);
        setValue("PinnedID", "");
        setValue("PinnedName", "");
    }

    const setValue = (name, value) => {
        let fakeEvent = {
            target: {
                name: name,
                value: value
            }
        }
        props.handleOnChange(fakeEvent, props.indexGroup)
    }

    const createPinnedComponent = () => {
        return (
            <>
                <div hidden={currentState !== State.selectPinnedQuest} className="allow-interact">
                    <div>選擇關注項目</div>
                    <select name="PinnedID" value={quest.PinnedID} onChange={e => props.handleOnChange(e, props.indexGroup)}>
                        {pinnedQuestList.map(pq => (<option value={pq.PinnedID}>{pq.PinnedName}</option>))}
                    </select>
                    <span class="ui-listItem-Add add-pinned-quest" onClick={e => changeState(State.addPinnedQuest)}></span>
                </div>
                <div hidden={currentState !== State.addPinnedQuest} className="allow-interact">
                    <div>增加關注項目</div>
                    <input name="PinnedName" value={quest.PinnedName} onChange={e => props.handleOnChange(e, props.indexGroup)} />
                    <span class="ui-listItem-Add return"
                        hidden={pinnedQuestList.length === 0}
                        onClick={e => changeState(State.selectPinnedQuest)}></span>
                </div>
            </>
        );
    }

    const createQuestTypeElement = (questType) => {
        switch(questType) {
            case "select":
            case "radio":
            case "checkbox":
                return <CheckBox className="required" text="其他" trueValue="true" name="OtherAns" value={quest.OtherAns} handleOnChange={e => handleCheckboxChange(e, props.indexGroup)} />
        }
    }

    let validationElement = null;
    let pinnedComponent = null;
    if (quest.Pin) {
        validationElement = createValidation(quest.QuestionType);
        pinnedComponent = createPinnedComponent();
    }

    let questTypeElement = createQuestTypeElement(quest.QuestionType);

    return (
        <>
            <CheckBox disabled={quest.QuestionType === "scoresum"} className="required" text="必填" trueValue="true" name="Required" value={quest.Required} handleOnChange={e => handleCheckboxChange(e, props.indexGroup)} />
            <CheckBox className="required allow-interact" text="關注" trueValue="true" name="Pin" value={quest.Pin} handleOnChange={e => handleCheckboxChange(e, props.indexGroup)} />
            <TextInput className="text-editor-container" label="問題編號(用於對應匯入及匯出資料問題)" name="QuestionNo" inputProps={{ maxLength: "50" }} handleOnChange={e => props.handleOnChange(e, props.indexGroup)} value={quest.QuestionNo} />
            <TextInput type="number" className="text-editor-container" label="匯出資料排序編號(-1: 不顯示)" name="CodingBookIndex" handleOnChange={e => props.handleOnChange(e, props.indexGroup)} value={quest.CodingBookIndex} required={true} />
            {pinnedComponent}
            {validationElement}
            {questTypeElement}
        </>
    );
}

export default QuestAttr;