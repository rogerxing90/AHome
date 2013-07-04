//更改该文件默认Line ending  Windows crlf
//提交用户信息

function Comit() {
    //如果验证不通过,就不提交
    if (!$("#form1").validationEngine('validate'))
        return;
    $("#btnUpdate").click(function () {
        $.ajax({
            url: "Data/GetMemberInfo.ashx?method=UpdateUserInfo",
            data: {
                "UserName": $("#txtUserName").val(),
                "QQ": $("#txtQQ").val(),
                "Email": $("#txtEmail").val(),
                "Nation": $("#selNational").val(),
                "Sex": $("#Man").val(),
                "Province": $("#selProvince").val(),
                "City": $("#selCity").val(),
                "Country": $("#selCountry").val(),
                "Address": $("#txtHomeBase").val(),
                "ZipCode": $("#txtZipCode").val(),
                "MobilePhone": $("#txtMobilePhone").val(),
                "TelPhone": $("#txtTelPhone").val()
            },
            type: "post",
            success: function (text) {
                var DataJson = $.parseJSON(text);
                if (DataJson.Status) {
                    alert('保存成功!');
                }
            }
        });
    });
}
//获取民族信息

function GetNation() {
    $.ajax({
        url: "../Admin/CommonLibs/national.txt",
        dataType: "text",
        success: function (text) {
            //不安全的用法
            var DataJson = eval("(" + text + ")");

            var item = '';
            $.each(DataJson, function (key, value) {
                item += '<option value="' + value.id + '">' + value.text + '</option>';
            })
            $("#selNational").empty();
            $("#selNational").append(item);
        }

    });
}
//获取用户信息

function GetUserInfo() {
    $.ajax({
        url: "/Member/GetUserInfo",
        type: "post",
        success: function (text) {
            var DataJson = $.parseJSON(text);
            if (DataJson.Status) {
                $("#txtUserName").val(DataJson.Data[0].UserName);

                $("#selNational").val(DataJson.Data[0].Nation);
                $("#txtQQ").val(DataJson.Data[0].QQ);
                $("#txtEmail").val(DataJson.Data[0].Email);
                if (DataJson.Data[0].Sex == '1')
                    $("#Man").val(DataJson.Data[0].Sex);
                else
                    $("#Woman").val('1');
                //给省市县赋值
                var Province = DataJson.Data[0].Province;
                if (Province != '')
                    $("#selProvince").val(DataJson.Data[0].Province);
                var City = DataJson.Data[0].City;
                if (City != '')
                    GetArea($("#selProvince").val(), "#selCity", City);
                var Country = DataJson.Data[0].Country;
                if (Country != '')
                    GetArea(DataJson.Data[0].City, "#selCountry", Country);
                $("#txtHomeBase").val(DataJson.Data[0].Address);
                $("#txtZipCode").val(DataJson.Data[0].ZipCode);
                $("#txtMobilePhone").val(DataJson.Data[0].MobilePhone);
                $("#txtTelPhone").val(DataJson.Data[0].TelePhone);

            }

        }
    });

}
//更改邮箱事件

function ChangeEmail() {

    $("#txtEmail").attr("disabled", false);

}
//获取省级数据

function GetProvince() {
    //首先省市数据初始化(获取省级CodeId=0
    $("#selProvince").empty();
    //  $("#selProvince").append("<option value='-1'>请选择</option>");
    GetArea(0, "#selProvince");
    //清空市级数据
    $("#selCity").empty();
    //     $("#selCity").append("<option value='-1'>请选择</option>");
    //清空县级数据
    $("#selCountry").empty();
    //    $("#selCountry").append("<option value='-1'>请选择</option>");

}
//联动市级

function GetCity() {
    if ($("#selProvince").val() == 'null') {
        return;
    } else {
        $("#selCity").empty();
        // $("#selCity").append("<option value='-1'>请选择</option>");
        GetArea($("#selProvince").val(), "#selCity", '');

    }
}
//联动县级

function GetCountry() {
    if ($("#selCity").val() == 'null') {
        return;
    } else {
        $("#selCountry").empty();
        //  $("#selCountry").append("<option value='-1'>请选择</option>");
        GetArea($("#selCity").val(), "#selCountry", '');

    }
}
//联动获取json数据

function GetArea(CodeId, e, InitData) {
    $.ajax({
        url: "/Member/GetArea",
        type: "post",
        data: {
            "CodeId": CodeId,
        },
        success: function (text) {
            var DataJson = $.parseJSON(text);
            var item = '';
            if (DataJson.Status) {
                $.each(DataJson.Data, function (key, value) {
                    item += '<option value="' + value.CodeId + '">' + value.CityName + '</option>';
                });

            }

            $(e).append(item);
            //第一次进MemberInfo页面，赋初值，省：北京，对应市、县也要初始化
            if (InitData != '') {
                $(e).val(InitData);
                if (e == "#selProvince") {
                    GetCity();
                }
            }
            //使得每次省变化时对应的市被动变化，此时县也跟着变化
            if (e == "#selCity" && $("#selCity").val() != null) {
                GetCountry();
            }

        }
    });
}