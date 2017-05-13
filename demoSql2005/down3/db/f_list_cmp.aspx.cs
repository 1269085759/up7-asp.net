﻿using up6.demoSql2005.down3.biz;
using System;
using System.Web;

namespace up6.demoSql2005.down3.db
{
    /// <summary>
    /// 列出所有已经上传完的文件和文件夹
    /// 格式：
    /// json:
    ///     [{f1,f2,f3,f4,f5}]
    /// xdb_files
    /// 文件：xdb_files
    /// 文件夹：xdb_files.fd_json
    /// 
    /// </summary>
    public partial class f_list_cmp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uid = Request.QueryString["uid"];
            string cbk = Request.QueryString["callback"];//jsonp

            if (!string.IsNullOrEmpty(uid))
            {
                cmp_builder cb = new cmp_builder();
                string json = cb.read(int.Parse(uid));
                if (!string.IsNullOrEmpty(json))
                {
                    System.Diagnostics.Debug.WriteLine(json);
                    json = HttpUtility.UrlEncode(json);
                    //UrlEncode会将空格解析成+号
                    json = json.Replace("+", "%20");
                    Response.Write(cbk + "({\"value\":\"" + json + "\"})");
                    return;
                }
            }
            Response.Write(cbk+"({\"value\":null})");
        }
    }
}