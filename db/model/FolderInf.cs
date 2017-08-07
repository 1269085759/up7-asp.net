namespace up7.db.model
{
    /// <summary>
    /// 文件夹信息
    /// </summary>
    public class FolderInf : FileInf
    {
        public FolderInf()
        {
            this.fdTask = true;
        }

        public int foldersCount = 0;
        /// <summary>
        /// 子文件数
        /// </summary>
        public int filesCount = 0;
    }
}