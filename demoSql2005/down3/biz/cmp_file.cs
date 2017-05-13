using System.Data.Common;

namespace up7.demoSql2005.down3.biz
{
    public class cmp_file : model.DnFileInf
    {
        //对文件夹的支持
        public int filesCount = 0;
        public cmp_file() { }

        public void read(int pidRoot,ref DbDataReader r)
        {
            this.idSvr = r.GetInt32(0);//
            this.nameLoc = (string)r["f_nameLoc"];
            this.pathLoc = (string)r["f_pathLoc"];//
            this.lenSvr = (long)r["f_lenSvr"];
            this.sizeSvr = (string)r["f_sizeLoc"];
            this.pidRoot = pidRoot;
            this.fdTask = (bool)r["f_fdTask"];
            this.fdID = (int)r["f_fdID"];
            if (this.fdTask) this.pathLoc = (string)r["f_pathLoc"];
        }
    }
}