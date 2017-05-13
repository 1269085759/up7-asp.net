using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using up7.demoSql2005.db.biz.redis;
using up7.demoSql2005.db.redis;

namespace up7.demoSql2005.db
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
            string idSign       = Request.QueryString["idSign"];
            string perSvr       = Request.QueryString["perSvr"];//文件百分比
            string lenSvr       = Request.QueryString["lenSvr"];//已传大小
            string lenLoc       = Request.QueryString["lenLoc"];//本地文件大小

            //参数为空
            if (string.IsNullOrEmpty(lenLoc)
                || string.IsNullOrEmpty(uid)
                || string.IsNullOrEmpty(idSign)
                )
            {
                XDebug.Output("lenLoc", lenLoc);
                XDebug.Output("uid", uid);
                XDebug.Output("idSvr", idSign);
                Response.Write("param is null");
                return;
            }

            //更新redis进度
            var con = RedisConfig.getCon();
            FileRedis rf = new FileRedis(ref con);
            rf.process(idSign, perSvr, lenSvr,"0");
        }
    }
}