CREATE TABLE [dbo].[X1_Report_Authorization] (
    [ReportID]   INT           NOT NULL,
    [PGID]       INT           NOT NULL,
    [CreateDate] DATETIME2 (7) CONSTRAINT [DF_X1_Report_Authorization_CreateDate] DEFAULT (getdate()) NOT NULL,
    [CreateMan]  VARCHAR (254) NOT NULL,
    CONSTRAINT [PK_X1_Report_Authorization_1] PRIMARY KEY CLUSTERED ([ReportID] ASC, [PGID] ASC),
    CONSTRAINT [FK_X1_Report_Authorization_X1_PatientGroup] FOREIGN KEY ([PGID]) REFERENCES [dbo].[X1_PatientGroup] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_X1_Report_Authorization_X1_Report_Main] FOREIGN KEY ([ReportID]) REFERENCES [dbo].[X1_Report_Main] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [NameIndex]
    ON [dbo].[X1_Report_Authorization]([PGID] ASC)
    INCLUDE([ReportID], [CreateDate], [CreateMan]);


GO
CREATE NONCLUSTERED INDEX [ReportIDIndex]
    ON [dbo].[X1_Report_Authorization]([ReportID] ASC)
    INCLUDE([PGID], [CreateDate], [CreateMan]);

