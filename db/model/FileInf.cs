using System;

namespace up7.db.model
{
    public class FileInf
    {
        public string id = string.Empty;
        public string pid = string.Empty;
        public string pidRoot = string.Empty;
        public bool fdTask = false;//是否是一个文件夹
        public int blockCount = 0;
        public int blockSize = 0;//块大小
        public int fileCount = 0;
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
        public string sizeLoc = "0byte";
        public string sizeSvr = "0byte";
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
    }
}
