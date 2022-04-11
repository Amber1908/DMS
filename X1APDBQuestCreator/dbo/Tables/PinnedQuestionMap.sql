CREATE TABLE [dbo].[PinnedQuestionMap]
(
	[PinnedID] INT NOT NULL, 
    [QuestionID] INT NOT NULL, 
    CONSTRAINT [PK_PinnedQuestionMap] PRIMARY KEY ([PinnedID], [QuestionID]), 
    CONSTRAINT [FK_PinnedQuestionMap_ToPinned_Question] FOREIGN KEY ([PinnedID]) REFERENCES [Pinned_Question]([PinnedID]) ON DELETE CASCADE ON UPDATE CASCADE, 
    CONSTRAINT [FK_PinnedQuestionMap_ToX1_Report_Question] FOREIGN KEY ([QuestionID]) REFERENCES [X1_Report_Question]([ID]) ON DELETE CASCADE ON UPDATE CASCADE
)
