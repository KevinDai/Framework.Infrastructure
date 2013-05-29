using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Domain
{
    /// <summary>
    /// 实体基类
    /// </summary>
    /// <typeparam name="TId">实体标识符类型</typeparam>
    public abstract class EntityBase<TId>
    {
        private List<BusinessRule> _brokenRules = new List<BusinessRule>();

        public virtual TId Id { get; set; }

        protected abstract void Validate();

        public IEnumerable<BusinessRule> GetBrokenRules()
        {
            _brokenRules.Clear();
            Validate();
            return _brokenRules;
        }

        protected void AddBrokenRule(BusinessRule businessRule)
        {
            _brokenRules.Add(businessRule);
        }

        public override bool Equals(object entity)
        {
            return entity != null
               && entity is EntityBase<TId>
               && this == (EntityBase<TId>)entity;

        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(EntityBase<TId> entity1, EntityBase<TId> entity2)
        {
            if (entity1 == null && entity2 == null)
            {
                return true;
            }

            if (entity1 == null || entity2 == null)
            {
                return false;
            }
            if (entity1.Id.Equals(entity2.Id))
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(EntityBase<TId> entity1,
            EntityBase<TId> entity2)
        {
            return (!(entity1 == entity2));
        }
    }
}
