using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using up7.demoSql2005.db;

namespace up7.demoSql2005.down3.biz
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
	                                        select f_nameLoc,f_pathLoc,f_pathSvr,f_pathRel,f_lenLoc,f_sizeLoc,f_blockPath,f_blockSize,ROW_NUMBER() OVER(Order by (select null) ) as RowNumber from up7_files where f_rootSign='{0}'
                                        )a
                                        where RowNumber BETWEEN {1} and {2}
                                        ", id, pageStart, pageEnd);

            List<xdb_files> files = new List<xdb_files>();
            DbHelper db = new DbHelper();
            using (var cmd = db.GetCommand(sql))
            {
                using (var r = db.ExecuteReader(cmd))
                {
                    while (r.Read())
                    {
                        var f = new xdb_files();
                        f.nameLoc = r.GetString(0);//f_nameLoc
                        f.pathLoc = r.GetString(1);//f_pathLoc
                        f.pathSvr = r.GetString(2);
                        f.pathRel = r.GetString(3);
                        f.lenLoc = r.GetInt64(4);
                        f.sizeLoc = r.GetString(5);
                        f.blockPath = r.GetString(6);
                        f.blockSize = r.GetInt32(7);
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