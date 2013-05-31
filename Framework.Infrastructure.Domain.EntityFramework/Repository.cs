using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Framework.Infrastructure.Domain.EntityFramework
{

    /// <summary>
    /// 使用EntityFramework的实体数据仓库基类
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <typeparam name="TId">实体标识符类型</typeparam>
    public abstract class Repository<T, TId> : IRepository<T, TId>
        where T : EntityBase<TId>, IAggregateRoot
    {

        #region Members

        protected IEntityUnitOfWork UnitOfWork
        {
            get
            {
                return _unitOfWork;
            }
        }
        private IEntityUnitOfWork _unitOfWork;

        /// <summary>
        /// 进行查询的实体数据源
        /// </summary>
        protected virtual IQueryable<T> Query
        {
            get
            {
                return _unitOfWork.DbSet<T>() as IQueryable<T>;
            }
        }

        ///// <summary>
        ///// 在执行查询时，默认加载的关联属性的名称的集合，参考Entity的Include机制
        ///// </summary>
        //protected virtual IEnumerable<string> Paths
        //{
        //    get
        //    {
        //        return null;
        //    }
        //}

        #endregion

        #region Constructor

        public Repository(IEntityUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            _unitOfWork = unitOfWork;
        }

        #endregion

        #region IRepository<T, TId> Implementation

        /// <summary>
        /// <see cref="IRepository{T, TId}"/>
        /// </summary>
        /// <param name="id"><see cref="IRepository{T, TId}"/></param>
        /// <returns><see cref="IRepository{T, TId}"/></returns>
        public virtual T Get(TId id)
        {
            /*
             * 在数据库中进行查询时直接使用e => e.Id.Equals(id)时，
             * 由于将id作为object参数处理时会产生异常，因此代码构建Lambda表达式
             */
            Expression<Func<T, bool>> exp = CreateIdEqualExpression(id);

            var entity = _unitOfWork.DbSet<T>().Local.FirstOrDefault(exp.Compile());
            //当前上下文中不存在要查找的实体则从持久化数据源中获取
            if (entity == null)
            {
                entity = Query.FirstOrDefault(exp);
            }
            return entity;
        }

        /// <summary>
        /// <see cref="IRepository{T, TId}"/>
        /// </summary>
        /// <param name="entity"><see cref="IRepository{T, TId}"/></param>
        public virtual void Add(T entity)
        {
            _unitOfWork.RegisterNew<T, TId>(entity);
        }

        /// <summary>
        /// <see cref="IRepository{T, TId}"/>
        /// </summary>
        /// <param name="entity"><see cref="IRepository{T, TId}"/></param>
        public virtual void Update(T entity)
        {
            _unitOfWork.RegisterModify<T, TId>(entity);
        }

        /// <summary>
        /// <see cref="IRepository{T, TId}"/>
        /// </summary>
        /// <param name="entity"><see cref="IRepository{T, TId}"/></param>
        public virtual void Remove(T entity)
        {
            _unitOfWork.RegisterRemove<T, TId>(entity);
        }

        /// <summary>
        /// <see cref="IRepository{T, TId}"/>
        /// </summary>
        /// <returns><see cref="IRepository{T, TId}"/></returns>
        public virtual IEnumerable<T> FindAll(params SortDescriptor<T>[] sortDescriptors)
        {
            return Query.Sort<T, TId>(sortDescriptors).ToArray();
        }

        /// <summary>
        /// <see cref="IRepository{T, TId}"/>
        /// </summary>
        /// <param name="specification"><see cref="IRepository{T, TId}"/></param>
        /// <param name="sortDescriptors"><see cref="IRepository{T, TId}"/></param>
        /// <returns><see cref="IRepository{T, TId}.FindBy"/></returns>
        public virtual IEnumerable<T> FindBy(
            Specification.ISpecification<T> specification,
            params SortDescriptor<T>[] sortDescriptors)
        {
            var query = Query;

            query = query
                .FindBy<T, TId>(specification)
                .Sort<T, TId>(sortDescriptors);

            return query.ToArray();
        }

        /// <summary>
        /// <see cref="IRepository{T, TId}"/>
        /// </summary>
        /// <param name="specification"><see cref="IRepository{T, TId}"/></param>
        /// <param name="pageIndex"><see cref="IRepository{T, TId}"/></param>
        /// <param name="pageSize"><see cref="IRepository{T, TId}"/></param>
        /// <param name="sortDescriptors"><see cref="IRepository{T, TId}"/></param>
        /// <returns><see cref="IRepository{T, TId}"/></returns>
        public virtual IEnumerable<T> FindPageBy(
            Specification.ISpecification<T> specification,
            int pageIndex, 
            int pageSize,
            params SortDescriptor<T>[] sortDescriptors)
        {

            var query = Query;

            query = query
                .FindBy<T, TId>(specification)
                .Sort<T, TId>(sortDescriptors)
                .Paginate(pageIndex, pageSize);

            return query.ToArray();
        }

        /// <summary>
        /// <see cref="IRepository{T, TId}"/>
        /// </summary>
        /// <param name="specification"><see cref="IRepository{T, TId}"/></param>
        /// <param name="pageIndex"><see cref="IRepository{T, TId}"/></param>
        /// <param name="pageSize"><see cref="IRepository{T, TId}"/></param>
        /// <param name="totalCount"><see cref="IRepository{T, TId}"/></param>
        /// <param name="sortDescriptors"><see cref="IRepository{T, TId}"/></param>
        /// <returns><see cref="IRepository{T, TId}"/></returns>
        public virtual IEnumerable<T> FindPageBy(
            Specification.ISpecification<T> specification,
            int pageIndex,
            int pageSize, 
            out int totalCount,
            params SortDescriptor<T>[] sortDescriptors)
        {
            var query = Query;

            query = query.FindBy<T, TId>(specification);
            //获取过滤后的记录总数
            totalCount = query.Count();

            query = query
                .Sort<T, TId>(sortDescriptors)
                .Paginate(pageIndex, pageSize);

            return query.ToArray();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 构造根据标识符查询时的Lambda表达式
        /// </summary>
        /// <param name="id">标识符</param>
        /// <returns>Lamabda表达式</returns>
        private Expression<Func<T, bool>> CreateIdEqualExpression(TId id)
        {
            Expression<Func<T, TId>> memberAccess = e => e.Id;
            ConstantExpression constantExp = Expression.Constant(id, typeof(TId));
            Expression equal = Expression.Equal(memberAccess.Body, constantExp);
            Expression<Func<T, bool>> exp = Expression.Lambda<Func<T, bool>>(equal, memberAccess.Parameters);
            return exp;
        }

        #endregion

    }
}
