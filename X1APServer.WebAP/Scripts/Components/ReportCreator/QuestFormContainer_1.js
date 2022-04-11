import React, { useState, useEffect } from 'react';
import PropType from 'prop-types';
import TextInput from '../ReportItem/TextInput';
import TextArea from '../ReportItem/TextArea';
import CheckBox from '../ReportItem/CheckBox';
import FileUpload from '../ReportItem/FileUpload';
import { Ajax } from '../../Utilities/AjaxUtility';
import Log from '../../Utilities/LogUtility';
import Select from '../ReportItem/Select';

const QuestFormContainer = (props) => {
    // 問題圖片
    const [imgSrc, setImgSrc] = useState("");

    useEffect(() => {
        if (props.Image != null && props.Image != "") {
            let url = `${Ajax.webapiBaseURL}/Report/GetReportMainFile?RQID=${props.ID}&fileName=${props.Image}`;

            fetch(url)
                .then(res => {
                    if (res.status != 200) {
                        throw new Error("Not Found");
                    }

                    return res.blob();
                })
                .then(blob => {
                    // 顯示圖片
                    let objURL = URL.createObjectURL(blob);
                    setImgSrc(objURL);

                    // 將圖片填入State
                    let fakeEvent = {
                        target: {
                            name: "File",
                            value: new File([blob], props.Image, blob)
                        }
                    };
                    props.handleImageChange(fakeEvent, { groupIndex: props.groupIndex, questIndex: props.index });
                })
                .catch(error => {
                    // alert(error);
                })
        }
    }, [props.Image]);

    const handleImageChange = (e, args) => {
        let target = e.target;
        if (target.value) {
            if (!target.value.type.match("image.*")) {
                alert("檔案不是圖片格式!");
                return;
            }

            let sixMegaByte = 6291456;
            if (target.value.size > sixMegaByte) {
                alert("上傳的圖片大小不得超過 6 MB!");
                return;
            }

            // 預覽上傳的圖片
            let reader = new FileReader();

            reader.onload = e => {
                setImgSrc(e.target.result);
            }

            reader.readAsDataURL(target.value);
            props.handleImageChange(e, args);
        } else {
            setImgSrc(null);
            props.handleImageChange(e, args);
        }
    }

    const handleRequiredChange = (e, indexArg) => {
        let changeEvent = {
            target: {
                name: e.target.name,
                value: e.target.value == "true" || e.target.value == true ? true : false
            }
        }

        props.handleOnChange(changeEvent, indexArg)
    }

    let imgElement = null;
    if (imgSrc != null && imgSrc != "") {
        imgElement = (
            <div style={{ position: "relative", display: "inline-block" }}>
                <button className="remove-quest close fa fa-times" type="button" onClick={e => handleImageChange({ target: { name: "File", value: null } }, indexArg)} />
                <img style={{ maxWidth: "100%" }} src={imgSrc} />
            </div>
        );
    }

    let indexArg = { groupIndex: props.groupIndex, questIndex: props.index };

    let otherAnsElement = null;
    if (props.otherAnsFlag) {
        otherAnsElement = (<CheckBox key={"OtherAns_" + props.groupIndex + "_" + props.index} id={"OtherAns_" + props.groupIndex + "_" + props.index} key={"OtherAns_" + props.index} className="required" text="其他" trueValue="true" name="OtherAns" value={props.otherans} handleOnChange={e => handleRequiredChange(e, indexArg)} />);
    }

    return (
        <>
            <div className="quest-editor-container" key={props.index}>
                <button className="remove-quest close fa fa-times" type="button" onClick={e => props.handleDeleteQuest(e, props.groupIndex, props.index)}></button>
                <div className="row">
                    <TextInput className="text-editor-container" key={"Title_" + props.groupIndex + "_" + props.index} label={props.titleLabel} name={`QuestionText`} handleOnChange={e => props.handleOnChange(e, indexArg)} value={props.questText} />
                    <FileUpload key={"File_" + props.groupIndex + "_" + props.index} accept="image/*" icon="fa-photo" style={{ alignSelf: "center" }} text="上傳圖片" handleOnChange={e => handleImageChange(e, indexArg)} name="File" />
                    <CheckBox key={"Required_" + props.groupIndex + "_" + props.index} id={"Required_" + props.groupIndex + "_" + props.index} className="required" text="必填" trueValue="true" name="Required" value={props.required} handleOnChange={e => handleRequiredChange(e, indexArg)} />
                    <CheckBox key={"Follow_" + props.groupIndex + "_" + props.index} id={"Follow_" + props.groupIndex + "_" + props.index} className="required allow-interact" text="關注" trueValue="true" name="Pin" value={props.pin} handleOnChange={e => handleRequiredChange(e, indexArg)} />
                    {otherAnsElement}
                </div>
                <div className="row">
                    {/* <TextInput className="text-editor-container" key={"Desc_" + index} label="問題描述" name="Description" handleOnChange={e => props.handleOnChange(e, { groupIndex: props.index, questIndex: index })} value={value.Description} required={true} /> */}
                    <TextInput className="text-editor-container" key={"QuestionNo_" + props.groupIndex + "_" + props.index} label="問題編號(對應匯入及匯出模板編號)" name="QuestionNo" handleOnChange={e => props.handleOnChange(e, indexArg)} value={props.questionNo} />
                    <TextInput type="number" className="text-editor-container" key={"CodingBookIndex_" + props.groupIndex + "_" + props.index} label="CodingBook Index(-1: 不顯示)" name="CodingBookIndex" handleOnChange={e => props.handleOnChange(e, indexArg)} value={props.codingBookIndex} required={true} />
                    {/* <TextInput className="text-editor-container" key={"CodingBookTitle_" + props.groupIndex + "_" + props.index} label="CodingBook 標題" name="CodingBookTitle" handleOnChange={e => props.handleOnChange(e, indexArg)} value={props.codingBookTitle} /> */}
                </div>
                <div className="row">
                    {imgElement}
                </div>
                <div className="row">
                    {/* <TextInput className="text-editor-container" key={"Desc_" + index} label="問題描述" name="Description" handleOnChange={e => props.handleOnChange(e, { groupIndex: props.index, questIndex: index })} value={value.Description} required={true} /> */}
                    <TextArea rows={3} className="ui-Col-100" label="問題描述" name="Description" handleOnChange={e => props.handleOnChange(e, indexArg)} value={props.description} />
                </div>
                {props.children}
            </div>
        </>
    );
}

QuestFormContainer.defaultProps = {
    otherAnsFlag: false
}

QuestFormContainer.propTypes = {
    index: PropType.number,
    groupIndex: PropType.number,
    titleLabel: PropType.string,
    questText: PropType.string,
    required: PropType.bool,
    description: PropType.string,
    handleDeleteQuest: PropType.func,
    handleImageChange: PropType.func,
    handleOnChange: PropType.func,
    otherAnsFlag: PropType.bool
}

export default QuestFormContainer;