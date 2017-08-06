using System;

namespace up7.db.biz.redis
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

        public void del()
        {
            this.con.Del(this.getKey());
        }

        public String[] all()
        {
            String[] ids = this.con.LRange(this.getKey(), 0, -1);
            return ids;
        }
    }
}