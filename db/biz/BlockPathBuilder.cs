using System.IO;
using up7.db.model;

namespace up7.db.biz
{
    /// <summary>
    /// 文件块路径构建器
    /// </summary>
    public class BlockPathBuilder
    {
        /// <summary>
        /// 生成文件块路径
        /// 格式：
        ///   文件夹：
        ///     d:/webapps/folder-1/file-1-guid/1.part
        ///   文件：
        ///     d:/webapps/year/年/月/日/file-1-guid/1.part
        /// </summary>
        /// <param name="id"></param>
        /// <param name="blockIndex"></param>
        /// <param name="pathSvr"></param>
        /// <returns></returns>
        public string part(string id,string blockIndex,string pathSvr)
        {
            System.IO.FileInfo f = new System.IO.FileInfo(pathSvr);

            //d:\\soft
            pathSvr = Path.Combine(f.DirectoryName,id);
            pathSvr = Path.Combine(pathSvr, blockIndex + ".part");
            pathSvr = pathSvr.Replace("\\", "/");
            return pathSvr;
        }

        /// <summary>
        /// 文件块根路径
        /// d:/webapps/files/年/月/日/file-guid/
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pathSvr"></param>
        /// <returns></returns>
        public string root(string id,string pathSvr)
        {
            FileInfo f = new System.IO.FileInfo(pathSvr);
            pathSvr = Path.Combine(f.DirectoryName, id);
            pathSvr = pathSvr.Replace("\\", "/");
            return pathSvr;
        }
    }
}