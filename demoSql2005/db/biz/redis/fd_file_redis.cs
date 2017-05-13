﻿using System;
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
            this.perSvr = j.HGet(idSign, "perSvr");
            this.nameLoc = j.HGet(idSign, "nameLoc");
            this.nameSvr = j.HGet(idSign, "nameSvr");
            this.pidSign = j.HGet(idSign, "pidSign");
            this.rootSign = j.HGet(idSign, "rootSign");
            this.fdTask = j.HGet(idSign, "fdTask") == "true";
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
            j.HSet(this.idSign, "perSvr", this.lenLoc > 0 ? this.perSvr : "100%");//
            j.HSet(this.idSign, "nameLoc", this.nameLoc);//
            j.HSet(this.idSign, "nameSvr", this.nameSvr);//
            j.HSet(this.idSign, "pidSign", this.pidSign);//
            j.HSet(this.idSign, "rootSign", this.rootSign);//		
            j.HSet(this.idSign, "fdTask", this.fdTask);//
            j.HSet(this.idSign, "complete", this.lenLoc > 0 ? "false" : "true");//
            j.HSet(this.idSign, "sign", this.sign);//
        }

        //合并所有块
        public void merge()
        {
            if (File.Exists(pathSvr)) return;//文件已存在

            var fd = Path.GetDirectoryName(pathSvr);
            if(!Directory.Exists(fd)) Directory.CreateDirectory(fd);

            //取文件块路径
            fd = fd + "/" + this.idSign + "/";//f:/files/folder/guid/
            String[] parts = Directory.GetFiles(fd);
            long prevLen = 0;

            using (var mapFile = MemoryMappedFile.CreateFromFile(pathSvr,FileMode.CreateNew,this.idSign,this.lenLoc))
            {

                for (int i = 0, l = parts.Length; i < l; ++i)
                {
                    String partName = fd + (i + 1) + ".part";
                    var partData = File.ReadAllBytes(partName);
                    //每一个文件块为64mb，最后一个文件块<=64mb
                    long partOffset = prevLen;
                    using (var ss = mapFile.CreateViewStream(partOffset, partData.Length))
                    {
                        ss.Write(partData, 0, partData.Length);
                    }
                    prevLen += partData.Length;
                }
            }

            //删除文件块
            foreach (var part in parts)
            {
                System.IO.File.Delete(part);
            }
            //删除文件块目录
            System.IO.Directory.Delete(fd);
        }
    }
}