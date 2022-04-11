CREATE TABLE [dbo].[SysInfo] (
    [SysCode]    CHAR (6)      NOT NULL,
    [SysName]    NVARCHAR (30) NOT NULL,
    [SysStatus]  CHAR (1)      CONSTRAINT [DF_SysInfo_SysStatus] DEFAULT ('Y') NOT NULL,
    [CreateMan]  INT           NOT NULL,
    [CreateDate] DATETIME      CONSTRAINT [DF_SysInfo_CreateDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_SysInfo_1] PRIMARY KEY CLUSTERED ([SysCode] ASC)
);

