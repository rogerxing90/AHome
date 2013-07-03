//获取新闻类别

function GetNewsType() {
	$.ajax({
		url: '/MasterPage/GetNewsTypes',
		type: 'get',
		//dataType: 'json',
		//data: {param1: 'value1'},
		complete: function(xhr, textStatus) {
			//called when complete
		},
		success: function(data, textStatus, xhr) {
			var JsonData = $.parseJSON(data);
			$("#newsList").empty();
			$.each(JsonData, function(key, value) {
				$("#newsList").append('<a href=\"' + "#" + '?TypeId=' + value.Id + '\">' + value.Name + '</a>');
			})
		},
		// success: function(text) {
		//     var JsonData = $.parseJSON(text);
		//     $("#newsList").empty();
		//     $.each(JsonData, function (key, value) {
		//         $("#newsList").append('<a href=\"' + "#" + '?TypeId=' + value.Id + '\">' + value.Name + '</a>');
		//     })
		// },
		error: function(xhr, textStatus, errorThrown) {
			//called when there is an error
		}
	});

}

//获取工艺知识类别

function GetProductType() {
	$.ajax({
		url: '/MasterPage/GetProductTypes',
		type: 'POST',
		// dataType: 'string',
		// data: {param1: 'value1'},
		complete: function(xhr, textStatus) {
			//called when complete
		},
		success: function(data, textStatus, xhr) {
			var craftData = $.parseJSON(data);
			$("#tecList").empty();
			$.each(craftData, function(key, value) {
				$("#tecList").append('<a href=\"' + "#" + '?TypeId=' + value.Id + '\">' + value.Name + '</a>');
			})
		},
		error: function(xhr, textStatus, errorThrown) {
			//called when there is an error
		}
	});

}

