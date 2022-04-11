import React, { useState, useEffect } from 'react';
import RadioGroup from './RadioGroup';
import PropType from 'prop-types';
import { useAdditionalQuest } from '../../CustomHook/CustomHook';

const RadioGroupAQ = (props) => {
    const { ChangeAddionalQuest, componentChildren } = useAdditionalQuest(props);

    return (
        <div>
            <RadioGroup {...props} handleOnChange={ChangeAddionalQuest} />
            {componentChildren}
        </div>
    );
}

RadioGroupAQ.defaultProps = {
    required: false,
    value: ""
};

RadioGroupAQ.propTypes = {
    name: PropType.string,
    handleOnChange: PropType.func,
    value: PropType.string,
    options: PropType.arrayOf(PropType.shape({
        text: PropType.string.isRequired,
        value: PropType.string.isRequired,
        additionalQuest: PropType.string
    })).isRequired,
    className: PropType.string,
    label: PropType.string,
    required: PropType.bool,
    hasChild: PropType.bool,
    form: PropType.object
};

export default RadioGroupAQ;