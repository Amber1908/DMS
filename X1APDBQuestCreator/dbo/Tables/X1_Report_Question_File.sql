CREATE TABLE [dbo].[X1_Report_Question_File] (
    [ID]       INT             IDENTITY (1, 1) NOT NULL,
    [RMID]     INT             NOT NULL,
    [RQID]     INT             NOT NULL,
    [FileData] VARBINARY (MAX) NOT NULL,
    [FileName] NVARCHAR (200)  NOT NULL,
    [MimeType] VARCHAR (100)   NOT NULL,
    CONSTRAINT [PK_X1_Report_Quest_File] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_X1_Report_Question_File_X1_Report_Main] FOREIGN KEY ([RMID]) REFERENCES [dbo].[X1_Report_Main] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_X1_Report_Question_File_X1_Report_Question] FOREIGN KEY ([RQID]) REFERENCES [dbo].[X1_Report_Question] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [FileNameIndex]
    ON [dbo].[X1_Report_Question_File]([FileName] ASC)
    INCLUDE([ID], [RQID], [FileData], [MimeType]);


GO
CREATE NONCLUSTERED INDEX [RMIDIndex]
    ON [dbo].[X1_Report_Question_File]([RMID] ASC)
    INCLUDE([ID], [RQID], [FileData], [FileName], [MimeType]);

