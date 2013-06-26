//   
//function getRootPath(){
//    //获取当前网址，如： http://localhost:8083/uimcardprj/share/meun.jsp
//    var curWwwPath=window.document.location.href;
//    //获取主机地址之后的目录，如： uimcardprj/share/meun.jsp
//    var pathName=window.document.location.pathname;
//    var pos=curWwwPath.indexOf(pathName);
//    //获取主机地址，如： http://localhost:8083
//    var localhostPaht=curWwwPath.substring(0,pos);
//    //获取带"/"的项目名，如：/uimcardprj
//    var projectName=pathName.substring(0,pathName.substr(1).indexOf('/')+1);
//    return(localhostPaht+projectName);
//}
//登录 

function Login() {

    $("#aLogin").click(function() {
        var Name = $("#txtUserName").val();
        var pwd = $("#txtPwd").val();
        var cbName = $("#cbUserName").attr("checked");
        var cbPwd = $("#cbPwd").attr("checked");
        if (Name == "" || pwd == "") {
            alert("用户名或密码不能为空!");
            return;
        }
        if (cbName == "checked")
            cbName = "1";
        else
            cbName = "0";
        if (cbPwd == "checked")
            cbPwd = "1";
        else
            cbPwd = "0";
        var Data = {
            "Name": Name,
            "Pwd": pwd,
            "cbName": cbName,
            "cbPwd": cbPwd
        }
        $.ajax({
            url: "/Member/Data/GetMemberInfo.ashx?method=MemberLogin",
            type: "post",
            data: Data,
            success: function(ReturnData, status) {
                var jsonInfo = $.parseJSON(ReturnData);
                if (jsonInfo.Status) {

                    $("#divNotLogin").hide();
                    $("#divIsLogin").show();
                    $("#liLogin").hide();
                    $("#liLogout").show();
                    $("#lbUserName").text(jsonInfo.UserName);
                    $("#lbUserNameInfo").text(jsonInfo.UserName + "的");
                } else {
                    alert("您输入的帐号或密码错误!也有可能您的帐号未邮箱激活!");
                }

            }

        });

    });

}
//检测登录状态

function CheckLoginState() {
    $.ajax({
        url: '/Member/Data/GetMemberInfo.ashx?method=CheckLoginStatus',
        type: "post",
         success: function(data) {
             var jsonInfo = $.parseJSON(data);
             // alert(data);
             //同时记住账号和密码
             if (jsonInfo.Status && jsonInfo.UserName != "") {
                 $("#divNotLogin").hide();
                 $("#divIsLogin").show();
                 $("#liLogin").hide();
                 $("#liLogout").show();
                 $("#lbUserName").text(jsonInfo.UserName);
                 $("#lbUserNameInfo").text(jsonInfo.UserName + "的");

             }
             //如果只记住账号
             else if (jsonInfo.Status && jsonInfo.UserName == "") {
                 $("#divNotLogin").show();
                 $("#divIsLogin").hide();
                 $("#liLogin").show();
                 $("#liLogout").hide();
                 $("#txtUserName").val(jsonInfo.UserName);

             } else {
                 $("#divNotLogin").show();
                 $("#divIsLogin").hide();
                 $("#liLogin").show();
                 $("#liLogout").hide();
             }

         }
    });
}
//排名信息

function TopRank() {
    $.ajax({
        url: '/Product/Data/GetProductInfo.ashx?method=GetTopByRank',
        type: "get",
        success: function(text) {

            var JsonData = $.parseJSON(text);
            if (JsonData.Status) {
                //大师排名增加
                var item = '';
                item += ' <ul style="display: block;">';
                //            alert(JsonData.Data[0].ProductData[0].Id);
                $.each(JsonData.Data[1].MasterData, function(key, value) { //注意这里

                    item += '<li class="ph_li' + (key + 1) + '"><a href="' + MasterInfoURL + '?MasterId=' + value.Id + '" >' + value.Name + '</a></li>';

                });
                item += '</ul>';
                //工艺品
                item += ' <ul style="display: none;">';
                //            alert(JsonData.Data[0].ProductData[0].Id);
                $.each(JsonData.Data[0].ProductData, function(key, value) { //注意这里

                    item += '<li class="ph_li' + (key + 1) + '"><a href="' + ProductURL + '?ProductId=' + value.Id + '" >' + value.Name + '</a></li>';

                });
                item += '</ul>';
                //企业
                item += ' <ul style="display: none;">';
                $.each(JsonData.Data[2].CompanyData, function(key, value) { //注意这里

                    item += '<li class="ph_li' + (key + 1) + '"><a href="' + CompanyInfoURL + '?CompanyId=' + value.Id + '" >' + value.Name + '</a></li>';

                });
                item += '</ul>';
                $("#ph2").empty();
                $("#ph2").append(item);

            }

        }
    });
}
//获取关于我们的信息

function GetOutInfo() {
    $.ajax({
        url: '/Other/Data/GetOtherInfo.ashx?method=GetOurInfo',
        type: "get",
        success: function(text) {

            var JsonData = $.parseJSON(text);

            var item = '';
            item += '<li class="us_11">QQ:' + JsonData.qq + '&nbsp;&nbsp;&nbsp;&nbsp;<a class="rad2" target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=' + JsonData.qq + '&site=qq&menu=yes">点击在线聊天</a></li><li class="us_12">TEL:' + JsonData.Telephone + '</li><li class="us_13">Mobile:' + JsonData.mobilephone + '</li>';
            $("#ulContact").empty();
            $("#ulContact").append(item);
            var item1 = '';
            item1 += '<li class="us_21">联系地址：' + JsonData.Address + '</li><li class="us_22">邮政编码：' + JsonData.Zipcode + '</li><li>Email:' + JsonData.Email + '</li>';
            $("#ulContactOther").empty();
            $("#ulContactOther").append(item1);



        }
    });
}