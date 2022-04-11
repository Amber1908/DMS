import moment from "moment";

export const GlobalConstants = {
    FuncCode: {
        ViewWebsite: "FN0101",
        Backend: "FN0102",
        AddPatient: "FN0004"
    },
    ValidationRules: {
        PassWord: {
            required: true,
            minLength: 4,
            maxLength: 16,
            title: "請輸入超過4個字元",
            type: "password"
        },
        AccName: {
            required: true,
            maxLength: 30
        },
        AccTitle: {
            maxLength: 20
        },
        CellPhone: {
            type: "tel",
            maxLength: 20
        },
        Email: {
            type: "email",
            maxLength: 50
        },
        AccID: {
            required: true,
            maxLength: 20
        }
    },
    CookieName: "SecurityInfo",
    WebApiStatus: {
        OK: 0,
        AuthError: 2000
    },
    QuestionPreFix: "Q_",
    Status: {
        BEFORE_INIT: -1,
        INIT: 0,
        LOADING: 1
    }
};

export const GetDate = (datetime, defaultVal = "", splitter = "/") => {
    let date = new Date(datetime);
    const month = PadLeft(date.getMonth() + 1, 2, "0"),
        dateF = PadLeft(date.getDate(), 2, "0");
    const dateStr = `${date.getFullYear()}${splitter}${month}${splitter}${dateF}`;

    if (isNaN(date.getTime()) || datetime == null) {
        return defaultVal;
    } else {
        return dateStr;
    }
};

export const GetROC = (date, formatStr) => {
    let dateObj = moment(date);
    let rsp = "";
    if (dateObj.isValid()) {
        let roc = dateObj.year() - 1911;
        rsp = dateObj.format(formatStr);
        rsp = rsp.replace("ROC", roc);
    }

    return rsp;
}

export const GetDateTime = (datetime) => {
    let date = new Date(datetime);
    const month = PadLeft(date.getMonth() + 1, 2, "0"),
        dateF = PadLeft(date.getDate(), 2, "0"),
        hour = PadLeft(date.getHours(), 2, "0"),
        minute = PadLeft(date.getMinutes(), 2, "0");
    const dateStr = `${date.getFullYear()}/${month}/${dateF} ${hour}:${minute}`;

    if (isNaN(date.getTime()) || datetime == null) {
        return "";
    } else {
        return dateStr;
    }
}

export const GetStatus = (status) => {
    let retStr = "待檢驗";
    switch (status) {
        case 2:
            retStr = "檢驗中";
            break;
        case 3:
            retStr = "待覆核";
            break;
        case 4:
            retStr = "覆核中";
            break;
        case 5:
            retStr = "已覆核";
            break;
        case 6:
            retStr = "已結案";
            break;
        case 7:
            retStr = "已匯出";
            break;
    }
    return retStr;
}

export const GetStatusColor = (status) => {
    let retStr = "text-secondary";
    switch (status) {
        case 2:
            retStr = "text-danger";
            break;
        case 3:
            retStr = "text-primary";
            break;
        case 4:
            retStr = "text-danger";
            break;
        case 5:
            retStr = "text-primary";
            break;
        case 6:
            retStr = "text-success";
            break;
        case 7:
            retStr = "text-info";
            break;
    }
    return retStr;
}

export const PadLeft = (str, length, char) => {
    let output = str.toString();
    while (output.length < length) {
        output = char + output;
    }

    return output;
}

export const NullIfEmpty = (data) => {
    let isEmpty = data == null ||
        data === "" ||
        (Array.isArray(data) && data.length === 0);
    return isEmpty ? null : data;
}
