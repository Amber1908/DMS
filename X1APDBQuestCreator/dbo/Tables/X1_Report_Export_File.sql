CREATE TABLE [dbo].[X1_Report_Export_File] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [RAMID]      INT            NOT NULL,
    [GUID]       CHAR (32)      NOT NULL,
    [FileName]   NVARCHAR (100) NOT NULL,
    [MimeType]   VARCHAR (100)  NOT NULL,
    [CreateDate] DATETIME       NOT NULL,
    [CreateMan]  VARCHAR (254)  NOT NULL,
    [ModifyDate] DATETIME       NOT NULL,
    [ModifyMan]  VARCHAR (254)  NOT NULL,
    CONSTRAINT [PK__tmp_ms_x__3214EC2789D12962] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_X1_Report_Export_File_X1_Report_Answer_Main] FOREIGN KEY ([RAMID]) REFERENCES [dbo].[X1_Report_Answer_Main] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_X1_Report_Export_File_Column]
    ON [dbo].[X1_Report_Export_File]([RAMID] ASC);

