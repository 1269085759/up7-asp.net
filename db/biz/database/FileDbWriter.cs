using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using up7.db.model;

namespace up7.db.biz.database
{
    public class FileDbWriter
    {
        public string pidRoot = string.Empty;
        DbConnection con = null;
        CSRedis.RedisClient m_cache = null;
        public bool merge = true;//合并文件

        public FileDbWriter(DbConnection con, CSRedis.RedisClient c)
        {
            this.con = con;
            this.m_cache = c;
        }

        DbCommand makeCmd(DbConnection con)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into up7_files(");
            sb.Append(" f_id");
            sb.Append(",f_pid");
            sb.Append(",f_pidRoot");
            sb.Append(",f_fdChild");
            sb.Append(",f_uid");
            sb.Append(",f_nameLoc");
            sb.Append(",f_nameSvr");
            sb.Append(",f_pathLoc");
            sb.Append(",f_pathSvr");
            sb.Append(",f_pathRel");
            sb.Append(",f_lenLoc");
            sb.Append(",f_sizeLoc");
            sb.Append(",f_lenSvr");
            sb.Append(",f_perSvr");
            sb.Append(",f_complete");
            sb.Append(",f_fdTask");
            sb.Append(",f_blockPath");
            sb.Append(",f_blockSize");

            sb.Append(") values(");

            sb.Append(" @f_id");//f_id
            sb.Append(",@f_pid");//f_pid
            sb.Append(",@f_pidRoot");//f_pidRoot
            sb.Append(",@f_fdChild");//f_fdChild
            sb.Append(",@f_uid");//f_uid
            sb.Append(",@f_nameLoc");//f_nameLoc
            sb.Append(",@f_nameSvr");//f_nameSvr
            sb.Append(",@f_pathLoc");//f_pathLoc
            sb.Append(",@f_pathSvr");//f_pathSvr
            sb.Append(",@f_pathRel");//f_pathRel
            sb.Append(",@f_lenLoc");//f_lenLoc
            sb.Append(",@f_sizeLoc");//f_sizeLoc
            sb.Append(",@f_lenSvr");//f_lenSvr
            sb.Append(",@f_perSvr");//f_perSvr
            sb.Append(",1");//f_complete
            sb.Append(",@f_fdTask");//f_fdTask
            sb.Append(",@f_blockPath");//f_blockPath
            sb.Append(",@f_blockSize");//f_blockSize
            sb.Append(")");

            var cmd = con.CreateCommand();
            cmd.CommandText = sb.ToString();
            DbHelper db = new DbHelper();
            db.AddString(ref cmd, "@f_id", string.Empty, 32);
            db.AddString(ref cmd, "@f_pid", string.Empty, 32);
            db.AddString(ref cmd, "@f_pidRoot", string.Empty, 32);
            db.AddBool(ref cmd  , "@f_fdChild",false);
            db.AddInt(ref cmd   , "@f_uid", 0);
            db.AddString(ref cmd, "@f_nameLoc", string.Empty, 255);
            db.AddString(ref cmd, "@f_nameSvr", string.Empty, 255);
            db.AddString(ref cmd, "@f_pathLoc", string.Empty, 512);
            db.AddString(ref cmd, "@f_pathSvr", string.Empty, 512);
            db.AddString(ref cmd, "@f_pathRel", string.Empty, 512);
            db.AddInt64(ref cmd , "@f_lenLoc", 0);
            db.AddString(ref cmd, "@f_sizeLoc", string.Empty, 15);
            db.AddInt64(ref cmd , "@f_lenSvr",0);
            db.AddString(ref cmd, "@f_perSvr", string.Empty, 6);
            db.AddBool(ref cmd  , "@f_fdTask", false);
            db.AddString(ref cmd, "@f_blockPath", string.Empty,2000);
            db.AddInt(ref cmd   , "@f_blockSize", 0);
            return cmd;
        }

        void save(ref DbCommand cmd,FileInf f)
        {
            cmd.Parameters[0].Value = f.id;//idSign
            cmd.Parameters[1].Value = f.pid;//pidSign
            cmd.Parameters[2].Value = f.pidRoot;//rootSign
            cmd.Parameters[3].Value = f.f_fdChild;//fdChild
            cmd.Parameters[4].Value = f.uid;//uid
            cmd.Parameters[5].Value = f.nameLoc;//nameLoc
            cmd.Parameters[6].Value = f.nameSvr;//nameSvr
            cmd.Parameters[7].Value = f.pathLoc;//pathLoc
            cmd.Parameters[8].Value = f.pathSvr;//pathSvr
            cmd.Parameters[9].Value = f.pathRel;//pathRel
            cmd.Parameters[10].Value = f.lenLoc;//lenLoc
            cmd.Parameters[11].Value = f.sizeLoc;//sizeLoc
            cmd.Parameters[12].Value = f.lenLoc;//lenSvr
            cmd.Parameters[13].Value = "100%";//perSvr
            //cmd.Parameters[14].Value = string.IsNullOrEmpty(f.sign) ? string.Empty : f.sign;//sign
            cmd.Parameters[14].Value = f.fdTask;//fdTask
            cmd.Parameters[15].Value = f.blockPath;//
            cmd.Parameters[16].Value = f.blockSize;//
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 每次从缓存中读取100数据写入数据库，
        /// 防止一次性加载全部文件项导致内存不足，比如10w+
        /// </summary>
        public void save()
        {
            var cmd = this.makeCmd(con);
            int index = 0;
            long len = this.m_cache.LLen(this.pidRoot);
            redis.RedisFile svr = new redis.RedisFile(ref this.m_cache);
            BlockMeger bm = new BlockMeger();
            List<FileInf> files = null;

            while (index<len)
            {
                var keys = this.m_cache.LRange(this.pidRoot, index, index + 1000);
                index += keys.Length;

                files = new List<FileInf>();
                foreach(var k in keys)
                {
                    System.Diagnostics.Debug.WriteLine(k);
                    FileInf f = svr.read(k);
                    f.f_fdChild = true;
                    this.save(ref cmd,f);//添加到数据库
                    files.Add(f);
                }

                //合并文件
                if(this.merge)
                {
                    foreach(var f in files)
                    {
                        bm.merge(f);
                    }
                }
                //清除文件缓存
                this.m_cache.Del(keys);
                files.Clear();
            }
            this.m_cache.Del(this.pidRoot);
            cmd.Dispose();
        }        
    }
}