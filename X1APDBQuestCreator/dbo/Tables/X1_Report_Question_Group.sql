CREATE TABLE [dbo].[X1_Report_Question_Group] (
    [ID]          INT             IDENTITY (1, 1) NOT NULL,
    [ReportID]    INT             NOT NULL,
    [Title]       NVARCHAR (1000) NULL,
    [Description] NVARCHAR (3000) NULL,
    CONSTRAINT [PK_Report_Question_Group] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_X1_Report_Question_Group_X1_Report_Main] FOREIGN KEY ([ReportID]) REFERENCES [dbo].[X1_Report_Main] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Report_Question_Group]
    ON [dbo].[X1_Report_Question_Group]([ReportID] ASC);

