using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Domain.Test.Specification
{
    #region Entity Class

    public class BaseEntity
    {
        public int Id { get; set; }
        public string SampleProperty { get; set; }
    }

    public class InheritEntity : BaseEntity
    {
        public string InheritProperty { get; set; }
    }

    #endregion
}
