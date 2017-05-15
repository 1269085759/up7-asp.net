using System;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using up7.demoSql2005.db.redis;
using up7.demoSql2005.db.biz.redis;
using up7.demoSql2005.db.biz;
using up7.demoSql2005.db.utils;

namespace up7.demoSql2005.db
{
    /// <summary>
    /// 此文件处理单文件上传逻辑
    /// 此页面需要返回文件的pathSvr路径。并进行urlEncode编码
    /// 更新记录：
    ///     2016-03-23 优化逻辑，分享子文件逻辑
    /// </summary>
    public partial class f_create : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uid          = Request.QueryString["uid"];
            string idSign       = Request.QueryString["idSign"];
            string lenLoc       = Request.QueryString["lenLoc"];
            string sizeLoc      = Request.QueryString["sizeLoc"];
            string callback     = Request.QueryString["callback"];//jsonp参数
            //客户端使用的是encodeURIComponent编码，
            string pathLoc      = HttpUtility.UrlDecode(Request.QueryString["pathLoc"]);//utf-8解码

            //参数为空
            if (    string.IsNullOrEmpty(uid)
                ||  string.IsNullOrEmpty(sizeLoc)
                )
            {
                Response.Write(callback + "({\"value\":null,\"inf\":\"参数为空，请检查uid,sizeLoc参数。\"})");
                return;
            }

            xdb_files fileSvr = new xdb_files();
            fileSvr.idSign = idSign;
            fileSvr.uid = int.Parse(uid);//将当前文件UID设置为当前用户UID
            fileSvr.nameLoc = Path.GetFileName(pathLoc);
            fileSvr.pathLoc = pathLoc;
            fileSvr.lenLoc = Convert.ToInt64(lenLoc);
            fileSvr.sizeLoc = sizeLoc;
            fileSvr.deleted = false;
            fileSvr.nameSvr = fileSvr.nameLoc;

            PathGuidBuilder pb = new PathGuidBuilder();
            fileSvr.pathSvr = pb.genFile(fileSvr.uid, fileSvr.sign,fileSvr.nameLoc);
            BlockPathBuilder bpb = new BlockPathBuilder();
            fileSvr.blockPath = bpb.root(idSign, fileSvr.pathSvr);

            //添加到redis
            var con = RedisConfig.getCon();
            tasks svr = new tasks(ref con);
            svr.uid = uid;
            svr.add(fileSvr);            

            string jv = JsonConvert.SerializeObject(fileSvr);
            jv = HttpUtility.UrlEncode(jv);
            jv = jv.Replace("+", "%20");
            string json = callback + "({\"value\":\"" + jv + "\"})";//返回jsonp格式数据。
            Response.Write(json);
        }
    }
}