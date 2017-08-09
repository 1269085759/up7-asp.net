//错误类型
var DownloadErrorCode = {
      "0": "发送数据错误"
	, "1": "接收数据错误"
	, "2": "访问本地文件错误"
	, "3": "域名未授权"
	, "4": "文件大小超过限制"
	, "5": "地址为空"
};
//状态
var HttpDownloaderState = {
    Ready: 0,
    Posting: 1,
    Stop: 2,
    Error: 3,
    GetNewID: 4,
    Complete: 5,
    WaitContinueUpload: 6,
    None: 7,
    Waiting: 8
};
//文件下载对象
function FileDownloader(fileLoc, mgr)
{
    var _this = this;
    this.ui = { msg: null, process: null, percent: null, btn: {del:null,cancel:null,down:null,stop:null},div:null,split:null};
    this.app = mgr.app;
    this.Manager = mgr;
    this.Config = mgr.Config;
    this.fields = jQuery.extend({}, mgr.Config.Fields, { nameLoc: encodeURIComponent(fileLoc.nameLoc), sizeSvr: fileLoc.sizeSvr });//每一个对象自带一个fields幅本
    this.State = HttpDownloaderState.None;
    this.svr_inited = false;
    this.event = mgr.event;
    this.fileSvr = {
          id:""//累加，唯一标识
        , f_id: ""
        , uid: 0
        , nameLoc: ""//自定义文件名称
        , folderLoc: this.Config["Folder"]
        , pathLoc: ""
        , fileUrl:this.Config["UrlDown"]
        , lenLoc: 0
        , perLoc: "0%"
        , lenSvr: 0
        , sizeSvr:"0byte"
        , complete: false
        , fdTask: false
    };
    jQuery.extend(this.fileSvr, fileLoc);//覆盖配置
    jQuery.extend(this.fileSvr, { fields: this.fields });//附加字段
    var url = this.Config["UrlDown"] + "?" + this.Manager.to_params(this.fields);
    jQuery.extend(this.fileSvr, fileLoc, { fileUrl: url });//覆盖配置

    this.hideBtns = function ()
    {
        $.each(this.ui.btn, function (i, n)
        {
            $(n).hide();
        });
    };

    //方法-准备
    this.ready = function ()
    {
        this.hideBtns();
        this.ui.btn.down.show();
        this.ui.btn.cancel.show();
        this.ui.msg.text("正在下载队列中等待...");
        this.State = HttpDownloaderState.Ready;
    };
    //自定义配置,
    this.reset_fields = function (v) {
        if (v == null) return;
        jQuery.extend(this.fields, v);
        //单独拼接url
        var url = this.Config["UrlDown"] + "?" + this.Manager.to_params(this.fields);
        jQuery.extend(this.fileSvr, { fileUrl: url });//覆盖配置
    };

    this.addQueue = function ()
    {
        this.app.addFile(this.fileSvr);
    };
    
    this.add_end = function(json)
    {
    	//续传不初始化
    	if(this.svr_inited) return;
    	this.fileSvr.pathLoc = json.pathLoc;    	
    	this.svr_create();//
    };

    //方法-开始下载
    this.down = function ()
    {
        //续传
        this.hideBtns();
        this.ui.btn.stop.show();
        this.ui.msg.text("开始连接服务器...");
        this.State = HttpDownloaderState.Posting;
        this.app.addFile(this.fileSvr);
        this.Manager.start_queue();//下载队列
    };

    //方法-停止传输
    this.stop = function ()
    {
        this.hideBtns();
        this.ui.btn.down.show();
        //this.SvrUpdate();
        this.State = HttpDownloaderState.Stop;
        this.ui.msg.text("下载已停止");
        this.app.stopFile(this.fileSvr);
    };

    this.remove = function ()
    {
        this.app.stopFile(this.fileSvr);
        //从上传列表中删除
        this.ui.split.remove();
        this.ui.div.remove();
        this.Manager.remove_url(this.fileSvr.fileUrl);
        this.svr_delete();
    };

    this.open = function ()
    {
        this.app.openFile(this.fileSvr);
    };

    this.openPath = function ()
    {
        this.app.openPath(this.fileSvr);
    };
    this.init_complete = function (json) {
        jQuery.extend(this.fileSvr, json);
        if (!this.svr_inited) this.svr_create();//
    };

    //在出错，停止中调用
    this.svr_update = function ()
    {
        var param = jQuery.extend({}, this.fields, { time: new Date().getTime() });
        jQuery.extend(param, { signSvr: this.fileSvr.signSvr });
        jQuery.extend(param, { lenLoc: this.fileSvr.lenLoc});
        jQuery.extend(param, { sizeLoc: this.fileSvr.sizeLoc });
        jQuery.extend(param, { perLoc: encodeURIComponent(this.fileSvr.perLoc) });
        $.ajax({
            type: "GET"
            , dataType: 'jsonp'
            , jsonp: "callback" //自定义的jsonp回调函数名称，默认为jQuery自动生成的随机函数名
            , url: _this.Config["UrlUpdate"]
            , data: param
            , success: function (msg) { }
            , error: function (req, txt, err) { alert("更新下载信息失败！" + req.responseText); }
            , complete: function (req, sta) { req = null; }
        });
    };

    //在服务端创建一个数据，用于记录下载信息，一般在HttpDownloader_BeginDown中调用
    this.svr_create = function ()
    {
        var param = jQuery.extend({}, this.fields, this.fileSvr, { time: new Date().getTime() });
        jQuery.extend(param, {pathLoc:encodeURIComponent(this.fileSvr.pathLoc),nameLoc:encodeURIComponent(this.fileSvr.nameLoc)});

        $.ajax({
            type: "GET"
            , dataType: 'jsonp'
            , jsonp: "callback" //自定义的jsonp回调函数名称，默认为jQuery自动生成的随机函数名
            , url: _this.Config["UrlCreate"]
            , data: param
            , success: function (msg)
            {
                if (msg.value == null) return;
                var json = JSON.parse(decodeURIComponent(msg.value));
                _this.svr_inited = true;
                _this.svr_create_cmp();
            }
            , error: function (req, txt, err) { alert("创建信息失败！" + req.responseText); }
            , complete: function (req, sta) { req = null; }
        });
    };

    this.svr_create_cmp = function () {
        setTimeout(function () {
            _this.down();
        }, 200);
    };

    this.isComplete = function () { return this.State == HttpDownloaderState.Complete; };
    this.svr_delete = function ()
    {
        var param = jQuery.extend({}, { uid: this.fileSvr.uid,id:this.fileSvr.id,time: new Date().getTime() });
        $.ajax({
            type: "GET"
            , dataType: 'jsonp'
            , jsonp: "callback" //自定义的jsonp回调函数名称，默认为jQuery自动生成的随机函数名
            , url: _this.Config["UrlDel"]
            , data: param
            , success: function (json){}
            , error: function (req, txt, err) { alert("删除数据错误！" + req.responseText); }
            , complete: function (req, sta) { req = null; }
        });
    };

    this.down_complete = function ()
    {
        this.hideBtns();
        this.event.downComplete(this);//biz event
        //this.ui.btn.del.text("打开");
        this.ui.process.css("width", "100%");
        this.ui.percent.text("(100%)");
        this.ui.msg.text("下载完成");
        this.State = HttpDownloaderState.Complete;
        //this.SvrDelete();
        this.Manager.filesCmp.push(this);

        this.svr_delete();
    };

    this.down_process = function (json)
    {
        this.fileSvr.lenLoc = json.lenLoc;//保存进度
        this.fileSvr.perLoc = json.percent;
        this.ui.percent.text("("+json.percent+")");
        this.ui.process.css("width", json.percent);
        var msg = [json.sizeLoc , " ", json.speed, " ", json.time];
        this.ui.msg.text(msg.join(""));
    };

    this.down_error = function (json)
    {
        this.hideBtns();
        this.ui.btn.down.show();
        this.ui.btn.del.show();
        this.event.downError(this, json.code);//biz event
        this.ui.msg.text(DownloadErrorCode[json.code+""]);
        this.State = HttpDownloaderState.Stop;
        //this.SvrUpdate();
    };

    this.down_stoped = function (json)
    {
        this.hideBtns();
        this.ui.btn.down.show();
        this.ui.btn.del.show();
        this.svr_update();
    };
}