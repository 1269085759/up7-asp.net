﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using up7.db.biz.database;
using up7.db.model;

namespace up7.down3.biz
{
    public class fd_page
    {

        /**
         * 
         * @param pageIndex 页索引，基于1
         */
        public String read(String pageIndex, String id)
        {
            int pageSize = 100;//每页100条数据
            int index = int.Parse(pageIndex);
            index = Math.Max(index, 1);//基于1
            int pageStart = ((index - 1) * pageSize) + 1;
            int pageEnd = index * pageSize;
            string sql = string.Format(@"select * from 
                                        (
	                                        select f_nameLoc,f_pathSvr,f_pathRel,f_lenLoc,f_sizeLoc,f_blockPath,f_blockSize,ROW_NUMBER() OVER(Order by (select null) ) as RowNumber from up7_files where f_pidRoot='{0}'
                                        )a
                                        where RowNumber BETWEEN {1} and {2}
                                        ", id, pageStart, pageEnd);

            List<FileInf> files = new List<FileInf>();
            DbHelper db = new DbHelper();
            using (var cmd = db.GetCommand(sql))
            {
                using (var r = db.ExecuteReader(cmd))
                {
                    while (r.Read())
                    {
                        var f = new FileInf();
                        f.nameLoc = r.GetString(0);//f_nameLoc
                        f.pathSvr = r.GetString(1);
                        f.pathRel = r.GetString(2);
                        f.lenSvr = r.GetInt64(3);
                        f.sizeSvr = r.GetString(4);
                        f.blockPath = r.GetString(5);
                        f.blockSize = r.GetInt32(6);
                        files.Add(f);
                    }
                    r.Close();
                }
            }

            if (files.Count > 0) return JsonConvert.SerializeObject(files);
            return "";
        }
    }
}