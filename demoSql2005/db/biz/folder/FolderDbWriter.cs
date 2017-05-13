using System.Data.Common;
using System.Text;

namespace up7.demoSql2005.db.biz.folder
{
    public class FolderDbWriter
    {
        fd_root root;//根目录
        DbConnection con = null;

        public FolderDbWriter(DbConnection con, fd_root fd)
        {
            this.con = con;
            this.root = fd;
        }

        DbCommand makeCmd(DbConnection con)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("update up7_folders set");
            sb.Append(" fd_name=@fd_name");
            sb.Append(",fd_pidSign=@fd_pidSign");
            sb.Append(",fd_uid=@fd_uid");
            sb.Append(",fd_length=@fd_length");
            sb.Append(",fd_size=@fd_size");
            sb.Append(",fd_pathLoc=@fd_pathLoc");
            sb.Append(",fd_pathSvr=@fd_pathSvr");
            sb.Append(",fd_folders=@fd_folders");
            sb.Append(",fd_files=@fd_files");
            sb.Append(",fd_rootSign=@fd_rootSign");
            sb.Append(" where fd_sign=@fd_sign");

            DbHelper db = new DbHelper();
            var cmd = con.CreateCommand();
            cmd.CommandText = sb.ToString();
            db.AddString(ref cmd, "@fd_name", string.Empty, 50);
            db.AddString(ref cmd, "@fd_pidSign", string.Empty, 36);
            db.AddInt(ref cmd, "@fd_uid", 0);
            db.AddInt(ref cmd, "@fd_length", 0);
            db.AddInt(ref cmd, "@fd_size", 0);
            db.AddString(ref cmd, "@fd_pathLoc", string.Empty, 512);
            db.AddString(ref cmd, "@fd_pathSvr", string.Empty, 512);
            db.AddInt(ref cmd, "@fd_folders", 0);
            db.AddInt(ref cmd, "@fd_files", 0);
            db.AddString(ref cmd, "@fd_rootSign", string.Empty, 512);
            db.AddString(ref cmd, "@fd_sign", string.Empty, 512);

            return cmd;
        }

        public void save()
        {
            if (this.root.folders == null) return;
            if (this.root.folders.Count < 1) return;
            using (var cmd = this.makeCmd(con))
            {

                //写根目录
                cmd.Parameters[0].Value = this.root.nameLoc;
                cmd.Parameters[1].Value = this.root.pidSvr;//fd_pid
                cmd.Parameters[2].Value = this.root.uid;//fd_uid
                cmd.Parameters[3].Value = this.root.lenLoc;//fd_length
                cmd.Parameters[4].Value = this.root.sizeLoc;//fd_size
                cmd.Parameters[5].Value = this.root.pathLoc;//fd_pathLoc
                cmd.Parameters[6].Value = this.root.pathSvr;//fd_pathSvr
                cmd.Parameters[7].Value = this.root.foldersCount;//fd_folders
                cmd.Parameters[8].Value = this.root.filesCount;//fd_files
                cmd.Parameters[9].Value = this.root.rootSign;//fd_pidRoot
                cmd.Parameters[10].Value = this.root.idSign;//fd_id
                cmd.ExecuteNonQuery();

                //写子目录列表
                foreach (var fd in this.root.folders)
                {
                    cmd.Parameters[0].Value = fd.nameLoc;
                    cmd.Parameters[1].Value = fd.pidSvr;//fd_pid
                    cmd.Parameters[2].Value = fd.uid;//fd_uid
                    cmd.Parameters[3].Value = fd.lenLoc;//fd_length
                    cmd.Parameters[4].Value = fd.sizeLoc;//fd_size
                    cmd.Parameters[5].Value = fd.pathLoc;//fd_pathLoc
                    cmd.Parameters[6].Value = fd.pathSvr;//fd_pathSvr
                    cmd.Parameters[7].Value = fd.foldersCount;//fd_folders
                    cmd.Parameters[8].Value = fd.filesCount;//fd_files
                    cmd.Parameters[9].Value = fd.rootSign;//fd_pidRoot
                    cmd.Parameters[10].Value = fd.idSign;//fd_id
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}