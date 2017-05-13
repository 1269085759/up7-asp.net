using System;

namespace up7.demoSql2005.down3.db
{
    /// <summary>
    /// 创建一个文件夹下载任务，添加到redis中
    /// </summary>
    public partial class fd_create : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uid      = Request.Form["uid"];
            string fdStr    = Request.Form["folder"];//客户端使用的是encodeURIComponent编码，
            if (!string.IsNullOrEmpty(fdStr)) fdStr = fdStr.Replace("%20", "+");
            if (!string.IsNullOrEmpty(fdStr)) fdStr = Server.UrlDecode(fdStr);

            if (string.IsNullOrEmpty(uid)
               || string.IsNullOrEmpty(fdStr))
            {
                Response.Write(0);
                Response.End();
                return;
            }
        }
    }
}