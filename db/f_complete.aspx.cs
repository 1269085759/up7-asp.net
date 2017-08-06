using System;
using up7.db.biz;
using up7.db.biz.database;
using up7.db.biz.redis;

namespace up7.db
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
            string merge = Request.QueryString["merge"];
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
                RedisFile cache = new RedisFile(ref j);
                var fileSvr = cache.read(fid);

                //合并块
                if (merge == "1")
                {
                    BlockMeger pm = new BlockMeger();
                    pm.merge(fileSvr);
                }
                j.Del(fid);

                //从任务列表（未完成）中删除
                tasks svr = new tasks(ref j);
                svr.uid = uid;
                svr.del(fid);
                j.Dispose();

                //添加到数据库
                DBFile db = new DBFile();
                db.addComplete(ref fileSvr);
                ret = 1;
            }
            
            Response.Write(cbk + "(" + ret + ")");//必须返回jsonp格式数据
        }
    }
}