﻿/*
版权所有(C) 2009-2018 荆门泽优软件有限公司
保留所有权利
官方网站：http://www.ncmem.com
官方博客：http://www.cnblogs.com/xproer
产品首页：http://www.ncmem.com/webplug/http-downloader/index.asp
在线演示：http://www.ncmem.com/products/http-downloader/demo/index.html
开发文档：http://www.cnblogs.com/xproer/archive/2011/03/15/1984950.html
升级日志：http://www.cnblogs.com/xproer/archive/2011/03/15/1985091.html
示例下载(asp.net)：http://www.ncmem.com/download/up7/asp.net/up7.rar
示例下载(jsp-mysql)：http://www.ncmem.com/download/up7/jsp/Uploader7Mysql.rar
示例下载(jsp-oracle)：http://www.ncmem.com/download/up7/jsp/Uploader7Oracle.rar
示例下载(jsp-sql)：http://www.ncmem.com/download/up7/jsp/Uploader7SQL.rar
示例下载(php)：http://www.ncmem.com/download/up7/php/up7.rar
文档下载：http://www.ncmem.com/download/up7/up7-doc.rar
联系邮箱：1085617561@qq.com
联系QQ：1085617561
版本：7.2.2
更新记录：
    2009-11-05 创建
	2014-02-27 优化版本号。
    2015-08-13 优化
*/
function debug_msg(v) { $(document.body).append("<div>"+v+"</div>");}
//删除元素值
Array.prototype.remove = function(val)
{
	for (var i = 0, n = 0; i < this.length; i++)
	{
		if (this[i] != val)
		{
			this[n++] = this[i]
		}
	}
	this.length -= 1
}

function DownloaderMgr()
{
	var _this = this;
	this.Config = {
		  "Folder"		: ""
		, "Debug"		: false//调试模式
		, "LogFile"		: "f:\\log.txt"//日志文件路径。
		, "Company"		: "荆门泽优软件有限公司"
		, "Version"		: "1,2,110,51262"
		, "License"		: ""//
		, "Cookie"		: ""//
		, "ThreadCount"	: 1//并发数
		, "FilePart"	: 10485760//文件块大小，更新进度时使用，计算器：http://www.beesky.com/newsite/bit_byte.htm
		, "ThreadBlock"	: 3//文件块线程数
        //file
        , "UrlCreate"   : "http://localhost:88/down3/db/f_create.aspx"
        , "UrlDel"      : "http://localhost:88/down3/db/f_del.aspx"
        , "UrlList"     : "http://localhost:88/down3/db/f_list.aspx"
        , "UrlListCmp"  : "http://localhost:88/down3/db/f_list_cmp.aspx"
        , "UrlUpdate"   : "http://localhost:88/down3/db/f_update.aspx"
        , "UrlDown"     : "http://localhost:88/down3/db/f_down.aspx"
	    //folder
        , "UrlFdCreate" : "http://localhost:88/down3/db/fd_create.aspx"
        , "UrlFdPage"   : "http://localhost:88/down3/db/fd_page.aspx"
        //x86
        , ie: {
              part: { clsid: "57FA11EE-5E98-415C-933D-BCA188B86B5E", name: "Xproer.DownloaderPartition3" }
            , path: "http://www.ncmem.com/download/down3/3.2/down3.cab"
        }
        //x64
        , ie64: {
            part: { clsid: "21B0B682-5C37-470D-8DFF-950EF93FFC08", name: "Xproer.DownloaderPartition3x64" }
            , path: "http://www.ncmem.com/download/down3/3.2/down64.cab"
        }
        , firefox: { name: "", type: "application/npHttpDown3", path: "http://www.ncmem.com/download/down3/3.2/down3.xpi" }
        , chrome: { name: "npHttpDown3", type: "application/npHttpDown3", path: "http://www.ncmem.com/download/down3/3.2/down3.crx" }
	    //Chrome 45
        , chrome45: { name: "com.xproer.down3", path: "http://www.ncmem.com/download/down3/3.2/down3.nat.crx" }
        , exe: { path: "http://www.ncmem.com/download/down3/3.2/down3.exe" }
        , edge: {protocol:"down3",port:9700,visible:false}
        , "Fields": {"uname": "test","upass": "test","uid":"0"}
    };

    this.event = {
          downComplete: function (obj) { }
        , downError: function (obj, err) { }
        , queueComplete: function () { }
	};
	
	var browserName = navigator.userAgent.toLowerCase();
	this.ie = browserName.indexOf("msie") > 0;
	this.ie = this.ie ? this.ie : browserName.search(/(msie\s|trident.*rv:)([\w.]+)/) != -1;
	this.firefox = browserName.indexOf("firefox") > 0;
	this.chrome = browserName.indexOf("chrome") > 0;
	this.chrome45 = false;
	this.nat_load = false;
    this.chrVer = navigator.appVersion.match(/Chrome\/(\d+)/);
    this.edge = navigator.userAgent.indexOf("Edge") > 0;
    this.edgeApp = new WebServer(this);
    this.app = up6_app;
    this.app.edgeApp = this.edgeApp;
    this.app.Config = this.Config;
    this.app.ins = this;
    if (this.edge) { this.ie = this.firefox = this.chrome = this.chrome45 = false; }
	
	this.idCount = 1; 	//上传项总数，只累加
	this.queueCount = 0;//队列总数
	this.filesMap = new Object(); //本地文件列表映射表,id,obj-json
	this.filesCmp = new Array();//已完成列表
	this.filesUrl = new Array();
    this.queueWait = new Array(); //等待队列，数据:id1,id2,id3
    this.queueWork = new Array(); //正在上传的队列，数据:id1,id2,id3
    this.spliter = null;
	this.pnlFiles = null;//文件上传列表面板
	this.parter = null;
	this.btnSetup = null;//安装控件的按钮
	this.working = false;
    this.allStoped = false;//

	this.getHtml = function()
	{ 
	    //自动安装CAB
	    var html = "";
		/*
			IE静态加载代码：
			<object id="objDownloader" classid="clsid:E94D2BA0-37F4-4978-B9B9-A4F548300E48" codebase="http://www.qq.com/HttpDownloader.cab#version=1,2,22,65068" width="1" height="1" ></object>
			<object id="objPartition" classid="clsid:6528602B-7DF7-445A-8BA0-F6F996472569" codebase="http://www.qq.com/HttpDownloader.cab#version=1,2,22,65068" width="1" height="1" ></object>
		*/
        html += '<object name="parter" classid="clsid:' + this.Config.ie.part.clsid + '"';
        html += ' codebase="' + this.Config.ie.part.path + '#version=' + _this.Config["Version"] + '" width="1" height="1" ></object>';
        html += '<embed name="ffParter" type="' + this.Config.firefox.type + '" pluginspage="' + this.Config.firefox.path + '" width="1" height="1"/>';
	    
		//acx += '</div>';
	    //上传列表项模板
	    html += '<div class="file-item file-item-single" name="fileItem">\
                    <div class="img-box"><img name="fileImg" src="js/file.png"/><img class="hide" name="fdImg" src="js/folder.png"/></div>\
					<div class="area-l">\
						<div name="fileName" class="name">HttpUploader程序开发.pdf</div>\
						<div name="percent" class="percent">(35%)</div>\
						<div name="fileSize" class="size" child="1">1000.23MB</div>\
						<div class="process-border"><div name="process" class="process"></div></div>\
						<div name="msg" class="msg top-space">15.3MB 20KB/S 10:02:00</div>\
					</div>\
					<div class="area-r">\
                        <a class="btn-box hide" name="down" title="继续"><div>继续</div></a>\
						<a class="btn-box hide" name="stop" title="停止"><div>停止</div></a>\
                        <a class="btn-box" name="cancel" title="取消">取消</a>\
						<a class="btn-box hide" name="del" title="删除"><div>删除</div></a>\
						<span tp="btn-item" class="btn-box hide" name="open" title="打开"><div>打开</div></span>\
						<span tp="btn-item" class="btn-box hide" name="open-fd" title="文件夹"><div>文件夹</div></span>\
					</div>\
				</div>';
		//分隔线
	    html += '<div class="file-line" name="spliter"></div>';
		//上传列表
	    html += '<div class="files-panel" name="down_panel">\
                    <div class="header" name="down_header">下载文件</div>\
					<div name="down_toolbar" class="toolbar">\
						<a class="btn" name="btnSetFolder"><div>设置下载目录</div></a>\
						<a href="javascript:void(0)" class="btn" name="btnStart">全部下载</a>\
						<a href="javascript:void(0)" class="btn" name="btnStop">全部停止</a>\
						<a href="javascript:void(0)" class="btn hide" name="btnSetup">安装控件</a>\
					</div>\
					<div class="content" name="down_content">\
						<div name="down_body" class="file-post-view"></div>\
					</div>\
					<div class="footer" name="down_footer">\
						<a href="javascript:void(0)" class="btn-footer" name="btnClear">清除已完成文件</a>\
					</div>\
				</div>';
	    return html;
	};

    this.to_params = function (param, key) {
        var paramStr = "";
        if (param instanceof String || param instanceof Number || param instanceof Boolean) {
            paramStr += "&" + key + "=" + encodeURIComponent(param);
        } else {
            $.each(param, function (i) {
                var k = key == null ? i : key + (param instanceof Array ? "[" + i + "]" : "." + i);
                paramStr += '&' + _this.to_params(this, k);
            });
        }
        return paramStr.substr(1);
    };
	this.set_config = function (v) { jQuery.extend(this.Config, v); };
	this.clearComplete = function ()
	{
	    $.each(this.filesCmp, function (i,n)
	    {
	        n.remove();
	    });
	    this.filesCmp.length = 0;
	};
	this.add_ui = function (fileSvr)
	{
	    //存在相同项
	    if (this.exist_url(fileSvr.id)) { alert("已存在相同项"); return null; };
        this.filesUrl.push(fileSvr.id);

	    var ui = this.tmpFile.clone();
	    var sp = this.spliter.clone();
	    ui.css("display", "block");
	    sp.css("display", "block");
	    this.pnlFiles.append(ui);
	    this.pnlFiles.append(sp);

	    var uiIcoF    = ui.find("img[name='fileImg']")
	    var uiIcoFD   = ui.find("img[name='fdImg']")
	    var uiName    = ui.find("div[name='fileName']")
	    var uiSize    = ui.find("div[name='fileSize']");
	    var uiProcess = ui.find("div[name='process']");
	    var uiPercent = ui.find("div[name='percent']");
	    var uiMsg     = ui.find("div[name='msg']");
	    var btnCancel = ui.find("a[name='cancel']");
	    var btnStop   = ui.find("a[name='stop']");
	    var btnDown   = ui.find("a[name='down']");
	    var btnDel    = ui.find("a[name='del']");
        var btnOpen   = ui.find("span[name='open']");
        var btnOpenFd = ui.find("span[name='open-fd']");
        var ui_eles = { ico: { file: uiIcoF, fd: uiIcoFD }, msg: uiMsg, name: uiName, size: uiSize, process: uiProcess, percent: uiPercent, btn: { cancel: btnCancel, stop: btnStop, down: btnDown, del: btnDel, open: btnOpen, openFd: btnOpenFd }, div: ui, split: sp };

	    var downer;
        if (fileSvr.fdTask) { downer = new FdDownloader(fileSvr, this); }
        else { downer = new FileDownloader(fileSvr,this);}
	    this.filesMap[fileSvr.id] = downer;//
	    jQuery.extend(downer.ui, ui_eles);

        uiName.text(fileSvr.nameLoc);
        uiName.attr("title", fileSvr.fileUrl);
	    uiMsg.text("");
	    uiSize.text(fileSvr.sizeSvr);
        uiPercent.text("(" + fileSvr.perLoc + ")");
        uiProcess.width(fileSvr.perLoc);
	    //btnDel.click(function () { downer.remove(); });
	    //btnStop.click(function () { downer.stop(); });
	    //btnDown.click(function () { downer.down(); });
	    //btnCancel.click(function () { downer.remove(); });

	    downer.ready(); //准备
        setTimeout(function () { _this.down_next(); },500);
    };
	this.resume_file = function (fSvr)
    {
        var f = jQuery.extend({}, fSvr, { svrInit: true });
        this.add_ui(f);
    };
    this.init_file = function (f) {
        this.app.initFile(f);
    };
    this.init_folder = function (f) {
        this.app.initFolder(jQuery.extend({}, this.Config, f));
    };
    this.init_file_cmp = function (json) {
        var p = this.filesMap[json.id];
        p.init_complete(json);
    };
	this.add_file = function (f)
	{
        var obj = this.add_ui(f);
	};
    this.add_folder = function (f)
	{
        var obj = this.add_ui(f);
	};
	this.exist_url = function (url)
	{
	    var v = false;
	    for (var i = 0, l = this.filesUrl.length; i < l; ++i)
	    {
	        v = this.filesUrl[i] == url;
	        if (v) break;
	    }
	    return v;
	};
	this.remove_url = function (url) { this.filesUrl.remove(url); };
	this.open_folder = function (json)
	{
	    this.app.openFolder();
	};
	this.down_file = function (json) { };
    //队列控制
    this.work_full = function () { return (this.queueWork.length + 1) > this.Config.ThreadCount; };
    this.add_wait = function (id) { this.queueWait.push(id); };
    this.del_wait = function (id) {
        if (_this.queueWait.length < 1) return;
        this.queueWait.remove(id);
    };
    this.add_work = function (id) { this.queueWork.push(id); };
    this.del_work = function (id) {
        if (_this.queueWork.length < 1) return;
        this.queueWork.remove(id);
    };
    this.down_next = function () {
        if (_this.allStoped) return;
        if (_this.work_full()) return;
        if (_this.queueWait.length < 1) return;
        var f_id = _this.queueWait.shift();
        var f = _this.filesMap[f_id];
        _this.add_work(f_id);
        f.down();
    };

    this.init_end = function (json)
	{
	    var p = this.filesMap[json.id];
	    p.init_end(json);
	};
	this.add_end = function (json)
	{
	    var p = this.filesMap[json.id];
	    p.add_end(json);
	};
	this.down_begin = function (json)
	{
        var p = this.filesMap[json.id];
	    p.down_begin(json);
	};
	this.down_process = function (json)
	{
        var p = this.filesMap[json.id];
	    p.down_process(json);
	};
	this.down_part = function (json)
	{
        var p = this.filesMap[json.id];
	    p.down_part(json);
	};
	this.down_error = function (json)
	{
        var p = this.filesMap[json.id];
	    p.down_error(json);
    };
    this.down_open_folder = function (json) {
        //用户选择的路径
        //json.path
        this.Config["Folder"] = json.path;
    };
	this.down_recv_size = function (json)
	{
        var p = this.filesMap[json.id];
	    p.down_recv_size(json);
	};
	this.down_recv_name = function (json)
	{
        var p = this.filesMap[json.id];
	    p.down_recv_name(json);
	};
	this.down_complete = function (json)
	{
        var p = this.filesMap[json.id];
	    p.down_complete(json);
	};
	this.down_stoped = function (json)
	{
        var p = this.filesMap[json.id];
	    p.down_stoped(json);
	};
	this.start_queue = function () { this.app.startQueue();};
	this.stop_queue = function (json)
	{
	    this.app.stopQueue();
	};
	this.queue_begin = function (json) { this.working = true;};
	this.queue_end = function (json) { this.working = false;};
    this.load_complete = function (json)
    {
        this.btnSetup.hide();
        var needUpdate = true;
        if (typeof (json.version) != "undefined") {
            if (json.version == this.Config.Version) {
                needUpdate = false;
            }
        }
        if (needUpdate) this.update_notice();
        else { this.btnSetup.hide(); }
    };
    this.load_complete_edge = function (json) {
        this.edge_load = true;
        this.btnSetup.hide();
        _this.app.init();
    };
	this.recvMessage = function (str)
	{
	    var json = JSON.parse(str);
	         if (json.name == "open_files") { _this.open_files(json); }
	    else if (json.name == "init_file_cmp") { _this.init_file_cmp(json); }
	    else if (json.name == "open_folder") { _this.down_open_folder(json); }
	    else if (json.name == "down_recv_size") { _this.down_recv_size(json); }
	    else if (json.name == "down_recv_name") { _this.down_recv_name(json); }
	    else if (json.name == "init_end") { _this.init_end(json); }
	    else if (json.name == "add_file") { _this.add_file(json); }
	    else if (json.name == "add_folder") { _this.add_folder(json); }
	    else if (json.name == "add_end") { _this.add_end(json); }
	    else if (json.name == "down_begin") { _this.down_begin(json); }
	    else if (json.name == "down_process") { _this.down_process(json); }
	    else if (json.name == "down_part") { _this.down_part(json); }
	    else if (json.name == "down_error") { _this.down_error(json); }
	    else if (json.name == "down_complete") { _this.down_complete(json); }
	    else if (json.name == "down_stoped") { _this.down_stoped(json); }
	    else if (json.name == "queue_complete") { _this.event.queueComplete(); }
	    else if (json.name == "queue_begin") { _this.queue_begin(json); }
	    else if (json.name == "queue_end") { _this.queue_end(json); }
        else if (json.name == "load_complete") { _this.load_complete(json); }
	    else if (json.name == "load_complete_edge") { _this.load_complete_edge(json); }
        else if (json.name == "extension_complete") { 
            setTimeout(function () {
                var param = { name: "init", config: _this.Config };
                _this.app.postMessage(param);
            }, 1000);
        }
	};

	this.checkVersion = function ()
	{
	    //Win64
	    if (window.navigator.platform == "Win64")
	    {
	        jQuery.extend(this.Config.ie, this.Config.ie64);
	    }
	    else if (this.firefox)
        {
            if (!this.app.checkFF())//仍然支持npapi
            {
                this.edge = true;
                this.app.postMessage = this.app.postMessageEdge;
                this.edgeApp.run = this.edgeApp.runChr;
            }
	    }
	    else if (this.chrome)
	    {
	        this.app.check = this.app.checkFF;
	        jQuery.extend(this.Config.firefox, this.Config.chrome);
	        //44+版本使用Native Message
	        if (parseInt(this.chrVer[1]) >= 44)
	        {
	            _this.firefox = true;
	            if (!this.app.checkFF())//仍然支持npapi
                {
                    this.edge = true;
                    this.app.postMessage = this.app.postMessageEdge;
                    this.edgeApp.run = this.edgeApp.runChr;
	            }
	        }
        }
        else if (this.edge) {
            this.app.postMessage = this.app.postMessageEdge;
        }
	};
	this.checkVersion();

    //升级通知
    this.update_notice = function () {
        this.btnSetup.text("升级控件");
        this.btnSetup.css("color", "red");
        this.btnSetup.show();
    };

	//安全检查，在用户关闭网页时自动停止所有上传任务。
	this.safeCheck = function()
	{
	    $(window).bind("beforeunload", function (event)
	    {
	        if (_this.working)
	        {
	            event.returnValue = "您还有程序正在运行，确定关闭？";
	        }
	    });

		$(window).bind("unload", function()
		{ 
			if (_this.working)
			{
			    _this.stop_queue();
			}
		});
	};
	
	this.loadAuto = function()
	{
	    var html = this.getHtml();
	    var ui = $(document.body).append(html);
	    this.initUI(ui);
	    this.loadFiles();
	};
	//加截到指定dom
	this.loadTo = function(id)
	{
	    var obj = $("#" + id);
	    var html = this.getHtml();
	    var ui = obj.append(html);
	    this.initUI(ui);
	    this.loadFiles();
	};
	this.initUI = function (ui/*jquery obj*/)
	{
	    this.down_panel = ui.find('div[name="down_panel"]');
	    this.btnSetup = ui.find('a[name="btnSetup"]');
        this.tmpFile  = ui.find('div[name="fileItem"]');
        this.parter   = ui.find('embed[name="ffParter"]').get(0);
        this.ieParter = ui.find('object[name="parter"]').get(0);
	    var down_body = ui.find("div[name='down_body']");
	    var down_head = ui.find('div[name="down_header"]');
	    var post_bar  = ui.find('div[name="down_toolbar"]');
	    var post_foot = ui.find('div[name="down_footer"]');
	    down_body.height(this.down_panel.height() - post_bar.height() - down_head.height() - post_foot.outerHeight() - 1);

	    var btnSetFolder = ui.find('a[name="btnSetFolder"]');
	    this.spliter = ui.find('div[name="spliter"]');
	    this.pnlFiles = down_body;
        	    
	    //设置下载文件夹
	    btnSetFolder.click(function () { _this.open_folder(); });
		//清除已完成
		ui.find('a[name="btnClear"]').click(function(){_this.clearComplete();});
		ui.find('a[name="btnStart"]').click(function () { _this.start_queue(); });
		ui.find('a[name="btnStop"]').click(function () { _this.stop_queue(); });

	    //this.LoadData();
        this.safeCheck();//

        $(function () {
            if (!_this.edge) {
                if (_this.ie) {
                    _this.parter = _this.ieParter;
                }
                _this.parter.recvMessage = _this.recvMessage;
            }

            if (_this.edge) {
                _this.edgeApp.run();
            }
            else {
                _this.app.init();
            }
        });
	};

    //加载未未完成列表
	this.loadFiles = function ()
	{
	    var param = jQuery.extend({}, this.Config.Fields, { time: new Date().getTime()});
	    $.ajax({
	        type: "GET"
            , dataType: 'jsonp'
            , jsonp: "callback" //自定义的jsonp回调函数名称，默认为jQuery自动生成的随机函数名
            , url: _this.Config["UrlList"]
            , data: param
            , success: function (msg)
            {
                if (msg.value == null) return;
                var files = JSON.parse(decodeURIComponent(msg.value));

                for (var i = 0, l = files.length; i < l; ++i)
                {
                    _this.resume_file(files[i]);
                }
            }
            , error: function (req, txt, err) { alert("加载文件列表失败！" +req.responseText); }
            , complete: function (req, sta) {req = null;}
	    });
	};
}