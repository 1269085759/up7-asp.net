﻿using System;
using up7.db.biz.redis;
using up7.db.utils;
using FileRedis = up7.down3.biz.redis.FileRedis;

namespace up7.down3.db
{
    /// <summary>
    /// 更新文件下载进度
    /// </summary>
    public partial class f_update : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string fid      = Request.QueryString["signSvr"];
            string uid      = Request.QueryString["uid"];
            string lenLoc   = Request.QueryString["lenLoc"];
            string sizeLoc  = Request.QueryString["sizeLoc"];
            string per      = Request.QueryString["perLoc"];
            string cbk      = Request.QueryString["callback"];
            sizeLoc = PathTool.url_decode(sizeLoc);
            per = PathTool.url_decode(per);
            //
            if (    string.IsNullOrEmpty(fid)
                ||  string.IsNullOrEmpty(cbk)
                ||  string.IsNullOrEmpty(lenLoc))
            {
                Response.Write(cbk+"({\"value\":0})");
                Response.End();
                return;
            }
            var j = RedisConfig.getCon();
            FileRedis f = new FileRedis(ref j);
            f.process(fid, per, long.Parse(lenLoc),sizeLoc);//更新文件或文件夹进度
            
            Response.Write(cbk + "({\"value\":1})");
        }
    }
}