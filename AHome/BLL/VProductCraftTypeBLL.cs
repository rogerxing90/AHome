using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using AHome.DAL;
using AHome.Models;
using Newtonsoft.Json;

namespace AHome.BLL
{
    public class VProductCraftTypeBLL
    {
        private VProductCraftTypeDAL dal = new VProductCraftTypeDAL();

        #region 根据大师id查找大师的产品信息(每种分别显示前8个)
        /// <summary>
        /// 根据大师id查找大师的产品信息(每种分别显示前8个)
        /// </summary>
        /// <param name="MasterId">大师Id</param>
        /// <returns></returns>
        public string GetMasterWorkForJson(string MasterId)
        {
            //查询状态
            bool Status = false;
            //获取大师的产品信息(每种显示前8个)
            List<VProductCraftType> dtListProduct = dal.ListAllByMasterIdToDatable(MasterId);
            //转化为json格式
            StringBuilder json = new StringBuilder();
            StringWriter sw = new StringWriter(json);

            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {

                jsonWriter.Formatting = Formatting.Indented;
                //判断数据读取状态
                if (dtListProduct.Count > 0)
                {
                    Status = true;
                }
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("Status");
                jsonWriter.WriteValue(Status);
                jsonWriter.WritePropertyName("Data");

                jsonWriter.WriteStartArray();
                if (Status == true)
                {
                    //先输出第一个元素的类别信息
                    jsonWriter.WriteStartObject();
                    jsonWriter.WritePropertyName("TypeId");
                    jsonWriter.WriteValue(dtListProduct[0].CraftType.ID.ToString());
                    jsonWriter.WritePropertyName("TypeName");
                    jsonWriter.WriteValue(dtListProduct[0].CraftType.Name.ToString());
                    //第一个元素的开始
                    jsonWriter.WritePropertyName("Product");
                    jsonWriter.WriteStartArray();

                    //按照类别分组
                    //产品计数(一个分组下的产品,从1开始算起)

                    for (int num = 0, numProduct = 1; num < dtListProduct.Count; num++, numProduct++)
                    {

                        //获取该类别下的分组总个数

                        int Total = Convert.ToInt32(dtListProduct[num].Num);
                        //如果该类别下还存在未输出的产品
                        if (numProduct <= Total)
                        {


                            jsonWriter.WriteStartObject();
                            jsonWriter.WritePropertyName("ProductId");
                            jsonWriter.WriteValue(dtListProduct[num].Id.ToString());
                            jsonWriter.WritePropertyName("Name");
                            jsonWriter.WriteValue(dtListProduct[num].Name.ToString());
                            jsonWriter.WritePropertyName("SimpleName");
                            jsonWriter.WriteValue(dtListProduct[num].Simplename.ToString());
                            jsonWriter.WritePropertyName("Lsprice");
                            jsonWriter.WriteValue(dtListProduct[num].Lsprice.ToString());
                            jsonWriter.WritePropertyName("Picturepath");
                            jsonWriter.WriteValue(dtListProduct[num].Picturepath.ToString());
                            jsonWriter.WriteEndObject();

                        }
                        else
                        {
                            //将该类别的产品计数重置为1
                            numProduct = 1;
                            //这里给上一个类别的产品结束标记

                            jsonWriter.WriteEndArray();
                            jsonWriter.WriteEndObject();

                            jsonWriter.WriteStartObject();
                            jsonWriter.WritePropertyName("TypeId");
                            jsonWriter.WriteValue(dtListProduct[num].CraftType.ID.ToString());
                            jsonWriter.WritePropertyName("TypeName");
                            jsonWriter.WriteValue(dtListProduct[num].CraftType.Name.ToString());
                            //如果还存在产品
                            if (num < dtListProduct.Count)
                            {
                                //下一个元素的开始
                                jsonWriter.WritePropertyName("Product");
                                jsonWriter.WriteStartArray();

                            }

                        }
                    }
                }


                jsonWriter.WriteEndArray();
                jsonWriter.WriteEndObject();

            }
            return json.ToString();
        }
        #endregion
    }
}