using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace up7.demoSql2005.down3.model
{
    public class DnFolderInf : DnFileInf
    {
        public DnFolderInf()
        {
            this.fdTask = true;
            this.m_files = new List<DnFileInf>();
        }
    }
}