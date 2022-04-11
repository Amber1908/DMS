import React from 'react';
import PropType from 'prop-types';

const UserListTab = (props) => {
    let selected = props.selectedTab == props.id ? "Y" : "";

    return (
        <div onClick={props.handleOnClick} id={props.id} className={`ui-Tab X ${selected} user-list-tab`}>
            {props.tabName}
        </div>
    );
}

UserListTab.propTypes = {
    selectedTab: PropType.string,
    handleOnClick: PropType.func.isRequired,
    id: PropType.string.isRequired,
    tabName: PropType.string.isRequired
};

export default UserListTab;