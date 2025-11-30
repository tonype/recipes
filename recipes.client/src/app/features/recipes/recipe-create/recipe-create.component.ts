import { Component, ChangeDetectionStrategy, signal, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NgIcon } from '@ng-icons/core';
import { RecipeService } from '../../../core/services/recipe.service';
import { CreateRecipeRequest } from '../../../shared/models/recipe.model';
import { TagInputComponent } from '../../../shared/components/tag-input/tag-input.component';
import { TagSelection } from '../../../shared/models/tag.model';

@Component({
  selector: 'app-recipe-create',
  templateUrl: './recipe-create.component.html',
  imports: [ReactiveFormsModule, NgIcon, TagInputComponent],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RecipeCreateComponent {
  private readonly fb = inject(FormBuilder);
  private readonly recipeService = inject(RecipeService);
  private readonly router = inject(Router);

  // Signals for state management
  isSubmitting = signal(false);
  errorMessage = signal<string | null>(null);

  // Reactive form
  recipeForm: FormGroup;

  constructor() {
    this.recipeForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.maxLength(200)]],
      prepTime: [0, [Validators.required, Validators.min(0)]],
      cookTime: [0, [Validators.required, Validators.min(0)]],
      difficulty: [3, [Validators.required, Validators.min(1), Validators.max(5)]],
      instructions: ['', [Validators.required]],
      notes: [''],
      tags: [[] as TagSelection[]]
    });
  }

  onSubmit(): void {
    if (this.recipeForm.invalid) {
      this.recipeForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    const formValue = this.recipeForm.value;
    const request: CreateRecipeRequest = {
      name: formValue.name,
      description: formValue.description,
      prepTime: formValue.prepTime,
      cookTime: formValue.cookTime,
      difficulty: formValue.difficulty,
      instructions: formValue.instructions,
      notes: formValue.notes,
      tags: formValue.tags?.map((tag: TagSelection) => ({
        tagId: tag.isNew ? undefined : tag.id,
        tagName: tag.isNew ? tag.name : undefined
      })) || []
    };

    this.recipeService.createRecipe(request).subscribe({
      next: () => {
        this.router.navigate(['/recipes']);
      },
      error: (error) => {
        this.errorMessage.set(error.message);
        this.isSubmitting.set(false);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/recipes']);
  }

  hasError(fieldName: string): boolean {
    const field = this.recipeForm.get(fieldName);
    return !!(field?.invalid && field?.touched);
  }

  getErrorMessage(fieldName: string): string {
    const field = this.recipeForm.get(fieldName);
    if (!field?.errors || !field?.touched) return '';

    if (field.errors['required']) return `${this.getFieldLabel(fieldName)} is required`;
    if (field.errors['maxlength']) {
      return `${this.getFieldLabel(fieldName)} cannot exceed ${field.errors['maxlength'].requiredLength} characters`;
    }
    if (field.errors['min']) return `${this.getFieldLabel(fieldName)} must be 0 or greater`;

    return 'Invalid value';
  }

  private getFieldLabel(fieldName: string): string {
    const labels: Record<string, string> = {
      name: 'Name',
      description: 'Description',
      prepTime: 'Prep time',
      cookTime: 'Cook time',
      difficulty: 'Difficulty',
      instructions: 'Instructions',
      notes: 'Notes',
      tags: 'Tags'
    };
    return labels[fieldName] || fieldName;
  }
}
