import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { TagResponse } from '../../shared/models/tag.model';

@Injectable({
  providedIn: 'root'
})
export class TagService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/api/tags`;

  searchTags(query: string): Observable<TagResponse[]> {
    if (!query.trim()) {
      return of([]);
    }

    const params = new HttpParams().set('query', query.trim());
    return this.http.get<TagResponse[]>(`${this.apiUrl}/search`, { params }).pipe(
      catchError(error => {
        console.error('Error searching tags:', error);
        return of([]);
      })
    );
  }

  getAllTags(): Observable<TagResponse[]> {
    return this.http.get<TagResponse[]>(this.apiUrl).pipe(
      catchError(error => {
        console.error('Error fetching tags:', error);
        return throwError(() => new Error('Failed to load tags.'));
      })
    );
  }
}