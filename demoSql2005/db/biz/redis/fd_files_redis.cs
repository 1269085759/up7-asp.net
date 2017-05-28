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
        CSRedis.RedisClient cache = null;

        public fd_files_redis(ref CSRedis.RedisClient c, string id) { this.cache = c;this.idSign = id; }
        string getKey()
        {
            string key = idSign + "-files";
            return key;
        }

        public void del() { this.cache.Del(this.getKey()); }
        public void add(string id)
        {
            this.cache.SAdd(this.getKey(), id);
        }
    }
}