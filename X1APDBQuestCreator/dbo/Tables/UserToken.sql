CREATE TABLE [dbo].[UserToken] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [UserID]       INT           NOT NULL,
    [SysCode]      CHAR (6)      NOT NULL,
    [AccessToken]  CHAR (32)     NOT NULL,
    [RefreshToken] CHAR (32)     NOT NULL,
    [TokenTime]    DATETIME2 (7) NOT NULL,
    [CreateDate]   DATETIME      CONSTRAINT [DF_UserToken_CreateDate] DEFAULT (getdate()) NOT NULL,
    [ModifyDate]   DATETIME      CONSTRAINT [DF_UserToken_ModifyDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_UserToken] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserToken_UserID]
    ON [dbo].[UserToken]([UserID] ASC, [SysCode] ASC);

