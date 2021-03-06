<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="up7.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>up7-SQL2005演示页面</title>
    <link href="js/up7.css" type="text/css" rel="Stylesheet" charset="gb2312"/>
    <script type="text/javascript" src="js/jquery-1.4.min.js"></script>
    <script type="text/javascript" src="js/json2.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="js/up7.app.js" charset="utf-8"></script>
    <script type="text/javascript" src="js/up7.edge.js" charset="utf-8"></script>
    <script type="text/javascript" src="js/up7.config.js" charset="utf-8"></script>
    <script type="text/javascript" src="js/up7.file.js" charset="utf-8"></script>
    <script type="text/javascript" src="js/up7.folder.js" charset="utf-8"></script>
    <script type="text/javascript" src="js/up7.js" charset="utf-8"></script>
    <script language="javascript" type="text/javascript">
        var cbMgr = new HttpUploaderMgr();
        cbMgr.event.md5Complete = function (obj, md5) { /*alert(md5);*/ };
        cbMgr.event.fileComplete = function (obj) { /*alert(obj.pathSvr);*/ };
        cbMgr.Config["Cookie"] = 'ASP.NET_SessionId=<%=Session.SessionID%>';
        //使用不同项目配置
        //cbMgr.set_config(up7_config.qq);
        //cbMgr.set_config(up7_config.qq_mail);
        //cbMgr.set_config(up7_config.qq_zone);
        //cbMgr.set_config(up7_config.erp);
        //cbMgr.set_config(up7_config.oa);
        //cbMgr.set_config(up7_config.share_point);
        //cbMgr.set_config(up7_config.vm);

        $(document).ready(function ()
        {
            cbMgr.load_to("FilePanel");
            //上传指定文件
            $("#btnUpF").click(function () {
                var path = $("#filePath").val();
                cbMgr.app.addFile({ pathLoc: path });
            });
            //上传指定目录
            $("#btnUpFd").click(function () {
                var path = $("#folderPath").val();
                cbMgr.app.addFolder({ pathLoc: path });
            });
        });
    </script>
</head>
<body>
    <p>up7.2多文件上传演示页面</p>
    <p><a href="debug/check_config.aspx" target="_blank">检查配置</a></p>
    <p><a href="db/clear.aspx" target="_blank">清空数据库和Redis缓存</a></p>
    <p><a href="down3/index.htm" target="_blank">打开下载页面</a></p>
    <p>注意：文件夹上传功能必须启动redis服务。文件上传功能不需要此服务。</p>
    <p>
        文件路径：<input id="filePath" type="text" size="50" value="D:\\360safe-inst.exe" />&nbsp;
        <input id="btnUpF" type="button" value="上传本地文件" />
    </p>
    <p>
        目录路径：<input id="folderPath" type="text" size="50" value="C:\\Users\\Administrator\\Desktop\\test" />&nbsp;
        <input id="btnUpFd" type="button" value="上传本地目录" />
    </p>
	<div id="FilePanel"></div>
    <div id="msg"></div>
</body>
</html>

