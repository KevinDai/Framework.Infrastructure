using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;


namespace Framework.Infrastructure
{
    /// <summary>
    /// AppSettings配置信息
    /// </summary>
    public static class AppSettings
    {
        public const string ApplicationIdKey = "ApplicationId";
        public const string ApplicationTypeKey = "ApplicationType";
        public const string ApplicationGroupKey = "ApplicationGroup";
        public const string ComponentsPathKey = "ComponentsPath";

        public static string ApplicationId
        {
            get
            {
                return ConfigurationManager.AppSettings[ApplicationIdKey];
            }
        }

        public static string ApplicationType
        {
            get
            {
                return ConfigurationManager.AppSettings[ApplicationTypeKey];
            }
        }

        public static string ApplicationGroup
        {
            get
            {
                return ConfigurationManager.AppSettings[ApplicationGroupKey];
            }
        }

        public static string ComponentsPath
        {
            get
            {
                return ConfigurationManager.AppSettings[ComponentsPathKey];
            }
        }
    }
}
