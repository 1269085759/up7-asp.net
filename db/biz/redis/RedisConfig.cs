using CSRedis;

namespace up7.db.biz.redis
{
    public class RedisConfig
    {
        public static RedisClient getCon()
        {
            string host = "127.0.0.1";
            RedisClient c = new RedisClient(host, 6379);
            return c;
        }
    }
}