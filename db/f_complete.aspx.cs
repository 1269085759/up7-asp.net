using System;
using up7.db.biz;
using up7.db.biz.database;

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
            string uid   = Request.QueryString["uid"];
            string id    = Request.QueryString["id"];
            string cbk   = Request.QueryString["callback"];

            //返回值。1表示成功
            int ret = 0;

            if (    string.IsNullOrEmpty(uid)
                ||  string.IsNullOrEmpty(id))
            {
            }//参数不为空
            else
            {
                //标识已完成
                DBFile.complete(id);

                //合并完毕
                DBFile.merged(id);
                ret = 1;

                //触发事件
                up7_biz_event.file_post_complete(id);
            }
            
            Response.Write(cbk + "(" + ret + ")");//必须返回jsonp格式数据
        }
    }
}