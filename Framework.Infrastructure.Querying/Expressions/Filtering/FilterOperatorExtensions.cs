using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Infrastructure.Querying.Expressions
{
    internal static class FilterOperatorExtensions
    {
        internal static readonly MethodInfo StringToLowerMethodInfo = typeof(string).GetMethod("ToLower", new Type[0]);
        internal static readonly MethodInfo StringStartsWithMethodInfo = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        internal static readonly MethodInfo StringEndsWithMethodInfo = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
        internal static readonly MethodInfo StringCompareMethodInfo = typeof(string).GetMethod("Compare", new[] { typeof(string), typeof(string) });
        internal static readonly MethodInfo StringContainsMethodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });


        /// <exception cref="InvalidOperationException"><c>InvalidOperationException</c>.</exception>
        internal static Expression CreateExpression(this FilterOperator filterOperator, Expression left, Expression right)
        {
            switch (filterOperator)
            {
                case FilterOperator.IsLessThan:
                    return GenerateLessThan(left, right);

                case FilterOperator.IsLessThanOrEqualTo:
                    return GenerateLessThanEqual(left, right);

                case FilterOperator.IsEqualTo:
                    return GenerateEqual(left, right);

                case FilterOperator.IsNotEqualTo:
                    return GenerateNotEqual(left, right);

                case FilterOperator.IsGreaterThanOrEqualTo:
                    return GenerateGreaterThanEqual(left, right);

                case FilterOperator.IsGreaterThan:
                    return GenerateGreaterThan(left, right);

                case FilterOperator.StartsWith:
                    return GenerateStartsWith(left, right);

                case FilterOperator.EndsWith:
                    return GenerateEndsWith(left, right);

                case FilterOperator.Contains:
                    return GenerateContains(left, right);

                case FilterOperator.IsContainedIn:
                    return GenerateIsContainedIn(left, right);
            }

            throw new InvalidOperationException();
        }

        private static Expression GenerateEqual(Expression left, Expression right)
        {
            if (left.Type == typeof(string))
            {
                left = GenerateToLowerCall(left);
                right = GenerateToLowerCall(right);
            }
            return Expression.Equal(left, right);
        }

        private static Expression GenerateNotEqual(Expression left, Expression right)
        {
            if (left.Type == typeof(string))
            {
                left = GenerateToLowerCall(left);
                right = GenerateToLowerCall(right);
            }
            return Expression.NotEqual(left, right);
        }

        private static Expression GenerateGreaterThan(Expression left, Expression right)
        {
            if (left.Type == typeof(string))
            {
                return Expression.GreaterThan(
                    GenerateCaseInsensitiveStringMethodCall(StringCompareMethodInfo, left, right),
                    ExpressionFactory.ZeroExpression);
            }
            return Expression.GreaterThan(left, right);
        }

        private static Expression GenerateGreaterThanEqual(Expression left, Expression right)
        {
            if (left.Type == typeof(string))
            {
                return Expression.GreaterThanOrEqual(
                    GenerateCaseInsensitiveStringMethodCall(StringCompareMethodInfo, left, right),
                    ExpressionFactory.ZeroExpression);
            }
            return Expression.GreaterThanOrEqual(left, right);
        }

        private static Expression GenerateLessThan(Expression left, Expression right)
        {
            if (left.Type == typeof(string))
            {
                return Expression.LessThan(
                    GenerateCaseInsensitiveStringMethodCall(StringCompareMethodInfo, left, right),
                    ExpressionFactory.ZeroExpression);
            }
            return Expression.LessThan(left, right);
        }

        private static Expression GenerateLessThanEqual(Expression left, Expression right)
        {
            if (left.Type == typeof(string))
            {
                return Expression.LessThanOrEqual(
                    GenerateCaseInsensitiveStringMethodCall(StringCompareMethodInfo, left, right),
                    ExpressionFactory.ZeroExpression);
            }
            return Expression.LessThanOrEqual(left, right);
        }

        private static Expression GenerateContains(Expression left, Expression right)
        {
            return Expression.Equal(
                GenerateCaseInsensitiveStringMethodCall(StringContainsMethodInfo, left, right),
                ExpressionConstants.TrueLiteral);
        }

        private static Expression GenerateIsContainedIn(Expression left, Expression right)
        {
            return Expression.Equal(
                GenerateCaseInsensitiveStringMethodCall(StringContainsMethodInfo, right, left),
                ExpressionConstants.TrueLiteral);
        }

        private static Expression GenerateStartsWith(Expression left, Expression right)
        {
            return Expression.Equal(
                GenerateCaseInsensitiveStringMethodCall(StringStartsWithMethodInfo, left, right),
                ExpressionConstants.TrueLiteral);
        }

        private static Expression GenerateEndsWith(Expression left, Expression right)
        {
            return Expression.Equal(
                GenerateCaseInsensitiveStringMethodCall(StringEndsWithMethodInfo, left, right),
                ExpressionConstants.TrueLiteral);
        }

        private static Expression GenerateCaseInsensitiveStringMethodCall(MethodInfo methodInfo, Expression left, Expression right)
        {
            var leftToLower = GenerateToLowerCall(left);
            var rightToLower = GenerateToLowerCall(right);

            if (methodInfo.IsStatic)
            {
                return Expression.Call(methodInfo, new[] { leftToLower, rightToLower });
            }

            return Expression.Call(leftToLower, methodInfo, rightToLower);
        }

        private static Expression GenerateToLowerCall(Expression stringExpression)
        {
            var liftedToEmpty = ExpressionFactory.LiftStringExpressionToEmpty(stringExpression);

            return Expression.Call(liftedToEmpty, StringToLowerMethodInfo);
        }
    }
}
