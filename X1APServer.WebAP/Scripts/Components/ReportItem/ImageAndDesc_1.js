import React from "react";
import PropType from 'prop-types';

import TextInput from './TextInput';

const ImageAndDesc = (props) => {
    let colwidthClass;

    if (props.colwidth != null) {
        colwidthClass = `ui-Col-${props.colwidth}`;
    }

    let hidden = props.value == null || props.value == "" ? true : false;

    return (
        <div className={`imagedesc-container ${colwidthClass}`} hidden={hidden}>
            <div className="image-container">
                <img className="ui-statistic-img" src={props.value} />
            </div>
            <TextInput placeholder={props.placeholder} inputStyleType="text-item" colwidth={100} label={props.label} name={props.descName} handleOnChange={props.handleOnChange} value={props.form[props.descName]} />
        </div>
    );
};

ImageAndDesc.defaultProps = {
    colwidth: 100
};

ImageAndDesc.propTypes = {
    colwidth: PropType.number,
    label: PropType.string,
    name: PropType.string.isRequired,
    placeholder: PropType.string,
    hasChild: PropType.bool.isRequired,
    descName: PropType.string.isRequired,
    handleOnChange: PropType.func
};

export default ImageAndDesc;