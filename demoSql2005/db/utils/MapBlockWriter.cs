using System.IO;
//using System.IO.MemoryMappedFiles;
using System.Web;

namespace up7.demoSql2005.db
{
    /// <summary>
    /// 基于内存映射文件的文件块保存类
    /// 1.可提高大文件保存的效率
    /// 2.减少系统IO
    /// </summary>
    public class MapBlockWriter
    {
        /// <summary>
        /// 创建一个文件
        /// </summary>
        /// <param name="path"></param>
        public void makeFile(string path,long len)
        {
            long mb = 1048576;//1mb
            long gb = 1073741824;//1GB

            //自动创建目录
            if (!Directory.Exists(path)) Directory.CreateDirectory(Path.GetDirectoryName(path));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">本地文件路径</param>
        /// <param name="offset">块索引</param>
        /// <param name="length">块大小</param>
        public void write(string path,long offset, ref HttpPostedFile block)
        {
            long length = block.InputStream.Length;
        }
    }
}