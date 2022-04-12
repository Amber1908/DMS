import React, { useState, useEffect, useContext } from 'react';
import { useParams } from 'react-router';
import { Redirect, useHistory } from 'react-router-dom';
import WorkSpaceMain from '../WorkSpaceMain';

import TextInput from '../ReportItem/TextInput';
import QuestionGroup from '../ReportItem/QuestionGroup';
import RadioGroup from '../ReportItem/RadioGroup';
import { CaseInfoContext, URLInfoContext, SharedContext } from '../Context';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { GlobalConstants } from '../../Utilities/CommonUtility';
import BirthDatePicker from '../ReportItem/BirthDatePicker';
import Select from '../ReportItem/Select';


const AddPatientToolbar = (props) => {
    return (
        <>
            <div className="ui-toolBar-Group ui-Col-30">
                <label>工具</label>
                <div className="ui-buttonGroup">
                    <button form="patientForm" className="offsetLeft-5" data-tooltip="儲存個案">
                        <i className="fa fa-floppy-o" aria-hidden="true" />
                        儲存
                    </button>
                </div>
            </div>
        </>
    );
};

const AddPatientWindow = (props) => {

    const [hcchidden, setHcchidden] = useState(true);

    const [hccsource, setHccsource] = useState([]);

    const [source, setSource] = useState([]);

    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        if (source == null || source.length == 0) {
            PostWithAuth({
                url: "/User/GetAreaCodeLazy",
                data: {
                    "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                    "AuthCode": 1
                },
                success: (response) => {

                    if (response.Data) {
                        setSource(response.Data);
                    }
                }
            });
        }
        changeHCC(props.form.AddrCode);
    }, [source, props.form.AddrCode]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        if (name == "AddrCode") {
            changeHCC(value);
        }
        props.handleInputChange(e);
    }

    const changeHCC = (code) => {
        switch (code) {
            case "0732": // 高雄市三民區
                setHccsource([{ "text": "第一衛生所", "value": "2302050015" }, { "text": "第二衛生所", "value": "2302051021" }]);
                if (props.form.HCCode != "2302051021")
                    props.form.HCCode = "2302050015";
                setHcchidden(false);
                break;
            case "4401": // 澎湖縣馬公市
                setHccsource([{ "text": "第一衛生所", "value": "2344010011" }, { "text": "第二衛生所", "value": "2344011027" }, { "text": "第三衛生所", "value": "2344011036" }]);
                if (props.form.HCCode != "2344011027" && props.form.HCCode != "2344011036")
                    props.form.HCCode = "2344010011";
                setHcchidden(false);
                break;
            case "4403": // 澎湖縣白沙鄉
                setHccsource([{ "text": "白沙衛生所", "value": "2344030013" }, { "text": "鳥嶼衛生所", "value": "2344030022" }, { "text": "吉貝衛生所", "value": "2344030031" }]);
                if (props.form.HCCode != "2344030022" && props.form.HCCode != "2344030031")
                    props.form.HCCode = "2344030013";
                setHcchidden(false);
                break;
            case "4405": // 澎湖縣望安鄉
                setHccsource([{ "text": "望安衛生所", "value": "2344050015" }, { "text": "將軍衛生所", "value": "2344050024" }]);
                if (props.form.HCCode != "2344050024")
                    props.form.HCCode = "2344050015";
                setHcchidden(false);
                break;
            case "0321": // 台中市和平區
                setHccsource([{ "text": "和平衛生所", "value": "2336210025" }, { "text": "梨山衛生所", "value": "2336210034" }]);
                if (props.form.HCCode != "2336210034")
                    props.form.HCCode = "2336210025";
                setHcchidden(false);
                break;
            case "0329": // 台中市北屯區
                setHccsource([{ "text": "軍功衛生所", "value": "2317080016" }, { "text": "四民衛生所", "value": "2317081022" }]);
                if (props.form.HCCode != "2317081022")
                    props.form.HCCode = "2317080016";
                setHcchidden(false);
                break;
            case "3701": // 彰化縣彰化市
                setHccsource([{ "text": "南西北區衛生所", "value": "2337010010" }, { "text": "東區衛生所", "value": "2337010029" }]);
                if (props.form.HCCode != "2337010029")
                    props.form.HCCode = "2337010010";
                setHcchidden(false);
                break;
            case "9103": // 連江縣莒光鄉
                setHccsource([{ "text": "西莒衛生所", "value": "2391030016" }, { "text": "東莒衛生所", "value": "2391030026" }]);
                if (props.form.HCCode != "2391030026")
                    props.form.HCCode = "2391030016";
                setHcchidden(false);
                break;
            case "0701": // 高雄市鳳山區
                setHccsource([{ "text": "第一衛生所", "value": "2342010013" }, { "text": "第二衛生所", "value": "9A07010049" }]);
                if (props.form.HCCode != "9A07010049")
                    props.form.HCCode = "2342010013";
                setHcchidden(false);
                break;
            default:
                setHccsource([]);
                props.form.HCCode = "";
                setHcchidden(true);
                break;
        }
    }

    return (

        <div className="QForm-Wrap">
            <form id="patientForm" onSubmit={props.handlePatientSubmit}>
                <fieldset>
                    <QuestionGroup title="個案資料">
                        
                        <div className="Layer3" >
                            <TextInput handleOnChange={props.handleInputChange} value={props.form.ID} colwidth={30} label="ID" name="ID" className="d-none" />
                        </div>
                        <div className="Layer3">
                            <RadioGroup className="ui-col-30" handleOnChange={props.handleInputChange} value={props.form.PUCountry} className="item-wrap" label="國籍" name="PUCountry" options={[
                                { text: "本國", value: "1" },
                                { text: "外籍人士", value: "2" },
                            ]} />
                        </div>
                        <div className="Layer3">
                            <TextInput handleOnChange={props.handleInputChange} value={props.form.PUName} colwidth={30} label="姓名" name="PUName" inputProps={{ required: true, maxLength: "30" }} />
                            <TextInput handleOnChange={props.handleInputChange} value={props.form.IDNo} colwidth={30} label="身份證號" name="IDNo" inputProps={{ title: "大寫英文字母開頭接續9個數字", pattern: "^[A-Z]\\d{9}$", required: true, maxLength: "10" }} />
                            <RadioGroup className="ui-col-30" handleOnChange={props.handleInputChange} value={props.form.Gender} className="item-wrap" label="性別" name="Gender" options={[
                                { text: "男", value: "M" },
                                { text: "女", value: "F" },
                            ]} />
                        </div>
                        <div className="Layer3">
                            <BirthDatePicker className="ui-Col-30" onChange={props.handleInputChange} value={props.form.PUDOB} name="PUDOB" required={true} />
                            <TextInput colwidth={30} label="聯絡電話" name="Phone" value={props.form.Phone} handleOnChange={props.handleInputChange} inputProps={{ required: true, maxLength: "10" }} />
                        </div>
                        <div className="Layer3">
                            <TextInput handleOnChange={props.handleInputChange} value={props.form.ContactPhone} colwidth={30} label="緊急聯絡人電話" name="ContactPhone" inputProps={{ required: false, maxLength: "50" }} />
                            <TextInput handleOnChange={props.handleInputChange} value={props.form.ContactRelation} colwidth={30} label="緊急聯絡人關係" name="ContactRelation" inputProps={{ required: false, maxLength: "100" }} />
                        </div>
                        <div className="Layer3">
                            <Select label="現居地區" colwidth={20} name="AddrCode" value={props.form.AddrCode} options={source} style={{ float: "none" }} handleOnChange={handleInputChange} />
                            <Select label="所屬衛生所醫療機構" colwidth={20} name="HCCode" value={props.form.HCCode} options={hccsource} style={{ float: "none" }} handleOnChange={props.handleInputChange} hidden={hcchidden} />
                            <Select label="戶籍地區" colwidth={20} name="Domicile" value={props.form.Domicile} options={source} style={{ float: "none" }} handleOnChange={props.handleInputChange} />
                        </div>
                        <div className="Layer3">
                            <br />
                            <TextInput handleOnChange={props.handleInputChange} value={props.form.Addr} colwidth={60} label="現居完整地址" name="Addr" inputProps={{ required: false, maxLength: "100" }} />
                        </div>
                        <div className="Layer3">
                            <RadioGroup className="ui-col-30" handleOnChange={props.handleInputChange} value={props.form.Education} className="item-wrap" label="教育" name="Education" options={[
                                { text: "無", value: "1" },
                                { text: "小學", value: "2" },
                                { text: "國(初)中", value: "3" },
                                { text: "高中、高職", value: "4" },
                                { text: "專科、大學", value: "5" },
                                { text: "研究所以上", value: "6" },
                                { text: "拒答", value: "7" },
                            ]} />
                        </div>
                    </QuestionGroup>
                </fieldset>
            </form>
        </div>
    );
};

const AddPatient = (props) => {

    // 個案ID
    const { id } = useParams();

    // 設定個案資料
    const { setCaseInfo } = useContext(CaseInfoContext);
    // 重整使用者列表
    const { setRefreshUserList } = useContext(SharedContext);

    // 個案表單
    const [form, setForm] = useState({
        PUID: "",
        PUCountry: "",
        PUName: "",
        PUDOB: "",
        Gender: "",
        IDNo: "",
        MorphFrom: "",
        Division: "",
        Doctor: "",
        DiagnosisDate: "",
        DiagnosisNo: "",
        State: "",
        Height: "-1",
        Weight: "-1",
        Phone: "",
        Cellphone: "NoData",
        Education: "",
        AddrCode: "",
        HCCode: "",
        Addr: "",
        Domicile: "",
        ID:id
    });

    const history = useHistory();

    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        // 設定個案為新個案
        if (id != null) {
            // 取得個案資料
            PostWithAuth({
                url: "/Patient/GetPatientInfo",
                data: {
                    "ID": id,
                    "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                    "AuthCode": 1
                },
                success: (response) => {
                    setCaseInfo({ ...response.Patient });
                    setForm({ ...response.Patient });
                }
            });
        }
        else {
            setCaseInfo({
                "ID": null,
                "PUID": "",
                "IDNo": "",
                "PUCountry": "",
                "PUName": "新個案",
                "PUDOB": "",
                "Gender": "",
                "Education": "",
                "AddrCode": "",
                "HCCode": "",
                "Addr": "",
                "Domicile": "",
                
            });
        }
    }, [id]);

    // 新增個案
    const handleSavePatientClick = (e) => {
        e.preventDefault();

        PostWithAuth({
            url: "/Patient/AddPatientInfo",
            data: {
                ...form,
                FuncCode: GlobalConstants.FuncCode.AddPatient,
                AuthCode: 1,
                ID: id,
            },
            success: () => {
                alert("儲存成功");
                // 清空個案資料
                setCaseInfo(null);
                // 重整個案清單
                setRefreshUserList((new Date).getTime());
                // 導回首頁
                history.replace("/Index");
            }
        })
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;

        setForm(prev => {
            return {
                ...prev,
                [name]: value
            };
        });
    }

    return (
        <WorkSpaceMain
            toolbar={<AddPatientToolbar />}
            window={<AddPatientWindow
                form={form}
                handleInputChange={handleInputChange}
                handlePatientSubmit={handleSavePatientClick}
            />}
        />
    );
};

export default AddPatient;