using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Event
{
    /// <summary>
    /// 触发事件的范围
    /// </summary>
    public abstract class RaiseScope
    {
    }

    /// <summary>
    /// 触发指定应用Id
    /// </summary>
    public class AppIdRasieScope : RaiseScope
    {

        public string ApplicationId
        {
            get;
            private set;
        }

        public AppIdRasieScope(string applicationId)
        {
            Preconditions.CheckNotBlank(applicationId, "applicationId");

            ApplicationId = applicationId;
        }

        public static AppIdRasieScope Create(string applicationId)
        {
            return new AppIdRasieScope(applicationId);
        }
    }

    /// <summary>
    /// 触发指定类型的应用
    /// </summary>
    public class AppTypeRasieScope : RaiseScope
    {
        public string ApplicationType
        {
            get;
            private set;
        }


        public AppTypeRasieScope(string applicationType)
        {
            Preconditions.CheckNotBlank(applicationType, "applicationType");

            ApplicationType = applicationType;
        }

        public static AppTypeRasieScope Create(string applicationType)
        {
            return new AppTypeRasieScope(applicationType);
        }
    }

    /// <summary>
    /// 触发指定分组的应用
    /// </summary>
    public class AppGroupRasieScope : RaiseScope
    {
        public string ApplicationGroup
        {
            get;
            private set;
        }

        public AppGroupRasieScope(string applicationGroup)
        {
            Preconditions.CheckNotBlank(applicationGroup, "applicationGroup");

            ApplicationGroup = applicationGroup;
        }

        public static AppGroupRasieScope Create(string applicationGroup)
        {
            return new AppGroupRasieScope(applicationGroup);
        }
    }

}
