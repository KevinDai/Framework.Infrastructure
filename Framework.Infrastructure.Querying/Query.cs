using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Framework.Infrastructure.Querying
{
    [DataContract]
    [KnownType(typeof(FilterDescriptor))]
    [KnownType(typeof(CompositeFilterDescriptor))]
    public class Query
    {
        private IList<FilterDescriptorBase> _filterDescriptors = new List<FilterDescriptorBase>();
        private IList<SortDescriptor> _sortDescriptors = new List<SortDescriptor>();

        [DataMember]
        public IEnumerable<FilterDescriptorBase> FilterDescriptors
        {
            get { return _filterDescriptors; }
            set
            {
                if (value == null)
                {
                    _filterDescriptors = new List<FilterDescriptorBase>();
                }
                else
                {
                    _filterDescriptors = new List<FilterDescriptorBase>(value);
                }
            }
        }

        [DataMember]
        public IEnumerable<SortDescriptor> SortDescriptors
        {
            get { return _sortDescriptors; }
            set
            {
                if (value == null)
                {
                    _sortDescriptors = new List<SortDescriptor>();
                }
                else
                {
                    _sortDescriptors = new List<SortDescriptor>(value);
                }
            }
        }

        [DataMember]
        public Pagination Pagination
        {
            get;
            set;
        }

        public void AddFilterDescriptor(FilterDescriptorBase filterDescriptor)
        {
            _filterDescriptors.Add(filterDescriptor);
        }

        public void AddSortDescriptor(SortDescriptor sortDescriptor)
        {
            _sortDescriptors.Add(sortDescriptor);
        }
    }

}
