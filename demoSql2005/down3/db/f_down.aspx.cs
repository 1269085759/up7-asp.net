using System;
using System.IO;
using System.Web;
using up7.demoSql2005.db.redis;
using up7.demoSql2005.down3.biz;
using up7.demoSql2005.down3.model;

namespace up7.demoSql2005.down3.db
{
    public partial class f_down : System.Web.UI.Page
    {
        String lenSvr       = string.Empty;
        String nameLoc      = string.Empty;
        String sizeSvr      = string.Empty;
        String pathSvr      = string.Empty;
        String pathLoc      = string.Empty;
        String blockIndex   = string.Empty;
        String fileOffset   = string.Empty;
        String blockOffset  = string.Empty;
        String blockSize    = string.Empty;
        String rangeSize    = string.Empty;
        String lenLoc       = string.Empty;
        String signSvr      = string.Empty;
        String fd_signSvr   = string.Empty;
        String fd_lenLoc    = string.Empty;
        String fd_sizeLoc   = string.Empty;
        String fd_percent   = string.Empty;
        String uid          = string.Empty;
        String percent      = string.Empty;

        void recvParam()
        {
            this.lenSvr         = Request.Headers["f-lenSvr"];
            this.nameLoc        = Request.Headers["f-nameLoc"];
            this.sizeSvr        = Request.Headers["f-sizeSvr"];
            this.pathSvr        = Request.Headers["f-pathSvr"];
            this.pathLoc        = Request.Headers["f-pathLoc"];
            this.blockIndex     = Request.Headers["f-blockIndex"];//块索引基于1
            this.fileOffset     = Request.Headers["f-fileOffset"];//基于文件的位置
            this.blockOffset    = Request.Headers["f-blockOffset"];//基于块的位置
            this.blockSize      = Request.Headers["f-blockSize"];//逻辑块大小
            this.rangeSize      = Request.Headers["f-rangeSize"];//当前请求的块大小
            this.lenLoc         = Request.Headers["f-lenLoc"];
            this.signSvr        = Request.Headers["f-signSvr"];
            this.percent        = Request.Headers["f-percent"];
            this.uid            = Request.Headers["f-uid"];
            this.fd_signSvr     = Request.Headers["fd-signSvr"];
            this.fd_lenLoc      = Request.Headers["fd-lenLoc"];
            this.fd_sizeLoc     = Request.Headers["fd-sizeLoc"];
            this.fd_percent     = Request.Headers["fd-percent"];

            pathSvr = pathSvr.Replace("+", "%20");
            pathLoc = pathLoc.Replace("+", "%20");
            nameLoc = nameLoc.Replace("+", "%20");
            pathSvr = HttpUtility.UrlDecode(pathSvr);//utf-8解码
            pathLoc = HttpUtility.UrlDecode(pathLoc);//utf-8解码
            nameLoc = HttpUtility.UrlDecode(nameLoc);//utf-8解码
        }

        bool checkParam()
        {
            if (string.IsNullOrEmpty(lenSvr)
                || string.IsNullOrEmpty(pathSvr)
                || string.IsNullOrEmpty(pathLoc)
                || string.IsNullOrEmpty(blockIndex)
                || string.IsNullOrEmpty(lenLoc)
                || string.IsNullOrEmpty(percent)
                )
            {
                System.Diagnostics.Debug.WriteLine("lenSvr:" + lenSvr);
                System.Diagnostics.Debug.WriteLine("lenLoc:" + lenLoc);
                System.Diagnostics.Debug.WriteLine("nameLoc:" + nameLoc);
                System.Diagnostics.Debug.WriteLine("sizeSvr:" + sizeSvr);
                System.Diagnostics.Debug.WriteLine("pathSvr:" + pathSvr);
                System.Diagnostics.Debug.WriteLine("pathLoc:" + pathLoc);
                System.Diagnostics.Debug.WriteLine("blockIndex:" + blockIndex);
                System.Diagnostics.Debug.WriteLine("blockSize:" + blockSize);
                System.Diagnostics.Debug.WriteLine("signSvr:" + signSvr);
                System.Diagnostics.Debug.WriteLine("percent:" + percent);
                System.Diagnostics.Debug.WriteLine("f_down.jsp 业务逻辑参数为空。");
                return false;
            }
            return true;
        }

        void saveToRedis()
        {
            //仅添加单文件下载
            if (string.IsNullOrEmpty(fd_signSvr))
            {
                //添加到缓存
                var j = RedisConfig.getCon();
                FileRedis fr = new FileRedis(ref j);
                fr.process(this.signSvr, this.percent, this.lenLoc);
            }//更新文件夹进度
            else
            {
                //仅更新文件夹进度
                var j = RedisConfig.getCon();
                FileRedis fr = new FileRedis(ref j);
                fr.process(fd_signSvr, fd_percent, fd_lenLoc);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.recvParam();
            if (!this.checkParam()) return;
            this.saveToRedis();
            
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Pragma", "No-cache");
            Response.AddHeader("Cache-Control", "no-cache");
            Response.AddHeader("Expires", "0");
            Response.AddHeader("Content-Disposition", "attachment;filename=" + nameLoc);
            Response.AddHeader("Content-Length", rangeSize.ToString());

            Stream iStream = null;
            try
            {
                // Open the file.
                iStream = new FileStream(pathSvr, FileMode.Open, FileAccess.Read, FileShare.Read);
                iStream.Seek(long.Parse(fileOffset), SeekOrigin.Begin);

                // Total bytes to read:
                long dataToRead = long.Parse(rangeSize);

                byte[] buffer = new Byte[10000];
                int length;
                // Read the bytes.
                while (dataToRead > 0)
                {
                    // Verify that the client is connected.
                    if (Response.IsClientConnected)
                    {
                        // Read the data in buffer.
                        length = iStream.Read(buffer, 0, 10000);

                        // Write the data to the current output stream.
                        Response.OutputStream.Write(buffer, 0, length);

                        // Flush the data to the HTML output.
                        Response.Flush();

                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        //prevent infinite loop if user disconnects
                        dataToRead = -1;
                    }
                }
                Response.AddHeader("f-ret", "ok");
            }
            catch (Exception ex)
            {
                // Trap the error, if any.
                Response.Write("Error : " + ex.Message);
            }
            finally
            {
                if (iStream != null)
                {
                    //Close the file.
                    iStream.Close();
                }
            }

        }
    }
}