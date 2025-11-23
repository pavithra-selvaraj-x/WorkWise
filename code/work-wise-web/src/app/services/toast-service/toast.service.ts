import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  constructor(private toastr: ToastrService) {}

  showSuccess(message: string, title?: string) {
    this.toastr.success(message, title ?? 'Success', {
      progressBar: true,
      timeOut: 3000,
      extendedTimeOut: 2000,
    });
  }

  showError(message: string, title?: string) {
    this.toastr.error(message, title ?? 'Error', {
      progressBar: true,
      timeOut: 3000,
      extendedTimeOut: 2000,
    });
  }
}
