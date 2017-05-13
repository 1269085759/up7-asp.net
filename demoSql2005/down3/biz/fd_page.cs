using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using up7.demoSql2005.db;
using up7.demoSql2005.db.biz.folder;

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
            String sqlData = "select f_nameLoc,f_pathLoc,f_pathSvr,f_lenLoc,f_sizeLoc from up7_files where f_rootSign='" + id + "'";
            String sql = String.Format("select * from (select a.*, rownum rn from (%s) a where rownum <= %d) where rn >= %d", sqlData, pageEnd, pageStart);

            List<fd_file> files = new List<fd_file>();
            DbHelper db = new DbHelper();
            using (var cmd = db.GetCommand(sql))
            {
                using (var r = db.ExecuteReader(cmd))
                {
                    while (r.Read())
                    {
                        fd_file f = new fd_file();
                        f.nameLoc = r.GetString(1);//f_nameLoc
                        f.pathLoc = r.GetString(2);//f_pathLoc
                        f.pathSvr = r.GetString(3);
                        f.lenLoc = r.GetInt64(4);
                        f.sizeLoc = r.GetString(5);
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