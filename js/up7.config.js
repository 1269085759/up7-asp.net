var up7_config = {
    qq: { 
        //文件夹操作相关
          "UrlFdCreate"		: "http://www.qq.com/demoSql2005/db/fd_create.aspx"
		, "UrlFdComplete"	: "http://www.qq.com/demoSql2005/db/fd_complete.aspx"
		, "UrlFdDel"	    : "http://www.qq.com/demoSql2005/db/fd_del.aspx"
		, "UrlFdFile"	    : "http://www.qq.com/demoSql2005/db/fd_file.aspx"
    //文件操作相关
		, "UrlCreate"		: "http://www.qq.com/demoSql2005/db/f_create.aspx"
		, "UrlPost"			: "http://www.qq.com/demoSql2005/db/f_post.aspx"
		, "UrlComplete"		: "http://www.qq.com/demoSql2005/db/f_complete.aspx"
		, "UrlList"			: "http://www.qq.com/demoSql2005/db/f_list.aspx"
		, "UrlDel"			: "http://www.qq.com/demoSql2005/db/f_del.aspx"
	    //x86
        , ie: {
              drop: { clsid: "0868BADD-C17E-4819-81DE-1D60E5E734A6", name: "QQ.HttpDroper6" }
            , part: { clsid: "BA0B719E-F4B7-464b-A664-6FC02126B652", name: "QQ.HttpPartition6" }
            , path: "http://www.qq.com/up7/HttpUploader6.cab"
        }
	    //x64
        , ie64: {
              drop: { clsid: "7B9F1B50-A7B9-4665-A6D1-0406E643A856", name: "QQ.HttpDroper6x64" }
            , part: { clsid: "307DE0A1-5384-4CD0-8FA8-500F0FFEA388", name: "QQ.HttpPartition6x64" }
            , path: "http://www.qq.com/up7/HttpUploader64.cab"
        }
        , firefox: { name: "", type: "application/npup7QQ", path: "http://www.qq.com/up7/HttpUploader6.xpi" }
        , chrome: { name: "npup7QQ", type: "application/npup7QQ", path: "http://www.qq.com/up7/HttpUploader6.crx" }
        , exe: { path: "http://www.qq.com/up7/HttpUploader6.exe" }
    }
    //qq mail
    ,qq_mail:{ 
        //文件夹操作相关
          "UrlFdCreate"		: "http://mqil.qq.com/demoSql2005/db/fd_create.aspx"
		, "UrlFdComplete"	: "http://mqil.qq.com/demoSql2005/db/fd_complete.aspx"
		, "UrlFdDel"	    : "http://mqil.qq.com/demoSql2005/db/fd_del.aspx"
		, "UrlFdFile"	    : "http://mqil.qq.com/demoSql2005/db/fd_file.aspx"
    //文件操作相关
		, "UrlCreate"		: "http://mqil.qq.com/demoSql2005/db/f_create.aspx"
		, "UrlPost"			: "http://mqil.qq.com/demoSql2005/db/f_post.aspx"
		, "UrlComplete"		: "http://mqil.qq.com/demoSql2005/db/f_complete.aspx"
		, "UrlList"			: "http://mqil.qq.com/demoSql2005/db/f_list.aspx"
		, "UrlDel"			: "http://mqil.qq.com/demoSql2005/db/f_del.aspx"
	    //x86
        , ie: {
              drop: { clsid: "0868BADD-C17E-4819-81DE-1D60E5E734A6", name: "QQMail.HttpDroper6" }
            , part: { clsid: "BA0B719E-F4B7-464b-A664-6FC02126B652", name: "QQMail.HttpPartition6" }
            , path: "http://mail.qq.com/up7/HttpUploader6.cab"
        }
	    //x64
        , ie64: {
              drop: { clsid: "7B9F1B50-A7B9-4665-A6D1-0406E643A856", name: "QQMail.HttpDroper6x64" }
            , part: { clsid: "307DE0A1-5384-4CD0-8FA8-500F0FFEA388", name: "QQMail.HttpPartition6x64" }
            , path: "http://mail.qq.com/up7/HttpUploader64.cab"
        }
        , firefox: { name: "", type: "application/npup7QQMail", path: "http://mail.qq.com/up7/HttpUploader6.xpi" }
        , chrome: { name: "npup7QQMail", type: "application/npup7QQMail", path: "http://mail.qq.com/up7/HttpUploader6.crx" }
        , exe: { path: "http://mail.qq.com/up7/HttpUploader6.exe" }
    }
    //qq zone
    ,qq_zone:{ 
        //文件夹操作相关
          "UrlFdCreate"		: "http://qzone.qq.com/demoSql2005/db/fd_create.aspx"
		, "UrlFdComplete"	: "http://qzone.qq.com/demoSql2005/db/fd_complete.aspx"
		, "UrlFdDel"	    : "http://qzone.qq.com/demoSql2005/db/fd_del.aspx"
		, "UrlFdFile"	    : "http://qzone.qq.com/demoSql2005/db/fd_file.aspx"
    //文件操作相关
		, "UrlCreate"		: "http://qzone.qq.com/demoSql2005/db/f_create.aspx"
		, "UrlPost"			: "http://qzone.qq.com/demoSql2005/db/f_post.aspx"
		, "UrlComplete"		: "http://qzone.qq.com/demoSql2005/db/f_complete.aspx"
		, "UrlList"			: "http://qzone.qq.com/demoSql2005/db/f_list.aspx"
		, "UrlDel"			: "http://qzone.qq.com/demoSql2005/db/f_del.aspx"
	    //x86
        , ie: {
              drop: { clsid: "0868BADD-C17E-4819-81DE-1D60E5E734A6", name: "QZone.HttpDroper6" }
            , part: { clsid: "BA0B719E-F4B7-464b-A664-6FC02126B652", name: "QZone.HttpPartition6" }
            , path: "http://mail.qq.com/up7/HttpUploader6.cab"
        }
	    //x64
        , ie64: {
              drop: { clsid: "7B9F1B50-A7B9-4665-A6D1-0406E643A856", name: "QZone.HttpDroper6x64" }
            , part: { clsid: "307DE0A1-5384-4CD0-8FA8-500F0FFEA388", name: "QZone.HttpPartition6x64" }
            , path: "http://qzone.qq.com/up7/HttpUploader64.cab"
        }
        , firefox: { name: "", type: "application/npup7QZone", path: "http://qzone.qq.com/up7/HttpUploader6.xpi" }
        , chrome: { name: "npup7QZone", type: "application/npup7QZone", path: "http://qzone.qq.com/up7/HttpUploader6.crx" }
        , exe: { path: "http://qzone.qq.com/up7/HttpUploader6.exe" }
    }
    //oa项目
    ,oa:{ 
        //文件夹操作相关
          "UrlFdCreate"		: "http://oa.qq.com/demoSql2005/db/fd_create.aspx"
		, "UrlFdComplete"	: "http://oa.qq.com/demoSql2005/db/fd_complete.aspx"
		, "UrlFdDel"	    : "http://oa.qq.com/demoSql2005/db/fd_del.aspx"
		, "UrlFdFile"	    : "http://oa.qq.com/demoSql2005/db/fd_file.aspx"
    //文件操作相关
		, "UrlCreate"		: "http://oa.qq.com/demoSql2005/db/f_create.aspx"
		, "UrlPost"			: "http://oa.qq.com/demoSql2005/db/f_post.aspx"
		, "UrlComplete"		: "http://oa.qq.com/demoSql2005/db/f_complete.aspx"
		, "UrlList"			: "http://oa.qq.com/demoSql2005/db/f_list.aspx"
		, "UrlDel"			: "http://oa.qq.com/demoSql2005/db/f_del.aspx"
        , exe: { path: "http://oa.qq.com/up7/HttpUploader6.exe" }
    }
    //erp
    , erp: { 
        //文件夹操作相关
          "UrlFdCreate"		: "http://erp.qq.com/demoSql2005/db/fd_create.aspx"
		, "UrlFdComplete"	: "http://erp.qq.com/demoSql2005/db/fd_complete.aspx"
		, "UrlFdDel"	    : "http://erp.qq.com/demoSql2005/db/fd_del.aspx"
		, "UrlFdFile"	    : "http://erp.qq.com/demoSql2005/db/fd_file.aspx"
    //文件操作相关
		, "UrlCreate"		: "http://erp.qq.com/demoSql2005/db/f_create.aspx"
		, "UrlPost"			: "http://erp.qq.com/demoSql2005/db/f_post.aspx"
		, "UrlComplete"		: "http://erp.qq.com/demoSql2005/db/f_complete.aspx"
		, "UrlList"			: "http://erp.qq.com/demoSql2005/db/f_list.aspx"
		, "UrlDel"			: "http://erp.qq.com/demoSql2005/db/f_del.aspx"
        , exe: { path: "http://erp.qq.com/up7/HttpUploader6.exe" }
    }
    //share point
    ,share_point:{ 
        //文件夹操作相关
          "UrlFdCreate"		: "http://share.point.qq.com/demoSql2005/db/fd_create.aspx"
		, "UrlFdComplete"	: "http://share.point.qq.com/demoSql2005/db/fd_complete.aspx"
		, "UrlFdDel"	    : "http://share.point.qq.com/demoSql2005/db/fd_del.aspx"
		, "UrlFdFile"	    : "http://share.point.qq.com/demoSql2005/db/fd_file.aspx"
    //文件操作相关
		, "UrlCreate"		: "http://share.point.qq.com/demoSql2005/db/f_create.aspx"
		, "UrlPost"			: "http://share.point.qq.com/demoSql2005/db/f_post.aspx"
		, "UrlComplete"		: "http://share.point.qq.com/demoSql2005/db/f_complete.aspx"
		, "UrlList"			: "http://share.point.qq.com/demoSql2005/db/f_list.aspx"
		, "UrlDel"			: "http://share.point.qq.com/demoSql2005/db/f_del.aspx"
        , exe: { path: "http://share.point.qq.com/up7/HttpUploader6.exe" }
    }
};