import React from 'react';

const DeleteButton = (props) => {
    return <button className="delete-button close fa fa-times" type="button" onClick={props.handleOnClick}></button>;
}

export default DeleteButton;