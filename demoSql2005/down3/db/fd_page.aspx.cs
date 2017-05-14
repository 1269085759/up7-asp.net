using System;
using System.Web;

namespace up7.demoSql2005.down3.db
{
    /// <summary>
    /// 获取文件夹数据，以分页方式获取，
    /// </summary>
    public partial class fd_page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["idSign"];//文件夹id,与up7_files.f_idSign对应
            string index = Request.QueryString["page"];//页数，基于1

            if (string.IsNullOrEmpty(id)
                || string.IsNullOrEmpty(index))
            {
                Response.Write("");
                return;
            }

            biz.fd_page p = new biz.fd_page();
            string json = p.read(index,id);
            Response.Write(json);
        }
    }
}