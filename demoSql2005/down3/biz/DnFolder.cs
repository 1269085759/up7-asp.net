﻿using up6.demoSql2005.db;
using System.Data.Common;

namespace up6.demoSql2005.down2.db
{
    public class DnFolder
    {
        /// <summary>
        /// 清空数据库
        /// </summary>
        public static void Clear()
        {
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand("delete from down3_folders;");
            db.ExecuteNonQuery(ref cmd);
        }
    }
}