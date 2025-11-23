import { CommonModule } from '@angular/common';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { PageLoaderComponent } from '../../components/page-loader/page-loader.component';
import { PageTitleBarComponent } from '../../components/page-title-bar/page-title-bar.component';
import { TaskService } from '../../services/task-service/task.service';
import { ToastService } from '../../services/toast-service/toast.service';
import { Priority, Status, TaskType } from '../../types/enumerations';
import { GoalDto, TaskDto } from '../../types/goal';
import { CreateTaskComponent } from '../create-task/create-task.component';
import { GoalService } from './../../services/goal-service/goal.service';

@Component({
  selector: 'ww-goal-details',
  standalone: true,
  templateUrl: './goal-details.component.html',
  styleUrl: './goal-details.component.scss',
  imports: [
    PageLoaderComponent,
    CommonModule,
    PageTitleBarComponent,
    MatTableModule,
    MatSortModule,
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatRadioModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatDialogModule,
    CreateTaskComponent,
  ],
})
export class GoalDetailsComponent {
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('deleteConfirmationDialog', { static: true })
  deleteConfirmationDialog!: TemplateRef<any>;
  isLoading = false;
  goalId = '';
  goal!: GoalDto;
  isEdit = false;
  goalForm!: FormGroup;
  tasks!: MatTableDataSource<TaskDto>;
  displayedColumns: string[] = [
    'title',
    'description',
    'start_date',
    'end_date',
    'status',
    'priority',
  ];
  selectedTask!: TaskDto | null;
  Priority = Priority;
  Status = Status;
  TaskType = TaskType;
  isDeleteLoading = false;
  showUpdateTask = false;
  isGoalSaveLoading = false;
  isDeleteGoalLoading = false;

  constructor(
    private goalService: GoalService,
    private route: ActivatedRoute,
    private toastService: ToastService,
    private router: Router,
    private fb: FormBuilder,
    private taskService: TaskService,
    private dialog: MatDialog
  ) {
    this.getGoalDetails();
  }

  getGoalDetails() {
    this.goalId = this.route.snapshot.paramMap.get('goalId') ?? '';
    if (this.goalId) {
      this.isLoading = true;
      this.goalService.getGoalById(this.goalId).subscribe({
        next: (response) => {
          this.goal = response as GoalDto;
          this.initializeGoalForm();
          this.tasks = new MatTableDataSource(this.goal.task_list);
          this.tasks.sort = this.sort;
          this.isLoading = false;
        },
        error: ({ error }) => {
          this.isLoading = false;
          this.toastService.showError(error.description);
          this.router.navigateByUrl('/ww/goal');
        },
      });
    }
  }

  initializeGoalForm() {
    this.goalForm = this.fb.group({
      title: [this.goal.title, Validators.required],
      description: [this.goal.description, Validators.required],
      start_date: [this.goal.start_date],
      end_date: [this.goal.end_date],
      status: [this.goal.status, Validators.required],
      priority: [this.goal.priority, Validators.required],
    });
    this.goalForm.disable();
  }

  editGoal() {
    if (this.isEdit) {
      this.initializeGoalForm();
      this.displayedColumns.pop();
    } else {
      this.goalForm.enable();
      this.displayedColumns.push('actions');
    }
    this.isEdit = !this.isEdit;
  }

  editTask(task: TaskDto) {
    this.selectedTask = task;
    this.showUpdateTask = true;
  }

  createNewTask() {
    this.selectedTask = null;
    this.showUpdateTask = true;
  }

  closeDialog(updated: boolean) {
    if (updated) {
      this.getGoalDetails();
    }
    this.showUpdateTask = false;
    this.selectedTask = null;
  }

  deleteTask(task: TaskDto) {
    this.selectedTask = task;
    this.dialog.open(this.deleteConfirmationDialog);
  }

  deleteTaskApiCall() {
    this.isDeleteLoading = true;
    this.taskService.deleteTask(this.selectedTask?.id!).subscribe({
      next: (response) => {
        this.isDeleteLoading = false;
        this.dialog.closeAll();
        this.selectedTask = null;
        this.getGoalDetails();
      },
      error: ({ error }) => {
        this.isDeleteLoading = false;
        this.selectedTask = null;
        this.toastService.showSuccess('Task deleted Successfully!');
        this.toastService.showError(error.description);
      },
    });
  }

  updateGoalApiCall() {
    if (this.goalForm.valid) {
      this.isGoalSaveLoading = true;
      this.selectedTask = null;
      const goalDto: GoalDto = this.goalForm.value;
      console.log(goalDto);
      this.goalService.updateGoal(this.goalId, goalDto).subscribe({
        next: (response) => {
          this.isGoalSaveLoading = false;
          this.toastService.showSuccess('Goal Updated Successfully!');
          this.getGoalDetails();
        },
        error: ({ error }) => {
          this.isGoalSaveLoading = false;
          this.toastService.showError(error.description);
        },
      });
    } else {
      this.toastService.showError(
        'There are validation errors. Please check the goal details!'
      );
    }
  }

  deleteGoal() {
    this.selectedTask = null;
    this.dialog.open(this.deleteConfirmationDialog);
  }

  deleteGoalApiCall() {
    this.isDeleteGoalLoading = true;
    this.selectedTask = null;
    this.dialog.closeAll();
    this.goalService.deleteGoal(this.goalId).subscribe({
      next: (response) => {
        this.isDeleteGoalLoading = false;
        this.toastService.showSuccess('Goal Deleted Successfully!');
        this.router.navigateByUrl('/ww/goal');
      },
      error: ({ error }) => {
        this.isDeleteGoalLoading = false;
        this.toastService.showError(error.description);
      },
    });
  }
}
