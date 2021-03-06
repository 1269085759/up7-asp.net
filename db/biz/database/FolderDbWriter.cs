﻿using System.Data.Common;
using System.Text;
using up7.db.model;

namespace up7.db.biz.database
{
    public class FolderDbWriter
    {
        FolderInf root;//根目录
        DbConnection con = null;

        public FolderDbWriter(DbConnection con, FolderInf fd)
        {
            this.con = con;
            this.root = fd;
        }

        DbCommand makeCmd(DbConnection con)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into up7_folders(");
            sb.Append(" fd_id");
            sb.Append(",fd_name");
            sb.Append(",fd_pid");
            sb.Append(",fd_uid");
            sb.Append(",fd_length");
            sb.Append(",fd_size");
            sb.Append(",fd_pathLoc");
            sb.Append(",fd_pathSvr");
            sb.Append(",fd_folders");
            sb.Append(",fd_files");
            sb.Append(",fd_pidRoot");

            sb.Append(") values(");
            sb.Append(" @fd_id");
            sb.Append(",@fd_name");
            sb.Append(",@fd_pid");
            sb.Append(",@fd_uid");
            sb.Append(",@fd_length");
            sb.Append(",@fd_size");
            sb.Append(",@fd_pathLoc");
            sb.Append(",@fd_pathSvr");
            sb.Append(",@fd_folders");
            sb.Append(",@fd_files");
            sb.Append(",@fd_pidRoot");
            sb.Append(")");

            DbHelper db = new DbHelper();
            var cmd = con.CreateCommand();
            cmd.CommandText = sb.ToString();
            db.AddString(ref cmd, "@fd_id", string.Empty, 512);
            db.AddString(ref cmd, "@fd_name", string.Empty, 50);
            db.AddString(ref cmd, "@Sign", string.Empty, 36);
            db.AddInt(ref cmd, "@fd_uid", 0);
            db.AddInt(ref cmd, "@fd_length", 0);
            db.AddString(ref cmd, "@fd_size", string.Empty,50);
            db.AddString(ref cmd, "@fd_pathLoc", string.Empty, 512);
            db.AddString(ref cmd, "@fd_pathSvr", string.Empty, 512);
            db.AddInt(ref cmd, "@fd_folders", 0);
            db.AddInt(ref cmd, "@fd_files", 0);
            db.AddString(ref cmd, "@fd_pidRoot", string.Empty, 512);

            return cmd;
        }

        public void save()
        {
            using (var cmd = this.makeCmd(con))
            {

                //写根目录
                cmd.Parameters[0].Value = this.root.id;
                cmd.Parameters[1].Value = this.root.nameLoc;//
                cmd.Parameters[2].Value = this.root.pid;//
                cmd.Parameters[3].Value = this.root.uid;//
                cmd.Parameters[4].Value = this.root.lenLoc;//
                cmd.Parameters[5].Value = this.root.sizeLoc;//
                cmd.Parameters[6].Value = this.root.pathLoc;//
                cmd.Parameters[7].Value = this.root.pathSvr;//
                cmd.Parameters[8].Value = 0;//
                cmd.Parameters[9].Value = this.root.fileCount;//
                cmd.Parameters[10].Value = this.root.pidRoot;//
                cmd.ExecuteNonQuery();

                //if (this.root.folders == null) return;
                //if (this.root.folders.Count < 1) return;
                //写子目录列表
                //foreach (var fd in this.root.folders)
                //{
                //    cmd.Parameters[0].Value = fd.id;
                //    cmd.Parameters[1].Value = fd.nameLoc;//
                //    cmd.Parameters[2].Value = fd.pid;//fd_uid
                //    cmd.Parameters[3].Value = fd.uid;//fd_length
                //    cmd.Parameters[4].Value = fd.lenLoc;//fd_size
                //    cmd.Parameters[5].Value = fd.sizeLoc;//fd_pathLoc
                //    cmd.Parameters[6].Value = fd.pathLoc;//fd_pathSvr
                //    cmd.Parameters[7].Value = fd.pathSvr;//fd_folders
                //    //cmd.Parameters[8].Value = fd.folderCount;//fd_files
                //    cmd.Parameters[9].Value = fd.fileCount;//Root
                //    cmd.Parameters[10].Value = fd.pidRoot;//fd_id
                //    cmd.ExecuteNonQuery();
                //}
            }
        }
    }
}