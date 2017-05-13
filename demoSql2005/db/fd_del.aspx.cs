﻿using System;
using up7.demoSql2005.db;

namespace up7.demoSql2005.db
{
    public partial class fd_del : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string fid = Request.QueryString["fid"];
            string fdID = Request.QueryString["fd_id"];
            string uid = Request.QueryString["uid"];
            string cbk = Request.QueryString["callback"];
            int ret = 0;

            if (string.IsNullOrEmpty(fid)
                || string.IsNullOrEmpty(uid)
                || string.IsNullOrEmpty(fdID)
                )
            {
            }//参数不为空
            else
            {
                DBFile db = new DBFile();
                DBFolder.Remove(int.Parse(fid), int.Parse(fdID), int.Parse(uid));
                ret = 1;
            }
            Response.Write(cbk + "({\"value\":" + ret + "})");//返回jsonp格式数据
        }
    }
}