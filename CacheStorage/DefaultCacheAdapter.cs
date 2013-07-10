using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Collections;
namespace CacheStorage
{
    /// <summary>
    /// 默认的asp.net中Cache
    /// </summary>
    class DefaultCacheAdapter : ICacheStorage
    {
        /// <summary>
        /// 当前请求上下文
        /// </summary>
        private static HttpContext context = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        static DefaultCacheAdapter()
        {
            context = HttpContext.Current;
        }
        #region ICacheStorage 成员
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public void Insert(string key, object value)
        {
            context.Cache.Insert(key, value);
        }
        /// <summary>
        /// 添加缓存(默认滑动时间为20分钟)
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiration">绝对过期时间</param>
        public void Insert(string key, object value, DateTime expiration)
        {
            context.Cache.Insert(key, value, null, expiration, TimeSpan.FromMinutes(20), CacheItemPriority.Normal, null);
        }
        /// <summary>
        /// 添加缓存(滑动过期时间)
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiration">滑动过期时间</param>
        public void Insert(string key, object value, TimeSpan expiration)
        {
            context.Cache.Insert(key, value, null, DateTime.MaxValue, expiration, CacheItemPriority.Normal, null);

        }
        /// <summary>
        /// 添加缓存(多参数)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dateTimeExpiration"></param>
        /// <param name="timeSpanExpiration"></param>
        /// <param name="priority"></param>
        public void Insert(string key, object value, DateTime dateTimeExpiration, TimeSpan timeSpanExpiration, CacheItemPriority priority)
        {
            context.Cache.Insert(key, value, null, dateTimeExpiration, timeSpanExpiration, priority, null);
        }
        /// <summary>
        /// 获取当前缓存中key的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            return context.Cache[key];

        }
        /// <summary>
        /// 删除当前key的value值
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            if (Exist(key))
                context.Cache.Remove(key);
        }
        /// <summary>
        /// 缓存是否存在key的value
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public bool Exist(string key)
        {
            if (context.Cache[key] == null)
                return false;
            else
                return true;
        }
        /// <summary>
        /// 获取所有的缓存key
        /// </summary>
        /// <returns></returns>
        public List<string> GetCacheKeys()
        {
            List<string> keys = new List<string>();
            IDictionaryEnumerator ide = context.Cache.GetEnumerator();
            while (ide.MoveNext())
            {
                keys.Add(ide.Key.ToString());
            }
            return keys;
        }
        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Flush()
        {
            foreach (string s in GetCacheKeys())
            {
                Remove(s);
            }
        }
        #endregion
    }
}
