import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth-service/auth.service';
import { LoginDto } from '../../types/user';
import { ToastService } from './../../services/toast-service/toast.service';
import { TokenDto } from './../../types/user';

@Component({
  selector: 'ww-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    CommonModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  loginForm!: FormGroup;
  isLoading: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
    });
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }

  async onSubmit(): Promise<void> {
    if (this.loginForm.valid) {
      this.isLoading = true;
      const loginDto: LoginDto = {
        user_name: this.email?.value,
        password: this.password?.value,
      };
      this.authService.login(loginDto).subscribe({
        next: (response) => {
          const tokenDto: TokenDto = response;
          localStorage.setItem('token', tokenDto.access_token);
          this.authService.decodeTokenDetails();
          this.toastService.showSuccess('Login Successful!');
          this.isLoading = false;
          this.router.navigateByUrl('/ww');
        },
        error: ({ error }) => {
          this.isLoading = false;
          this.toastService.showError(error.description);
        },
      });
    }
  }
}
