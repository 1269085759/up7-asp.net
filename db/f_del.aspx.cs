using System;
using up7.db.biz.database;
using up7.db.biz.redis;

namespace up7.db
{
    public partial class f_del : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            string uid = Request.QueryString["uid"];
            string cbk = Request.QueryString["callback"];
            int ret = 0;

            if (string.IsNullOrEmpty(id)
                || string.IsNullOrEmpty(uid))
            {
            }//参数不为空
            else
            {
                DBFileQueue db = new DBFileQueue();
                db.remove(id);
                ret = 1;
            }
            Response.Write(cbk + "(" + ret + ")");//返回jsonp格式数据
        }
    }
}