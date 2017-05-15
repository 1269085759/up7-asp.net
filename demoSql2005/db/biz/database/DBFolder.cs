using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Newtonsoft.Json.Linq;

namespace up7.demoSql2005.db
{
    public class DBFolder
    {
        static public void Clear()
        {
            string sql = "delete from up7_folders";
            DbHelper db = new DbHelper();
            DbCommand cmd = db.GetCommand(sql);
            db.ExecuteNonQuery(cmd);
        }
        /// <summary>
        /// 根据文件夹ID获取文件夹信息和未上传完的文件列表，转为JSON格式。
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        static public string GetFilesUnComplete(string fid)
        {
            return GetFilesUnComplete(fid);
        }
    }
}