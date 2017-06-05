using System;
using System.IO;
using System.Web;
using up7.demoSql2005.db;
using up7.demoSql2005.db.biz;
using up7.demoSql2005.db.biz.redis;
using up7.demoSql2005.db.redis;

namespace up7.demoSql2005.db
{
    public partial class f_post : System.Web.UI.Page
    {
        string uid      = string.Empty;
        string idSign   = string.Empty;
        string perSvr   = string.Empty;
        string lenSvr   = string.Empty;
        string lenLoc   = string.Empty;
        string nameLoc  = string.Empty;
        string pathLoc  = string.Empty;
        string sizeLoc  = string.Empty;
        string f_pos    = string.Empty;
        string blockIndex = string.Empty;
        string blockCount = string.Empty;
        string blockSize = string.Empty;
        string fd_idSign = string.Empty;
        string fd_lenSvr = string.Empty;
        string fd_perSvr = string.Empty;

        void recvParam()
        {
            this.uid        = Request.Headers["f-uid"];
            this.idSign     = Request.Headers["f-idSign"];//
            this.perSvr     = Request.Headers["f-perSvr"];//文件百分比
            this.lenSvr     = Request.Headers["f-lenSvr"];//已传大小
            this.lenLoc     = Request.Headers["f-lenLoc"];//本地文件大小
            this.nameLoc    = Request.Headers["f-nameLoc"];//
            this.pathLoc    = Request.Headers["f-pathLoc"];//
            this.sizeLoc    = Request.Headers["f-sizeLoc"];//
            this.f_pos      = Request.Headers["f-RangePos"];
            this.blockIndex = Request.Headers["f-rangeIndex"];
            this.blockCount = Request.Headers["f-rangeCount"];
            this.blockSize  = Request.Headers["f-rangeSize"];
            //string complete     = Request.Headers["complete"];//true/false
            this.fd_idSign  = Request.Headers["fd-idSign"];//文件夹标识(guid)
            this.fd_lenSvr  = Request.Headers["fd-lenSvr"];//文件夹已传大小
            this.fd_perSvr  = Request.Headers["fd-perSvr"];//文件夹百分比
            this.pathLoc    = pathLoc.Replace("+", "%20");
            this.pathLoc    = HttpUtility.UrlDecode(pathLoc);
            this.nameLoc    = pathLoc.Replace("+", "%20");
            this.nameLoc    = HttpUtility.UrlDecode(nameLoc);
        }

        void savePart()
        {
            var con = RedisConfig.getCon();
            FileRedis f_svr = new FileRedis(ref con);
            var fileSvr = f_svr.read(this.idSign);

            BlockPathBuilder bpb = new BlockPathBuilder();
            string partPath = bpb.part(this.idSign, this.blockIndex, fileSvr.pathSvr);

            //自动创建目录
            if (!Directory.Exists(partPath)) Directory.CreateDirectory(Path.GetDirectoryName(partPath));

            HttpPostedFile part = Request.Files.Get(0);
            part.SaveAs(partPath);

            //更新缓存进度
            f_svr.process(idSign, perSvr, lenSvr, blockCount);
        }
        void savePartFolder()
        {
            HttpPostedFile part = Request.Files.Get(0);
            var con = RedisConfig.getCon();

            FileRedis fr = new FileRedis(ref con);
            var fd = fr.read(this.fd_idSign);

            xdb_files fileSvr = new xdb_files();
            fileSvr.idSign = idSign;
            fileSvr.nameLoc = Path.GetFileName(pathLoc);
            fileSvr.nameSvr = nameLoc;
            fileSvr.lenLoc = long.Parse(lenLoc);
            fileSvr.sizeLoc = sizeLoc;
            fileSvr.pathLoc = pathLoc.Replace("\\", "/");//路径规范化处理
            fileSvr.pathSvr = pathLoc.Replace(fd.pathLoc, fd.pathSvr);
            fileSvr.pathSvr = fileSvr.pathSvr.Replace("\\", "/");
            fileSvr.pathRel = pathLoc.Replace(fd.pathLoc+"\\", string.Empty);
            fileSvr.rootSign = fd_idSign;
            fileSvr.blockCount = int.Parse(blockCount);
            fileSvr.blockSize = int.Parse(blockSize);
            BlockPathBuilder bpb = new BlockPathBuilder();
            fileSvr.blockPath = bpb.rootFd(ref fileSvr, this.blockIndex, ref fd);
            if (!Directory.Exists(fileSvr.blockPath)) Directory.CreateDirectory(fileSvr.blockPath);
            
            FileRedis f_svr = new FileRedis(ref con);
            if(!con.Exists(idSign))
            {
                //添加到文件夹
                fd_files_redis root = new fd_files_redis(ref con, fd_idSign);
                root.add(idSign);

                f_svr.create(fileSvr);//添加到缓存
            }//更新文件夹进度
            else if (f_pos == "0")
            {                
                 f_svr.process(fd_idSign, fd_perSvr, fd_lenSvr, "0");
            }

            //块路径
            string partPath = Path.Combine(fileSvr.blockPath,blockIndex+".part");

            //自动创建目录
            if (!Directory.Exists(partPath)) Directory.CreateDirectory(Path.GetDirectoryName(partPath));

            part.SaveAs(partPath);
        }

        bool checkParam()
        {
            if (string.IsNullOrEmpty(lenLoc)
                || string.IsNullOrEmpty(uid)
                || string.IsNullOrEmpty(idSign)
                || string.IsNullOrEmpty(f_pos)
                || string.IsNullOrEmpty(nameLoc))
            {
                XDebug.Output("lenLoc", lenLoc);
                XDebug.Output("uid", uid);
                XDebug.Output("idSvr", idSign);
                XDebug.Output("nameLoc", nameLoc);
                XDebug.Output("pathLoc", pathLoc);
                XDebug.Output("fd-idSvr", fd_idSign);
                XDebug.Output("fd-lenSvr", fd_lenSvr);
                Response.Write("param is null");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 只负责拼接文件块。将接收的文件块数据写入到文件中。
        /// 更新记录：
        ///		2012-04-12 更新文件大小变量类型，增加对2G以上文件的支持。
        ///		2012-04-18 取消更新文件上传进度信息逻辑。
        ///		2012-10-30 增加更新文件进度功能。
        ///		2015-03-19 文件路径由客户端提供，此页面不再查询文件在服务端的路径。减少一次数据库访问操作。
        ///     2016-03-31 增加文件夹信息字段
        ///     2017-05-13 完善文件块逻辑，完善子文件块逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.recvParam();

            //参数为空
            if (!this.checkParam()) return;

            //有文件块数据
            if (Request.Files.Count > 0)
            {
                //文件块
                if (string.IsNullOrEmpty(fd_idSign))
                {
                    this.savePart();
                }//子文件块
                else
                {
                    this.savePartFolder();
                }

                Response.Write("ok");
            }
        }
    }
}