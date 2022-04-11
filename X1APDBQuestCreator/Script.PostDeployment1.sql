/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
SET IDENTITY_INSERT [dbo].[X1_Report_Question_Type] ON 
GO
BEGIN
    IF NOT EXISTS (SELECT * FROM [X1_Report_Question_Type] WHERE ID = 1)
    BEGIN
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (1, N'display')
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (2, N'text')
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (3, N'readonlyimage')
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (6, N'panel')
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (7, N'select')
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (8, N'radio')
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (9, N'checkbox')
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (10, N'texttemplate')
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (11, N'switch')
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (12, N'paneltexttemplate')
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (13, N'image')
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (14, N'scoresum')
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (15, N'pdf')
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (16, N'otherans')
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (17, N'number')
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (19, N'date')
    END
END
BEGIN
    IF NOT EXISTS (SELECT * FROM [X1_Report_Question_Type] WHERE ID = 18)
    BEGIN
        INSERT [dbo].[X1_Report_Question_Type] ([ID], [Name]) VALUES (18, N'file')
    END
END
SET IDENTITY_INSERT [dbo].[X1_Report_Question_Type] OFF
GO

BEGIN
    IF NOT EXISTS (SELECT * FROM [FuncGroup] WHERE [GroupCode] = 'G01')
    BEGIN
        INSERT [dbo].[FuncGroup] ([SysCode], [GroupCode], [GroupType], [GroupName], [GroupState], [CreateMan], [CreateDate]) VALUES (N'SYS002', N'G01', N'F', N'X1Web功能群組', N'Y', 1, CAST(N'2019-06-24T00:00:00.000' AS DateTime))
        INSERT [dbo].[Functions] ([SysCode], [GroupCode], [FuncCode], [FuncName], [FuncPath], [FuncStatus], [AuthFlag1], [AuthName1], [AuthFlag2], [AuthName2], [AuthFlag3], [AuthName3], [AuthFlag4], [AuthName4], [AuthFlag5], [AuthName5], [CreateMan], [CreateDate]) VALUES (N'SYS002', N'G01', N'FN0004', N'新增個案', N'/', N'Y', N'Y', N'啟用', N'N', N'', N'N', N'', N'N', N'', N'N', N'', 1, CAST(N'2020-07-28T13:16:26.187' AS DateTime))
        INSERT [dbo].[Functions] ([SysCode], [GroupCode], [FuncCode], [FuncName], [FuncPath], [FuncStatus], [AuthFlag1], [AuthName1], [AuthFlag2], [AuthName2], [AuthFlag3], [AuthName3], [AuthFlag4], [AuthName4], [AuthFlag5], [AuthName5], [CreateMan], [CreateDate]) VALUES (N'SYS002', N'G01', N'FN0101', N'Web檢視', N'/', N'Y', N'Y', N'啟用', N'N', N'', N'N', N'', N'N', N'', N'N', N'', 1, CAST(N'2019-06-24T00:00:00.000' AS DateTime))
        INSERT [dbo].[Functions] ([SysCode], [GroupCode], [FuncCode], [FuncName], [FuncPath], [FuncStatus], [AuthFlag1], [AuthName1], [AuthFlag2], [AuthName2], [AuthFlag3], [AuthName3], [AuthFlag4], [AuthName4], [AuthFlag5], [AuthName5], [CreateMan], [CreateDate]) VALUES (N'SYS002', N'G01', N'FN0102', N'後台管理', N'/', N'Y', N'Y', N'啟用', N'N', N'', N'N', N'', N'N', N'', N'N', N'', 1, CAST(N'2019-07-30T17:41:20.670' AS DateTime))

        SET IDENTITY_INSERT [dbo].[RoleAuthMap] ON 
        INSERT [dbo].[RoleAuthMap] ([ID], [RoleCode], [SysCode], [FuncCode], [AuthNo1], [AuthNo2], [AuthNo3], [AuthNo4], [AuthNo5], [CreateMan], [CreateDate]) VALUES (2213, N'X1R000', N'SYS002', N'FN0004', N'N', N'N', N'N', N'N', N'N', 58, CAST(N'2021-04-06T10:05:07.173' AS DateTime))
        INSERT [dbo].[RoleAuthMap] ([ID], [RoleCode], [SysCode], [FuncCode], [AuthNo1], [AuthNo2], [AuthNo3], [AuthNo4], [AuthNo5], [CreateMan], [CreateDate]) VALUES (2214, N'X1R000', N'SYS002', N'FN0101', N'Y', N'N', N'N', N'N', N'N', 58, CAST(N'2021-04-06T10:05:07.173' AS DateTime))
        INSERT [dbo].[RoleAuthMap] ([ID], [RoleCode], [SysCode], [FuncCode], [AuthNo1], [AuthNo2], [AuthNo3], [AuthNo4], [AuthNo5], [CreateMan], [CreateDate]) VALUES (2215, N'X1R000', N'SYS002', N'FN0102', N'N', N'N', N'N', N'N', N'N', 58, CAST(N'2021-04-06T10:05:07.173' AS DateTime))
        INSERT [dbo].[RoleAuthMap] ([ID], [RoleCode], [SysCode], [FuncCode], [AuthNo1], [AuthNo2], [AuthNo3], [AuthNo4], [AuthNo5], [CreateMan], [CreateDate]) VALUES (2236, N'X1R002', N'SYS002', N'FN0004', N'Y', N'N', N'N', N'N', N'N', 6, CAST(N'2021-04-14T09:39:14.217' AS DateTime))
        INSERT [dbo].[RoleAuthMap] ([ID], [RoleCode], [SysCode], [FuncCode], [AuthNo1], [AuthNo2], [AuthNo3], [AuthNo4], [AuthNo5], [CreateMan], [CreateDate]) VALUES (2237, N'X1R002', N'SYS002', N'FN0101', N'Y', N'N', N'N', N'N', N'N', 6, CAST(N'2021-04-14T09:39:14.217' AS DateTime))
        INSERT [dbo].[RoleAuthMap] ([ID], [RoleCode], [SysCode], [FuncCode], [AuthNo1], [AuthNo2], [AuthNo3], [AuthNo4], [AuthNo5], [CreateMan], [CreateDate]) VALUES (2238, N'X1R002', N'SYS002', N'FN0102', N'Y', N'N', N'N', N'N', N'N', 6, CAST(N'2021-04-14T09:39:14.217' AS DateTime))
        SET IDENTITY_INSERT [dbo].[RoleAuthMap] OFF
        
        INSERT [dbo].[Roles] ([RoleCode], [RoleName], [RoleTitle], [CreateDate], [Hidden], [RoleBackendName]) VALUES (N'X1R000', N'一般使用者', N'一般使用者', CAST(N'2020-11-20T15:31:29.927' AS DateTime), 0, N'一般使用者')
        
        INSERT [dbo].[Roles] ([RoleCode], [RoleName], [RoleTitle], [CreateDate], [Hidden], [RoleBackendName]) VALUES (N'X1R002', N'系統管理員', N'系統管理員', CAST(N'2019-05-10T10:03:52.710' AS DateTime), 0, N'系統管理員')
        
        INSERT [dbo].[SysInfo] ([SysCode], [SysName], [SysStatus], [CreateMan], [CreateDate]) VALUES (N'SYS002', N'X1Web', N'Y', 1, CAST(N'2019-07-25T15:39:16.040' AS DateTime))
        
    END
END
