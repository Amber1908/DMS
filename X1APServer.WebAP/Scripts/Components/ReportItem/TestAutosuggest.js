import React from "react";
import { useState } from "react";
import ReactDOM from "react-dom";
import PropType from 'prop-types';
import AutoSuggest from "react-autosuggest";

const companies = [
    "Company1",
    "Company2",
    "Big Corp",
    "Happy Toy Company"
];

const lowerCasedCompanies = companies.map(language => language.toLowerCase());

const TestAutosuggest = (props) => {

    const [value, setValue] = useState("");
    const [suggestions, setSuggestions] = useState([]);

    const getSuggestions = value => {
        return lowerCasedCompanies.filter(language =>
            language.startsWith(value.trim().toLowerCase())
        );
    }
    return (
        <div>
            <AutoSuggest
                suggestions={suggestions}
                onSuggestionsClearRequested={() => setSuggestions([])}
                onSuggestionsFetchRequested={({ value }) => {
                    setValue(value);
                    setSuggestions(getSuggestions(value));
                }}
                onSuggestionSelected={(_, { suggestionValue }) =>
                    console.log("Selected: " + suggestionValue)
                }
                getSuggestionValue={suggestion => suggestion}
                renderSuggestion={suggestion => <span>{suggestion}</span>}
                inputProps={{
                    placeholder: "Type 'c'",
                    value: value,
                    onChange: (_, { newValue, method }) => {
                        setValue(newValue);
                    }
                }}
                highlightFirstSuggestion={true}
            />
        </div>
    );
};

TestAutosuggest.propTypes = {
    handleOnChange: PropType.func,
    label: PropType.string,
    name: PropType.string,
    value: PropType.string
};

export default TestAutosuggest;
