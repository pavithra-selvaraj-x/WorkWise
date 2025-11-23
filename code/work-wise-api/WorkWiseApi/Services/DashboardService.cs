using Contracts;
using Contracts.IRepository;
using Contracts.IServices;
using Entities.Dtos;
using Entities.Enums;
using MyExtensions;
using Newtonsoft.Json;

namespace Services;

public class DashboardService : IDashboardService
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryWrapper _repoWrapper;

    private readonly IGoalManagementService _goalManagementService;

    /// <summary>
    /// Constructor for injecting repoWrapper, logger
    /// </summary>
    /// <param name="repoWrapper">The repoWrapper object</param>
    /// <param name="logger">The logger object</param>
    public DashboardService(IRepositoryWrapper repoWrapper, ILoggerManager logger, IGoalManagementService goalManagementService)
    {
        _repoWrapper = repoWrapper;
        _logger = logger;
        _goalManagementService = goalManagementService;
    }

    /// <summary>
    /// Retrieve dashboard insights.
    /// </summary>
    /// <param name="userId">The Id of the user for whom the insights are retrieved.</param>
    /// <returns>The DashboardInsightsDto object containing insights.</returns>
    public DashboardInsightsDto GetDashboardInsights(Guid userId)
    {
        _logger.LogInfo($"Retrieving dashboard insights for user with ID: {userId}");

        // Retrieve goal insights
        int totalGoalsCount = _repoWrapper.Goal.FindCountByCondition((g) => g.UserId.Equals(userId) && g.IsActive);

        List<CountPercentageDto> goalStatusCounts = _repoWrapper.Goal
                                                        .FindByCondition(g => g.UserId == userId && g.IsActive)
                                                        .GroupBy(g => g.Status)
                                                        .Select(group => new CountPercentageDto
                                                        {
                                                            Category = group.Key.ToString(),
                                                            Count = group.Count(),
                                                            Percentage = CalculatePercentage(group.Count(), totalGoalsCount)
                                                        })
                                                        .ToList();

        List<CountPercentageDto> goalPriorityCounts = _repoWrapper.Goal
                                                        .FindByCondition(g => g.UserId == userId && g.IsActive)
                                                        .GroupBy(g => g.Priority)
                                                        .Select(group => new CountPercentageDto
                                                        {
                                                            Category = group.Key.ToString(),
                                                            Count = group.Count(),
                                                            Percentage = CalculatePercentage(group.Count(), totalGoalsCount)
                                                        })
                                                        .ToList();

        var goalInsights = new GoalInsightsDto
        {
            TotalGoals = totalGoalsCount,
            GoalStatusCounts = goalStatusCounts,
            GoalPriorityCounts = goalPriorityCounts
        };

        // Retrieve task insights
        int totalTasksCount = _repoWrapper.Task.FindCountByCondition((t) => t.UserId.Equals(userId) && t.IsActive);
        int totalIndividialTasksCount = _repoWrapper.Task.FindCountByCondition((t) => t.UserId.Equals(userId) && t.IsActive && t.TaskType.Equals(TaskType.Independent));
        int totalIndividualTasksPercentage = CalculatePercentage(totalIndividialTasksCount, totalTasksCount);
        int totalGoalTasksCount = _repoWrapper.Task.FindCountByCondition((t) => t.UserId.Equals(userId) && t.IsActive && t.TaskType.Equals(TaskType.GoalRelated));
        int totalGoalTasksPercentage = CalculatePercentage(totalGoalTasksCount, totalTasksCount);

        var taskInsights = new TaskInsightsDto
        {
            TotalTasks = totalTasksCount,
            IndividualTasksCount = totalIndividialTasksCount,
            IndividualTasksPercentage = totalIndividualTasksPercentage,
            IndividualTaskStatusCounts = GetTaskStatusCounts(userId, TaskType.Independent, totalIndividialTasksCount),
            IndividualTaskPriorityCounts = GetTaskPriorityCounts(userId, TaskType.Independent, totalIndividialTasksCount),
            GoalRelatedTasksCount = totalGoalTasksCount,
            GoalRelatedTasksPercentage = totalGoalTasksPercentage,
            GoalRelatedTaskStatusCounts = GetTaskStatusCounts(userId, TaskType.GoalRelated, totalGoalTasksCount),
            GoalRelatedTaskPriorityCounts = GetTaskPriorityCounts(userId, TaskType.GoalRelated, totalGoalTasksCount),
        };

        var dashboardInsights = new DashboardInsightsDto
        {
            GoalInsights = goalInsights,
            TaskInsights = taskInsights
        };

        _logger.LogInfo($"Retrieved dashboard insights for user with ID: {userId}");
        return dashboardInsights;
    }

    /// <summary>
    /// Calculate percentage based on given numerator and denominator.
    /// </summary>
    /// <param name="numerator">The numerator value.</param>
    /// <param name="denominator">The denominator value.</param>
    /// <returns>The calculated percentage as an integer. Returns 0 if either numerator or denominator is 0.</returns>
    static private int CalculatePercentage(int numerator, int denominator)
    {
        if (denominator == 0 || numerator == 0)
        {
            return 0;
        }

        return (int)((double)numerator / denominator * 100);
    }

    private List<CountPercentageDto> GetTaskStatusCounts(Guid userId, TaskType taskType, int totalCount)
    {
        List<CountPercentageDto> taskStatusCounts = _repoWrapper.Task
            .FindByCondition(task => task.UserId == userId && task.IsActive && task.TaskType.Equals(taskType))
            .GroupBy(task => task.Status)
            .Select(group => new CountPercentageDto
            {
                Category = EnumExtensions.GetEnumMemberValue<Status>(group.Key),
                Count = group.Count(),
                Percentage = CalculatePercentage(group.Count(), totalCount)
            })
            .ToList();
        var statusValues = EnumExtensions.GetEnumMemberValues<Status>();
        foreach (string status in statusValues)
        {
            if (!taskStatusCounts.Exists(t => t.Category.Equals(status)))
            {
                CountPercentageDto emptyObject = new CountPercentageDto
                {
                    Category = status,
                    Count = 0,
                    Percentage = 0
                };
                taskStatusCounts.Add(emptyObject);
            }
        }
        return taskStatusCounts;
    }

    private List<CountPercentageDto> GetTaskPriorityCounts(Guid userId, TaskType taskType, int totalCount)
    {
        List<CountPercentageDto> taskPriorityCounts = _repoWrapper.Task
            .FindByCondition(task => task.UserId == userId && task.IsActive && task.TaskType.Equals(taskType))
            .GroupBy(task => task.Priority)
            .Select(group => new CountPercentageDto
            {
                Category = EnumExtensions.GetEnumMemberValue<Priority>(group.Key),
                Count = group.Count(),
                Percentage = CalculatePercentage(group.Count(), totalCount)
            })
            .ToList();
        var priorityValues = EnumExtensions.GetEnumMemberValues<Priority>();
        foreach (string priority in priorityValues)
        {
            if (!taskPriorityCounts.Exists(t => t.Category.Equals(priority)))
            {
                CountPercentageDto emptyObject = new CountPercentageDto
                {
                    Category = priority,
                    Count = 0,
                    Percentage = 0
                };
                taskPriorityCounts.Add(emptyObject);
            }
        }
        return taskPriorityCounts;
    }

    /// <summary>
    /// Retrieve dashboard info.
    /// </summary>
    /// <param name="userId">The Id of the user for whom the info are retrieved.</param>
    /// <returns>The DashboardInfoDto object containing info.</returns>
    public async Task<DashboardInfoDto> GetDashboardInfo(Guid userId)
    {
        _logger.LogInfo($"Retrieving dashboard info for user with ID: {userId}");

        try
        {
            DashboardInsightsDto dashboardInsights = GetDashboardInsights(userId);
            int openGoalsCount = _repoWrapper.Goal.FindCountByCondition((g) => g.IsActive && g.Status.Equals(Status.Open) && g.UserId.Equals(userId));
            string prompt = $"You are developing an application for goal management, and a user have some goal and tasks tasks created following the SMART technique. Here's the goal and task information:\n\n" +
                            $"Total number of goals: {dashboardInsights.GoalInsights.TotalGoals}\n" +
                            $"Total number of goals open: {openGoalsCount}\n" +
                            $"Total number of tasks: {dashboardInsights.TaskInsights.TotalTasks}\n\n" +
                            $"Based on this information, your task is to generate a list of insights and information that can be shown to the users to know more about the progress and how they can imporve.\n\n" +
                            $"The response should be a json object with the below properies that can be parsed easily\n" +
                            $"- Insights: A list of minimum 3 strings with insights\n" +
                            $"- Motivations: A list of minumum 3 strings with creative motivationfor the user\n";
            GeminiResponseDto result = await _goalManagementService.GetResponseFromGenAIModel(prompt);

            // Access text part
            string text = result.Candidates[0].Content.Parts[0].Text;
            string jsonText = text.Replace("```json", "").Replace("```", "");
            DashboardInfoDto dashboardInfo = JsonConvert.DeserializeObject<DashboardInfoDto>(jsonText);
            return dashboardInfo;
        }
        catch (Exception)
        {
            _logger.LogError("Exception occurred in GetDashboardInfo");
            throw;
        }
    }
}