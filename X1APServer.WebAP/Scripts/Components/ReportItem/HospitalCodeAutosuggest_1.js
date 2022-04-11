import React, { useState, useEffect } from 'react';
import Autosuggest from 'react-autosuggest';
import PropType from 'prop-types';

import { GlobalConstants } from '../../Utilities/CommonUtility';
import { usePostAuth, useLazyLoading } from '../../CustomHook/CustomHook';

// When suggestion is clicked, Autosuggest needs to populate the input
// based on the clicked suggestion. Teach Autosuggest how to calculate the
// input value for every given suggestion.
const getSuggestionValue = suggestion => suggestion.HospitalCode;

const HospitalCodeAutosuggest = (props) => {
    const [autoSuggestValue, setAutoSuggestValue] = useState({
        value: "",
        suggestions: []
    });

    // Post With Auth Function
    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        if (props.value != null) {
            setAutoSuggestValue(prev => {
                return {
                    ...prev,
                    value: props.value
                }
            });
        }
    }, [props.value])

    // Teach Autosuggest how to calculate suggestions for any given input value.
    const getSuggestions = (value) => {
        PostWithAuth({
            url: "/User/GetHospitalCodeLazy",
            data: {
                code: value,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (response) => {
                if (response.Data == null || response.Data.length == 0) return;

                setAutoSuggestValue(prev => {
                    return {
                        ...prev,
                        suggestions: prev.suggestions.concat(response.Data)
                    };
                });
            }
        });
    };

    const onChange = (event, { newValue }) => {
        setAutoSuggestValue(prev => {
            return {
                ...prev,
                value: newValue
            };
        });

        let fakeEvent = Object.assign({}, event);
        fakeEvent.target.name = props.name;
        fakeEvent.target.value = newValue;
        props.handleOnChange(fakeEvent);
    };

    // Autosuggest will call this function every time you need to update suggestions.
    // You already implemented this logic above, so just use it.
    const onSuggestionsFetchRequested = ({ value }) => {
        onSuggestionsClearRequested();
        getSuggestions(value);
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
            {suggestion.HospitalCode}<br />
            {suggestion.HospitalName}
        </div>
    );

    return (
        <div style={{ width: "100%" }}>
            <div className={"input-item edit"}>
                <Autosuggest
                    suggestions={autoSuggestValue.suggestions}
                    onSuggestionsFetchRequested={onSuggestionsFetchRequested}
                    onSuggestionsClearRequested={onSuggestionsClearRequested}
                    getSuggestionValue={getSuggestionValue}
                    renderSuggestion={renderSuggestion}
                    inputProps={inputProps}
                />
            </div>
        </div>
    );
}

HospitalCodeAutosuggest.defaultProps = {
    style: {},
    handleOnChange: () => { }
}

HospitalCodeAutosuggest.propTypes = {
    handleOnChange: PropType.func,
    name: PropType.string,
    value: PropType.string,
    handleSuggestionSelected: PropType.func,
    style: PropType.object
};

export default HospitalCodeAutosuggest;