using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace up7.demoSql2005.db.biz.redis
{
    public class fd_folders_redis
    {
        public string idSign = string.Empty;
        CSRedis.RedisClient con = null;
        public fd_folders_redis(ref CSRedis.RedisClient c,string idSign) { this.con = c;this.idSign = idSign; }
        String getKey()
        {
            String key = idSign + "-folders";
            return key;
        }

        public void add(String fSign)
        {
            this.con.LPush(this.getKey(), fSign);
        }

        public void del()
        {
            this.con.Del(this.getKey());
        }

        public void add(List<fd_child_redis> fs)
        {
            String key = this.getKey();
            foreach(fd_child_redis f in fs)
            {
                this.con.LPush(key, f.idSign);
            }
        }

        public String[] all()
        {
            String[] ids = this.con.LRange(this.getKey(), 0, -1);
            return ids;
        }
    }
}