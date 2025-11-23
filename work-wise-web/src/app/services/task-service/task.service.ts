import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { TaskDto } from '../../types/goal';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  constructor(private http: HttpClient) {}

  createTasks(taskDto: TaskDto[]): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/tasks`, taskDto);
  }

  getTasks(): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/tasks`);
  }

  deleteTask(taskId: string): Observable<any> {
    return this.http.delete<any>(`${environment.apiUrl}/task/${taskId}`);
  }

  editTask(taskDto: TaskDto): Observable<any> {
    return this.http.put<any>(
      `${environment.apiUrl}/task/${taskDto.id}`,
      taskDto
    );
  }
}
