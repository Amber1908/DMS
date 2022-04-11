import React, { useEffect } from "react";
import PropType from 'prop-types';
import $ from 'jquery';
import '../../lib/bootstrap-datepicker';
import { useAuthEffect } from "../../../obj/Release/AspnetCompileMerge/TempBuildDir/Scripts/CustomHook/useAuthEffect";

// 一般類型問題
const TwDatePicker = (props) => {

    useEffect(() => {
        $('.datepicker').datepicker(
            {
                language: 'zh-TW',
                maxViewModel: 1,
                autoclose: true,
            }).on("input change", function (e) {
                let fakeEvent = {
                    target: {
                        name: e.target.name,
                        value: e.target.value
                    }
                };
                props.handleOnChange(fakeEvent);
            });
    }, [])

    return (
        <div style={{ width: "100%" }}>
            <div></div>
            <div className={"input-item edit"}>
                <input value={props.value} type={props.type} name={props.name} placeholder={props.placeholder} className={"datepicker"} onChange={props.handleOnChange} style={{ resize: "vertical", width: "100%" }} required={props.required} />
            </div>
        </div>
    );
};

TwDatePicker.propTypes = {
    label: PropType.string,
    name: PropType.string.isRequired,
    placeholder: PropType.string,
    handleOnChange: PropType.func
};

export default TwDatePicker;
