CREATE TABLE [dbo].[Users] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [AccID]       VARCHAR (254)  NULL,
    [UserGUID]    CHAR (32)      NOT NULL,
    [UseState]    CHAR (1)       CONSTRAINT [DF_Users_UseState] DEFAULT ('Y') NOT NULL,
    [StateUDate]  DATETIME       CONSTRAINT [DF_Users_StateUDate] DEFAULT (getdate()) NOT NULL,
    [StateUMan]   INT            NOT NULL,
    [UserStatus]  CHAR (1)       CONSTRAINT [DF_Users_UserStatus] DEFAULT ('Y') NOT NULL,
    [StatusUDate] DATETIME       CONSTRAINT [DF_Users_StatusUDate] DEFAULT (getdate()) NOT NULL,
    [StatusUMan]  INT            NOT NULL,
    [IsAdmin]     CHAR (1)       CONSTRAINT [DF_Users_IsAdmin] DEFAULT ('N') NOT NULL,
    [CreateMan]   INT            NOT NULL,
    [UpdateMan]   INT            NOT NULL,
    [CreateDate]  DATETIME       CONSTRAINT [DF_Users_CreateDate] DEFAULT (getdate()) NOT NULL,
    [ModifyDate]  DATETIME       CONSTRAINT [DF_Users_ModifyDate] DEFAULT (getdate()) NOT NULL,
    [Hidden]      BIT            CONSTRAINT [DF_Users_Hidden] DEFAULT ('0') NOT NULL,
    [Valid]       BIT            CONSTRAINT [DF_Users_Valid] DEFAULT ((0)) NOT NULL,
    [AccName]     NVARCHAR (100) NULL,
    [DoctorNo] NVARCHAR(10) NULL, 
    [Senior] INT NOT NULL DEFAULT 2, 
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_GUID]
    ON [dbo].[Users]([UserGUID] ASC);


GO
CREATE NONCLUSTERED INDEX [AccIDIndex]
    ON [dbo].[Users]([AccID] ASC)
    INCLUDE([ID], [UserGUID], [UseState], [StateUDate], [StateUMan], [CreateDate], [ModifyDate], [Hidden], [Valid], [UserStatus], [StatusUDate], [StatusUMan], [IsAdmin], [CreateMan], [UpdateMan]);

