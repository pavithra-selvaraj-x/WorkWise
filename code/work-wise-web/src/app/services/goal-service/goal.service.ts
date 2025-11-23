import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { GoalDetailsDto, GoalDto } from '../../types/goal';

@Injectable({
  providedIn: 'root',
})
export class GoalService {
  constructor(private http: HttpClient) {}

  getGoalSuggestions(goalDetailsDto: GoalDetailsDto): Observable<any> {
    return this.http.post<any>(
      `${environment.apiUrl}/goal/get-suggestions`,
      goalDetailsDto
    );
  }

  getAllGoals(): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/goal`);
  }

  getGoalById(goalId: string): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/goal/${goalId}`);
  }

  createGoal(goalDto: GoalDto): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/goal`, goalDto);
  }

  updateGoal(goalId: string, goalDto: GoalDto): Observable<any> {
    return this.http.put<any>(`${environment.apiUrl}/goal/${goalId}`, goalDto);
  }

  deleteGoal(goalId: string): Observable<any> {
    return this.http.delete<any>(`${environment.apiUrl}/goal/${goalId}`);
  }
}
