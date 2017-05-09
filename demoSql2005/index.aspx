<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="up6.demoSql2005.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>up7-SQL2005��ʾҳ��</title>
    <link href="js/up7.css" type="text/css" rel="Stylesheet" charset="gb2312"/>
    <script type="text/javascript" src="js/jquery-1.4.min.js"></script>
    <script type="text/javascript" src="js/json2.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="js/up7.config.js" charset="utf-8"></script>
    <script type="text/javascript" src="js/up7.file.js" charset="utf-8"></script>
    <script type="text/javascript" src="js/up7.folder.js" charset="utf-8"></script>
    <script type="text/javascript" src="js/up7.js" charset="utf-8"></script>
    <script language="javascript" type="text/javascript">
        var cbMgr = new HttpUploaderMgr();
        cbMgr.event.md5Complete = function (obj, md5) { /*alert(md5);*/ };
        cbMgr.event.fileComplete = function (obj) { /*alert(obj.pathSvr);*/ };
        cbMgr.Config["Cookie"] = 'ASP.NET_SessionId=<%=Session.SessionID%>';
        //ʹ�ò�ͬ��Ŀ����
        //cbMgr.set_config(up6_config.qq);
        //cbMgr.set_config(up6_config.qq_mail);
        //cbMgr.set_config(up6_config.qq_zone);
        //cbMgr.set_config(up6_config.erp);
        //cbMgr.set_config(up6_config.oa);
        //cbMgr.set_config(up6_config.share_point);
        //cbMgr.set_config(up6_config.vm);

        $(document).ready(function ()
        {
            cbMgr.load_to("FilePanel");
            //�ϴ�ָ���ļ�
            $("#btnUpF").click(function () {
                var path = $("#filePath").val();
                cbMgr.browser.addFile({ pathLoc: path });
            });
            //�ϴ�ָ��Ŀ¼
            $("#btnUpFd").click(function () {
                var path = $("#folderPath").val();
                cbMgr.browser.addFolder({ pathLoc: path });
            });
        });
    </script>
</head>
<body>
    <p>up6.2���ļ��ϴ���ʾҳ��</p>
    <p><a href="debug/check_config.aspx" target="_blank">�������</a></p>
    <p><a href="db/clear.aspx" target="_blank">������ݿ��Redis����</a></p>
    <p><a href="down3/index.htm" target="_blank">������ҳ��</a></p>
    <p>
        �ļ�·����<input id="filePath" type="text" size="50" value="D:\\360safe-inst.exe" />&nbsp;
        <input id="btnUpF" type="button" value="�ϴ������ļ�" />
    </p>
    <p>
        Ŀ¼·����<input id="folderPath" type="text" size="50" value="C:\\Users\\Administrator\\Desktop\\test" />&nbsp;
        <input id="btnUpFd" type="button" value="�ϴ�����Ŀ¼" />
    </p>
	<div id="FilePanel"></div>
    <div id="msg"></div>
</body>
</html>

