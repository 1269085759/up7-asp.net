using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace up7.demoSql2005.db.biz.folder
{
    public class FileDbWriter
    {
        fd_root root;
        DbConnection con = null;
        CSRedis.RedisClient m_cache = null;
        public bool merge = true;//合并文件

        public FileDbWriter(DbConnection con, fd_root fd,CSRedis.RedisClient c)
        {
            this.con = con;
            this.root = fd;
            this.m_cache = c;
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
            sb.Append(",f_pathRel");
            sb.Append(",f_lenLoc");
            sb.Append(",f_sizeLoc");
            sb.Append(",f_lenSvr");
            sb.Append(",f_perSvr");
            sb.Append(",f_sign");
            sb.Append(",f_complete");
            sb.Append(",f_fdTask");
            sb.Append(",f_blockPath");
            sb.Append(",f_blockSize");

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
            sb.Append(",@f_pathRel");//f_pathRel
            sb.Append(",@f_lenLoc");//f_lenLoc
            sb.Append(",@f_sizeLoc");//f_sizeLoc
            sb.Append(",@f_lenSvr");//f_lenSvr
            sb.Append(",@f_perSvr");//f_perSvr
            sb.Append(",@f_sign");//f_sign
            sb.Append(",1");//f_complete
            sb.Append(",@f_fdTask");//f_fdTask
            sb.Append(",@f_blockPath");//f_blockPath
            sb.Append(",@f_blockSize");//f_blockSize
            sb.Append(")");

            var cmd = con.CreateCommand();
            cmd.CommandText = sb.ToString();
            DbHelper db = new DbHelper();
            db.AddString(ref cmd, "@f_idSign", string.Empty, 36);
            db.AddString(ref cmd, "@f_pidSign", string.Empty, 36);
            db.AddString(ref cmd, "@f_rootSign", string.Empty, 36);
            db.AddBool(ref cmd, "@f_fdChild",false);
            db.AddInt(ref cmd, "@f_uid", 0);
            db.AddString(ref cmd, "@f_nameLoc", string.Empty, 255);
            db.AddString(ref cmd, "@f_nameSvr", string.Empty, 255);
            db.AddString(ref cmd, "@f_pathLoc", string.Empty, 512);
            db.AddString(ref cmd, "@f_pathSvr", string.Empty, 512);
            db.AddString(ref cmd, "@f_pathRel", string.Empty, 512);
            db.AddInt64(ref cmd, "@f_lenLoc", 0);
            db.AddString(ref cmd, "@f_sizeLoc", string.Empty, 15);
            db.AddInt64(ref cmd, "@f_lenSvr",0);
            db.AddString(ref cmd, "@f_perSvr", string.Empty, 6);
            db.AddString(ref cmd, "@f_sign", string.Empty, 32);
            db.AddBool(ref cmd, "@f_fdTask", false);
            db.AddString(ref cmd, "@f_blockPath", string.Empty,2000);
            db.AddInt(ref cmd, "@f_blockSize", 0);
            return cmd;
        }

        void save(ref DbCommand cmd,xdb_files f)
        {
            cmd.Parameters[0].Value = f.idSign;//idSign
            cmd.Parameters[1].Value = string.IsNullOrEmpty(f.pidSign) ? string.Empty : f.pidSign;//pidSign
            cmd.Parameters[2].Value = string.IsNullOrEmpty(f.rootSign) ? string.Empty : f.rootSign;//rootSign
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
            cmd.Parameters[14].Value = string.IsNullOrEmpty(f.sign) ? string.Empty : f.sign;//sign
            cmd.Parameters[15].Value = f.folder;//fdTask
            cmd.Parameters[16].Value = f.blockPath;//
            cmd.Parameters[17].Value = f.blockSize;//
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 每次从缓存中读取100数据写入数据库，
        /// 防止一次性加载全部文件项导致内存不足，比如10w+
        /// </summary>
        public void save()
        {
            var cmd = this.makeCmd(con);
            //保存文件夹
            this.save(ref cmd, this.root);

            string key = this.root.idSign + "-files";
            int index = 0;
            long len = this.m_cache.LLen(key);
            redis.FileRedis svr = new redis.FileRedis(ref this.m_cache);
            BlockMeger bm = new BlockMeger();
            List<xdb_files> files = null;

            while (index<len)
            {
                var keys = this.m_cache.LRange(key,len,len+100);
                index += keys.Length;

                files = new List<xdb_files>();
                foreach(var k in keys)
                {
                System.Diagnostics.Debug.WriteLine(k);
                    xdb_files f = svr.read(k);
                    f.f_fdChild = true;
                    f.rootSign = this.root.idSign;
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
                files.Clear();
            }
            this.m_cache.Del(key);
            cmd.Dispose();
        }        
    }
}