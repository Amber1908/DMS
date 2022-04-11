CREATE TABLE [dbo].[X1_Report_Answer_File] (
    [ID]         INT              IDENTITY (1, 1) NOT NULL,
    [AnswerMID]  INT              NOT NULL,
    [QuestionID] INT              NOT NULL,
    [GUID]       UNIQUEIDENTIFIER NOT NULL,
    [FileName]   NVARCHAR (100)   NOT NULL,
    [FileExt]    VARCHAR (50)     NOT NULL,
    [FilePath]   NVARCHAR (500)   NOT NULL,
    [MimeType]   VARCHAR (200)    NOT NULL,
    CONSTRAINT [PK_X1_Report_Answer_File] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_X1_Report_Answer_File_X1_Report_Answer_Main] FOREIGN KEY ([AnswerMID]) REFERENCES [dbo].[X1_Report_Answer_Main] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_X1_Report_Answer_File_X1_Report_Question] FOREIGN KEY ([QuestionID]) REFERENCES [dbo].[X1_Report_Question] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_X1_Report_Answer_File]
    ON [dbo].[X1_Report_Answer_File]([AnswerMID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_X1_Report_Answer_File_1]
    ON [dbo].[X1_Report_Answer_File]([QuestionID] ASC);

