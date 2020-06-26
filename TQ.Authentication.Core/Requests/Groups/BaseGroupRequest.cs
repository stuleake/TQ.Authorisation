namespace TQ.Authentication.Core.Requests.Groups
{
    /// <summary>
    /// Represents a BaseGroupRequest
    /// </summary>
    public class BaseGroupRequest
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the service URL
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active
        /// </summary>
        public bool IsActive { get; set; }
    }
}