CREATE TABLE [dbo].[FuncGroup] (
    [SysCode]    CHAR (6)      NOT NULL,
    [GroupCode]  CHAR (3)      NOT NULL,
    [GroupType]  CHAR (1)      NOT NULL,
    [GroupName]  NVARCHAR (20) NOT NULL,
    [GroupState] CHAR (1)      CONSTRAINT [DF_FuncGroup_GroupState] DEFAULT ('Y') NOT NULL,
    [CreateMan]  INT           NOT NULL,
    [CreateDate] DATETIME      CONSTRAINT [DF_FuncGroup_CreateDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_FuncGroup] PRIMARY KEY CLUSTERED ([SysCode] ASC, [GroupCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'資料新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FuncGroup', @level2type = N'COLUMN', @level2name = N'CreateDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FuncGroup', @level2type = N'COLUMN', @level2name = N'CreateMan';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'群組狀態
‘Y’：有效
‘N’：停用
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FuncGroup', @level2type = N'COLUMN', @level2name = N'GroupState';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'群組名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FuncGroup', @level2type = N'COLUMN', @level2name = N'GroupName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'群組類別
‘F’：前台
‘B’：後台
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FuncGroup', @level2type = N'COLUMN', @level2name = N'GroupType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'群組代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FuncGroup', @level2type = N'COLUMN', @level2name = N'GroupCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統對應ID
參考SysInfo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FuncGroup', @level2type = N'COLUMN', @level2name = N'SysCode';

