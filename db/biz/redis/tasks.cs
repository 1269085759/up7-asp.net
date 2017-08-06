using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using up7.db.model;

namespace up7.db.biz.redis
{
    public class tasks
    {
        public string uid = "";
        string keyName = "tasks";
        CSRedis.RedisClient con = null;

        public tasks(ref CSRedis.RedisClient c) { this.con = c; }
        string getKey() { return this.keyName + "-" + this.uid; }

        public void del(string id)
        {
            //从队列中删除
            this.con.SRem(this.getKey(), id);
            //删除key
            this.con.Del(id);
        }

        /// <summary>
        /// 清除所有子文件信息
        /// 清除文件夹信息
        /// </summary>
        /// <param name="id"></param>
        public void delFd(String id)
        {
            long len = this.con.LLen(id);
            long index = 0;
            while (index < len)
            {
                var keys = this.con.LRange(id, index, index + 1000);
                index += keys.Length;
                
                //清除文件缓存
                this.con.Del(keys);
            }
            this.con.Del(id);
        }

        public void clear() { this.con.FlushDb(); }
    }
}