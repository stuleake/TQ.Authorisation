using System;
using System.Collections.Generic;
using System.Linq;

namespace TQ.Authentication.Data
{
	/// <summary>
	/// Generic PagedList class
	/// </summary>
	/// <typeparam name="T">the generic type parameter</typeparam>
    public class PagedList<T> : List<T>
	{
		private const int DefaultPageSize = 5;

		/// <summary>
		/// Gets or sets the current page
		/// </summary>
		public int CurrentPage { get; private set; }

		/// <summary>
		/// Gets or sets the total pages
		/// </summary>
		public int TotalPages { get; private set; }

		/// <summary>
		/// Gets or sets the page size
		/// </summary>
		public int PageSize { get; private set; }

		/// <summary>
		/// Gets or sets the total count
		/// </summary>
		public int TotalCount { get; private set; }

		/// <summary>
		/// Gets or sets the has previous flag
		/// </summary>
		public bool HasPrevious => CurrentPage > 1;
		
		/// <summary>
		/// Gets or sets the has next flag
		/// </summary>
		public bool HasNext => CurrentPage < TotalPages;

		/// <summary>
		/// Gets the next page number
		/// </summary>
		public int NextPageNumber => this.HasNext ? this.PageSize + 1 : this.TotalPages;
		
		/// <summary>
		/// Gets the previous page number
		/// </summary>
		public int PreviousPageNumber => this.HasPrevious ? this.PageSize - 1 : 1;

		/// <summary>
		/// Initialises a new instance of the <see cref="PagedList{T}"/ class>
		/// </summary>
		/// <param name="items">the items to page</param>
		/// <param name="count">the count of the number </param>
		/// <param name="pageNumber">the page number</param>
		/// <param name="pageSize">the page size</param>
		public PagedList(List<T> items, int count, int pageNumber, int pageSize)
		{
			TotalCount = count;
			PageSize = pageSize;
			CurrentPage = pageNumber == 0 ? 1 : pageNumber;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);

			AddRange(items);
		}

		/// <summary>
		/// Initialises a new instance of the <see cref="PagedList{T}"/ class>
		/// </summary>
		/// <param name="source">the source</param>
		/// <param name="pageNumber">the page number</param>
		/// <param name="pageSize">the page size</param>
		/// <returns></returns>
		public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
		{
			var count = source.Count();

			var skip = pageNumber == 0 ? 0 : pageNumber - 1;
			var take = pageSize == 0 ? DefaultPageSize : pageSize;

			var items = source.Skip((skip) * pageSize).Take(take).ToList();

			return new PagedList<T>(items, count, pageNumber, pageSize);
		}
	}
}