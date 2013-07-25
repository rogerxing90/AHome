using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Common
{
    public class RogerLogger
    {
        private static ILog logger;

        /// <summary>
        /// 根据Type获取logger实例。
        /// <example>
        /// AveLogger mLog = AveLogger.GetInstance(MethodBase.GetCurrentMethod().DeclaringType);
        /// </example>
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>logger实例</returns>
        public static ILog GetInstance(Type type)
        {
            logger = LogManager.GetLogger(type);
            return logger;
        }


    }
}
