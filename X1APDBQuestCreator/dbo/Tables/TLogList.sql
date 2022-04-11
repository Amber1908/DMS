CREATE TABLE [dbo].[TLogList] (
    [ID]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [MainID]     INT           NOT NULL,
    [ActionType] CHAR (1)      NOT NULL,
    [FGID]       BIGINT        NULL,
    [DetailFlag] CHAR (1)      CONSTRAINT [DF_TLogList_DetailFlag] DEFAULT ('Y') NOT NULL,
    [CreateMan]  VARCHAR (20)  NOT NULL,
    [CreateName] NVARCHAR (20) NOT NULL,
    [CreateDate] DATETIME      CONSTRAINT [DF_TLogList_CreateDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_TLogList] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TLogList_MainID]
    ON [dbo].[TLogList]([MainID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TLogList_FGID]
    ON [dbo].[TLogList]([FGID] ASC);

