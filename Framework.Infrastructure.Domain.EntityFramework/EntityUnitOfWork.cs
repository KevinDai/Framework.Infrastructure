using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace Framework.Infrastructure.Domain.EntityFramework
{
    public class EntityUnitOfWork : IEntityUnitOfWork
    {
        #region Members

        internal DbContext Context
        {
            get
            {
                return _context;
            }
        }
        private DbContext _context;

        #endregion

        #region Contstructor

        public EntityUnitOfWork(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            _context = dbContext;
            _context.Configuration.LazyLoadingEnabled = false;
            _context.Configuration.ProxyCreationEnabled = false;
        }


        #endregion

        #region Methods

        #endregion

        #region IQueryUnitOfWork Implementation

        public virtual IDbSet<T> DbSet<T>()
            where T : class
        {
            return this.Context.Set<T>();
        }

        public virtual void SaveChanges()
        {
            this.Context.SaveChanges();
        }

        public virtual void RegisterNew<T, TId>(T entity) where T : EntityBase<TId>
        {
            this.Context.Entry<T>(entity).State = EntityState.Added;
        }

        public virtual void RegisterModify<T, TId>(T entity) where T : EntityBase<TId>
        {
            this.Context.Entry<T>(entity).State = EntityState.Modified;
        }

        public virtual void RegisterRemove<T, TId>(T entity) where T : EntityBase<TId>
        {
            this.Context.Entry<T>(entity).State = EntityState.Deleted;
        }

        public virtual IEnumerable<T> ExecuteQuery<T, TId>(string sqlQuery, params object[] parameters)
            where T : EntityBase<TId>
        {
            var query = this.Context.Database.SqlQuery<T>(sqlQuery, parameters);
            return query.ToArray();
        }

        public virtual int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return this.Context.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        #endregion
    }
}
