using System;

namespace up6.demoSql2005.db
{
    public partial class fd_complete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id  = Request.QueryString["idSign"];
            string uid = Request.QueryString["uid"];
            string cak = Request.QueryString["callback"];
            int ret = 0;

            //参数为空
            if (!string.IsNullOrEmpty(id) )
            {
                Jedis j = JedisTool.con();
                fd_redis fd = new fd_redis(j);
                fd.read(sign);

                //清除缓存
                tasks svr = new tasks(j);
                svr.uid = uid;
                svr.delFd(sign);
                j.close();

                fd.mergeAll();//合并文件块
                fd.saveToDb();//保存到数据库
                ret = 1;
            }
            Response.Write(cak + "(" + ret + ")");
        }
    }
}