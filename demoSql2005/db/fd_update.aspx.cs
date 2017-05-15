using System;
using up7.demoSql2005.db;

namespace up7.demoSql2005.db
{
    public partial class fd_update : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uid      = Request.QueryString["uid"];
            string id       = Request.QueryString["idSign"];
            string perSvr   = Request.QueryString["perSvr"];//文件百分比
            string lenSvr   = Request.QueryString["lenSvr"];//已传大小
            string lenLoc   = Request.QueryString["lenLoc"];//本地文件大小

            //参数为空
            if (string.IsNullOrEmpty(lenSvr)
                || string.IsNullOrEmpty(uid)
                || string.IsNullOrEmpty(id)
                || string.IsNullOrEmpty(perSvr)
                )
            {
                XDebug.Output("lenLoc", lenLoc);
                XDebug.Output("uid", uid);
                XDebug.Output("idSvr", id);
                Response.Write("param is null");
                return;
            }

            //文件夹进度
            DBFolder.update(id, perSvr, long.Parse(lenSvr), int.Parse(uid));
        }
    }
}