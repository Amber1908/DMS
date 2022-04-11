CREATE TABLE [dbo].[FileGroup] (
    [ID]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [FloderKey]  CHAR (32)     NOT NULL,
    [FGState]    CHAR (1)      NOT NULL,
    [ManType]    CHAR (1)      NOT NULL,
    [CreateMan]  VARCHAR (20)  NOT NULL,
    [CreateName] NVARCHAR (20) NOT NULL,
    [CreateDate] DATETIME      CONSTRAINT [DF_FileGroup_CreateDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_FileGroup] PRIMARY KEY CLUSTERED ([ID] ASC)
);

