using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Linq.Expressions;
using System.Linq;

namespace Framework.Infrastructure.Querying
{
    /// <summary>
    /// Represents declarative sorting.
    /// </summary>
    [DataContract]
    public partial class SortDescriptor 
    {

        public SortDescriptor(string member, ListSortDirection sortDirection)
        {
            Member = member;
            SortDirection = sortDirection;
        }

        public static SortDescriptor Create<T>(Expression<Func<T, object>> expression, ListSortDirection sortDirection)
        {
            string propertyName = PropertyNameHelper.ResolvePropertyName<T>(expression);
            SortDescriptor sortDescriptor = new SortDescriptor(propertyName, sortDirection);
            IQueryable<object> test;
            return sortDescriptor;
        }

        /// <summary>
        /// Gets or sets the member name which will be used for sorting.
        /// </summary>
        /// <filterValue>The member that will be used for sorting.</filterValue>
        [DataMember]
        public string Member
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sort direction for this sort descriptor. If the value is null
        /// no sorting will be applied.
        /// </summary>
        /// <value>The sort direction. The default value is null.</value>
        [DataMember]
        public ListSortDirection SortDirection
        {
            get;
            set;
        }
    }
}
