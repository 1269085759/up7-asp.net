using System;
using up7.down3.model;

namespace up7.down3.biz.redis
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
            j.HSet(f.id, "nameLoc", f.nameLoc);
            j.HSet(f.id, "pathLoc", f.pathLoc);
            j.HSet(f.id, "pathSvr", f.pathSvr);
            j.HSet(f.id, "lenLoc", f.lenLoc);//已下载大小		
            j.HSet(f.id, "lenSvr", f.lenSvr);//文件大小
            j.HSet(f.id, "sizeSvr", f.sizeSvr);
            j.HSet(f.id, "perLoc", f.perLoc);//已下载百分比	
        }

        public DnFileInf read(string id)
        {
            if (!this.con.Exists(id)) return null;
            DnFileInf f = new DnFileInf();
            f.id = id;
            f.lenLoc = long.Parse(this.con.HGet(id, "lenLoc"));//已经下载的大小
            f.lenSvr = long.Parse(this.con.HGet(id, "lenSvr"));//服务器文件大小。
            f.perLoc = this.con.HGet(id, "perLoc");
            f.pathLoc = this.con.HGet(id, "pathLoc");//本地下载地址
            f.pathSvr = this.con.HGet(id, "pathSvr");//服务器文件地址
            f.sizeSvr = this.con.HGet(id, "sizeSvr");//
            f.nameLoc = this.con.HGet(id, "nameLoc");//
            return f;
        }

        public void process(string id,string perLoc,long lenLoc,string sizeLoc)
        {
            var j = this.con;

            j.HSet(id, "lenLoc", lenLoc);//已下载大小		
            j.HSet(id, "perLoc", perLoc);//已下载百分比
            j.HSet(id, "sizeLoc", sizeLoc);//已下载百分比
        }
    }
}