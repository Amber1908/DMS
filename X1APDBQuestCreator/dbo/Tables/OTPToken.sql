CREATE TABLE [dbo].[OTPToken] (
    [ID]          INT       IDENTITY (1, 1) NOT NULL,
    [UserID]      INT       NOT NULL,
    [ReqSysCode]  CHAR (6)  NOT NULL,
    [RspSysCode]  CHAR (6)  NOT NULL,
    [OTPTokenKey] CHAR (32) NOT NULL,
    [OTPStatus]   CHAR (1)  NOT NULL,
    [RegTime]     DATETIME  CONSTRAINT [DF_OTPToken_RegTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_OTPToken] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_OTPToken_TokenInfo]
    ON [dbo].[OTPToken]([UserID] ASC, [ReqSysCode] ASC, [RspSysCode] ASC, [OTPTokenKey] ASC);

