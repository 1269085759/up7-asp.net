using System.Collections.Generic;
using up7.db.model;

namespace up7.db.biz.folder
{
    /// <summary>
    /// 文件夹
    /// </summary>
    public class fd_root : FileInf
    {
        public List<FileInf> folders;
        public List<FileInf> files;
        public int folderCount = 0;
        public fd_root() { this.folder = true; }
    }
}