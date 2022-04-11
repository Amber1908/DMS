import React, { useRef, useState } from 'react';

const FlowTable = (props) => {
    const [tableData, setTableData] = useState([]);

    const editableTable = useRef(null);

    const generateBarChart = () => {
        let table = editableTable.current.getElementsByTagName("table")[0];

        if (table != null) {
            let tablechildren = table.childNodes[1].children;
            let liveTableData = [];
            let sum = 0;

            for (let i = 1; i < tablechildren.length; i++) {
                sum += parseInt(tablechildren[i].children[1].textContent);
            }

            for (let i = 1; i < tablechildren.length; i++) {
                liveTableData.push({
                    text: tablechildren[i].children[0].textContent,
                    value: Math.round((parseFloat(tablechildren[i].children[1].textContent) / sum) * 10000) / 100,
                    background: tablechildren[i].children[0].style.background
                });
            }

            setTableData(liveTableData);
        }

    }

    const clear = () => {
        editableTable.current.innerHTML = null;
        setTableData([]);
    }

    let liveTable = [];

    tableData.forEach((element, index) => {
        liveTable.push(
            <div key={index} className="live-table-Row">
                <span>{element.text}</span>
                <span>{element.value}</span>
                <div>
                    <div style={{ width: element.value + "%", background: element.background }}></div>
                </div>
            </div>
        )
    });

    return (
        <div className="formContent">
            <button type="button" onClick={generateBarChart}>Create Bar Chart</button>
            <button type="button" onClick={clear}>Clear</button>
            <div>
                <div ref={editableTable} className="live-table-CE" contentEditable="true"></div>
                <div style={{ width: '60%' }} className="live-table-Wrap">
                    {liveTable}
                </div>
            </div>
        </div>
    )
}

export default FlowTable;