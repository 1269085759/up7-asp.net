using System;
using System.IO;
using up6.demoSql2005.db.biz;

namespace up6.demoSql2005.db
{
    public partial class clear : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DBFile.Clear();
            DBFolder.Clear();

            //删除upload文件夹
            PathBuilder pb = new PathBuilder();
            string pathSvr = pb.getRoot();
            if(Directory.Exists(pathSvr)) Directory.Delete(pathSvr,true);
        }
    }
}