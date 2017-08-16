using System;
using System.IO;
using System.Web;
using up7.db.biz;
using up7.db.biz.redis;
using up7.db.model;
using up7.db.utils;

namespace up7.db
{
    public partial class f_post : System.Web.UI.Page
    {
        string uid      = string.Empty;
        string id       = string.Empty;
        string pid       = string.Empty;
        string perSvr   = string.Empty;
        string lenSvr   = string.Empty;
        string lenLoc   = string.Empty;
        string nameLoc  = string.Empty;
        string pathLoc  = string.Empty;
        string sizeLoc  = string.Empty;
        string blockOffset= string.Empty;
        string blockIndex = string.Empty;
        string blockCount = string.Empty;
        string blockSize = string.Empty;
        string blockSizeLogic = string.Empty;
        string pathSvr = string.Empty;
        string pathRel = string.Empty;
        string pidRoot = string.Empty;

        void recvParam()
        {
            this.uid        = Request.Headers["uid"];
            this.id         = Request.Headers["id"];//
            this.pid        = Request.Headers["pid"];//
            this.perSvr     = Request.Headers["perSvr"];//文件百分比
            this.lenSvr     = Request.Headers["lenSvr"];//已传大小
            this.lenLoc     = Request.Headers["lenLoc"];//本地文件大小
            this.nameLoc    = Request.Headers["nameLoc"];//
            this.sizeLoc    = Request.Headers["sizeLoc"];//
            this.blockOffset= Request.Headers["blockOffset"];
            this.blockIndex = Request.Headers["blockIndex"];//块偏移，相对于文件
            this.blockCount = Request.Headers["blockCount"];//块总数
            this.blockSize  = Request.Headers["blockSize"];//块大小
            this.blockSizeLogic = Request.Headers["blockSizeLogic"];//逻辑块大小（定义的块大小）
            this.pathLoc    = Request.Headers["pathLoc"];//
            this.pathSvr    = Request.Headers["pathSvr"];
            this.pathRel    = Request.Headers["pathRel"];
            this.pidRoot    = Request.Headers["pidRoot"];//文件夹标识(guid)
            this.pathLoc    = PathTool.url_decode(this.pathLoc);
            this.nameLoc    = PathTool.url_decode(this.nameLoc);
            this.pathSvr    = PathTool.url_decode(this.pathSvr);
            this.pathRel    = PathTool.url_decode(this.pathRel);
        }

        void savePart()
        {
            BlockPathBuilder bpb = new BlockPathBuilder();
            string partPath = bpb.part(this.id, this.blockIndex, pathSvr);

            //自动创建目录
            if (!Directory.Exists(partPath)) Directory.CreateDirectory(Path.GetDirectoryName(partPath));

            HttpPostedFile part = Request.Files.Get(0);
            part.SaveAs(partPath);
        }
        void savePartFolder()
        {
            HttpPostedFile part = Request.Files.Get(0);

            FileInf fileSvr    = new FileInf();
            fileSvr.id         = id;
            fileSvr.nameLoc    = Path.GetFileName(pathLoc);
            fileSvr.nameSvr    = nameLoc;
            fileSvr.lenLoc     = long.Parse(lenLoc);
            fileSvr.sizeLoc    = sizeLoc;
            fileSvr.pathLoc    = this.pathLoc;
            fileSvr.pathSvr    = this.pathSvr;
            fileSvr.pathRel    = this.pathRel;
            fileSvr.pid        = this.pid;
            fileSvr.pidRoot    = pidRoot;
            fileSvr.blockCount = int.Parse(blockCount);
            fileSvr.blockSize  = int.Parse(blockSize);
            //块路径：d:/webapps/files/年/月/日/folder/folder-child/file-guid/
            fileSvr.blockPath  = Path.GetDirectoryName(fileSvr.pathSvr);
            fileSvr.blockPath  = Path.Combine(fileSvr.blockPath, id);
            if (!Directory.Exists(fileSvr.blockPath)) Directory.CreateDirectory(fileSvr.blockPath);
            
            //将文件列表添加到缓存
            if(blockOffset=="0")
            {
                var con = RedisConfig.getCon();
                //保存文件信息
                RedisFile f_svr = new RedisFile(ref con);
                f_svr.create(fileSvr);
                //保存到文件夹
                con.LPush(pidRoot, id);
            }

            //块路径
            string partPath = Path.Combine(fileSvr.blockPath,blockIndex+".part");

            part.SaveAs(partPath);
        }

        bool checkParam()
        {
            if (string.IsNullOrEmpty(lenLoc)
                || string.IsNullOrEmpty(uid)
                || string.IsNullOrEmpty(id)
                || string.IsNullOrEmpty(blockOffset)
                || string.IsNullOrEmpty(nameLoc))
            {
                XDebug.Output("lenLoc", lenLoc);
                XDebug.Output("uid", uid);
                XDebug.Output("idSvr", id);
                XDebug.Output("nameLoc", nameLoc);
                XDebug.Output("pathLoc", pathLoc);
                XDebug.Output("fd-idSvr", pidRoot);
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
                if (string.IsNullOrEmpty(pidRoot))
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