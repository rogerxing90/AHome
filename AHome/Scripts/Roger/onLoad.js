$(function () {

//在MasterPage.js中定义的
    //获取新闻类别 菜单
    GetNewsType(); 
    //获取工艺知识类别
    GetProductType();
    //检测登录状态
    CheckLoginState()



//注册导航栏鼠标经过时显示菜单事件

    //工艺知识
    var techBtn = $("#technology");
    techBtn.mouseover(function(){
        mopen("tecList");
    });
    techBtn.mouseout(function(){
        mclosetime();
    })

    var tecListDom = document.getElementById("tecList");
    tecListDom.onmouseover = mcancelclosetime;
    tecListDom.onmouseout = mclosetime;

    //作品展览
    var exhiBtn = $("#exhibition");
    exhiBtn.mouseover(function(){
        mopen("exhList");
    });
    exhiBtn.mouseout(function(){
        mclosetime();
    })

    var exhListDom = document.getElementById("exhList");
    exhListDom.onmouseover = mcancelclosetime;
    exhListDom.onmouseout = mclosetime;

    //非遗组品
    var heriBtn = $("#heritage");
    heriBtn.mouseover(function(){
        mopen("herList");
    });
    heriBtn.mouseout(function(){
        mclosetime();
    })

    var herListDom = document.getElementById("herList");
    herListDom.onmouseover = mcancelclosetime;
    herListDom.onmouseout = mclosetime;

    //新闻中心
    var newsCenterBtn = $("#newsCenter");
    newsCenterBtn.mouseover(function () {
        mopen("newsList")
    });

    newsCenterBtn.mouseout(function () {
        mclosetime();
    });

    //var newsList = $("#newsList");
    //newsList.mouseover(function () {
    //    mcancelclosetime();
    //});
    //newsList.mouseout(function () {
    //    mclosetime();
    //});

    var newListDom = document.getElementById("newsList");
    newListDom.onmouseover = mcancelclosetime;
    newListDom.onmouseout = mclosetime;


});