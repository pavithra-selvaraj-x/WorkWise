import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  constructor(private http: HttpClient) {}

  getDashboardInsights(): Observable<any> {
    return this.http.get<any>(
      `${environment.apiUrl}/dashboard/goal-dashboard-insights`
    );
  }

  getDashboardInfo(): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/dashboard/dashboard-info`);
  }
}
