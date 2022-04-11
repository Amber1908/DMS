CREATE TABLE [dbo].[X1_PatientInfo] (
    [ID]              INT             IDENTITY (1, 1) NOT NULL,
    [PUCountry]       INT             NOT NULL,
    [PUName]          NVARCHAR (100)  NOT NULL,
    [IDNo]            CHAR (10)       NOT NULL,
    [PUDOB]           DATE            NULL,
    [Gender]          CHAR (1)        NULL,
    [CreateDate]      DATETIME        NOT NULL,
    [CreateMan]       VARCHAR (254)   NOT NULL,
    [ModifyDate]      DATETIME        NOT NULL,
    [ModifyMan]       VARCHAR (254)   NOT NULL,
    [Height]          DECIMAL (18, 4) NULL,
    [Weight]          DECIMAL (18, 4) NULL,
    [Phone]           VARCHAR (50)    NULL,
    [Cellphone]       VARCHAR (50)    NULL,
    [ContactPhone]    VARCHAR (50)    NULL,
    [ContactRelation] NVARCHAR (100)  NULL,
    [Education]       INT             NULL,
    [AddrCode]        NVARCHAR (10)   NULL,
    [HCCode]        NVARCHAR (10)   NULL,
    [Addr]            NVARCHAR (100)  NULL,
    [Domicile]        NVARCHAR (10)   NULL,
    CONSTRAINT [PK_PatinetInfo] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_X1_PatientInfo]
    ON [dbo].[X1_PatientInfo]([PUName] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_X1_PatientInfo_2]
    ON [dbo].[X1_PatientInfo]([IDNo] ASC);

