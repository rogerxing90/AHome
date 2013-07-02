  //检测登录状态

  function CheckLogin() {
    $.ajax({
      url: '/Member/CheckLoginStatus',
      type: "post",
      success: function(data) {
        var jsonInfo = $.parseJSON(data);
        if (jsonInfo.Status && jsonInfo.UserName != "")
          window.location.href = "/Member/MemberInfo";
      }
    });
  }
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
        url: "/Member/MemberAjaxLogin",
        type: "post",
        data: Data,
        success: function(ReturnData, status) {
          var jsonInfo = $.parseJSON(ReturnData);
          if (jsonInfo.Status) {
            //此处操作母版Dom，跳到MemberInfo页面后被重置。
            //$("#liLogin").hide();
            //$("#liLogout").show();
            //$("#lbUserNameInfo").text(jsonInfo.UserName + "的");

            //BUG_1 
            window.location.href = '/Member/MemberInfo';
          } else {
            alert("您输入的帐号或密码错误!也有可能您的帐号未邮箱激活!");
          }

        },
        error: function (ReturnData, status)
        { }

      });

    });

  }