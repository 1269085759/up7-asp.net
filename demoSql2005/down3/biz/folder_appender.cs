using System.Collections.Generic;
using System.Data.Common;
using up7.demoSql2005.db;
using up7.demoSql2005.down3.model;

namespace up7.demoSql2005.down3.biz
{
    public class folder_appender
    {
        public folder_appender()
        {
        }

        public void add(ref DnFolderInf fd)
        {
            string sql = "fd_add_batch";
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommandStored(sql);
            db.AddInt(ref cmd, "@f_count", fd.files.Count+1);//单独增加一个文件夹
            db.AddInt(ref cmd, "@uid", fd.uid);

            cmd.Connection.Open();
            var r = cmd.ExecuteReader();
            List<string> id_lst = new List<string>();
            while (r.Read())
            {
                id_lst.Add(r.GetInt32(0).ToString());
            }
            r.Close();
            cmd.Parameters.Clear();
            string[] ids = id_lst.ToArray();

            //批量更新文件
            sql = "update down3_files set ";
            sql += " f_nameLoc=@f_nameLoc";
            sql += ",f_pathLoc=@f_pathLoc";
            sql += ",f_fileUrl=@f_fileUrl";
            sql += ",f_lenSvr=@f_lenSvr";
            sql += ",f_sizeSvr=@f_sizeSvr";
            sql += ",f_pidRoot=@f_pidRoot";
            sql += ",f_fdTask=@f_fdTask";
            sql += " where f_id=@f_id";
            cmd.CommandText = sql;
            cmd.CommandType = System.Data.CommandType.Text;
            db.AddString(ref cmd, "@f_nameLoc", "", 255);
            db.AddString(ref cmd, "@f_pathLoc", "", 255);
            db.AddString(ref cmd, "@f_fileUrl", "", 255);
            db.AddInt64(ref cmd, "@f_lenSvr", 0);
            db.AddString(ref cmd, "@f_sizeSvr", "", 10);
            db.AddInt(ref cmd, "@f_pidRoot", 0);
            db.AddBool(ref cmd, "@f_fdTask",false);
            db.AddInt(ref cmd, "@f_id", 0);
            cmd.Prepare();

            //String[] ids = f_ids.Split(',');
            System.Diagnostics.Debug.Write("ids总数:"+ids.Length+"\n");
            System.Diagnostics.Debug.Write("files总数:"+fd.files.Count+"\n");
                        
            //更新文件夹
            fd.idSvr = int.Parse( ids[0]);
            fd.folder = true;
            this.update_file(ref cmd, fd);

            //更新文件
            for(int i = 1,f_index=0 , l = ids.Length;i< l;++i,++f_index)
            {
                fd.files[f_index].idSvr = int.Parse(ids[i]);
                fd.files[f_index].pidRoot = fd.idSvr;

                this.update_file(ref cmd, fd.files[f_index]);
            }
            cmd.Connection.Close();
        }

        void update_file(ref DbCommand cmd,DnFileInf f)
        {
            cmd.Parameters[0].Value = f.nameLoc;
            cmd.Parameters[1].Value = f.pathLoc;
            cmd.Parameters[2].Value = f.fileUrl;
            cmd.Parameters[3].Value = f.lenSvr;
            cmd.Parameters[4].Value = f.sizeSvr;
            cmd.Parameters[5].Value = f.pidRoot;
            cmd.Parameters[6].Value = f.folder;
            cmd.Parameters[7].Value = f.idSvr;
            cmd.ExecuteNonQuery();
        }
    }
}