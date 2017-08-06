using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using up7.db.model;

namespace up7.db.biz
{
    /// <summary>
    /// 文件块合并逻辑
    /// </summary>
    public class BlockMeger
    {
        /// <summary>
        /// 将所有文件块合并成一个文件
        ///   目标文件,
        ///     D:/webapps/files/年/月/日/guid/QQ2013.exe
        ///   块路径：
        ///     D:/webapps/files/年/月/日/guid/file-idSign/
        /// </summary>
        /// <param name="pathSvr"></param>
        /// <param name="blockPath"></param>
        /// <param name="blockCount">文件块总数</param>
        public void merge(FileInf fileSvr)
        {
            if (File.Exists(fileSvr.pathSvr)) return;//文件已存在

            //创建目标文件夹
            var fd = Path.GetDirectoryName(fileSvr.pathSvr);
            if (!Directory.Exists(fd)) Directory.CreateDirectory(fd);

            //取文件块
            String[] parts = Directory.GetFiles(fileSvr.blockPath);
            long prevLen = 0;

            using (var mapFile = MemoryMappedFile.CreateFromFile(fileSvr.pathSvr, FileMode.CreateNew, fileSvr.id, fileSvr.lenLoc))
            {

                for (int i = 0, l = parts.Length; i < l; ++i)
                {
                    String partName = Path.Combine(fd,fileSvr.id,(i + 1) + ".part");
                    var partData = File.ReadAllBytes(partName);
                    //每一个文件块为64mb，最后一个文件块<=64mb
                    long partOffset = prevLen;
                    using (var ss = mapFile.CreateViewStream(partOffset, partData.Length))
                    {
                        ss.Write(partData, 0, partData.Length);
                    }
                    prevLen += partData.Length;
                }
            }

            Directory.Delete(fileSvr.blockPath, true);
        }
    }
}