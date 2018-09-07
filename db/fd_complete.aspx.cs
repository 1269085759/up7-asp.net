using System;
using up7.db.biz.database;
using up7.db.biz.redis;

namespace up7.db
{
    public partial class fd_complete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id    = Request.QueryString["id"];
            string uid   = Request.QueryString["uid"];
            string merge = Request.QueryString["merge"];
            string cak   = Request.QueryString["callback"];
            int ret = 0;

            //参数为空
            if (!string.IsNullOrEmpty(id) )
            {
                DBFile.complete(id);

                var rd = RedisConfig.getCon();
                RedisFolder fd = new RedisFolder(ref rd);
                fd.id = id;
                fd.fileMerge = merge.Equals("1");
                fd.saveToDb();//保存到数据库

                rd.Dispose();

                //合并完毕
                DBFile.merged(id);
                ret = 1;
            }
            Response.Write(cak + "(" + ret + ")");
        }
    }
}