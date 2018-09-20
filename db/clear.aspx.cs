using System;
using up7.db.biz;
using up7.db.biz.database;

namespace up7.db
{
    public partial class clear : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DBFile.Clear();
            DBFolder.Clear();

            Response.Write("数据库清除成功<br/>");

            //删除upload文件夹
            PathBuilder pb = new PathBuilder();
            string pathSvr = pb.getRoot();
            //if(Directory.Exists(pathSvr)) Directory.Delete(pathSvr,true);
        }
    }
}