using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Newtonsoft.Json;
using up7.db.model;

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

        public void addComplete(ref FileInf model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into up7_files(");
            sb.Append(" f_id");
            sb.Append(",f_uid");
            sb.Append(",f_nameLoc");
            sb.Append(",f_nameSvr");
            sb.Append(",f_pathLoc");
            sb.Append(",f_pathSvr");
            sb.Append(",f_pathRel");
            sb.Append(",f_blockSize");
            sb.Append(",f_blockPath");
            sb.Append(",f_lenLoc");
            sb.Append(",f_sizeLoc");
            sb.Append(",f_lenSvr");
            sb.Append(",f_perSvr");
            sb.Append(",f_complete");

            sb.Append(") values (");

            sb.Append(" @f_id");
            sb.Append(",@f_uid");
            sb.Append(",@f_nameLoc");
            sb.Append(",@f_nameSvr");
            sb.Append(",@f_pathLoc");
            sb.Append(",@f_pathSvr");
            sb.Append(",@f_pathRel");
            sb.Append(",@f_blockSize");
            sb.Append(",@f_blockPath");
            sb.Append(",@f_lenLoc");
            sb.Append(",@f_sizeLoc");
            sb.Append(",@f_lenLoc");
            sb.Append(",'100%'");
            sb.Append(",1");
            sb.Append(") ;");

            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sb.ToString());

            db.AddString(ref cmd, "@f_id", model.id, 32);
            db.AddInt(ref cmd, "@f_uid", model.uid);
            db.AddString(ref cmd, "@f_nameLoc", model.nameLoc, 255);
            db.AddString(ref cmd, "@f_nameSvr", model.nameSvr, 255);
            db.AddString(ref cmd, "@f_pathLoc", model.pathLoc, 512);
            db.AddString(ref cmd, "@f_pathSvr", model.pathSvr, 512);
            db.AddString(ref cmd, "@f_pathRel", model.pathRel, 512);
            db.AddInt(ref cmd, "@f_blockSize", model.blockSize);
            db.AddString(ref cmd, "@f_blockPath", model.blockPath, 512);
            db.AddInt64(ref cmd, "@f_lenLoc", model.lenLoc);
            db.AddString(ref cmd, "@f_sizeLoc", model.sizeLoc, 15);

            db.ExecuteNonQuery(cmd);
        }
        public void add(ref FileInf f)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into up7_files(");
            sb.Append(" f_id");
            sb.Append(",f_uid");
            sb.Append(",f_nameLoc");
            sb.Append(",f_nameSvr");
            sb.Append(",f_pathLoc");
            sb.Append(",f_pathSvr");
            sb.Append(",f_pathRel");
            sb.Append(",f_blockCount");
            sb.Append(",f_blockSize");
            sb.Append(",f_blockPath");
            sb.Append(",f_lenLoc");
            sb.Append(",f_sizeLoc");

            sb.Append(") values (");

            sb.Append(" @f_id");
            sb.Append(",@f_uid");
            sb.Append(",@f_nameLoc");
            sb.Append(",@f_nameSvr");
            sb.Append(",@f_pathLoc");
            sb.Append(",@f_pathSvr");
            sb.Append(",@f_pathRel");
            sb.Append(",@f_blockCount");
            sb.Append(",@f_blockSize");
            sb.Append(",@f_blockPath");
            sb.Append(",@f_lenLoc");
            sb.Append(",@f_sizeLoc");
            sb.Append(") ;");

            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sb.ToString());

            db.AddString(ref cmd, "@f_id", f.id, 32);
            db.AddInt(ref cmd, "@f_uid", f.uid);
            db.AddString(ref cmd, "@f_nameLoc", f.nameLoc, 255);
            db.AddString(ref cmd, "@f_nameSvr", f.nameSvr, 255);
            db.AddString(ref cmd, "@f_pathLoc", f.pathLoc, 512);
            db.AddString(ref cmd, "@f_pathSvr", f.pathSvr, 512);
            db.AddString(ref cmd, "@f_pathRel", f.pathRel, 512);
            db.AddInt(ref cmd, "@f_blockCount", f.blockCount);
            db.AddInt(ref cmd, "@f_blockSize", f.blockSize);
            db.AddString(ref cmd, "@f_blockPath", f.blockPath, 512);
            db.AddInt64(ref cmd, "@f_lenLoc", f.lenLoc);
            db.AddString(ref cmd, "@f_sizeLoc", f.sizeLoc, 15);

            db.ExecuteNonQuery(cmd);
        }

        public string all_uncmp(int uid)
        {
            List<FileInf> files = new List<FileInf>();
            StringBuilder sb = new StringBuilder();
            sb.Append("select f_id,f_nameLoc,f_pathLoc,f_pathSvr,f_blockPath from up7_files where uid=@uid and f_complete=0 and f_delete=0;");
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sb.ToString());

            db.AddInInt32(cmd, "@uid", uid);

            var r = db.ExecuteReader(cmd);
            while(r.Read())
            {
                FileInf f = new FileInf();
                f.id = r.GetString(0);
                f.nameLoc = r.GetString(1);
                f.pathLoc = r.GetString(2);
                f.pathSvr = r.GetString(3);
                f.blockPath = r.GetString(4);
                files.Add(f);
            }
            r.Close();

            if (files.Count < 1) return string.Empty;
            return JsonConvert.SerializeObject(files);
        }

        /// <summary>
        /// 所有已经上传完毕且合并完毕的文件和文件夹
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string all_cmp(string uid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select ");
            sb.Append(" f_id");//0
            sb.Append(",f_nameLoc");//1
            sb.Append(",f_lenLoc");//2
            sb.Append(",f_sizeLoc");//3
            sb.Append(",f_fdTask");//4
            sb.Append(",f_pathLoc");//5
            sb.Append(",f_pathSvr");//6
            sb.Append(",f_blockSize");//7
            sb.Append(",f_blockPath");//8
            sb.Append(",f_blockCount");//9
            sb.Append(",fd_files");//10
                                   //
            sb.Append(" from up7_files");
            sb.Append(" left join up7_folders on up7_folders.fd_id=up7_files.f_id");
            //
            sb.Append(" where f_uid=@uid and f_complete=1 and f_fdChild=0");

            DbHelper db = new DbHelper();
            var cmd = db.GetCommand(sb.ToString());
            db.AddInt(ref cmd, "@uid", int.Parse(uid) );
            List<FileInf> files = new List<FileInf>();
            using (var r = db.ExecuteReader(cmd))
            {
                while (r.Read())
                {
                    FileInf fi = new FileInf();
                    fi.id = r.GetString(0);//与up7_files表对应
                    fi.nameLoc = r.GetString(1);
                    fi.lenSvr = r.GetInt64(2);
                    fi.sizeSvr = r.GetString(3);
                    fi.fdTask = r.GetBoolean(4);
                    fi.pathLoc = r.GetString(5);
                    fi.pathSvr = r.GetString(6);
                    //如果是文件夹则pathSvr保存本地路径，用来替换
                    if (fi.fdTask) fi.pathSvr = fi.pathLoc;
                    fi.blockSize = r.GetInt32(7);
                    fi.blockPath = r.GetString(8);
                    fi.blockCount = r.GetInt32(9);
                    fi.fileCount = r.IsDBNull(10) ? 0 : r.GetInt32(10);
                    files.Add(fi);
                }
            }

            return JsonConvert.SerializeObject(files);
        }

        public void delete(string id)
        {
            string sql = "update up7_files set f_deleted=1 where f_id=@f_id";
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sql);

            db.AddString(ref cmd, "@f_id", id,32);
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