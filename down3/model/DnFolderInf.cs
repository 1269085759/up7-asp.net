using System.Collections.Generic;

namespace up7.down3.model
{
    public class DnFolderInf : DnFileInf
    {
        public DnFolderInf()
        {
            this.folder = true;
            this.m_files = new List<DnFileInf>();
        }
    }
}