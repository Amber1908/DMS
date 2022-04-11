CREATE TABLE [dbo].[X1_Report_Answer_Main] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [MainID]      INT           NULL,
    [ReportID]    INT           NOT NULL,
    [PID]         INT           NOT NULL,
    [OID]         INT           NULL,
    [FillingDate] DATETIME2 (7) CONSTRAINT [DF_X1_Report_Answer_Main_FillingDate] DEFAULT (getdate()) NOT NULL,
    [CreateDate]  DATETIME      NOT NULL,
    [CreateMan]   VARCHAR (254) NOT NULL,
    [ModifyDate]  DATETIME      NOT NULL,
    [ModifyMan]   VARCHAR (254) NOT NULL,
    [DeleteDate]  DATETIME      NULL,
    [DeleteMan]   VARCHAR (254) NULL,
    [IsDelete]    BIT           NOT NULL,
    [Lock]        BIT           CONSTRAINT [DF_X1_Report_Answer_Main_Lock] DEFAULT ((0)) NOT NULL,
    [Status]      INT           CONSTRAINT [DF_X1_Report_Answer_Main_Status] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_X1_Report] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_X1_Report_Answer_Main_X1_PatientInfo] FOREIGN KEY ([PID]) REFERENCES [dbo].[X1_PatientInfo] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_X1_Report_Answer_Main_X1_Report_Main] FOREIGN KEY ([ReportID]) REFERENCES [dbo].[X1_Report_Main] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_X1_Report]
    ON [dbo].[X1_Report_Answer_Main]([MainID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_X1_Report_Answer_Main]
    ON [dbo].[X1_Report_Answer_Main]([ReportID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_X1_Report_Answer_Main_1]
    ON [dbo].[X1_Report_Answer_Main]([OID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_X1_Report_Answer_Main_2]
    ON [dbo].[X1_Report_Answer_Main]([PID] ASC);

