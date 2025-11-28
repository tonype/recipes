export interface PagedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

export interface RecipeQueryParams {
  pageNumber: number;
  pageSize: number;
  sortBy?: string;
  sortOrder?: 'asc' | 'desc';
}

export type SortField = 'name' | 'createdAt' | 'prepTime' | 'cookTime' | 'difficulty';
