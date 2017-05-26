using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace up7.demoSql2005.db.biz.redis
{
    public class fd_file_redis : xdb_files
    {
        public void read(CSRedis.RedisClient j, String idSign)
        {
            this.idSign = idSign;
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
            this.pidSign = j.HGet(idSign, "pidSign");
            this.rootSign = j.HGet(idSign, "rootSign");
            this.folder = j.HGet(idSign, "fdTask") == "True";
            this.complete = j.HGet(idSign, "complete") == "true";
            this.sign = j.HGet(idSign, "sign");
        }

        public void write(CSRedis.RedisClient j)
        {
            j.Del(this.idSign);

            j.HSet(this.idSign, "lenLoc", this.lenLoc);//数字化的长度
            j.HSet(this.idSign, "lenSvr", this.lenSvr);//数字化的长度
            j.HSet(this.idSign, "sizeLoc", this.sizeLoc);//格式化的
            j.HSet(this.idSign, "pathLoc", this.pathLoc);//
            j.HSet(this.idSign, "pathSvr", this.pathSvr);//
            j.HSet(this.idSign, "pathRel", this.pathRel);//
            j.HSet(this.idSign, "blockPath", this.blockPath);//
            j.HSet(this.idSign, "blockSize", this.blockSize);
            j.HSet(this.idSign, "perSvr", this.lenLoc > 0 ? this.perSvr : "100%");//
            j.HSet(this.idSign, "nameLoc", this.nameLoc);//
            j.HSet(this.idSign, "nameSvr", this.nameSvr);//
            j.HSet(this.idSign, "pidSign", this.pidSign);//
            j.HSet(this.idSign, "rootSign", this.rootSign);//		
            j.HSet(this.idSign, "fdTask", this.folder);//
            j.HSet(this.idSign, "complete", this.lenLoc > 0 ? "false" : "true");//
            j.HSet(this.idSign, "sign", this.sign);//
        }
    }
}