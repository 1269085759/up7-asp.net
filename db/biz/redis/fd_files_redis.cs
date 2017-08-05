namespace up7.db.biz.redis
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
            this.cache.LPush(this.getKey(), id);
        }
    }
}