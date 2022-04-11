import React from 'react';
import PropTypes from 'prop-types';

const QuestionGroup = (props) => {
    let tool;
    if (props.toolFlag) {
        tool = (
            <>
                <input type="checkbox" className="dock" />
                <div className="form-ico-collapse"></div>
                <input type="checkbox" className="collapse-form" />
                <div className="form-ico-dock"></div>
                <div className="peek">Peek</div>
            </>
        )
    }

    return (
        <div className={`QForm Layer2 ${props.className}`} style={props.style}>
            <div className="QIndex" />
            <h4>{props.title}</h4>
            {tool}
            <p className="description">{props.description}</p>
            <div className="formContent">
                {props.children}
            </div>
        </div>    
    );
};

QuestionGroup.defaultProps = {
    className: "",
    title: "",
    toolFlag: false,
    style: {}
}

QuestionGroup.propTypes = {
    title: PropTypes.string,
    className: PropTypes.string,
    toolFlag: PropTypes.bool,
    style: PropTypes.object
};

export default QuestionGroup;