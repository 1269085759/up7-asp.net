using System;
using up7.db.biz;
using up7.db.biz.database;
using up7.db.biz.redis;

namespace up7.db
{
    /// <summary>
    /// 文件上传完毕
    ///  处理是否合并的逻辑
    /// </summary>
    public partial class f_complete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uid = Request.QueryString["uid"];
            string id = Request.QueryString["id"];
            string merge = Request.QueryString["merge"];
            string cbk = Request.QueryString["callback"];

            //返回值。1表示成功
            int ret = 0;

            if (    string.IsNullOrEmpty(uid)
                ||  string.IsNullOrEmpty(id))
            {
            }//参数不为空
            else
            {
                DBFileQueue db = new DBFileQueue();
                var fileSvr = db.read(id);

                //合并块
                if (merge == "1")
                {
                    BlockMeger pm = new BlockMeger();
                    pm.merge(fileSvr);
                }

                //添加到数据库
                DBFileQueue q = new DBFileQueue();
                q.complete(id);
                ret = 1;
            }
            
            Response.Write(cbk + "(" + ret + ")");//必须返回jsonp格式数据
        }
    }
}