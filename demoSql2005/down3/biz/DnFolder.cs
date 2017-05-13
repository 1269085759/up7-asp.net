using up7.demoSql2005.db;
using System.Data.Common;
using up7.demoSql2005.db;

namespace up7.demoSql2005.down3.db
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