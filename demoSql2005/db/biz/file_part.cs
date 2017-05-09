using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace up7.demoSql2005.db.biz
{
    public class file_part
    {
        public void save(string path,ref HttpPostedFile data)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            //创建文件夹：目录/guid/1

        }
    }
}