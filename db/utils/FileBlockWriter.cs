using System.IO;
using System.Web;

namespace up7.db.utils
{
    /// <summary>
    /// 文件块处理器
    /// 优化文件创建逻辑，按文件实际大小创建
    /// </summary>
    public class FileBlockWriter
	{
		public FileBlockWriter()
		{
		}

		/// <summary>
		/// 根据文件大小创建文件。
		/// </summary>
		public void make(string filePath,long len)
		{
            //文件不存在则创建
            if (string.IsNullOrEmpty(filePath)) return;
			if (!File.Exists(filePath))
			{
                //自动创建目录
                if(!Directory.Exists(filePath)) Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                //创建文件
                FileStream fs = new FileStream(filePath, FileMode.Create);
                fs.SetLength(len);
                fs.Close();
			}
		}

		/// <summary>
		/// 续传文件
		/// </summary>
		/// <param name="fileRange">文件块</param>
		/// <param name="path">远程文件完整路径。d:\www\web\upload\201204\10\md5.exe</param>
		public void write(string path,long fileLen, long offset, ref HttpPostedFile fileRange)
		{
			//上传的文件大小不为空
			if (fileRange.InputStream.Length > 0)
			{
                //创建文件
                if(offset==0)
                {
                    if (!File.Exists(path)) this.make(path, fileLen);
                }

                //文件已存在，写入数据
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.Write);
				fs.Seek(offset, SeekOrigin.Begin);                
				byte[] ByteArray = new byte[fileRange.InputStream.Length];
				fileRange.InputStream.Read(ByteArray, 0, (int)fileRange.InputStream.Length);
				fs.Write(ByteArray, 0, (int)fileRange.InputStream.Length);
				fs.Flush();
				fs.Close();
			}

		}
	}
}
