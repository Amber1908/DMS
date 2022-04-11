import React, { useState, useEffect, useRef } from 'react';
import PropType from 'prop-types';
import { Ajax } from '../../Utilities/AjaxUtility';

const ImageClipboard = (props) => {
    // States
    // 圖片網址
    const [imgSrc, setImgSrc] = useState("");
    // 圖片class
    const [imgClassName, setImgClassName] = useState("");

    const inputRef = useRef(null);

    useEffect(() => {
        if (props.value != null && props.value != "") {
            setImgSrc(props.value);
        }
    }, [props.value])

    // 將貼至input的圖片設置到圖片網址
    const handlePaste = (e) => {
        let items = e.clipboardData.items;
        let blob = null;

        for(let i = 0; i < items.length; i++) {
            if (items[i].type.indexOf("image") === 0) {
                blob = items[i].getAsFile();
            }
        }

        if (blob != null) {
            let reader = new FileReader();
            
            reader.onload = (e) => {
                setImgSrc(e.target.result);
            };

            reader.readAsDataURL(blob);
            let event = { target: { name: props.name, value: blob } };
            if (props.handleOnChange != null) {
                props.handleOnChange(event);
            }
        }
    }

    // input focus 使 onPaste有效
    const handleImageClick = (e) => {
        setImgClassName("image-clipboard-focus");
        inputRef.current.focus();
    }

    // 未貼上圖片顯示預設圖片
    const handleImageError = (e) => {
        setImgSrc(Ajax.BaseURL + "/Content/Images/PasteImageHere.png");
    }

    // 移除圖片點擊外觀
    const handleBlur = (e) => {
        setImgClassName("");
    }


    const handleKeyUp = (e) => {
        switch (e.keyCode) {
            // 刪除圖片改為預設值
            case 46:
                setImgSrc(Ajax.BaseURL + "/Content/Images/PasteImageHere.png");
                break;
        }
    }

    return (
        <div name={props.name} className={`image-clipboard ${imgClassName}`} onClick={handleImageClick} onKeyUp={handleKeyUp}>
            <input className="image-clipboard-input" ref={inputRef} readOnly onPaste={handlePaste} onBlur={handleBlur} />
            <div style={{display: "table", width: "100%", height: "100%"}}>
                <div style={{display: "table-cell", verticalAlign: "middle"}}>
                    <img className="image-clipboard-img" src={imgSrc} onError={handleImageError} />
                </div>
            </div>
        </div>
    );
}

ImageClipboard.defaultProps = {
    focus: false
};

ImageClipboard.propTypes = {
    width: PropType.number,
    name: PropType.string,
    value: PropType.string
};

export default ImageClipboard;