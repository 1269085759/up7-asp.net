using System;
using up7.db.biz.database;

namespace up7.down3.db
{
    public partial class f_complete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uid = Request.QueryString["uid"];
            string id  = Request.QueryString["id"];
            string cbk = Request.QueryString["callback"];

            if (string.IsNullOrEmpty(uid)
                || string.IsNullOrEmpty(id))
            {
                Response.Write(cbk + "(0)");
                return;
            }

            DBFileQueue db = new DBFileQueue();
            db.complete(id);

            Response.Write(cbk + "(1)");
        }
    }
}