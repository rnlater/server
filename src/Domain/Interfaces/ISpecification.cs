using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Domain.Interfaces
{
    public interface ISpecification<T>
    {
        /// <summary>
        /// Criteria to filter entities
        /// </summary>
        Expression<Func<T, bool>>? Criteria { get; }

        /// <summary>
        /// Includes expressions to include related entities
        /// </summary>
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? Includes { get; }

        /// <summary>
        /// Order by expression
        /// </summary>
        Expression<Func<T, object>>? OrderBy { get; }

        /// <summary>
        /// Order by descending expression
        /// </summary>
        Expression<Func<T, object>>? OrderByDescending { get; }

        /// <summary>
        /// Group by expression
        /// </summary>
        Expression<Func<T, object>>? GroupBy { get; }

        /// <summary>
        /// Number of records to take
        /// </summary>
        int Take { get; }

        /// <summary>
        /// Number of records to skip
        /// </summary>
        int Skip { get; }

        /// <summary>
        /// Indicates if paging is enabled
        /// </summary>
        bool IsPagingEnabled { get; }

        /// <summary>
        /// Indicates if tracking is enabled
        /// </summary>
        bool IsTrackingEnabled { get; }

        /// <summary>
        /// Adds an include expression to include related entities
        /// </summary>
        /// <param name="Includes">The include expression</param>
        /// <returns>The specification with the include expression added</returns>
        public ISpecification<T> AddInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> Includes);

        /// <summary>
        /// Adds an order by expression
        /// </summary>
        /// <param name="orderByExpression">The order by expression</param>
        /// <returns>The specification with the order by expression added</returns>
        public ISpecification<T> AddOrderBy(Expression<Func<T, object>> orderByExpression);

        /// <summary>
        /// Adds an order by descending expression
        /// </summary>
        /// <param name="orderByDescendingExpression">The order by descending expression</param>
        /// <returns>The specification with the order by descending expression added</returns>
        public ISpecification<T> AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression);

        /// <summary>
        /// Applies paging to the specification
        /// </summary>
        /// <param name="skip">The number of records to skip</param>
        /// <param name="take">The number of records to take</param>
        /// <returns>The specification with paging applied</returns>
        public ISpecification<T> ApplyPaging(int skip, int take);

        /// <summary>
        /// Applies tracking to the specification
        /// </summary>
        /// <param name="isTrackingEnabled">Indicates if tracking is enabled</param>
        /// <returns>The specification with tracking applied</returns>
        public ISpecification<T> ApplyTracking(bool isTrackingEnabled);

        /// <summary>
        /// Applies a group by expression to the specification
        /// </summary>
        /// <param name="groupByExpression">The group by expression</param>
        /// <returns>The specification with the group by expression applied</returns>
        public ISpecification<T> ApplyGroupBy(Expression<Func<T, object>> groupByExpression);
    }
}