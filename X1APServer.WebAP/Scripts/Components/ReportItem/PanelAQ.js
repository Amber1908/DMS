import React, { useEffect, useState } from 'react';
import PropType from 'prop-types';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { GlobalConstants } from '../../Utilities/CommonUtility';
import Select from './Select';

const PanelAQ = (props) => {
    const [hidden, setHidden] = useState(true);
    const [igList, setIgList] = useState([]);

    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        if (props.value != "" && props.value != null) {
            let ignore = false;

            // 若有 Panel ID 抓取對應的抗體及細胞類型
            PostWithAuth({
                url: "/Report/GetPanelMappingIg",
                data: {
                    "PanelID": props.value,
                    "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                    "AuthCode": 1
                },
                success: (response) => {
                    if (ignore) return;

                    GeneratePanelAQ(response.PanelIgList);
                }
            });

            return () => { ignore = true; };
        }
    }, [props.value]);

    // 產生Panel 對應題目
    const GeneratePanelAQ = (panelIgList) => {
        if (panelIgList.length == 0) {
            return;
        }

        let requestData = panelIgList.map(element => {
            return element.CellType + "_" + element.IgName;
        });

        PostWithAuth({
            url: "/Report/GetQuestionList",
            data: {
                "QuestTextList": requestData,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (response) => {
                // 測試用選項
                // const options = [
                //     { value: "0", text: "neg" },
                //     { value: "1", text: "dim" },
                //     { value: "2", text: "pos" },
                //     { value: "3", text: "bri" },
                //     { value: "4", text: "neg-dim" },
                //     { value: "5", text: "neg-pos" },
                //     { value: "6", text: "neg-bri" },
                //     { value: "7", text: "dim-pos" },
                //     { value: "8", text: "dim-bri" },
                //     { value: "9", text: "pos-bri" }
                // ];

                let tempList = [];

                const questionList = response.QuestionList;

                for (let i = 0; i < panelIgList.length; i++) {
                    if (questionList[i] != null) {
                        let options = [], object = JSON.parse(questionList[i].AnswerOption);
                        for (let k in object) {
                            if (object.hasOwnProperty(k)) {
                                options.push({ value: k, text: object[k] });
                            }
                        }

                        tempList.push({
                            cellType: panelIgList[i].CellType,
                            name: GlobalConstants.QuestionPreFix + questionList[i].ID,
                            label: panelIgList[i].IgName,
                            options: options,
                            IgName: panelIgList[i].IgName
                        });
                    }
                }

                setIgList(tempList);

                if (tempList.length > 0) {
                    setHidden(false);
                }
            }
        })
    }

    let bCellSelects = [],
        tCellSelects = [],
        nkCellSelects = [],
        pcdCellSelects = [],
        alCellSelects = [];

    igList.forEach((element, i) => {
        const tag = (<Select handleOnChange={props.handleOnChange} value={props.form[element.name]} key={i} name={element.name} colwidth={30} label={element.IgName} options={element.options} />);

        switch (element.cellType) {
            case "B":
                bCellSelects.push(tag);
                break;
            case "T":
                tCellSelects.push(tag);
                break;
            case "NK":
                nkCellSelects.push(tag);
                break;
            case "PCD":
                pcdCellSelects.push(tag);
                break;
            case "AL":
                alCellSelects.push(tag);
                break;
        }
    });

    // 取得對應的細胞是否顯示
    const getCellVisibility = (cellIndex) => {
        return props.checkedCells[cellIndex] == "1" ? false : true;
    }

    return (
        <>
            <div style={{float: "left"}} hidden={hidden}>
                <div className="zone right ui-col-33 ui-Eq-2" hidden={getCellVisibility(1)}>
                    <h6>T-Cell Markers</h6>
                    {bCellSelects}
                </div>

                <div className="zone right ui-col-33 ui-Eq-2" hidden={getCellVisibility(0)}>
                    <h6>B-Cell Markers</h6>
                    {tCellSelects}
                </div>

                <div className="zone ui-col-33 ui-Eq-2" hidden={getCellVisibility(2)}>
                    <h6>NK/Aberrant Markers</h6>
                    {nkCellSelects}
                </div>
            </div>

            <div style={{ float: "left" }} hidden={hidden}>
                <div className="zone right ui-col-33 ui-Eq-2" hidden={getCellVisibility(3)}>
                    <h6>PCD</h6>
                    {nkCellSelects}
                </div>

                <div className="zone ui-col-33 ui-Eq-2" hidden={getCellVisibility(4)}>
                    <h6>AL</h6>
                    {nkCellSelects}
                </div>
            </div>
        </>
    )
}

PanelAQ.defaultprops = {
    checkedCells: []
};

PanelAQ.propTypes = {
    // 問題編號
    name: PropType.string,
    // 問題值
    value: PropType.string,
    // 是否有子問題
    hasChild: PropType.bool,
    // Report 答案
    form: PropType.object,
    // input change function
    handleOnChange: PropType.func,
    // 勾選的細胞
    checkedCells: PropType.array
};

export default PanelAQ;