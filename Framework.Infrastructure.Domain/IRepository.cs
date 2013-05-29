using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Framework.Infrastructure.Domain
{

    using Specification;

    /// <summary>
    /// 实体数据仓库操作接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TId">实体标识符类型</typeparam>
    public interface IRepository<TEntity, TId>
        where TEntity : EntityBase<TId>, IAggregateRoot
    {

        /// <summary>
        /// 获取指定标识符的实体对象
        /// </summary>
        /// <param name="id">标识符</param>
        /// <returns>实体</returns>
        TEntity Get(TId id);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Add(TEntity entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Update(TEntity entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);

        /// <summary>
        /// 查询全部实体
        /// </summary>
        /// <returns>实体列表</returns>
        IEnumerable<TEntity> FindAll(params SortDescriptor<TEntity>[] sortDescriptors);

        /// <summary>
        /// 根据过滤的规约查询实体
        /// </summary>
        /// <param name="specification">过滤的规约</param>
        /// <param name="sortDescriptors">排序说明对象</param>
        /// <returns>实体列表</returns>
        IEnumerable<TEntity> FindBy(ISpecification<TEntity> specification, params SortDescriptor<TEntity>[] sortDescriptors);

        /// <summary>
        /// 根据过滤的规约查询实体并分页后返回指定页的实体列表
        /// </summary>
        /// <param name="specification">过滤的规约</param>
        /// <param name="pageIndex">指定页的页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sortDescriptors">排序说明对象</param>
        /// <returns>实体列表</returns>
        IEnumerable<TEntity> FindPageBy(
            ISpecification<TEntity> specification,
            int pageIndex,
            int pageSize,
            params SortDescriptor<TEntity>[] sortDescriptors);

        /// <summary>
        /// 根据过滤的规约查询实体并分页后返回指定页的实体列表
        /// </summary>
        /// <param name="specification">过滤的规约</param>
        /// <param name="pageIndex">指定页的页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="totalCount">符合过滤规约实体的总数</param>
        /// <param name="sortDescriptors">排序说明对象</param>
        /// <returns>实体列表</returns>
        IEnumerable<TEntity> FindPageBy(
            ISpecification<TEntity> specification,
            int pageIndex,
            int pageSize,
            out int totalCount,
            params SortDescriptor<TEntity>[] sortDescriptors);
    }
}
