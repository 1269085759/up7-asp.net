﻿using System;
using up7.db.utils;
using up7.down3.biz;

namespace up7.down3.db
{
    /// <summary>
    /// 更新文件下载进度
    /// </summary>
    public partial class f_update : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string fid      = Request.QueryString["id"];
            string uid      = Request.QueryString["uid"];
            string sizeLoc   = Request.QueryString["sizeLoc"];
            string per      = Request.QueryString["perLoc"];
            string cbk      = Request.QueryString["callback"];
            per = PathTool.url_decode(per);
            sizeLoc = PathTool.url_decode(sizeLoc);
            //
            if (    string.IsNullOrEmpty(fid)
                ||  string.IsNullOrEmpty(cbk)
                ||  string.IsNullOrEmpty(sizeLoc))
            {
                Response.Write(cbk+"({\"value\":0})");
                Response.End();
                return;
            }

            DnFile db = new DnFile();
            db.process(fid, int.Parse(uid), sizeLoc, per);
            
            Response.Write(cbk + "({\"value\":1})");
        }
    }
}