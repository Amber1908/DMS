import produce from 'immer';
import React, { useContext, useEffect, useState } from 'react';
import { usePostAuth } from '../../../CustomHook/CustomHook';
import { GlobalConstants } from '../../../Utilities/CommonUtility';
import { CookieContext } from '../../Context';

const ManagePinedQuestModal = (props) => {
    const [pinQuestList, setPinQuestList] = useState({
        data: []
    });

    const { cookies } = useContext(CookieContext);
    const { PostWithAuth } = usePostAuth();

    useEffect(() => {
        $("#editPinQuestModal").on('show.bs.modal', init);
        return () => {
            $("#editPinQuestModal").off('show.bs.modal', init);
        }
    }, [cookies[GlobalConstants.CookieName]])

    const init = () => {
        let usercookie = cookies[GlobalConstants.CookieName];
        if (usercookie != null && usercookie.AccID != null && usercookie.AccID !== "") {
            reqGetPinnedQuestList();
        }
    }

    const reqGetPinnedQuestList = () => {
        PostWithAuth({
            url: "/Report/GetPinnedQuestList",
            data: {
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (rsp) => {
                setPinQuestList({ data: rsp.PinnedQuestList });
            }
        })
    }

    const rowUp = (e, i) => {
        setPinQuestList(prev => {
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

    const rowDown = (e, i) => {
        setPinQuestList(prev => {
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

    const savePinQuest = () => {
        const reqeustData = pinQuestList.data.map((q, i) => ({ ...q, Index: i }));
        PostWithAuth({
            url: "/Report/UpdatePinnedQuest",
            data: {
                "PinnedQuestList": reqeustData,
                "FuncCode": GlobalConstants.FuncCode.ViewWebsite,
                "AuthCode": 1
            },
            success: (rsp) => {
                alert("儲存成功!");
            }
        })
    }

    const resetPinQuest = () => {
        reqGetPinnedQuestList();
    }

    const handleInputTextChange = (e, i) => {
        const target = e.target;
        setPinQuestList(prev => {
            return produce(prev, draft => {
                draft.data[i].PinnedName = target.value;
            });
        });
    }

    const handleInputChange = (e, i) => {
        const target = e.target;
        setPinQuestList(prev => {
            return produce(prev, draft => {
                draft.data[i].Visible = target.checked;
            });
        });
    }

    return (
        <div id="editPinQuestModal" className="modal fade" tabIndex={-1} role="dialog">
            <div className="modal-dialog modal-lg" role="document">
                <div className="modal-content">
                    <div className="modal-header">
                        <button type="button" className="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                        <h4 className="modal-title">編輯關注項目</h4>
                    </div>
                    <div className="modal-body">
                        <table className="table">
                            <thead>
                                <tr>
                                    <th>名稱</th>
                                    <th style={{ width: "50px" }}>顯示</th>
                                    <th style={{ width: "105px" }}>排序</th>
                                </tr>
                            </thead>
                            <tbody>
                                {pinQuestList.data.map((q, i) => (
                                    <tr>
                                        <td><input type="text" class="form-control" value={q.PinnedName} onChange={e => handleInputTextChange(e, i)} /></td>
                                        <td>
                                            <div className="checkbox">
                                                <label>
                                                    <input type="checkbox" checked={q.Visible} onChange={e => handleInputChange(e, i)} />
                                                </label>
                                            </div>
                                        </td>
                                        <td>
                                            <div>
                                                <button type="button" className="btn btn-default" style={{ marginRight: "5px" }} onClick={e => rowUp(e, i)}>&#9650;</button>
                                                <button type="button" className="btn btn-default" onClick={e => rowDown(e, i)}>&#9660;</button>
                                            </div>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                        <div style={{ display: "flex", justifyContent: "flex-end", margin: "10px" }}>
                            <button type="button" className="btn btn-primary" onClick={savePinQuest} style={{ marginRight: "5px" }}>儲存</button>
                            <button type="button" className="btn btn-default" onClick={resetPinQuest}>復原</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default ManagePinedQuestModal;