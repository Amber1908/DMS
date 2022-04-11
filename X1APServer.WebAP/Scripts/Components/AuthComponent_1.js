import React from 'react';
import PropType from 'prop-types';
import { useAuthCheck } from '../CustomHook/CustomHook';

// 判斷權限元件，若無權限回傳null
const AuthComponent = (props) => {
    const { AuthCheck } = useAuthCheck();

    if (AuthCheck({ FuncCode: props.FuncCode, AuthNos: props.AuthNos })) {
        return props.children;
    } else {
        return null;
    }
}

AuthComponent.propTypes = {
    FuncCode: PropType.string.isRequired,
    AuthNos: PropType.array
}



export default AuthComponent;