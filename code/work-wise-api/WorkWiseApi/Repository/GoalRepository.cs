using Contracts.IRepository;
using Entities;
using Entities.Dtos;
using Entities.Enums;
using Entities.Models;

namespace Repository
{
    /// <summary>
    /// Class <c>GoalRepository</c> used to implement the methods related to goal
    /// </summary>
    public class GoalRepository : RepositoryBase<Goal>, IGoalRepository
    {
        /// <summary>
        /// Constructor for injecting 
        /// </summary>
        /// <param name="repositoryContext"></param>
        /// <returns></returns>
        public GoalRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public Goal FindGoalByIdAndUserId(Guid goalId, Guid userId)
        {
            return FindFirstByCondition(g => g.Id == goalId && g.UserId == userId && g.IsActive);
        }

        /// <summary>
        /// Retrieve all goals for a specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of GoalDto representing goals belonging to the user.</returns>
        public List<GoalDto> GetAllGoalsByUserId(Guid userId)
        {
            // Query goals from the database using LINQ
            List<GoalDto> goals = (from goal in RepositoryContext.Goals
                                   where goal.UserId == userId && goal.IsActive
                                   let totalTasks = RepositoryContext.Tasks.Count(task => task.GoalId == goal.Id && task.IsActive && goal.UserId == userId)
                                   let completedTasks = RepositoryContext.Tasks.Count(task => task.GoalId == goal.Id && task.Status.Equals(Status.Completed) && task.IsActive)
                                   select new GoalDto
                                   {
                                       Id = goal.Id,
                                       Title = goal.Title,
                                       Description = goal.Description,
                                       StartDate = goal.StartDate,
                                       EndDate = goal.EndDate,
                                       Status = goal.Status,
                                       Priority = goal.Priority,
                                       Progress = (decimal)(totalTasks == 0 ? 0 : (double)completedTasks / totalTasks * 100),
                                       TaskList = (from task in RepositoryContext.Tasks
                                                   where task.GoalId == goal.Id && task.IsActive
                                                   select new TaskDto
                                                   {
                                                       Id = task.Id,
                                                       UserId = task.UserId,
                                                       GoalId = task.GoalId,
                                                       Title = task.Title,
                                                       Description = task.Description,
                                                       StartDate = task.StartDate,
                                                       EndDate = task.EndDate,
                                                       DueDate = task.DueDate,
                                                       Status = task.Status,
                                                       Priority = task.Priority,
                                                       TaskType = task.TaskType
                                                   }).ToList()
                                   }).ToList();
            return goals;
        }

        /// <summary>
        /// Get goalby Id for a specified user.
        /// </summary>
        /// <param name="goalId">The unique identifier of the goal.</param>
        /// <param name="loggedInUserId">The unique identifier of the user.</param>
        /// <returns>The GoalDto object</returns>
        public GoalDto? GetGoalById(Guid goalId, Guid loggedInUserId)
        {
            GoalDto? goal = (from g in RepositoryContext.Goals
                             where g.UserId == loggedInUserId && g.Id == goalId && g.IsActive
                             let totalTasks = RepositoryContext.Tasks.Count(task => task.GoalId == g.Id && task.IsActive && g.UserId == loggedInUserId)
                             let completedTasks = RepositoryContext.Tasks.Count(task => task.GoalId == goalId && task.Status.Equals(Status.Completed) && task.IsActive)
                             select new GoalDto
                             {
                                 Id = g.Id,
                                 Title = g.Title,
                                 Description = g.Description,
                                 StartDate = g.StartDate,
                                 EndDate = g.EndDate,
                                 Status = g.Status,
                                 Priority = g.Priority,
                                 Progress = (decimal)(totalTasks == 0 ? 0 : (double)completedTasks / totalTasks * 100),
                                 TaskList = (from task in RepositoryContext.Tasks
                                             where task.GoalId == g.Id && task.IsActive
                                             select new TaskDto
                                             {
                                                 Id = task.Id,
                                                 UserId = task.UserId,
                                                 GoalId = task.GoalId,
                                                 Title = task.Title,
                                                 Description = task.Description,
                                                 StartDate = task.StartDate,
                                                 EndDate = task.EndDate,
                                                 DueDate = task.DueDate,
                                                 Status = task.Status,
                                                 Priority = task.Priority,
                                                 TaskType = task.TaskType
                                             }).ToList()
                             }).FirstOrDefault();
            return goal;
        }
    }
}