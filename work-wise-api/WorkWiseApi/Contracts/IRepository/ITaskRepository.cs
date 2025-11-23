using Entities.Dtos;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Contracts.IRepository
{
    /// <summary>
    /// Interface <c>IGoalRepository</c> used to define the methods related to task.
    /// </summary>
    public interface ITaskRepository : IRepositoryBase<Entities.Models.Task>
    {
        /// <summary>
        /// Finds tasks associated with a specific goal by the goal ID.
        /// </summary>
        /// <param name="goalId">The unique identifier for the goal.</param>
        /// <returns>A list of tasks related to the provided goal ID.</returns>
        List<Entities.Models.Task> FindTasksByGoalId(Guid goalId);

        /// <summary>
        /// Retrieve all individual task for a specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of TaskDto representing individual tasks belonging to the user.</returns>
        List<TaskDto> GetAllIndividualTasksByUserId(Guid userId);
    }
}