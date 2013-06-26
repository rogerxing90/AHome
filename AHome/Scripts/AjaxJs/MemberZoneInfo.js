 //修改密码功能

 function ChangeSercret() {
   $.ajax({
     url: "Data/GetMemberInfo.ashx?method=UpdatePwd",
     data: {
       "txtOldPwd": $("#txtOldPwd").val(),
       "txtNewPwd": $("#txtNewPwd").val()
     },
     type: "post",
     success: function(text) {
       var dataJson = $.parseJSON(text);
       if (dataJson.Status) {
         alert("修改成功!");
       } else if (!dataJson.Status && dataJson.Data == '') {
         alert("未登录!");
         window.location.replace("MemberLogin.aspx");
       } else {
         alert("修改失败!");
       }
     }

   });

 }
 //邮箱验证

 function EmailChecking() {
   $.ajax({
     url: "Data/GetMemberInfo.ashx?method=ActivationMemberNumber",
     type: "post",
     data: Data,
     success: function(ReturnData) {
       var jsonInfo = $.parseJSON(ReturnData);
       if (jsonInfo.Status) {
         $("#lbStatus").text("激活成功!");
         $("#lbInfo").text("您现在可以进入您自己的个人空间进行管理了!");
         //window.location.href="../Default.aspx";
       } else {
         $("#lbStatus").text("激活失败");
         $("#lbInfo").text("激活失败,可能是您未能在规定的时间激活,一般是注册的一个小时之内激活!");

       }
     }

   });
 }
 //忘记密码

 function ForgetSercret() {
   $.ajax({
     url: "Data/GetMemberInfo.ashx?method=ForgetPwd",
     data: {
       "UserName": $("#txtUserName").val(),
       "CheckCode": $("#txtCheckCode").val(),
       "Email": $("#txtEmail").val()
     },
     type: "post",
     success: function(text) {
       var dataJson = $.parseJSON(text);
       if (dataJson.Status) {
         alert("发送成功!您的密码已经发送到" + dataJson.UserName + "邮箱中");
         window.location.href = "MemberLogin.aspx";
       } else {
         alert("找回失败!" + dataJson.UserName);
       }
     }

   });
 }
 //图片(看不见)

 function changeImg() {
   $("#img").attr("src", "../Admin/FileManage/VerifyChars.ashx?date=" + new Date());
 }


 //RegisterMember页面用户名检验

 function CheckUserName() {
   //当鼠标失去焦点的时候验证
   $("#txtUserName").blur(function() {
     if ($("#txtUserName").val() == '') {
       return;
     }
     $.ajax({
       // url: "/Member/CheckExistUserName",
       url: '/Member/CheckExistUserName',
       data: {
         "username": $("#txtUserName").val()
       },
       type: "post",
       success: function(text) {
         $("#tdUser").empty(); //清空内容
         var item;
         if (text == "True") {
           item = '<img src="../images/ok.png"/>恭喜您,这个帐号可以注册!';
           IsCheck = true;
         } else if(text == ""){
           item = '<img src="../images/no.png"/>对不起，用户名中包含特殊字符!';
         }
         else
           item = '<img src="../images/no.png"/>对不起,这个帐号已经有人注册了!';
         $("#tdUser").append(item);

       },
       error: function(xhr, textStatus, errorThrown) {
      //called when there is an error
    }
     });

   });

 }
 //提交表单

 function CheckForm1() {
   if (IsCheck) {
     var Data = {
       "txtUserName": $("#txtUserName").val(),
       "txtPwd": $("#txtPwd").val(),
       "txtEmail": $("#txtEmail").val(),
       "txtCheckCode": $("#txtCheckCode").val()
     };
     $.ajax({
       url: '/Member/SaveMemberInfo',
       type: "post",
       data: Data,
       success: function(ReturnData) {
         var jsonInfo = $.parseJSON(ReturnData);
         if (jsonInfo.Status) {
           alert("用户注册成功,请在一个小时内前往" + jsonInfo.Email + "激活邮箱!");
           window.location.href = "/Home/Index";
         } else {
           alert("注册失败!");
         }

       }

     });
   } else {
     alert("请验证用户名");
   }
 }