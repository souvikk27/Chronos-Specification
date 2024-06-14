using System.Linq.Expressions;

namespace Chronos.Specification
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; private set; } = null!;
        private readonly List<Expression<Func<T, object>>> _includes = new List<Expression<Func<T, object>>>();
        private readonly List<OrderingExpression<T>> _orderingExpressions = new List<OrderingExpression<T>>();
        public IEnumerable<Expression<Func<T, object>>> Includes => _includes.AsReadOnly();
        public IEnumerable<OrderingExpression<T>> OrderingExpressions => _orderingExpressions.AsReadOnly();
        public int? Take { get; private set; }
        public int? Skip { get; private set; }

        protected Specification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
        }

        protected Specification() { }

        public void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            _includes.Add(includeExpression);
        }

        public void AddOrderBy(Expression<Func<T, object>> orderByExpression, OrderingDirection direction)
        {
            _orderingExpressions.Add(new OrderingExpression<T> { OrderingKeySelector = orderByExpression, Direction = direction });
        }

        public void ApplyPaging(int skip, int take)
        {
            if (skip < 0) throw new ArgumentOutOfRangeException(nameof(skip), "Skip must be non-negative.");
            if (take <= 0) throw new ArgumentOutOfRangeException(nameof(take), "Take must be positive.");

            Skip = skip;
            Take = take;
        }

        public Specification<T> And(Specification<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            Criteria = Criteria == null ? other.Criteria : Criteria.And(other.Criteria);
            _includes.AddRange(other.Includes);
            _orderingExpressions.AddRange(other.OrderingExpressions);

            return this;
        }

        public Specification<T> Or(Specification<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            Criteria = Criteria == null ? other.Criteria : Criteria.Or(other.Criteria);
            _includes.AddRange(other.Includes);
            _orderingExpressions.AddRange(other.OrderingExpressions);

            return this;
        }
    }
}
