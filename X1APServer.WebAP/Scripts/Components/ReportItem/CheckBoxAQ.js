import React from 'react'
import PropType from 'prop-types';
import CheckBox from './CheckBox';
import { useAdditionalQuest } from '../../CustomHook/CustomHook';

const CheckBoxAQ = (props) => {
    const { ChangeAddionalQuest, componentChildren } = useAdditionalQuest(props);

    return (
        <div style={{float: "left"}}>
            <CheckBox {...props} handleOnChange={ChangeAddionalQuest} />
            {componentChildren}
        </div>
    );
}

CheckBoxAQ.propTypes = {
    handleOnChange: PropType.func,
    name: PropType.string,
    trueValue: PropType.string.isRequired,
    text: PropType.string.isRequired,
    value: PropType.any,
    id: PropType.string
};

export default CheckBoxAQ;