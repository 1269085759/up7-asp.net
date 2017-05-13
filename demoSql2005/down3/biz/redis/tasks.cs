using Newtonsoft.Json;
using System.Collections.Generic;
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
        /// <summary>
        /// 下载缓存的命名空间，所有下载相关的key都保存在此空间下。
        /// </summary>
        string space = "down3";
        /// <summary>
        /// 用户的下载列表，包含文件和文件夹列表
        /// tasks-down3-uid
        /// </summary>
        string keyUser = "tasks-down3-";
        CSRedis.RedisClient con = null;
        string uid = string.Empty;

        public tasks(string uid,CSRedis.RedisClient c)
        {
            this.uid = uid;
            this.con = c;
        }

        string getKey()
        {
            return this.keyUser + this.uid;
        }

        void addSpace(string key)
        {
            this.con.SAdd(this.space, key);
        }

        public void clear()
        {
            var len = this.con.SCard(this.space);
            while (len > 0)
            {
                var keys = this.con.SScan(this.space, 0, null, 500);
                foreach (var k in keys.Items)
                {
                    this.con.Del(k);
                }
            }
        }

        public void add(DnFileInf f)
        {
            //添加到队列（当前用户下载列表）
            this.con.SAdd(this.getKey(), f.signSvr);
            //添加一条信息
            FileRedis f_svr = new FileRedis(ref this.con);
            f_svr.create(ref f);

            this.addSpace(this.getKey());
            this.addSpace(f.signSvr);
        }

        public void del(string signSvr)
        {
            //从队列中删除（当前用户的下载列表）
            this.con.SRem(this.getKey(), signSvr);
            //从空间中删除
            this.con.SRem(this.space, signSvr);

            //删除文件信息
            this.con.Del(signSvr);
        }

        /// <summary>
        /// 加载用户未完成列表
        /// </summary>
        /// <returns></returns>
        public string toJson()
        {
            var keys = this.con.SMembers(this.getKey());
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