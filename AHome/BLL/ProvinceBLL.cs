using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using AHome.DAL;
using AHome.Models;
using Newtonsoft.Json;

namespace AHome.BLL
{
    public class ProvinceBLL
    {
        #region 根据父级id获取下级
        /// <summary>
        /// 根据父级id获取下级
        /// </summary>
        /// <param name="id">父级id</param>
        /// <returns></returns>
        public List<Province> GetArea(string id)
        {
            return new ProvinceDAL().GetArea(id);
        }
        #endregion
        #region 根据父级id获取下级
        /// <summary>
        /// 根据父级id获取下级
        /// </summary>
        /// <param name="id">父级id</param>
        /// <returns></returns>
        public string GetAreaByJson(string id)
        {
            //查询状态
            bool Status = false;
            //根据父级id获取下级
            List<Province> list = new ProvinceDAL().GetArea(id);
            //转化为json格式
            StringBuilder json = new StringBuilder();
            StringWriter sw = new StringWriter(json);
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {

                jsonWriter.Formatting = Formatting.Indented;
                //判断数据读取状态
                if (list.Count() > 0)
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
                    foreach (Province dbInfo in list)
                    {
                        jsonWriter.WriteStartObject();
                        jsonWriter.WritePropertyName("CodeId");
                        jsonWriter.WriteValue(dbInfo.Id);
                        jsonWriter.WritePropertyName("CityName");
                        jsonWriter.WriteValue(dbInfo.CityName);
                        jsonWriter.WriteEndObject();
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
