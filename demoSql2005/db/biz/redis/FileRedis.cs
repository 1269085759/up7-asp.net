using System;

namespace up7.demoSql2005.db.biz.redis
{
    public class FileRedis
    {
        CSRedis.RedisClient con = null;
        public FileRedis(ref CSRedis.RedisClient c) { this.con = c; }

        public void process(String idSign, String perSvr, String lenSvr,String blockCount,String blockSize)
        {
            var j = this.con;
            j.HSet(idSign, "perSvr", perSvr);
            j.HSet(idSign, "lenSvr", lenSvr);
            j.HSet(idSign, "blockCount", blockCount);
            j.HSet(idSign, "blockSize", blockSize);
        }

        public void complete(string id) { this.con.Del(id); }
        public void create(xdb_files f)
        {
            if (this.con.Exists(f.idSign)) return;

            this.con.HSet(f.idSign, "fdTask", f.folder);
            this.con.HSet(f.idSign, "rootSign", f.rootSign);
            this.con.HSet(f.idSign, "pathLoc", f.pathLoc);
            this.con.HSet(f.idSign, "pathSvr", f.pathSvr);
            this.con.HSet(f.idSign, "pathRel", f.pathRel);
            this.con.HSet(f.idSign, "blockPath", f.blockPath);
            this.con.HSet(f.idSign, "nameLoc", f.nameLoc);
            this.con.HSet(f.idSign, "nameSvr", f.nameSvr);
            this.con.HSet(f.idSign, "lenLoc", f.lenLoc);
            this.con.HSet(f.idSign, "lenSvr", "0");
            this.con.HSet(f.idSign, "blockCount", f.blockCount);
            this.con.HSet(f.idSign, "blockSize", f.blockSize);
            this.con.HSet(f.idSign, "sizeLoc", f.sizeLoc);
            this.con.HSet(f.idSign, "filesCount", f.fileCount);
            this.con.HSet(f.idSign, "foldersCount", "0");
        }

        public xdb_files read(string id)
        {
            if (!this.con.Exists(id)) return null;

            xdb_files f = new xdb_files();
            f.idSign = id;
            f.folder  = this.con.HGet(id, "fdTask")=="True";
            f.pathLoc = this.con.HGet(id, "pathLoc");
            f.pathSvr = this.con.HGet(id, "pathSvr");
            f.pathRel = this.con.HGet(id, "pathRel");
            f.blockPath = this.con.HGet(id, "blockPath");
            f.nameLoc = this.con.HGet(id, "nameLoc");
            f.nameSvr = this.con.HGet(id, "nameSvr");
            f.lenLoc = long.Parse(this.con.HGet(id, "lenLoc"));
            f.lenSvr = long.Parse(this.con.HGet(id,"lenSvr"));
            f.perSvr = this.con.HGet(id, "perSvr");
            f.sizeLoc = this.con.HGet(id, "sizeLoc");
            f.blockCount = int.Parse(this.con.HGet(id, "blockCount"));
            f.blockSize  = int.Parse(this.con.HGet(id, "blockSize"));
            f.fileCount  = int.Parse(this.con.HGet(id, "filesCount"));
            return f;
        }
    }
}