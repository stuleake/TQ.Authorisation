using System;
using System.Collections.Generic;

namespace TQ.Authentication.Core.Dto.Users
{
    /// <summary>
    /// Represents a User DTO.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the name of the given.
        /// </summary>
        /// <value>
        /// The name of the given.
        /// </value>
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the account enabled.
        /// </summary>
        /// <value>
        /// The account enabled.
        /// </value>
        public bool? AccountEnabled { get; set; }

        /// <summary>
        /// Gets or sets the name of the user principal.
        /// </summary>
        /// <value>
        /// The name of the user principal.
        /// </value>
        public string UserPrincipalName { get; set; }

        /// <summary>
        /// Gets or sets the business phones.
        /// </summary>
        /// <value>
        /// The business phones.
        /// </value>
        public IEnumerable<string> BusinessPhones { get; set; }

        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        /// <value>
        /// The job title.
        /// </value>
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the mobile phone.
        /// </summary>
        /// <value>
        /// The mobile phone.
        /// </value>
        public string MobilePhone { get; set; }

        /// <summary>
        /// Gets or sets the office location.
        /// </summary>
        /// <value>
        /// The office location.
        /// </value>
        public string OfficeLocation { get; set; }

        /// <summary>
        /// Gets or sets the preferred language.
        /// </summary>
        /// <value>
        /// The preferred language.
        /// </value>
        public string PreferredLanguage { get; set; }

        /// <summary>
        /// Gets or sets the mail.
        /// </summary>
        /// <value>
        /// The mail.
        /// </value>
        public string Mail { get; set; }
    }
}