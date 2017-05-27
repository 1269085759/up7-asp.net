using System;
using up7.demoSql2005.down3.model;

namespace up7.demoSql2005.down3.biz
{
    public class FileRedis
    {
        CSRedis.RedisClient con = null;

        public FileRedis(ref CSRedis.RedisClient c)
        {
            this.con = c;
        }

        public void create(ref DnFileInf f)
        {
            var j = this.con;
            j.HSet(f.signSvr, "nameLoc", f.nameLoc);
            j.HSet(f.signSvr, "pathLoc", f.pathLoc);
            j.HSet(f.signSvr, "pathSvr", f.pathSvr);
            j.HSet(f.signSvr, "lenLoc", f.lenLoc);//已下载大小		
            j.HSet(f.signSvr, "lenSvr", f.lenSvr);//文件大小
            j.HSet(f.signSvr, "sizeSvr", f.sizeSvr);
            j.HSet(f.signSvr, "perLoc", f.perLoc);//已下载百分比	
            j.HSet(f.signSvr, "fdTask", f.folder);
        }

        public DnFileInf read(string signSvr)
        {
            if (!this.con.Exists(signSvr)) return null;
            DnFileInf f = new DnFileInf();
            f.signSvr = signSvr;
            f.lenLoc = long.Parse(this.con.HGet(signSvr, "lenLoc"));//已经下载的大小
            f.lenSvr = long.Parse(this.con.HGet(signSvr, "lenSvr"));//服务器文件大小。
            f.perLoc = this.con.HGet(signSvr, "perLoc");
            f.pathLoc = this.con.HGet(signSvr, "pathLoc");//本地下载地址
            f.pathSvr = this.con.HGet(signSvr, "pathSvr");//服务器文件地址
            f.sizeSvr = this.con.HGet(signSvr, "sizeSvr");//
            f.nameLoc = this.con.HGet(signSvr, "nameLoc");//
            f.folder = this.con.HGet(signSvr,"fdTask").Equals("true",StringComparison.CurrentCultureIgnoreCase);
            return f;
        }

        public void process(string signSvr,string perLoc,long lenLoc)
        {
            var j = this.con;

            j.HSet(signSvr, "lenLoc", lenLoc);//已下载大小		
            j.HSet(signSvr, "perLoc", perLoc);//已下载百分比
        }
    }
}