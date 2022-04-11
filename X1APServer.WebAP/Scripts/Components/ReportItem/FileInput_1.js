import React, { useRef } from 'react';
import PropType from 'prop-types';

const FileInput = (props) => {
    const fileInputRef = useRef();

    const handleOnChange = (e) => {
        let target = e.target;
        let file = target.files == null ? null : target.files[0];
        let onehundredMB = 104857600

        if (file && file.size > onehundredMB) {
            window.alert("檔案大小不能超過100MB!");
            return;
        }

        let fakeEvent = {
            target: {
                name: props.name,
                value: file
            }
        };
        props.handleOnChange(fakeEvent);
    }

    // 刪除上傳檔案
    const handleDeleteFile = (e) => {
        fileInputRef.current.value = "";
        handleOnChange({target: {name: props.name, value: ""}});
    }

    // 讓使用者可以下載先前上傳的檔案
    let downloadElement = null;
    if (typeof props.value == "string") {
        downloadElement = (
            <a className="btn btn-default" href={props.value} style={{marginRight: "10px"}}>
                <i className="fa fa-download" style={{margin: "0px"}}></i>
                下載檔案
            </a>
        );
    }

    let deleteElement = null;
    if (props.value != null) {
        deleteElement = (
            <button type="button btn-error" onClick={handleDeleteFile}>刪除檔案</button>
        );
    }

    const isRequired = () => {
        return props.required && !props.value;
    }

    return (
        <div>
            <label style={{fontWeight: "normal"}}>{props.label}</label>
            <div>
                {downloadElement}
                <input ref={fileInputRef} type="file" name={props.name} onChange={handleOnChange} required={isRequired()} style={{ display: "inline-block" }} />
                {/* {deleteElement} */}
            </div>
            <p>僅能上傳小於100MB的檔案</p>
        </div>
    )
}

FileInput.propTypes = {
    label: PropType.string,
    name: PropType.string,
    handleOnChange: PropType.func,
    required: PropType.bool,
};

export default FileInput;