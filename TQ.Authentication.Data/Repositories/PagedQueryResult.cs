using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TQ.Authentication.Data.Repositories
{
    /// <summary>
    /// Used to get the paged result.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PagedQueryResult<TEntity>
    {
        // private fields
        private readonly IQueryable<TEntity> _queryableData;

        // Properties
        public int PageSize { get; }

        public int PreviousPage => CurrentPage > 1 ? CurrentPage - 1 : -1;
        public int NextPage => CurrentPage < TotalPages ? CurrentPage + 1 : -1;
        public int TotalRecords { get; }
        public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
        public int CurrentPage { get; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public IEnumerable<TEntity> Data => GetPagedResult();

        /// <summary>
        /// Constructor.
        /// </summary>
        public PagedQueryResult(IQueryable<TEntity> queryableData, int pageSize = 10, int pageNumber = 1)
        {
            _queryableData = queryableData;

            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalRecords = queryableData.Count();
        }

        /// <summary>
        /// Gets called when Result property is called.
        /// </summary>
        /// <returns>a <see cref="IEnumerable{TEntity}"/></returns>
        private IEnumerable<TEntity> GetPagedResult()
        {
            var result = _queryableData
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize);

            // Return
            return result;
        }
    }
}