using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using up7.db.biz.database;
using up7.down3.model;

namespace up7.down3.biz
{
    public class DnFile
    {
        public void Add(ref DnFileInf inf)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into down3_files(");
            sql.Append(" f_id");
            sql.Append(",f_uid");
            sql.Append(",f_mac");
            sql.Append(",f_nameLoc");
            sql.Append(",f_pathLoc");
            sql.Append(",f_fileUrl");
            sql.Append(",f_lenSvr");
            sql.Append(",f_sizeLoc");
            sql.Append(",f_sizeSvr");
            sql.Append(",f_fdTask");

            sql.Append(") values(");
            sql.Append(" @f_id");
            sql.Append(",@f_uid");
            sql.Append(",@f_mac");
            sql.Append(",@f_nameLoc");
            sql.Append(",@f_pathLoc");
            sql.Append(",@f_fileUrl");
            sql.Append(",@f_lenSvr");
            sql.Append(",@f_sizeLoc");
            sql.Append(",@f_sizeSvr");
            sql.Append(",@f_fdTask");
            sql.Append(");");

            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sql.ToString());
            db.AddString(ref cmd, "@f_id", inf.id, 32);
            db.AddInt(ref cmd, "@f_uid", inf.uid);
            db.AddString(ref cmd, "@f_mac", inf.mac,50);
            db.AddString(ref cmd, "@f_nameLoc", inf.nameLoc, 255);
            db.AddString(ref cmd, "@f_pathLoc", inf.pathLoc, 255);
            db.AddString(ref cmd, "@f_fileUrl", inf.fileUrl, 255);
            db.AddInt64(ref cmd, "@f_lenSvr", inf.lenSvr);
            db.AddString(ref cmd, "@f_sizeLoc", inf.sizeLoc, 10);
            db.AddString(ref cmd, "@f_sizeSvr", inf.sizeSvr, 10);
            db.AddBool(ref cmd, "@f_fdTask", inf.fdTask);
            db.ExecuteNonQuery(ref cmd);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fid"></param>
        public void Delete(string fid, int uid)
        {
            string sql = "delete from down3_files where f_id=@f_id and f_uid=@f_uid";
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sql);
            db.AddString(ref cmd, "@f_id", fid, 32);
            db.AddInt(ref cmd, "@f_uid", uid);
            db.ExecuteNonQuery(ref cmd);
        }

        public void process(string fid, int uid, string sizeLoc, string perLoc)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("update down3_files set ");
            sb.Append(" f_sizeLoc =@sizeLoc");
            sb.Append(",f_perLoc=@f_perLoc");
            sb.Append(" where");
            sb.Append(" f_id =@f_id and f_uid=@f_uid;");

            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sb.ToString());
            db.AddString(ref cmd, "@sizeLoc", sizeLoc, 10);
            db.AddString(ref cmd, "@f_perLoc", perLoc, 6);
            db.AddString(ref cmd, "@f_id", fid, 32);
            db.AddInt(ref cmd, "@f_uid", uid);
            db.ExecuteNonQuery(ref cmd);
        }

        static public void Clear()
        {
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand("delete from down3_files;");
            db.ExecuteNonQuery(ref cmd);
        }

        /// <summary>
        /// 列出未下载完的任务
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string all_uncmp(int uid)
        {
            List<DnFileInf> files = new List<DnFileInf>();
            StringBuilder sb = new StringBuilder();
            sb.Append("select ");
            sb.Append(" f_id");//0
            sb.Append(",f_nameLoc");//1
            sb.Append(",f_pathLoc");//2
            sb.Append(",f_perLoc");//3
            sb.Append(",f_sizeLoc");//4
            sb.Append(",f_fdTask");//5
            //
            sb.Append(" from down3_files");
            //
            sb.Append(" where f_uid=@f_uid ");

            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sb.ToString());
            db.AddInt(ref cmd, "@f_uid", uid);
            DbDataReader r = db.ExecuteReader(cmd);

            while (r.Read())
            {
                DnFileInf f = new DnFileInf();
                f.id = r.GetString(0);
                f.nameLoc = r.GetString(1);
                f.pathLoc = r.GetString(2);
                f.perLoc = r.GetString(3);
                f.sizeLoc = r.GetString(4);
                f.fdTask = r.GetBoolean(5);
                files.Add(f);
            }
            r.Close();

            if (files.Count > 0)
            {
                return JsonConvert.SerializeObject(files);
            }
            return string.Empty;
        }

        /// <summary>
        /// 从up7_files表中加载所有已经上传完毕的文件
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string all_complete(int uid)
        {
            List<DnFileInf> fs = new List<DnFileInf>();
            StringBuilder sb = new StringBuilder();
            sb.Append("select ");
            sb.Append(" f_id");//0
            sb.Append(",f_fdTask");//1
            sb.Append(",f_nameLoc");//2
            sb.Append(",f_sizeSvr");//3
            sb.Append(",f_lenSvr");//4
            sb.Append(",f_pathSvr");//5
            sb.Append(",f_blockPath");//6
            sb.Append(",fd_files");//7
            sb.Append(" from up7_files ");
            sb.Append(" left join up7_folders on up7_folders.fd_id=up7_files.f_id");
            //
            sb.Append(" where f_uid=@f_uid and f_deleted=0 and f_complete=1 and f_merged=1 and f_fdChild=0");

            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sb.ToString());
            db.AddInt(ref cmd, "@f_uid", uid);
            DbDataReader r = db.ExecuteReader(cmd);

            while (r.Read())
            {
                DnFileInf f = new DnFileInf();
                f.id = Guid.NewGuid().ToString("N");
                f.idFile = r.GetString(0);
                f.fdTask = r.GetBoolean(1);
                f.nameLoc = r.GetString(2);
                f.sizeLoc = "0byte";
                f.sizeSvr = r.GetString(3);
                f.lenSvr = r.GetInt64(4);
                f.pathSvr = r.GetString(5);
                f.blockPath = r.GetString(6);
                f.fileCount = r.IsDBNull(7) ? 0 : r.GetInt32(7);
                fs.Add(f);
            }
            r.Close();
            if (fs.Count < 1) return string.Empty;
            return JsonConvert.SerializeObject(fs);
        }
    }
}