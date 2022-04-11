CREATE TABLE [dbo].[SessionDMSMap] (
    [Sessionkey] VARCHAR (100) NOT NULL,
    [Web_sn]     INT           NOT NULL,
    [AccID]      VARCHAR (254) NOT NULL,
    CONSTRAINT [PK_SessionDMSMap_1] PRIMARY KEY CLUSTERED ([Sessionkey] ASC, [Web_sn] ASC)
);

