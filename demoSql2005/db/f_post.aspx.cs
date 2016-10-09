﻿using System;
using System.Web;

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
            string uid          = Request.Form["uid"];
            string idSvr        = Request.Form["idSvr"];//与up6_files对应
            string perSvr       = Request.Form["perSvr"];//文件百分比
            string lenSvr       = Request.Form["lenSvr"];//已传大小
            string lenLoc       = Request.Form["lenLoc"];//本地文件大小
            string f_pos        = Request.Form["RangePos"];
            string complete     = Request.Form["complete"];//true/false
            string fd_idSvr     = Request.Form["fd-idSvr"];//文件夹ID,与up6_folders对应
            string fd_lenSvr    = Request.Form["fd-lenSvr"];//文件夹已传大小
            string fd_perSvr    = Request.Form["fd-perSvr"];//文件夹百分比
            string pathSvr      = Request.Form["pathSvr"];//add(2015-03-19):
            pathSvr             = HttpUtility.UrlDecode(pathSvr);

            //参数为空
            if (string.IsNullOrEmpty(lenLoc)
                || string.IsNullOrEmpty(uid)
                || string.IsNullOrEmpty(idSvr)
                || string.IsNullOrEmpty(f_pos)
                || string.IsNullOrEmpty(pathSvr))
            {
                XDebug.Output("lenLoc", lenLoc);
                XDebug.Output("uid", uid);
                XDebug.Output("idSvr", idSvr);
                XDebug.Output("pathSvr", pathSvr);
                XDebug.Output("fd-idSvr", fd_idSvr);
                XDebug.Output("fd-lenSvr", fd_lenSvr);
                Response.Write("param is null");
                return;
            }

            //有文件块数据
            if (Request.Files.Count > 0)
            {
                long rangePos = Convert.ToInt64(f_pos);

                //临时文件大小
                HttpPostedFile file = Request.Files.Get(0);

                XDebug.Output("lenLoc", lenLoc);
                XDebug.Output("uid", uid);
                XDebug.Output("idSvr", idSvr);
                XDebug.Output("rangePos", rangePos);
                XDebug.Output("lenSvr", lenSvr);
                XDebug.Output("perSvr", perSvr);
                XDebug.Output("fd_idSvr", fd_idSvr);
                XDebug.Output("fd_lenSvr", fd_lenSvr);
                XDebug.Output("fd_perSvr", fd_perSvr);

                //2.0保存文件块数据
                FileBlockWriter res = new FileBlockWriter();
                res.write(pathSvr, rangePos, ref file);

                bool fd = !string.IsNullOrEmpty(fd_idSvr);
                if (fd) fd = !string.IsNullOrEmpty(fd_lenSvr);
                if(fd)  fd = int.Parse(fd_idSvr) > 0;
                if(fd) fd = long.Parse(fd_lenSvr) > 0;
                
                //第一块数据
                if(rangePos==0)
                {
                    //文件夹进度
                    DBFile db = new DBFile();
                    if (fd)
                    {
                        db.fd_fileProcess(Convert.ToInt32(uid), Convert.ToInt32(idSvr), rangePos, Convert.ToInt64(lenSvr), perSvr, Convert.ToInt32(fd_idSvr), Convert.ToInt64(fd_lenSvr), fd_perSvr, complete == "true");
                    }//文件进度
                    else
                    {
                        db.f_process(Convert.ToInt32(uid), Convert.ToInt32(idSvr), rangePos, file.InputStream.Length, perSvr, complete == "true");
                    }
                }

                Response.Write("ok");
            }
        }
    }
}