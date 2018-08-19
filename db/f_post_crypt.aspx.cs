using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using up7.db.biz;
using up7.db.biz.redis;
using up7.db.model;
using up7.db.utils;

namespace up7.db
{
    public partial class f_post_crypt : System.Web.UI.Page
    {
        string uid            = string.Empty;
        string id             = string.Empty;
        string pid            = string.Empty;
        string perSvr         = string.Empty;
        string lenSvr         = string.Empty;
        string lenLoc         = string.Empty;
        string nameLoc        = string.Empty;
        string pathLoc        = string.Empty;
        string sizeLoc        = string.Empty;
        string blockOffset    = string.Empty;
        string blockIndex     = string.Empty;
        string blockCount     = string.Empty;
        string blockSize      = string.Empty;
        string blockSizeLogic = string.Empty;
        string blockMd5       = string.Empty;
        string pathSvr        = string.Empty;
        string pathRel        = string.Empty;
        string pidRoot        = string.Empty;


        void recvParam()
        {
            CryptoTool ct = new CryptoTool();
            var blockData = ct.cbc_decode( Request.Form["blockData"]);
            var kv = JsonConvert.DeserializeObject<Dictionary<string, string>>(blockData);

            this.id             = kv["id"];
            this.uid            = kv["uid"];
            this.pid            = kv["pid"];//
            this.perSvr         = kv["perSvr"];//文件百分比
            this.lenSvr         = kv["lenSvr"];//已传大小
            this.lenLoc         = kv["lenLoc"];//本地文件大小
            this.nameLoc        = kv["nameLoc"];//
            this.sizeLoc        = kv["sizeLoc"];//
            this.blockOffset    = kv["blockOffset"];
            this.blockIndex     = kv["blockIndex"];//块偏移，相对于文件
            this.blockCount     = kv["blockCount"];//块总数
            this.blockSize      = kv["blockSize"];//块大小
            this.blockSizeLogic = kv["blockSizeLogic"];//逻辑块大小（定义的块大小）
            this.blockMd5       = kv["blockMd5"];//块MD5
            this.pathLoc        = kv["pathLoc"];//
            this.pathSvr        = kv["pathSvr"];
            this.pathRel        = kv["pathRel"];
            this.pidRoot        = kv["pidRoot"];//文件夹标识(guid)
            this.pathLoc        = PathTool.url_decode(this.pathLoc);
            this.nameLoc        = PathTool.url_decode(this.nameLoc);
            this.pathSvr        = PathTool.url_decode(this.pathSvr);
            this.pathRel        = PathTool.url_decode(this.pathRel);
        }

        void savePart()
        {
            BlockPathBuilder bpb = new BlockPathBuilder();
            string partPath = bpb.part(this.blockIndex, pathSvr);

            //自动创建目录
            if (!Directory.Exists(partPath)) Directory.CreateDirectory(Path.GetDirectoryName(partPath));

            HttpPostedFile part = Request.Files.Get(0);
            //验证大小
            if (part.InputStream.Length != long.Parse(this.blockSize)) return;
            part.SaveAs(partPath);

            //计算块md5
            string md5Svr = string.Empty;
            if ( !string.IsNullOrEmpty(this.blockMd5)) md5Svr = this.mkMD5(part.InputStream);

            //返回信息
            JObject o = new JObject();
            o["msg"] = "ok";
            o["md5"] = md5Svr;
            o["offset"] = long.Parse(this.blockOffset);
            string json = JsonConvert.SerializeObject(o);//取消格式化
            Response.Write(json);
        }
        void savePartFolder()
        {
            FileInf fileSvr      = new FileInf();
            fileSvr.id           = id;
            fileSvr.nameLoc      = Path.GetFileName(pathLoc);
            fileSvr.nameSvr      = nameLoc;
            fileSvr.lenLoc       = long.Parse(lenLoc);
            fileSvr.sizeLoc      = sizeLoc;
            fileSvr.pathLoc      = this.pathLoc;
            fileSvr.pathSvr      = this.pathSvr;
            fileSvr.pathRel      = this.pathRel;
            fileSvr.pid          = this.pid;
            fileSvr.pidRoot      = pidRoot;
            fileSvr.blockCount   = int.Parse(blockCount);
            fileSvr.blockSize    = int.Parse(blockSize);
            //块路径：d:/webapps/files/年/月/日/folder-id/folder-name//file-id/
            BlockPathBuilder bpb = new BlockPathBuilder();
            fileSvr.blockPath    = bpb.rootFD(id, fileSvr.pathSvr);
            if (!Directory.Exists(fileSvr.blockPath)) Directory.CreateDirectory(fileSvr.blockPath);
            //块路径
            string partPath = Path.Combine(fileSvr.blockPath, blockIndex + ".part");

            //将文件列表添加到缓存
            if (blockOffset == "0")
            {
                var con = RedisConfig.getCon();
                //保存文件信息
                RedisFile f_svr = new RedisFile(ref con);
                f_svr.create(fileSvr);
                //保存到文件夹
                con.LPush(pidRoot, id);
            }

            HttpPostedFile part = Request.Files.Get(0);
            //验证大小
            if (part.InputStream.Length != long.Parse(this.blockSize)) return;
            part.SaveAs(partPath);

            //计算块md5
            string md5Svr = string.Empty;
            if ( !string.IsNullOrEmpty(this.blockMd5) ) md5Svr = this.mkMD5(part.InputStream);

            //返回信息
            JObject o = new JObject();
            o["msg"] = "ok";
            o["md5"] = md5Svr;
            o["offset"] = long.Parse(this.blockOffset);
            string json = JsonConvert.SerializeObject(o);//取消格式化
            Response.Write(json);
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

        string mkMD5(Stream s)
        {
            HttpPostedFile part = Request.Files.Get(0);
            byte[] data = new byte[s.Length];
            s.Read(data, 0, (int)s.Length);
            MD5 md5 = MD5.Create();
            byte[] ret = md5.ComputeHash(data);
            StringBuilder strbul = new StringBuilder(40);
            for (int i = 0; i < ret.Length; i++)
            {
                strbul.Append(ret[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位
            }
            return strbul.ToString();
        }

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
            }
        }
    }
}