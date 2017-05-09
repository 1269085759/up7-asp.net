using System;
using up7.demoSql2005.db.biz.redis;
using up7.demoSql2005.db.redis;

namespace up6.demoSql2005.db
{
    /// <summary>
    /// 此文件处理单文件上传
    /// </summary>
    public partial class f_complete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uid = Request.QueryString["uid"];
            string fid = Request.QueryString["idSign"];
            string cbk = Request.QueryString["callback"];

            //返回值。1表示成功
            int ret = 0;

            if (    string.IsNullOrEmpty(uid)
                ||  string.IsNullOrEmpty(fid))
            {
            }//参数不为空
            else
            {
                var j = RedisConfig.getCon();
                var f_svr = new fd_file_redis();
                f_svr.read(j, fid);
                f_svr.merge();
                j.Del(fid);

                //从任务列表（未完成）中删除
                tasks svr = new tasks(ref j);
                svr.uid = uid;
                svr.del(fid);
                j.Dispose();
                ret = 1;
            }
            
            Response.Write(cbk + "(" + ret + ")");//必须返回jsonp格式数据
        }
    }
}