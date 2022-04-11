CREATE TABLE [dbo].[Schedule] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [PatientID]   INT            NOT NULL,
    [ContentText] NVARCHAR (200) NOT NULL,
    [ReturnDate]  DATETIME2 (7)  NOT NULL,
    [CreateDate]  DATETIME2 (7)  CONSTRAINT [DF_Schedule_CreateDate] DEFAULT (getdate()) NOT NULL,
    [CreateMan]   VARCHAR (254)  NOT NULL,
    [ModifyDate]  DATETIME2 (7)  CONSTRAINT [DF_Schedule_ModifyDate] DEFAULT (getdate()) NOT NULL,
    [ModifyMan]   VARCHAR (254)  NOT NULL,
    [DeleteDate]  DATETIME2 (7)  NULL,
    [DeleteMan]   VARCHAR (254)  NULL,
    [IsDelete]    BIT            CONSTRAINT [DF_Schedule_IsDelete] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Schedule] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Schedule_X1_PatientInfo] FOREIGN KEY ([PatientID]) REFERENCES [dbo].[X1_PatientInfo] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'回診時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Schedule', @level2type = N'COLUMN', @level2name = N'ReturnDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'個案ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Schedule', @level2type = N'COLUMN', @level2name = N'PatientID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Schedule', @level2type = N'COLUMN', @level2name = N'ModifyMan';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Schedule', @level2type = N'COLUMN', @level2name = N'ModifyDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否刪除', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Schedule', @level2type = N'COLUMN', @level2name = N'IsDelete';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'刪除人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Schedule', @level2type = N'COLUMN', @level2name = N'DeleteMan';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'刪除日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Schedule', @level2type = N'COLUMN', @level2name = N'DeleteDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Schedule', @level2type = N'COLUMN', @level2name = N'CreateMan';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Schedule', @level2type = N'COLUMN', @level2name = N'CreateDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'回診內容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Schedule', @level2type = N'COLUMN', @level2name = N'ContentText';

