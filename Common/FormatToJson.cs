using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Common
{
    public class FormatToJson
    {
        #region ListToJson
        /// <summary>  
        /// List转成json   
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="jsonName"></param>  
        /// <param name="list"></param>  
        /// <returns></returns>  
        public static string ListToJson<T>(IList<T> list, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = list[0].GetType().Name;
            Json.Append("{\"" + jsonName + "\":[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pi.Length; j++)
                    {
                        //注意这里要做非空判断,否则空值会发生异常
                        if (Common.Tools.IsNullOrEmpty(pi[j].GetValue(list[i], null)))
                        {

                            Json.Append("\"" + pi[j].Name.ToString() + "\":" + "\"\"");
                        }
                        else
                        {
                            Type type = pi[j].GetValue(list[i], null).GetType();
                            Json.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(pi[j].GetValue(list[i], null).ToString(), type));
                        }
                        if (j < pi.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }
        /// <summary>  
        /// List转成json (不生成jsonName版本)  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="list"></param>  
        /// <returns></returns>  
        public static string ListToJsonNoName<T>(IList<T> list)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pi.Length; j++)
                    {
                        //注意这里要做非空判断,否则空值会发生异常
                        if (Common.Tools.IsNullOrEmpty(pi[j].GetValue(list[i], null)))
                        {

                            Json.Append("\"" + pi[j].Name.ToString() + "\":" + "\"\"");
                        }
                        else
                        {
                            Type type = pi[j].GetValue(list[i], null).GetType();
                            Json.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(pi[j].GetValue(list[i], null).ToString(), type));
                        }
                        if (j < pi.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]");
            return Json.ToString();

        }

        /// <summary>  
        /// List转成json   
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="list"></param>  
        /// <returns></returns>  
        public static string ListToJson<T>(IList<T> list)
        {
            object obj = list[0];
            return ListToJson<T>(list, obj.GetType().Name);
        }
        #endregion

        /// <summary>  
        /// 过滤特殊字符  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        /// <summary>  
        /// 格式化字符型、日期型、布尔型  
        /// </summary>  
        /// <param name="str"></param>  
        /// <param name="type"></param>  
        /// <returns></returns>  
        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                DateTime dt = DateTime.Parse(str);
                str = "\"" + dt.GetDateTimeFormats('s')[0].ToString() + "\"";
                // str = + str + ;
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            return str;
        }

        /// <summary>
        /// 专门生成为MiniUi生成json数据(List->json)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">实现了Ilist接口的list</param>
        /// <param name="total">记录总数</param>
        /// <param name="paramMaxMinAvg">这里放排序的参数例如,string para="\"maxAge\":37,\"avgAge\":27,\"minAge\":24"</param>
        /// <returns></returns>
        public static string MiniUiListToJson<T>(IList<T> list, int total, string paramMaxMinAvg)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\"total\":" + total + ",");
            if (!string.IsNullOrEmpty(paramMaxMinAvg))
                Json.Append(paramMaxMinAvg);
            Json.Append("\"data\":[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pi.Length; j++)
                    {
                        //注意这里要做非空判断,否则空值会发生异常
                        if (Common.Tools.IsNullOrEmpty(pi[j].GetValue(list[i], null)))
                        {

                            Json.Append("\"" + pi[j].Name.ToString() + "\":" + "\"\"");
                        }
                        else
                        {
                            Type type = pi[j].GetValue(list[i], null).GetType();
                            //List类型的不需要序列化
                            if (!pi[j].PropertyType.FullName.Contains("List"))
                            {
                                Json.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(pi[j].GetValue(list[i], null).ToString(), type));
                            }
                        }

                        //去掉多余, 只算非List类型的数量
                        int count = pi.Where(p => !p.PropertyType.FullName.Contains("List")).Count();
                        if (j < count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();

        }

        /// <summary>
        /// 序列化object类型
        /// </summary>
        /// <param name="obj">object类型</param>
        /// <returns></returns>
        public static string ScriptSerializationToJson(object obj)
        {
            return new JavaScriptSerializer().Serialize(obj);

        }
    }
}
