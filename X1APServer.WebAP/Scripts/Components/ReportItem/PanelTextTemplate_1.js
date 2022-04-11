import React, { useEffect, useState } from 'react';
import PropType from 'prop-types';
import TextTemplate from './TextTemplate';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { GlobalConstants } from '../../Utilities/CommonUtility';

const PanelTextTemplate = (props) => {
    const [source, setSource] = useState([]);
    const [name, setName] = useState("");
    const { PostWithAuth } = usePostAuth();
    
    useEffect(() => {
        if (props.value != null && props.value != "") {
            let ignore = false;

            PostWithAuth({
                url: "/Report/GetPanelTextTemplate",
                data: {
                    PanelID: props.value,
                    QuestionID: props.questionID,
                    FuncCode: GlobalConstants.FuncCode.ViewWebsite,
                    AuthCode: 1
                },
                success: (response) => {
                    if (ignore) return;

                    if (response.TextTemplateList.length > 0) {
                        let filterList = response.TextTemplateList.map(element => {
                            return element.Value;
                        });

                        setSource(filterList);
                        setName(GlobalConstants.QuestionPreFix + response.TextTemplateList[0].QuestionID);
                    }
                }
            });

            return () => { ignore = true; };

        }
    }, [props.value]);

    let value;
    if (props.form != null && name != "") {
        value = props.form[name];
    } 

    return (<TextTemplate value={value} name={name} handleOnChange={props.handleOnChange} source={source} />);
}

PanelTextTemplate.propTypes = {
    name: PropType.string,
    value: PropType.string,
    hasChild: PropType.bool,
    handleOnChange: PropType.func,
    form: PropType.object,
    questionID: PropType.string
};

export default PanelTextTemplate;