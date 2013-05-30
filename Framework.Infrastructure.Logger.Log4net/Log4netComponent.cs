using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Infrastructure.Container;
using System.ComponentModel.Composition;

namespace Framework.Infrastructure.Logger.Log4net
{
    [Export(typeof(IComponent))]
    public class Log4netComponent : IComponent
    {
        public string Name
        {
            get
            {
                return "Log4netComponent";
            }
        }

        public void Initialize()
        {
            ServiceFactory.Instance.Container.RegisterType<ILoggerProvider, Log4netLoggerProvider>(LifeTime.Singleton);
        }
    }
}
