CREATE TABLE [dbo].[UserRoleMap] (
    [ID]         INT      IDENTITY (1, 1) NOT NULL,
    [UserID]     INT      NOT NULL,
    [RoleCode]   CHAR (6) NOT NULL,
    [CreateMan]  INT      NOT NULL,
    [CreateDate] DATETIME CONSTRAINT [DF_UserRoleMap_CreateDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_UserRoleMap] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserRoleMapUIDRCode]
    ON [dbo].[UserRoleMap]([UserID] ASC, [RoleCode] ASC);

