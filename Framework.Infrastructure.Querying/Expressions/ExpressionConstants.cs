using System.Linq.Expressions;

namespace Framework.Infrastructure.Querying.Expressions
{
    internal class ExpressionConstants
    {
        internal static readonly Expression TrueLiteral = Expression.Constant(true);
        internal static readonly Expression FalseLiteral = Expression.Constant(false);
        internal static readonly Expression NullLiteral = Expression.Constant(null);
    } 
}
