CREATE TABLE [dbo].[RoleAuthMap] (
    [ID]         INT      IDENTITY (1, 1) NOT NULL,
    [RoleCode]   CHAR (6) NOT NULL,
    [SysCode]    CHAR (6) NOT NULL,
    [FuncCode]   CHAR (6) NOT NULL,
    [AuthNo1]    CHAR (1) NOT NULL,
    [AuthNo2]    CHAR (1) NOT NULL,
    [AuthNo3]    CHAR (1) NOT NULL,
    [AuthNo4]    CHAR (1) NOT NULL,
    [AuthNo5]    CHAR (1) NOT NULL,
    [CreateMan]  INT      NOT NULL,
    [CreateDate] DATETIME CONSTRAINT [DF_RoleAuthMap_CreateDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_RoleAuthMap] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_RoleAuthMap_RoleIdx]
    ON [dbo].[RoleAuthMap]([RoleCode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RoleAuthMap_FuncIdx]
    ON [dbo].[RoleAuthMap]([SysCode] ASC, [FuncCode] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'資料新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleAuthMap', @level2type = N'COLUMN', @level2name = N'CreateDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleAuthMap', @level2type = N'COLUMN', @level2name = N'CreateMan';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限5', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleAuthMap', @level2type = N'COLUMN', @level2name = N'AuthNo5';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleAuthMap', @level2type = N'COLUMN', @level2name = N'AuthNo4';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleAuthMap', @level2type = N'COLUMN', @level2name = N'AuthNo3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleAuthMap', @level2type = N'COLUMN', @level2name = N'AuthNo2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'權限1 ‘Y’：是 ‘N’：否', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleAuthMap', @level2type = N'COLUMN', @level2name = N'AuthNo1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'功能代碼 Functions FK', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleAuthMap', @level2type = N'COLUMN', @level2name = N'FuncCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統代碼 SysInfo FK', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleAuthMap', @level2type = N'COLUMN', @level2name = N'SysCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'角色代碼 Roles FK', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoleAuthMap', @level2type = N'COLUMN', @level2name = N'RoleCode';

