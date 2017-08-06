using System;
using up7.db.biz.redis;

namespace up7.db
{
    public partial class fd_complete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id    = Request.QueryString["id"];
            string uid   = Request.QueryString["uid"];
            string merge = Request.QueryString["merge"];
            string cak   = Request.QueryString["callback"];
            int ret = 0;

            //参数为空
            if (!string.IsNullOrEmpty(id) )
            {
                var con = RedisConfig.getCon();
                fd_redis fd = new fd_redis(ref con);
                fd.fileMerge = merge.Equals("1");
                fd.read(id);
                fd.saveToDb();//保存到数据库

                //清除缓存
                tasks svr = new tasks(ref con);
                svr.uid = uid;
                svr.delFd(id);

                con.Dispose();
                ret = 1;
            }
            Response.Write(cak + "(" + ret + ")");
        }
    }
}