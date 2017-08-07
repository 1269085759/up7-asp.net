using System.Data.Common;
using System.Text;
using up7.db.model;

namespace up7.db.biz.database
{
    public class DBFolder
    {
        static public void Clear()
        {
            string sql = "delete from up7_folders";
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sql);
            db.ExecuteNonQuery(cmd);
        }

        public void add(ref FileInf f)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into up7_folders(");
            sb.Append(" fd_id");
            sb.Append(",fd_uid");
            sb.Append(",fd_name");
            sb.Append(",fd_pathLoc");
            sb.Append(",fd_pathSvr");
            sb.Append(",fd_pathRel");
            sb.Append(",fd_files");

            sb.Append(") values (");

            sb.Append(" @fd_id");
            sb.Append(",@fd_uid");
            sb.Append(",@fd_name");
            sb.Append(",@fd_pathLoc");
            sb.Append(",@fd_pathSvr");
            sb.Append(",@fd_pathRel");
            sb.Append(",@fd_files");
            sb.Append(") ;");

            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sb.ToString());

            db.AddString(ref cmd, "@fd_id", f.id, 32);
            db.AddInt(ref cmd, "@fd_uid", f.uid);
            db.AddString(ref cmd, "@fd_name", f.nameLoc, 255);
            db.AddString(ref cmd, "@fd_pathLoc", f.pathLoc, 512);
            db.AddString(ref cmd, "@fd_pathSvr", f.pathSvr, 512);
            db.AddString(ref cmd, "@fd_pathRel", f.pathRel, 512);
            db.AddInt(ref cmd, "@fd_files", f.fileCount);

            db.ExecuteNonQuery(cmd);
        }
    }
}