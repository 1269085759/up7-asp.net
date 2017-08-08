using System;
using System.Web;
using up7.down3.biz;

namespace up7.down3.db
{
    /// <summary>
    /// 从up7_files表中加载所有已经上传完毕的文件和文件夹
    /// </summary>
    public partial class f_list_cmp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uid = Request.QueryString["uid"];
            string cbk = Request.QueryString["callback"];//jsonp

            if (!string.IsNullOrEmpty(uid))
            {
                DnFile db = new DnFile();
                string json = db.all_complete(int.Parse(uid) );
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