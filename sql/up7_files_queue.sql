USE [up7]
GO
/****** 对象:  Table [dbo].[up7_files_queue]    脚本日期: 05/13/2017 14:46:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[up7_files_queue](
	[f_id] [char](36) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[f_pidSign] [char](36) COLLATE Chinese_PRC_CI_AS NULL CONSTRAINT [DF_up7_files_queue_f_pidSign]  DEFAULT (''),
	[f_rootSign] [char](36) COLLATE Chinese_PRC_CI_AS NULL CONSTRAINT [DF_up7_files_queue_f_rootSign]  DEFAULT (''),
	[f_fdTask] [bit] NULL CONSTRAINT [DF_up7_files_queue_f_fdTask]  DEFAULT ((0)),
	[f_fdChild] [bit] NULL CONSTRAINT [DF_up7_files_queue_f_fdChild]  DEFAULT ((0)),
	[f_uid] [int] NULL CONSTRAINT [DF_up7_files_queue_f_uid]  DEFAULT ((0)),
	[f_nameLoc] [varchar](255) COLLATE Chinese_PRC_CI_AS NULL CONSTRAINT [DF_up7_files_queue_f_nameLoc]  DEFAULT (''),
	[f_nameSvr] [varchar](255) COLLATE Chinese_PRC_CI_AS NULL CONSTRAINT [DF_up7_files_queue_f_nameSvr]  DEFAULT (''),
	[f_pathLoc] [varchar](512) COLLATE Chinese_PRC_CI_AS NULL CONSTRAINT [DF_up7_files_queue_f_pathLoc]  DEFAULT (''),
	[f_pathSvr] [varchar](512) COLLATE Chinese_PRC_CI_AS NULL CONSTRAINT [DF_up7_files_queue_f_pathSvr]  DEFAULT (''),
	[f_pathRel] [varchar](512) COLLATE Chinese_PRC_CI_AS NULL CONSTRAINT [DF_up7_files_queue_f_pathRel]  DEFAULT (''),
	[f_md5] [varchar](40) COLLATE Chinese_PRC_CI_AS NULL CONSTRAINT [DF_up7_files_queue_f_md5]  DEFAULT (''),
	[f_lenLoc] [bigint] NULL CONSTRAINT [DF_up7_files_queue_f_lenLoc]  DEFAULT ((0)),
	[f_sizeLoc] [varchar](15) COLLATE Chinese_PRC_CI_AS NULL CONSTRAINT [DF_up7_files_queue_f_sizeLoc]  DEFAULT ('0Bytes'),
	[f_pos] [bigint] NULL CONSTRAINT [DF_up7_files_queue_f_pos]  DEFAULT ((0)),
	[f_blockCount] [int] NULL CONSTRAINT [DF_up7_files_queue_f_blockCount]  DEFAULT ((1)),
	[f_blockSize] [int] NULL CONSTRAINT [DF_up7_files_queue_f_blockSize]  DEFAULT ((1)),
	[f_blockPath] [varchar](2000) COLLATE Chinese_PRC_CI_AS NULL,
	[f_lenSvr] [bigint] NULL CONSTRAINT [DF_up7_files_queue_f_lenSvr]  DEFAULT ((0)),
	[f_perSvr] [varchar](6) COLLATE Chinese_PRC_CI_AS NULL CONSTRAINT [DF_up7_files_queue_f_perSvr]  DEFAULT ('0%'),
	[f_complete] [bit] NULL CONSTRAINT [DF_up7_files_queue_f_complete]  DEFAULT ((0)),
	[f_time] [datetime] NULL CONSTRAINT [DF_up7_files_queue_f_time]  DEFAULT (getdate()),
	[f_deleted] [bit] NULL CONSTRAINT [DF_up7_files_queue_f_deleted]  DEFAULT ((0)),
	[f_sign] [nvarchar](32) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_up7_files_queue] PRIMARY KEY CLUSTERED 
(
	[f_id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表示是否是一个文件夹上传任务' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_fdTask'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否是文件夹中的子项' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_fdChild'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件在本地电脑中的名称。例：QQ.exe ' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_nameLoc'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件在服务器中的名称。一般为文件MD5+扩展名。' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_nameSvr'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件在本地电脑中的完整路径。
示例：D:\Soft\QQ.exe
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_pathLoc'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件在服务器中的完整路径。
示例：F:\ftp\user1\QQ2012.exe
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_pathSvr'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件在服务器中的相对路径。
示例：/www/web/upload/QQ2012.exe
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_pathRel'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件MD5' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_md5'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件总长度。以字节为单位
最大值：9,223,372,036,854,775,807
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_lenLoc'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'格式化的文件尺寸。示例：10MB' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_sizeLoc'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件续传位置。
最大值：9,223,372,036,854,775,807
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_pos'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件块总数' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_blockCount'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'块大小' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_blockSize'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件块保存路径' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_blockPath'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'已上传长度。以字节为单位。
最大值：9,223,372,036,854,775,807
' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_lenSvr'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'已上传百分比。示例：10%' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_perSvr'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已上传完毕。' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_complete'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件上传时间' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_time'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已删除。' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'up7_files_queue', @level2type=N'COLUMN', @level2name=N'f_deleted'
