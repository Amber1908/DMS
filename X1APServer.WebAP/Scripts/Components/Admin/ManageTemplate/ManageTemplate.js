import produce from 'immer';
import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { usePostAuth } from '../../../CustomHook/CustomHook';
import { Ajax } from '../../../Utilities/AjaxUtility';
import { GetDate, GetDateTime, GlobalConstants } from '../../../Utilities/CommonUtility';
import LoadingComponent from '../../LoadingComponent';
import CheckBox from '../../ReportItem/CheckBox';
import FileInput from '../../ReportItem/FileInput';
import Select from '../../ReportItem/Select';
import TextInput from '../../ReportItem/TextInput';

const extraQuestInit = {
    "QuestText": "",
    "QuestType": "",
    "Required": false,
    "QuestName": ""
};

const ManageTemplate = (props) => {
    const [templateList, setTemplateList] = useState({
        data: [],
        status: GlobalConstants.Status.INIT
    });
    const [selectedTemplate, setSelectedTemplate] = useState({
        data: {
            Name: "",
            TemplateFile: "",
            ExtraQuest: []
        },
        status: GlobalConstants.Status.BEFORE_INIT
    });
    const [searchVal, setSearchVal] = useState("");

    const { PostWithAuth } = usePostAuth();

    const reqGetTemplateList = () => {
        setTemplateList(prev => ({ ...prev, status: GlobalConstants.Status.LOADING }));
        PostWithAuth({
            url: "/Report/GetReportTemplateList",
            data: {
                "FuncCode": GlobalConstants.FuncCode.Backend,
                "AuthCode": 1
            },
            success: (rsp) => {
                setTemplateList({
                    data: rsp.TemplateList.map(x => ({ ...x, hidden: false })),
                    status: GlobalConstants.Status.INIT
                });
            }
        });
    }

    const reqGetExtrQuestList = (id) => {
        PostWithAuth({
            url: "/Report/GetETemplateEQuestList",
            data: {
                "ExportTemplateID": id,
                "FuncCode": GlobalConstants.FuncCode.Backend,
                "AuthCode": 1
            },
            success: (rsp) => {
                setSelectedTemplate(prev => ({
                    status: GlobalConstants.Status.INIT, 
                    data: { ...prev.data, ExtraQuest: rsp.ExtraQuestList } 
                }));
            }
        });
    }

    const reqGetTemplate = (id) => {
        setSelectedTemplate(prev => ({ ...prev, status: GlobalConstants.Status.LOADING }));
        PostWithAuth({
            url: "/Report/GetReportTemplate",
            data: {
                "ID": id,
                "FuncCode": GlobalConstants.FuncCode.Backend,
                "AuthCode": 1
            },
            success: (rsp) => {
                setSelectedTemplate(prev => {
                    return {
                        ...prev,
                        data: { ...prev.data, ...rsp.Data }
                    };
                });
                reqGetExtrQuestList(id)
            }
        })
    }

    useEffect(() => {
        reqGetTemplateList();
    }, []);

    const handleTemplateClick = (e, id) => {
        reqGetTemplate(id);
        // setSelectedTemplate(prev => ({ status: GlobalConstants.Status.INIT, data: templateList.data.find(x => x.ID === id)}));
    }

    const checkStatus = (data, beforeInitVal, initVal) => {
        if (data.status === GlobalConstants.Status.BEFORE_INIT) {
            return beforeInitVal;
        } else {
            return initVal;
        }
    }

    const handleSearch = (e, value = searchVal) => {
        e.preventDefault();
        // let filterVal = value || searchVal;
        setTemplateList(prev => {
            let list = prev.data.map(x => {
                let hiddenVal = false;
                if (x.Name.indexOf(value) === -1) hiddenVal = true;
                return { ...x, hidden: hiddenVal };
            });

            return {
                ...prev,
                data: list
            }
        });
    }

    const handleSearchChange = (e) => {
        setSearchVal(e.target.value);
    }

    const handleResetSearch = (e) => {
        setSearchVal("");
        handleSearch(e, "");
    }

    const handleInputChange = (e) => {
        const target = e.target;
        setSelectedTemplate(prev => ({ ...prev, data: { ...prev.data, [target.name]: target.value } }));
    }

    const handleExtraQuestChange = (e, index) => {
        const target = e.target;
        setSelectedTemplate(prev => {
            let newState = produce(prev, draft => {
                draft.data.ExtraQuest[index][target.name] = target.value
            });
            return newState;
        });
    }

    const handleAddQuest = (e, index) => {
        setSelectedTemplate(prev => {
            let newState = produce(prev, draft => {
                draft = draft.data.ExtraQuest.splice(index + 1, 0, {
                    ...extraQuestInit,
                    ID: (new Date()).getTime()
                });
            });
            return newState;
        });
    }

    const handleDeleteQuest = (e, index) => {
        setSelectedTemplate(prev => {
            let newState = produce(prev, draft => {
                draft = draft.data.ExtraQuest.splice(index, 1);
            });
            return newState;
        });
    }

    let templateElements = [];
    templateList.data.map(x => {
        if (x.hidden) return;

        templateElements.push(
            <div key={x.ID} className="ui-ItemList" onClick={e => handleTemplateClick(e, x.ID)}>
                <img src={Ajax.BaseURL + "/Content/Images/document.png"} />
                <div className="ui-Title">
                    <span name="CaseName">{x.Name}</span>
                </div>
                <div className="ui-Info">
                    <span>建立時間: {GetDateTime(x.CreateDate)}</span>
                </div>
            </div>
        );
    });    

    let contentElement = null;
    if (selectedTemplate.status === GlobalConstants.Status.INIT) {
        let questElements = selectedTemplate.data.ExtraQuest.map((x, index) => (
            <>
                <div key={x.ID} style={{backgroundColor: "rgba(0, 0, 0, 0.1)", padding: "30px", margin: "10px 0px"}}>
                    <button className="remove-quest close fa fa-times" type="button" onClick={e => handleDeleteQuest(e, index)}></button>
                    <div>
                        <TextInput label="問題文字" name="QuestText" value={x.QuestText} style={{ width: "calc(100% - 145px)" }} inputProps={{ maxLength: "50" }} required handleOnChange={e => handleExtraQuestChange(e, index)} />
                        <CheckBox name="Required" text="是否必填?" trueValue={x.Required} style={{ float: "none" }} handleOnChange={e => handleExtraQuestChange(e, index)} />
                    </div>
                    <TextInput label="問題編號(綁定模板中相同編號，只能輸入英文、數字)" name="QuestName" value={x.QuestName} inputProps={{ maxLength: "50", pattern: "[a-zA-Z0-9]+" }} required handleOnChange={e => handleExtraQuestChange(e, index)} />
                    <Select label="問題類型" name="QuestType" value={x.QuestType} style={{ float: "none" }} required handleOnChange={e => handleExtraQuestChange(e, index)} options={[
                        { text: "文字", value: "text" },
                        { text: "日期", value: "date" },
                        { text: "E-mail", value: "email" },
                        { text: "數字", value: "number" }
                    ]} />
                </div>
                <button type="button" className="btn btn-default btn-block" style={{ marginTop: "10px" }} onClick={e => handleAddQuest(e, index)}>+</button>
            </>
        ));

        contentElement = (
            <div className="QForm-Wrap">
                <div className="QForm ui-profileWrap">
                    <TextInput label="模板名稱" value={selectedTemplate.data.Name} inputProps={{ maxLength: "50" }} required handleOnChange={handleInputChange} />
                    <FileInput label="模板檔案" name="TemplateFile" value={selectedTemplate.data.FileUrl} required handleOnChange={handleInputChange} />
                    <h3>額外問題</h3>
                    {questElements}
                </div>
            </div>
        );
    }

    return (
        <div>
            <div className="section">
                <div className="container containerMax">
                    <div className="row">
                        <div className="col-md-12">
                            <div className="ui-monoBlock ui-cardBlock ui-FX-shadow">
                                <div className="ui-controllerBlock  col-md-3">
                                    <form className="ui-SearchBlockWrap" onSubmit={handleSearch}>
                                        <h4><i className="fa fa-search" aria-hidden="true" />列表搜尋</h4>
                                        <input placeholder="關鍵字" value={searchVal} style={{width: "155px"}} onChange={handleSearchChange} />
                                        <button>搜尋</button>
                                        <button type="button" onClick={handleResetSearch}>重置</button>
                                    </form>
                                    <div to={`${props.match.url}/ExportTemplate/New`} className="ui-listItem-Add" />
                                    <LoadingComponent fontSize="24px" status={templateList.status}>
                                        {templateElements}
                                    </LoadingComponent>
                                </div>
                                <div className={"templateManage ui-tableBlock " + checkStatus(selectedTemplate, "ev-workspaceUnset", "")}>
                                    <form id="mainContainer" className="ui-workspace templateManage" style={{height: "100%"}}>
                                        <div className="ui-toolBar-Wrap">
                                            <div className="ui-toolBar-Group" style={{ border: 'none', overflow: 'visible' }}>
                                                <span className="toolBar-name">{selectedTemplate.data.Name}</span>
                                            </div>
                                            <div className="ui-toolBar-Group ui-Col-55" hidden={checkStatus(selectedTemplate, true, false)}>
                                                <label>模板編輯工具</label>
                                                <div className="ui-buttonGroup">
                                                    <button className="offsetLeft5" data-tooltip="儲存模板"><i className="fa fa-floppy-o" />儲存</button>
                                                    <button className="offsetLeft5" data-tooltip="刪除模板"><i className="fa fa-times" />刪除</button>
                                                </div>
                                            </div>
                                        </div>
                                        <LoadingComponent status={selectedTemplate.status} fontSize="24px">
                                            {contentElement}
                                        </LoadingComponent>
                                    </form>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default ManageTemplate;