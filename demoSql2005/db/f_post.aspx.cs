using System;
using System.IO;
using System.Web;
using up7.demoSql2005.db.biz.redis;
using up7.demoSql2005.db.redis;

namespace up6.demoSql2005.db
{
    public partial class f_post : System.Web.UI.Page
    {
        /// <summary>
        /// 只负责拼接文件块。将接收的文件块数据写入到文件中。
        /// 更新记录：
        ///		2012-04-12 更新文件大小变量类型，增加对2G以上文件的支持。
        ///		2012-04-18 取消更新文件上传进度信息逻辑。
        ///		2012-10-30 增加更新文件进度功能。
        ///		2015-03-19 文件路径由客户端提供，此页面不再查询文件在服务端的路径。减少一次数据库访问操作。
        ///     2016-03-31 增加文件夹信息字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string uid          = Request.Headers["f-uid"];
            string idSign       = Request.Headers["f-idSign"];//
            string perSvr       = Request.Headers["f-perSvr"];//文件百分比
            string lenSvr       = Request.Headers["f-lenSvr"];//已传大小
            string lenLoc       = Request.Headers["f-lenLoc"];//本地文件大小
            string nameLoc      = Request.Headers["f-nameLoc"];//
            string pathLoc      = Request.Headers["f-pathLoc"];//
            string sizeLoc      = Request.Headers["f-sizeLoc"];//
            string f_pos        = Request.Headers["f-RangePos"];
            string rangeIndex   = Request.Headers["f-rangeIndex"];
            string rangeCount   = Request.Headers["f-rangeCount"];
            //string complete     = Request.Headers["complete"];//true/false
            string fd_idSign     = Request.Headers["fd-idSign"];//文件夹标识(guid)
            string fd_lenSvr    = Request.Headers["fd-lenSvr"];//文件夹已传大小
            string fd_perSvr    = Request.Headers["fd-perSvr"];//文件夹百分比
            pathLoc = pathLoc.Replace("+", "%20");
            pathLoc = HttpUtility.UrlDecode(pathLoc);
            nameLoc = pathLoc.Replace("+", "%20");
            nameLoc = HttpUtility.UrlDecode(nameLoc);

            //参数为空
            if (string.IsNullOrEmpty(lenLoc)
                || string.IsNullOrEmpty(uid)
                || string.IsNullOrEmpty(idSign)
                || string.IsNullOrEmpty(f_pos)
                || string.IsNullOrEmpty(nameLoc))
            {
                XDebug.Output("lenLoc", lenLoc);
                XDebug.Output("uid", uid);
                XDebug.Output("idSvr", idSign);
                XDebug.Output("nameLoc", nameLoc);
                XDebug.Output("pathLoc", pathLoc);
                XDebug.Output("fd-idSvr", fd_idSign);
                XDebug.Output("fd-lenSvr", fd_lenSvr);
                Response.Write("param is null");
                return;
            }

            //有文件块数据
            if (Request.Files.Count > 0)
            {
                long rangePos = Convert.ToInt64(f_pos);

                HttpPostedFile part = Request.Files.Get(0);
                var con = RedisConfig.getCon();
                file f_svr = new file(ref con);
                string partPath = f_svr.getPartPath(idSign, rangeIndex, rangeCount);

                //文件块
                if (string.IsNullOrEmpty(fd_idSign))
                {
                    //更新文件进度
                    if (f_pos == "0") f_svr.process(idSign, perSvr, lenSvr, rangeCount);
                }//子文件块
                else
                {
                    //向redis添加子文件信息
                    xdb_files f_child = new xdb_files();
                    f_child.blockCount = int.Parse(rangeCount);
                    f_child.idSign = idSign;
                    f_child.nameLoc = System.IO.Path.GetFileName(pathLoc);
                    f_child.nameSvr = nameLoc;
                    f_child.lenLoc = long.Parse(lenLoc);
                    f_child.sizeLoc = sizeLoc;
                    f_child.pathLoc = pathLoc.Replace("\\", "/");//路径规范化处理
                    f_child.rootSign = fd_idSign;
                    f_child.blockCount = int.Parse(rangeCount);
                    f_svr.create(f_child);

                    //添加到文件夹
                    fd_files_redis root = new fd_files_redis(ref con, fd_idSign);
                    root.add(idSign);
                    
                    //更新文件夹进度
                    if (f_pos == "0") f_svr.process(fd_idSign, fd_perSvr, fd_lenSvr,"0");

                    //块路径
                    partPath = f_svr.getPartPath(idSign, rangeIndex, rangeCount, fd_idSign);
                }


                //自动创建目录
                if (!Directory.Exists(partPath)) Directory.CreateDirectory(Path.GetDirectoryName(partPath));
                part.SaveAs(partPath);


                Response.Write("ok");
            }
        }
    }
}