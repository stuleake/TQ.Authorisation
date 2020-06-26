using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using TQ.Authentication.Core.Dto.Roles;
using TQ.Authentication.Core.Dto.Users;
using TQ.Authentication.Core.Requests.Roles;
using TQ.Authentication.Services.Interfaces;

namespace TQ.Authentication.API.Controllers
{
    /// <summary>
    /// Represents a Roles Controller for managing Roles.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController]
    [Route("api/v1/roles")]
    public class RolesController : ControllerBase
    {
        // Dependencies
        private readonly IRoleManager roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/> class.
        /// </summary>
        /// <param name="roleManager">The role manager.</param>
        public RolesController(IRoleManager roleManager)
        {
            this.roleManager = roleManager;
        }

        /// <summary>
        /// Creates the role asynchronous.
        /// </summary>
        /// <param name="request">The request that contains the details of the role to create</param>
        /// <returns>a <see cref="Task"/ that contains the result of the asynchronous action></returns>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> CreateRoleAsync(CreateRoleRequest request)
        {
            var roleId = await roleManager.CreateRoleAsync(request);

            return StatusCode((int)HttpStatusCode.Created, new { RoleId = roleId });
        }

        /// <summary>
        /// Compares users against the role specified in the url asynchronous.
        /// </summary>
        /// <param name="request">The request that contains the details of the users to add to the role</param>
        /// <param name="roleId">the id of the role to which the users will be added</param>
        /// <returns>a <see cref="Task"/ that contains the result of the asynchronous action></returns>
        [HttpPost]
        [Route("{roleId:guid}/users")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> AddUsersToRoleAsync(Guid roleId, AddUsersRequest request)
        {
            await roleManager.AddUsersToRoleAsync(roleId, request.UserIds);

            return StatusCode((int)HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets users assigned to the role specified in the url asynchronous.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        [HttpGet]
        [Route("{roleId:guid}/users")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetPagedUsersDto>> GetUsersByRoleAsync(Guid roleId)
        {
            var usersByRole = await roleManager.GetUsersByRoleAsync(roleId);

            return Ok(usersByRole);
        }

        /// <summary>
        /// Updates the role asynchronous.
        /// </summary>
        /// <param name="request">The request that contains the details of the role to update</param>
        /// <param name="roleId">The id of the role to update</param>
        /// <returns>a <see cref="Task"/ that contains the result of the asynchronous action></returns>
        [HttpPut]
        [Route("{roleId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateRoleAsync(Guid roleId, UpdateRoleRequest request)
        {
            await roleManager.UpdateRoleAsync(roleId, request);

            return NoContent();
        }

        /// <summary>
        /// Returns the role details including the nested permissions
        /// </summary>
        /// <param name="roleId">The id of the role to get</param>
        /// <returns>a <see cref="Task"/ that contains the result of the asynchronous action></returns>
        [HttpGet]
        [Route("{roleId:guid}")]
        [ProducesResponseType(typeof(GetRoleGroupPermissionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetRoleByIdAsync(Guid roleId)
        {
            var result = await roleManager.GetRoleByIdAsync(roleId);

            return Ok(result);
        }
    }
}