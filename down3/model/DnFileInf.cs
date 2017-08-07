using up7.db.model;

namespace up7.down3.model
{
    public class DnFileInf : FileInf
    {
        public DnFileInf()
        {
        }

        public string mac = string.Empty;
        public string idFile = string.Empty;//与up7_files.f_id表关联
        public string fileUrl = string.Empty;
        //本地已下载百分比
        public string perLoc = string.Empty;
    }
}