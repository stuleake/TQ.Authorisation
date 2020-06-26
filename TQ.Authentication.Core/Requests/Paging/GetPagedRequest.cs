namespace TQ.Authentication.Core.Requests.Paging
{
    /// <summary>
    /// A request for returning paged results
    /// </summary>
    public class GetPagedRequest
    {
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        public int PageSize { get; set; } = 5;

        /// <summary>
        /// Gets or sets the next page number.
        /// </summary>
        public int PageNumber { get; set; }
    }
}