using Newtonsoft.Json;
using System;
using System.Web;
using up7.demoSql2005.db.redis;
using up7.demoSql2005.down3.biz;
using up7.demoSql2005.down3.model;

namespace up7.demoSql2005.down3.db
{
    /// <summary>
    /// 创建一个文件夹下载任务，添加到redis中
    /// </summary>
    public partial class fd_create : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uid     = Request.QueryString["uid"];
            string cbk     = Request.QueryString["callback"];
            string signSvr = Request.QueryString["signSvr"];
            string nameLoc = Request.QueryString["nameLoc"];
            string pathLoc = Request.QueryString["pathLoc"];
            string sizeSvr = Request.QueryString["sizeSvr"];

            sizeSvr = sizeSvr.Replace("+", "%20");
            pathLoc = pathLoc.Replace("+", "%20");
            nameLoc = nameLoc.Replace("+", "%20");
            pathLoc = HttpUtility.UrlDecode(pathLoc);//utf-8解码
            nameLoc = HttpUtility.UrlDecode(nameLoc);//utf-8解码
            sizeSvr = HttpUtility.UrlDecode(sizeSvr);//utf-8解码

            if (string.IsNullOrEmpty(uid)
               || string.IsNullOrEmpty(nameLoc)
               || string.IsNullOrEmpty(pathLoc)
               )
            {
                Response.Write(cbk+"(0)");
                return;
            }

            DnFileInf fd = new DnFileInf();
            fd.nameLoc = nameLoc;
            fd.pathLoc = pathLoc;
            fd.signSvr = signSvr;
            fd.sizeSvr = sizeSvr;
            fd.folder = true;
            var j = RedisConfig.getCon();
            tasks svr = new tasks(uid,j);
            svr.add(fd);
            Response.Write(cbk+"(1)");
        }
    }
}