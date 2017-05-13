using System;
using up7.demoSql2005.db.redis;
using up7.demoSql2005.down3.biz;

namespace up7.demoSql2005.down3.db
{
    public partial class clear : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var j = RedisConfig.getCon();
            tasks svr = new tasks("0",j);
            svr.clear();
        }
    }
}