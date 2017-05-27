namespace up7.demoSql2005.down3.biz.redis
{
    public class KeyMaker
    {
        /// <summary>
        /// 下载项前缀
        /// d-0
        /// </summary>
        /// <returns></returns>
        public string space(string uid)
        {
            return "d-" + uid;
        }
    }
}