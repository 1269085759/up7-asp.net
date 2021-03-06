﻿using System;
using up7.db.biz.database;
using up7.db.utils;

namespace up7.db
{
    /// <summary>
    /// 更新文件或文件夹进度信息
    /// 在停止时调用
    /// 在出错时调用
    /// </summary>
    public partial class f_update : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uid      = Request.QueryString["uid"];
            string id       = Request.QueryString["id"];
            string perSvr   = Request.QueryString["perSvr"];//文件百分比
            string blockSize= Request.QueryString["blockSize"];//本地文件大小

            //参数为空
            if (    string.IsNullOrEmpty(uid)
                ||  string.IsNullOrEmpty(id)
                )
            {
                XDebug.Output("uid", uid);
                XDebug.Output("idSvr", id);
                Response.Write("param is null");
                return;
            }

            DBFile.process(id, perSvr);
        }
    }
}