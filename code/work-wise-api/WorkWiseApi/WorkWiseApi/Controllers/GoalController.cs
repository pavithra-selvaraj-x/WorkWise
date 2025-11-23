using System.ComponentModel.DataAnnotations;
using Contracts;
using Contracts.IServices;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WorkWiseApi.Attributes;

namespace WorkWiseApi.Controllers;

/// <summary>
/// Class <c>UserController</c> for managing the goal operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class GoalController : ControllerBase
{
    private readonly ILoggerManager _logger;

    private readonly IGoalManagementService _goalManagementService;

    /// <summary>
    /// Constructor for inject the dependencies.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="goalService"></param>
    public GoalController(ILoggerManager logger, IGoalManagementService goalService)
    {
        _logger = logger;
        _goalManagementService = goalService;
    }

    /// <summary>
    /// Get goal suggestions based on the provided details.
    /// </summary>
    /// <remarks>Get suggestions for goals based on the provided details.</remarks>
    /// <param name="goalDetailsDto">The GoalDetailsDto object with details.</param>
    /// <response code="200">Goal suggestions retrieved successfully.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [Route("/api/goal/get-suggestions")]
    [ValidateModelState]
    [Authorize]
    [SwaggerOperation("GetGoalSuggestions")]
    [SwaggerResponse(statusCode: 200, type: typeof(ResponseDto), description: "Goal suggestions retrieved successfully.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Bad Request.")]
    [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponse), description: "The user is not authorized.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error")]
    public IActionResult GetGoalSuggestions([FromBody] GoalDetailsDto goalDetailsDto)
    {
        _logger.LogDebug("Get goal suggestions for given details");
        GoalSuggestionsDto goalSuggestions = _goalManagementService.GetGoalSuggestions(goalDetailsDto).Result;
        return StatusCode(200, goalSuggestions);
    }

    /// <summary>
    /// Create goal.
    /// </summary>
    /// <remarks>Create goal with the given input.</remarks>
    /// <param name="goalDto">The GoalDto object with goal details.</param>
    /// <response code="201">Goal created successfully.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [Route("/api/goal")]
    [ValidateModelState]
    [Authorize]
    [SwaggerOperation("CreateGoal")]
    [SwaggerResponse(statusCode: 201, type: typeof(ResponseDto), description: "Goals created successfully.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Bad Request.")]
    [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponse), description: "The user is not authorized.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error")]
    public IActionResult CreateGoal([FromBody] GoalDto goalDto)
    {
        _logger.LogDebug("Create goal for given details");
        Guid loggedInUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        ResponseDto response = _goalManagementService.CreateGoal(goalDto, loggedInUserId);
        return StatusCode(201, response);
    }

    /// <summary>
    /// Delete a goal by ID.
    /// </summary>
    /// <param name="goalId">ID of the goal to be deleted.</param>
    [HttpDelete]
    [Route("/api/goal/{goal-id}")]
    [Authorize]
    [SwaggerOperation("DeleteGoal")]
    [SwaggerResponse(statusCode: 204, description: "Goal deleted successfully.")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "Goal not found.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error")]
    public IActionResult DeleteGoal([FromRoute(Name = "goal-id")][Required] Guid goalId)
    {
        _logger.LogDebug($"Delete goal with ID: {goalId}");
        Guid loggedInUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

        // Call the service to delete the goal
        _goalManagementService.DeleteGoal(goalId, loggedInUserId);

        return StatusCode(204);
    }

    /// <summary>
    /// Update Goal By Id.
    /// </summary>
    /// <remarks>Update the goal with the given input.</remarks>
    /// <param name="goalId">ID of the goal to be updated.</param>
    /// <param name="goalDto">The GoalDto object with goal details.</param>
    /// <response code="200">Goal updated successfully.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="500">Internal server error</response>
    [HttpPut]
    [Route("/api/goal/{goal-id}")]
    [ValidateModelState]
    [Authorize]
    [SwaggerOperation("UpdateGoal")]
    [SwaggerResponse(statusCode: 200, type: typeof(ResponseDto), description: "Goal updated successfully.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Bad Request.")]
    [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponse), description: "The user is not authorized.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error")]
    public IActionResult UpdateGoal([FromRoute(Name = "goal-id")][Required] Guid goalId, [FromBody] GoalDto goalDto)
    {
        _logger.LogDebug($"Update goal for Id: {goalId}");
        Guid loggedInUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

        // Call the service to update the goal
        _goalManagementService.UpdateGoal(goalId, goalDto, loggedInUserId);

        return StatusCode(200);
    }

    /// <summary>
    /// Get all goals for the user.
    /// </summary>
    /// <remarks>Retrieves all goals for the currently authenticated user.</remarks>
    /// <response code="200">Returns all goals for the user.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="500">Internal server error</response>
    [HttpGet]
    [Route("/api/goal")]
    [Authorize]
    [SwaggerOperation("GetAllGoals")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<GoalDto>), description: "All goals retrieved successfully.")]
    [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponse), description: "The user is not authorized.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error")]
    public IActionResult GetAllGoals()
    {
        Guid loggedInUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        _logger.LogDebug($"Get all goals for the user ${loggedInUserId}");
        List<GoalDto> goals = _goalManagementService.GetAllGoals(loggedInUserId);
        return StatusCode(200, goals);
    }

    /// <summary>
    /// Get goal details by goalId.
    /// </summary>
    /// <remarks>Retrieves the details of a goal by its unique identifier.</remarks>
    /// <param name="goalId">The unique identifier of the goal.</param>
    /// <response code="200">Returns the goal details.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="404">Goal not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet]
    [Route("/api/goal/{goal-id}")]
    [Authorize]
    [SwaggerOperation("GetGoalDetails")]
    [SwaggerResponse(statusCode: 200, type: typeof(GoalDto), description: "Goal details retrieved successfully.")]
    [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponse), description: "The user is not authorized.")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "Goal not found.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error.")]
    public IActionResult GetGoalDetails([FromRoute(Name = "goal-id")][Required] Guid goalId)
    {
        Guid loggedInUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        _logger.LogDebug($"Get goal details for the user {loggedInUserId} and goalId {goalId}");

        // Call the service to get the goal details by goalId
        GoalDto goal = _goalManagementService.GetGoalById(goalId, loggedInUserId);

        // Return the goal details
        return StatusCode(200, goal);
    }
}

