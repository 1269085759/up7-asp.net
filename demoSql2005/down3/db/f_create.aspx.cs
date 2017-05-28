using Newtonsoft.Json;
using System;
using System.Web;
using up7.demoSql2005.db.redis;
using up7.demoSql2005.down3.biz;
using up7.demoSql2005.down3.model;

namespace up7.demoSql2005.down3.db
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
            pathLoc         = pathLoc.Replace("+", "%20");
            pathLoc         = HttpUtility.UrlDecode(pathLoc);//utf-8解码
            nameLoc         = nameLoc.Replace("+", "%20");
            nameLoc         = HttpUtility.UrlDecode(nameLoc);
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