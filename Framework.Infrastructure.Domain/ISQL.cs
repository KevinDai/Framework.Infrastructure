using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Domain
{
    /// <summary>
    /// 进行SQL查询的接口定义
    /// </summary>
    public interface ISQL
    {
        /// <summary>
        /// 执行SQL语句进行查询
        /// </summary>
        /// <typeparam name="TEntity">查询结果对应的实体类型</typeparam>
        /// <param name="sqlQuery">SQL语句</param>
        /// <param name="parameters">查询参数容器</param>
        /// <returns>实体列表</returns>
        IEnumerable<TEntity> ExecuteQuery<TEntity,TId>(string sqlQuery, params object[] parameters) where TEntity : EntityBase<TId>;

        /// <summary>
        /// 执行SQL语句进行操作
        /// </summary>
        /// <param name="sqlCommand">SQL语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>受影响的记录数量</returns>
        int ExecuteCommand(string sqlCommand, params object[] parameters);

    }
}
