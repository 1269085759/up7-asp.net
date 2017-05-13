using System;

namespace up7.demoSql2005.db.biz.redis
{
    public class FileRedis
    {
        CSRedis.RedisClient con = null;
        public FileRedis(ref CSRedis.RedisClient c) { this.con = c; }

        public void process(String idSign, String perSvr, String lenSvr,String blockCount)
        {
            var j = this.con;
            j.HSet(idSign, "perSvr", perSvr);
            j.HSet(idSign, "lenSvr", lenSvr);
            j.HSet(idSign, "blockCount", blockCount);
        }

        public void complete(string id) { this.con.Del(id); }
        public void create(xdb_files f)
        {
            if (this.con.Exists(f.idSign)) return;

            this.con.HSet(f.idSign, "fdTask", f.f_fdTask);
            this.con.HSet(f.idSign, "rootSign", f.rootSign);
            this.con.HSet(f.idSign, "pathLoc", f.pathLoc);
            this.con.HSet(f.idSign, "pathSvr", f.pathSvr);
            this.con.HSet(f.idSign, "blockPath", f.blockPath);
            this.con.HSet(f.idSign, "nameLoc", f.nameLoc);
            this.con.HSet(f.idSign, "nameSvr", f.nameSvr);
            this.con.HSet(f.idSign, "lenLoc", f.lenLoc);
            this.con.HSet(f.idSign, "lenSvr", "0");
            this.con.HSet(f.idSign, "blockCount", f.blockCount);
            this.con.HSet(f.idSign, "sizeLoc", f.sizeLoc);
            this.con.HSet(f.idSign, "filesCount", f.filesCount);
            this.con.HSet(f.idSign, "foldersCount", "0");
        }

        public xdb_files read(string id)
        {
            if (!this.con.Exists(id)) return null;

            xdb_files f = new xdb_files();
            f.idSign = id;
            f.pathLoc = this.con.HGet(id, "pathLoc");
            f.pathSvr = this.con.HGet(id, "pathSvr");
            f.blockPath = this.con.HGet(id, "blockPath");
            f.nameLoc = this.con.HGet(id, "nameLoc");
            f.nameSvr = this.con.HGet(id, "nameSvr");
            f.lenLoc = long.Parse(this.con.HGet(id, "lenLoc"));
            f.sizeLoc = this.con.HGet(id, "sizeLoc");
            f.blockCount = int.Parse(this.con.HGet(id, "blockCount"));
            f.filesCount = int.Parse(this.con.HGet(id, "filesCount"));
            return f;
        }
        public String getPartPath(String idSign, String blockIndex,String blockCount)
        {
            var j = this.con;
            if (!j.Exists(idSign)) return "";

            String pathSvr = j.HGet(idSign, "pathSvr");//f:/files/guid/QQ2013.exe
            System.IO.FileInfo f = new System.IO.FileInfo(pathSvr);
            if (blockCount == "1") return pathSvr;//只有一块时不分块

            pathSvr = f.DirectoryName;//d:\\soft
            pathSvr = pathSvr + "/" + idSign + "/";
            pathSvr = pathSvr + blockIndex + ".part";
            pathSvr = pathSvr.Replace("\\", "/");
            return pathSvr;
        }
        public String makePathFile(String idSign, String fdSign)
        {
            var j = this.con;
            String pathSvrF = "";
            String fPath = j.HGet(idSign, "pathSvr");
            if (string.IsNullOrEmpty(fPath))
            {
                String pathLocFD = j.HGet(fdSign, "pathLoc");
                String pathSvrFD = j.HGet(fdSign, "pathSvr");
                String pathLocF = j.HGet(idSign, "pathLoc");


                //将文件的本地根路径替换为服务器路径
                pathLocFD = pathLocFD.Replace("\\", "/");
                pathSvrFD = pathSvrFD.Replace("\\", "/");
                pathSvrF = pathLocF.Replace(pathLocFD, pathSvrFD);
                j.HSet(idSign, "pathSvr", pathSvrF);
            }
            else
            {
                pathSvrF = j.HGet(idSign, "pathSvr");
            }
            return pathSvrF;
        }

        public String getPartPath(String idSign, String blockIndex, String blockCount, String fdSign)
        {
            String pathSvr = "";
            var j = this.con;
            Boolean hasData = j.Exists(idSign);
            if (hasData) hasData = j.Exists(fdSign);
            if (hasData)
            {
                pathSvr = this.makePathFile(idSign, fdSign);
                //需要分块			
                if ( blockCount!="1" )
                {
                    var index = pathSvr.LastIndexOf("/");
                    if (index != -1) pathSvr = pathSvr.Substring(0, index);
                    pathSvr = pathSvr + "/" + idSign + "/" + blockIndex + ".part";
                }
            }
            return pathSvr;
        }
    }
}