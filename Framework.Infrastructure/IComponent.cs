using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure
{
    public interface IComponent
    {
        string Name { get; }
        void Initialize();
    }
}
