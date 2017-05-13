using System;
using up7.demoSql2005.db.redis;

namespace up7.demoSql2005.down3.debug
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var p = System.IO.Path.Combine("d:\\soft\\", "image");
            Response.Write(p);
            Response.Write("<br/>");

            p = System.IO.Path.Combine("d:\\soft", "image2");
            Response.Write(p);
            Response.Write("<br/>");

            p = System.IO.Path.Combine("d:/soft/", "image2");
            Response.Write(p);
            Response.Write("<br/>");
        }
    }
}