import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { Recipe } from '../../shared/models/recipe.model';
import { PagedResponse, RecipeQueryParams } from '../../shared/models/pagination.model';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/api/recipes`;

  getRecipesPaged(params: RecipeQueryParams): Observable<PagedResponse<Recipe>> {
    let httpParams = new HttpParams()
      .set('pageNumber', params.pageNumber.toString())
      .set('pageSize', params.pageSize.toString());

    if (params.sortBy) {
      httpParams = httpParams.set('sortBy', params.sortBy);
    }
    if (params.sortOrder) {
      httpParams = httpParams.set('sortOrder', params.sortOrder);
    }

    return this.http.get<PagedResponse<Recipe>>(this.apiUrl, { params: httpParams }).pipe(
      catchError(error => {
        console.error('Error fetching recipes:', error);
        return throwError(() => new Error('Failed to load recipes. Please try again later.'));
      })
    );
  }

  getRecipes(): Observable<Recipe[]> {
    return this.http.get<Recipe[]>(this.apiUrl).pipe(
      catchError(error => {
        console.error('Error fetching recipes:', error);
        return throwError(() => new Error('Failed to load recipes. Please try again later.'));
      })
    );
  }
}
