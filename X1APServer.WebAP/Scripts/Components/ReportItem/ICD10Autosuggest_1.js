import React, { useState, useEffect } from 'react';
import Autosuggest from 'react-autosuggest';
import PropType from 'prop-types';

import { GlobalConstants } from '../../Utilities/CommonUtility';
import { usePostAuth, useLazyLoading } from '../../CustomHook/CustomHook';

// When suggestion is clicked, Autosuggest needs to populate the input
// based on the clicked suggestion. Teach Autosuggest how to calculate the
// input value for every given suggestion.
const getSuggestionValue = suggestion => suggestion.FullCode;

const ICD10Autosuggest = (props) => {
    const [autoSuggestValue, setAutoSuggestValue] = useState({
        value: "",
        suggestions: []
    });
    // Page
    const [currentPage, setCurrentPage] = useState({ Page: 0, ScrollFlag: true });

    // Custom Hooks
    // Post With Auth Function
    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        setAutoSuggestValue(prev => {
            return {
                ...prev,
                value: props.value
            };
        });
    }, [props.value]);

    // Teach Autosuggest how to calculate suggestions for any given input value.
    const getSuggestions = ({value, Page = 0}) => {
        PostWithAuth({
            url: "/ICD10/GetICD10Lazy",
            data: {
                Page: Page,
                ICD10Code: value,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (response) => {
                if (response.ICD10List.length == 0) return;

                setCurrentPage(prev => {
                    return {
                        ScrollFlag: true,
                        Page: prev.Page + 1
                    };
                });

                setAutoSuggestValue(prev => {
                    return {
                        ...prev,
                        suggestions: prev.suggestions.concat(response.ICD10List)
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

        props.handleOnChange(event);
    };

    // Autosuggest will call this function every time you need to update suggestions.
    // You already implemented this logic above, so just use it.
    const onSuggestionsFetchRequested = ({ value }) => {
        // 重設 Page
        setCurrentPage({
            ScrollFlag: false,
            Page: 0
        });

        // 清空列表
        onSuggestionsClearRequested();
        getSuggestions({value});
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
            {suggestion.FullCode}<br />
            {suggestion.AbbreviatedDescription}
        </div>
    );

    const renderSuggestionsContainer = ({ containerProps, children }) => {
        return (
            <div onScroll={handleScroll} {...containerProps}>
                {children}
            </div>
        )
    }

    const handleScroll = (e) => {
        let target = e.target;

        // 若當下沒在抓取資料及快滾動至底部，抓取接著30筆資料
        if (currentPage.ScrollFlag &&
            target.scrollHeight - target.scrollTop - target.clientHeight < 50) {
            setCurrentPage(prev => {
                return {
                    ...prev,
                    ScrollFlag: false
                };
            });

            getSuggestions({ Page: currentPage.Page, value: autoSuggestValue.value });
        }
    };

    return (
        <div>
            <label>{props.label}</label>
            <Autosuggest
                suggestions={autoSuggestValue.suggestions}
                onSuggestionsFetchRequested={onSuggestionsFetchRequested}
                onSuggestionsClearRequested={onSuggestionsClearRequested}
                getSuggestionValue={getSuggestionValue}
                renderSuggestion={renderSuggestion}
                renderSuggestionsContainer={renderSuggestionsContainer}
                inputProps={inputProps}
            />
        </div>
        
    );
};

ICD10Autosuggest.propTypes = {
    handleOnChange: PropType.func,
    label: PropType.string,
    name: PropType.string,
    value: PropType.string
};

export default ICD10Autosuggest;