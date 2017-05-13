using System;
using up7.demoSql2005.db.biz.folder;

namespace up7.demoSql2005.db.biz.redis
{
    public class fd_child_redis : fd_child
    {
        public void read(CSRedis.RedisClient j, String idSign)
        {
            this.idSign = idSign;
            if (!j.Exists(idSign)) return;

            this.pathLoc = j.HGet(idSign, "pathLoc");
            this.pathSvr = j.HGet(idSign, "pathSvr");
            this.nameLoc = j.HGet(idSign, "nameLoc");
            this.nameSvr = j.HGet(idSign, "nameSvr");
            this.pidSign = j.HGet(idSign, "pidSign");
            this.rootSign = j.HGet(idSign, "rootSign");
        }

        public void write(CSRedis.RedisClient j)
        {

            j.Del(this.idSign);


            j.HSet(this.idSign, "pathLoc", this.pathLoc);//
            j.HSet(this.idSign, "pathSvr", this.pathSvr);//
            j.HSet(this.idSign, "nameLoc", this.nameLoc);//
            j.HSet(this.idSign, "nameSvr", this.nameSvr);//
            j.HSet(this.idSign, "pidSign", this.pidSign);//
            j.HSet(this.idSign, "rootSign", this.rootSign);//
        }
    }
}