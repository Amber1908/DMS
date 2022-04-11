import React, { useEffect, useState } from "react";
import TextInput from "../../ReportItem/TextInput";
import FileUpload from "../../ReportItem/FileUpload";
import Select from "../../ReportItem/Select";
import Textarea from "../../ReportItem/TextArea";
import DeleteButton from "./DeleteButton";
import { Ajax } from "../../../Utilities/AjaxUtility";

const questTypes = [
    { text: "文字", value: "text" },
    { text: "數值", value: "number" },
    { text: "下拉", value: "select" },
    { text: "單選", value: "radio" },
    { text: "多選", value: "checkbox" },
    { text: "檔案", value: "file" },
    { text: "PDF", value: "pdf" },
    { text: "民國年", value: "date" },
];

const QuestCommon = (props) => {
    // 問題圖片
    const [imgSrc, setImgSrc] = useState("");

    let indexArg = { groupIndex: props.groupIndex, questIndex: props.index };

    useEffect(() => {
        if (props.Image != null && props.Image != "") {
            let url = `${Ajax.webapiBaseURL}/Report/GetReportMainFile?RQID=${props.ID}&fileName=${props.Image}`;

            fetch(url)
                .then((res) => {
                    if (res.status != 200) {
                        throw new Error("Not Found");
                    }

                    return res.blob();
                })
                .then((blob) => {
                    // 顯示圖片
                    let objURL = URL.createObjectURL(blob);
                    setImgSrc(objURL);

                    // 將圖片填入State
                    let fakeEvent = {
                        target: {
                            name: "File",
                            value: new File([blob], props.Image, blob),
                        },
                    };
                    props.handleImageChange(fakeEvent, { groupIndex: props.groupIndex, questIndex: props.index });
                })
                .catch((error) => {
                    // alert(error);
                });
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

            reader.onload = (e) => {
                setImgSrc(e.target.result);
            };

            reader.readAsDataURL(target.value);
            props.handleImageChange(e, args);
        } else {
            setImgSrc(null);
            props.handleImageChange(e, args);
        }

        let fakeEvent = {
            target: {
                name: "Image",
                value: target.value == null ? null : target.value.name
            }
        };
        props.handleOnChange(fakeEvent, indexArg);
    };

    const handleQuestTextChange = (e) => {
        props.handleOnChange(e, indexArg);
        let fakeEvent = {
            target: {
                name: "QuestionNo",
                value: e.target.value
            },
        };
        props.handleOnChange(fakeEvent, indexArg);
    }

    const handleQuestTypeChange = (e) => {
        props.handleOnChange(e, indexArg);
    }

    let imgElement = null;
    if (imgSrc != null && imgSrc != "") {
        imgElement = (
            <div style={{ position: "relative", display: "inline-block" }}>
                <button className="remove-quest close fa fa-times" type="button" onClick={(e) => handleImageChange({ target: { name: "File", value: null } }, indexArg)} />
                <img style={{ maxWidth: "100%" }} src={imgSrc} />
            </div>
        );
    }

    return (
        <>
            <DeleteButton handleOnClick={(e) => props.handleDeleteQuest(e, props.groupIndex, props.index)} />
            <div className="edit-quest-heading">
                <TextInput
                    placeholder="問題標題"
                    required={true}
                    colwidth={60}
                    name="QuestionText"
                    value={props.QuestionText}
                    inputProps={{ maxLength: "1000" }}
                    handleOnChange={handleQuestTextChange}
                />
                <FileUpload accept="image/*" icon="fa-photo" text="上傳圖片" name="File" handleOnChange={(e) => handleImageChange(e, indexArg)} />
                <Select colwidth={20} name="QuestionType" value={props.QuestionType} options={questTypes} style={{ float: "none" }} handleOnChange={(e) => props.handleOnChange(e, indexArg)} />
            </div>
            <div>
                <Textarea
                    name="Description"
                    value={props.Description}
                    rows={3}
                    placeholder="問題描述"
                    textAreaProps={{ maxLength: "3000" }}
                    handleOnChange={(e) => props.handleOnChange(e, indexArg)}
                />
            </div>
            <div>{imgElement}</div>
        </>
    );
};

export default QuestCommon;
