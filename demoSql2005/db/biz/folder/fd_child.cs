using System;
using System.Collections.Generic;
using System.Web;

namespace up7.demoSql2005.db.biz.folder
{
    /// <summary>
    /// 子文件夹
    /// </summary>
    public class fd_child : xdb_files
    {
        public fd_child() { this.fdTask = true; }
        public int folders=0;
        public int filesCount = 0;
    }
}