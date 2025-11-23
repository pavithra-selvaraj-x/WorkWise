export interface DashboardInsightsDto {
  goal_insights: GoalInsightsDto;
  task_insights: TaskInsightsDto;
}

export interface GoalInsightsDto {
  total_goals: number;
  goal_status_counts: CountPercentageDto[];
  goal_priority_counts: CountPercentageDto[];
}

export interface TaskInsightsDto {
  total_tasks: number;
  individual_tasks_count: number;
  individual_tasks_percentage: number;
  individual_task_status_counts: CountPercentageDto[];
  individual_task_priority_counts: CountPercentageDto[];
  goal_related_tasks_count: number;
  goal_related_tasks_percentage: number;
  goal_related_task_status_counts: CountPercentageDto[];
  goal_related_task_priority_counts: CountPercentageDto[];
}

export interface CountPercentageDto {
  category: string;
  count: number;
  percentage: number;
}

export interface DashboardInfoDto {
  insights: string[];
  motivations: string[];
}
