using System.Runtime.Serialization;

namespace Entities.Dtos;

/// <summary>
/// Dashboard insights DTO containing goal and task insights.
/// </summary>
[DataContract]
public class DashboardInsightsDto
{
    /// <summary>
    /// Goal insights.
    /// </summary>
    [DataMember(Name = "goal_insights")]
    public GoalInsightsDto GoalInsights { get; set; }

    /// <summary>
    /// Task insights.
    /// </summary>
    [DataMember(Name = "task_insights")]
    public TaskInsightsDto TaskInsights { get; set; }
}

/// <summary>
/// Goal insights DTO containing total goals, goal status counts, and goal priority counts.
/// </summary>
[DataContract]
public class GoalInsightsDto
{
    /// <summary>
    /// Total number of goals.
    /// </summary>
    [DataMember(Name = "total_goals")]
    public int TotalGoals { get; set; }

    /// <summary>
    /// List of goal status counts.
    /// </summary>
    [DataMember(Name = "goal_status_counts")]
    public List<CountPercentageDto> GoalStatusCounts { get; set; }

    /// <summary>
    /// List of goal priority counts.
    /// </summary>
    [DataMember(Name = "goal_priority_counts")]
    public List<CountPercentageDto> GoalPriorityCounts { get; set; }
}

/// <summary>
/// DTO for task insights, containing information about total tasks, individual tasks, and goal-related tasks.
/// </summary>
[DataContract]
public class TaskInsightsDto
{
    /// <summary>
    /// Total number of tasks.
    /// </summary>
    [DataMember(Name = "total_tasks")]
    public int TotalTasks { get; set; }

    /// <summary>
    /// Number of individual tasks.
    /// </summary>
    [DataMember(Name = "individual_tasks_count")]
    public int IndividualTasksCount { get; set; }

    /// <summary>
    /// Percentage of individual tasks.
    /// </summary>
    [DataMember(Name = "individual_tasks_percentage")]
    public int IndividualTasksPercentage { get; set; }

    /// <summary>
    /// List of individual task status counts.
    /// </summary>
    [DataMember(Name = "individual_task_status_counts")]
    public List<CountPercentageDto> IndividualTaskStatusCounts { get; set; }

    /// <summary>
    /// List of individual task priority counts.
    /// </summary>
    [DataMember(Name = "individual_task_priority_counts")]
    public List<CountPercentageDto> IndividualTaskPriorityCounts { get; set; }

    /// <summary>
    /// Number of goal-related tasks.
    /// </summary>
    [DataMember(Name = "goal_related_tasks_count")]
    public int GoalRelatedTasksCount { get; set; }

    /// <summary>
    /// Percentage of goal-related tasks.
    /// </summary>
    [DataMember(Name = "goal_related_tasks_percentage")]
    public int GoalRelatedTasksPercentage { get; set; }

    /// <summary>
    /// List of goal-related task status counts.
    /// </summary>
    [DataMember(Name = "goal_related_task_status_counts")]
    public List<CountPercentageDto> GoalRelatedTaskStatusCounts { get; set; }

    /// <summary>
    /// List of goal-related task priority counts.
    /// </summary>
    [DataMember(Name = "goal_related_task_priority_counts")]
    public List<CountPercentageDto> GoalRelatedTaskPriorityCounts { get; set; }
}

/// <summary>
/// DTO for count percentage, containing category, count, and percentage.
/// </summary>
[DataContract]
public class CountPercentageDto
{
    /// <summary>
    /// Category of the count percentage.
    /// </summary>
    [DataMember(Name = "category")]
    public string Category { get; set; }

    /// <summary>
    /// Count value.
    /// </summary>
    [DataMember(Name = "count")]
    public int Count { get; set; }

    /// <summary>
    /// Percentage value.
    /// </summary>
    [DataMember(Name = "percentage")]
    public double Percentage { get; set; }
}