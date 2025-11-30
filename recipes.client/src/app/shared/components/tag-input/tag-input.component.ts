import {
  Component,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  OnDestroy,
  ElementRef,
  ViewChild,
  forwardRef,
  signal,
  computed,
  inject
} from '@angular/core';
import {
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgIcon } from '@ng-icons/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, takeUntil } from 'rxjs/operators';
import { of } from 'rxjs';
import { TagService } from '../../../core/services/tag.service';
import { TagResponse, TagSelection } from '../../models/tag.model';

@Component({
  selector: 'app-tag-input',
  templateUrl: './tag-input.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, NgIcon, ReactiveFormsModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TagInputComponent),
      multi: true
    }
  ]
})
export class TagInputComponent implements ControlValueAccessor, OnDestroy {
  private readonly tagService = inject(TagService);
  private readonly cdr = inject(ChangeDetectorRef);

  @ViewChild('searchInput') searchInput!: ElementRef<HTMLInputElement>;

  // Signals for reactive state
  selectedTags = signal<TagSelection[]>([]);
  searchQuery = signal('');
  searchResults = signal<TagResponse[]>([]);
  isSearching = signal(false);
  showDropdown = signal(false);

  // Computed values
  filteredResults = computed(() => {
    const query = this.searchQuery();
    const selected = this.selectedTags();
    return this.searchResults().filter(tag =>
      !selected.some(selectedTag => selectedTag.name.toLowerCase() === tag.name.toLowerCase())
    );
  });

  canCreateNew = computed(() => {
    const query = this.searchQuery().trim();
    if (!query || query.length < 2) return false;

    const exists = this.searchResults().some(tag =>
      tag.name.toLowerCase() === query.toLowerCase()
    );
    const alreadySelected = this.selectedTags().some(tag =>
      tag.name.toLowerCase() === query.toLowerCase()
    );

    return !exists && !alreadySelected && this.isValidTagName(query);
  });

  private searchSubject = new Subject<string>();
  private destroy$ = new Subject<void>();

  // Form control integration
  private onChange = (value: TagSelection[]) => {};
  private onTouched = () => {};

  constructor() {
    // Debounced search setup
    this.searchSubject.pipe(
      debounceTime(250),
      distinctUntilChanged(),
      switchMap(query => {
        if (!query.trim()) {
          return of([]);
        }
        this.isSearching.set(true);
        return this.tagService.searchTags(query);
      }),
      takeUntil(this.destroy$)
    ).subscribe(results => {
      this.searchResults.set(results);
      this.isSearching.set(false);
      this.cdr.markForCheck();
    });
  }

  // Component methods
  onSearchInput(event: Event): void {
    const target = event.target as HTMLInputElement;
    const query = target.value;
    this.searchQuery.set(query);
    this.searchSubject.next(query);
    this.showDropdown.set(true);
  }

  onInputFocus(): void {
    this.showDropdown.set(true);
    this.onTouched();
  }

  onInputBlur(): void {
    // Delay hiding dropdown to allow clicks on dropdown items
    setTimeout(() => {
      this.showDropdown.set(false);
      this.cdr.markForCheck();
    }, 200);
  }

  onKeyDown(event: KeyboardEvent): void {
    if (event.key === 'Enter' && this.canCreateNew()) {
      event.preventDefault();
      this.createNewTag();
    } else if (event.key === 'Escape') {
      this.clearSearch();
      this.showDropdown.set(false);
    }
  }

  selectTag(tag: TagResponse): void {
    const selection: TagSelection = {
      id: tag.id,
      name: tag.name,
      isNew: false
    };

    this.addTagSelection(selection);
  }

  createNewTag(): void {
    const query = this.searchQuery().trim();
    if (!this.canCreateNew()) return;

    const selection: TagSelection = {
      name: query,
      isNew: true
    };

    this.addTagSelection(selection);
  }

  private addTagSelection(selection: TagSelection): void {
    const current = this.selectedTags();
    const updated = [...current, selection];
    this.selectedTags.set(updated);
    this.onChange(updated);
    this.clearSearch();
  }

  removeTag(tagToRemove: TagSelection): void {
    const updated = this.selectedTags().filter(tag => tag.name !== tagToRemove.name);
    this.selectedTags.set(updated);
    this.onChange(updated);
  }

  private clearSearch(): void {
    this.searchQuery.set('');
    this.searchResults.set([]);
    this.showDropdown.set(false);
    if (this.searchInput) {
      this.searchInput.nativeElement.value = '';
    }
  }

  private isValidTagName(name: string): boolean {
    // Check length (2-100 characters)
    if (name.length < 2 || name.length > 100) return false;

    // Check allowed characters: alphanumeric, spaces, hyphens, underscores
    const allowedPattern = /^[a-zA-Z0-9\s\-_]+$/;
    return allowedPattern.test(name);
  }

  // ControlValueAccessor implementation
  writeValue(value: TagSelection[]): void {
    this.selectedTags.set(value || []);
    this.cdr.markForCheck();
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    // Handle disabled state if needed
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}