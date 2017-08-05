using System;
using up7.db.biz.redis;
using tasks = up7.down3.biz.redis.tasks;

namespace up7.down3.db
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