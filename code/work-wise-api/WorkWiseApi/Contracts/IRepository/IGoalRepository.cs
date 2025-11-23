using Entities.Dtos;
using Entities.Models;

namespace Contracts.IRepository
{
    /// <summary>
    /// Interface <c>IGoalRepository</c> used to define the methods related to goal.
    /// </summary>
    public interface IGoalRepository : IRepositoryBase<Goal>
    {
        /// <summary>
        /// Finds a goal by both the goal ID and the user ID.
        /// </summary>
        /// <param name="goalId">The unique identifier for the goal.</param>
        /// <param name="userId">The unique identifier for the user associated with the goal.</param>
        /// <returns>The goal matching the provided goal ID and user ID.</returns>
        Goal FindGoalByIdAndUserId(Guid goalId, Guid userId);

        /// <summary>
        /// Retrieve all goals for a specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of GoalDto representing goals belonging to the user.</returns>
        List<GoalDto> GetAllGoalsByUserId(Guid userId);

        /// <summary>
        /// Get goalby Id for a specified user.
        /// </summary>
        /// <param name="goalId">The unique identifier of the goal.</param>
        /// <param name="loggedInUserId">The unique identifier of the user.</param>
        /// <returns>The GoalDto object</returns>
        GoalDto? GetGoalById(Guid goalId, Guid loggedInUserId);
    }
}