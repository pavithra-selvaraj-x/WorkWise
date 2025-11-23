using Contracts.IRepository;
using Entities;
using Entities.Dtos;
using Entities.Enums;
using Entities.Models;

namespace Repository
{
    /// <summary>
    /// Class <c>GoalRepository</c> used to implement the methods related to task
    /// </summary>
    public class TaskRepository : RepositoryBase<Entities.Models.Task>, ITaskRepository
    {
        /// <summary>
        /// Constructor for injecting
        /// </summary>
        /// <param name="repositoryContext"></param>
        /// <returns></returns>
        public TaskRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        /// <summary>
        /// Finds tasks associated with a specific goal by the goal ID.
        /// </summary>
        /// <param name="goalId">The unique identifier for the goal.</param>
        /// <returns>A list of tasks related to the provided goal ID.</returns>
        public List<Entities.Models.Task> FindTasksByGoalId(Guid goalId)
        {
            return FindByCondition(t => t.GoalId == goalId && t.IsActive).ToList();
        }

        /// <summary>
        /// Retrieve all individual task for a specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of TaskDto representing individual tasks belonging to the user.</returns>
        public List<TaskDto> GetAllIndividualTasksByUserId(Guid userId)
        {
            return RepositoryContext.Tasks
                        .Where(t => t.UserId == userId && t.IsActive && t.TaskType.Equals(TaskType.Independent))
                        .Select(t => new TaskDto
                        {
                            Id = t.Id,
                            UserId = t.UserId,
                            GoalId = t.GoalId,
                            Title = t.Title,
                            Description = t.Description,
                            StartDate = t.StartDate,
                            EndDate = t.EndDate,
                            DueDate = t.DueDate,
                            Priority = t.Priority,
                            Status = t.Status,
                            TaskType = t.TaskType
                        })
                        .ToList();
        }
    }
}