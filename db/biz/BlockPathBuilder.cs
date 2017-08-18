﻿using System.IO;
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
        ///     d:/webapps/year/年/月/日/file-id/blocks/1.part
        /// </summary>
        /// <param name="id"></param>
        /// <param name="blockIndex"></param>
        /// <param name="pathSvr"></param>
        /// <returns></returns>
        public string part(string id,string blockIndex,string pathSvr)
        {
            string part = this.root(id, pathSvr);
            part        = Path.Combine(part, blockIndex + ".part");
            part        = part.Replace("\\", "/");
            return part;
        }

        /// <summary>
        /// 文件块根路径
        /// d:/webapps/files/年/月/日/file-id/blocks
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pathSvr"></param>
        /// <returns></returns>
        public string root(string id,string pathSvr)
        {
            string parent = Path.GetDirectoryName(pathSvr);
            pathSvr    = Path.Combine(parent, "blocks");
            pathSvr    = pathSvr.Replace("\\", "/");
            return pathSvr;
        }

        /// <summary>
        /// 子文件块根路径
        /// d:/webapps/files/年/月/日/folder-id/folder-name/file-id/blocks
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pathSvr"></param>
        /// <returns></returns>
        public string rootFD(string id,string pathSvr)
        {
            string parent = Path.GetDirectoryName(pathSvr);
            pathSvr = Path.Combine(parent, id,"blocks");
            pathSvr = pathSvr.Replace("\\", "/");
            return pathSvr;
        }
    }
}