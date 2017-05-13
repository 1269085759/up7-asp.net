using System;
using up7.demoSql2005.db.redis;

namespace up7.demoSql2005.down3.debug
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var j = RedisConfig.getCon();
            for (int i = 0; i < 10000; ++i)
            {
                j.SAdd("a", i);
            }

            var len = j.SCard("a");
            while (len > 0)
            {
                var keys = j.SScan("a", 0, null, 500);
                len -= keys.Items.Length;
                foreach (var k in keys.Items)
                {
                    j.Del(k);
                }
            }
        }
    }
}