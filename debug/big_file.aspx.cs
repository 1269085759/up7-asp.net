using System;
using System.IO;
using up7.db.utils;

namespace up7.debug
{
    public partial class big_file : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.file_create();
        }

        void create4()
        {
            File.Delete("D:\\test.rar");
            BinaryWriter bw = new BinaryWriter(new FileStream("D:\\test.rar", FileMode.Create));
            Random ran = new Random();

            long mb = 1048576;

            for (int i = 0; i < (mb * 500); ++i)
            {
                int RandKey = ran.Next(524288000);
                bw.Write((byte)RandKey);
            }
            bw.Close();
        }

        void file_create()
        {
            long gb = 1073741824;
            FileBlockWriter fw = new FileBlockWriter();
            fw.make("d:\\test.db", gb * 10);
        }
    }
}