using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using up7.demoSql2005.db;

namespace up7.demoSql2005.db.biz.redis
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
            this.add(f.idSign);

            FileRedis fs = new FileRedis(ref this.con);
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
            FileRedis cache = new FileRedis(ref this.con);

            foreach(String s in ls)
            {
                var f = cache.read(s);                
                arr.Add(f);
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