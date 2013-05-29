using System;
using System.Linq.Expressions;

namespace Framework.Infrastructure.Querying
{
    public static class PropertyNameHelper
    {
        public static string ResolvePropertyName<T>(Expression<Func<T, object>> expression)
        {
            var expr = expression.Body as MemberExpression;
            if (expr == null)
            {
                var u = expression.Body as UnaryExpression;
                expr = u.Operand as MemberExpression;
            }
            return expr.ToString().Substring(expr.ToString().IndexOf(".") + 1);
        }

        public static string[] ResolvePropertyNames<T>(params Expression<Func<T, object>>[] expressions)
        {
            string[] result = new string[expressions.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = ResolvePropertyName<T>(expressions[i]);
            }
            return result;
        }
    }
}
