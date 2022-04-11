import React from 'react';
import PropType from 'prop-types';
import Select from './Select';
import { useAdditionalQuest } from '../../CustomHook/CustomHook';

const SelectAQ = (props) => {
    const { ChangeAddionalQuest, componentChildren } = useAdditionalQuest(props);

    return (
        <div className={`ui-Col-${props.colwidth}`} style={{float: "left"}}>
            <Select {...props} colwidth={100} handleOnChange={ChangeAddionalQuest} />
            {componentChildren}
        </div>
    );
}

Select.defaultProps = {
    colwidth: 100,
    disabled: false,
    value: ""
};

Select.propTypes = {
    colwidth: PropType.number,
    label: PropType.string,
    name: PropType.string,
    handleOnChange: PropType.func,
    value: PropType.string,
    options: PropType.arrayOf(PropType.shape({
        text: PropType.string.isRequired,
        value: PropType.string.isRequired
    })).isRequired,
    disabled: PropType.bool
};

export default SelectAQ;