using System.Web;
using up7.db.model;

namespace up7.db.biz
{
    /// <summary>
    /// 路径生成器基类
    /// 提供文件或文件夹的存储路径
    /// </summary>
    public class PathBuilder
    {
        /// <summary>
        /// 根级存储路径,格式：up7\\upload
        /// </summary>
        /// <returns></returns>
        public string getRoot()
        {
            return HttpContext.Current.Server.MapPath("/upload");
        }

        public virtual string genFolder(int uid,string nameLoc)
        {
            return string.Empty;
        }

        public virtual string genFolder(int uid, ref FolderInf fd)
        {
            return string.Empty;
        }

        public virtual string genFile(int uid, ref FileInf f)
        {
            return string.Empty;
        }
        public virtual string genFile(int uid, string md5, string nameLoc)
        {
            return string.Empty;
        }
    }
}