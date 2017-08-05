using System;
using System.Web;
using Newtonsoft.Json;
using up7.db.biz.redis;
using up7.db.utils;
using up7.down3.model;
using tasks = up7.down3.biz.redis.tasks;

namespace up7.down3.db
{
    public partial class f_create : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uid      = Request.QueryString["uid"];
            string signSvr  = Request.QueryString["signSvr"];
            string nameLoc  = Request.QueryString["nameLoc"];//客户端使用的是encodeURIComponent编码，
            string pathLoc  = Request.QueryString["pathLoc"];//客户端使用的是encodeURIComponent编码，
            string pathSvr  = Request.QueryString["pathSvr"];
            string fileUrl  = Request.QueryString["fileUrl"];
            pathLoc         = PathTool.url_decode(pathLoc);
            nameLoc         = PathTool.url_decode(nameLoc);
            string lenSvr = Request.QueryString["lenSvr"];
            string sizeSvr  = Request.QueryString["sizeSvr"];
            string cbk      = Request.QueryString["callback"];//应用于jsonp数据

            if (string.IsNullOrEmpty(uid)
                || string.IsNullOrEmpty(pathLoc)
                || string.IsNullOrEmpty(fileUrl)
                || string.IsNullOrEmpty(lenSvr))
            {
                Response.Write(cbk + "({\"value\":null})");
                Response.End();
                return;
            }

            DnFileInf inf = new DnFileInf();
            inf.signSvr = signSvr;
            inf.uid = int.Parse(uid);
            inf.nameLoc = nameLoc;
            inf.pathLoc = pathLoc;//记录本地存储位置
            inf.fileUrl = fileUrl;
            inf.lenSvr = long.Parse(lenSvr);
            inf.sizeSvr = sizeSvr;

            var j = RedisConfig.getCon();
            tasks svr = new tasks(uid,j);
            svr.add(inf);//添加到缓存

            string json = JsonConvert.SerializeObject(inf);
            json = HttpUtility.UrlEncode(json);
            json = json.Replace("+", "%20");
            json = cbk + "({\"value\":\"" + json + "\"})";//返回jsonp格式数据。
            Response.Write(json);
        }
    }
}