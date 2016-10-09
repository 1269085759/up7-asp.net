using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace up6.demoSql2005.db
{
    /// <summary>
    /// 更新文件或文件夹进度信息
    /// 在停止时调用
    /// 在出错时调用
    /// </summary>
    public partial class f_update : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uid          = Request.QueryString["uid"];
            string sign         = Request.QueryString["sign"];
            string idSvr        = Request.QueryString["idSvr"];
            string perSvr       = Request.QueryString["perSvr"];//文件百分比
            string lenSvr       = Request.QueryString["lenSvr"];//已传大小
            string lenLoc       = Request.QueryString["lenLoc"];//本地文件大小

            //参数为空
            if (string.IsNullOrEmpty(lenLoc)
                || string.IsNullOrEmpty(uid)
                || string.IsNullOrEmpty(idSvr)
                )
            {
                XDebug.Output("lenLoc", lenLoc);
                XDebug.Output("uid", uid);
                XDebug.Output("idSvr", idSvr);
                Response.Write("param is null");
                return;
            }

            //文件夹进度
            DBFile db = new DBFile();
            db.f_process(Convert.ToInt32(uid), Convert.ToInt32(idSvr), 0, Convert.ToInt64(lenSvr), perSvr, false);
        }
    }
}