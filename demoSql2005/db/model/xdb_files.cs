using System;
using Newtonsoft.Json;

namespace up7.demoSql2005.db
{
    public class xdb_files
    {
        public string idSign = string.Empty;
        public string pidSign = string.Empty;
        public string rootSign = string.Empty;
        public string signSvr = string.Empty;
        public bool fdTask = false;//是否是一个文件夹
        public int blockCount = 0;
        public int fileCount = 0;
        public int idSvr = 0;
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public int pid = 0;
        /// <summary>
        /// 根级文件夹ID
        /// </summary>
        public int pidRoot = 0;
        /// <summary>
        /// 表示当前项是否是一个文件夹项。
        /// </summary>
        public bool f_fdTask = false;
        /// <summary>
        /// 与xdb_folders.fd_id对应
        /// </summary>
        public int f_fdID = 0;
        /// <summary>
        /// 是否是文件夹中的子文件
        /// </summary>
        public bool f_fdChild = false;
        /// <summary>
        /// 用户ID。与第三方系统整合使用。
        /// </summary>	
        public int uid = 0;
        /// <summary>
        /// 文件在本地电脑中的名称。
        /// </summary>
        public string nameLoc = string.Empty;
        /// <summary>
        /// 文件在服务器中的名称。
        /// </summary>
        public string nameSvr = string.Empty;
        /// <summary>
        /// 文件在本地电脑中的完整路径。示例：D:\Soft\QQ2012.exe
        /// </summary>
        public string pathLoc = string.Empty;
        /// <summary>
        /// 文件在服务器中的完整路径。示例：F:\ftp\uer\md5.exe
        /// </summary>
        public string pathSvr = string.Empty;
        /// <summary>
        /// 文件块根目录
        /// f:/webapps/files/年/月/日/guid/file-guid/
        /// </summary>
        public string blockPath = string.Empty;
        /// <summary>
        /// 本地路径：D:/soft/safe/360.exe
        /// 相对路径 soft/safe/360.exe
        /// 文件在服务器中的相对路径。
        /// </summary>
        public string pathRel = string.Empty;
        /// <summary>
        /// 文件MD5
        /// </summary>
        public string md5 = string.Empty;
        /// <summary>
        /// 数字化的文件长度。以字节为单位，示例：120125
        /// 文件大小可能超过2G，所以使用long
        /// </summary>
        public long lenLoc = 0;
        /// <summary>
        /// 格式化的文件尺寸。示例：10.03MB
        /// </summary>
        public string sizeLoc = string.Empty;
        public string sizeSvr = string.Empty;
        /// <summary>
        /// 文件续传位置。
        /// 文件大小可能超过2G，所以使用long
        /// </summary>
        public long FilePos = 0;
        /// <summary>
        /// 已上传大小。以字节为单位
        /// 文件大小可能超过2G，所以使用long
        /// </summary>
        public long lenSvr = 0;
        /// <summary>
        /// 已上传百分比。示例：10%
        /// </summary>
        public string perSvr = "0%";
        /// <summary>
        /// PostComplete
        /// </summary>
        public bool complete = false;
        /// <summary>
        /// PostedTime
        /// </summary>
        public DateTime time = DateTime.Now;
        /// <summary>
        /// IsDeleted
        /// </summary>
        public bool deleted = false;
        /// <summary>
        /// 文件唯一标识
        /// </summary>
        public string sign = Guid.NewGuid().ToString("N");
        /// <summary>
        /// 文件夹JSON信息
        /// </summary>
        public string fd_json = string.Empty;
    }
}
