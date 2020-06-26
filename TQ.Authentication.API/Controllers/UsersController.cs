using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TQ.Authentication.Core.Dto.Users;
using TQ.Authentication.Core.Requests.Users;
using TQ.Authentication.Services.Interfaces;

namespace TQ.Authentication.API.Controllers
{
    /// <summary>
    /// Represents a Users Controller for managing Users.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        // Dependencies
        private readonly IUserManager userManager;

        private readonly IPermissionManager permissionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="permissionManager">The permission manager.</param>
        public UsersController(IUserManager userManager, IPermissionManager permissionManager)
        {
            this.userManager = userManager;
            this.permissionManager = permissionManager;
        }

        /// <summary>
        /// Gets the filtered.
        /// </summary>
        /// <param name="request">The request that contains the user filters to apply</param>
        /// <returns>a <see cref="Task"/ that contains the result of the asynchronous action></returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetPagedUsersDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetPagedUsersDto>> GetFilteredAsync([FromQuery] GetFilteredUsersRequest request)
        {
            var users = await userManager.GetUsersAsync(request);

            return Ok(users);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="userId">The id of the user to get</param>
        /// <returns>a <see cref="Task"/ that contains the result of the asynchronous action></returns>
        [HttpGet]
        [Route("{userId:guid}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetByIdAsync(Guid userId)
        {
            var user = await userManager.GetUserAsync(userId);

            return Ok(user);
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">The request that contains the details of the user to update</param>
        /// <returns>a <see cref="Task"/ that contains the result of the asynchronous action></returns>
        [HttpPut]
        [Route("{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateUserAsync(Guid userId, UpdateUserRequest request)
        {
            await userManager.UpdateUserAsync(userId, request);

            return NoContent();
        }

        /// <summary>
        /// Gets permissions by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>a <see cref="Task{IActionResult}"/>that contains an anonymous object of user permissions</returns>
        [HttpGet]
        [Route("permissions")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetPermissionsByUserIdAsync([FromQuery(Name = "objectId")] Guid userId)
        {
            var userPermissions = await permissionManager.GetUserPermissionsAsync(userId);

            return Ok(userPermissions);
        }
    }
}