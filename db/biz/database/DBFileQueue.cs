using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using up7.db.model;

namespace up7.db.biz.database
{
    /// <summary>
    /// 队列表，保存未上传完的任务列表(文件，文件夹)
    /// 文件或文件夹上传完毕后才会保存到up7_files中。
    /// 未上传完的数据和已上传完毕的数据区分开
    /// </summary>
    public class DBFileQueue
    {
        public FileInf read(string id)
        {
            FileInf f = new FileInf();
            string sql = "select * from up7_files where f_id=@id;";
            DbHelper db = new DbHelper();
            var cmd = db.GetCommand(sql);
            db.AddString(ref cmd, "@id", id, 32);
            var r = db.ExecuteReader(cmd);
            r.Read();
            f.id = id;
            f.pathSvr = r.GetString(9);
            f.lenLoc = r.GetInt64(12);
            f.blockPath = r.GetString(17);
            r.Close();
            return f;
        }

        public void remove(string id)
        {
            string sql = "delete from up7_files where f_id=@id;";
            DbHelper db = new DbHelper();
            var cmd = db.GetCommand(sql);
            db.AddString(ref cmd, "@id", id, 32);
            db.ExecuteNonQuery(cmd);
        }

        public void add(ref FileInf f)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into up7_files_queue(");
            sb.Append(" f_id");
            sb.Append(",f_fdTask");
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
            sb.Append(",@f_fdTask");
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

            db.AddString(ref cmd, "@f_id", f.id, 36);
            db.AddBool(ref cmd, "@f_fdTask", f.fdTask);
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

        public void complete(string id)
        {
            string sql = "insert into up7_files select * from up7_files_queue where f_id=@id;" +
                "delete from up7_files_queue where f_id=@id;";
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sql);
            db.AddString(ref cmd, "@id", id, 32);
            db.ExecuteNonQuery(cmd);
        }

        public void process(string id,string perSvr)
        {
            string sql = "update up7_files_queue set f_perSvr=@perSvr where f_id=@id;";
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sql);
            db.AddString(ref cmd, "@perSvr", perSvr, 6);
            db.AddString(ref cmd, "@id", id, 32);
            db.ExecuteNonQuery(cmd);
        }

        public string all_uncmp(int uid)
        {
            List<FileInf> files = new List<FileInf>();
            StringBuilder sb = new StringBuilder();
            sb.Append("select f_id,f_fdTask,f_nameLoc,f_pathLoc,f_pathSvr,f_blockPath,f_sizeLoc,f_perSvr from up7_files_queue where f_uid=@uid and f_complete=0 and f_deleted=0;");
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sb.ToString());

            db.AddInInt32(cmd, "@uid", uid);

            var r = db.ExecuteReader(cmd);
            while (r.Read())
            {
                FileInf f = new FileInf();
                f.id = r.GetString(0);
                f.fdTask = r.GetBoolean(1);
                f.nameLoc = r.GetString(2);
                f.pathLoc = r.GetString(3);
                f.pathSvr = r.GetString(4);
                f.blockPath = r.GetString(5);
                f.sizeLoc = r.GetString(6);
                f.perSvr = r.GetString(7);
                files.Add(f);
            }
            r.Close();

            if (files.Count < 1) return string.Empty;
            return JsonConvert.SerializeObject(files);
        }

        public static void clear()
        {
            string sql = "delete from up7_files_queue ;";
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sql);
            db.ExecuteNonQuery(cmd);
        }
    }
}