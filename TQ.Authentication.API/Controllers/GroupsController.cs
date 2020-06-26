using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TQ.Authentication.API.Helpers;
using TQ.Authentication.Core.Dto.Groups;
using TQ.Authentication.Core.Dto.Permissions;
using TQ.Authentication.Core.Dto.Roles;
using TQ.Authentication.Core.Requests.Groups;
using TQ.Authentication.Core.Requests.Paging;
using TQ.Authentication.Data;
using TQ.Authentication.Data.Entities;
using TQ.Authentication.Services.Interfaces;

namespace TQ.Authentication.API.Controllers
{
    /// <summary>
    /// Represents a Groups Controller for managing Groups.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController]
    [Route("api/v1/groups")]
    public class GroupsController : ControllerBase
    {
        // Dependencies
        private readonly IGroupManager groupManager;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupsController"/> class.
        /// </summary>
        /// <param name="groupManager">The group manager.</param>
        public GroupsController(IGroupManager groupManager)
        {
            this.groupManager = groupManager;
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetGroupGetDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GetGroupGetDto>>> GetAllAsync()
        {
            var groups = await groupManager.GetGroupsAsync();

            return Ok(groups);
        }

        /// <summary>
        /// Gets the Group by identifier asynchronous.
        /// </summary>
        /// <param name="groupId">The id of the group to get</param>
        /// <returns>a <see cref="Task"/ that contains the result of the asynchronous action></returns>
        [HttpGet]
        [Route("{groupId:guid}")]
        [ProducesResponseType(typeof(GetGroupGetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetGroupGetDto>> GetByIdAsync(Guid groupId)
        {
            var group = await groupManager.GetGroupAsync(groupId);

            return Ok(group);
        }

        /// <summary>
        /// Creates a new group
        /// </summary>
        /// <param name="request">The request that contains the details of the group to create</param>
        /// <returns>a <see cref="Task"/ that contains the result of the asynchronous action></returns>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> CreateGroupAsync(CreateGroupRequest request)
        {
            var newGroup = await groupManager.CreateGroupAsync(request);

            return StatusCode(201, new { GroupId = newGroup.Id });
        }

        /// <summary>
        /// Updates the group asynchronous.
        /// </summary>
        /// <param name="request">The request that contains the details of the group to update</param>
        /// <param name="groupId">The id of the group to update</param>
        /// <returns>a <see cref="Task"/ that contains the result of the asynchronous action></returns>
        [HttpPut]
        [Route("{groupId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateGroupAsync(Guid groupId, UpdateGroupRequest request)
        {
            await groupManager.UpdateGroupAsync(groupId, request);

            return NoContent();
        }

        /// <summary>
        /// Adds the user asynchronous.
        /// </summary>
        /// <param name="groupId">the group identifier</param>
        /// <param name="roleId">the role identifier</param>
        /// <returns>a <see cref="Task"/ that contains the result of the asynchronous action></returns>
        [HttpPost]
        [Route("{groupId:guid}/users/{userId:guid}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddUserAsync(Guid groupId, Guid userId)
        {
            ActionResult result;

            var statusCode = HttpStatusCode.OK;

            var userGroupDto = await groupManager.AddUserAsync(groupId, userId);

            if (!userGroupDto.UserAlreadyInGroup)
            {
                statusCode = HttpStatusCode.Created;
            }

            result = StatusCode((int)statusCode, new
            {
                userGroupId = userGroupDto.UserGroupId
            });

            // Return
            return result;
        }

        /// <summary>
        /// Removes the user asynchronous.
        /// </summary>
        /// <param name="groupId">the group identifier</param>
        /// <param name="roleId">the role identifier</param>
        /// <returns>a <see cref="Task"/ that contains the result of the asynchronous action></returns>
        [HttpDelete]
        [Route("{groupId:guid}/users/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RemoveUserAsync(Guid groupId, Guid userId)
        {
            await groupManager.RemoveUserAsync(groupId, userId);

            return NoContent();
        }

        /// <summary>
        /// Gets a list of permissions for a group identifier asynchronously.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>a <see cref="Task{IActionResult<typeparamref name="GetPermissionsDto"/>}"/>that contains group permissions</returns>
        [HttpGet]
        [Route("{groupId:guid}/permissions")]
        [ProducesResponseType(typeof(GetPermissionsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetPermissionsDto>> GetPermissionsByGroupIdAsync(Guid groupId)
        {
            var group = await groupManager.GetGroupPermissionsAsync(groupId);

            return Ok(group);
        }

        /// <summary>
        /// Gets the roles assigned to the specified group
        /// </summary>
        /// <param name="groupId">the group identifier</param>
        /// <param name="request">the request that contains the paging properties</param>
        /// <returns>a <see cref="Task"/ that contains the result of the asynchronous action></returns>
        [HttpGet]
        [Route("{groupId:guid}/roles")]
        [ProducesResponseType(typeof(PagedList<GetRoleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedList<GetRoleDto>>> GetRolesAsync(Guid groupId, [FromQuery] GetPagedRequest request)
        {
            var roles = await this.groupManager.GetRolesAsync(groupId, request);
            
            var paginationHeaders = PaginationHelpers<GetRoleDto>.GetPaginationHeaders($"{Request.Scheme}://{Request.Host}{Request.Path}", Request.QueryString.ToString(), 
                                    roles, 
                                    request.PageNumber);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationHeaders));

            return Ok(roles);
        }
    }
}