CREATE TABLE [dbo].[OTPTSysMap] (
    [ReqSysCode] CHAR (6) NOT NULL,
    [RspSysCode] CHAR (6) NOT NULL,
    CONSTRAINT [PK_OTPTSysMap] PRIMARY KEY CLUSTERED ([ReqSysCode] ASC, [RspSysCode] ASC)
);

