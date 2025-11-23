using System.ComponentModel.DataAnnotations;
using WorkWiseApi.Attributes;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using Contracts;
using Contracts.IServices;

namespace WorkWiseApi.Controllers;

/// <summary>
/// Class <c>UserController</c> for managing the user operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ILoggerManager _logger;

    private readonly IUserService _userService;

    /// <summary>
    /// Constructor for inject the dependencies.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="userService"></param>
    public UserController(ILoggerManager logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    /// <summary>
    /// Authentication API.
    /// </summary>
    /// <remarks>Authenticate the user with the credentials.</remarks>
    /// <param name="body">LoginDto object with user credentials.</param>
    /// <response code="200">Successfully Authenticated.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost]
    [Route("~/api/user/login")]
    [ValidateModelState]
    [SwaggerOperation("AuthenticateUser")]
    [SwaggerResponse(statusCode: 200, type: typeof(TokenDto), description: "Successfully Authenticated.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Bad Request.")]
    [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponse), description: "The user is not authorized.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error.")]
    public virtual IActionResult AuthenticateUser([FromBody] LoginDto body)
    {
        _logger.LogDebug("Authenticating user with username and password");
        TokenDto _tokenDto = _userService.GetAuthToken(body);
        _logger.LogInfo($"Authentication successful for the user :{body.UserName}");
        return StatusCode(200, _tokenDto);
    }

    /// <summary>
    /// Reset Password API.
    /// </summary>
    /// <remarks>Reset the password of the user.</remarks>
    /// <param name="resetPasswordDto">ResetPasswordDto object with user credentials.</param>
    /// <response code="200">Password reset successfully.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost]
    [Route("/api/user/reset-password")]
    [Authorize]
    [ValidateModelState]
    [SwaggerOperation("ResetUserPassword")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Bad Request.")]
    [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponse), description: "The user is not authorized.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error.")]
    public virtual IActionResult ResetUserPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        _logger.LogDebug("Updating user secret value");
        Guid loggedInUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        _userService.ResetUserPassword(resetPasswordDto, loggedInUserId);
        return StatusCode(200);
    }

    /// <summary>
    /// Create a new user.
    /// </summary>
    /// <remarks>Create a new user with the given input.</remarks>
    /// <param name="userDto">UserDto object with user details.</param>
    /// <response code="201">User created successfully.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="409">User already exists</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [Route("/api/user")]
    [Authorize]
    [ValidateModelState]
    [SwaggerOperation("CreateUser")]
    [SwaggerResponse(statusCode: 201, type: typeof(ResponseDto), description: "User created successfully.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Bad Request.")]
    [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponse), description: "The user is not authorized.")]
    [SwaggerResponse(statusCode: 409, type: typeof(ErrorResponse), description: "User already exists")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error")]
    public virtual IActionResult CreateUser([FromBody] UserDto userDto)
    {
        _logger.LogDebug("Create user for given details");
        return StatusCode(201, _userService.CreateUser(userDto));
    }

    /// <summary>
    /// Get user details.
    /// </summary>
    /// <remarks>Get the user details for the given user id.</remarks>
    /// <param name="userId">Id of the user.</param>
    /// <response code="200">User found successfully.</response>
    /// <response code="401">The user is not authorized</response>
    /// <response code="404">The user not found for given id.</response>
    /// <response code="500">Internal server error</response>
    [HttpGet]
    [Route("/api/user/{user-id}")]
    [Authorize]
    [ValidateModelState]
    [SwaggerOperation("GetUser")]
    [SwaggerResponse(statusCode: 200, type: typeof(UserDto), description: "User found successfully.")]
    [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponse), description: "The user is not authorized")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "The user not found for given id.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error")]
    public virtual IActionResult GetUser([FromRoute(Name = "user-id")][Required] Guid userId)
    {
        _logger.LogDebug("Get user details");
        return StatusCode(200, _userService.GetUser(userId));
    }

    /// <summary>
    /// Update user details.
    /// </summary>
    /// <remarks>Update the user details for the given user id.</remarks>
    /// <param name="userId">Id of the user.</param>
    /// <param name="userDto">UserDto object with user details.</param>
    /// <response code="200">User updated successfully.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="401">The user is not authorized</response>
    /// <response code="404">The user not found for given id.</response>
    /// <response code="409">User already exists.</response>
    /// <response code="500">Internal server error</response>
    [HttpPut]
    [Route("/api/user/{user-id}")]
    [Authorize]
    [ValidateModelState]
    [SwaggerOperation("UpdateUser")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Bad Request.")]
    [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponse), description: "The user is not authorized")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "The user not found for given id.")]
    [SwaggerResponse(statusCode: 409, type: typeof(ErrorResponse), description: "User already exists.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error")]
    public virtual IActionResult UpdateUser([FromRoute(Name = "user-id")][Required] Guid userId, [FromBody] UserDto userDto)
    {
        _logger.LogDebug("Update user details");
        _userService.UpdateUserDetails(userId, userDto);
        return StatusCode(200);
    }

    /// <summary>
    /// Delete the user.
    /// </summary>
    /// <remarks>Delete the user details for the given user id.</remarks>
    /// <param name="userId">Id of the user.</param>
    /// <response code="200">User deleted successfully.</response>
    /// <response code="401">The user is not authorized</response>
    /// <response code="404">The user not found for given id.</response>
    /// <response code="500">Internal server error</response>
    [HttpDelete]
    [Route("/api/user/{user-id}")]
    [Authorize]
    [ValidateModelState]
    [SwaggerOperation("DeleteUser")]
    [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponse), description: "The user is not authorized")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "The user not found for given id.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error")]
    public virtual IActionResult DeleteUser([FromRoute(Name = "user-id")][Required] Guid userId)
    {
        _logger.LogDebug("Deleting the user");
        _userService.DeleteUser(userId);
        return StatusCode(204);
    }
}