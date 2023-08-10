using System.Linq.Expressions;

namespace FileStatisticsWatcher.Models.DTO.FilteringDTO
{
    public class WhereSettings
    {
        public string FieldName { get; set; }
        public WhereState State { get; set; }
        public string TargetValue { get; set; }
        public bool NotFlag { get; set; } = false;

        public Expression<Func<TData, bool>> CreateFilter<TData, TKey>(Expression<Func<TData, TKey>> selector, TKey valueToCompare, WhereState whereState)
        {
            var parameter = Expression.Parameter(typeof(TData));
            var expressionParameter = Expression.Property(parameter, GetParameterName(selector));

            BinaryExpression body = Expression.Equal(expressionParameter, Expression.Constant(valueToCompare, typeof(TKey)));


            switch (whereState)
            {
                case WhereState.eq:
                    if (NotFlag)
                    {
                        body = Expression.NotEqual(expressionParameter, Expression.Constant(valueToCompare, typeof(TKey)));
                    }
                    else
                    {
                        body = Expression.Equal(expressionParameter, Expression.Constant(valueToCompare, typeof(TKey)));
                    }
                    break;
                case WhereState.more:
                    if (NotFlag)
                    {
                        body = Expression.LessThanOrEqual(expressionParameter, Expression.Constant(valueToCompare, typeof(TKey)));
                    }
                    else
                    {
                        body = Expression.GreaterThan(expressionParameter, Expression.Constant(valueToCompare, typeof(TKey)));
                    }
                    break;
                case WhereState.less:
                    if (NotFlag)
                    {
                        body = Expression.GreaterThanOrEqual(expressionParameter, Expression.Constant(valueToCompare, typeof(TKey)));
                    }
                    else
                    {
                        body = Expression.LessThan(expressionParameter, Expression.Constant(valueToCompare, typeof(TKey)));
                    }
                    break;
                default:
                    break;
            }

            return Expression.Lambda<Func<TData, bool>>(body, parameter);
        }

        private string GetParameterName<TData, TKey>(Expression<Func<TData, TKey>> expression)
        {
            if (!(expression.Body is MemberExpression memberExpression))
            {
                memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            return memberExpression.ToString().Substring(2);
        }
    }
}
