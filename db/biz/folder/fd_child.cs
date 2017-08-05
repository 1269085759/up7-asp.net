using up7.db.model;

namespace up7.db.biz.folder
{
    /// <summary>
    /// 子文件夹
    /// </summary>
    public class fd_child : xdb_files
    {
        public fd_child() { this.folder = true; }
        public int folderCount=0;
    }
}