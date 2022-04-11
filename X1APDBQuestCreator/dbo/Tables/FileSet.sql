CREATE TABLE [dbo].[FileSet] (
    [ID]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [FGID]      BIGINT        NOT NULL,
    [FileKey]   CHAR (32)     NOT NULL,
    [FType]     CHAR (1)      NOT NULL,
    [FileName]  NVARCHAR (30) NOT NULL,
    [FileEName] VARCHAR (6)   NOT NULL,
    [FileState] CHAR (1)      CONSTRAINT [DF_FileSet_FileState] DEFAULT ('I') NOT NULL,
    CONSTRAINT [PK_FileSet] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_FileSet_FGID]
    ON [dbo].[FileSet]([FGID] ASC);

