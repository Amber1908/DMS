import React, { useState } from 'react';
import PropType from 'prop-types';
import moment from 'moment';

const currentROC = moment().year() - 1911;
const BirthDatePicker = (props) => {
    const handleOnChange = (e) => {
        let birth = splitBirthDate(props.value);
        let name = e.target.name;
        let value = e.target.value;
        let fakeEvent = { target: { name: "", value: "" }};

        birth[name] = value;
        birth.year = birth.year !== "" ? parseInt(birth.year) + 1911 : "";
        fakeEvent.target.name = props.name;
        fakeEvent.target.value = `${birth.year}-${birth.month}-${birth.date}`;
        props.onChange(fakeEvent);
    }

    const splitBirthDate = (date) => {
        let year = "", month = "",  birthdate = "";

        if (date !== "") {
            let dateAry = date.split("-");
            year = dateAry[0] !== "" ? parseInt(dateAry[0]) - 1911 : "";
            month = dateAry[1];
            birthdate = dateAry[2].substring(0, 2);
        }

        return { year, month, date: birthdate };
    }

    let birthObj = splitBirthDate(props.value);
    return (
        <div className={`item-wrap edit ${props.className}`}>
            <div>生日</div>
            <div className="input-item">
                民國
                <input value={birthObj.year} name="year" type="number" onChange={handleOnChange} required={props.required} min="1" max={currentROC} style={{ width: "60px", float: "none" }} />
                年
                <input value={birthObj.month} name="month" type="number" onChange={handleOnChange} required={props.required} min="1" max="12" style={{ width: "60px", float: "none" }} />
                月
                <input value={birthObj.date} name="date" type="number" onChange={handleOnChange} required={props.required} min="1" max="31" style={{ width: "60px", float: "none" }} />
                日
            </div>
        </div>
    );
}

BirthDatePicker.defaultProps = {
    required: false,
    value: "",
    onChange: () => {}
}

BirthDatePicker.propTypes = {
    required: PropType.bool,
    value: PropType.string,
    onChange: PropType.func
}

export default BirthDatePicker;