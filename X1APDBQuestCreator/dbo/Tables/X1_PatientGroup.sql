CREATE TABLE [dbo].[X1_PatientGroup] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [GroupName]  NVARCHAR (100) NOT NULL,
    [CreateDate] DATETIME2 (7)  NOT NULL,
    [CreateMan]  VARCHAR (254)  NOT NULL,
    [ModifyDate] DATETIME2 (7)  NOT NULL,
    [ModifyMan]  VARCHAR (254)  NOT NULL,
    [DeleteDate] DATETIME2 (7)  NULL,
    [DeleteMan]  VARCHAR (254)  NULL,
    [IsDelete]   BIT            CONSTRAINT [DF_X1_PatientGroup_IsDelete] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_X1_PatientGroup] PRIMARY KEY CLUSTERED ([ID] ASC)
);



