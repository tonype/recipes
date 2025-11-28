import { Routes } from '@angular/router';
import { RecipeListComponent } from './features/recipes/recipe-list/recipe-list.component';

export const routes: Routes = [
  { path: '', redirectTo: '/recipes', pathMatch: 'full' },
  { path: 'recipes', component: RecipeListComponent }
];
