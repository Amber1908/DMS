CREATE TABLE [dbo].[Personal_Pinned_Question] (
    [PatientID]  INT NOT NULL,
    [PinnedID] INT NOT NULL,
    [Visible]    BIT CONSTRAINT [DF_Personal_Pinned_Question_Visible] DEFAULT ((1)) NOT NULL,
    [Index]      INT NOT NULL,
    CONSTRAINT [FK_Personal_Pinned_Question_X1_PatientInfo] FOREIGN KEY ([PatientID]) REFERENCES [dbo].[X1_PatientInfo] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Personal_Pinned_Question_Pinned_Question] FOREIGN KEY ([PinnedID]) REFERENCES [dbo].[Pinned_Question] ([PinnedID]) ON DELETE CASCADE ON UPDATE CASCADE, 
    CONSTRAINT [PK_Personal_Pinned_Question] PRIMARY KEY ([PatientID], [PinnedID])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否顯示', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Personal_Pinned_Question', @level2type = N'COLUMN', @level2name = N'Visible';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'問題ID 參考 X1_Report_Question', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Personal_Pinned_Question', @level2type = N'COLUMN', @level2name = 'PinnedID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'個案ID 參考X1_PatientInfo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Personal_Pinned_Question', @level2type = N'COLUMN', @level2name = N'PatientID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序索引', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Personal_Pinned_Question', @level2type = N'COLUMN', @level2name = N'Index';

