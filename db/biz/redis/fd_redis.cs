using System;
using System.Collections.Generic;
using up7.db.biz.database;
using up7.db.biz.folder;

namespace up7.db.biz.redis
{
    public class fd_redis
    {

        //文件夹json数据
        public String data;
        fd_root m_root = null;
        CSRedis.RedisClient cache = null;
        /// <summary>
        /// 合并文件
        /// </summary>
        public bool fileMerge = true;
        Dictionary<String/*guid*/, String/*pathSvr*/> parentPathMap = new Dictionary<String, String>();

        public fd_redis() { }
        public fd_redis(ref CSRedis.RedisClient j) { this.cache = j; }

        //从redis中读取数据
        public void read(String idSign)
        {
            var j = this.cache;
            //folder不存在
            if (!j.Exists(idSign))
            {
                return;
            }

            this.m_root = new fd_root();
            this.m_root.idSign = idSign;
            this.m_root.nameLoc = j.HGet(idSign, "nameLoc");
            this.m_root.nameSvr = this.m_root.nameLoc;
            this.m_root.lenLoc = long.Parse(j.HGet(idSign, "lenLoc"));
            this.m_root.lenSvr = long.Parse(j.HGet(idSign, "lenSvr"));
            this.m_root.sizeLoc = j.HGet(idSign, "sizeLoc");
            this.m_root.pathLoc = j.HGet(idSign, "pathLoc");
            this.m_root.pathSvr = j.HGet(idSign, "pathSvr");
            this.m_root.fileCount = int.Parse(j.HGet(idSign, "filesCount"));
            this.m_root.folder = true;
            //
            //this.loadFiles();//加载文件列表		
            this.loadFolders();//加载目录列表
        }

        //保存到数据库
        public void saveToDb()
        {
            using (var con = DbHelper.CreateConnection())
            {
                con.Open();
                FolderDbWriter fd = new FolderDbWriter(con, this.m_root);
                fd.save();

                FileDbWriter fw = new FileDbWriter(con, this.m_root,this.cache);
                fw.merge = this.fileMerge;
                fw.save();
                con.Close();
            }                
        }


        void loadFolders()
        {
            //取文件ID列表
            fd_folders_redis rfs = new fd_folders_redis(ref this.cache, this.m_root.idSign);
            this.m_root.folders = new List<fd_child>();
            var fs = rfs.all();
            foreach (String s in fs)
            {
                fd_child_redis fd = new fd_child_redis();
                fd.read(this.cache, s);
                this.m_root.folders.Add(fd);
            }
        }
    }
}