﻿using System;
using up7.db.biz.database;

namespace up7.db.biz.redis
{
    public class RedisFolder
    {
        CSRedis.RedisClient cache = null;
        /// <summary>
        /// 合并文件
        /// </summary>
        public bool fileMerge = true;
        /// <summary>
        /// 文件夹ID
        /// </summary>
        public string id = string.Empty;

        public RedisFolder() { }
        public RedisFolder(ref CSRedis.RedisClient j) { this.cache = j; }

        //保存到数据库
        public void saveToDb()
        {
            using (var con = DbHelper.CreateConnection())
            {
                con.Open();
                //FolderDbWriter fd = new FolderDbWriter(con, this.m_root);
                //fd.save();

                FileDbWriter fw = new FileDbWriter(con, this.cache);
                fw.pidRoot = this.id;
                fw.merge = this.fileMerge;
                fw.save();
                con.Close();
            }                
        }
    }
}