using System;
using System.Web;
using up7.db.biz.redis;
using up7.db.utils;
using up7.down3.model;
using tasks = up7.down3.biz.redis.tasks;

namespace up7.down3.db
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
            string id      = Request.QueryString["id"];
            string nameLoc = Request.QueryString["nameLoc"];
            string pathLoc = Request.QueryString["pathLoc"];
            string sizeSvr = Request.QueryString["sizeSvr"];

            sizeSvr = PathTool.url_decode(sizeSvr);
            pathLoc = PathTool.url_decode(pathLoc);
            nameLoc = PathTool.url_decode(nameLoc);

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
            fd.id = id;
            fd.sizeSvr = sizeSvr;
            var j = RedisConfig.getCon();
            tasks svr = new tasks(uid,j);
            svr.add(fd);
            Response.Write(cbk+"(1)");
        }
    }
}