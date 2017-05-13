using System;
using System.IO;
using up7.demoSql2005.db.biz;
using up7.demoSql2005.db.biz.redis;
using up7.demoSql2005.db.redis;

namespace up7.demoSql2005.db
{
    public partial class clear : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DBFile.Clear();
            DBFolder.Clear();


            Response.Write("数据库清除成功<br/>");
            var j = RedisConfig.getCon();
            tasks t = new tasks(ref j);
            t.clear();
            Response.Write("redis缓存清除成功<br/>");

            //删除upload文件夹
            PathBuilder pb = new PathBuilder();
            string pathSvr = pb.getRoot();
            if(Directory.Exists(pathSvr)) Directory.Delete(pathSvr,true);
        }
    }
}