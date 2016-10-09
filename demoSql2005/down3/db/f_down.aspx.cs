using System;
using System.IO;
using System.Text;
using System.Web;

namespace up6.demoSql2005.down2.db
{
    public partial class f_down : System.Web.UI.Page
    {
        /// <summary>
        /// 为字符串中的非英文字符编码Encodes non-US-ASCII characters in a string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToHexString(string s)
        {
            char[] chars = s.ToCharArray();
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < chars.Length; index++)
            {
                bool needToEncode = NeedToEncode(chars[index]);
                if (needToEncode)
                {
                    string encodedString = ToHexString(chars[index]);
                    builder.Append(encodedString);
                }
                else
                {
                    builder.Append(chars[index]);
                }
            }
            return builder.ToString();
        }
        /// <summary>
        ///指定一个字符是否应该被编码 Determines if the character needs to be encoded.
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        private static bool NeedToEncode(char chr)
        {
            string reservedChars = "$-_.+!*'(),@=&";
            if (chr > 127)
                return true;
            if (char.IsLetterOrDigit(chr) || reservedChars.IndexOf(chr) >= 0)
                return false;
            return true;
        }
        /// <summary>
        /// 为非英文字符串编码Encodes a non-US-ASCII character.
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        private static string ToHexString(char chr)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] encodedBytes = utf8.GetBytes(chr.ToString());
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < encodedBytes.Length; index++)
            {
                builder.AppendFormat("%{0}", Convert.ToString(encodedBytes[index], 16));
            }
            return builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string fid = Request.QueryString["fid"];
            if (String.IsNullOrEmpty(fid))
            {
                return;
            }
            demoSql2005.db.DBFile f = new demoSql2005.db.DBFile();
            demoSql2005.db.xdb_files inf = new demoSql2005.db.xdb_files();
            //数据库不存在文件
            if (!f.GetFileInfByFid(int.Parse(fid), ref inf))
            {
                Response.AddHeader("Content-Range", "0-0/0");
                Response.AddHeader("Content-Length", "0");
                return;
            }
            string nameLoc = Path.GetFileName(inf.nameLoc);
            string ext = Path.GetExtension(nameLoc);
            string encodefileName = ToHexString(nameLoc);//使用自定义的
            if (Request.Browser.Browser.Contains("IE"))
            {
                string name = encodefileName.Remove(encodefileName.Length - ext.Length);//得到文件名称
                name = name.Replace(".", "%2e"); //关键代码
                encodefileName = name + ext;
            }
            string pathSvr = inf.pathSvr;

            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] encodedBytes = utf8.GetBytes(nameLoc);

            string fnUtf8 = Encoding.UTF8.GetString(Encoding.GetEncoding("gb2312").GetBytes(nameLoc));
            fnUtf8 = HttpUtility.UrlEncode(nameLoc);

            Stream iStream = null;
            try
            {
                // Open the file.
                iStream = new System.IO.FileStream(pathSvr, FileMode.Open, FileAccess.Read, FileShare.Read);

                // Total bytes to read:
                long dataToRead = iStream.Length;

                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fnUtf8 + "\"");
                /*
                    表示头500个字节：bytes=0-499
                    表示第二个500字节：bytes=500-999
                    表示最后500个字节：bytes=-500
                    表示500字节以后的范围：bytes=500-
                    第一个和最后一个字节：bytes=0-0,-1
                    同时指定几个范围：bytes=500-600,601-999
                */
                string range = Request.Headers.Get("Range");//续传
                if (!string.IsNullOrEmpty(range))
                {
                    string[] rs = range.Split("-".ToCharArray());//bytes=10254
                    int posBegin = rs[0].IndexOf("=") + 1;
                    string pos = rs[0].Substring(posBegin);
                    long offset_begin = long.Parse(pos);
                    iStream.Seek(offset_begin, SeekOrigin.Begin);

                    string con_range;
                    if (rs.Length == 2)
                    {
                        string offset_end = rs[1];
                        long totalLen = long.Parse(offset_end) - offset_begin;
                        //fix(2016-09-13):只有1byte的文件
                        if(totalLen>1) ++totalLen;//字段大小+1
                        dataToRead = totalLen;//
                        con_range = string.Format("bytes {0}-{1}/{2}", offset_begin, offset_end, iStream.Length);
                    }
                    else
                    {
                        dataToRead -= offset_begin;//fix(2015-08-12):修复返回长度不正确的问题。
                        con_range = string.Format("bytes {0}-{1}/{2}", offset_begin, dataToRead, iStream.Length);
                    }

                    //add(2016-08-30):添加content-range，为多线程提供支持
                    Response.AddHeader("Content-Range", con_range);
                }
                Response.AddHeader("Content-Length", dataToRead.ToString());

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