import { CommonModule, DatePipe } from '@angular/common';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Router, RouterModule } from '@angular/router';
import { PageLoaderComponent } from '../../components/page-loader/page-loader.component';
import { PageTitleBarComponent } from '../../components/page-title-bar/page-title-bar.component';
import { GoalService } from '../../services/goal-service/goal.service';
import { ToastService } from '../../services/toast-service/toast.service';
import { GoalDto } from '../../types/goal';

@Component({
  selector: 'ww-goal-management',
  standalone: true,
  templateUrl: './goal-management.component.html',
  styleUrl: './goal-management.component.scss',
  imports: [
    RouterModule,
    DatePipe,
    PageTitleBarComponent,
    MatIconModule,
    MatTableModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    CommonModule,
    PageLoaderComponent,
  ],
})
export class GoalManagementComponent {
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('deleteConfirmationDialog', { static: true })
  deleteConfirmationDialog!: TemplateRef<any>;

  isLoading = false;
  goalList!: MatTableDataSource<GoalDto[]>;
  displayedColumns: string[] = [
    'title',
    'description',
    'start_date',
    'end_date',
    'status',
    'priority',
    'progress',
    'task_count',
  ];
  selectedGoal!: GoalDto | null;

  constructor(
    private router: Router,
    private goalService: GoalService,
    private toastService: ToastService
  ) {
    this.getAllGoals();
  }

  getAllGoals() {
    this.isLoading = true;
    this.goalService.getAllGoals().subscribe({
      next: (response) => {
        this.goalList = new MatTableDataSource(response);
        this.goalList.sort = this.sort;
        this.isLoading = false;
      },
      error: ({ error }) => {
        this.isLoading = false;
        this.toastService.showError(error.description);
      },
    });
  }

  onCreateClicked() {
    this.router.navigateByUrl('/ww/goal/create');
  }

  onRowClick(goal: GoalDto) {
    this.router.navigateByUrl(`/ww/goal/${goal.id}`);
  }
}
