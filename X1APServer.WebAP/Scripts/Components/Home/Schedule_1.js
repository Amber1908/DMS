import React, { useContext, useEffect, useState } from 'react';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { useAuthEffect } from '../../CustomHook/useAuthEffect';
import { GetDate, GlobalConstants } from '../../Utilities/CommonUtility';
import { CaseInfoContext } from '../Context';
import LoadingComponent from '../LoadingComponent';
import moment from 'moment';

const Schedule = (props) => {
    const formInit = {
        id: null,
        content: "",
        date: ""
    };
    const now = moment().format("YYYY-MM-DD");

    const [scheduleList, setScheduleList] = useState({
        data: [],
        status: GlobalConstants.Status.INIT
    });
    const [form, setForm] = useState(formInit);
    const [mode, setMode] = useState("");

    const { caseInfo } = useContext(CaseInfoContext);

    const { PostWithAuth } = usePostAuth();

    useAuthEffect(() => {
        if (caseInfo.IDNo == null || caseInfo.IDNo === "") return;

        getScheduleList(caseInfo.ID);
    }, [caseInfo.IDNo])

    const getScheduleList = (pid) => {
        setScheduleList(prev => ({ ...prev, status: GlobalConstants.Status.LOADING }));
        PostWithAuth({
            url: "/Patient/GetScheduleList",
            data: {
                "PatientID": pid,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (rsp) => {
                setScheduleList(prev => ({ ...prev, data: rsp.ScheduleList }));
            },
            final: () => {
                setScheduleList(prev => ({ ...prev, status: GlobalConstants.Status.INIT }));
            }
        })
    }

    const createScheduleList = () => {
        let scheduleListElement = scheduleList.data.map((s, i) => (
            <a className="list-group-item" key={i} style={{ position: "relative" }} onClick={e => handleEditSchedule(e, s.ID)}>
                <h4 className="list-group-item-heading" style={{ marginLeft: 0 }}>{s.ContentText}</h4>
                <p className="list-group-item-text">{GetDate(s.ReturnDate)}</p>
                <button type="button" className="close" style={{ position: "absolute", top: "10px", right: "10px" }} onClick={e => handleDeleteSchedule(e, s.ID)}><span aria-hidden="true">×</span></button>
            </a>
        ));

        if (scheduleListElement.length == 0) {
            scheduleListElement = (<div className="text-center">無資料</div>);
        }

        return scheduleListElement;
    }

    const handleEditSchedule = (e, id) => {
        const schedule = scheduleList.data.find(s => s.ID === id);
        const returnDate = moment(schedule.ReturnDate).format("YYYY-MM-DD");
        setForm({
            id: schedule.ID,
            content: schedule.ContentText,
            date: returnDate
        });
        setMode("edit");
    }

    const handleCancelEdit = (e) => {
        setForm(formInit);
        setMode("");
    }

    const handleDeleteSchedule = (e, id) => {
        e.stopPropagation();
        if (!confirm("確定刪除?")) return;

        PostWithAuth({
            url: "/Patient/DeleteSchedule",
            data: {
                "ID": id,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (rsp) => {
                getScheduleList(caseInfo.ID);
            }
        })
    }

    const handleSaveSchedule = (e) => {
        e.preventDefault();

        PostWithAuth({
            url: "/Patient/AddOrUpdateSchedule",
            data: {
                "ID": form.id,
                "PatientID": caseInfo.ID,
                "ContentText": form.content,
                "ReturnDate": form.date,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: () => {
                setForm(formInit);
                getScheduleList(caseInfo.ID);
                setMode("");
                window.alert("儲存成功!");
            }
        })
    }

    const handleInputChange = (e) => {
        const target = e.target;
        setForm(prev => ({
            ...prev,
            [target.name]: target.value
        }));
    }


    let saveBtnText = "新增回診時間";
    if (mode === "edit") saveBtnText = "儲存";

    return (
        <div className="list-group">
            <h4 style={{marginLeft: "0"}}>新增回診</h4>
            <div className="panel panel-default">
                <div className="panel-body appointmentForm">
                    <form onSubmit={handleSaveSchedule}>
                        <div className="form-group">
                            <label htmlFor="returnDate">回診時間</label>
                            <input type="date" className="form-control" id="date" name="date" min={now} value={form.date} required style={{ fontSize: "12px" }} onChange={handleInputChange} />
                        </div>
                        <div className="form-group">
                            <label htmlFor="content">回診內容</label>
                            <input type="text" className="form-control" id="content" name="content" value={form.content} required style={{ fontSize: "12px" }} onChange={handleInputChange} />
                        </div>
                        <div>
                            <button className={`btn btn-v2 save-schedule-btn ${mode}`}>{saveBtnText}</button>
                            <button type="button" className={`btn btn-v2 cancel-schedule-btn ${mode}`} onClick={handleCancelEdit} >取消</button>
                        </div>
                    </form>
                </div>
            </div>
            <h4 style={{marginLeft: "0"}}>回診清單</h4>
            <LoadingComponent status={scheduleList.status}>
                {createScheduleList()}
            </LoadingComponent>
        </div>
    );
}

export default Schedule;
