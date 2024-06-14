using System.Linq.Expressions;

namespace Chronos.Specification
{
    public class OrderingExpression<T>
    {
        public Expression<Func<T, object>>? OrderingKeySelector { get; set; }
        public OrderingDirection Direction { get; set; }
    }

    public enum OrderingDirection
    {
        Ascending,
        Descending
    }
}