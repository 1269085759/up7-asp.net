using CSRedis;

namespace up7.demoSql2005.db.redis
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