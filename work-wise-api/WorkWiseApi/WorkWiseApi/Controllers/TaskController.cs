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
/// Class <c>UserController</c> for managing the task operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ILoggerManager _logger;

    private readonly ITaskManagementService _taskManagementService;

    /// <summary>
    /// Constructor for inject the dependencies.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="taskService"></param>
    public TaskController(ILoggerManager logger, ITaskManagementService taskService)
    {
        _logger = logger;
        _taskManagementService = taskService;
    }

    /// <summary>
    /// Get tasks.
    /// </summary>
    /// <remarks>Get a list of tasks.</remarks>
    /// <response code="200">List of tasks retrieved successfully.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="500">Internal server error</response>
    [HttpGet]
    [Route("/api/tasks")]
    [Authorize]
    [SwaggerOperation("GetTasks")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<TaskDto>), description: "List of tasks retrieved successfully.")]
    [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponse), description: "The user is not authorized.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error")]
    public IActionResult GetTasks()
    {
        _logger.LogDebug("Get all tasks for the user");

        // Call the service to retrieve tasks
        Guid loggedInUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        var tasks = _taskManagementService.GetAllIndividualTasks(loggedInUserId);

        return StatusCode(200, tasks);
    }

    /// <summary>
    /// Create tasks.
    /// </summary>
    /// <remarks>Create tasks with the given input.</remarks>
    /// <param name="taskDtos">List of TaskDto objects with task details.</param>
    /// <response code="201">Tasks created successfully.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [Route("/api/tasks")]
    [ValidateModelState]
    [Authorize]
    [SwaggerOperation("CreateTasks")]
    [SwaggerResponse(statusCode: 201, type: typeof(ResponseDto), description: "Tasks created successfully.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Bad Request.")]
    [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponse), description: "The user is not authorized.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error")]
    public IActionResult CreateTasks([FromBody] List<TaskDto> taskDtos)
    {
        _logger.LogDebug("Create tasks for given details");

        // Call the service to create the tasks
        Guid loggedInUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        _taskManagementService.CreateTasks(taskDtos, loggedInUserId);

        return StatusCode(201);
    }

    /// <summary>
    /// Delete a task by ID.
    /// </summary>
    /// <param name="taskId">ID of the task to be deleted.</param>
    /// <returns>Response status.</returns>
    [HttpDelete]
    [Route("/api/task/{task-id}")]
    [Authorize]
    [SwaggerOperation("DeleteTask")]
    [SwaggerResponse(statusCode: 204, description: "Task deleted successfully.")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "Task not found.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error")]
    public IActionResult DeleteTask([FromRoute(Name = "task-id")][Required] Guid taskId)
    {
        _logger.LogDebug($"Delete task with ID: {taskId}");

        // Call the service to delete the task
        _taskManagementService.DeleteTask(taskId);

        return StatusCode(204);
    }

    /// <summary>
    /// Update Task By Id.
    /// </summary>
    /// <remarks>Update the task with the given input.</remarks>
    /// <param name="taskId">ID of the task to be deleted.</param>
    /// <param name="taskDto">The TaskDto object with task details.</param>
    /// <response code="200">Task updated successfully.</response>
    /// <response code="400">Bad Request.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="500">Internal server error</response>
    [HttpPut]
    [Route("/api/task/{task-id}")]
    [ValidateModelState]
    [Authorize]
    [SwaggerOperation("UpdateTask")]
    [SwaggerResponse(statusCode: 200, type: typeof(ResponseDto), description: "Task updated successfully.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Bad Request.")]
    [SwaggerResponse(statusCode: 401, type: typeof(ErrorResponse), description: "The user is not authorized.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Internal server error")]
    public IActionResult CreateTasks([FromRoute(Name = "task-id")][Required] Guid taskId, [FromBody] TaskDto taskDto)
    {
        _logger.LogDebug($"Update task for Id: {taskId}");

        // Call the service to update the task
        _taskManagementService.UpdateTask(taskId, taskDto);

        return StatusCode(200);
    }

}