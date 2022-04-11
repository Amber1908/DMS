CREATE TABLE [dbo].[Functions] (
    [SysCode]    CHAR (6)      NOT NULL,
    [GroupCode]  CHAR (3)      NOT NULL,
    [FuncCode]   CHAR (6)      NOT NULL,
    [FuncName]   NVARCHAR (30) NOT NULL,
    [FuncPath]   VARCHAR (50)  NOT NULL,
    [FuncStatus] CHAR (1)      CONSTRAINT [DF_Functions_FuncStatus] DEFAULT ('Y') NOT NULL,
    [AuthFlag1]  CHAR (1)      CONSTRAINT [DF_Functions_AuthFlag1] DEFAULT ('Y') NOT NULL,
    [AuthName1]  NVARCHAR (10) CONSTRAINT [DF_Functions_AuthName1] DEFAULT ('??') NOT NULL,
    [AuthFlag2]  CHAR (1)      CONSTRAINT [DF_Table_1_AuthFlag11] DEFAULT ('N') NOT NULL,
    [AuthName2]  NVARCHAR (10) CONSTRAINT [DF_Table_1_AuthName11] DEFAULT ('') NOT NULL,
    [AuthFlag3]  CHAR (1)      CONSTRAINT [DF_Table_1_AuthFlag12] DEFAULT ('N') NOT NULL,
    [AuthName3]  NVARCHAR (10) CONSTRAINT [DF_Table_1_AuthName12] DEFAULT ('') NOT NULL,
    [AuthFlag4]  CHAR (1)      CONSTRAINT [DF_Table_1_AuthFlag13] DEFAULT ('N') NOT NULL,
    [AuthName4]  NVARCHAR (10) CONSTRAINT [DF_Table_1_AuthName13] DEFAULT ('') NOT NULL,
    [AuthFlag5]  CHAR (1)      CONSTRAINT [DF_Table_1_AuthFlag14] DEFAULT ('N') NOT NULL,
    [AuthName5]  NVARCHAR (10) CONSTRAINT [DF_Table_1_AuthName14] DEFAULT ('') NOT NULL,
    [CreateMan]  INT           NOT NULL,
    [CreateDate] DATETIME      CONSTRAINT [DF_Functions_CreateDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Functions_1] PRIMARY KEY CLUSTERED ([SysCode] ASC, [FuncCode] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Functions_MenuListIdx]
    ON [dbo].[Functions]([SysCode] ASC, [GroupCode] ASC, [FuncCode] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'資料新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'CreateDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'CreateMan';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限5 Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'AuthName5';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限5 Flag ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'AuthFlag5';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限4 Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'AuthName4';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限4 Flag ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'AuthFlag4';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限3 Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'AuthName3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限3 Flag ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'AuthFlag3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限2 Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'AuthName2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限2 Flag', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'AuthFlag2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限1 Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'AuthName1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限1 Flag
‘Y’：開啟
‘N’：關閉
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'AuthFlag1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'功能狀態
‘Y’：有效
‘N’：關閉服務
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'FuncStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'功能路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'FuncPath';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'功能名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'FuncName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'功能代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'FuncCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'功能群組代碼
參考FuncGroup', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'GroupCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統對應ID
參考SysInfo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'SysCode';

