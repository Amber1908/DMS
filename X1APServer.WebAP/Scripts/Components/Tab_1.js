import React from 'react';
import PropType from 'prop-types';
import { NavLink } from "react-router-dom";


const Tab = (props) => {
    const getDisabledClass = (disabled) => disabled ? "disabled" : "";

    return (
        <NavLink activeClassName="Y" to={props.link} hidden={props.hidden} className="ui-Tab X" id={props.id}>
            {props.tabName}
            <div className="ui-TabTip">{props.tabToolTip}</div>
        </NavLink>
    );
};

Tab.defaultProps = {
    hidden: false,
    handleTabClick: () => {},
    tabToolTip: ""
};

Tab.propTypes = {
    link: PropType.string.isRequired,
    hidden: PropType.bool,
    id: PropType.string.isRequired,
    tabName: PropType.string,
    tabToolTip: PropType.string
};

export default Tab;