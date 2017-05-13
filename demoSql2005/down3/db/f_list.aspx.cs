using System;

using System.Web;
using up7.demoSql2005.db.redis;
using up7.demoSql2005.down3.biz;

namespace up7.demoSql2005.down3.db
{
    /// <summary>
    /// 列出未完成的文件和文件夹下载任务。
    /// 从redis缓存中取数据，
    /// 格式：json
    ///     [f1,f2,f3,f4]
    /// f1为xdb_files对象
    /// </summary>
    public partial class f_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uid = Request.QueryString["uid"];
            string cbk = Request.QueryString["callback"];

            if (string.IsNullOrEmpty(uid))
            {
                Response.Write(cbk+"({\"value\":null})");
                Response.End();
                return;
            }

            var j = RedisConfig.getCon();
            tasks svr = new tasks(uid,j);
            string json = svr.toJson();
            if (!string.IsNullOrEmpty(json))
            {
                json = HttpUtility.UrlEncode(json);
                json = json.Replace("+", "%20");
                json = cbk + "({\"value\":\"" + json + "\"})";
            }
            else { json = cbk + "({\"value\":null})"; };

            Response.Write(json);
        }
    }
}