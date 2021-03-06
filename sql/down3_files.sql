USE [up7]
GO
/****** 对象:  Table [dbo].[down3_files]    脚本日期: 08/08/2017 17:07:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[down3_files](
	[f_id]      [char](32) NOT  NULL,
	[f_uid]     [int] NOT       NULL CONSTRAINT [DF_down3_files_f_uid]  DEFAULT ((0)),
	[f_mac]     [nvarchar](50)  NULL,
	[f_nameLoc] [nvarchar](255) NULL,
	[f_pathLoc] [nvarchar](255) NULL,
	[f_fileUrl] [nvarchar](255) NULL,
	[f_perLoc]  [nvarchar](6)   NULL CONSTRAINT [DF_down3_files_f_percent]  DEFAULT (N'0%'),
	[f_lenLoc]  [bigint]        NULL CONSTRAINT [DF_down3_files_f_lenLoc]  DEFAULT ((0)),
	[f_lenSvr]  [bigint]        NULL CONSTRAINT [DF_down3_files_f_lenSvr]  DEFAULT ((0)),
	[f_sizeLoc] [nvarchar](10)  NULL,
	[f_sizeSvr] [nvarchar](10)  NULL CONSTRAINT [DF_down3_files_f_sizeSvr]  DEFAULT (N'0byte'),
	[f_fdTask]  [bit]           NULL CONSTRAINT [DF_down3_files_f_folder]  DEFAULT ((0))
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'down3_files', @level2type=N'COLUMN',@level2name=N'f_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID,用来与第三方系统整合。' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'down3_files', @level2type=N'COLUMN',@level2name=N'f_uid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MAC地址，用来识别不同电脑的下载任务' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'down3_files', @level2type=N'COLUMN',@level2name=N'f_mac'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本地文件路径。' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'down3_files', @level2type=N'COLUMN',@level2name=N'f_pathLoc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务器文件地址。' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'down3_files', @level2type=N'COLUMN',@level2name=N'f_fileUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'已下载进度。' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'down3_files', @level2type=N'COLUMN',@level2name=N'f_perLoc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本地文件长度（已下载文件长度）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'down3_files', @level2type=N'COLUMN',@level2name=N'f_lenLoc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务器文件长度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'down3_files', @level2type=N'COLUMN',@level2name=N'f_lenSvr'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'格式化的远程文件大小。1MB' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'down3_files', @level2type=N'COLUMN',@level2name=N'f_sizeLoc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标识是否是一个文件夹' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'down3_files', @level2type=N'COLUMN',@level2name=N'f_fdTask'