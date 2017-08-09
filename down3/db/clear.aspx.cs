using System;
using up7.down3.biz;

namespace up7.down3.db
{
    public partial class clear : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DnFile.Clear();
        }
    }
}