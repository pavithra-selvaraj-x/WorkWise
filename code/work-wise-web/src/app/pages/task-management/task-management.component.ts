import { CommonModule } from '@angular/common';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { PageLoaderComponent } from '../../components/page-loader/page-loader.component';
import { PageTitleBarComponent } from '../../components/page-title-bar/page-title-bar.component';
import { ToastService } from '../../services/toast-service/toast.service';
import { TaskDto } from '../../types/goal';
import { CreateTaskComponent } from '../create-task/create-task.component';
import { TaskService } from './../../services/task-service/task.service';

@Component({
  selector: 'ww-task-management',
  standalone: true,
  templateUrl: './task-management.component.html',
  styleUrl: './task-management.component.scss',
  imports: [
    PageTitleBarComponent,
    CreateTaskComponent,
    CommonModule,
    MatIconModule,
    MatTableModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    PageLoaderComponent,
  ],
})
export class TaskManagementComponent {
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('deleteConfirmationDialog', { static: true })
  deleteConfirmationDialog!: TemplateRef<any>;

  showCreateTask: boolean = false;
  isLoading = false;
  isDeleteLoading = false;
  tasks!: MatTableDataSource<TaskDto[]>;
  displayedColumns: string[] = [
    'title',
    'description',
    'start_date',
    'end_date',
    'status',
    'priority',
    'actions',
  ];
  selectedTask!: TaskDto | null;

  constructor(
    private taskService: TaskService,
    private toastService: ToastService,
    private dialog: MatDialog
  ) {
    this.getTasks();
  }

  getTasks() {
    this.isLoading = true;
    this.taskService.getTasks().subscribe({
      next: (response) => {
        this.tasks = new MatTableDataSource(response);
        this.tasks.sort = this.sort;
        this.isLoading = false;
      },
      error: ({ error }) => {
        this.isLoading = false;
        this.toastService.showError(error.description);
      },
    });
  }

  createTask() {
    this.showCreateTask = true;
  }

  closeDialog(updated: boolean) {
    if (updated) {
      this.getTasks();
    }
    this.showCreateTask = false;
    this.selectedTask = null;
  }

  editTask(task: TaskDto) {
    this.selectedTask = task;
    this.showCreateTask = true;
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
        this.getTasks();
      },
      error: ({ error }) => {
        this.isDeleteLoading = false;
        this.selectedTask = null;
        this.toastService.showError(error.description);
      },
    });
  }
}
