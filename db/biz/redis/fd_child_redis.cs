using System;
using up7.db.model;

namespace up7.db.biz.redis
{
    public class fd_child_redis : FileInf
    {
        public void read(CSRedis.RedisClient j, String idSign)
        {
            this.id = idSign;
            if (!j.Exists(idSign)) return;

            this.pathLoc = j.HGet(idSign, "pathLoc");
            this.pathSvr = j.HGet(idSign, "pathSvr");
            this.nameLoc = j.HGet(idSign, "nameLoc");
            this.nameSvr = j.HGet(idSign, "nameSvr");
            this.pid = j.HGet(idSign, "pidSign");
            this.pidRoot = j.HGet(idSign, "rootSign");
        }

        public void write(CSRedis.RedisClient j)
        {

            j.Del(this.id);


            j.HSet(this.id, "pathLoc", this.pathLoc);//
            j.HSet(this.id, "pathSvr", this.pathSvr);//
            j.HSet(this.id, "nameLoc", this.nameLoc);//
            j.HSet(this.id, "nameSvr", this.nameSvr);//
            j.HSet(this.id, "pidSign", this.pid);//
            j.HSet(this.id, "rootSign", this.pidRoot);//
        }
    }
}