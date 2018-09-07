using System;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using up7.db.biz;
using up7.db.model;
using up7.db.utils;
using up7.db.biz.database;

namespace up7.db
{
    /// <summary>
    /// 为了解决100W+的文件夹存储，此页面仅记录文件夹基本信息
    /// 文件夹文件和层级结构在f_post.aspx页面提供。
    /// </summary>
    public partial class fd_create : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String id       = Request.QueryString["id"];
            String pathLoc  = Request.QueryString["pathLoc"];
            String sizeLoc  = Request.QueryString["sizeLoc"];
            String lenLoc   = Request.QueryString["lenLoc"];
            String uid      = Request.QueryString["uid"];
            String fCount   = Request.QueryString["filesCount"];
            String callback = Request.QueryString["callback"];
            pathLoc         = PathTool.url_decode(pathLoc);

            FileInf f          = new FileInf();
            f.nameLoc          = Path.GetFileName(pathLoc);
            f.nameSvr          = f.nameLoc;
            f.id               = id;
            f.pathLoc          = pathLoc;
            f.sizeLoc          = sizeLoc;
            f.lenLoc           = long.Parse(lenLoc);
            f.fileCount        = int.Parse(fCount);
            f.fdTask           = true;
            f.uid              = int.Parse( uid);
            //生成路径，格式：upload/年/月/日/guid/文件夹名称
            PathGuidBuilder pb = new PathGuidBuilder();
            f.pathSvr          = Path.Combine( pb.genFolder(f.uid,f.id),f.nameLoc);
            f.pathSvr          = f.pathSvr.Replace("\\", "/");
            Directory.CreateDirectory(f.pathSvr);

            //添加到队列表
            DBFile.add(ref f);

            //添加到文件夹表
            DBFolder dbf = new DBFolder();
            dbf.add(ref f);

            string json = JsonConvert.SerializeObject(f);
            json = HttpUtility.UrlEncode(json);
            json = json.Replace("+", "%20");
            Response.Write(callback + "({\"value\":\""+json+"\"})");
        }
    }
}