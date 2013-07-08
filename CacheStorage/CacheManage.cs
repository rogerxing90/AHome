using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CacheStorage;
using Common;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

/// <summary>
///CacheManage 的摘要说明
/// </summary>
public class CacheManage
{
    /// <summary>
    /// 缓存路径
    /// </summary>
    public static string DefaultPath = System.Web.HttpContext.Current.Server.MapPath(@"~/Config/CacheConfig.xml");

    #region 通过配置文件读取时间信息(缓存相关)
    /// <summary>
    /// 通过配置文件读取时间信息(缓存相关)
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static TimeSpan GetTimeConfig(string key)
    {

        //读取配置文件缓存信息
        int Day = Convert.ToInt32(XMlHelper.Read(DefaultPath, "/Root/CacheSettings/Cache[@Name='" + key + "']", "Day"));
        int Hour = Convert.ToInt32(XMlHelper.Read(DefaultPath, "/Root/CacheSettings/Cache[@Name='" + key + "']", "Hour"));
        return new TimeSpan(Day, Hour, 0, 0, 0);
    }
    #endregion
    /// <summary>
    /// 更新缓存信息
    /// </summary>
    /// <param name="id">缓存id</param>
    /// <param name="day">天</param>
    /// <param name="hour">小时</param>
    /// <returns></returns>
    public bool UpdateCacheInfo(string id, int day, int hour)
    {
        XDocument xDoc = XDocument.Load(DefaultPath);

        IEnumerable<XElement> CacheList = xDoc.Root.Element("CacheSettings").Descendants("Cache");
        foreach (XElement Cache in CacheList)
        {
            //设置缓存id的day和hour
            if (Cache.Attribute("Id").Value == id)
            {
                Cache.SetAttributeValue("Day", day);
                Cache.SetAttributeValue("Hour", hour);
                break;
            }

        }
        xDoc.Save(DefaultPath);
        return true;
    }
    #region 获取所有缓存信息并返回json格式
    /// <summary>
    /// 获取所有缓存信息并返回json格式
    /// </summary>
    /// <returns></returns>
    public string GetCacheAllInfoForMiniUIJson(int PageIndex, int PageSize)
    {
        //加载XML
        XDocument xDoc = XDocument.Load(DefaultPath);

        IEnumerable<XElement> CacheList = xDoc.Root.Element("CacheSettings").Descendants("Cache");
        // CacheList = CacheList.Skip((PageIndex - 1) * PageSize).Select(PageSize);
        //输出json格式数据
        StringBuilder json = new StringBuilder();
        StringWriter sw = new StringWriter(json);
        using (JsonWriter jsonWriter = new JsonTextWriter(sw))
        {
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("total");
            jsonWriter.WriteValue(CacheList.Count());
            jsonWriter.WritePropertyName("data");
            jsonWriter.WriteStartArray();
            foreach (XElement Elem in CacheList)
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("Id");
                jsonWriter.WriteValue(Elem.Attribute("Id").Value);
                jsonWriter.WritePropertyName("Title");
                jsonWriter.WriteValue(Elem.Attribute("Title").Value);
                jsonWriter.WritePropertyName("Day");
                jsonWriter.WriteValue(Elem.Attribute("Day").Value);
                jsonWriter.WritePropertyName("Hour");
                jsonWriter.WriteValue(Elem.Attribute("Hour").Value);
                jsonWriter.WritePropertyName("Name");
                jsonWriter.WriteValue(Elem.Attribute("Name").Value);
                jsonWriter.WriteEndObject();
            }
            jsonWriter.WriteEndArray();
            jsonWriter.WriteEndObject();

        }
        return json.ToString();
    }
    #endregion
}
