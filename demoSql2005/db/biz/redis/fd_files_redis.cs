using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using up7.demoSql2005.db.biz.folder;

namespace up7.demoSql2005.db.biz.redis
{
    /// <summary>
    /// 文件夹子文件列表
    /// </summary>
    public class fd_files_redis
    {
        public string idSign = string.Empty;
        CSRedis.RedisClient con = null;

        public fd_files_redis(ref CSRedis.RedisClient c, string id) { this.con = c;this.idSign = id; }
        string getKey()
        {
            string key = idSign + "-files";
            return key;
        }

        public void del() { this.con.Del(this.getKey()); }
        public void add(string id)
        {
            this.con.SAdd(this.getKey(), id);
        }

        public void add(List<xdb_files> fs)
        {
            String key = this.getKey();
            foreach (var f in fs)
            {
                this.con.SAdd(key, f.idSign);
            }
        }

        public void add(List<fd_file> fs)
        {
            String key = this.getKey();
            foreach (var f in fs)
            {
                this.con.SAdd(key, f.idSign);
            }
        }

        public String[] all()
        {
            return this.con.SMembers(this.getKey());
        }
    }
}