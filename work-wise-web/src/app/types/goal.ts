import { Priority, Status, TaskType } from "./enumerations";

export interface GoalDetailsDto {
  goal: string;
  importance?: string;
  time_frame: number;
  time_per_week: number;
}

export interface GoalSuggestionsDto {
  tasks: GoalTask[];
}

export interface GoalTask {
  title: string;
  description: string;
  priority: string;
  deadline: string;
}

export interface Task {
  title: string;
  description?: string;
  startDate?: Date;
  endDate?: Date;
  dueDate?: Date;
  priority: Priority;
}

export interface GoalDto {
  id?: string;
  title: string;
  description?: string;
  start_date?: string;
  end_date?: string;
  status: Status;
  priority: Priority;
  progress: number;
  task_list?: TaskDto[];
}

export interface TaskDto {
  id?: string;
  user_id: string;
  goal_id?: string;
  title: string;
  description?: string;
  start_date?: Date;
  end_date?: Date;
  due_date?: Date;
  status: Status;
  priority: Priority;
  task_type: TaskType;
}
