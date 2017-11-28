console.log('Comm.js由ChenYongLi整理');
//自定义Ajax
function _Ajax(url, parms, fuSuccess, fuError, isAsync, isPost, retrueType) {
    /// <summary>Ajax自定义请求</summary>
    /// <param name="url" type="string">请求url</param>
    /// <param name="parms" type="JSON">参数</param>
    /// <param name="fuSuccess" type="Function">请求成功</param>
    /// <param name="fuError" type="Function">请求失败（选填）</param>
    /// <param name="isAsync" type="Bool">是否异步，默认异步（选填）</param>
    /// <param name="isPost" type="Bool">是否POST，默认POST（选填）</param>
    /// <param name="retrueType" type="string">返回数据类型，默认JOSN（选填）</param>
    if (!arguments[0]) { return }
    var obj = new Object();
    obj["isAsync"] = arguments[4] == undefined ? true : arguments[4]; //不填写，默认为true异步请求
    obj["isPost"] = arguments[5] == undefined ? "POST" : (arguments[5] == true) ? "POST" : "GET"; //不填写，默认为Post请求
    obj["retrueType"] = arguments[6] == undefined ? "json" : arguments[6]; //不填写，默认为json异步请求
    var newparms = new Object();
    jQuery.extend(newparms, parms);

    $.ajax({
        type: obj["isPost"],
        async: obj["isAsync"],
        url: url,
        data: newparms,
        dataType: obj["retrueType"],
        success: fuSuccess,
        error: fuError
    });
}

//将时间戳(/Date(1426896000000+0800)\)转化为时间对象
function _ConverTime(val) {
    return eval('new ' + (val.replace(/\//g, '')));
}


//字符串长度
function _fuStrLength(str) {
    /// <summary>计算字符串长度"汉字二个字节"（效率很低）</summary>
    /// <param name="str" type="string">要计长度的字符串</param>
    var len = 0;
    for (var i = 0; i < str.length; i++) {
        if ((str.charCodeAt(i) >= 0) && (str.charCodeAt(i) <= 255)) {
            len = len + 1;
        } else {
            len = len + 2;
        }
    }
    return len;
}


//url参数获取
function _GetUrlParam(name) {
    /// <summary>url参数获取,不存在返回null</summary>
    /// <param name="name" type="string">参数名称</param>
    //构造一个含有目标参数的正则表达式对象  
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    //匹配目标参数  
    var r = window.location.search.substr(1).match(reg);
    //返回参数值  
    if (r != null) return r[2];
    return null;
}


//优雅字符串拼接
function _Substitute(str, obj) {
    /// <summary>优雅字符串拼接</summary>
    /// <param name="str" type="string">格式字符串:你好{title},你是在{where}？</param>
    /// <param name="obj" type="Object">一个对象：obj = { title: "小陈", where: "中国" };</param>
    if (!(Object.prototype.toString.call(str) === '[object String]')) {
        return '';
    }
    if (!(Object.prototype.toString.call(obj) === '[object Object]' && 'isPrototypeOf' in obj)) {
        return str;
    }
    return str.replace(/\{([^{}]+)\}/g, function (match, key) {
        var value = obj[key];
        return (value !== undefined) ? '' + value : '';
    });
}


//模拟form提交
function _Post(url, params) {
    var tempForm = document.createElement("form");
    tempForm.action = url;
    tempForm.method = "post";
    tempForm.style.display = "none";
    for (var x in params) {
        var opt = document.createElement("textarea");
        opt.name = x;
        opt.value = params[x];
        tempForm.appendChild(opt);
    }
    document.body.appendChild(tempForm);
    tempForm.submit();
    return tempForm;
}

