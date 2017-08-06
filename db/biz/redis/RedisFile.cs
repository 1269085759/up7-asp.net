using System;
using up7.db.model;

namespace up7.db.biz.redis
{
    public class RedisFile
    {
        CSRedis.RedisClient con = null;
        public RedisFile(ref CSRedis.RedisClient c) { this.con = c; }

        public void process(String idSign, String perSvr, String lenSvr,String blockCount)
        {
            var j = this.con;
            j.HSet(idSign, "perSvr", perSvr);
            j.HSet(idSign, "lenSvr", lenSvr);
            j.HSet(idSign, "blockCount", blockCount);
        }

        public void complete(string id) { this.con.Del(id); }
        public void create(FileInf f)
        {
            if (this.con.Exists(f.id)) return;

            this.con.HSet(f.id, "fdTask", f.folder);
            this.con.HSet(f.id, "pid", f.pid);
            this.con.HSet(f.id, "pidRoot", f.pidRoot);
            this.con.HSet(f.id, "pathLoc", f.pathLoc);
            this.con.HSet(f.id, "pathSvr", f.pathSvr);
            this.con.HSet(f.id, "pathRel", f.pathRel);
            this.con.HSet(f.id, "blockPath", f.blockPath);
            this.con.HSet(f.id, "nameLoc", f.nameLoc);
            this.con.HSet(f.id, "nameSvr", f.nameSvr);
            this.con.HSet(f.id, "lenLoc", f.lenLoc);
            this.con.HSet(f.id, "lenSvr", "0");
            this.con.HSet(f.id, "blockCount", f.blockCount);
            this.con.HSet(f.id, "blockSize", f.blockSize);
            this.con.HSet(f.id, "sizeLoc", f.sizeLoc);
            this.con.HSet(f.id, "filesCount", f.fileCount);
            this.con.HSet(f.id, "foldersCount", "0");
        }

        public FileInf read(string id)
        {
            if (!this.con.Exists(id)) return null;

            FileInf f = new FileInf();
            f.id = id;
            f.folder  = this.con.HGet(id, "fdTask").Equals("true",StringComparison.CurrentCultureIgnoreCase);
            f.pid= this.con.HGet(id, "pid");
            f.pidRoot = this.con.HGet(id, "pidRoot");
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