import React, { useState, useEffect } from 'react';
import Autosuggest from 'react-autosuggest';
import PropType from 'prop-types';

import { GlobalConstants } from '../../Utilities/CommonUtility';
import { usePostAuth } from '../../CustomHook/CustomHook';

// When suggestion is clicked, Autosuggest needs to populate the input
// based on the clicked suggestion. Teach Autosuggest how to calculate the
// input value for every given suggestion.
const getSuggestionValue = suggestion => suggestion;

const CustomAutosuggest = (props) => {
    const [autoSuggestValue, setAutoSuggestValue] = useState({
        value: "",
        suggestions: []
    });

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
    const getSuggestions = value => {
        props.getSuggestions(value, setAutoSuggestValue);
    };

    const onChange = (event, { newValue }) => {
        setAutoSuggestValue(prev => {
            return {
                ...prev,
                value: newValue
            };
        });

        let fakeEvent = Object.assign({}, event);
        fakeEvent.target.value = newValue;
        props.handleOnChange(fakeEvent);
    };

    // Autosuggest will call this function every time you need to update suggestions.
    // You already implemented this logic above, so just use it.
    const onSuggestionsFetchRequested = ({ value }) => {
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
            {suggestion}
        </div>
    );

    return (
        <div style={props.style}>
            <Autosuggest
                suggestions={autoSuggestValue.suggestions}
                onSuggestionsFetchRequested={onSuggestionsFetchRequested}
                onSuggestionsClearRequested={onSuggestionsClearRequested}
                getSuggestionValue={getSuggestionValue}
                renderSuggestion={renderSuggestion}
                inputProps={inputProps}
                onSuggestionSelected={props.handleSuggestionSelected}
            />
        </div>
    );
}

CustomAutosuggest.defaultProps = {
    style: {},
    handleOnChange: () => {}
}

CustomAutosuggest.propTypes = {
    handleOnChange: PropType.func,
    name: PropType.string,
    value: PropType.string,
    getSuggestions: PropType.func,
    handleSuggestionSelected: PropType.func,
    style: PropType.object
};

export default CustomAutosuggest;