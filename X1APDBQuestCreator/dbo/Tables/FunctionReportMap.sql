CREATE TABLE [dbo].[FunctionReportMap] (
    [ReportCategory] VARCHAR (50) NOT NULL,
    [FuncCode]       CHAR (6)     NOT NULL,
    CONSTRAINT [PK_FunctionReportMap] PRIMARY KEY CLUSTERED ([ReportCategory] ASC, [FuncCode] ASC)
);

