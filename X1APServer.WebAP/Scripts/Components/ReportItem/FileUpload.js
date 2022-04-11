import React from 'react';
import PropType from 'prop-types';

// 上傳檔案元件
const FileUpload = (props) => {
    const handleOnChange = (e) => {
        let fakeEvent = {
            target: {
                name: e.target.name,
                value: e.target.files[0]
            }
        };
        
        props.handleOnChange(fakeEvent);
        e.target.value = "";
    }
    
    return (
        <label className="btn btn-default" style={props.style}>
            <input name={props.name} onChange={handleOnChange} style={{ display: "none" }} type="file" accept={props.accept} />
            <i className={`fa ${props.icon}`}></i>
            {props.text}
        </label>
    );
}

FileUpload.defaultProps = {
    icon: ""
}

FileUpload.propTypes = {
    style: PropType.object,
    icon: PropType.string,
    text: PropType.string,
    handleOnChange: PropType.func,
    name: PropType.string,
    accept: PropType.string,
    value: PropType.string
}

export default FileUpload;