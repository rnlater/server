using System.Linq.Expressions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

namespace Domain.Base
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>>? Criteria { get; private set; }
        public Func<IQueryable<T>, IIncludableQueryable<T, object>>? Includes { get; private set; }
        public Expression<Func<T, object>>? OrderBy { get; private set; }
        public Expression<Func<T, object>>? OrderByDescending { get; private set; }
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; }
        public bool IsTrackingEnabled { get; private set; }
        public Expression<Func<T, object>>? GroupBy { get; private set; }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public BaseSpecification() { }

        public ISpecification<T> AddInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> Includes)
        {
            this.Includes = Includes;
            return this;
        }

        public ISpecification<T> AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
            return this;
        }

        public ISpecification<T> AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
            return this;
        }

        public ISpecification<T> ApplyPaging(int page, int pageSize)
        {
            Skip = (page - 1) * pageSize;
            Take = pageSize;
            IsPagingEnabled = true;
            return this;
        }

        public ISpecification<T> ApplyTracking(bool isTrackingEnabled)
        {
            IsTrackingEnabled = isTrackingEnabled;
            return this;
        }

        public ISpecification<T> ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
        {
            GroupBy = groupByExpression;
            return this;
        }
    }
}