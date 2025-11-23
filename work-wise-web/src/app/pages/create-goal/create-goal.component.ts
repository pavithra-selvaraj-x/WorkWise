import { CommonModule } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatStepper, MatStepperModule } from '@angular/material/stepper';
import { Router } from '@angular/router';
import {
  ConfirmationDialogComponent,
  ConfirmationDialogData,
} from '../../components/confirmation-dialog/confirmation-dialog.component';
import { PageLoaderComponent } from '../../components/page-loader/page-loader.component';
import { PageTitleBarComponent } from '../../components/page-title-bar/page-title-bar.component';
import { GoalService } from '../../services/goal-service/goal.service';
import { ToastService } from '../../services/toast-service/toast.service';
import { Status } from '../../types/enumerations';
import { Priority, TaskType } from './../../types/enumerations';
import {
  GoalDetailsDto,
  GoalDto,
  GoalSuggestionsDto,
  GoalTask,
  Task,
  TaskDto,
} from './../../types/goal';

@Component({
  selector: 'ww-create-goal',
  standalone: true,
  templateUrl: './create-goal.component.html',
  styleUrl: './create-goal.component.scss',
  imports: [
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatIconModule,
    MatRadioModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatStepperModule,
    CommonModule,
    PageTitleBarComponent,
    MatProgressSpinnerModule,
    PageLoaderComponent,
  ],
})
export class CreateGoalComponent {
  @ViewChild('stepper') stepper: MatStepper | undefined;
  goalForm!: FormGroup;
  taskForm!: FormGroup;
  priorities: string[] = Object.values(Priority);

  isGoalSuggestionLoading = false;
  goalSuggestions: GoalSuggestionsDto = { tasks: [] };

  isCreateGoalLoading = false;

  constructor(
    private fb: FormBuilder,
    private goalService: GoalService,
    private router: Router,
    private toastService: ToastService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.initializeForms();
  }

  initializeForms() {
    this.goalForm = this.fb.group({
      goal: ['', Validators.required],
      importance: ['', Validators.required],
      time_frame: ['', Validators.required],
      time_per_week: ['', Validators.required],
    });
    this.taskForm = this.fb.group({
      tasks: this.fb.array([]),
    });
  }

  // Function to mark all form controls in a form group as touched
  markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach((control) => {
      control.markAsTouched();

      if (control instanceof FormGroup) {
        // If the control is a nested form group, recursively call markFormGroupTouched
        this.markFormGroupTouched(control);
      }
    });
  }

  onGetGoalSuggestions() {
    if (this.goalForm.valid) {
      this.getGoalSuggestions();
    } else {
      this.markFormGroupTouched(this.goalForm);
    }
  }

  getGoalSuggestions() {
    this.isGoalSuggestionLoading = true;
    const goalFormData = this.goalForm.value;
    const goal: GoalDetailsDto = {
      goal: goalFormData.goal,
      importance: goalFormData.importance,
      time_frame: goalFormData.time_frame,
      time_per_week: goalFormData.time_per_week,
    };
    this.goalService.getGoalSuggestions(goal).subscribe({
      next: (response) => {
        this.goalSuggestions = response;
        this.goalSuggestions.tasks.forEach((task) => this.addTask(task));
        this.isGoalSuggestionLoading = false;
        this.stepper?.next();
      },
      error: ({ error }) => {
        this.isGoalSuggestionLoading = false;
        this.toastService.showError(error.description);
      },
    });
  }

  get tasks(): FormArray {
    return this.taskForm.get('tasks') as FormArray;
  }

  createTask(task?: Task): FormGroup {
    return this.fb.group({
      title: [task ? task.title : '', Validators.required],
      description: [task ? task.description : ''],
      startDate: [task ? task.startDate : ''],
      endDate: [task ? task.endDate : ''],
      priority: [task ? task.priority : Priority.Low, Validators.required],
    });
  }

  addTask(goalTask?: GoalTask): void {
    const task: Task = {
      title: goalTask?.title ?? '',
      description: goalTask?.description,
      priority: this.mapPriority(goalTask?.priority ?? ''),
      endDate: goalTask?.deadline ? new Date(goalTask?.deadline) : undefined,
    };
    this.tasks.push(this.createTask(task));
  }

  mapPriority(priority: string): Priority {
    switch (priority) {
      case 'Low':
        return Priority.Low;
      case 'Medium':
        return Priority.Medium;
      case 'High':
        return Priority.High;
      case 'Urgent':
        return Priority.Urgent;
      default:
        return Priority.Low;
    }
  }

  removeTask(index: number): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '30rem',
      data: {
        title: 'Confirm Delete?',
        message: 'Are you sure you want to remove the task from the goal?',
        cancelButtonText: 'Dismiss',
        okButtonText: 'Confirm',
      } as ConfirmationDialogData,
    });
    dialogRef.afterClosed().subscribe((result: boolean) => {
      if (result) {
        this.tasks.removeAt(index);
        this.toastService.showSuccess('Task Removed Successfully!');
      }
    });
  }

  onSave(): void {
    if (this.taskForm.valid) {
      this.stepper?.next();
    } else {
      this.toastService.showError(
        'There are vaidation errors. Please check the task data!'
      );
    }
  }

  createGoal() {
    this.isCreateGoalLoading = true;
    const goalDto: GoalDto = {
      title: this.goalForm.value.goal,
      description: this.goalForm.value.importance,
      start_date: new Date().toISOString(),
      end_date: this.calculateEndDate(),
      status: Status.Open,
      priority: Priority.Low,
      progress: 0,
      task_list: this.taskForm.value.tasks.map((task: any) => {
        return {
          title: task.title,
          description: task.description,
          start_date: task.startDate,
          end_date: task.endDate,
          status: Status.Open,
          priority: this.mapPriority(task.priority),
          task_type: TaskType.GoalRelated,
        } as TaskDto;
      }),
    };
    this.goalService.createGoal(goalDto).subscribe({
      next: (response) => {
        this.isCreateGoalLoading = false;
        this.toastService.showSuccess('Goal Created Successfully!');
        this.router.navigateByUrl('/ww/goal');
      },
      error: ({ error }) => {
        this.isCreateGoalLoading = false;
        this.toastService.showError(error.description);
      },
    });
  }

  calculateEndDate() {
    const timeFrame = parseInt(this.goalForm.value.time_frame);
    const startDate = new Date();
    const endDate = new Date(startDate);
    endDate.setMonth(startDate.getMonth() + timeFrame);
    return endDate.toISOString();
  }
}
