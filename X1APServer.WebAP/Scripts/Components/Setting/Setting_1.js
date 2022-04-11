import React from 'react';
import RoleSetting from './RoleSetting';

const Setting = (props) => {
    return (
        <div {...props}>
            <input type="checkbox" className="ui-Setting-Btn" />
            <div className="ui-Setting-Modal">
                <div className="ui-Setting">
                    {/* <div style={{ display: 'inline-block', position: 'relative', float: 'left', width: '100%' }}>
                            <input type="text" style={{ width: '19%', marginBottom: '20px', borderRadius: '20px' }} placeholder="Search Settings" />
                        </div> */}
                    <input className="ui-Setting-Radio" type="radio" name="settingOption" defaultChecked="checked" />
                    <span className="ui-Setting-Label">角色管理</span>
                    {/* <input className="ui-Setting-Radio" type="radio" name="settingOption" />
                    <span className="ui-Setting-Label">病患群組</span> */}
                    {/* <input className="ui-Setting-Radio" type="radio" name="settingOption" />
                    <span className="ui-Setting-Label">Interface</span>
                    <input className="ui-Setting-Radio" type="radio" name="settingOption" />
                    <span className="ui-Setting-Label">Account</span>
                    <input className="ui-Setting-Radio" type="radio" name="settingOption" />
                    <span className="ui-Setting-Label">Networking</span>
                    <input className="ui-Setting-Radio" type="radio" name="settingOption" />
                    <span className="ui-Setting-Label">All</span> */}
                    <RoleSetting />
                    {/* <PatientGroupSetting /> */}
                </div>
            </div>
        </div>
    );
}

export default Setting;