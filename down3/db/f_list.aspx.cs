using System;
using System.Web;
using up7.down3.biz;

namespace up7.down3.db
{
    /// <summary>
    /// 列出未下载完的任务
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

            DnFile df = new DnFile();
            string json = df.all_uncmp(int.Parse(uid));
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