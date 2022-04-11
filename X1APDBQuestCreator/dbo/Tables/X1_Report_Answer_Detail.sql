CREATE TABLE [dbo].[X1_Report_Answer_Detail] (
    [ID]         INT             IDENTITY (1, 1) NOT NULL,
    [AnswerMID]  INT             NOT NULL,
    [QuestionID] INT             NOT NULL,
    [Value]      NVARCHAR (3000) NULL,
    CONSTRAINT [PK_X1_Report_Answer_Detail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_X1_Report_Answer_Detail_X1_Report_Answer_Detail] FOREIGN KEY ([AnswerMID]) REFERENCES [dbo].[X1_Report_Answer_Main] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_X1_Report_Answer_Detail_X1_Report_Question] FOREIGN KEY ([QuestionID]) REFERENCES [dbo].[X1_Report_Question] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_X1_Report_Answer_Detail]
    ON [dbo].[X1_Report_Answer_Detail]([AnswerMID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_X1_Report_Answer_Detail_1]
    ON [dbo].[X1_Report_Answer_Detail]([QuestionID] ASC);

