using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Framework.Infrastructure.Container;
using Framework.Infrastructure.Logger;

namespace Framework.Infrastructure
{
    internal class ComponentHost
    {
        [ImportMany]
        protected IEnumerable<IComponent> Components
        {
            get;
            private set;
        }

        protected ILogger Logger
        {
            get;
            private set;
        }

        public ComponentHost(CompositionContainer CompositionContainer, ILogger logger)
        {
            Logger = logger;
            CompositionContainer.ComposeParts(this);

            foreach (var item in Components)
            {
                Logger.Info(string.Format("加载{0}组件", item.Name));
            }
        }

        public void Initialize(IContainer ontainer)
        {
            foreach (var item in Components)
            {
                Logger.Debug(string.Format("{0}OnComponentsInitializing", item.Name));

                item.OnComponentsInitializing(ontainer, Logger);
            }

            foreach (var item in Components)
            {
                Logger.Debug(string.Format("{0}OnComponentsInitialized", item.Name));

                item.OnComponentsInitialized();
            }
        }
    }
}
