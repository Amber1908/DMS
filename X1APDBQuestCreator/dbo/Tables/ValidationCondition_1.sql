CREATE TABLE [dbo].[ValidationCondition] (
    [ID]            INT           IDENTITY (1, 1) NOT NULL,
    [QuestionID]    INT           NOT NULL,
    [AttributeName] VARCHAR (50)  NOT NULL,
    [Value1]        NVARCHAR (50) NOT NULL,
    [Operator1]     VARCHAR (50)  NOT NULL,
    [Value2]        NVARCHAR (50) NULL,
    [Operator2]     VARCHAR (50)  NULL,
    [GroupNum]      INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ValidationCondition_Question] FOREIGN KEY ([QuestionID]) REFERENCES [dbo].[X1_Report_Question] ([ID])
);

