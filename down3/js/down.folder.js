function FdDownloader(fileLoc, mgr)
{
    var _this = this;
    this.ui = { msg: null, process: null, percent: null, btn: {del:null,cancel:null,down:null,stop:null},div:null,split:null};
    this.app = mgr.app;
    this.Manager = mgr;
    this.Config = mgr.Config;
    this.fields = jQuery.extend({},mgr.Config.Fields);//每一个对象自带一个fields幅本
    this.State = HttpDownloaderState.None;
    this.svr_inited = false;
    this.event = mgr.event;
    this.fileSvr = {
          id:""
        , f_id: ""
        , uid: 0
        , nameLoc: ""//自定义文件名称
        , folderLoc: this.Config["Folder"]
        , pathLoc: ""
        , fileUrl: this.Config["UrlDown"]
        , lenLoc: 0
        , perLoc: "0%"
        , lenSvr: 0
        , sizeSvr:"0byte"
        , complete: false
        , errors: 0
        , success:0
        , fdTask: true
        ,files:null
    };
    jQuery.extend(this.fileSvr, fileLoc);//覆盖配置
    jQuery.extend(this.fileSvr, { fields: this.fields });
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
        this.ui.ico.file.hide();
        this.ui.ico.fd.show();
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
        this.app.addFolder(this.fileSvr);
    };

    //方法-开始下载
    this.down = function ()
    {
        //续传
        this.hideBtns();
        this.ui.btn.stop.show();
        this.ui.msg.text("开始连接服务器...");
        this.State = HttpDownloaderState.Posting;        
        this.app.addFolder(this.fileSvr);
        this.Manager.start_queue();//下载队列
    };

    //方法-停止传输
    this.stop = function ()
    {
        this.hideBtns();
        this.ui.btn.down.show();
        this.ui.btn.del.show();
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
    this.svr_update = function (json)
    {
        var param = jQuery.extend({}, this.fields, this.fileSvr, { time: new Date().getTime() });
        jQuery.extend(param, { perLoc: encodeURIComponent(this.fileSvr.perLoc), sizeLoc: encodeURIComponent(this.fileSvr.sizeLoc) });

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

    //添加记录
    this.svr_create = function ()
    {
        this.ui.btn.down.hide();
        this.ui.msg.text("正在初始化...");
        var param = jQuery.extend({}, this.fields,this.fileSvr, { time: new Date().getTime() });
        jQuery.extend(param, { nameLoc: encodeURIComponent(this.fileSvr.nameLoc) });
        jQuery.extend(param, { pathLoc: encodeURIComponent(this.fileSvr.pathLoc) });
        jQuery.extend(param, { sizeSvr: encodeURIComponent(this.fileSvr.sizeSvr) });

        $.ajax({
            type: "get"
            , dataType: 'jsonp'
            , jsonp: "callback" //自定义的jsonp回调函数名称，默认为jQuery自动生成的随机函数名
            , url: _this.Config["UrlFdCreate"]
            , data: param
            , success: function (msg)
            {
                _this.ui.btn.down.show();
                _this.ui.msg.text("初始化完毕...");
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
        var param = jQuery.extend({}, this.fields,{id:this.fileSvr.id,time:new Date().getTime()});
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

    this.svr_delete_file = function (f_id)
    {
        var param = jQuery.extend({}, this.fields, {id:f_id, time: new Date().getTime() });

        $.ajax({
            type: "GET"
            , dataType: 'jsonp'
            , jsonp: "callback" //自定义的jsonp回调函数名称，默认为jQuery自动生成的随机函数名
            , url: _this.Config["UrlDel"]
            , data: param
            , success: function (json) { }
            , error: function (req, txt, err) { alert("删除数据错误！" + req.responseText); }
            , complete: function (req, sta) { req = null; }
        });
    };

    this.down_complete = function (json)
    {
        //所有文件下载完毕
        if (json.all)
        {
            this.hideBtns();
            this.event.downComplete(this);//biz event
            //this.ui.btn.del.text("打开");
            this.ui.process.css("width", "100%");
            this.ui.percent.text("(100%)");
            this.ui.msg.text("文件数：" + json.fileCount + " 成功：" + json.fileComplete);
            this.State = HttpDownloaderState.Complete;
            //this.SvrDelete();
            this.Manager.filesCmp.push(this);
            this.svr_delete();
        }
        else
        {
            //var f = this.fileSvr.files[json.file.id];
            //f.complete = true;
            //f.lenLoc = f.lenSvr;
            //this.fileSvr.success = json.success;
            //this.svr_delete_file(f.idSvr);
            //this.svr_update(null);//更新文件夹进度
        }
    };

    this.down_process = function (json)
    {
        this.fileSvr.lenLoc = json.lenLoc;//保存进度
        this.fileSvr.sizeLoc = json.sizeLoc;
        this.fileSvr.perLoc = json.percent;
        //更新文件进度
        //this.fileSvr.files[json.file.id];
        //this.fileSvr.files[json.file.id].lenLoc = json.file.lenLoc;
        //this.fileSvr.files[json.file.id].percent = json.file.percent;

        this.ui.percent.text("("+json.percent+")");
        this.ui.process.css("width", json.percent);
        var msg = [json.fileIndex + 1, "/", json.filesCount, " ", json.sizeLoc, " ", json.speed, " ", json.time];
        this.ui.msg.text(msg.join(""));
    };

    //更新服务器进度
    this.down_part = function (json)
    {
    };

    this.init_end = function (json)
    {
        jQuery.extend(true,this.fileSvr, json);
        this.svr_create();//添加记录
    };

    this.down_begin = function (json)
    {
    };

    this.down_error = function (json)
    {
        this.hideBtns();
        this.ui.btn.down.show();
        this.ui.btn.del.show();
        this.event.downError(this, json.code);//biz event
        if (typeof (json.msg) == "string") { this.ui.msg.text(json.msg); }
        else { this.ui.msg.text(DownloadErrorCode[json.code + ""]); }
        this.State = HttpDownloaderState.Stop;
        this.svr_update();
    };

    this.down_stoped = function (json)
    {
        this.hideBtns();
        this.ui.btn.down.show();
        this.ui.btn.del.show();
        this.svr_update();
    };
}