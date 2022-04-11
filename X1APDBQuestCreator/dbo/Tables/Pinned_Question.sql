CREATE TABLE [dbo].[Pinned_Question] (
    [PinnedID] INT NOT NULL IDENTITY,
    [PinnedName] nvarchar(20) NOT NULL,
    [Visible]    BIT CONSTRAINT [DF_Pinned_Question_Visible] DEFAULT ((1)) NOT NULL,
    [Index]      INT NOT NULL,
    CONSTRAINT [PK_Pinned_Question] PRIMARY KEY ([PinnedID])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否顯示', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pinned_Question', @level2type = N'COLUMN', @level2name = N'Visible';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序索引', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pinned_Question', @level2type = N'COLUMN', @level2name = N'Index';

