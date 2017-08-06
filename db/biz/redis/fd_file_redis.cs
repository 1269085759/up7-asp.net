using System;
using up7.db.model;

namespace up7.db.biz.redis
{
    public class fd_file_redis : xdb_files
    {
        public void read(CSRedis.RedisClient j, String idSign)
        {
            this.id = idSign;
            if (!j.Exists(idSign)) return;

            this.lenLoc = long.Parse(j.HGet(idSign, "lenLoc"));
            this.lenSvr = long.Parse(j.HGet(idSign, "lenSvr"));
            this.sizeLoc = j.HGet(idSign, "sizeLoc");
            this.pathLoc = j.HGet(idSign, "pathLoc");
            this.pathSvr = j.HGet(idSign, "pathSvr");
            this.pathRel = j.HGet(idSign, "pathRel");
            this.blockPath = j.HGet(idSign, "blockPath");
            this.blockSize = int.Parse( j.HGet(idSign, "blockSize") );
            this.perSvr = j.HGet(idSign, "perSvr");
            this.nameLoc = j.HGet(idSign, "nameLoc");
            this.nameSvr = j.HGet(idSign, "nameSvr");
            this.pid = j.HGet(idSign, "pidSign");
            this.pidRoot = j.HGet(idSign, "rootSign");
            this.folder = j.HGet(idSign, "fdTask") == "True";
            this.complete = j.HGet(idSign, "complete") == "true";
        }

        public void write(CSRedis.RedisClient j)
        {
            j.Del(this.id);

            j.HSet(this.id, "lenLoc", this.lenLoc);//数字化的长度
            j.HSet(this.id, "lenSvr", this.lenSvr);//数字化的长度
            j.HSet(this.id, "sizeLoc", this.sizeLoc);//格式化的
            j.HSet(this.id, "pathLoc", this.pathLoc);//
            j.HSet(this.id, "pathSvr", this.pathSvr);//
            j.HSet(this.id, "pathRel", this.pathRel);//
            j.HSet(this.id, "blockPath", this.blockPath);//
            j.HSet(this.id, "blockSize", this.blockSize);
            j.HSet(this.id, "perSvr", this.lenLoc > 0 ? this.perSvr : "100%");//
            j.HSet(this.id, "nameLoc", this.nameLoc);//
            j.HSet(this.id, "nameSvr", this.nameSvr);//
            j.HSet(this.id, "pidSign", this.pid);//
            j.HSet(this.id, "rootSign", this.pidRoot);//		
            j.HSet(this.id, "fdTask", this.folder);//
            j.HSet(this.id, "complete", this.lenLoc > 0 ? "false" : "true");//
        }
    }
}