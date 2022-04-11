CREATE TABLE [dbo].[X1_Report_Main] (
    [ID]          INT             IDENTITY (1, 1) NOT NULL,
    [IndexNum]    INT             CONSTRAINT [DF_X1_Report_Main_IndexNum] DEFAULT ((1)) NOT NULL,
    [Category]    NVARCHAR (50)   NOT NULL,
    [Title]       NVARCHAR (1000) NULL,
    [Description] NVARCHAR (3000) NULL,
    [OutputJson]  NVARCHAR (MAX)  NULL,
    [CreateDate]  DATETIME        NOT NULL,
    [CreateMan]   VARCHAR (254)   NOT NULL,
    [ModifyDate]  DATETIME        NOT NULL,
    [ModifyMan]   VARCHAR (254)   NOT NULL,
    [DeleteDate]  DATETIME        NULL,
    [DeleteMan]   VARCHAR (254)   NULL,
    [IsDelete]    BIT             NOT NULL,
    [PublishDate] DATETIME2 (7)   NULL,
    [IsPublish]   BIT             CONSTRAINT [DF_X1_Report_Main_IsPublish] DEFAULT ('0') NOT NULL,
    [ReserveDate] DATETIME2 (7)   CONSTRAINT [DF_X1_Report_Main_ReserveDate] DEFAULT (getdate()) NOT NULL,
    [FuncCode]    CHAR (6)        NULL,
    CONSTRAINT [PK_ReportM] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_ReportM]
    ON [dbo].[X1_Report_Main]([Category] ASC);

