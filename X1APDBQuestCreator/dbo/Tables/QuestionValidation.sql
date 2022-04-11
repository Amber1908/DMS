CREATE TABLE [dbo].[QuestionValidation] (
    [ID]         INT           IDENTITY (1, 1) NOT NULL,
    [QuestionID] INT           NOT NULL,
    [Operator]   VARCHAR (50)  NOT NULL,
    [Value]      NVARCHAR (50) NOT NULL,
    [Color]      VARCHAR (50)  NULL,
    [Normal]     BIT           DEFAULT ((0)) NOT NULL,
    [GroupNum]   INT           NULL,
    CONSTRAINT [PK_QuestionValidation] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_QuestionValidation_X1_Report_Question] FOREIGN KEY ([QuestionID]) REFERENCES [dbo].[X1_Report_Question] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'運算元', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QuestionValidation', @level2type = N'COLUMN', @level2name = N'Value';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'問題ID 參考 X1_Report_Question', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QuestionValidation', @level2type = N'COLUMN', @level2name = N'QuestionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'運算子 >、>=、=、<、<=、Equals、Contains', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QuestionValidation', @level2type = N'COLUMN', @level2name = N'Operator';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為標準值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QuestionValidation', @level2type = N'COLUMN', @level2name = N'Normal';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標準值群組代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QuestionValidation', @level2type = N'COLUMN', @level2name = N'GroupNum';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'符合條件時顏色 css color能接受的顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QuestionValidation', @level2type = N'COLUMN', @level2name = N'Color';

