CREATE TABLE [dbo].[UserGroup] (
    [GroupCode]  CHAR (6)      NOT NULL,
    [GroupName]  NVARCHAR (20) NOT NULL,
    [State]      CHAR (1)      CONSTRAINT [DF_UserGroup_State] DEFAULT ('Y') NOT NULL,
    [CreateDate] DATETIME      CONSTRAINT [DF_UserGroup_CreateDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_UserGroup] PRIMARY KEY CLUSTERED ([GroupCode] ASC)
);

