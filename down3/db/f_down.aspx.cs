using System;
using System.IO;
using up7.db.utils;

namespace up7.down3.db
{
    public partial class f_down : System.Web.UI.Page
    {
        String pathSvr      = string.Empty;
        String blockIndex   = string.Empty;
        String fileOffset   = string.Empty;
        String blockOffset  = string.Empty;
        String blockSize    = string.Empty;

        void recvParam()
        {
            this.pathSvr        = Request.Headers["pathSvr"];
            this.blockIndex     = Request.Headers["blockIndex"];//块索引基于1
            this.fileOffset     = Request.Headers["fileOffset"];//基于文件的位置
            this.blockOffset    = Request.Headers["blockOffset"];//基于块的位置
            this.blockSize      = Request.Headers["blockSize"];//当前需要下载的块大小

            pathSvr = PathTool.url_decode(pathSvr);
        }

        bool checkParam()
        {
            if (   string.IsNullOrEmpty(pathSvr)
                || string.IsNullOrEmpty(blockIndex)
                || string.IsNullOrEmpty(blockOffset)
                || string.IsNullOrEmpty(fileOffset)
                || string.IsNullOrEmpty(blockSize)
                )
            {
                System.Diagnostics.Debug.WriteLine("pathSvr:" + pathSvr);
                System.Diagnostics.Debug.WriteLine("blockIndex:" + blockIndex);
                System.Diagnostics.Debug.WriteLine("blockSize:" + blockSize);
                System.Diagnostics.Debug.WriteLine("f_down.jsp 业务逻辑参数为空。");
                return false;
            }
            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.recvParam();
            if (!this.checkParam()) return;
            
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Pragma", "No-cache");
            Response.AddHeader("Cache-Control", "no-cache");
            Response.AddHeader("Expires", "0");
            Response.AddHeader("Content-Length", blockSize);

            Stream iStream = null;
            try
            {
                // Open the file.
                iStream = new FileStream(pathSvr, FileMode.Open, FileAccess.Read, FileShare.Read);
                iStream.Seek(long.Parse(fileOffset), SeekOrigin.Begin);

                // Total bytes to read:
                long dataToRead = long.Parse(blockSize);

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
                Response.StatusCode = 500;
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