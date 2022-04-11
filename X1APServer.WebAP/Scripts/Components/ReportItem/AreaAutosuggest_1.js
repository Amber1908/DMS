import React, { useState, useEffect } from 'react';
import Autosuggest from 'react-autosuggest';
import PropType from 'prop-types';

import { GlobalConstants } from '../../Utilities/CommonUtility';
import { usePostAuth, useLazyLoading } from '../../CustomHook/CustomHook';

// When suggestion is clicked, Autosuggest needs to populate the input
// based on the clicked suggestion. Teach Autosuggest how to calculate the
// input value for every given suggestion.
const getSuggestionValue = suggestion => suggestion.FullCode;

const AreaAutosuggest = (props) => {
    let colwidthClass;
    const [autoSuggestValue, setAutoSuggestValue] = useState({
        value: "",
        suggestions: []
    });

    // Custom Hooks
    // Post With Auth Function
    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        PostWithAuth({
            url: "/User/GetAreaCode",
            data: {
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (response) => {
                if (response.Data.length == 0) return;

                setAutoSuggestValue(prev => {
                    return {
                        ...prev,
                        suggestions: prev.suggestions.concat(response.Data)
                    };
                });
            }
        });
    }, [props.value]);

    // Teach Autosuggest how to calculate suggestions for any given input value.
    const getSuggestions = value => {
        const inputValue = value;
        const inputLength = inputValue.length;

        return inputLength === 0 ? [] : suggestions.filter(lang =>
            lang.AreaCode.slice(0, inputLength) === inputValue
        );
    };

    const onChange = (event, { newValue }) => {
        setAutoSuggestValue(prev => {
            return {
                ...prev,
                value: newValue
            };
        });

        props.handleOnChange(event);
    };

    // Autosuggest will call this function every time you need to update suggestions.
    // You already implemented this logic above, so just use it.
    const onSuggestionsFetchRequested = ({ value }) => {
        // 清空列表
        onSuggestionsClearRequested();
        getSuggestions();
    };

    // Autosuggest will call this function every time you need to clear suggestions.
    const onSuggestionsClearRequested = () => {
        setAutoSuggestValue(prev => {
            return {
                ...prev,
                suggestions: []
            };
        });
    };

    const inputProps = {
        value: autoSuggestValue.value,
        onChange: onChange,
        name: props.name
    };

    // Use your imagination to render suggestions.
    const renderSuggestion = suggestion => (
        <div>
            {suggestion.AreaCode}<br />
            {suggestion.AreaName}
        </div>
    );

    if (props.colwidth != null) {
        colwidthClass = `ui-Col-${props.colwidth}`;
    }

    return (
        <div className={`${props.className} ${colwidthClass}`} style={props.style}>
            <label>{props.label}</label>
            <Autosuggest
                suggestions={autoSuggestValue.suggestions}
                onSuggestionsFetchRequested={onSuggestionsFetchRequested}
                onSuggestionsClearRequested={onSuggestionsClearRequested}
                getSuggestionValue={getSuggestionValue}
                renderSuggestion={renderSuggestion}
                inputProps={inputProps}
            />
        </div>

    );
};

AreaAutosuggest.defaultProps = {
    colwidth: 100
};

AreaAutosuggest.propTypes = {
    handleOnChange: PropType.func,
    label: PropType.string,
    name: PropType.string,
    value: PropType.string
};

export default AreaAutosuggest;