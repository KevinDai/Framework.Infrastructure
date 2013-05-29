using System.Runtime.Serialization;

namespace Framework.Infrastructure.Querying
{
    /// <summary>
    /// Logical operator used for filter descriptor composition.
    /// </summary>
    [DataContract]
    public enum FilterCompositionLogicalOperator
    {
        /// <summary>
        /// Combines filters with logical AND.
        /// </summary>
        [EnumMember]
        And,

        /// <summary>
        /// Combines filters with logical OR.
        /// </summary>
        [EnumMember]
        Or
    }
}
