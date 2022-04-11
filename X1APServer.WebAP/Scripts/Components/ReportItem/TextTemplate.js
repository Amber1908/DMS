import React, { useState, useEffect, useRef } from 'react';
import PropType from 'prop-types';
import Select from './Select';
import TextArea from './TextArea';
import CustomAutosuggest from './CustomAutosuggest';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { GlobalConstants } from '../../Utilities/CommonUtility';


const TextTemplate = (props) => {
    let colwidthClass;
    // State
    const [text, setText] = useState("");
    const [autosuggestValue, setAutosuggestValue] = useState("");
    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        setText(props.value);
    }, [props.value]);

    // 切換罐頭語
    const handleOnChange = (e, { suggestion }) => {
        e.persist();
        setText(prev => {
            prev = prev == null ? "" : prev;
            const textAreaValue = prev + suggestion;
            let argE = { target: { value: textAreaValue, name: props.name } };
            props.handleOnChange(argE);
            return textAreaValue;
        });

        setAutosuggestValue("");
    };

    const handleAutosuggestChange = (e) => {
        setAutosuggestValue(e.target.value);
    };

    const getSuggestions = (value, setAutoSuggestValue) => {
        if (Array.isArray(props.source)) {
            setAutoSuggestValue(prev => {
                let suggestions = props.source.filter(element => {
                    return element.toLowerCase().indexOf(value.toLowerCase()) > -1;
                });

                return {
                    ...prev,
                    suggestions: suggestions
                };
            })
        }

        // setAutoSuggestValue(prev => {
        //     return {
        //         ...prev,
        //         suggestions: ["A", "B", "C"]
        //     };
        // });
    }

    if (props.colwidth != null) {
        colwidthClass = `ui-Col-${props.colwidth}`;
    }

    return (
        <div className={`${props.className} ${colwidthClass}`} style={props.style}>
            {/* <Select colwidth={100}
                options={props.options} handleOnChange={handleOnChange} value={selectValue}/> */}
            <CustomAutosuggest handleOnChange={handleAutosuggestChange} value={autosuggestValue} handleSuggestionSelected={handleOnChange} getSuggestions={getSuggestions} style={{marginBottom: "10px"}} />
            <TextArea value={text} name={props.name} rows={props.rows} handleOnChange={props.handleOnChange} />
        </div>
    );
};

TextTemplate.defaultProps = {
    colwidth: 100
};

TextTemplate.propTypes = {
    colwidth: PropType.number.isRequired,
    name: PropType.string,
    handleOnChange: PropType.func,
    value: PropType.string,
    text: PropType.string,
    rows: PropType.number,
    source: PropType.array,
    className: PropType.string,
    style: PropType.object
};

export default TextTemplate;