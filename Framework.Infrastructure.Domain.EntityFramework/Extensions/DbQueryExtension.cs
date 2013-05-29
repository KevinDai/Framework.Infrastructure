using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Framework.Infrastructure.Domain.EntityFramework
{
    using Framework.Infrastructure.Domain.Specification;

    public static class DbQueryExtension
    {

        public static DbQuery<T> Include<T>(this DbQuery<T> query, IEnumerable<string> paths)
        {
            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }

            foreach (var path in paths)
            {
                query = query.Include(path);
            }
            return query;
        }

    }
}
