using System.ComponentModel;

namespace Framework.Infrastructure.Querying
{

    public interface ISortDescriptor
    {
        string Member
        {
            get;
            set;
        }

        ListSortDirection SortDirection
        {
            get;
            set;
        }
    }
}
