using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using up7.db.biz.database;
using up7.db.model;

namespace up7.down3.biz
{
    public class CompleteReader
    {
        public String all(int uid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select ");
            sb.Append(" f_idSign");//0
            sb.Append(",f_nameLoc");//1
            sb.Append(",f_lenLoc");//2
            sb.Append(",f_sizeLoc");//3
            sb.Append(",f_fdTask");//4
            sb.Append(",f_pathLoc");//5
            sb.Append(",f_pathSvr");//6
            sb.Append(",f_blockSize");//7
            sb.Append(",f_blockPath");//8
            sb.Append(",f_blockCount");//9
            sb.Append(",fd_files");//10
                                   //
            sb.Append(" from up7_files");
            sb.Append(" left join up7_folders on up7_folders.fd_sign=up7_files.f_idSign");
            //
            sb.Append(" where f_uid=@uid and f_complete=1 and f_fdChild=0");

            DbHelper db = new DbHelper();
            var cmd = db.GetCommand(sb.ToString());
            db.AddInt(ref cmd,"@uid",uid);
            List<FileInf> files = new List<FileInf>();
            using (var r = db.ExecuteReader(cmd))
            {
                while (r.Read())
                {
                    FileInf fi = new FileInf();
                    fi.id   = r.GetString(0);//与up7_files表对应
                    fi.nameLoc  = r.GetString(1);
                    fi.lenSvr   = r.GetInt64(2);
                    fi.sizeSvr  = r.GetString(3);
                    fi.folder   = r.GetBoolean(4);
                    fi.pathLoc  = r.GetString(5);
                    fi.pathSvr  = r.GetString(6);
                    //如果是文件夹则pathSvr保存本地路径，用来替换
                    if (fi.folder) fi.pathSvr = fi.pathLoc;
                    fi.blockSize = r.GetInt32(7);
                    fi.blockPath = r.GetString(8);
                    fi.blockCount = r.GetInt32(9);
                    fi.fileCount = r.IsDBNull(10) ? 0 : r.GetInt32(10);
                    files.Add(fi);
                }
            }

            return JsonConvert.SerializeObject(files);
        }
    }
}