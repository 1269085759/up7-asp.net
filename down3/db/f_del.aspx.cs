﻿using System;
using up7.db.biz.redis;
using tasks = up7.down3.biz.redis.tasks;

namespace up7.down3.db
{
    public partial class f_del : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string fid = Request.QueryString["signSvr"];
            string uid = Request.QueryString["uid"];
            string cbk = Request.QueryString["callback"];

            if (string.IsNullOrEmpty(uid)
                || string.IsNullOrEmpty(fid))
            {
                Response.Write(cbk + "({\"value\":null})");
                return;
            }

            var j = RedisConfig.getCon();
            tasks svr = new tasks(uid,j);
            svr.del(fid);

            Response.Write(cbk + "({\"value\":1})");
        }
    }
}