using System.Linq.Expressions;

namespace LexisNexis.Common.Filters
{
    public static class PredicateExtensions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>>? left, Expression<Func<T, bool>> right)
        {
            if (left == null)
            {
                return right;
            }

            ParameterExpression parameter = Expression.Parameter(typeof(T));

            Expression leftBody = ReplaceParameter(left.Body, left.Parameters[0], parameter);
            Expression rightBody = ReplaceParameter(right.Body, right.Parameters[0], parameter);

            return Expression.Lambda<Func<T, bool>>(  Expression.AndAlso(leftBody, rightBody), parameter);
        }

        private static Expression ReplaceParameter(Expression body,  ParameterExpression source, ParameterExpression target)=> new ParameterReplacer(source, target).Visit(body)!;

        private sealed class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _source;
            private readonly ParameterExpression _target;

            public ParameterReplacer(ParameterExpression source, ParameterExpression target)
            {
                _source = source;
                _target = target;
            }

            protected override Expression VisitParameter(ParameterExpression node) => node == _source ? _target : base.VisitParameter(node);
        }
    }
}
