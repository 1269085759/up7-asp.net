using System.Data.Common;

namespace up7.db.biz.database
{
    /// <summary>
    /// 数据库访问操作
    /// 更新记录：
    ///		2012-04-10 创建
    ///		2014-03-11 将OleDb对象全部改为使用DbHelper对象，简化代码。
    /// </summary>
    public class DBFile
    {
        static public void Clear()
        {
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand("delete from up7_files;");
            db.ExecuteNonQuery(cmd);
            cmd.CommandText = "delete from up7_folders;";
            db.ExecuteNonQuery(cmd);
        }

        public void merged(string id)
        {
            string sql = "update up7_files set f_lenSvr=f_lenLoc,f_perSvr='100%',f_complete=1,f_merged=1 where f_id=@id";
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sql);

            db.AddString(ref cmd, "@id", id, 32);
            db.ExecuteNonQuery(cmd);
        }
    }
}