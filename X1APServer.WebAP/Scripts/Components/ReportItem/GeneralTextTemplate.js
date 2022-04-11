import React, { useState, useEffect } from 'react';
import PropType from 'prop-types';
import TextTemplate from './TextTemplate';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { GlobalConstants } from '../../Utilities/CommonUtility';

const GeneralTextTemplate = (props) => {
    const [source, setSource] = useState([]);

    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        if (props.name != null) {
            const questionID = parseInt(props.name.replace(GlobalConstants.QuestionPreFix, ""));
            let ignore = false;

            PostWithAuth({
                url: "/Report/GetTextTemplate",
                data: {
                    "QuestionID": questionID,
                    "Group": 1,
                    "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                    "AuthCode": 1
                },
                success: (response) => {
                    if (ignore) return;

                    const filterList = response.TextTemplateList.map(element => {
                        return element.Value;
                    });

                    setSource(filterList);
                }
            });

            return () => { ignore = true; };
        }
    }, [props.name]);

    return (
        <TextTemplate source={source} {...props} />
    )
}

GeneralTextTemplate.propTypes = {
    // 問題 ID
    name: PropType.string.isRequired
}

export default GeneralTextTemplate;