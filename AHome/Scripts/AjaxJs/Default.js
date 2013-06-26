//获取新闻
function GetTopNews(){
         $.ajax({
         url:'/News/Data/GetNewsInfo.ashx?method=GetTopNewsForMain',
         type:"get",
         success:function(text){
        
         var JsonData=$.parseJSON(text);
            if(JsonData.Status){
           var item='';
           $.each(JsonData.Data,function(key,value){   //注意这里
               item+='<li><a href="'+value.ArticleHtmlUrl+'">'+value.Title+'</a><span class="time">['+value.Time+']</span></li>';
                   
             });
              $("#ulNews").empty();
             $("#ulNews").append(item);
           }
         }
         });
}
//获取工艺知识
function GetCraftKnowledge(){

        $.ajax({
         url:'/CraftKnowledge/Data/CraftKnowledgeInfo.ashx?method=GetCraftKnowledgeForMain',
         type:"get",
         success:function(text){
        
         var JsonData=$.parseJSON(text);
            if(JsonData.Status){
           var item='';
           $.each(JsonData.Data,function(key,value){   //注意这里
               item+='<li><a href="'+value.ArticleHtmlUrl+'">'+value.Title+'</a><span class="time">['+value.Time+']</span></li>';
                   
             });
              $("#ulCraftKnowledge").empty();
             $("#ulCraftKnowledge").append(item);
           }
         }
         });
}
//获取大师信息
function GetTopMasterInfo(){
     $.ajax({
         url:'/Master/Data/MasterInfo.ashx?method=GetTopMasterForMain',
         type:"get",
         success:function(text){
        
         var JsonData=$.parseJSON(text);
            if(JsonData.Status){
           var item=' <div class="r_s_title4"><p class="hide">大师展示</p><a href="#"><img src="/images/more.png" /></a></div>';
           $.each(JsonData.Data,function(key,value){   //注意这里
           item+=' <div class="r_s_p m_border">';
           var PicSrc="/Admin/FileManage/GetImg.ashx?method=GetMasterPic&type=small&fileName="+value.PicturePath;
           item+='<div class="gk_pic"><a href="'+MasterInfoURL+'?MasterId='+value.Id+'"><img src="'+PicSrc+'" alt="'+value.Name+'"/></a></div>';    
           item+=' <div class="master_p">';
           item+='  <p><a href="'+MasterInfoURL+'?MasterId='+value.Id+'"><strong>'+value.Name+'</strong></p></a>'; 
            item+='</div>';   
             });
                 item+='</div>';
              $("#divMaster").empty();
             $("#divMaster").append(item);
           }
         }
         });

}
//获得企业信息
function GetTopCompanyInfo(){
   $.ajax({
         url:'/Company/Data/CompanyInfo.ashx?method=GetTopCompanyForMain',
         type:"get",
         success:function(text){
        
         var JsonData=$.parseJSON(text);
            if(JsonData.Status){
           var item='';
           $.each(JsonData.Data,function(key,value){   //注意这里
           item+='<li><a href="'+CompanyInfoURL+'?CompanyId='+value.Id+'">'+value.Name+'</a></li>';
             });
              $("#ulCompany").empty();
             $("#ulCompany").append(item);
           }
         }
         });

}
//获取产品图片信息
function GetTopProduct(){

 $.ajax({
         url:'/Product/Data/GetProductInfo.ashx?method=GetTopProductForMain',
         type:"get",
         success:function(text){
         var JsonData=$.parseJSON(text);
            if(JsonData.Status){
           var item='';
           $.each(JsonData.Data,function(key,value){   //注意这里
             var PicSrc="/Admin/FileManage/GetImg.ashx?method=GetMainProductPic&type=small&fileName="+value.PicturePath;
           item+='<div class="pic"><a href="'+ProductURL+'?ProductId='+value.Id+'" target="_blank" class="pic_a"><img src="'+PicSrc+'" alt="pic1"/></a> </div>';
           // 
             });
              $("#List1").empty();
             $("#List1").append(item);
           }
         }
         });
}