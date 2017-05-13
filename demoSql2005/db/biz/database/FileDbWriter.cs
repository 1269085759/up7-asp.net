using System.Data.Common;
using System.Text;

namespace up7.demoSql2005.db.biz.folder
{
    public class FileDbWriter
    {
        fd_root root;
        DbConnection con = null;

        public FileDbWriter(DbConnection con, fd_root fd)
        {
            this.con = con;
            this.root = fd;
        }

        DbCommand makeCmd(DbConnection con)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into up7_files(");
            sb.Append(" f_idSign");
            sb.Append(",f_pidSign");
            sb.Append(",f_rootSign");
            sb.Append(",f_fdChild");
            sb.Append(",f_uid");
            sb.Append(",f_nameLoc");
            sb.Append(",f_nameSvr");
            sb.Append(",f_pathLoc");
            sb.Append(",f_pathSvr");
            sb.Append(",f_lenLoc");
            sb.Append(",f_sizeLoc");
            sb.Append(",f_lenSvr");
            sb.Append(",f_perSvr");
            sb.Append(",f_sign");
            sb.Append(",f_complete");
            sb.Append(",f_fdTask");

            sb.Append(") values(");

            sb.Append(" @f_idSign");//f_idSign
            sb.Append(",@f_pidSign");//f_pidSign
            sb.Append(",@f_rootSign");//f_rootSign
            sb.Append(",@f_fdChild");//f_fdChild
            sb.Append(",@f_uid");//f_uid
            sb.Append(",@f_nameLoc");//f_nameLoc
            sb.Append(",@f_nameSvr");//f_nameSvr
            sb.Append(",@f_pathLoc");//f_pathLoc
            sb.Append(",@f_pathSvr");//f_pathSvr
            sb.Append(",@f_lenLoc");//f_lenLoc
            sb.Append(",@f_sizeLoc");//f_sizeLoc
            sb.Append(",@f_lenSvr");//f_lenSvr
            sb.Append(",@f_perSvr");//f_perSvr
            sb.Append(",@f_sign");//f_sign
            sb.Append(",1");//f_complete
            sb.Append(",@f_fdTask");//f_fdTask
            sb.Append(")");

            var cmd = con.CreateCommand();
            cmd.CommandText = sb.ToString();
            DbHelper db = new DbHelper();
            db.AddString(ref cmd, "@f_idSign", string.Empty, 36);
            db.AddString(ref cmd, "@f_pidSign", string.Empty, 36);
            db.AddString(ref cmd, "@f_rootSign", string.Empty, 36);
            db.AddBool(ref cmd, "@f_fdChild",false);
            db.AddInt(ref cmd, "@f_uid", 0);
            db.AddString(ref cmd, "@f_nameLoc", string.Empty, 36);
            db.AddString(ref cmd, "@f_nameSvr", string.Empty, 36);
            db.AddString(ref cmd, "@f_pathLoc", string.Empty, 36);
            db.AddString(ref cmd, "@f_pathSvr", string.Empty, 36);
            db.AddInt64(ref cmd, "@f_lenLoc", 0);
            db.AddString(ref cmd, "@f_sizeLoc", string.Empty, 36);
            db.AddInt64(ref cmd, "@f_lenSvr",0);
            db.AddString(ref cmd, "@f_perSvr", string.Empty, 36);
            db.AddString(ref cmd, "@f_sign", string.Empty, 36);
            db.AddBool(ref cmd, "@f_fdTask", false);
            return cmd;
        }

        public void save()
        {
            if (this.root.files == null) return;
            if (this.root.files.Count < 1) return;
            using (var cmd = this.makeCmd(con))
            {

                //写根目录
                cmd.Parameters[0].Value = this.root.idSign;
                cmd.Parameters[1].Value = this.root.pidSign;
                cmd.Parameters[2].Value = string.Empty;
                cmd.Parameters[3].Value = false;
                cmd.Parameters[4].Value = this.root.uid;
                cmd.Parameters[5].Value = this.root.nameLoc;
                cmd.Parameters[6].Value = this.root.nameSvr;
                cmd.Parameters[7].Value = this.root.pathLoc;
                cmd.Parameters[8].Value = this.root.pathSvr;
                cmd.Parameters[9].Value = this.root.lenLoc;
                cmd.Parameters[10].Value = this.root.sizeLoc;
                cmd.Parameters[11].Value = this.root.lenLoc;
                cmd.Parameters[12].Value = "100%";
                cmd.Parameters[13].Value = this.root.sign;
                cmd.Parameters[14].Value = true;
                cmd.ExecuteNonQuery();

                //写子文件列表
                foreach (var f in this.root.files)
                {
                    cmd.Parameters[0].Value = f.idSign;//idSign
                    cmd.Parameters[1].Value = string.IsNullOrEmpty(f.pidSign) ? string.Empty : f.pidSign;//pidSign
                    cmd.Parameters[2].Value = string.IsNullOrEmpty(f.rootSign) ?string.Empty:f.rootSign;//rootSign
                    cmd.Parameters[3].Value = true;//fdChild
                    cmd.Parameters[4].Value = f.uid;//uid
                    cmd.Parameters[5].Value = f.nameLoc;//nameLoc
                    cmd.Parameters[6].Value = f.nameSvr;//nameSvr
                    cmd.Parameters[7].Value = f.pathLoc;//pathLoc
                    cmd.Parameters[8].Value = f.pathSvr;//pathSvr
                    cmd.Parameters[9].Value = f.lenLoc;//lenLoc
                    cmd.Parameters[10].Value = f.sizeLoc;//sizeLoc
                    cmd.Parameters[11].Value = f.lenLoc;//lenSvr
                    cmd.Parameters[12].Value = "100%";//perSvr
                    cmd.Parameters[13].Value = string.IsNullOrEmpty(f.sign)?string.Empty:f.sign;//sign
                    cmd.Parameters[14].Value = false;//fdTask
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}