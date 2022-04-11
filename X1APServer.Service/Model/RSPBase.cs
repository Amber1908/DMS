using System;

namespace X1APServer.Service.Model
{
    [Serializable]
    public class RSPBase
    {
        /// <summary>
        /// 回傳狀態
        /// </summary>
        public ErrorCode ReturnCode { get; set; } = ErrorCode.None;
        /// <summary>
        /// 回傳訊息
        /// </summary>
        public string ReturnMsg { get; set; } = "系統執行時發生錯誤!";
        /// <summary>
        /// Token是否已被更新
        /// </summary>
        public bool TokenChgFlag { get; set; } = false;
        /// <summary>
        /// Token更新後內容
        /// </summary>
        public string UserSecurityInfo { get; set; }
    }

    public enum ErrorCode
    {
        /// <summary>
        /// 空值
        /// </summary>
        None = -1,
        /// <summary>
        /// 正常(X1)
        /// </summary>
        OK = 0,
        /// <summary>
        /// 無資料
        /// </summary>
        NotFound = 100,
        /// <summary>
        /// 處理錯誤
        /// </summary>
        OperateError = 110,
        /// <summary>
        /// 已存在
        /// </summary>
        Exist = 120,
        /// <summary>
        /// iDoctor 超時
        /// </summary>
        Timeout = 130,
        /// <summary>
        /// iDoctor Exception
        /// </summary>
        Exception = 999,
        /// <summary>
        /// 輸入參數格式錯誤(X1)
        /// </summary>
        ArgInvalid = 1000,
        /// <summary>
        /// 權限錯誤(X1)
        /// </summary>
        AuthError = 2000,
        /// <summary>
        /// DB無Token資料
        /// </summary>
        NoTokenData = 2010,
        /// <summary>
        /// Access Token錯誤
        /// </summary>
        AccessTokenInvalid = 2020,
        /// <summary>
        /// Refresh Token錯誤
        /// </summary>
        RefreshTokenInvalid = 2030,
        /// <summary>
        /// Refresh Token過期(X1)
        /// </summary>
        RefreshTokenExpired = 2040,
        /// <summary>
        /// 使用者帳號跟SecurityInfo不符
        /// </summary>
        AccIDInvalid = 2130,
        /// <summary>
        /// Session Key錯誤
        /// </summary>
        SessionKeyInvalid = 2200,
        /// <summary>
        /// 資料驗證處理錯誤(X1)
        /// </summary>
        ProcessError = 3000,
        /// <summary>
        /// 表單狀態錯誤
        /// </summary>
        StatusError = 3010,
        /// <summary>
        /// 登入錯誤
        /// </summary>
        LoginError = 4000,
        /// <summary>
        /// 登入帳號錯誤(X1)
        /// </summary>
        AccountInvalid = 4100,
        /// <summary>
        /// 登入密碼錯誤(X1)
        /// </summary>
        PasswordInvalid = 4110,
        /// <summary>
        /// 登入帳號被停權(X1)
        /// </summary>
        AccountSuspended = 4120,
        /// <summary>
        /// 登入密碼錯誤次數超過5次
        /// </summary>
        ExceedRetryLimit = 4130,
        /// <summary>
        /// 有同個帳號正在線上
        /// </summary>
        IsOnline = 4140,
        /// <summary>
        /// Email 尚未驗證
        /// </summary>
        EmailNotVerify = 4150,
        /// <summary>
        /// 登入token錯誤
        /// </summary>
        LoginTokenInvalid = 4160,
        /// <summary>
        /// 註冊錯誤
        /// </summary>
        RegisterError = 5000,
        /// <summary>
        /// 註冊帳號重複
        /// </summary>
        AccountDuplicated = 5110,
        /// <summary>
        /// 註冊角色類型錯誤
        /// </summary>
        RoleCodeInvalid = 5120,
        /// <summary>
        /// 伺服器內部錯誤
        /// </summary>
        ServerError = 9999
    }
}