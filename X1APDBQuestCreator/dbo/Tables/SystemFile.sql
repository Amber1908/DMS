CREATE TABLE [dbo].[SystemFile] (
    [ID]         UNIQUEIDENTIFIER NOT NULL,
    [FilePath]   VARCHAR (100)    NOT NULL,
    [FileName]   NVARCHAR (100)   NOT NULL,
    [MimeType]   VARCHAR (100)    NOT NULL,
    [CreateDate] DATETIME2 (7)    CONSTRAINT [DF_SystemFile_CreateDate] DEFAULT (getdate()) NOT NULL,
    [CreateMan]  VARCHAR (254)    NOT NULL,
    [ModifyDate] DATETIME2 (7)    CONSTRAINT [DF_SystemFile_ModifyDate] DEFAULT (getdate()) NOT NULL,
    [ModifyMan]  VARCHAR (254)    NOT NULL,
    [DeleteDate] DATETIME2 (7)    NULL,
    [DeleteMan]  VARCHAR (254)    NULL,
    [IsDelete]   BIT              CONSTRAINT [DF_SystemFile_IsDelete] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SystemFile] PRIMARY KEY CLUSTERED ([ID] ASC)
);



