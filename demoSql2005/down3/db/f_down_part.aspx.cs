using System;
using System.IO;
using System.Web;
using up7.demoSql2005.db.redis;
using up7.demoSql2005.down3.biz;
using up7.demoSql2005.down3.model;

namespace up7.demoSql2005.down3.db
{
    /// <summary>
    /// 下载文件块，对没有合并的文件块进行处理
    /// </summary>
    public partial class f_down_part : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String lenSvr = Request.Headers["f-lenSvr"];
            String nameLoc = Request.Headers["f-nameLoc"];
            String sizeSvr = Request.Headers["f-sizeSvr"];
            String pathSvr = Request.Headers["f-pathSvr"];
            String pathLoc = Request.Headers["f-pathLoc"];
            String blockIndex = Request.Headers["f-blockIndex"];
            String blockOffset = Request.Headers["f-blockOffset"];
            String blockSize = Request.Headers["f-blockSize"];//逻辑块大小
            String rangeSize = Request.Headers["f-rangeSize"];//当前请求的块大小
            String lenLoc = Request.Headers["f-lenLoc"];
            String signSvr = Request.Headers["f-signSvr"];
            String fd_signSvr = Request.Headers["fd-signSvr"];
            String fd_lenLoc = Request.Headers["fd-lenLoc"];
            String fd_sizeLoc = Request.Headers["fd-sizeLoc"];
            String uid = Request.Headers["f-uid"];
            String percent = Request.Headers["f-percent"];

            pathSvr = pathSvr.Replace("+", "%20");
            pathLoc = pathLoc.Replace("+", "%20");
            nameLoc = nameLoc.Replace("+", "%20");
            pathSvr = HttpUtility.UrlDecode(pathSvr);//utf-8解码
            pathLoc = HttpUtility.UrlDecode(pathLoc);//utf-8解码
            nameLoc = HttpUtility.UrlDecode(nameLoc);//utf-8解码

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
                return;
            }

            DnFileInf fileSvr = new DnFileInf();
            fileSvr.signSvr = signSvr;
            //子文件项时仅保存文件夹信息
            if (fd_signSvr != null) fileSvr.signSvr = fd_signSvr;
            fileSvr.uid = uid == null ? 0 : int.Parse(uid);
            fileSvr.lenLoc = long.Parse(lenLoc);
            if (fd_lenLoc != null) fileSvr.lenLoc = long.Parse(fd_lenLoc);
            fileSvr.lenSvr = long.Parse(lenSvr);
            fileSvr.sizeSvr = sizeSvr == null ? "" : sizeSvr;
            fileSvr.perLoc = percent;
            fileSvr.pathSvr = pathSvr;
            fileSvr.pathLoc = pathLoc;
            fileSvr.nameLoc = nameLoc == null ? "" : nameLoc;

            //添加到缓存
            var j = RedisConfig.getCon();
            tasks svr = new tasks(uid, j);
            svr.add(fileSvr);

            long fileLen = fileSvr.lenSvr;

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
                iStream.Seek(long.Parse(blockOffset), SeekOrigin.Begin);

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