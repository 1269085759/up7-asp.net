using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using up7.db.model;

namespace up7.db.biz.database
{
    public class DBFileQueue
    {
        public void add(ref xdb_files f)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into up7_files_queue(");
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

            db.AddString(ref cmd, "@f_id", f.id, 36);
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
            string sql = "select * into up7_files from up7_files_queue where f_id=@id;" +
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
            List<xdb_files> files = new List<xdb_files>();
            StringBuilder sb = new StringBuilder();
            sb.Append("select f_id,f_nameLoc,f_pathLoc,f_pathSvr,f_blockPath,f_sizeLoc,f_perSvr from up7_files_queue where f_uid=@uid and f_complete=0 and f_deleted=0;");
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sb.ToString());

            db.AddInInt32(cmd, "@uid", uid);

            var r = db.ExecuteReader(cmd);
            while (r.Read())
            {
                xdb_files f = new xdb_files();
                f.id = r.GetString(0);
                f.nameLoc = r.GetString(1);
                f.pathLoc = r.GetString(2);
                f.pathSvr = r.GetString(3);
                f.blockPath = r.GetString(4);
                f.sizeLoc = r.GetString(5);
                f.perSvr = r.GetString(6);
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