using System.Web;

namespace up7.db.utils
{
    public class PathTool
    {
        public static string url_decode(string v)
        {
            if (string.IsNullOrEmpty(v)) return string.Empty;
            v = v.Replace("+", "%20");
            v = HttpUtility.UrlDecode(v);//utf-8解码
            return v;
        }
    }
}