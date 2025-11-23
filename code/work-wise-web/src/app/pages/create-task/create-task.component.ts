import { animate, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { PageTitleBarComponent } from '../../components/page-title-bar/page-title-bar.component';
import { TaskService } from '../../services/task-service/task.service';
import { Priority, Status, TaskType } from '../../types/enumerations';
import { TaskDto } from '../../types/goal';
import { ToastService } from './../../services/toast-service/toast.service';

@Component({
  selector: 'ww-create-task',
  standalone: true,
  templateUrl: './create-task.component.html',
  styleUrl: './create-task.component.scss',
  imports: [
    PageTitleBarComponent,
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatRadioModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    CommonModule,
  ],
  animations: [
    trigger('slideInOut', [
      transition(':enter', [
        style({ transform: 'translateX(100%)' }),
        animate('500ms ease-out', style({ transform: 'translateX(0%)' })),
      ]),
      transition(':leave', [
        animate('500ms ease-out', style({ transform: 'translateX(100%)' })),
      ]),
    ]),
  ],
})
export class CreateTaskComponent {
  @Input() taskDetails!: TaskDto | null;
  @Input() taskType: TaskType = TaskType.Independent;
  @Input() goalId!: string;
  @Output() closeDialog = new EventEmitter();

  taskForm!: FormGroup;
  Priority = Priority;
  Status = Status;
  isLoading: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private taskService: TaskService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.taskForm = this.formBuilder.group({
      title: [
        this.taskDetails ? this.taskDetails.title : '',
        Validators.required,
      ],
      description: [this.taskDetails ? this.taskDetails.description : ''],
      start_date: [this.taskDetails ? this.taskDetails.start_date : ''],
      end_date: [this.taskDetails ? this.taskDetails.end_date : ''],
      status: [
        this.taskDetails ? this.taskDetails.status : 'Open',
        Validators.required,
      ],
      priority: [
        this.taskDetails ? this.taskDetails.priority : 'Low',
        Validators.required,
      ],
    });
  }

  close() {
    this.closeDialog.emit();
  }

  onSubmit() {
    if (this.taskForm.valid) {
      this.isLoading = true;
      const formData: TaskDto = this.taskForm.value;
      formData.task_type = this.taskType;
      formData.goal_id = this.goalId;
      if (this.taskDetails) {
        formData.id = this.taskDetails?.id;
        this.taskService.editTask(formData).subscribe({
          next: () => {
            this.isLoading = false;
            this.toastService.showSuccess('Task Updated Successfully!');
            this.closeDialog.emit(true);
          },
          error: ({ error }) => {
            this.isLoading = false;
            this.toastService.showError(error.description);
            this.closeDialog.emit(false);
          },
        });
      } else {
        this.taskService.createTasks([formData]).subscribe({
          next: (response) => {
            this.isLoading = false;
            this.toastService.showSuccess('Task Created Successfully!');
            this.closeDialog.emit(true);
          },
          error: ({ error }) => {
            this.isLoading = false;
            this.toastService.showError(error.description);
            this.closeDialog.emit(false);
          },
        });
      }
    }
  }
}
