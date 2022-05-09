import React, { useState, useEffect } from 'react';
import Autosuggest from 'react-autosuggest';
import PropType from 'prop-types';

import { GlobalConstants } from '../../Utilities/CommonUtility';
import { usePostAuth, useLazyLoading } from '../../CustomHook/CustomHook';
import { set } from 'lodash';

const getSuggestionValue = suggestion => suggestion.HospitalCode +" "+suggestion.HospitalName

const HospitalCodeAndNameAutosuggest = (props) => {
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
    const getSuggestions = (value, reason) => {
        var setTimeout05sec;
        var setTimeout03sec;
        var setTimeout01sec;
        function myStopFunctionset(valueLength) {
            switch (valueLength) {
                case 1:
                    clearTimeout(setTimeout03sec)
                    clearTimeout(setTimeout01sec)
                    brake;
                case 2:
                    clearTimeout(setTimeout05sec)
                    clearTimeout(setTimeout01sec)
                    brake;
                case 3:
                    clearTimeout(setTimeout05sec)
                    clearTimeout(setTimeout03sec)
                    brake;
            };
        }

        if (reason === 'input-changed' && value.length < 2) {
            setTimeout05sec=
            setTimeout(function () {
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
                                suggestions: prev.suggestions.concat(response.Data),
                            };
                        });
                    }
                });
            }, 500)
           myStopFunctionset(value.length);
        } else if (reason === 'input-changed' && value.length < 3) {
            setTimeout(function () {
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
                                suggestions: prev.suggestions.concat(response.Data),
                            };
                        });
                    }
                });
            }, 300)
            myStopFunctionset(value.length);
        } else if (reason === 'input-changed' && value.length <4) {
            setTimeout(function () {
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
                                suggestions: prev.suggestions.concat(response.Data),
                            };
                        });
                    }
                });
            }, 100)
            myStopFunctionset(value.length);
        } else {
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
                            suggestions: prev.suggestions.concat(response.Data),
                        };
                    });
                }
            });
        }
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
    const onSuggestionsFetchRequested = ({ value,reason }) => {
        onSuggestionsClearRequested();
        getSuggestions(value,reason);
    };

    // Autosuggest will call this function every time you need to clear suggestions.
    const onSuggestionsClearRequested = () => {
        setAutoSuggestValue(prev => {
            return {
                ...prev,
                suggestions:[]
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
            {suggestion.HospitalName}<br />
            {suggestion.HospitalCode}
        </div>
    );
    return (
        <div style={{ width: "100%" }} >
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

HospitalCodeAndNameAutosuggest.defaultProps = {
    style: {},
    handleOnChange: () => { }
}

HospitalCodeAndNameAutosuggest.propTypes = {
    handleOnChange: PropType.func,
    name: PropType.string,
    value: PropType.string,
    handleSuggestionSelected: PropType.func,
    style: PropType.object
};
export default HospitalCodeAndNameAutosuggest;