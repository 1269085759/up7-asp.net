using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using up7.db.model;

namespace up7.db.biz.database
{
    /// <summary>
    /// 数据库访问操作
    /// 更新记录：
    ///		2012-04-10 创建
    ///		2014-03-11 将OleDb对象全部改为使用DbHelper对象，简化代码。
    ///		2018-09-07 增加逻辑，与up7_files_queue表合并
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

        public static void merged(string id)
        {
            string sql = "update up7_files set f_lenSvr=f_lenLoc,f_perSvr='100%',f_merged=1 where f_id=@id";
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sql);

            db.AddString(ref cmd, "@id", id, 32);
            db.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 添加一条数据
        /// </summary>
        public static void add(ref FileInf f)
        {
            string sql = @"
                insert into up7_files(
                 f_id
                ,f_fdTask
                ,f_uid
                ,f_nameLoc
                ,f_nameSvr
                ,f_pathLoc
                ,f_pathSvr
                ,f_pathRel
                ,f_blockCount
                ,f_blockSize
                ,f_blockPath
                ,f_lenLoc
                ,f_sizeLoc
                ) values (
                 @f_id
                ,@f_fdTask
                ,@f_uid
                ,@f_nameLoc
                ,@f_nameSvr
                ,@f_pathLoc
                ,@f_pathSvr
                ,@f_pathRel
                ,@f_blockCount
                ,@f_blockSize
                ,@f_blockPath
                ,@f_lenLoc
                ,@f_sizeLoc
                ) ";

            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sql);

            db.AddString(ref cmd, "@f_id", f.id, 36);
            db.AddBool(ref cmd  , "@f_fdTask", f.fdTask);
            db.AddInt(ref cmd   , "@f_uid", f.uid);
            db.AddString(ref cmd, "@f_nameLoc", f.nameLoc, 255);
            db.AddString(ref cmd, "@f_nameSvr", f.nameSvr, 255);
            db.AddString(ref cmd, "@f_pathLoc", f.pathLoc, 512);
            db.AddString(ref cmd, "@f_pathSvr", f.pathSvr, 512);
            db.AddString(ref cmd, "@f_pathRel", f.pathRel, 512);
            db.AddInt(ref cmd   , "@f_blockCount", f.blockCount);
            db.AddInt(ref cmd   , "@f_blockSize", f.blockSize);
            db.AddString(ref cmd, "@f_blockPath", f.blockPath, 512);
            db.AddInt64(ref cmd , "@f_lenLoc", f.lenLoc);
            db.AddString(ref cmd, "@f_sizeLoc", f.sizeLoc, 15);

            db.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        public static void AddBatch(ref List<FileInf> arr)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into up7_files(");
            sql.Append(" f_id");
            sql.Append(",f_pathSvr");
            sql.Append(",f_pathRel");
            sql.Append(",f_md5");
            sql.Append(",f_lenLoc");
            sql.Append(",f_sizeLoc");
            sql.Append(",f_pos");
            sql.Append(",f_blockCount");
            sql.Append(",f_blockSize");
            sql.Append(",f_blockPath");
            sql.Append(",f_lenSvr");
            sql.Append(",f_pid");
            sql.Append(",f_perSvr");
            sql.Append(",f_complete");
            sql.Append(",f_time");
            sql.Append(",f_deleted");
            sql.Append(",f_merged");
            sql.Append(",f_pidRoot");
            sql.Append(",f_fdTask");
            sql.Append(",f_fdChild");
            sql.Append(",f_uid");
            sql.Append(",f_nameLoc");
            sql.Append(",f_nameSvr");
            sql.Append(",f_pathLoc");

            sql.Append(") values (");

            sql.Append(" @f_id");
            sql.Append(",@f_pathSvr");
            sql.Append(",@f_pathRel");
            sql.Append(",@f_md5");
            sql.Append(",@f_lenLoc");
            sql.Append(",@f_sizeLoc");
            sql.Append(",@f_pos");
            sql.Append(",@f_blockCount");
            sql.Append(",@f_blockSize");
            sql.Append(",@f_blockPath");
            sql.Append(",@f_lenSvr");
            sql.Append(",@f_pid");
            sql.Append(",@f_perSvr");
            sql.Append(",@f_complete");
            sql.Append(",@f_time");
            sql.Append(",@f_deleted");
            sql.Append(",@f_merged");
            sql.Append(",@f_pidRoot");
            sql.Append(",@f_fdTask");
            sql.Append(",@f_fdChild");
            sql.Append(",@f_uid");
            sql.Append(",@f_nameLoc");
            sql.Append(",@f_nameSvr");
            sql.Append(",@f_pathLoc");
            sql.Append(") ");
            DbHelper db = new DbHelper();
            var cmd = db.GetCommand(sql.ToString());

            db.AddString(ref cmd, "@f_id", string.Empty, 32);
            db.AddString(ref cmd, "@f_pathSvr", string.Empty, 512);
            db.AddString(ref cmd, "@f_pathRel", string.Empty, 512);
            db.AddString(ref cmd, "@f_md5", string.Empty, 40);
            db.AddString(ref cmd, "@f_sizeLoc", string.Empty, 15);
            db.AddInt(ref cmd, "@f_blockCount", 0);
            db.AddInt(ref cmd, "@f_blockSize", 0);
            db.AddString(ref cmd, "@f_blockPath", string.Empty, 2000);
            db.AddString(ref cmd, "@f_pid", string.Empty, 32);
            db.AddString(ref cmd, "@f_perSvr", string.Empty, 6);
            db.AddString(ref cmd, "@f_pidRoot", string.Empty, 32);
            db.AddInt(ref cmd, "@f_uid", 0);
            db.AddString(ref cmd, "@f_nameLoc", string.Empty, 255);
            db.AddString(ref cmd, "@f_nameSvr", string.Empty, 255);
            db.AddString(ref cmd, "@f_pathLoc", string.Empty, 512);
            cmd.Connection.Open();
            cmd.Prepare();

            //
            foreach (FileInf a in arr)
            {
                cmd.Parameters[0].Value = a.id;
                cmd.Parameters[1].Value = a.pathSvr;
                cmd.Parameters[2].Value = a.pathRel;
                cmd.Parameters[3].Value = a.md5;
                cmd.Parameters[4].Value = a.lenLoc;
                cmd.Parameters[5].Value = a.sizeLoc;
                cmd.Parameters[6].Value = 0;
                cmd.Parameters[7].Value = a.blockCount;
                cmd.Parameters[8].Value = a.blockSize;
                cmd.Parameters[9].Value = a.blockPath;
                cmd.Parameters[10].Value = a.lenSvr;
                cmd.Parameters[11].Value = a.pid;
                cmd.Parameters[12].Value = a.perSvr;
                cmd.Parameters[13].Value = a.complete;
                cmd.Parameters[14].Value = a.time;
                cmd.Parameters[15].Value = a.deleted;
                cmd.Parameters[16].Value = false;
                cmd.Parameters[17].Value = a.pidRoot;
                cmd.Parameters[18].Value = a.fdTask;
                cmd.Parameters[19].Value = a.f_fdChild;
                cmd.Parameters[20].Value = a.uid;
                cmd.Parameters[21].Value = a.nameLoc;
                cmd.Parameters[22].Value = a.nameSvr;
                cmd.Parameters[23].Value = a.pathLoc;
                cmd.ExecuteNonQuery();
            }
            cmd.Connection.Close();
        }

        public static void update(ref FileInf m)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update up7_files set");
            sql.Append(" f_pathSvr=@f_pathSvr");
            sql.Append(",f_pathRel=@f_pathRel");
            sql.Append(",f_md5=@f_md5");
            sql.Append(",f_lenLoc=@f_lenLoc");
            sql.Append(",f_sizeLoc=@f_sizeLoc");
            sql.Append(",f_blockCount=@f_blockCount");
            sql.Append(",f_blockSize=@f_blockSize");
            sql.Append(",f_blockPath=@f_blockPath");
            sql.Append(",f_lenSvr=@f_lenSvr");
            sql.Append(",f_pid=@f_pid");
            sql.Append(",f_perSvr=@f_perSvr");
            sql.Append(",f_deleted=@f_deleted");
            sql.Append(",f_pidRoot=@f_pidRoot");
            sql.Append(",f_fdTask=@f_fdTask");
            sql.Append(",f_fdChild=@f_fdChild");
            sql.Append(",f_uid=@f_uid");
            sql.Append(",f_nameLoc=@f_nameLoc");
            sql.Append(",f_nameSvr=@f_nameSvr");
            sql.Append(",f_pathLoc=@f_pathLoc");
            sql.Append(" where f_id=@f_id ");

            DbHelper db = new DbHelper();
            var cmd = db.GetCommand(sql.ToString());

            db.AddString(ref cmd, "@f_pathSvr", m.pathSvr, 512);
            db.AddString(ref cmd, "@f_pathRel", m.pathRel, 512);
            db.AddString(ref cmd, "@f_md5", m.md5, 40);
            db.AddInt64(ref cmd, "@f_lenLoc", m.lenLoc);
            db.AddString(ref cmd, "@f_sizeLoc", m.sizeLoc, 15);
            db.AddInt(ref cmd, "@f_blockCount", m.blockCount);
            db.AddInt(ref cmd, "@f_blockSize", m.blockSize);
            db.AddString(ref cmd, "@f_blockPath", m.blockPath, 2000);
            db.AddInt64(ref cmd, "@f_lenSvr", m.lenSvr);
            db.AddString(ref cmd, "@f_pid", m.pid, 32);
            db.AddString(ref cmd, "@f_perSvr", m.perSvr, 6);
            db.AddBool(ref cmd, "@f_deleted", m.deleted);
            db.AddString(ref cmd, "@f_pidRoot", m.pidRoot, 32);
            db.AddBool(ref cmd, "@f_fdTask", m.fdTask);
            db.AddBool(ref cmd, "@f_fdChild", m.f_fdChild);
            db.AddInt(ref cmd, "@f_uid", m.uid);
            db.AddString(ref cmd, "@f_nameLoc", m.nameLoc, 255);
            db.AddString(ref cmd, "@f_nameSvr", m.nameSvr, 255);
            db.AddString(ref cmd, "@f_pathLoc", m.pathLoc, 512);
            db.AddString(ref cmd, "@f_id", m.id, 32);
            db.ExecuteNonQuery(ref cmd);
        }

        public static void remove(string f_id)
        {
            string sql = "update up7_files set f_deleted=1 where f_id=@f_id ";

            DbHelper db = new DbHelper();
            var cmd = db.GetCommand(sql);

            db.AddString(ref cmd, "@f_id", f_id,32);
            db.ExecuteNonQuery(ref cmd);
        }

        public static void complete(string id)
        {
            string sql = "update up7_files set f_complete=1 where f_id=@id";
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sql);
            db.AddString(ref cmd, "@id", id, 32);
            db.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 取的有未上传完的文件和文件夹
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static string all_uncmp(int uid)
        {
            List<FileInf> files = new List<FileInf>();
            string sql = @"select f_id
                            ,f_fdTask
                            ,f_nameLoc
                            ,f_pathLoc
                            ,f_pathSvr
                            ,f_blockPath
                            ,f_sizeLoc
                            ,f_perSvr 
                            from up7_files
                            where f_uid=@uid 
                                and f_complete=0 
                                and f_deleted=0;";
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sql);

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

        /// <summary>
        /// 删除列表
        /// </summary>
        public static bool DeleteList(string f_idlist)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("delete from up7_files ");
            sql.Append(" where ID in (" + f_idlist + ")  ");
            DbHelper db = new DbHelper();
            var cmd = db.GetCommand(sql.ToString());
            int rows = db.ExecuteNonQuery(cmd);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool read(ref FileInf m)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.Append("select top 1 ");
            sql.Append(" f_pathSvr");
            sql.Append(",f_pathRel");
            sql.Append(",f_md5");
            sql.Append(",f_lenLoc");
            sql.Append(",f_sizeLoc");
            sql.Append(",f_pos");
            sql.Append(",f_blockCount");
            sql.Append(",f_blockSize");
            sql.Append(",f_blockPath");
            sql.Append(",f_lenSvr");
            sql.Append(",f_pid");
            sql.Append(",f_perSvr");
            sql.Append(",f_complete");
            sql.Append(",f_time");
            sql.Append(",f_deleted");
            sql.Append(",f_merged");
            sql.Append(",f_pidRoot");
            sql.Append(",f_fdTask");
            sql.Append(",f_fdChild");
            sql.Append(",f_uid");
            sql.Append(",f_nameLoc");
            sql.Append(",f_nameSvr");
            sql.Append(",f_pathLoc");
            sql.Append(" from up7_files where f_id=@f_id ");

            DbHelper db = new DbHelper();
            var cmd = db.GetCommand(sql.ToString());
            db.AddString(ref cmd, "@f_id", m.id, 32);
            var r = db.ExecuteReader(cmd);

            if (r.Read())
            {
                m.pathSvr    = r.IsDBNull(0) ? string.Empty : r.GetString(0);
                m.pathRel    = r.IsDBNull(1) ? string.Empty : r.GetString(1);
                m.md5        = r.IsDBNull(2) ? string.Empty : r.GetString(2);
                m.lenLoc     = r.IsDBNull(3) ? 0 : r.GetInt64(3);
                m.sizeLoc    = r.IsDBNull(4) ? string.Empty : r.GetString(4);
                m.blockCount = r.IsDBNull(6) ? 0 : r.GetInt32(6);
                m.blockSize  = r.IsDBNull(7) ? 0 : r.GetInt32(7);
                m.blockPath  = r.IsDBNull(8) ? string.Empty : r.GetString(8);
                m.lenSvr     = r.IsDBNull(9) ? 0 : r.GetInt64(9);
                m.pid        = r.IsDBNull(10) ? string.Empty : r.GetString(10);
                m.perSvr     = r.IsDBNull(11) ? string.Empty : r.GetString(11);
                m.complete   = r.IsDBNull(12) ? false : r.GetBoolean(12);
                m.time       = r.IsDBNull(13) ? DateTime.MinValue : r.GetDateTime(13);
                m.deleted    = r.IsDBNull(14) ? false : r.GetBoolean(14);
                //m.merged   = r.IsDBNull(15) ? false : r.GetBoolean(15);
                m.pidRoot    = r.IsDBNull(16) ? string.Empty : r.GetString(16);
                m.fdTask     = r.IsDBNull(17) ? false : r.GetBoolean(17);
                m.f_fdChild  = r.IsDBNull(18) ? false : r.GetBoolean(18);
                m.uid        = r.IsDBNull(19) ? 0 : r.GetInt32(19);
                m.nameLoc    = r.IsDBNull(20) ? string.Empty : r.GetString(20);
                m.nameSvr    = r.IsDBNull(21) ? string.Empty : r.GetString(21);
                m.pathLoc    = r.IsDBNull(22) ? string.Empty : r.GetString(22);
                ret = true;
            }
            r.Close();
            return ret;
        }

        public static void process(string id, string perSvr)
        {
            string sql = "update up7_files set f_perSvr=@perSvr where f_id=@id;";
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sql);
            db.AddString(ref cmd, "@perSvr", perSvr, 6);
            db.AddString(ref cmd, "@id", id, 32);
            db.ExecuteNonQuery(cmd);
        }

        public static bool readAll(out List<FileInf> arr)
        {
            arr = new List<FileInf>();
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.Append("select");
            sql.Append(" f_id");
            sql.Append(",f_pathSvr");
            sql.Append(",f_pathRel");
            sql.Append(",f_md5");
            sql.Append(",f_lenLoc");
            sql.Append(",f_sizeLoc");
            sql.Append(",f_pos");
            sql.Append(",f_blockCount");
            sql.Append(",f_blockSize");
            sql.Append(",f_blockPath");
            sql.Append(",f_lenSvr");
            sql.Append(",f_pid");
            sql.Append(",f_perSvr");
            sql.Append(",f_complete");
            sql.Append(",f_time");
            sql.Append(",f_deleted");
            sql.Append(",f_merged");
            sql.Append(",f_pidRoot");
            sql.Append(",f_fdTask");
            sql.Append(",f_fdChild");
            sql.Append(",f_uid");
            sql.Append(",f_nameLoc");
            sql.Append(",f_nameSvr");
            sql.Append(",f_pathLoc");
            sql.Append(" from up7_files");

            DbHelper db = new DbHelper();
            var cmd = db.GetCommand(sql.ToString());
            var r = db.ExecuteReader(cmd);

            while (r.Read())
            {
                var m = new FileInf();
                m.id = r.IsDBNull(0) ? string.Empty : r.GetString(0);
                m.pathSvr = r.IsDBNull(1) ? string.Empty : r.GetString(1);
                m.pathRel = r.IsDBNull(2) ? string.Empty : r.GetString(2);
                m.md5 = r.IsDBNull(3) ? string.Empty : r.GetString(3);
                m.lenLoc = r.IsDBNull(4) ? 0 : r.GetInt64(4);
                m.sizeLoc = r.IsDBNull(5) ? string.Empty : r.GetString(5);
                m.blockCount = r.IsDBNull(7) ? 0 : r.GetInt32(7);
                m.blockSize = r.IsDBNull(8) ? 0 : r.GetInt32(8);
                m.blockPath = r.IsDBNull(9) ? string.Empty : r.GetString(9);
                m.lenSvr = r.IsDBNull(10) ? 0 : r.GetInt64(10);
                m.pid = r.IsDBNull(11) ? string.Empty : r.GetString(11);
                m.perSvr = r.IsDBNull(12) ? string.Empty : r.GetString(12);
                m.complete = r.IsDBNull(13) ? false : r.GetBoolean(13);
                m.time = r.IsDBNull(14) ? DateTime.MinValue : r.GetDateTime(14);
                m.deleted = r.IsDBNull(15) ? false : r.GetBoolean(15);
                //m.merged = r.IsDBNull(16) ? false : r.Boolean(16);
                m.pidRoot = r.IsDBNull(17) ? string.Empty : r.GetString(17);
                m.fdTask = r.IsDBNull(18) ? false : r.GetBoolean(18);
                m.f_fdChild= r.IsDBNull(19) ? false : r.GetBoolean(19);
                m.uid = r.IsDBNull(20) ? 0 : r.GetInt32(20);
                m.nameLoc = r.IsDBNull(21) ? string.Empty : r.GetString(21);
                m.nameSvr = r.IsDBNull(22) ? string.Empty : r.GetString(22);
                m.pathLoc = r.IsDBNull(23) ? string.Empty : r.GetString(23);
                arr.Add(m);
                ret = true;
            }
            r.Close();
            return ret;
        }
    }
}