using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Framework.Infrastructure.Domain.EntityFramework
{
    public interface IEntityUnitOfWork : IUnitOfWork
    {
        IDbSet<T> DbSet<T>() where T : class;
    }
}
