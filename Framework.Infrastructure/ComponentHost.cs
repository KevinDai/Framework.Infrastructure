using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

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

        public ComponentHost(CompositionContainer CompositionContainer)
        {
            CompositionContainer.ComposeParts(this);
        }

        public void Initialize()
        {
            foreach (var item in Components)
            {
                item.Initialize();
            }
        }
    }
}
