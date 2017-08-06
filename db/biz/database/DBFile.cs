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
            sb.Append(" f_idSign");
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

            sb.Append(" @f_idSign");
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

            db.AddString(ref cmd, "@f_idSign", model.id, 36);
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
            sb.Append(" f_idSign");
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

            sb.Append(" @f_idSign");
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

            db.AddString(ref cmd, "@f_idSign", f.id, 36);
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
            sb.Append("select f_idSign,f_nameLoc,f_pathLoc,f_pathSvr,f_blockPath from up7_files where uid=@uid and f_complete=0 and f_delete=0;");
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

        public void delete(string id)
        {
            string sql = "update up7_files set f_deleted=1 where f_idSign=@f_id";
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sql);

            db.AddString(ref cmd, "@f_id", id,36);
            db.ExecuteNonQuery(cmd);
        }
    }
}