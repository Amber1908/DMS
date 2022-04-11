CREATE TABLE [dbo].[Roles] (
    [RoleCode]        CHAR (6)      NOT NULL,
    [RoleName]        NVARCHAR (20) NOT NULL,
    [RoleTitle]       NVARCHAR (20) NOT NULL,
    [CreateDate]      DATETIME      CONSTRAINT [DF_Roles_CreateDate] DEFAULT (getdate()) NOT NULL,
    [Hidden]          BIT           CONSTRAINT [DF_Roles_Hidden] DEFAULT ('0') NOT NULL,
    [RoleBackendName] NVARCHAR (20) NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RoleCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'後端顯示名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Roles', @level2type = N'COLUMN', @level2name = N'RoleBackendName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'隱藏', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Roles', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'資料新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Roles', @level2type = N'COLUMN', @level2name = N'CreateDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'角色別名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Roles', @level2type = N'COLUMN', @level2name = N'RoleTitle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'角色名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Roles', @level2type = N'COLUMN', @level2name = N'RoleName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'角色代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Roles', @level2type = N'COLUMN', @level2name = N'RoleCode';

