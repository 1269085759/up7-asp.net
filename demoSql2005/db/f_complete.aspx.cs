﻿using System;

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
            string sign = Request.QueryString["sign"];
            string fid = Request.QueryString["idSvr"];
            string cbk = Request.QueryString["callback"];
            string fd_idSvr = Request.QueryString["fd_idSvr"];

            //返回值。1表示成功
            int ret = 0;

            if (    string.IsNullOrEmpty(uid)
                ||  string.IsNullOrEmpty(fid))
            {
            }//参数不为空
            else
            {
                DBFile db = new DBFile();
                db.complete(int.Parse(uid),int.Parse(fid));
                ret = 1;
            }

            //更新文件夹已上传文件数
            if (!string.IsNullOrEmpty(fd_idSvr))
            {
                DBFolder.child_complete(int.Parse(fd_idSvr));
            }
            Response.Write(cbk + "(" + ret + ")");//必须返回jsonp格式数据
        }
    }
}