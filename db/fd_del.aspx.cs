using System;
using up7.db.biz.redis;

namespace up7.db
{
    public partial class fd_del : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string fid = Request.QueryString["idSign"];
            string uid = Request.QueryString["uid"];
            string cbk = Request.QueryString["callback"];
            int ret = 0;

            if (string.IsNullOrEmpty(fid)
                || string.IsNullOrEmpty(uid)
                )
            {
            }//参数不为空
            else
            {
                var j = RedisConfig.getCon();
                tasks cache = new tasks(ref j);
                cache.delFd(fid);
                ret = 1;
            }
            Response.Write(cbk + "({\"value\":" + ret + "})");//返回jsonp格式数据
        }
    }
}