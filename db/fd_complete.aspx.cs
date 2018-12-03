using System;
using up7.db.biz;
using up7.db.biz.database;
using up7.db.model;

namespace up7.db
{
    public partial class fd_complete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id    = Request.QueryString["id"];
            string uid   = Request.QueryString["uid"];
            string cak   = Request.QueryString["callback"];
            int ret = 0;

            //参数为空
            if (!string.IsNullOrEmpty(id) )
            {
                DBFile.complete(id);
                FileInf folder = new FileInf();
                folder.id = id;
                DBFile.read(ref folder);

                //扫描文件夹
                fd_scan sc = new fd_scan();
                sc.scan(folder);

                //合并完毕
                DBFile.merged(id);
                ret = 1;
            }
            Response.Write(cak + "(" + ret + ")");
        }
    }
}