function changeImg() {
    $("#img").attr("src", "../Admin/FileManage/VerifyChars.ashx?date=" + new Date());
}
function CheckUserName() {
    //当鼠标失去焦点的时候验证
    $("#txtUserName").blur(function () {
        if ($("#txtUserName").val() == '') {
            return;
        }
        $.ajax({
            url: "/Master/CheckExistUserName",
            data: { "username": $("#txtUserName").val() },
            type: "post",
            success: function (text) {
                $("#ShowExist").empty();//清空内容
                var item;
                if (text == "True") {
                    item = '<img src="../images/ok.png"/>这个帐号可以注册!';
                    IsCheck = true;
                }
                else
                    item = '<img src="../images/no.png"/>这个帐号已经有人注册了!';
                $("#ShowExist").append(item);

            }
        });

    });

}
function CheckForm1() {
    //还未添加上传图片功能
    //if ($("#Picturepath").val() == '') {
    //    alert("请上传头像!");
    //}
    //else {
        var Sex;
        if ($("#Man").attr("checked"))
            Sex = '1';
        else
            Sex = '0';
        if (IsCheck) {
            var Data = { "UserName": $("#txtUserName").val(), "Pwd": $("#txtPwd").val(), "Birthday": $("#txtBirthday").val(), "Name": $("#txtName").val(), "Sex": Sex, "MobilePhone": $("#txtMobilePhone").val(), "TelePhone": $("#txtTelePhone").val(), "Email": $("#txtEmail").val(), "QQ": $("#txtQQ").val(), "Introduce": $("#txtIntroduce").val(), "CheckCode": $("#txtCheckCode").val(), "Picturepath": $("#Picturepath").val() };
            $.ajax({
                url: "/Member/SaveMasterInfo",
                type: "post",
                data: Data,
                success: function (ReturnData) {
                    var jsonInfo = $.parseJSON(ReturnData);
                    if (jsonInfo.Status) {
                        alert("用户注册成功,请在一个小时内前往" + jsonInfo.Data + "激活邮箱!");
                        window.location.href = DefaultURL;
                    }
                    else {
                        alert("注册失败!" + jsonInfo.Data);
                    }

                }

            });
        }
        else {
            alert("请验证用户名");
        }
    //}
}
//图片上传
function onUpload() {

    $(form1).ajaxSubmit({
        url: '../Admin/FileManage/FileUpload.ashx?method=UpLoadMasterPic',
        success: function (r) {
            var Json = eval("(" + r + ")");//(转义)解析json格式
            if (Json.status == "success") {
                var src = "../Admin/FileManage/GetImg.ashx?method=GetMasterPic&type=small&fileName=" + Json.website;
                $("#Picturepath").val(Json.website);//隐藏域设置图片的文件信息 

                $('#imgpic').attr("src", src);//回调显示图片
                $('input:file').MultiFile('reset');//一定要重置

            }
            else {
                alert("图片上传失败!");
            }
        }
    });
}

//检测登录状态
function CheckLogin() {
    $.ajax({
        url: '/Master/CheckLoginStatus',
        type: "post",
        success: function (data) {
            var jsonInfo = $.parseJSON(data);
            if (jsonInfo.Status && jsonInfo.UserName != "")
                window.location.href = '/Master.MasterInfo';
        }
    });
}
//登录 
function Login() {

    $("#aLogin").click(function () {
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
        var Data = { "Name": Name, "Pwd": pwd, "cbName": cbName, "cbPwd": cbPwd }
        $.ajax({
            url: "/Master/MasterAjaxLogin",
            type: "post",
            data: Data,
            success: function (ReturnData, status) {
                var jsonInfo = $.parseJSON(ReturnData);
                if (jsonInfo.Status) {
                    window.location.href = '/Master/MasterInfo';
                }
                else {
                    alert("您输入的帐号或密码错误!也有可能您的帐号未邮箱激活!");
                }
            }
        });
    });
}
//修改密码
function ChangePwd() {
    $.ajax({
        url: "Data/MasterZoneInfo.ashx?method=UpdatePwd",
        data: { "txtOldPwd": $("#txtOldPwd").val(), "txtNewPwd": $("#txtNewPwd").val() },
        type: "post",
        success: function (text) {
            var dataJson = $.parseJSON(text);
            if (dataJson.Status == 'True') {
                alert("修改成功!");
            }
            else if (dataJson.Status != 'True' && dataJson.Data == '') {
                alert("未登录!");
                window.location.replace(MasterLoginURL);
            }
            else {
                alert("密码出错!请检查密码!");
            }
        }

    });

}
//邮箱验证
function EmailChecking() {
    $.ajax({
        url: "Data/MasterZoneInfo.ashx?method=ActivationMasterNumber",
        type: "post",
        data: Data,
        success: function (ReturnData) {
            var jsonInfo = $.parseJSON(ReturnData);
            if (jsonInfo.Status) {
                $("#lbStatus").text("激活成功!");
                $("#lbInfo").text("您现在可以进入您自己的个人空间进行管理了!请耐心等待管理员审核您的帐号,在未通过审核之前,您的所有信息无法在网站显示!");
                //window.location.href=DefaultURL;
            }
            else {
                $("#lbStatus").text("激活失败");
                $("#lbInfo").text("激活失败,可能是您未能在规定的时间激活,一般是注册的一个小时之内激活!");

            }
        }

    });

}
//忘记密码
function ForgetSercret() {
    $.ajax({
        url: "Data/MasterZoneInfo.ashx?method=ForgetPwd",
        data: { "UserName": $("#txtUserName").val(), "CheckCode": $("#txtCheckCode").val(), "Email": $("#txtEmail").val() },
        type: "post",
        success: function (text) {
            var dataJson = $.parseJSON(text);
            if (dataJson.Status) {
                alert("发送成功!您的密码已经发送到" + dataJson.UserName + "邮箱中");
                window.location.href = MasterLoginURL;//"../Master/MasterLogin.aspx";
            }
            else {
                alert("找回失败!" + dataJson.UserName);
            }
        }

    });

}