using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using TQ.Authentication.Data;

namespace TQ.Authentication.API.Helpers
{
    public static  class PaginationHelpers<T>
    {
        public static object GetPaginationHeaders(string requestPath, string requestQueryString, PagedList<T> dtoPagedList, int pageNumber)
        {
            const string PageSize = "PageSize";
            const string PageNumber = "PageNumber";

            return new
            {
                dtoPagedList.TotalCount,
                dtoPagedList.PageSize,
                dtoPagedList.CurrentPage,
                dtoPagedList.TotalPages,
                dtoPagedList.HasNext,
                dtoPagedList.HasPrevious,
                NextPageLink = dtoPagedList.HasNext
                    ? QueryHelpers.AddQueryString(requestPath, new Dictionary<string, string>() {
                        { PageSize, dtoPagedList.PageSize.ToString() },
                        { PageNumber, (pageNumber + 1).ToString() } })
                    : string.Empty,
                PreviousPageLink = dtoPagedList.HasPrevious
                    ? QueryHelpers.AddQueryString(requestPath, new Dictionary<string, string>() {
                        { PageSize, dtoPagedList.PageSize.ToString() },
                        { PageNumber, (pageNumber + 1).ToString() } })
                    : string.Empty,
                CurrentPageLink = $"{requestPath}{requestQueryString}",
            };
        }
    }
}