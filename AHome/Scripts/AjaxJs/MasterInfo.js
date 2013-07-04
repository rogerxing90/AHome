
//获取大师基本信息
function GetMasterInfo() {
    $.ajax({
        url: "/Master/GetMasterInfo",
        type: "post",
        data: { "MasterId": id },
        success: function (text) {
            var jsonData = $.parseJSON(text);
            if (jsonData.Status) {
                var item = '<tr><td width="51"> 姓名：</td>';
                item += ' <td width="92">' + jsonData.Data[0].Name + '</td>';
                item += '<td width="47">性别: </td>';
                item += '<td width="42">' + jsonData.Data[0].Sex + '</td>';
                item += ' </tr>';
                item += '<tr> <td> 出生：</td>';
                item += '<td>' + jsonData.Data[0].Birthday + '</td>';
                item += '</tr>';
                item += ' <tr><td colspan="4">艺术家分类：';
                item += jsonData.Data[0].TypeName;
                item += '</td></tr>';

                $("#tbMasterInfo").empty();
                $("#tbMasterInfo").append(item);
                //此处未实现FileManage
                var src = "/Admin/FileManage/GetImg.ashx?method=GetMasterPic&type=medium&fileName=" + jsonData.Data[0].PicturePath;
                $("#imgMaster").attr("src", src);
            }
        }
    });

}
//获取大师简介信息
function GetMasterIntro() {
    $.ajax({
        url: "/Master/GetMasterIntro",
        type: "post",
        data: { "MasterId": id },
        success: function (text) {
            var jsonData = $.parseJSON(text);
            if (jsonData.Status) {
                var item = '<h4>大师简介</h4>';
                item += '<p>' + jsonData.Data[0].Introduction + '</p>';
                $("#MasterContent").empty();
                $("#MasterContent").append(item);
            }
        }
    });
}
//获取大师荣誉信息
function GetMasterReward() {
    $.ajax({
        url: "/Master/GetMasterReward",
        type: "post",
        data: { "MasterId": id },
        success: function (text) {
            var jsonData = $.parseJSON(text);
            var item = '<h4>大师荣誉</h4>';
            if (jsonData.Status) {
                item += '<ul class="gs_reward">';
                item += ' <li>' + jsonData.Data[0].Reward + '</li></ul>';
                item += ' <h4>获奖情况</h4><ul class="gs_cup">';
                $.each(jsonData.Data[0].CertPicList, function (key, value) {
                    var img = "/Admin/FileManage/GetImg.ashx?method=GetMasterCert&type=medium&fileName=" + value.CertPic;
                    item += '<li><span class="c_pic_a">';
                    item += '<img src=' + img + ' alt=' + value.CertName + ' title=' + value.CertName + '/></span><span class="a_title">' + value.CertName + '</span>';
                    item += '</li>';
                });
                item += '</ul>';

            }
            else {
                item += ' <h4>获奖情况</h4>';
            }
            $("#MasterContent").empty();
            $("#MasterContent").append(item);
        }
    });
}
//获取大师产品信息
function GetMasterWork() {
    $.ajax({
        url: "/Master/GetMasterWork",
        type: "post",
        data: { "MasterId": id },
        success: function (text) {
            var jsonData = $.parseJSON(text);
            var item = '';
            if (jsonData.Status) {
                $.each(jsonData.Data, function (key, value) {
                    item += '<h4>' + value.TypeName + '</h4>';
                    item += '<ul class="gs_pic">';
                    $.each(value.Product, function (PKey, PValue) {
                        var img = "/Admin/FileManage/GetImg.ashx?method=GetMainProductPic&type=medium&fileName=" + PValue.Picturepath;
                        item += '<li>';
                        item += '<a href="' + ProductURL + '?ProductId=' + PValue.ProductId + '" class="c_pic_a"><img src="' + img + '" alt="' + PValue.Name + '" title="' + PValue.SimpleName + '"/></a>';
                        item += '<a href="' + ProductURL + '?ProductId=' + PValue.ProductId + '" class="a_title">' + PValue.SimpleName + '<br/><span class="rad2">￥' + PValue.Lsprice + '</span></a> ';
                        item += '</li>';
                    })
                    item += '</ul>';
                    item += '<div style="text-align:right"><a href="' + Master_MoreProductURL + '?MasterId=' + id + '&&TypeId=' + value.TypeId + '">more>></a></div>';

                })

            }
            else {
                item += '没有数据!';
            }
            $("#MasterContent").empty();
            $("#MasterContent").append(item);
        }
    });
}
