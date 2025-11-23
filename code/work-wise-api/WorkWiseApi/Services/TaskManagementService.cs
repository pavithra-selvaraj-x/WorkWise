using Contracts;
using Contracts.IRepository;
using Contracts.IServices;
using Entities.Dtos;
using ExceptionHandler;
using Microsoft.Extensions.Configuration;

namespace Services;

public class TaskManagementService : ITaskManagementService
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryWrapper _repoWrapper;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructor for injecting mapper, repoWrapper, logger
    /// </summary>
    /// <param name="repoWrapper">The repoWrapper object</param>
    /// <param name="logger">The logger object</param>
    /// <param name="configuration"></param>
    public TaskManagementService(IRepositoryWrapper repoWrapper, ILoggerManager logger, IConfiguration configuration)
    {
        _repoWrapper = repoWrapper;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Get all individual tasks for the specified user.
    /// </summary>
    /// <param name="userId">The id of the user.</param>
    /// <returns>A list of goal DTOs.</returns>
    public List<TaskDto> GetAllIndividualTasks(Guid loggedInUserId)
    {
        _logger.LogDebug($"Getting individual tasks for userId: ${loggedInUserId}");
        return _repoWrapper.Task.GetAllIndividualTasksByUserId(loggedInUserId);
    }

    /// <summary>
    /// Create tasks.
    /// </summary>
    /// <param name="taskDtos">List of TaskDto objects with task details.</param>
    /// <param name="loggedInUserId">The id of the user.</param>
    /// <returns>Response status.</returns>
    public void CreateTasks(List<TaskDto> taskDtos, Guid loggedInUserId)
    {
        _logger.LogDebug("Create tasks for given details");
        List<Entities.Models.Task> tasks = new List<Entities.Models.Task>();
        foreach (var taskDto in taskDtos)
        {
            Entities.Models.Task task = new Entities.Models.Task()
            {
                UserId = loggedInUserId,
                Title = taskDto.Title,
                Description = taskDto.Description,
                StartDate = taskDto.StartDate,
                EndDate = taskDto.EndDate,
                DueDate = taskDto.DueDate,
                Status = taskDto.Status,
                Priority = taskDto.Priority,
                TaskType = taskDto.TaskType,
                GoalId = taskDto.GoalId
            };
            tasks.Add(task);
        }
        _repoWrapper.Task.CreateBatch(tasks);
        _repoWrapper.Save();
        _logger.LogDebug("Tasks created successfully for given details");
    }

    /// <summary>
    /// Delete a task by ID.
    /// </summary>
    /// <param name="taskId">ID of the task to be deleted.</param>
    public void DeleteTask(Guid taskId)
    {
        _logger.LogDebug($"Delete task with ID: {taskId}");

        var task = _repoWrapper.Task.FindFirstByCondition(t => t.Id == taskId && t.IsActive);

        if (task == null)
        {
            _logger.LogError("Task not found for the given Id");
            throw new NotFoundCustomException("Task not found", "Task not found for the given Id");
        }

        task.IsActive = false;
        _repoWrapper.Task.Update(task);
        _repoWrapper.Save();
    }

    /// <summary>
    /// Update the task by ID.
    /// </summary>
    /// <param name="taskId">ID of the task to be deleted.</param>
    /// <param name="taskDtos">List of TaskDto objects with task details.</param>
    public void UpdateTask(Guid taskId, TaskDto taskDto)
    {
        _logger.LogDebug($"Update task with ID: {taskId}");

        var task = _repoWrapper.Task.FindFirstByCondition(t => t.Id == taskId && t.IsActive);

        if (task == null)
        {
            _logger.LogError("Task not found for the given Id");
            throw new NotFoundCustomException("Task not found", "Task not found for the given Id");
        }

        task.Title = taskDto.Title;
        task.Description = taskDto.Description;
        task.StartDate = taskDto.StartDate;
        task.EndDate = taskDto.EndDate;
        task.DueDate = taskDto.DueDate;
        task.Status = taskDto.Status;
        task.Priority = taskDto.Priority;
        task.TaskType = taskDto.TaskType;

        _repoWrapper.Task.Update(task);
        _repoWrapper.Save();
    }
}