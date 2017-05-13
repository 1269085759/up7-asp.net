using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using up7.demoSql2005.db.biz.folder;
using up7.demoSql2005.db.redis;

namespace up7.demoSql2005.db.biz.redis
{
    public class fd_redis
    {

        //文件夹json数据
        public String data;
        fd_root m_root = null;
        CSRedis.RedisClient con = null;
        Dictionary<String/*guid*/, String/*pathSvr*/> parentPathMap = new Dictionary<String, String>();

        public fd_redis() { }
        public fd_redis(ref CSRedis.RedisClient j) { this.con = j; }


        void parse()
        {
            this.m_root = JsonConvert.DeserializeObject<fd_root>(this.data);
        }

        void init()
        {
            //更新文件夹路径（Server）
            foreach (fd_child fd in this.m_root.folders)

            {
                //取父级文件夹路径
                if (parentPathMap.ContainsKey(fd.rootSign))
                {
                    String path;
                    parentPathMap.TryGetValue(fd.rootSign, out path);//
                    fd.pathSvr = System.IO.Path.Combine(path, fd.nameLoc);
                }

                //添加当前路径
                parentPathMap.Add(fd.idSign, fd.pathSvr);
            }

            //更新文件路径（Server）
            foreach (fd_file f in this.m_root.files)

            {
                if (parentPathMap.ContainsKey(f.pidSign))
                {
                    String path;
                    parentPathMap.TryGetValue(f.pidSign, out path);//
                    f.pathSvr = System.IO.Path.Combine(path, f.nameLoc);
                    //创建空文件（原始大小）
                    //FileBlockWriter fr = new FileBlockWriter();
                    //fr.make(f.pathSvr, f.lenLoc);
                }
            }
        }

        //创建目录
        void makePath()
        {
            PathGuidBuilder pb = new PathGuidBuilder();
            this.m_root.pathRel = this.m_root.nameLoc;//

            this.m_root.pathSvr = pb.genFolder(this.m_root.uid, this.m_root.nameLoc);
            System.IO.Directory.CreateDirectory(this.m_root.pathSvr);
            parentPathMap.Add(this.m_root.idSign, this.m_root.pathSvr);
        }

        //保存到redis中
        public void save()
        {
            this.parse();//解析
            this.makePath();//创建根级目录

            this.init();//初始化

            var j = RedisConfig.getCon();
            this.saveRoot();
            this.saveFiles();
            this.saveFolders();

            //保存到队列
        }

        //从redis中读取数据
        public void read(String idSign)
        {
            var j = this.con;
            //folder不存在
            if (!j.Exists(idSign))
            {
                return;
            }

            this.m_root = new fd_root();
            this.m_root.idSign = idSign;
            this.m_root.nameLoc = j.HGet(idSign, "nameLoc");
            this.m_root.nameSvr = this.m_root.nameLoc;
            this.m_root.lenLoc = long.Parse(j.HGet(idSign, "lenLoc"));
            this.m_root.lenSvr = long.Parse(j.HGet(idSign, "lenSvr"));
            this.m_root.sizeLoc = j.HGet(idSign, "sizeLoc");
            this.m_root.pathLoc = j.HGet(idSign, "pathLoc");
            this.m_root.pathSvr = j.HGet(idSign, "pathSvr");
            this.m_root.filesCount = int.Parse(j.HGet(idSign, "filesCount"));
            //
            this.loadFiles();//加载文件列表		
            this.loadFolders();//加载目录列表
        }

        public void mergeAll()
        {
            foreach (fd_file_redis f in this.m_root.files)
            {
                f.merge();
            }
        }

        public void del(String idSign)
        {
            var j = this.con;
            //清除文件列表
            fd_files_redis fs = new fd_files_redis(ref this.con, idSign);
            fs.del();

            //清除目录列表
            fd_folders_redis ds = new fd_folders_redis(ref this.con, idSign);
            ds.del();

            //清除文件夹
            j.Del(idSign);
        }

        //保存到数据库
        public void saveToDb()
        {
            using (var con = DbHelper.CreateConnection())
            {
                con.Open();
                FolderDbWriter fd = new FolderDbWriter(con, this.m_root);
                fd.save();

                FileDbWriter fw = new FileDbWriter(con, this.m_root);
                fw.save();
                con.Close();
            }                
        }

        void loadFiles()
        {
            //取文件ID列表
            fd_files_redis rfs = new fd_files_redis(ref this.con, this.m_root.idSign);
            var fs = rfs.all();
            this.m_root.files = new List<fd_file>();
            
            foreach(String s in fs)
            {
                fd_file_redis file = new fd_file_redis();
                file.read(this.con,s);
                this.m_root.files.Add(file);
            }
        }

        void loadFolders()
        {
            //取文件ID列表
            fd_folders_redis rfs = new fd_folders_redis(ref this.con, this.m_root.idSign);
            this.m_root.folders = new List<fd_child>();
            var fs = rfs.all();
            foreach (String s in fs)
            {
                fd_child_redis fd = new fd_child_redis();
                fd.read(this.con, s);
                this.m_root.folders.Add(fd);
            }
        }

        void saveRoot()
        {
            var j = this.con;
            j.Del(this.m_root.idSign);

            j.HSet(this.m_root.idSign, "lenLoc", this.m_root.lenLoc);//数字化的长度
            j.HSet(this.m_root.idSign, "lenSvr", "0");//格式化的
            j.HSet(this.m_root.idSign, "sizeLoc", this.m_root.sizeLoc);//格式化的
            j.HSet(this.m_root.idSign, "pathLoc", this.m_root.pathLoc);//
            j.HSet(this.m_root.idSign, "pathSvr", this.m_root.pathSvr);//
            j.HSet(this.m_root.idSign, "filesCount", this.m_root.filesCount);

            fd_files_redis rfs = new fd_files_redis(ref j, this.m_root.idSign);
            rfs.add(m_root.files);

            //保存目录
            fd_folders_redis rds = new fd_folders_redis(ref j, this.m_root.idSign);
            rds.add(this.m_root.folders);
        }

        void saveFiles()
        {
            foreach (fd_file_redis f in this.m_root.files)
            {
                f.write(this.con);
            }
        }

        void saveFolders()
        {
            foreach (fd_child_redis fd in this.m_root.folders)
            {
                fd.write(this.con);
            }
        }

        public String toJson()
        {
            String json = JsonConvert.SerializeObject(this.m_root);
            json = HttpUtility.UrlEncode(json);
            json = json.Replace("+", "%20");
            return json;
        }
    }
}