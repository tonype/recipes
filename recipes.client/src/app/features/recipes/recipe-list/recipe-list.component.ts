import { Component, ChangeDetectionStrategy, signal, computed, inject, OnInit, effect, resource } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { RecipeService } from '../../../core/services/recipe.service';
import { getDifficultyLabel, formatTime } from '../../../shared/utils/recipe.utils';
import { SortField } from '../../../shared/models/pagination.model';

@Component({
  selector: 'app-recipe-list',
  templateUrl: './recipe-list.component.html',
  imports: [RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RecipeListComponent implements OnInit {
  private readonly recipeService = inject(RecipeService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  private readonly pageSize = 50;

  // Input signals (trigger data loading)
  currentPage = signal(1);
  sortBy = signal<SortField | undefined>(undefined);
  sortOrder = signal<'asc' | 'desc'>('asc');

  // Computed signal that combines all query parameters
  private readonly queryState = computed(() => ({
    pageNumber: this.currentPage(),
    pageSize: this.pageSize,
    sortBy: this.sortBy(),
    sortOrder: this.sortOrder()
  }));

  // Resource for loading recipes
  recipesResource = resource({
    loader: () => {
      const request = this.queryState();
      console.log('Resource loader called with:', request);
      return firstValueFrom(this.recipeService.getRecipesPaged(request));
    }
  });

  // Derived state from resource
  recipes = computed(() => this.recipesResource.value()?.items ?? []);
  totalCount = computed(() => this.recipesResource.value()?.totalCount ?? 0);
  totalPages = computed(() => this.recipesResource.value()?.totalPages ?? 0);
  isLoading = this.recipesResource.isLoading;
  error = computed(() => this.recipesResource.error()?.message ?? null);

  // Computed signals
  startIndex = computed(() => {
    return (this.currentPage() - 1) * this.pageSize + 1;
  });

  endIndex = computed(() => {
    const end = this.currentPage() * this.pageSize;
    return Math.min(end, this.totalCount());
  });

  canGoPrevious = computed(() => this.currentPage() > 1);
  canGoNext = computed(() => this.currentPage() < this.totalPages());

  constructor() {
    // Sync page number to URL query parameters
    effect(() => {
      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: {
          page: this.currentPage().toString()
        },
        queryParamsHandling: '',
        replaceUrl: true
      });
    });

    // Manually reload resource when query state changes
    effect(() => {
      // Track the query state
      this.queryState();
      // Reload the resource
      this.recipesResource.reload();
    });
  }

  ngOnInit(): void {
    // Read page number from URL (sort/filter state uses default values)
    const params = this.route.snapshot.queryParams;
    const page = parseInt(params['page']) || 1;
    this.currentPage.set(page);
  }

  nextPage(): void {
    if (this.canGoNext()) {
      this.currentPage.update(page => page + 1);
    }
  }

  previousPage(): void {
    if (this.canGoPrevious()) {
      this.currentPage.update(page => page - 1);
    }
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages()) {
      this.currentPage.set(page);
    }
  }

  sortByField(field: SortField): void {
    if (this.sortBy() === field) {
      // Toggle sort order if clicking same field
      this.sortOrder.update(order => order === 'asc' ? 'desc' : 'asc');
    } else {
      // New field, default to ascending
      this.sortBy.set(field);
      this.sortOrder.set('asc');
    }
    this.currentPage.set(1); // Reset to first page when sorting changes
  }

  getDifficultyLabel(difficulty: number): string {
    return getDifficultyLabel(difficulty);
  }

  formatTime(minutes: number): string {
    return formatTime(minutes);
  }
}
