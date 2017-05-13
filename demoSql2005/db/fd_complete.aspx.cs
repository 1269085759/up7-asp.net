using System;
using up7.demoSql2005.db.biz.redis;
using up7.demoSql2005.db.redis;

namespace up7.demoSql2005.db
{
    public partial class fd_complete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id  = Request.QueryString["idSign"];
            string uid = Request.QueryString["uid"];
            string cak = Request.QueryString["callback"];
            int ret = 0;

            //参数为空
            if (!string.IsNullOrEmpty(id) )
            {
                var con = RedisConfig.getCon();
                fd_redis fd = new fd_redis(ref con);
                fd.read(id);

                //清除缓存
                tasks svr = new tasks(ref con);
                svr.uid = uid;
                svr.delFd(id);

                fd.mergeAll();//合并文件块
                fd.saveToDb();//保存到数据库
                con.Dispose();
                ret = 1;
            }
            Response.Write(cak + "(" + ret + ")");
        }
    }
}