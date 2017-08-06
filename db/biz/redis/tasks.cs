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

        public void add(string id)
        {
            this.con.SAdd(this.getKey(), id);
        }

        public void add(xdb_files f)
        {
            this.add(f.id);

            RedisFile fs = new RedisFile(ref this.con);
            fs.create(f);
        }

        public void del(string id)
        {
            //从队列中删除
            this.con.SRem(this.getKey(), id);
            //删除key
            this.con.Del(id);
        }

        public void delFd(String sign)
        {
            //清除文件列表缓存
            fd_files_redis files = new fd_files_redis(ref this.con, sign);
            files.del();

            //清除目录列表缓存
            var folders = new fd_folders_redis(ref this.con, sign);
            folders.del();

            //从队列中清除
            this.del(sign);
        }

        public void clear() { this.con.FlushDb(); }

        public List<xdb_files> all()
        {
            List<xdb_files> arr = null;
            var ls = this.con.SMembers(this.getKey());

            if (ls.Length > 0) arr = new List<xdb_files>();
            RedisFile cache = new RedisFile(ref this.con);

            foreach(String s in ls)
            {
                var f = cache.read(s);
                if(f!=null)arr.Add(f);
            }
            return arr;
        }

        public String toJson()
        {
            var fs = this.all();
            if (fs == null) return "";

            var v = JsonConvert.SerializeObject(fs);
            return v;
        }
    }
}