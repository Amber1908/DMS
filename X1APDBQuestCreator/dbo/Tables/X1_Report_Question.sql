CREATE TABLE [dbo].[X1_Report_Question] (
    [ID]              INT              IDENTITY (1, 1) NOT NULL,
    [QuestionNo]      NVARCHAR (50)    NULL,
    [QuestionType]    INT              NOT NULL,
    [ReportID]        INT              NOT NULL,
    [QuestionText]    NVARCHAR (1000)  NULL,
    [Description]     NVARCHAR (3000)  NULL,
    [Required]        BIT              NOT NULL,
    [CodingBookTitle] NVARCHAR (50)    NULL,
    [CodingBookIndex] INT              NOT NULL,
    [AnswerOption]    NVARCHAR (4000)  NULL,
    [OtherAnsID]      INT              NULL,
    [Pin]             BIT              CONSTRAINT [DF_X1_Report_Question_Follow] DEFAULT ((0)) NOT NULL,
    [ParentQuestID]   INT              NULL,
    [Guid]            UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_X1_Report_Question] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_X1_Report_Question_X1_Report_Main] FOREIGN KEY ([ReportID]) REFERENCES [dbo].[X1_Report_Main] ([ID]),
    CONSTRAINT [FK_X1_Report_Question_X1_Report_Question_Type] FOREIGN KEY ([QuestionType]) REFERENCES [dbo].[X1_Report_Question_Type] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_X1_Report_Question]
    ON [dbo].[X1_Report_Question]([QuestionType] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_X1_Report_Question_1]
    ON [dbo].[X1_Report_Question]([ReportID] ASC);


GO
CREATE NONCLUSTERED INDEX [QuestionNoIndex]
    ON [dbo].[X1_Report_Question]([QuestionNo] ASC)
    INCLUDE([ID], [QuestionType], [ReportID], [QuestionText], [Description], [Required], [CodingBookTitle], [CodingBookIndex], [AnswerOption]);

