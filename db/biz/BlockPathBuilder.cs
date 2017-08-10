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
        /// <param name="idSign"></param>
        /// <param name="blockIndex"></param>
        /// <param name="pathSvr"></param>
        /// <returns></returns>
        public string part(string idSign,string blockIndex,string pathSvr)
        {
            System.IO.FileInfo f = new System.IO.FileInfo(pathSvr);

            //d:\\soft
            pathSvr = Path.Combine(f.DirectoryName,idSign);
            pathSvr = Path.Combine(pathSvr, blockIndex + ".part");
            pathSvr = pathSvr.Replace("\\", "/");
            return pathSvr;
        }

        /// <summary>
        /// 文件块根路径
        /// d:/webapps/files/年/月/日/file-guid/
        /// </summary>
        /// <param name="idSign"></param>
        /// <param name="pathSvr"></param>
        /// <returns></returns>
        public string root(string idSign,string pathSvr)
        {
            FileInfo f = new System.IO.FileInfo(pathSvr);
            pathSvr = Path.Combine(f.DirectoryName, idSign);
            pathSvr = pathSvr.Replace("\\", "/");
            return pathSvr;
        }

        /// <summary>
        /// 文件夹子文件块路径
        ///   d:/webapps/files/年/月/日/folder/file-guid/1.part
        /// </summary>
        /// <param name="idSign"></param>
        /// <param name="blockIndex"></param>
        /// <param name="pathSvr"></param>
        /// <returns></returns>
        public string partFd(ref FileInf f,string blockIndex,ref FileInf fd)
        {
            string pathSvr = fd.pathSvr;
            pathSvr = Path.Combine(pathSvr, f.pathRel);
            pathSvr = Path.Combine(pathSvr, blockIndex + ".part");
            return pathSvr;
        }

        /// <summary>
        /// 文件夹子文件块根目录
        /// d:/webapps/files/年/月/日/folder/folder-child/file-guid/
        /// </summary>
        /// <param name="idSign"></param>
        /// <param name="blockIndex"></param>
        /// <param name="fd"></param>
        /// <returns></returns>
        public string rootFd(ref FileInf f,string blockIndex,ref FileInf fd)
        {
            string pathSvr = fd.pathSvr;
            int pos = f.pathRel.LastIndexOf("\\");
            if(pos==-1)
            {
                pathSvr = Path.Combine(pathSvr, f.id);
            }
            else
            {
                string rel = f.pathRel.Substring(0, pos);
                pathSvr = Path.Combine(pathSvr, rel);
                pathSvr = Path.Combine(pathSvr, f.id);
            }
            return pathSvr;
        }
    }
}