using Entities.Dtos;

namespace Contracts.IServices;

/// <summary>
/// Interface for task management service.
/// </summary>
public interface ITaskManagementService
{
    /// <summary>
    /// Get all individual tasks for the specified user.
    /// </summary>
    /// <param name="userId">The id of the user.</param>
    /// <returns>A list of goal DTOs.</returns>
    List<TaskDto> GetAllIndividualTasks(Guid loggedInUserId);

    /// <summary>
    /// Create tasks.
    /// </summary>
    /// <param name="taskDtos">List of TaskDto objects with task details.</param>\
    /// <param name="loggedInUserId">The id of the user.</param>
    /// <returns>Response status.</returns>
    void CreateTasks(List<TaskDto> taskDtos, Guid loggedInUserId);

    /// <summary>
    /// Delete a task by ID.
    /// </summary>
    /// <param name="taskId">ID of the task to be deleted.</param>
    void DeleteTask(Guid taskId);

    /// <summary>
    /// Update the task by ID.
    /// </summary>
    /// <param name="taskId">ID of the task to be deleted.</param>
    /// <param name="taskDtos">List of TaskDto objects with task details.</param>
    void UpdateTask(Guid taskId, TaskDto taskDto);
}