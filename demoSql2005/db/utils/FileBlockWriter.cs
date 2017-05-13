﻿using System.Web;
using System.IO;
using System.Threading;

namespace up7.demoSql2005.db.utils
{
    /// <summary>
    /// 文件块处理器
    /// 优化文件创建逻辑，按文件实际大小创建
    /// </summary>
    public class FileBlockWriter
	{
		public long m_lenLoc;		//文件总大小。

		//文件读写锁，防止多个用户同时上传相同文件时，出现创建文件的错误
		static ReaderWriterLock m_writeLock = new ReaderWriterLock();

		public FileBlockWriter()
		{
		}

		/// <summary>
		/// 根据文件大小创建文件。
		/// 注意：多个用户同时上传相同文件时，可能会同时创建相同文件。
		/// </summary>
		public void make(string filePath,long len)
		{
            //文件不存在则创建
            if (string.IsNullOrEmpty(filePath)) return;
			if (!File.Exists(filePath))
			{
                //自动创建目录
                if(!Directory.Exists(filePath)) Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                FileStream fs = new FileStream(filePath, FileMode.Create);
                fs.SetLength(len);
                fs.Close();
				//FileStream fs = File.Create(filePath);
				//BinaryWriter w = new BinaryWriter(fs);
                //fix(2015-03-16):不再按实际大小创建文件，减少上传大文件时用户等待的时间。
                //w.Write((byte)0);
                //for (long i = 0; i < this.m_FileSize; ++i)
                //{
                //    w.Write((byte)0);
                //}
				//w.Close();
				//fs.Close();
			}
		}

		/// <summary>
		/// 续传文件
		/// </summary>
		/// <param name="fileRange">文件块</param>
		/// <param name="path">远程文件完整路径。d:\www\web\upload\201204\10\md5.exe</param>
		public void write(string path, long offset, ref HttpPostedFile fileRange)
		{
			//上传的文件大小不为空
			if (fileRange.InputStream.Length > 0)
			{
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
