import React, { useEffect, useState } from 'react';
import PropType from 'prop-types';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { GlobalConstants } from '../../Utilities/CommonUtility';
import TableRow from './TableRow';
import TableData from './TableData';
import TableLabel from './TabelLabel';

const Panel = (props) => {
    // State
    // Panel 資料
    const [panelTable, setPanelTable] = useState();

    var { PostWithAuth } = usePostAuth();
    
    useEffect(() => {
        if (props.value != "" && props.value != null) {
            let ignore = false;
            // 若有Panel ID 取得對應資料
            PostWithAuth({
                url: "/Report/GetPanelData",
                data: {
                    PanelID: props.value,
                    FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                    AuthCode: 1
                },
                success: (response) => {
                    if (ignore) return;

                    let igCollection = JSON.parse(response.IgCollection);
                    setPanelTable(GeneratePanel(igCollection));
                }
            })

            return () => { ignore = true; };
        }
    }, [props.value])

    const GeneratePanel = (igCollection) => {
        let table = [];

        let header = (
            <TableRow key={-1}>
                {igCollection.Dye.map((element, index) => {
                    return (<TableData key={index} colwidth={12}><TableLabel text={element} style={{fontSize: "14px"}}/></TableData>)
                })}
            </TableRow>
        );

        let body = igCollection.Tube.map((element, pi) => {
            return (
                <TableRow key={pi}>
                    {
                        element.Ig.map((ig, index) => {
                            return (<TableData key={index} colwidth={12} style={{ fontSize: "14px" }}>{ig}</TableData>)
                        })
                    }
                </TableRow>
            );
        });

        table.push(header);
        table.push(body);
        return table;
    }

    return (
        <div>
            {panelTable}
        </div>
    );
}

Panel.proptypes = {
    name: PropType.string,
    value: PropType.string
}

export default Panel;