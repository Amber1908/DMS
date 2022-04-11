import $ from 'jquery';
import { GlobalConstants } from './CommonUtility';
import Log from './LogUtility';

const Ajax = (() => {
    const EmptyFunction = function () { };
    let baseUrlInput = $("#BaseURL").val();
    const BaseURL = baseUrlInput.substring(0, baseUrlInput.length - 1);
    const webapiBaseURL = BaseURL + "/api";

    const PostBasic = ({ url, data, success = EmptyFunction, statusError, error = EmptyFunction, async = true, cache = true, contentType = "application/x-www-form-urlencoded; charset=UTF-8", processData = true, final = EmptyFunction, method = "POST", headers = {} }) => {
        const ajaxRequest = (resolve, reject) => {
            let request = {
                async: async,
                url: webapiBaseURL + url,
                cache: cache,
                contentType: contentType,
                processData: processData,
                method: method,
                data: data,
                headers: headers,
                success: function (response) {
                    Log.Debug(`Request "${webapiBaseURL + url}" Recieve:`);
                    Log.Debug(response);

                    if (response.ReturnCode == GlobalConstants.WebApiStatus.OK) {
                        success(response);
                        resolve(response);
                    } else {
                        // 沒有傳 statusError function 預設顯示錯誤訊息
                        if (statusError == null) {
                            let message = `${response.ReturnMsg}(錯誤碼:${response.ReturnCode})`;
                            alert(message);
                            reject(message);
                        } else {
                            statusError(response);
                        }
                    }

                    final(response);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    var errorMsg = "Request Error: " +
                        "\ntextStatus: " + textStatus +
                        "\nerrorThrown: " + errorThrown
                        "\njqXHR: " + jqXHR;
                    Log.Debug(errorMsg);
                    Log.Debug(jqXHR);
                    alert(errorMsg);
                    error(jqXHR, textStatus, errorThrown);
                    final();
                }
            };

            Log.Debug("Request:");
            Log.Debug(request, true);

            $.ajax(request);
        }

        return new Promise(ajaxRequest);
    };

    return { BaseURL, webapiBaseURL, PostBasic };
})();

const DynamicContent = (content) => ({ Data: content, Status: GlobalConstants.Status.INIT });

const SetInitStatus = (setFunc) => {
    setFunc(prev => ({ ...prev, Status: GlobalConstants.Status.INIT }));
};

const SetLoadingStatus = (setFunc) => {
    setFunc(prev => ({ ...prev, Status: GlobalConstants.Status.LOADING }));
}

const SetContent = (setFunc, content) => {
    setFunc(prev => ({ ...prev, Data: content }));
}

export { Ajax, DynamicContent, SetInitStatus, SetLoadingStatus, SetContent };