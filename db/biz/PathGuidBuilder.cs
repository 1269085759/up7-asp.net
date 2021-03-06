﻿using System;
using System.IO;

namespace up7.db.biz
{
    public class PathGuidBuilder : PathBuilder
    {
        /// <summary>
        /// 不创建文件夹，所有文件统一以日期格式存储
        /// 格式：upload/年/月/日/guid
        /// 示例：upload/2017/08/06/guid
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="fd"></param>
        /// <returns></returns>
        public override string genFolder(int uid, string guid)
        {
            DateTime timeCur = DateTime.Now;
            string path = Path.Combine(this.getRoot(), timeCur.ToString("yyyy"));
            path = Path.Combine(path, timeCur.ToString("MM"));
            path = Path.Combine(path, timeCur.ToString("dd"));
            path = Path.Combine(path, guid);

            return path;
        }

        /// <summary>
        /// 路径格式：upload/2016/09/30/guid/QQ2013.exe
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="guid"></param>
        /// <param name="nameLoc"></param>
        /// <returns></returns>
        public override string genFile(int uid, string guid, string nameLoc)
        {
            DateTime timeCur = DateTime.Now;
            string path = Path.Combine(this.getRoot(), timeCur.ToString("yyyy"));
            path = Path.Combine(path, timeCur.ToString("MM"));
            path = Path.Combine(path, timeCur.ToString("dd"));
            path = Path.Combine(path, guid);
            path = Path.Combine(path, nameLoc);
            path = path.Replace("\\", "/");

            return path;
        }
    }
}