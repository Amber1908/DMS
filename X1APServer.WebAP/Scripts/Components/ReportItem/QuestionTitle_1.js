import React from "react";
import PropTypes from "prop-types";

// 表單問題標題
const QuestionTitle = (props) => {
    let colwidthClass = "";

    if (props.colwidth != null) {
        colwidthClass = `ui-Col-${props.colwidth}`;
    }
    return (
        <div className={`item-wrap edit ${colwidthClass}`}>
            <div className="QForm-Table-Label">{props.title}</div>
        </div>
    );
};

QuestionTitle.propTypes = {
    // 標題文字
    title: PropTypes.string,
    // 元件寬度
    colwidth: PropTypes.number,
};

export default QuestionTitle;
