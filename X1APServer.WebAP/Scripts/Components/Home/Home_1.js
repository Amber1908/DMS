import produce from 'immer';
import React, { useContext, useEffect, useState } from 'react';
import { usePostAuth } from '../../CustomHook/CustomHook';
import { fillArray } from '../../Utilities/ArrayUtility';
import { GetDate, GlobalConstants } from '../../Utilities/CommonUtility';
import { CaseInfoContext } from '../Context';
import LoadingComponent from '../LoadingComponent';
import Schedule from './Schedule';

import WorkSpaceMain from '../WorkSpaceMain';

export const HomeMode = {
    // 檢視模式
    ViewMode: 0,
    // 編輯個人關注問題模式
    EditMode: 1
}

const HomeWindow = () => {
    // 個案資料
    const { caseInfo } = useContext(CaseInfoContext);
    // 關注項目
    const [pinnedQuestDataList, setPinnedQuestDataList] = useState({
        data: [],
        status: GlobalConstants.Status.INIT
    });
    // 個人關注問題模板
    const [personalPinQuest, setPersonalPinQuest] = useState({
        data: [],
        status: GlobalConstants.Status.INIT
    })
    // 當前模式
    const [currentMode, setCurrentMode] = useState(HomeMode.ViewMode);

    const { PostWithAuth } = usePostAuth()

    useEffect(() => {
        if (caseInfo.ID == null) return;

        getPinnedQuest();
        getPersonalPinnedQuest();
    }, [caseInfo])

    // 取得關注項目
    const getPinnedQuest = () => {
        // 設為 loading 狀態
        setPinnedQuestDataList(prev => ({ ...prev, status: GlobalConstants.Status.LOADING }));
        PostWithAuth({
            url: "/Report/GetPinQuest",
            data: {
                "PatientID": caseInfo.ID,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (rsp) => {
                setPinnedQuestDataList(prev => ({ ...prev, data: rsp.Data }));
            },
            final: () => {
                // 取消 loading 狀態
                setPinnedQuestDataList(prev => ({ ...prev, status: GlobalConstants.Status.INIT }));
            }
        })
    }

    // 取得個人關注問題模板
    const getPersonalPinnedQuest = () => {
        // 設為 loading 狀態
        setPersonalPinQuest(prev => ({ ...prev, status: GlobalConstants.Status.LOADING }));
        PostWithAuth({
            url: "/Report/GetPersonalPinnedQuestList",
            data: {
                "PatientID": caseInfo.ID,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (rsp) => {
                setPersonalPinQuest(prev => ({ ...prev, data: rsp.Data }));
            },
            final: () => {
                // 取消 loading 狀態
                setPersonalPinQuest(prev => ({ ...prev, status: GlobalConstants.Status.INIT }));
            }
        })
    }


    const CreatePinGroup = () => {
        // 無任何關注的問題或關注問題皆沒有答案顯示無資料
        if (pinnedQuestDataList.data.length === 0)
            return (<div style={{ textAlign: "center" }}>無資料</div>);

        const maxValueInRow = 5;
        const emptyObjArray = fillArray({}, maxValueInRow);

        const pinGroupElements = pinnedQuestDataList.data.map((questGroup, i) => {
            let recordList = questGroup.RecordList.concat(emptyObjArray).slice(0, maxValueInRow);
            // 若有標準值，顯示標準值條件
            let ruleText = "";

            return (
                <>
                    <list className="quest">
                        <h4>{questGroup.GroupTitle} {ruleText}</h4>
                        {recordList.map((record, j) => {
                            // 若是異常值顯示提醒
                            let alert = "";
                            if (record.Normal != null && !record.Normal) alert = "questAlert";
                            // 顯示問題答案，空則顯示無資料
                            let value = "無資料";
                            if (record.Value) value = record.Value;

                            return (
                                <div className={`questItem ${alert} ${record.Color}`}>
                                    <label>{GetDate(record.Date)}</label>
                                    <span>{record.QuestionTitle}</span>
                                    <span>{value}</span>
                                </div>
                            );
                        })}
                    </list>
                </>
            );
        });

        return pinGroupElements;
    }

    // 是檢視模式
    const isViewMode = () => {
        return currentMode === HomeMode.ViewMode;
    }

    // 重置個人關注問題
    const resetPersonalPinQuest = () => {
        if (!window.confirm("重置後此病患的關注項目會與公版同步，確定重置?")) return;

        // 刪除所有個人關注問題，刪除後關注項目的顯示及順序會與公版同步
        PostWithAuth({
            url: "/Report/DeletePersonalPinnedQuest",
            data: {
                "PatientID": caseInfo.ID,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (rsp) => {
                setCurrentMode(HomeMode.ViewMode);
                // 重整關注項目
                getPinnedQuest();
            }
        })
    }

    // 將問題上移
    const moveUp = (e, i) => {
        setPersonalPinQuest(prev => {
            const newState = produce(prev, draft => {
                if (i > 0) {
                    let temp = draft.data[i - 1];
                    draft.data[i - 1] = draft.data[i];
                    draft.data[i] = temp;
                }
            });
            return newState;
        });
    }

    // 將問題下移
    const moveDown = (e, i) => {
        setPersonalPinQuest(prev => {
            const newState = produce(prev, draft => {
                if (i < draft.data.length) {
                    let temp = draft.data[i];
                    draft.data[i] = draft.data[i + 1];
                    draft.data[i + 1] = temp;
                }
            });
            return newState;
        });
    }

    // 儲存個人關注模板
    const savePinQuest = () => {
        // 將關注問題加上排序編號
        const reqeustData = personalPinQuest.data.map((q, i) => ({ ...q, Index: i }));
        PostWithAuth({
            url: "/Report/UpdatePersonalPinnedQuest",
            data: {
                "PatientID": caseInfo.ID,
                "PinnedQuestList": reqeustData,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (rsp) => {
                alert("儲存成功!");
                setCurrentMode(HomeMode.ViewMode);
                // 重整關注項目
                getPinnedQuest();
            }
        })
    }

    // 切換關注項目顯示
    const handleQuestVisibleChange = (e, i) => {
        const checked = e.target.checked;
        setPersonalPinQuest(prev => {
            return produce(prev, draft => {
                draft.data[i].Visible = checked;
            });
        });
    }

    return (
        <div className="QForm-Wrap" id="QFormxContainer" name="QFormContainer">
            <span id="Status" hidden>關注項目</span>
            <div id="HistoryForm" className="Q-History ui-historyWrap ui-Col-100 cfg-historyListView">
                <div className="row">
                    <div className="col-md-3">
                        <Schedule />
                    </div>
                    <div className="col-md-9">
                        <div hidden={!isViewMode()}>
                            <div className="pinned-quest-toolbar">
                                <h4>關注項目</h4>
                                <div>
                                    <button type="button" className="btn btn-v2" onClick={() => setCurrentMode(HomeMode.EditMode)}>編輯病患關注</button>
                                </div>
                            </div>
                            <LoadingComponent status={pinnedQuestDataList.status}>
                                <div>
                                    {CreatePinGroup()}
                                </div>
                            </LoadingComponent>
                        </div>

                        <div hidden={isViewMode()}>
                            <div className="pinned-quest-toolbar">
                                <h4>關注項目</h4>
                                <div>
                                    <div>
                                        <button type="button" className="btn btn-v2" style={{ marginRight: "5px" }} onClick={savePinQuest}>完成編輯</button>                            
                                        <button type="button" className="btn btn-v2" style={{ marginRight: "5px" }} onClick={resetPersonalPinQuest}>重置</button>                            
                                        <button type="button" className="btn btn-v2" onClick={() => setCurrentMode(HomeMode.ViewMode)}>取消</button>                            
                                    </div>
                                </div>
                            </div>
                            <div>
                                <table className="table">
                                    <thead>
                                        <tr>
                                            <th>名稱</th>
                                            <th style={{ width: "50px" }}>顯示</th>
                                            <th style={{ width: "105px" }}>排序</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {personalPinQuest.data.map((q, i) => (
                                            <tr>
                                                <td>{q.PinnedName}</td>
                                                <td>
                                                    <div className="checkbox">
                                                        <label>
                                                            <input type="checkbox" checked={q.Visible} onChange={e => handleQuestVisibleChange(e, i)} />
                                                        </label>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <button type="button" className="btn btn-default" style={{ marginRight: "5px" }} onClick={e => moveUp(e, i)}>&#9650;</button>
                                                        <button type="button" className="btn btn-default" onClick={e => moveDown(e, i)}>&#9660;</button>
                                                    </div>
                                                </td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>                            
                        </div>                        
                    </div>
                </div>
            </div>
        </div>
    );
};

const Home = (props) => {
    return (
        <>
            <WorkSpaceMain window={<HomeWindow />} />
        </>
    );
};

export default Home;