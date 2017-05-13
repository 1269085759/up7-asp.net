using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using up7.demoSql2005.db;

namespace up7.demoSql2005.down3.biz
{
    public class CompleteReader
    {
        public String all(int uid)
        {
            List<cmp_file> files = new List<cmp_file>();
            StringBuilder sb = new StringBuilder();
            sb.Append("select ");
            sb.Append(" f_idSign");//1
            sb.Append(",f_nameLoc");//2
            sb.Append(",f_lenLoc");//3
            sb.Append(",f_sizeLoc");//4
            sb.Append(",f_fdTask");//5
            sb.Append(",f_pathLoc");//6
            sb.Append(",fd_files");//7
                                   //
            sb.Append(" from up7_files");
            sb.Append(" left join up7_folders on up7_folders.fd_sign=up7_files.f_idSign");
            //
            sb.Append(" where f_uid=@uid and f_complete=1 and f_fdChild=0");

            DbHelper db = new DbHelper();
            var cmd = db.GetCommand(sb.ToString());
            db.AddInt(ref cmd,"@uid",uid);
            using (var r = db.ExecuteReader(cmd))
            {
                while (r.Read())
                {
                    cmp_file fi = new cmp_file();
                    fi.idSign = r.GetString(1);//与up7_files表对应
                    fi.nameLoc = r.GetString(2);
                    fi.lenSvr = r.GetInt64(3);
                    fi.pathLoc = r.GetString(6);
                    fi.sizeSvr = r.GetString(4);
                    fi.fdTask = r.GetBoolean(5);
                    //如果是文件夹则pathSvr保存本地路径，用来替换
                    if (fi.fdTask) fi.pathSvr = fi.pathLoc;
                    fi.signSvr = System.Guid.NewGuid().ToString();//服务端生成，唯一标识
                    fi.filesCount = r.GetInt32(7);
                    files.Add(fi);
                }
            }

            return JsonConvert.SerializeObject(files);
        }
    }
}