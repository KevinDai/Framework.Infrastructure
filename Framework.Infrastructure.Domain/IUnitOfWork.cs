using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Domain
{
    /// <summary>
    /// 统一工作单元接口定义
    /// </summary>
    public interface IUnitOfWork : ISQL
    {
        /// <summary>
        /// 提交在当前容器中的所有更新
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// 记录新增的Entity对象
        /// </summary>
        /// <typeparam name="TEntity">Entity类型</typeparam>
        /// <typeparam name="TId">Entity对象的标识符类型</typeparam>
        /// <param name="entity">新增的Entity对象</param>
        void RegisterNew<TEntity, TId>(TEntity entity) where TEntity : EntityBase<TId>;

        /// <summary>
        /// 记录更新的Entity对象
        /// </summary>
        /// <typeparam name="TEntity">Entity类型</typeparam>
        /// <typeparam name="TId">Entity对象的标识符类型</typeparam>
        /// <param name="entity">更新的Entity对象</param>
        void RegisterModify<TEntity, TId>(TEntity entity) where TEntity : EntityBase<TId>;

        /// <summary>
        /// 记录删除的Entity对象
        /// </summary>
        /// <typeparam name="TEntity">Entity类型</typeparam>
        /// <typeparam name="TId">Entity对象的标识符类型</typeparam>
        /// <param name="entity">删除的Entity对象</param>
        void RegisterRemove<TEntity, TId>(TEntity entity) where TEntity : EntityBase<TId>;

    }
}
