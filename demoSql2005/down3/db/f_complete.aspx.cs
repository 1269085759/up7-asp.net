using System;
using up7.demoSql2005.db.redis;
using up7.demoSql2005.down3.biz;

namespace up6.demoSql2005.down3.db
{
    public partial class f_complete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uid = Request.QueryString["uid"];
            string signSvr = Request.QueryString["signSvr"];
            string cbk = Request.QueryString["callback"];

            if (string.IsNullOrEmpty(uid)
                || string.IsNullOrEmpty(signSvr))
            {
                Response.Write(cbk + "(0)");
                return;
            }

            var j = RedisConfig.getCon();
            tasks svr = new tasks(uid,j);
            svr.del(signSvr);

            Response.Write(cbk + "(1)");
        }
    }
}