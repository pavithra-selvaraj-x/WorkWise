import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import Chart from 'chart.js/auto';
import { PageLoaderComponent } from '../../components/page-loader/page-loader.component';
import { AuthService } from '../../services/auth-service/auth.service';
import { DashboardService } from '../../services/dashboard-service/dashboard.service';
import { ToastService } from '../../services/toast-service/toast.service';
import { DashboardInfoDto, DashboardInsightsDto } from '../../types/dashboard';

@Component({
  selector: 'ww-dashboard',
  standalone: true,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
  imports: [CommonModule, PageLoaderComponent],
})
export class DashboardComponent {
  isLoading = false;
  dashboardInsights!: DashboardInsightsDto;
  dashboardInfo!: DashboardInfoDto;

  goalStatusChartData: number[] = [];
  goalStatusChartLabels: string[] = [];
  goalStatusChart: any;
  taskStatusChart: any;

  constructor(
    public authService: AuthService,
    private dashboardService: DashboardService,
    private toastService: ToastService
  ) {
    this.getDashboardInsights();
    this.getDashboardInfo();
  }

  getDashboardInsights() {
    this.isLoading = true;
    this.dashboardService.getDashboardInsights().subscribe({
      next: (response) => {
        this.dashboardInsights = response as DashboardInsightsDto;
        this.isLoading = false;
        setTimeout(() => {
          this.createGoalStatusChart();
          this.createTaskStatusChart();
          this.createPriorityChart();
        }, 15);
      },
      error: ({ error }) => {
        this.isLoading = false;
        this.toastService.showError(error.description);
      },
    });
  }

  getDashboardInfo() {
    this.dashboardService.getDashboardInfo().subscribe({
      next: (response) => {
        this.dashboardInfo = response;
      },
      error: ({ error }) => {},
    });
  }

  createGoalStatusChart() {
    const canvas: HTMLCanvasElement | null = document.getElementById(
      'goal-status-chart'
    ) as HTMLCanvasElement;
    const goalStatusChartData =
      this.dashboardInsights.goal_insights.goal_status_counts.map(
        (s) => s.count
      );
    const goalStatusChartLabels =
      this.dashboardInsights.goal_insights.goal_status_counts.map(
        (s) => s.category
      );
    const isNoGoal = goalStatusChartData.reduce((a, b) => a + b, 0) === 0;
    if (canvas) {
      const context = canvas.getContext('2d');
      if (context) {
        this.goalStatusChart = new Chart(context, {
          type: 'doughnut',
          data: {
            labels: goalStatusChartLabels,
            datasets: [
              {
                data: isNoGoal ? [100] : goalStatusChartData,
                backgroundColor: isNoGoal
                  ? ['#ccc']
                  : ['#3e8c7e', '#f2bbc5', '#f27329', '#698c86', '#bfa8ade'],
              },
            ],
          },
          options: {
            plugins: {
              legend: {
                display: true,
                position: 'top',
                align: 'start',
                labels: {
                  color: '#000000',
                  font: {
                    size: 14,
                  },
                },
              },
            },
          },
        });
      } else {
        console.error('Failed to get 2D context from canvas element.');
      }
    } else {
      console.error('Canvas element with ID "chart" not found.');
    }
  }

  createTaskStatusChart() {
    const canvas: HTMLCanvasElement | null = document.getElementById(
      'task-status-chart'
    ) as HTMLCanvasElement;
    const individualTaskStatusChartData =
      this.dashboardInsights.task_insights.individual_task_status_counts.map(
        (s) => s.count
      );
    const goalTaskStatusChartData =
      this.dashboardInsights.task_insights.goal_related_task_status_counts.map(
        (s) => s.count
      );
    const taskStatusChartLabels =
      this.dashboardInsights.task_insights.individual_task_status_counts.map(
        (s) => s.category
      );
    if (canvas) {
      const context = canvas.getContext('2d');
      if (context) {
        this.taskStatusChart = new Chart(context, {
          type: 'bar',
          data: {
            labels: taskStatusChartLabels,
            datasets: [
              {
                label: 'Individual Tasks',
                data: individualTaskStatusChartData,
                backgroundColor: '#f2bbc5',
                stack: 'Stack 0',
              },
              {
                label: 'Goal Tasks',
                data: goalTaskStatusChartData,
                backgroundColor: '#bfa8ad',
                stack: 'Stack 0',
              },
            ],
          },
          options: {
            plugins: {
              legend: {
                display: true,
                position: 'top',
                align: 'start',
                labels: {
                  color: '#000000',
                  font: {
                    size: 14,
                  },
                },
              },
            },
            responsive: true,
            interaction: {
              intersect: false,
            },
            scales: {
              x: {
                stacked: true,
              },
              y: {
                stacked: true,
              },
            },
          },
        });
      } else {
        console.error('Failed to get 2D context from canvas element.');
      }
    } else {
      console.error('Canvas element with ID "chart" not found.');
    }
  }

  createPriorityChart() {
    const canvas: HTMLCanvasElement | null = document.getElementById(
      'priority-chart'
    ) as HTMLCanvasElement;
    const goalPriorityChartData =
      this.dashboardInsights.goal_insights.goal_priority_counts.map(
        (s) => s.count
      );
    const individualTaskPriorityChartData =
      this.dashboardInsights.task_insights.individual_task_priority_counts.map(
        (s) => s.count
      );
    const goalTaskPriorityChartData =
      this.dashboardInsights.task_insights.goal_related_task_priority_counts.map(
        (s) => s.count
      );
    const priorityChartLabels =
      this.dashboardInsights.task_insights.goal_related_task_priority_counts.map(
        (s) => s.category
      );
    if (canvas) {
      const context = canvas.getContext('2d');
      if (context) {
        this.taskStatusChart = new Chart(context, {
          type: 'bar',
          data: {
            labels: priorityChartLabels,
            datasets: [
              {
                label: 'Individual Task Priority',
                data: individualTaskPriorityChartData,
                backgroundColor: '#f8a586',
                stack: 'Stack 0',
              },
              {
                label: 'Goal Task Priority',
                data: goalTaskPriorityChartData,
                backgroundColor: '#bfa8ad',
                stack: 'Stack 0',
              },
              {
                label: 'Goal Priority',
                data: goalPriorityChartData,
                backgroundColor: '#89f6d6',
                stack: 'Stack 1',
              },
            ],
          },
          options: {
            plugins: {
              legend: {
                display: true,
                position: 'top',
                align: 'start',
                labels: {
                  color: '#000000',
                  font: {
                    size: 14,
                  },
                },
              },
            },
            responsive: true,
            interaction: {
              intersect: false,
            },
            scales: {
              x: {
                stacked: true,
              },
              y: {
                stacked: true,
              },
            },
          },
        });
      } else {
        console.error('Failed to get 2D context from canvas element.');
      }
    } else {
      console.error('Canvas element with ID "chart" not found.');
    }
  }
}
