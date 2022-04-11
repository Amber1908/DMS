CREATE TABLE [dbo].[DMSSetting] (
    [Web_sn]   INT            NOT NULL,
    [Web_name] NVARCHAR (100) NULL,
    [Web_db]   VARCHAR (500)  NOT NULL,
    [Logo]     INT            NULL,
    CONSTRAINT [PK_DMSSetting] PRIMARY KEY CLUSTERED ([Web_sn] ASC)
);

