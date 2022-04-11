CREATE TABLE [dbo].[X1_Report_Export_Template] (
    [ID]           INT              IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50)    NOT NULL,
    [SystemFileID] UNIQUEIDENTIFIER NOT NULL,
    [CreateDate]   DATETIME2 (7)    CONSTRAINT [DF_X1_Report_Export_Template_CreateDate] DEFAULT (getdate()) NOT NULL,
    [CreateMan]    VARCHAR (254)    NOT NULL,
    [ModifyDate]   DATETIME2 (7)    CONSTRAINT [DF_X1_Report_Export_Template_ModifyDate] DEFAULT (getdate()) NOT NULL,
    [ModifyMan]    VARCHAR (254)    NOT NULL,
    [DeleteDate]   DATETIME2 (7)    NULL,
    [DeleteMan]    VARCHAR (254)    NULL,
    [IsDelete]     BIT              CONSTRAINT [DF_X1_Report_Export_Template_IsDelete] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_X1_Report_Export_Template] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_X1_Report_Export_Template_SystemFile] FOREIGN KEY ([SystemFileID]) REFERENCES [dbo].[SystemFile] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);



