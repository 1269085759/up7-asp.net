using Newtonsoft.Json;
using System.Collections.Generic;
using up7.demoSql2005.down3.biz.redis;
using up7.demoSql2005.down3.model;

namespace up7.demoSql2005.down3.biz
{
    /// <summary>
    /// 下载的缓存处理类
    /// 更新记录：
    ///     207-05-13 创建
    /// </summary>
    public class tasks
    {
        CSRedis.RedisClient con = null;
        string uid = string.Empty;

        public tasks(string uid,CSRedis.RedisClient c)
        {
            this.uid = uid;
            this.con = c;
        }

        public void clear()
        {
            //找出所有用户下载队列
            var keys = this.con.Keys("d-*");
            foreach (var k in keys)
            {
                this.clearUser(k);
            }
        }

        public void clearUser(string key)
        {
            var len = this.con.SCard(key);
            while (len > 0)
            {
                var keys = this.con.SScan(key, 0, null, 500);
                len -= keys.Items.Length;
                foreach (var k in keys.Items)
                {
                    this.con.Del(k);
                }
            }
            this.con.Del(key);
        }

        public void add(DnFileInf f)
        {
            //string taskKey = this.keyUser + this.uid;
            //添加到队列（当前用户下载列表）
            //this.con.SAdd("udt-v", 12);
            //this.con.SAdd(taskKey, 1);

            //添加一条信息
            FileRedis f_svr = new FileRedis(ref this.con);
            f_svr.create(ref f);

            KeyMaker km = new KeyMaker();
            string space = km.space(this.uid);
            this.con.SAdd(space, f.signSvr);
            //this.addSpace(taskKey);
            //this.addSpace(f.signSvr);
        }

        public void del(string signSvr)
        {
            KeyMaker km = new KeyMaker();
            string space = km.space(this.uid);

            //从队列中删除（当前用户的下载列表）
            this.con.SRem(space, signSvr);

            //删除文件信息
            this.con.Del(signSvr);
        }

        /// <summary>
        /// 加载用户未完成列表
        /// </summary>
        /// <returns></returns>
        public string toJson()
        {
            KeyMaker km = new KeyMaker();
            string space = km.space(this.uid);
            var keys = this.con.SMembers(space);
            List<DnFileInf> files = new List<DnFileInf>();
            foreach (var key in keys)
            {
                FileRedis f_svr = new FileRedis(ref this.con);
                DnFileInf data = f_svr.read(key);
                files.Add(data);
            }

            return JsonConvert.SerializeObject(files);
        }
    }
}