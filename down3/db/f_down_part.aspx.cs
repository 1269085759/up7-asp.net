using System;
using System.IO;
using up7.db.utils;

namespace up7.down3.db
{
    /// <summary>
    /// 下载文件块，对没有合并的文件块进行处理
    /// </summary>
    public partial class f_down_part : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String pathSvr      = Request.Headers["pathSvr"];
            String blockPath    = Request.Headers["blockPath"];
            String blockIndex   = Request.Headers["blockIndex"];//基于1
            String blockOffset  = Request.Headers["blockOffset"];//基于块的偏移位置
            String fileOffset   = Request.Headers["fileOffset"];//基于文件的偏移位置
            String blockSize    = Request.Headers["blockSize"];//当前块大小
            blockPath = PathTool.url_decode(blockPath);

            if (   string.IsNullOrEmpty(blockIndex)
                || string.IsNullOrEmpty(blockPath)
                || string.IsNullOrEmpty(blockSize)
                || string.IsNullOrEmpty(pathSvr)
                )
            {
                System.Diagnostics.Debug.WriteLine("pathSvr:" + pathSvr);
                System.Diagnostics.Debug.WriteLine("blockIndex:" + blockIndex);
                System.Diagnostics.Debug.WriteLine("blockSize:" + blockSize);
                System.Diagnostics.Debug.WriteLine("blockPath:" + blockPath);
                System.Diagnostics.Debug.WriteLine("f_down.jsp 业务逻辑参数为空。");
                Response.Write("param is null");
                Response.StatusCode = 500;
                return;
            }

            long dataToRead = long.Parse(blockSize);

            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Pragma", "No-cache");
            Response.AddHeader("Cache-Control", "no-cache");
            Response.AddHeader("Expires", "0");
            Response.AddHeader("Content-Length", dataToRead.ToString());

            Stream iStream = null;
            try
            {
                pathSvr = Path.Combine(blockPath, blockIndex + ".part");
                // Open the file.
                iStream = new FileStream(pathSvr, FileMode.Open, FileAccess.Read, FileShare.Read);
                iStream.Seek(long.Parse(blockOffset), SeekOrigin.Begin);

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