using System.Linq.Expressions;
namespace Chronos.Specification
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        IEnumerable<Expression<Func<T, object>>> Includes { get; }
        IEnumerable<OrderingExpression<T>> OrderingExpressions { get; }
        int? Take { get; }
        int? Skip { get; }
    }
}
