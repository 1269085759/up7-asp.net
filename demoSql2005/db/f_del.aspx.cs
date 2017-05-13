using System;
using System.IO;
using up7.demoSql2005.db;
using up7.demoSql2005.db.biz.redis;
using up7.demoSql2005.db.redis;

namespace up6.demoSql2005.db
{
    public partial class f_del : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string fid = Request.QueryString["idSign"];
            string uid = Request.QueryString["uid"];
            string callback = Request.QueryString["callback"];
            int ret = 0;

            if (string.IsNullOrEmpty(fid)
                || string.IsNullOrEmpty(uid))
            {
            }//参数不为空
            else
            {
                var j = RedisConfig.getCon();
                tasks svr = new tasks(ref j);
                svr.uid = uid;
                svr.del(fid);

                DBFile db = new DBFile();
                db.delete(fid);
                ret = 1;
            }
            Response.Write(callback + "(" + ret + ")");//返回jsonp格式数据
        }
    }
}