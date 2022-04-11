CREATE TABLE [dbo].[ExportTemplateExtraQuest] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [ExportTemplateID] INT           NOT NULL,
    [QuestName]        VARCHAR (50)  CONSTRAINT [DF_ExportTemplateExtraQuest_QuestName] DEFAULT ('') NOT NULL,
    [QuestText]        NVARCHAR (50) NOT NULL,
    [QuestType]        VARCHAR (50)  CONSTRAINT [DF_ExportTemplateExtraQuest_QuestType] DEFAULT ('text') NOT NULL,
    [Required]         BIT           CONSTRAINT [DF_ExportTemplateExtraQuest_Required] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ExportTemplateExtraQuest] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ExportTemplateExtraQuest_X1_Report_Export_Template] FOREIGN KEY ([ExportTemplateID]) REFERENCES [dbo].[X1_Report_Export_Template] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [ExportTemplateIDIndex]
    ON [dbo].[ExportTemplateExtraQuest]([ExportTemplateID] ASC)
    INCLUDE([ID], [QuestText], [QuestType], [Required]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否必填', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ExportTemplateExtraQuest', @level2type = N'COLUMN', @level2name = N'Required';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'問題類型
html input type能接受的類型: text、number…', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ExportTemplateExtraQuest', @level2type = N'COLUMN', @level2name = N'QuestType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'問題顯示文字
顯示在網頁上的問題標題', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ExportTemplateExtraQuest', @level2type = N'COLUMN', @level2name = N'QuestText';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'問題名稱
用於對應報告模板中的問題', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ExportTemplateExtraQuest', @level2type = N'COLUMN', @level2name = N'QuestName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報告模板ID
參考X1_Report_Export_Template', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ExportTemplateExtraQuest', @level2type = N'COLUMN', @level2name = N'ExportTemplateID';

