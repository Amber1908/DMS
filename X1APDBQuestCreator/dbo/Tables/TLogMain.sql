CREATE TABLE [dbo].[TLogMain] (
    [ID]         INT           IDENTITY (1, 1) NOT NULL,
    [SysCode]    CHAR (6)      NOT NULL,
    [LogType]    CHAR (3)      NOT NULL,
    [LogKey]     VARCHAR (20)  NOT NULL,
    [CreateMan]  VARCHAR (20)  NOT NULL,
    [CreateName] NVARCHAR (20) NOT NULL,
    [CreateDate] DATETIME      CONSTRAINT [DF_TLogMain_CreateDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_TLogMain] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_TLogMain_Unique]
    ON [dbo].[TLogMain]([SysCode] ASC, [LogType] ASC, [LogKey] ASC);

