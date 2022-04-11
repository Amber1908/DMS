CREATE TABLE [dbo].[X1_PatientGroupMap] (
    [PID]        INT           NOT NULL,
    [PGID]       INT           NOT NULL,
    [CreateDate] DATETIME2 (7) NOT NULL,
    [CreateMan]  VARCHAR (254) NOT NULL,
    CONSTRAINT [PK_X1_PatientGroupMap] PRIMARY KEY CLUSTERED ([PID] ASC, [PGID] ASC),
    CONSTRAINT [FK_X1_PatientGroupMap_X1_PatientGroup] FOREIGN KEY ([PGID]) REFERENCES [dbo].[X1_PatientGroup] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_X1_PatientGroupMap_X1_PatientInfo] FOREIGN KEY ([PID]) REFERENCES [dbo].[X1_PatientInfo] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);



