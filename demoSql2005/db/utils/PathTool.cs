using System.Web;

namespace up7.demoSql2005.db.utils
{
    public class PathTool
    {
        public static string url_decode(string v)
        {
            v = v.Replace("+", "%20");
            v = HttpUtility.UrlDecode(v);//utf-8解码
            return v;
        }
    }
}