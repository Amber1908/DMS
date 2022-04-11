import React, { useContext } from 'react';
import { Redirect, Route } from 'react-router-dom';
import PropTypes from 'prop-types';
import { CookieContext } from './Context';
import { IsLoginAndSelectSite } from '../Utilities/TokenUtility';

const PrivateRoute = ({ children, funcCode, authNo, ...rest }) => {
    const { cookies } = useContext(CookieContext);

    return (
        <Route 
            {...rest}
            render={({ location }) =>
                IsLoginAndSelectSite(cookies) ? (
                    children
                ) : (
                    <Redirect to="/" />
                    // children
                )
            }
        />
    )
}

PrivateRoute.propTypes = {
    funcCode: PropTypes.string,
    authNo: PropTypes.arrayOf(PropTypes.string)
}

export default PrivateRoute;