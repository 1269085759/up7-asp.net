官网：www.ncmem.com
数据库：SQL2005
.net框架：2.0
语言：c#
编码：GB2312
版本说明：
  up7取消md5验证，所有文件和文件夹均使用uuid模式存储。up7一般用来处理TB级数据，计算MD5需要花费较长的时间，所以取消MD5验证。
布署说明：
  up7采用并发上传技术，DEMO需要布置在iis中测试，不建议使用vs内置的web server测试。up7在上传过程中会采用多线程方式上传
  这种方式会为web server带来较大的负载压力。

/////////////////////////////////////////////////////////////////////////////
上传SQL脚本：
  f_process.sql             用来更新文件进度。一般在f_post.aspx中调用。在上传第一块时调用。
  fd_fileProcess.sql        用来更新文件夹进度。一般在f_post.aspx中调用。
  fd_files_add_batch.sql    批量分配文件ID，一般在文件夹初始化(fd_create.aspx,fd_create_uuid.aspx)逻辑中调用。
  fd_files_check.sql        批量检查已存在的文件，一般在文件夹初始化（fd_create.aspx）逻辑中调用。
  spGetFileInfByFid.sql     暂时未使用。
  up6_files.sql             文件表
  up6_folders.sql           文件夹表

/////////////////////////////////////////////////////////////////////////////
下载SQL脚本
  down_f_del.sql        存储过程-删除下载记录
  down_f_process.sql    存储过程-更新下载进度
  fd_add_batch.sql      存储过程-批量分配文件ID，在下载文件夹（down3/fd_create.aspx）逻辑中调用。
  down_files.sql        文件表
  down_folders.sql      文件夹表