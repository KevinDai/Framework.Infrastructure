using System.Runtime.Serialization;

namespace Framework.Infrastructure.Querying
{
    /// <summary>
    /// Operator used in <see cref="FilterDescription"/>
    /// </summary>
    [DataContract]
    public enum FilterOperator
    {
        /// <summary>
        /// Left operand must be smaller than the right one.
        /// </summary>
        [EnumMember]
        IsLessThan,
        /// <summary>
        /// Left operand must be smaller than or equal to the right one.
        /// </summary>
        [EnumMember]
        IsLessThanOrEqualTo,
        /// <summary>
        /// Left operand must be equal to the right one.
        /// </summary>
        [EnumMember]
        IsEqualTo,
        /// <summary>
        /// Left operand must be different from the right one.
        /// </summary>
        [EnumMember]
        IsNotEqualTo,
        /// <summary>
        /// Left operand must be larger than the right one.
        /// </summary>
        [EnumMember]
        IsGreaterThanOrEqualTo,
        /// <summary>
        /// Left operand must be larger than or equal to the right one.
        /// </summary>
        [EnumMember]
        IsGreaterThan,
        /// <summary>
        /// Left operand must start with the right one.
        /// </summary>
        [EnumMember]
        StartsWith,
        /// <summary>
        /// Left operand must end with the right one.
        /// </summary>
        [EnumMember]
        EndsWith,
        /// <summary>
        /// Left operand must contain the right one.
        /// </summary>
        [EnumMember]
        Contains,
        /// <summary>
        /// Left operand must be contained in the right one.
        /// </summary>
        [EnumMember]
        IsContainedIn
    }

}
